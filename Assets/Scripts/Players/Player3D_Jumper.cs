using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player3D_Jumper : PlayerScript
{
	public Rigidbody _rigidbody { get; protected set; }

	[SerializeField]
	private float gravity = 0.1f;
	[SerializeField]
	private float maxFallSpeed = -2.0f;

	private float speedMove;
	private float speedRot;

	[SerializeField]
	private float speedForce = 15.0f;

	[SerializeField]
	private float speedMoveNormale = 4.0f;
	[SerializeField]
	private float speedRotNormale = 360.0f;

	[SerializeField]
	private float speedMoveRun = 6.0f;
	[SerializeField]
	private float speedRotRun = 360.0f;

	[SerializeField]
	private float speedMoveCrouch = 4.0f;
	[SerializeField]
	private float speedRotCrouch = 360.0f;

	[SerializeField]
	private LayerMask layerMaskGround;

	[SerializeField]
	private Vector3 checkBoxGroundOrig = new Vector3(0.0f, -1.0f, 0.0f);
	[SerializeField]
	private Vector3 checkBoxGround = new Vector3(0.15f, 0.15f, 0.15f);

	[SerializeField]
	private Vector3 checkBoxForwardOrig = new Vector3(0.0f, 0.0f, 0.5f);
	[SerializeField]
	private Vector3 checkBoxForward = new Vector3(0.15f, 0.15f, 0.15f);
	[SerializeField]
	private float checkBoxForwardDistance = 0.3f;

	[SerializeField]
	private Vector3 checkBoxBackwardOrig = new Vector3(0.0f, 0.0f, -0.5f);
	[SerializeField]
	private Vector3 checkBoxBackward = new Vector3(0.15f, 0.15f, 0.15f);
	[SerializeField]
	private float checkBoxBackwardDistance = 0.3f;

	[SerializeField]
	private Vector3 checkBoxUpOrig = new Vector3(0.0f, 1.0f, 0.0f);
	[SerializeField]
	private Vector3 checkBoxUp = new Vector3(0.15f, 0.15f, 0.15f);

	//private bool canJump = false;
	//private bool canDoubleJump = false;

	//private Animator animator;

	private Vector3 tempVector3 = Vector3.zero;
	private Vector3 direction = Vector3.zero;
	private Vector3 newVelocity = Vector3.zero;

	private Vector2 tempVector2 = Vector2.zero;
	private Vector2 lastJoystickValue = Vector2.zero;

	//Actions states
	private bool isMoving = false;
	private bool isRuning = false;
	private bool isCrouch = false;

	//Physic states
	private bool IsGrounded = false;
	private bool isJumping = false;
	private bool isOnWall = false;
	private bool isStoping = false;


	enum States
	{
		Imobile,
		Walk,
		Run,
		Falling,
		JumpUp,
		JumpDown,
		OnWall,
		WallJumpUp,
		WallJumpDown,
		Crouch,
		LongJump,
		OnWater,
		InWater,
		Flight,
		SuperRun,
		WallRun,
		SlideGround,
		SlideRamp
	}

	private States actualState = States.Imobile;

	private States ActualState
	{
		get
		{
			return actualState;
		}

		set
		{
			if (value == actualState)
			{
				return;
			}

			actualState = value;

			//Debug.Log(actualState);

			switch (actualState)
			{
				default:
				case States.Imobile:
				case States.Walk:
					IsGrounded = true;
					isOnWall = false;
					isJumping = false;
					speedMove = speedMoveNormale;
					speedRot = speedRotNormale;
					break;
				case States.Run:
					IsGrounded = true;
					isOnWall = false;
					isJumping = false;
					speedMove = speedMoveRun;
					speedRot = speedRotRun;
					break;
				case States.Falling:
					IsGrounded = false;
					isOnWall = false;
					isJumping = false;
					break;
				case States.JumpUp:
					IsGrounded = false;
					isOnWall = false;
					isJumping = true;
					//reduire longeur du check down
					break;
				case States.JumpDown:
					IsGrounded = false;
					isOnWall = false;
					isJumping = true;
					break;
				case States.OnWall:
					IsGrounded = false;
					isOnWall = true;
					isJumping = false;
					speedMove = 0.0f;
					speedRot = 0.0f;
					break;
				case States.WallJumpUp:
					IsGrounded = false;
					isOnWall = false;
					isJumping = true;
					break;
				case States.WallJumpDown:
					IsGrounded = false;
					isOnWall = false;
					isJumping = true;
					break;
				case States.Crouch:
					IsGrounded = true;
					isOnWall = false;
					isJumping = false;
					speedMove = speedMoveCrouch;
					speedRot = speedRotCrouch;
					break;
				case States.LongJump:
					IsGrounded = false;
					isOnWall = false;
					isJumping = true;
					break;
				case States.OnWater:
					IsGrounded = false;
					isOnWall = false;
					isJumping = false;
					break;
				case States.InWater:
					IsGrounded = false;
					isOnWall = false;
					isJumping = false;
					break;
				case States.Flight:
					IsGrounded = false;
					isOnWall = false;
					isJumping = false;
					break;
				case States.SuperRun:
					IsGrounded = true;
					isOnWall = false;
					isJumping = false;
					break;
				case States.WallRun:
					IsGrounded = false;
					isOnWall = true;
					isJumping = false;
					break;
				case States.SlideGround:
					IsGrounded = true;
					isOnWall = false;
					isJumping = false;
					break;
				case States.SlideRamp:
					IsGrounded = false;
					isOnWall = false;
					isJumping = false;
					break;
			}
		}
	}

	protected override void Awake()
	{
		base.Awake();
		//animator = GetComponent<Animator>();
		_rigidbody = GetComponent<Rigidbody>();

		speedMove = speedMoveNormale;
		speedRot = speedRotNormale;

		//Debug.Log("playerID: "+playerID);

		//Time.timeScale = 0.5f;
	}

	private Collider[] CheckDownInfos = new Collider[0];
	private bool CheckDown()
	{
		CheckDownInfos = Physics.OverlapBox(_rigidbody.position + _transform.TransformDirection(checkBoxGroundOrig), checkBoxGround, _rigidbody.rotation, layerMaskGround);
		return CheckDownInfos.Length > 0;
	}

	private Collider[] CheckUpInfos = new Collider[0];
	private bool CheckUp()
	{
		//CheckUpInfos = Physics.OverlapBox(_rigidbody.position + checkBoxUpOrig, checkBoxUp, _rigidbody.rotation, layerMaskGround);
		//return CheckUpInfos.Length > 0;
		return false;
	}

	private RaycastHit CheckForwardInfo = new RaycastHit();
	private bool CheckForward()
	{
		Physics.BoxCast(_rigidbody.position + _transform.TransformDirection(checkBoxForwardOrig), checkBoxForward, _transform.forward, out CheckForwardInfo, _rigidbody.rotation, checkBoxForwardDistance, layerMaskGround);

		return CheckForwardInfo.collider;
	}

	private RaycastHit CheckBackwardInfo = new RaycastHit();
	private bool CheckBackward()
	{
		Physics.BoxCast(_rigidbody.position + _transform.TransformDirection(checkBoxBackwardOrig), checkBoxBackward, -_transform.forward, out CheckBackwardInfo, _rigidbody.rotation, checkBoxBackwardDistance, layerMaskGround);

		return CheckBackwardInfo.collider;
	}

	private bool CheckSide()
	{
		return false;
	}

	private void GameOver()
	{
		isJumping = false;
		_rigidbody.velocity = Vector3.zero;
		LevelManager.Instance.Respawn();

		ActualState = States.Imobile;

		return;
	}

	private void Gravity()
	{
		newVelocity.y = _rigidbody.velocity.y - gravity;

		if (newVelocity.y < maxFallSpeed)
		{
			newVelocity.y = maxFallSpeed;
		}
	}

	private void Rotate()
	{
		if (isMoving)
		{
			_rigidbody.MoveRotation(Quaternion.RotateTowards(_rigidbody.rotation, Quaternion.LookRotation(direction), speedRot * Time.fixedDeltaTime));
		}
	}

	private void Move()
	{
		tempVector3 = _rigidbody.velocity;
		float saveY = tempVector3.y;
		tempVector3.y = 0;
		newVelocity = Vector3.MoveTowards(tempVector3, direction * speedMove, speedForce * Time.fixedDeltaTime);
		newVelocity.y = saveY;
	}

	private void MoveOnWall()
	{
		newVelocity = _rigidbody.velocity;
		newVelocity.x = newVelocity.z = 0.0f;
	}

	private void GoOnWall()
	{
		ActualState = States.OnWall;

		tempVector3 = CheckForwardInfo.normal;
		tempVector3.y = 0;
		_transform.rotation = Quaternion.LookRotation(tempVector3);

		//Debug.Log("Go on " + CheckForwardInfos[0].collider.name + ": " + tempVector3);
	}

	/*private void CheckHit(ref RaycastHit[] raycastHitInfos)
	{
		if (raycastHitInfos[0].distance > 0.0f)
		{
			return;
		}

		tempVector3.Set(_rigidbody.position.x, checkBoxForwardOrig.y, _rigidbody.position.z);

		raycastHitInfos = Physics.RaycastAll(tempVector3, _transform.forward, 0.5f, layerMaskGround);
	}*/

	void FixedUpdate()
	{
		if (!isControlled || PauseManager.Instance.IsPause)
		{
			return;
		}

		//Game over
		if (_rigidbody.position.y < -10.0f)
		{
			GameOver();
			return;
		}

		//valeur du joystic
		tempVector2.Set(Input.GetAxis("Horizontal_" + playerID), Input.GetAxis("Vertical_" + playerID));

		//Check si la valeur de l'angle ou de la magnitude du joystick a changée
		if (Vector2.Angle(tempVector2, lastJoystickValue) > 1.0f || Mathf.Abs(tempVector2.magnitude - lastJoystickValue.magnitude) > 0.1f)
		{
			//si oui: recup direction joystick et addapte selon camera

			lastJoystickValue = tempVector2;

			direction = tempVector2.x * LevelManager.Instance.mainCameraTrans.right;
			direction += tempVector2.y * LevelManager.Instance.mainCameraTrans.forward;
			direction.y = 0.0f;
			direction.Normalize();
		}

		//sinon: garde direction actuel
		isMoving = Mathf.Abs(direction.x) > 0.1f || Mathf.Abs(direction.z) > 0.1f;

		isCrouch = Input.GetButton("ActionY_" + playerID);
		isRuning = Input.GetButton("ActionX_" + playerID);

		int limit = 2;

	StateUpdate:

		if (limit <= 0)
		{
			Debug.LogWarning("limit of State Update");
			goto EndFixedUpdate;
		}
		else
		{
			limit--;
		}

		switch (actualState)
		{
			case States.Imobile:
			case States.Walk:
			case States.Run:
				if (!CheckDown())
				{
					ActualState = States.Falling;
					goto StateUpdate;
				}
				else if (CheckUp())
				{
					ActualState = States.Crouch;
					goto StateUpdate;
				}
				else if (CheckForward())
				{
					if (actualState != States.Imobile)
					{
						ActualState = States.Imobile;
						goto StateUpdate;
					}
				}

				Rotate();
				Move();
				Gravity();
				break;

			case States.JumpUp:
			case States.WallJumpUp:

				/*if (CheckDown())
				{
					ActualState = States.Imobile;
					goto StateUpdate;
				}*/

				if (CheckUp())
				{
					switch (actualState)
					{
						case States.JumpUp:
							ActualState = States.JumpDown;
							break;
						case States.WallJumpUp:
							ActualState = States.WallJumpDown;
							break;
					}
					goto StateUpdate;
				}

				Jumping();
				Move();

				break;

			case States.Falling:
				if (CheckDown())
				{
					ActualState = States.Imobile;
					goto StateUpdate;
				}
				else if (CheckForward())
				{
					GoOnWall();
					goto StateUpdate;
				}
				else if (CheckBackward() || CheckSide())
				{
					GoOnWall();
					goto StateUpdate;
				}

				Move();
				Gravity();
				break;

			case States.JumpDown:

				if (CheckDown())
				{
					ActualState = States.Imobile;
					goto StateUpdate;
				}
				else if (CheckForward())
				{
					GoOnWall();
					goto StateUpdate;
				}
				else if (CheckBackward() || CheckSide())
				{
					GoOnWall();
					goto StateUpdate;
				}

				Jumping();
				Move();

				break;

			case States.OnWall:
				if (CheckDown())
				{
					ActualState = States.Imobile;
					goto StateUpdate;
				}
				else if (CheckUp())
				{
					ActualState = States.Falling;
					goto StateUpdate;
				}
				else if (!CheckBackward())
				{
					ActualState = States.Falling;
					goto StateUpdate;
				}

				MoveOnWall();

				//Gravity();
				newVelocity.y = _rigidbody.velocity.y;

				newVelocity.y -= 0.12f;

				if (newVelocity.y < -3f)
				{
					newVelocity.y = -3f;
				}

				break;

			case States.WallJumpDown:

				if (CheckDown())
				{
					ActualState = States.Imobile;
					goto StateUpdate;
				}
				else if (CheckForward())
				{
					//ActualState = States.OnWall;
					GoOnWall();
					goto StateUpdate;
				}
				else if (CheckSide())
				{
					ActualState = States.OnWall;
					goto StateUpdate;
				}

				Jumping();
				Move();

				break;

			case States.Crouch:
				if (!CheckDown())
				{
					ActualState = States.Falling;
					goto StateUpdate;
				}
				else if (CheckUp())
				{

					//Check distance pour écrasement et si possible redressement

					//DEATH

					GameOver();
					return;
				}

				Rotate();
				Move();

				Gravity();
				break;

			case States.LongJump:
				/*if (CheckDown())
				{
					ActualState = States.Imobile;
					goto StateUpdate;
				}*/
				if (CheckForward())
				{
					//ActualState = States.OnWall;
					GoOnWall();
					goto StateUpdate;
				}
				else if (CheckSide())
				{
					ActualState = States.OnWall;
					goto StateUpdate;
				}

				Jumping();
				Move();
				break;

			case States.OnWater:

				if (CheckDown())
				{
					//info pour sol ou cascade = Imobile ou Falling

					ActualState = States.Imobile;
					goto StateUpdate;
				}

				Rotate();
				Move();
				break;
				/*
				case States.InWater:
					Rotate();
					break;
				case States.Flight:
					if (!CheckDown())
					{

					}
					break;
				case States.SuperRun:
					if (!CheckDown())
					{

					}

					Gravity();
					break;
				case States.WallRun:
					if (!CheckDown())
					{

					}
					break;
				case States.SlideGround:
					if (!CheckDown())
					{

					}

					Gravity();
					break;
				case States.SlideRamp:
					if (!CheckDown())
					{

					}
					break;*/
		}

	EndFixedUpdate:

		_rigidbody.velocity = newVelocity;
	}

	/*public override void UseActionA_Press()
	{
		switch (actualState)
		{
			case States.Imobile:
			case States.Walk:
			case States.Run:
			case States.OnWall:
			case States.Crouch:
			case States.OnWater:
			case States.Flight:
			case States.SuperRun:
			case States.WallRun:
			case States.SlideGround:
			case States.SlideRamp:
				Jump();
				break;
		}
	}*/

	/*
	public override void UseActionB_Press()
	{
		//Debug.Log("UseActionB_Press");
	}

	public override void UseActionX_Press()
	{
		//Debug.Log("UseActionX_Press");
	}

	public override void UseActionY_Press()
	{
		//Debug.Log("UseActionY_Press");
	}
	*/

	[Header("Jump Properties")]
	[SerializeField]
	private AnimationCurve jumpCurveDefault;

	[SerializeField]
	private AnimationCurve jumpCurveLong;

	[SerializeField]
	private AnimationCurve jumpCurveOther;

	[SerializeField]
	private float jumpTime = 1.0f;
	private float jumpTimer;

	[SerializeField]
	private float DecalJumpTargetY = 0.0f;

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
	private float JumpWallDistance = 4.0f;

	[SerializeField]
	private float JumpLongHeight = 1.2f;
	[SerializeField]
	private float JumpLongDistance = 6.0f;

	[SerializeField]
	private float JumpLongRunHeight = 1.2f;
	[SerializeField]
	private float JumpLongRunDistance = 8.0f;

	private Vector3 positionBeforeJump;
	private Vector3 positionTargetJump;

	#region JUMPS

	public void Jump()
	{
		if (isOnWall)
		{
			ActualState = States.WallJumpUp;
			JumpCalcul(true, JumpWallHeight, JumpWallDistance, speedMoveNormale);
		}
		else if (isRuning)
		{
			if (isCrouch)
			{
				ActualState = States.LongJump;
				JumpCalcul(true, JumpLongRunHeight, JumpLongRunDistance, speedMoveCrouch);
			}
			else if (isStoping)
			{
				ActualState = States.JumpUp;
				JumpCalcul(false, JumpReturnedHeight, 0.0f, 4.0f);
			}
			else
			{
				ActualState = States.JumpUp;
				JumpCalcul(false, JumpRunHeight, 0.0f, speedMoveRun);
			}
		}
		else if (isMoving)
		{
			if (isCrouch)
			{
				ActualState = States.LongJump;
				JumpCalcul(true, JumpLongHeight, JumpLongDistance, speedMoveCrouch);
			}
			else
			{
				ActualState = States.JumpUp;
				JumpCalcul(false, JumpNormalMovingHeight, 0.0f, speedMoveNormale);
			}
		}
		else
		{
			if (isCrouch)
			{
				ActualState = States.JumpUp;
				JumpCalcul(false, JumpCrouchHeight, 0, speedMoveCrouch);
			}
			else
			{
				ActualState = States.JumpUp;
				JumpCalcul(false, JumpNormalHeight, 0, speedMoveNormale);
			}
		}
	}

	private bool firstPartJump = false;

	void JumpCalcul(bool haveDirection, float _jumpHeight, float _jumpDistance, float _speedInJump)
	{
		JumpHeight = _jumpHeight;
		speedMove = _speedInJump;

		jumpTimer = 0.0f;

		transitionPosition = positionBeforeJump = _rigidbody.position;

		if (haveDirection)
		{
			tempVector3 = _transform.forward;
			tempVector3.y = 0;

			positionTargetJump = tempVector3.normalized * _jumpDistance + _rigidbody.position;
		}
		else
		{
			positionTargetJump = _rigidbody.position;
		}

		positionTargetJump.y += DecalJumpTargetY;

		firstPartJump = true;
		upTouchInJumping = false;
	}

	bool upTouchInJumping = false;

	public void JumpRebound()
	{
		JumpCalcul(false, JumpBounceHeight, 0.0f, speedMove);
	}

	private Vector3 transitionPosition;
	private Vector3 transitionPositionBefore;
	private Vector3 transitionFactor;

	private float pourcent;

	private void Jumping()
	{
		jumpTimer += Time.fixedDeltaTime;

		pourcent = jumpTimer / jumpTime;

		if (firstPartJump && pourcent > 0.5f)
		{
			firstPartJump = false;

			switch (actualState)
			{
				case States.JumpUp:
					ActualState = States.JumpDown;
					break;
				case States.WallJumpUp:
					ActualState = States.WallJumpDown;
					break;
			}
		}

		if (upTouchInJumping) //ascending
		{
			upTouchInJumping = false;
			if (pourcent < 0.5f)
			{
				jumpTimer = jumpTime * (1.0f - pourcent);
			}
		}

		if (pourcent >= 1.0f)
		{
			actualState = States.Falling;

			pourcent = 1.0f - Time.fixedDeltaTime;

			transitionPositionBefore = Vector3.Lerp(positionBeforeJump, positionTargetJump, pourcent);
			transitionPositionBefore.y += JumpHeight * jumpCurveDefault.Evaluate(pourcent);

			//Appli velocity 
			_rigidbody.velocity += (positionTargetJump - transitionPositionBefore) / Time.fixedDeltaTime;
		}
		else
		{
			transitionPositionBefore = transitionPosition;

			transitionPosition = Vector3.Lerp(positionBeforeJump, positionTargetJump, pourcent);
			transitionPosition.y += JumpHeight * jumpCurveDefault.Evaluate(pourcent);

			transitionFactor = transitionPosition - transitionPositionBefore;

			//Appli mouvement
			tempVector3 = transitionFactor;
			tempVector3.y = 0.0f;
			_rigidbody.MovePosition(tempVector3 + _rigidbody.position);

			tempVector3 = _rigidbody.velocity;
			tempVector3.y = transitionFactor.y / Time.fixedDeltaTime;
			_rigidbody.velocity = tempVector3;
		}
	}

	/*private void OnTriggerEnter(Collider other)
	{
		
	}*/

	/*	private void OnCollisionEnter(Collision collision)
		{
			if (isJumping)
			{
				if (collision.contacts[0].point.y >= _rigidbody.position.y)
				{
					if (true)
					{
						upTouchInJumping = true;
					}
				}

				float testAngle = Vector3.Angle(collision.contacts[0].point - _rigidbody.position, _transform.forward);

				if (testAngle <= 90.0f)
				{
					ActualState = States.OnWall;

					tempVector3 = collision.contacts[0].normal;
					tempVector3.y = 0;
					_rigidbody.MoveRotation(Quaternion.LookRotation(tempVector3));
				}
			}
		}*/

	#endregion


	#region UNITY_EDITOR
#if UNITY_EDITOR

	Vector3 start, end;

	private void DrawBoxCast(Vector3 origine, Vector3 size, float distance)
	{
		origine.z += origine.z > 0 ? distance / 2.0f : -distance / 2.0f;
		origine += transform.position;

		size *= 2.0f;
		size.z += distance;

		Gizmos.DrawWireCube(origine, size);
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireCube(transform.position + checkBoxGroundOrig, checkBoxGround * 2.0f);

		DrawBoxCast(checkBoxForwardOrig, checkBoxForward, checkBoxForwardDistance);

		DrawBoxCast(checkBoxBackwardOrig, checkBoxBackward, checkBoxBackwardDistance);

		//Gizmos.DrawWireCube(transform.position + checkBoxBackwardOrig, checkBoxBackward);
		Gizmos.DrawWireCube(transform.position + checkBoxUpOrig, checkBoxUp);

		//Jump transition
		Gizmos.color = Color.blue;
		start = positionBeforeJump;
		for (float i = 0; i <= 1; i += 0.05f)
		{
			end = Vector3.Lerp(positionBeforeJump, positionTargetJump, i);
			end.y += JumpHeight * jumpCurveDefault.Evaluate(i);

			Gizmos.DrawLine(start, end);

			start = end;
		}

		//Pronostique de direction près le saut
		Gizmos.color = Color.red;
		start = Vector3.Lerp(positionBeforeJump, positionTargetJump, 0.999f);
		start.y += JumpHeight * jumpCurveDefault.Evaluate(0.999f);
		Gizmos.DrawLine(start, start + (end - start).normalized * 3);
	}
#endif
	#endregion

}
