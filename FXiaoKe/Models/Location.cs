using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Shared.Exceptions;
using Shared.Serialization;

namespace FXiaoKe.Models {
	[JsonConverter(typeof(CastConverter<Location, string>))]
	public class Location {
		private static readonly Regex Pattern = new(@"^(?<longitude>\d{0,3}(?:\.\d+)?)#%\$(?<latitude>\d{0,3}(?:\.\d+)?)#%\$(?<name>.+)$", RegexOptions.Compiled);

		public Location() { }

		public Location(string location) {
			var match = Pattern.Match(location);
			if (!match.Success)
				throw new RegexNotMatchException(location, Pattern);
			Longitude = Convert.ToDecimal(match.Groups["longitude"].Value);
			Latitude = Convert.ToDecimal(match.Groups["latitude"].Value);
			Name = match.Groups["name"].Value;
		}

		public decimal Longitude { get; set; }

		public decimal Latitude { get; set; }

		public string Name { get; set; }

		public override string ToString() => $"{Longitude:#.000000}#%${Latitude:#.000000}#%${Name}";

		public static explicit operator string(Location info) => info.ToString();

		public static explicit operator Location(string location) => new(location);
	}
}