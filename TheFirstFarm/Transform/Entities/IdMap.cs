using System.ComponentModel.DataAnnotations;
using Shared.Exceptions;

namespace TheFirstFarm.Transform.Entities {
	public abstract class IdMap : IdMap<string, int?> {
		protected IdMap() { }

		protected IdMap(string fId, int? kId = null) : base(fId, kId) { }
	}

	public abstract class IdMap<TF, TK> : IIdMap {
		protected IdMap() { }

		protected IdMap(TF fXiaoKeId, TK kingdeeId = default) {
			FXiaoKeId = fXiaoKeId;
			KingdeeId = kingdeeId;
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

		[Key]
		public TF FXiaoKeId { get; set; }

		public TK KingdeeId { get; set; }
	}

	public interface IIdMap {
		public object FXiaoKeId { get; set; }

		public object KingdeeId { get; set; }
	}
}