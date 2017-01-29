using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player3D : Controllable
{
	private Rigidbody _rigidbody;

	[SerializeField]
	private float rotSpeed = 4;

	[SerializeField]
	private LayerMask layerMaskGround;
	[SerializeField]
	private float lineCastGroundOrig = 0.05f;
	[SerializeField]
	private float lineCastGround = 0.1f;

	private bool canJump = false;
	private bool canDoubleJump = false;

	[SerializeField]
	private float speedNormale = 10;

	[SerializeField]
	private float jumpForce = 10;

	//private Animator animator;

	protected override void Awake()
	{
		base.Awake();
		//animator = GetComponent<Animator>();
		_rigidbody = GetComponent<Rigidbody>();
	}

	protected override void Start()
	{
		ControlManager.AddControllableCharacters(this);
	}

	void Update()
	{
		if(!isControlled || PauseManager.IsPause)
		{
			return;
		}

		//Game over
		if(_transform.position.y <= -10)
		{
			//LevelManager.Respawn();
		}

		Vector3 direction = Input.GetAxis("Horizontal_" + playerID) * LevelManager.mainCamera.right;
		direction += Input.GetAxis("Vertical_" + playerID) * LevelManager.mainCamera.forward;

		direction = direction.normalized * speedNormale;
		direction.y = 0;

		Quaternion pastRot = _transform.rotation;

		if(direction.x != 0 || direction.z != 0)
		{
			_transform.LookAt(_transform.position + direction);
		}


		_transform.rotation = Quaternion.RotateTowards(pastRot, _transform.rotation, rotSpeed);

		direction.y = _rigidbody.velocity.y;
		_rigidbody.velocity = direction;

		//check ground
		canJump = Physics.Linecast(_transform.position + (Vector3.up * lineCastGroundOrig), _transform.position - (Vector3.up * (lineCastGround)), layerMaskGround);

/*
		if(canJump)
		{
			if(direction.x != 0 || direction.z != 0)
			{
				animator.SetInteger("State", 1);
			}
			else
			{
				animator.SetInteger("State", 0);
			}
		}
		else
		{
			animator.SetInteger("State", 2);
		}
*/
	}

	public override void UseActionA_Press()
	{
		if(canJump || canDoubleJump)
		{
			if(canDoubleJump)
			{
				canDoubleJump = false;
			}

			if(canJump)
			{
				canDoubleJump = true;
			}

			canJump = false;

			_rigidbody = GetComponent<Rigidbody>();

			Vector3 velocity = _rigidbody.velocity;
			velocity.y = 0;
			_rigidbody.velocity = velocity;
			_rigidbody.AddForce(_transform.up * jumpForce);
		}
	}

	public override void UseActionB_Press()
	{
	}

	public override void UseActionX_Press()
	{
	}

	public override void UseActionY_Press()
	{
	}

	public override void UseActionY_Release()
	{
	}


#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		Gizmos.color = Color.cyan;
		Gizmos.DrawLine(transform.position + (Vector3.up * lineCastGroundOrig), transform.position - (Vector3.up * lineCastGround));
	}
#endif

}
