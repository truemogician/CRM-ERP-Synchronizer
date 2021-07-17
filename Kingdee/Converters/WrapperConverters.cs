// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
using Shared.JsonConverters;

namespace Kingdee.Converters {
	public class FNumberConverter : ObjectWrapperConverterBase<string> {
		public override string PropertyName => "FNumber";
	}

	public class FNUMBERConverter : ObjectWrapperConverterBase<string> {
		public override string PropertyName => "FNUMBER";
	}
}