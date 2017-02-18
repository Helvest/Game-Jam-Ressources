using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player3D : PlayerScript
{
	private Rigidbody _rigidbody;

	[SerializeField]
	private float speedMoveNormale = 9;
	[SerializeField]
	private float speedRotNormale = 400;

	[SerializeField]
	private float speedMoveInAir = 9;
	[SerializeField]
	private float speedRotInAir = 400;

	[SerializeField]
	private float jumpForce = 4000;

	[SerializeField]
	private LayerMask layerMaskGround;
	[SerializeField]
	private float sphereCastGroundOrig = 0f;
	[SerializeField]
	private float sphereCastGround = 0.15f;

	private bool canJump = false;
	private bool canDoubleJump = false;

	//private Animator animator;

	protected override void Awake()
	{
		base.Awake();
		//animator = GetComponent<Animator>();
		_rigidbody = GetComponent<Rigidbody>();
	}

	protected override void Start()
	{
		ControlManager.AddPlayerScriptCharacters(this);
	}

	private float speedMove;
	private float speedRot;

	void FixedUpdate()
	{
		if(!isControlled || PauseManager.IsPause)
		{
			return;
		}

		//Game over
		if(_rigidbody.position.y <= -10)
		{
			_rigidbody.velocity = Vector3.zero;
			LevelManager.Respawn();
			return;
		}

		//check ground
		canJump = Physics.CheckSphere(_rigidbody.position + (Vector3.up * sphereCastGroundOrig), sphereCastGround, layerMaskGround);

		if(!canJump)
		{
			speedMove = speedMoveInAir;
			speedRot = speedRotInAir;
		}
		else
		{
			speedMove = speedMoveNormale;
			speedRot = speedRotNormale;
		}

		Vector3 direction = Input.GetAxis("Horizontal_" + playerID) * LevelManager.mainCameraTrans.right;
		direction += Input.GetAxis("Vertical_" + playerID) * LevelManager.mainCameraTrans.forward;
		direction.y = 0;
		direction = direction.normalized * speedMove;

		if(direction.x != 0 || direction.z != 0)
		{
			_rigidbody.MoveRotation(Quaternion.RotateTowards(_rigidbody.rotation, Quaternion.LookRotation(direction), speedRot * Time.fixedDeltaTime));

			if(canJump)
			{
				_rigidbody.MovePosition(direction * Time.fixedDeltaTime + _rigidbody.position);
			}
		}

		//in air the player don't add externe velocity
		if(!canJump)
		{
			direction.y = _rigidbody.velocity.y;
			_rigidbody.velocity = direction;
		}
	}

	void Jump()
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

			_rigidbody.AddForce(_transform.up * jumpForce);
		}
	}

	public override void UseActionA_Press()
	{
		Jump();
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


#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(transform.position + (Vector3.up * sphereCastGroundOrig), sphereCastGround);
	}
#endif

}
