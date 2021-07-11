using System;
using FXiaoKe.Api.Response;
using SystemHttpMethod = System.Net.Http.HttpMethod;

namespace FXiaoKe.Utilities {
	[AttributeUsage(AttributeTargets.Class)]
	public class RequestAttribute : Attribute {
		public RequestAttribute(string path, HttpMethod method = HttpMethod.Get) {
			Path = path;
			Method = method switch {
				HttpMethod.Get     => SystemHttpMethod.Get,
				HttpMethod.Post    => SystemHttpMethod.Post,
				HttpMethod.Put     => SystemHttpMethod.Put,
				HttpMethod.Patch   => SystemHttpMethod.Patch,
				HttpMethod.Delete  => SystemHttpMethod.Delete,
				HttpMethod.Head    => SystemHttpMethod.Head,
				HttpMethod.Trace   => SystemHttpMethod.Trace,
				HttpMethod.Options => SystemHttpMethod.Options,
				_                  => throw new ArgumentOutOfRangeException(nameof(method), method, null)
			};
		}

		public RequestAttribute(string path, HttpMethod method, Type responseType) : this(path, method) => ResponseType = responseType;

		public string Path { get; init; }

		public SystemHttpMethod Method { get; init; }

		public Type ResponseType { get; init; }
	}

	public enum HttpMethod {
		Get,
		Post,
		Put,
		Patch,
		Delete,
		Head,
		Trace,
		Options
	}
}