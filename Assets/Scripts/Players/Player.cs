using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Controllable
{

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public virtual void UseActionA_Press() { }

	public virtual void UseActionA_Release() { }

	public virtual void UseActionB_Press() { }

	public virtual void UseActionB_Release() { }

	public virtual void UseActionX_Press() { }

	public virtual void UseActionX_Release() { }

	public virtual void UseActionY_Press() { }

	public virtual void UseActionY_Release() { }

	public virtual void Up() { }

	public virtual void UpDouble() { }

	public virtual void Down() { }

	public virtual void DownDouble() { }

	public virtual void Right() { }

	public virtual void RightDouble() { }

	public virtual void Left() { }

	public virtual void LeftDouble() { }
}
