using System;

namespace FXiaoKe.Exceptions {
	public class ModelException : Exception {
		public ModelException(string message = null, Exception innerException = null) : base(message, innerException) { }
		public ModelException(Type modelType, string message = null, Exception innerException = null) : this(message, innerException) => ModelType = modelType;
		public Type ModelType { get; set; }
	}

	public class DuplicateKeyException : ModelException {
		public DuplicateKeyException(string message = null, Exception innerException = null) : base(message, innerException) { }
		public DuplicateKeyException(Type modelType, string message = null, Exception innerException = null) : base(modelType, message, innerException) { }
	}

	public class MissingKeyException : ModelException {
		public MissingKeyException(string message = null, Exception innerException = null) : base(message, innerException) { }
		public MissingKeyException(Type modelType, string message = null, Exception innerException = null) : base(modelType, message, innerException) { }
	}
}