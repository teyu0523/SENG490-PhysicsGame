using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class KeyboardNavigator : MonoBehaviour {

	/// <summary>
	/// Needs to be your scene's event system. I usually put this on the event system object.
	/// </summary>
	public EventSystem system;

	/// <summary>
	/// This function causes the tab button to rotate through active objects.
	/// </summary>
	public void Update()
	{
		Selectable next = null;

		if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.DownArrow))
		{
			next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
		}
		else if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
		}
		else if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnLeft();
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnRight();
		}


		if (next!= null) {
			
			InputField inputfield = next.GetComponent<InputField>();
			if (inputfield !=null) inputfield.OnPointerClick(new PointerEventData(system));
			
			system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
		}
	}
}
