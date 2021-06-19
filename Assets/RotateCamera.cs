using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{


	private bool startRotating = false;
	private Vector3 start;
	private Vector3 end;
	public float speed = 3f;
	public float amountOfTime = 5f;

	private void Start()
	{
		startRotating = true;
		start = new Vector3(0, 25, 144);
		transform.position = start;

		end = new Vector3(55, 25, 120);
	}


	private void Update()
	{

		transform.position = Vector3.MoveTowards(transform.position, end, amountOfTime * Time.deltaTime);
	}

}
