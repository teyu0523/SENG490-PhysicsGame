using UnityEngine;
using System.Collections;

public class PinchGesture : MonoBehaviour {

	public delegate void PinchGestureDelegate(float speed);
	public event PinchGestureDelegate OnPinchGestureStep;

	public float m_min_pinch_speed = 5.0f;
	public float m_variance_in_distance = 5.0f;

	// Update is called once per frame
	void Update () {
		if (Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved)
		{
			Vector2 curDist = Input.GetTouch(0).position - Input.GetTouch(1).position; //current distance between finger touches
			Vector2 prevDist = ((Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition) - (Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition)); //difference in previous locations using delta positions
			float touchDelta = curDist.magnitude - prevDist.magnitude;
			float speedTouch0 = Input.GetTouch(0).deltaPosition.magnitude / Input.GetTouch(0).deltaTime;
			float speedTouch1 = Input.GetTouch(1).deltaPosition.magnitude / Input.GetTouch(1).deltaTime;
			
			if ((touchDelta + m_variance_in_distance <= 1) && (speedTouch0 > m_min_pinch_speed) && (speedTouch1 > m_min_pinch_speed))
			{
				if(OnPinchGestureStep != null)
				{
					OnPinchGestureStep(touchDelta/m_min_pinch_speed);
				}
			}
			
			if ((touchDelta + m_variance_in_distance > 1) && (speedTouch0 > m_min_pinch_speed) && (speedTouch1 > m_min_pinch_speed))
			{
				if(OnPinchGestureStep != null)
				{
					OnPinchGestureStep(touchDelta/m_min_pinch_speed);
				}
			}
		}      
	}
}
