using System.ComponentModel.DataAnnotations;
using Shared.Exceptions;

namespace TheFirstFarm.Transform.Entities {
	#nullable enable
	public abstract class KIdMap : IdMap<string?, int> {
		protected KIdMap() { }

		protected KIdMap(int kId, string? fId = null) : base(fId, kId) { }

		[Key]
		public override int KingdeeId {
			get => base.KingdeeId;
			set => base.KingdeeId = value;
		}
	}
	#nullable disable

	public abstract class FIdMap : IdMap<string, int?> {
		protected FIdMap() { }

		protected FIdMap(string fId, int? kId = null) : base(fId, kId) { }

		[Key]
		public override string FXiaoKeId {
			get => base.FXiaoKeId;
			set => base.FXiaoKeId = value;
		}
	}

	public abstract class IdMap<TF, TK> : IIdMap {
		private TF _fXiaoKeId;

		private TK _kingdeeId;

		protected IdMap() { }

		protected IdMap(TF fXiaoKeId, TK kingdeeId = default) {
			_fXiaoKeId = fXiaoKeId;
			_kingdeeId = kingdeeId;
		}

		object IIdMap.FXiaoKeId {
			get => FXiaoKeId;
			set {
				if (value is TF or null)
					FXiaoKeId = (dynamic)value;
				else
					throw new InvariantTypeException(typeof(TF), value.GetType());
			}
		}

		object IIdMap.KingdeeId {
			get => KingdeeId;
			set {
				if (value is TK or null)
					KingdeeId = (dynamic)value;
				else
					throw new InvariantTypeException(typeof(TK), value.GetType());
			}
		}

		public virtual TF FXiaoKeId {
			get => _fXiaoKeId;
			set => _fXiaoKeId = value;
		}

		public virtual TK KingdeeId {
			get => _kingdeeId;
			set => _kingdeeId = value;
		}
	}

	public interface IIdMap {
		public object FXiaoKeId { get; set; }

		public object KingdeeId { get; set; }
	}
}