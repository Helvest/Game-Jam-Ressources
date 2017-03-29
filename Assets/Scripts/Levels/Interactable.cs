using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
	public bool isActive = true;

	[SerializeField]
	protected InteractableEnum _type;

	public InteractableEnum type
	{
		get
		{
			return _type;
		}
	}

	public virtual void Interagir() { }
}