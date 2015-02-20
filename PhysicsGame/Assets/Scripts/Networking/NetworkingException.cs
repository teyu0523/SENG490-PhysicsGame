using UnityEngine;
using System.Collections;
using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;

[Serializable]
public class NetworkingException : System.Exception, ISerializable {


	public NetworkingException() {
	}
	public NetworkingException(string message) : base(message) {
	}
	public NetworkingException(string message, Exception innerException) : base(message, innerException) {
	}
	protected NetworkingException(SerializationInfo info, StreamingContext context): base(info, context) {
	}
}

[Serializable]
public class AuthenticationException : NetworkingException, ISerializable {

	public AuthenticationException() {
	}
	public AuthenticationException(string message) : base(message) {
	}
	public AuthenticationException(string message, Exception innerException) : base(message, innerException) {
	}
	protected AuthenticationException(SerializationInfo info, StreamingContext context): base(info, context) {
	}
}
