using System;

namespace FXiaoKe.Exceptions {
	public class ModelException : Exception {
		public ModelException(string message = null, Exception innerException = null) : base(message, innerException) { }
		public ModelException(Type modelType, string message = null, Exception innerException = null) : this(message, innerException) => ModelType = modelType;
		public Type ModelType { get; set; }
	}

	public class DuplicatePrimaryKeyException : ModelException {
		public DuplicatePrimaryKeyException(string message = null, Exception innerException = null) : base(message, innerException) { }
		public DuplicatePrimaryKeyException(Type modelType, string message = null, Exception innerException = null) : base(modelType, message, innerException) { }
	}

	public class MissingPrimaryKeyException : ModelException {
		public MissingPrimaryKeyException(string message = null, Exception innerException = null) : base(message, innerException) { }
		public MissingPrimaryKeyException(Type modelType, string message = null, Exception innerException = null) : base(modelType, message, innerException) { }
	}
}