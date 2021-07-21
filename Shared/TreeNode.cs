using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Shared {
	public class TreeNode : TreeNodeBase<TreeNode> {
		public TreeNode() { }
		public TreeNode(TreeNode parent) : base(parent) { }
	}

	/// <summary>
	///     Base class of tree node
	/// </summary>
	/// <typeparam name="TNode">The actual type of node. Used to define relationship reference</typeparam>
	public class TreeNodeBase<TNode> : IEnumerable<TNode> where TNode : TreeNodeBase<TNode> {
		#region Implementations
		public IEnumerator<TNode> GetEnumerator() {
			yield return This;
			if (!IsLeaf)
				foreach (var node in Children.SelectMany(child => child))
					yield return node;
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		#endregion

		#region Fields
		private TNode _parent;
		private int _height;
		private int _size = 1;
		private int _depth;
		private bool _heightUpToDate = true;
		private bool _depthUpToDate = true;
		#endregion

		#region Constructors
		public TreeNodeBase() => Children = new List<TNode>();
		public TreeNodeBase(TNode parent) : this() => Parent = parent;
		#endregion

		#region Delegates
		public delegate void ValueChanging<TValue>(object sender, ValueChangingEventArg<TValue> e);

		public delegate void ValueChanged<TValue>(object sender, ValueChangedEventArg<TValue> e);
		#endregion

		#region Events
		/// <summary>
		///     Called before parent is changed. Everything remains unchanged.
		/// </summary>
		public event ValueChanging<TNode> ParentChanging = delegate { };

		/// <summary>
		///     Called after parent is changed. All related data are already updated.
		/// </summary>
		public event ValueChanged<TNode> ParentChanged = delegate { };
		#endregion

		#region Properties
		/// <summary>
		///     Parent of the node
		/// </summary>
		public virtual TNode Parent {
			get => _parent;
			set {
				if (_parent != null && !_parent.Equals(value) || _parent == null && value != null) {
					OnParentChanging(new ValueChangingEventArg<TNode>(_parent, value));
					_parent = value;
					OnParentChanged(new ValueChangedEventArg<TNode>(_parent));
				}
			}
		}

		/// <summary>
		///     Indicate whether this node is the root of the tree
		/// </summary>
		public bool IsRoot => _parent == null;

		/// <summary>
		///     Indicate whether this node is a leaf node
		/// </summary>
		public bool IsLeaf => Children.Count == 0;

		/// <summary>
		///     The root of the tree
		/// </summary>
		public TNode Root {
			get {
				var root = This;
				while (root is {IsRoot: false})
					root = root.Parent;
				return root;
			}
		}

		/// <summary>
		///     Children of the node
		/// </summary>
		public List<TNode> Children { get; }

		/// <summary>
		///     Height of the subtree whose root is this node
		/// </summary>
		public int Height {
			get {
				UpdateHeightAndSize();
				return _height;
			}
		}

		/// <summary>
		///     Size of the subtree whose root is this node
		/// </summary>
		public int Size {
			get {
				UpdateHeightAndSize();
				return _size;
			}
		}

		/// <summary>
		///     Depth of the subtree whose root is this node
		/// </summary>
		public int Depth {
			get {
				UpdateDepth();
				return _depth;
			}
		}

		protected TNode This => this as TNode;

		private bool HeightUpToDate {
			get => _heightUpToDate;
			set {
				if (_heightUpToDate && !value && !IsRoot)
					_parent.HeightUpToDate = false;
				_heightUpToDate = value;
			}
		}

		private bool DepthUpToDate {
			get => _depthUpToDate;
			set {
				if (_depthUpToDate && !value)
					foreach (var child in Children)
						child.DepthUpToDate = false;
				_depthUpToDate = value;
			}
		}
		#endregion

		#region Methods
		public static TNode BuildTree(IList<TNode> nodes, string keyName, string parentKeyName = null) => BuildForest(nodes, keyName, parentKeyName).Single();

		public static TNode BuildTree<TKey>(IList<TNode> nodes, Func<TNode, TKey> keySelector, Func<TNode, TKey> parentKeySelector = null) => BuildForest(nodes, keySelector, parentKeySelector).Single();

		public static List<TNode> BuildForest(IList<TNode> nodes, string keyName, string parentKeyName = null) {
			var key = typeof(TNode).GetMember(keyName).Single();
			var parentKey = parentKeyName is null ? null : typeof(TNode).GetMember(parentKeyName).Single();
			return BuildForest(nodes, node => key.GetValue(node), parentKey is null ? null : node => parentKey.GetValue(node));
		}

		public static List<TNode> BuildForest<TKey>(IList<TNode> nodes, Func<TNode, TKey> keySelector, Func<TNode, TKey> parentKeySelector = null) {
			parentKeySelector ??= node => keySelector(node.Parent);
			var dict = new Dictionary<TKey, int>();
			var index = new Dictionary<TNode, int>();
			bool[] hasParent = new bool[nodes.Count];
			for (int i = 0; i < nodes.Count; ++i) {
				dict[keySelector(nodes[i])] = i;
				index[nodes[i]] = i;
			}
			for (int i = 0; i < nodes.Count; ++i) {
				var node = nodes[i];
				var parentKey = parentKeySelector(node);
				hasParent[i] = dict.ContainsKey(parentKey);
				if (hasParent[i])
					node.Parent = nodes[dict[parentKey]];
			}
			//Check cycle
			int[] tags = new int[nodes.Count];
			for (int i = 0, tag = 0; i < nodes.Count; ++i) {
				if (tags[i] > 0)
					continue;
				++tag;
				int j = i;
				while (true) {
					if (tags[j] == 0)
						tags[j] = tag;
					else if (tags[j] == tag)
						throw new InvalidOperationException("Cycle detected");
					else
						break;
					if (!hasParent[j])
						break;
					j = index[nodes[j].Parent];
				}
			}
			return nodes.Where((_, i) => !hasParent[i]).ToList();
		}

		/// <summary>
		///     Calculate the latest common ancestor of two nodes
		/// </summary>
		/// <returns>Null if two nodes aren't in the same tree</returns>
		public static TNode GetLatestCommonAncestor(TNode node1, TNode node2) {
			if (node1.Depth > node2.Depth) {
				var temp = node1;
				node1 = node2;
				node2 = temp;
			}
			for (int i = 0; i < node2.Depth - node1.Depth; ++i)
				node2 = node2.Parent;
			while (!node1.Equals(node2) && !node1.IsRoot && !node2.IsRoot) {
				node1 = node1.Parent;
				node2 = node2.Parent;
			}
			return node1.Equals(node2) ? node1 : null;
		}

		public TTargetNode Cast<TTargetNode>(Func<TNode, TTargetNode> selector) where TTargetNode : TreeNodeBase<TTargetNode> {
			var srcQueue = new Queue<TNode>();
			var dstQueue = new Queue<TTargetNode>();
			var dstRoot = selector(This);
			srcQueue.Enqueue(This);
			dstQueue.Enqueue(dstRoot);
			while (srcQueue.Count > 0) {
				var srcParent = srcQueue.Dequeue();
				var dstParent = dstQueue.Dequeue();
				foreach (var srcChild in srcParent.Children) {
					var dstChild = selector(srcChild);
					dstChild.Parent = dstParent;
					srcQueue.Enqueue(srcChild);
					dstQueue.Enqueue(dstChild);
				}
			}
			return dstRoot;
		}

		public async Task<TTargetNode> CastAsync<TTargetNode>(Func<TNode, Task<TTargetNode>> selector) where TTargetNode : TreeNodeBase<TTargetNode> {
			var srcQueue = new Queue<TNode>();
			var dstQueue = new Queue<TTargetNode>();
			var dstRoot = await selector(This);
			srcQueue.Enqueue(This);
			dstQueue.Enqueue(dstRoot);
			while (srcQueue.Count > 0) {
				var srcParent = srcQueue.Dequeue();
				var dstParent = dstQueue.Dequeue();
				foreach (var srcChild in srcParent.Children) {
					var dstChild = await selector(srcChild);
					dstChild.Parent = dstParent;
					srcQueue.Enqueue(srcChild);
					dstQueue.Enqueue(dstChild);
				}
			}
			return dstRoot;
		}

		public TNode Search(Predicate<TNode> predicate) {
			var queue = new Queue<TNode>();
			queue.Enqueue(This);
			while (queue.Count > 0) {
				var top = queue.Dequeue();
				if (predicate(top))
					return top;
				foreach (var child in top.Children)
					queue.Enqueue(child);
			}
			return null;
		}

		public IEnumerable<TNode> SearchAll(Predicate<TNode> predicate) {
			var queue = new Queue<TNode>();
			queue.Enqueue(This);
			while (queue.Count > 0) {
				var top = queue.Dequeue();
				if (predicate(top))
					yield return top;
				foreach (var child in top.Children)
					queue.Enqueue(child);
			}
		}

		/// <summary>
		///     Called when parent is about to change. Will update the original parent and up-to-date flags after the event fires.
		/// </summary>
		/// <param name="e"></param>
		public virtual void OnParentChanging(ValueChangingEventArg<TNode> e) {
			ParentChanging(this, e);
			if (_parent != null) {
				_parent.Children.Remove(This);
				_parent.HeightUpToDate = false;
			}
			if (_parent?.Depth != e.NewValue?.Depth)
				DepthUpToDate = false;
		}

		/// <summary>
		///     Called when parent has changed. Will update the new parent and up-to-date flags before the event fires.
		/// </summary>
		/// <param name="e"></param>
		public virtual void OnParentChanged(ValueChangedEventArg<TNode> e) {
			if (_parent != null) {
				_parent.Children.Add(This);
				_parent.HeightUpToDate = false;
			}
			ParentChanged(this, e);
		}

		/// <summary>
		///     Check whether this node is the child of another node
		/// </summary>
		public bool IsChildOf(TNode node) {
			var cur = Parent;
			while (cur != null && !cur.Equals(node) && !cur.Equals(this))
				cur = cur.Parent;
			return node.Equals(cur);
		}

		/// <summary>
		///     Check whether this node is the ancestor of another node
		/// </summary>
		public bool IsAncestorOf(TNode node) {
			if (node == null)
				return false;
			return node.IsChildOf(This);
		}

		/// <summary>
		///     Update height and size if out of date. Called in the getters of Height and Size.
		/// </summary>
		private void UpdateHeightAndSize() {
			if (HeightUpToDate)
				return;
			_height = 0;
			_size = 1;
			foreach (var child in Children) {
				child.UpdateHeightAndSize();
				_size += child._size;
				_height = Math.Max(_height, child._height + 1);
			}
			HeightUpToDate = true;
		}

		/// <summary>
		///     Update depth if out of date. Called in the getter of Depth
		/// </summary>
		private void UpdateDepth() {
			if (DepthUpToDate)
				return;
			if (IsRoot) {
				_depth = 0;
				return;
			}
			Parent.UpdateDepth();
			_depth = Parent.Depth + 1;
			DepthUpToDate = true;
		}
		#endregion

		#region Classes
		/// <summary>
		///     A class that preserves the arguments ValueChanging event
		/// </summary>
		/// <typeparam name="TValue">Type of the changing value</typeparam>
		public class ValueChangingEventArg<TValue> : EventArgs {
			public TValue NewValue;
			public TValue OldValue;

			public ValueChangingEventArg(TValue old, TValue @new) {
				OldValue = old;
				NewValue = @new;
			}
		}

		/// <summary>
		///     A class that preserves the arguments ValueChanged event
		/// </summary>
		/// <typeparam name="TValue"></typeparam>
		public class ValueChangedEventArg<TValue> : EventArgs {
			public TValue NewValue;
			public ValueChangedEventArg(TValue @new) => NewValue = @new;
		}
		#endregion
	}
}