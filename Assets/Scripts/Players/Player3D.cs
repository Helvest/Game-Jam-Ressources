using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player3D : Controllable
{

	private Rigidbody _rigidbody;

	[SerializeField]
	private float speed = 10;

	[SerializeField]
	private float torqueForce = 30;

	[SerializeField]
	private float jumpForce = 10;

	protected override void Start()
	{
		base.Start();

		_rigidbody = GetComponent<Rigidbody>();
	}

	void Update()
	{
		if(!isControlled)
		{
			return;
		}

		if(Input.GetAxis("Vertical_" + playerID) != 0)
		{
			Vector3 direction = Input.GetAxis("Horizontal_" + playerID) * _transform.right;
			direction += Input.GetAxis("Vertical_" + playerID) * _transform.forward;
			direction = direction.normalized * speed;
			direction.y = _rigidbody.velocity.y;
			_rigidbody.velocity = direction;
		}

		if(Input.GetAxis("Horizontal_" + playerID) != 0)
		{
			_rigidbody.angularVelocity = Input.GetAxis("Horizontal_" + playerID) * _transform.up * torqueForce;
		}
	}

	public override void UseActionA_Press()
	{

	}

	public override void UseActionA_Release() { }

	public override void UseActionB_Press()
	{

	}

	public override void UseActionB_Release() { }

	public override void UseActionX_Press()
	{
		Debug.Log("UseActionX_Press");
	}

	public override void UseActionX_Release() { }

	public override void UseActionY_Press()
	{
		_rigidbody.AddForce(transform.up * jumpForce);
	}

	public override void UseActionY_Release() { }

	public override void Up()
	{
		Debug.Log("Up");
	}

	public override void UpDouble() { }

	public override void Down()
	{
		Debug.Log("Down");
	}

	public override void DownDouble() { }

	public override void Right()
	{
		Debug.Log("Right");
	}

	public override void RightDouble() { }

	public override void Left()
	{
		Debug.Log("Left");
	}

	public override void LeftDouble() { }
}
