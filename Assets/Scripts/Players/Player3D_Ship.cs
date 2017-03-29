using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player3D_Ship : PlayerScript
{
	private Rigidbody _rigidbody;

	[SerializeField]
	private float speedMoveNormale = 15;
	[SerializeField]
	private float speedMoveBoost = 25;
	[SerializeField]
	private float speedMoveCruise = 50;

	[SerializeField]
	private int speedYaw = 75;
	[SerializeField]
	private int speedPitch = 75;
	[SerializeField]
	private int speedRoll = 75;

	private bool cruiseMode = false;

	//private Vector3 tempVector3 = Vector3.zero;

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

	private float speedMove = 0;
	private Vector3 controlDirection = Vector3.zero;
	void FixedUpdate()
	{
		if(!isControlled || PauseManager.IsPause)
		{
			return;
		}

		//Game over
		/*if(life <= 0)
		{
			_rigidbody.velocity = Vector3.zero;
			LevelManager.Respawn();
			return;
		}*/

		controlDirection.Set(Input.GetAxis("Vertical_" + playerID), Input.GetAxis("Horizontal_" + playerID), Input.GetAxis("Mouse Y"));

		if(controlDirection.x != 0 || controlDirection.y != 0 || controlDirection.z != 0)
		{
			controlDirection.x *= speedPitch;
			controlDirection.y *= speedYaw;
			controlDirection.z *= speedRoll;

			controlDirection *= Time.fixedDeltaTime;
			_rigidbody.MoveRotation(_rigidbody.rotation * Quaternion.Euler(controlDirection));
		}

		if(cruiseMode)
		{
			_rigidbody.MovePosition(_transform.forward * speedMove * Time.fixedDeltaTime + _rigidbody.position);
		}
		else if(Input.GetAxisRaw("R2_" + playerID) == 1)
		{
			if(Input.GetButton("ActionA_" + playerID))
			{
				speedMove = speedMoveBoost;
			}
			else
			{
				speedMove = speedMoveNormale;
			}

			_rigidbody.MovePosition(_transform.forward * speedMove * Time.fixedDeltaTime + _rigidbody.position);
		}
		else
		{
			speedMove = 0;
		}

		//UpdateParticul();
	}

	public override void UseActionA_Press()
	{
		cruiseMode = !cruiseMode;
		speedMove = speedMoveCruise;
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

	/*[SerializeField]
	private ParticleSystem psTrail;
	private int psTrailRate = 600;
	[SerializeField]
	private ParticleSystem psGlow;
	private int psGlowRate = 200;

	private ParticleSystem.EmissionModule emission;
	private void UpdateParticul()
	{
		if(speedMove == 0)
		{
			emission = psTrail.emission;
			emission.rateOverTime = 0;

			emission = psGlow.emission;
			emission.rateOverTime = 0;
		}
		else
		{
			emission = psTrail.emission;
			emission.rateOverTime = psTrailRate;

			emission = psGlow.emission;
			emission.rateOverTime = psGlowRate;
		}
	}*/

	/*
	#if UNITY_EDITOR
		void OnDrawGizmos()
		{
			//Gizmos.color = Color.cyan;
		}
	#endif
	*/
}
