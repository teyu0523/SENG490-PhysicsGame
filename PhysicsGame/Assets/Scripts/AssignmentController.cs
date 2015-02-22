using UnityEngine;
using System.Collections;
using SimpleJSON;

public class AssignmentController : MonoBehaviour {

	private bool m_running = false;
	private JSONNode m_result = null;

	public bool startAssignment(int lesson_id)
	{
		if(m_running) {
			return false;
		}
		m_running = true;

		return m_running;
	}

}
