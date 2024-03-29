﻿using System;
using FXiaoKe.Responses;
using Shared.Exceptions;
using Shared.Validation;
using SystemHttpMethod = System.Net.Http.HttpMethod;

namespace FXiaoKe.Requests {
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	[ValidationIgnore]
	public class RequestAttribute : Attribute {
		private readonly Type _responseType;

		public RequestAttribute(string path = null) => Path = path;

		public RequestAttribute(string path, HttpMethod method) : this(path) {
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

		public RequestAttribute(string path, Type responseType) : this(path) => ResponseType = responseType;

		public RequestAttribute(string path, HttpMethod method, Type responseType) : this(path, method) => ResponseType = responseType;

		public Uri Url { get; init; } = new(Client.Origin);

		public string Path {
			get => Url.AbsolutePath;
			init {
				if (value is null)
					value = string.Empty;
				else if (value.StartsWith('/'))
					value = value[1..];
				Url = new Uri($"{Url.Scheme}://{(Url.IsDefaultPort ? Url.Host : $"{Url.Host}:{Url.Port}")}/{value}{Url.Query}");
			}
		}

		public string Origin {
			get => $"{Url.Scheme}://{(Url.IsDefaultPort ? Url.Host : $"{Url.Host}:{Url.Port}")}";
			init {
				if (value.EndsWith('/'))
					value = value[..^1];
				Url = new Uri($"{value}/{Url.PathAndQuery}");
			}
		}

		public SystemHttpMethod Method { get; init; }

		public Type ResponseType {
			get => _responseType;
			init {
				if (!value.IsSubclassOf(typeof(ResponseBase)))
					throw new TypeException(value, $"{nameof(ResponseType)} must derive from {nameof(ResponseBase)}");
				_responseType = value;
			}
		}

		public string ErrorMessage { get; init; }
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