using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtThat : MonoBehaviour
{
	[SerializeField]
	private Transform target;

	void Awake()
	{
		if(!target)
		{
			Destroy(this);
		}
	}

	void Update()
	{
		transform.LookAt(target);
	}
}
