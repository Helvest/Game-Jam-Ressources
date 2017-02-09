using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player3D_Jumper : Controllable
{
	private Rigidbody _rigidbody;

	[SerializeField]
	private float gravity = 10;

	private float speedMove;
	private float speedRot;

	[SerializeField]
	private float speedMoveNormale = 4;
	[SerializeField]
	private float speedRotNormale = 360;

	[SerializeField]
	private float speedMoveRun = 6;
	[SerializeField]
	private float speedRotRun = 360;

	[SerializeField]
	private float speedMoveCrouch = 4;
	[SerializeField]
	private float speedRotCrouch = 360;

	[SerializeField]
	private LayerMask layerMaskGround;
	[SerializeField]
	private Vector3 checkBoxGroundOrig = new Vector3(0,-1,0);
	[SerializeField]
	private Vector3 checkBoxGround = new Vector3(0.15f,0.15f,0.15f);

	//private bool canJump = false;
	//private bool canDoubleJump = false;

	//private Animator animator;

	private Vector3 direction = Vector3.zero;
	private Vector3 tempVector;

	//Actions states
	private bool isMoving = false;
	private bool isRuning = false;
	private bool isCrouch = false;

	//Physic states
	private bool IsGrounded = false;
	private bool isJumping = false;
	private bool isOnWall = false;
	private bool isStoping = false;

	protected override void Awake()
	{
		base.Awake();
		//animator = GetComponent<Animator>();
		_rigidbody = GetComponent<Rigidbody>();

		speedMove = speedMoveNormale;
		speedRot = speedRotNormale;

		//Debug.Log("playerID: "+playerID);
	}

	protected override void Start()
	{
		ControlManager.AddControllableCharacters(this);
	}

	void FixedUpdate()
	{
		if(!isControlled || PauseManager.IsPause)
		{
			return;
		}

		//Game over
		if(_rigidbody.position.y <= -10)
		{
			isJumping = false;
			_rigidbody.velocity = Vector3.zero;
			LevelManager.Respawn();
			return;
		}

		direction = Input.GetAxis("Horizontal_" + playerID) * LevelManager.mainCamera.right;
		direction += Input.GetAxis("Vertical_" + playerID) * LevelManager.mainCamera.forward;
		direction.y = 0;
		direction.Normalize();

		isMoving = direction.x != 0 || direction.z != 0;

		isCrouch = Input.GetButton("ActionY_" + playerID);
		isRuning = Input.GetButton("ActionX_" + playerID);

		if(isJumping)
		{
			Jumping();
		}
		else
		{
			if(isCrouch)
			{
				Debug.Log("isCrouch");
				speedMove = speedMoveCrouch;
				speedRot = speedRotCrouch;
			}
			else if(isRuning && isMoving)
			{
				Debug.Log("isRuning");
				speedMove = speedMoveRun;
				speedRot = speedRotRun;
			}
			else
			{
				speedMove = speedMoveNormale;
				speedRot = speedRotNormale;
			}
		}

		//check ground
		if(_rigidbody.velocity.y > 1 && isJumping)
		{
			Debug.Log("not Grounded");
			IsGrounded = false;
		}
		else
		{
			IsGrounded = Physics.CheckBox(_rigidbody.position + checkBoxGroundOrig, checkBoxGround, _rigidbody.rotation, layerMaskGround);
		}

		if(IsGrounded)
		{
			isJumping = false;
		}

		//Gravity
		if(!isJumping)
		{
			tempVector = _rigidbody.velocity;
			tempVector.y -= gravity * Time.fixedDeltaTime;
			_rigidbody.velocity = tempVector;
			//_rigidbody.MovePosition(_transform.up * -5 * Time.fixedDeltaTime + _rigidbody.position);
		}


		if(isMoving)
		{
			_rigidbody.MoveRotation(Quaternion.RotateTowards(_rigidbody.rotation, Quaternion.LookRotation(direction), speedRot * Time.fixedDeltaTime));
			_rigidbody.MovePosition(direction * speedMove * Time.fixedDeltaTime + _rigidbody.position);
		}
	}

	public override void UseActionA_Press()
	{
		if(IsGrounded)
		{
			Jump();
		}
	}

	public override void UseActionB_Press()
	{
		Debug.Log("UseActionB_Press");
	}

	public override void UseActionX_Press()
	{
		Debug.Log("UseActionX_Press");

	}

	public override void UseActionY_Press()
	{
		Debug.Log("UseActionY_Press");
	}

	[Header("Jump Properties")]
	[SerializeField]
	private AnimationCurve jumpCurveOne;

	//[SerializeField]
	//private AnimationCurve jumpCurveTwo;

	//[SerializeField]
	//private AnimationCurve jumpCurveThree;

	[SerializeField]
	private float jumpTime = 1;
	private float jumpTimer;

	[SerializeField]
	private float DecalJumpTargetY = 0;

	private float JumpHeight;

	[SerializeField]
	private float JumpNormalHeight = 2.2f;

	[SerializeField]
	private float JumpNormalMovingHeight = 3.2f;

	[SerializeField]
	private float JumpRunHeight = 3.2f;

	[SerializeField]
	private float JumpCrouchHeight = 3.2f;

	[SerializeField]
	private float JumpReturnedHeight = 3.2f;

	[SerializeField]
	private float JumpBounceHeight = 1.2f;

	[SerializeField]
	private float JumpWallHeight = 2.2f;
	[SerializeField]
	private float JumpWallDistance = 4;

	[SerializeField]
	private float JumpLongHeight = 1.2f;
	[SerializeField]
	private float JumpLongDistance = 6;

	[SerializeField]
	private float JumpLongRunHeight = 1.2f;
	[SerializeField]
	private float JumpLongRunDistance = 8;

	private Vector3 positionBeforeJump;
	private Vector3 positionTargetJump;



	#region JUMPS

	public void Jump()
	{
		//tempVector.Set(0, 0, 0);
		//_rigidbody.velocity = tempVector;
		if(isOnWall)
		{
			JumpCalcul(true, JumpWallHeight, JumpWallDistance, speedMoveNormale);
		}
		else if(isRuning)
		{
			if(isCrouch)
			{
				JumpCalcul(true, JumpLongRunHeight, JumpLongRunDistance, speedMoveCrouch);
			}
			else if(isStoping)
			{
				JumpCalcul(false, JumpReturnedHeight, 0, 4);
			}
			else
			{
				JumpCalcul(false, JumpRunHeight, 0, speedMoveRun);
			}
		}
		else if(isMoving)
		{
			if(isCrouch)
			{
				JumpCalcul(true, JumpLongHeight, JumpLongDistance, speedMoveCrouch);
			}
			else
			{
				JumpCalcul(false, JumpNormalMovingHeight, 0, speedMoveNormale);
			}
		}
		else
		{
			if(isCrouch)
			{
				JumpCalcul(false, JumpCrouchHeight, 0, speedMoveCrouch);
			}
			else
			{
				JumpCalcul(false, JumpNormalHeight, 0, speedMoveNormale);
			}
		}



	}

	void JumpCalcul(bool haveDirection, float _jumpHeight, float _jumpDistance, float _speedInJump)
	{
		JumpHeight = _jumpHeight;
		speedMove = _speedInJump;

		jumpTimer = 0;

		transitionPosition = positionBeforeJump = _rigidbody.position;

		if(haveDirection)
		{
			tempVector = _transform.forward;
			tempVector.y = 0;

			positionTargetJump = tempVector.normalized * _jumpDistance + _rigidbody.position;
		}
		else
		{
			positionTargetJump = _rigidbody.position;
		}
		positionTargetJump.y += DecalJumpTargetY;

		IsGrounded = false;
		isJumping = true;
	}

	void StopJump(Vector3 newVector)
	{
		if(isJumping)
		{
			jumpTimer = jumpTime + 1;
			_rigidbody.velocity = newVector;
		}
	}

	public void JumpRebound()
	{
		Vector3 tempVector = _rigidbody.velocity;
		tempVector.y = Mathf.Abs(tempVector.y);
		_rigidbody.velocity = tempVector;
	}

	Vector3 transitionPosition;
	Vector3 transitionPositionBefore;
	Vector3 transitionFactor;

	private void Jumping()
	{

		jumpTimer += Time.fixedDeltaTime;

		float pourcent = jumpTimer / jumpTime;

		if(pourcent > 1)
		{
			isJumping = false;
			pourcent = 1;
			/*if(_rigidbody.velocity.y > 0) // ascending
			{
				//_rigidbody.UpCast();
				bool upTouch = false;
				if(upTouch)
				{
					tempVector = _rigidbody.velocity;
					tempVector.y = -tempVector.y;
					_rigidbody.velocity = tempVector;
				}
				//_rigidbody.ForwardCast();
			}*/
		}

		transitionPositionBefore = transitionPosition;

		transitionPosition = Vector3.Lerp(positionBeforeJump, positionTargetJump, pourcent);
		transitionPosition.y += JumpHeight * jumpCurveOne.Evaluate(pourcent);

		transitionFactor = transitionPosition - transitionPositionBefore;

		//Appli mouvement
		//_rigidbody.velocity = Time.fixedDeltaTime > 0f ? (transitionPosition - transitionPositionBefore) * MovePower / Time.fixedDeltaTime : Vector3.zero;
		//_rigidbody.velocity = transitionPosition - transitionPositionBefore;
		_rigidbody.MovePosition(transitionFactor + _rigidbody.position);
		//_rigidbody.position = transitionPosition;

		if(_rigidbody.velocity.y >= 0) //ascending
		{
			//_rigidbody.UpCast();
			bool upTouch = false;

			if(upTouch)
			{
				tempVector = _rigidbody.velocity;
				tempVector.y = -tempVector.y;
				_rigidbody.velocity = tempVector;

				if(pourcent < 0.5f)
				{
					jumpTimer = jumpTime * (1 - pourcent);
				}
			}
			//_rigidbody.ForwardCast();
		}

	}

	#endregion

	#region UNITY_EDITOR
#if UNITY_EDITOR

	Vector3 start, end;
	void OnDrawGizmos()
	{
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireCube(transform.position + checkBoxGroundOrig, checkBoxGround);

		//Jump transition
		Gizmos.color = Color.blue;
		start = positionBeforeJump;
		for(float i = 0; i <= 1; i += 0.05f)
		{
			end = Vector3.Lerp(positionBeforeJump, positionTargetJump, i);
			end.y += JumpHeight * jumpCurveOne.Evaluate(i);

			Gizmos.DrawLine(start, end);

			start = end;
		}

		//Pronostique de direction près le saut
		Gizmos.color = Color.red;
		start = Vector3.Lerp(positionBeforeJump, positionTargetJump, 0.999f);
		start.y += JumpHeight * jumpCurveOne.Evaluate(0.999f);
		Gizmos.DrawLine(start, start + (end - start).normalized * 3);
	}
#endif
	#endregion

}
