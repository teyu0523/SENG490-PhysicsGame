using UnityEngine;
using System.Collections;

public class WWWHelper {
	public enum HTTP_RESPONSE_CODE {
		OK = 200,
	}

	public HTTP_RESPONSE_CODE getResponseCode(WWW www) {
		if(!www.isDone) {
			throw new NetworkingException("Cannot get response code from incomplete www instance.");
		}
		return HTTP_RESPONSE_CODE.OK;
	}
}
