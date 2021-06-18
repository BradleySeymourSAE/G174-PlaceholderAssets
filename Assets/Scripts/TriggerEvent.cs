using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEvent : MonoBehaviour
{

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player") { Debug.Log("TriggerEvent - Triggering Environment Change!"); Showcase.ToggleEnvironmentChange?.Invoke(); }
	}


	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Player") { Debug.Log("TriggerEvent - Player left environment trigger zone!"); Showcase.ToggleEnvironmentChange?.Invoke(); }
	}
}
