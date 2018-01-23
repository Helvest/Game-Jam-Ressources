using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2D_beat_em_up : PlayerScript
{
	public Rigidbody _rigidbody { get; protected set; }

	[SerializeField]
	private Transform ground;

	protected override void Awake()
	{
		base.Awake();

		_rigidbody = GetComponent<Rigidbody>();

		origine = _transform.position;
	}

	private void LightHit()
	{

	}

	private void MediumHit()
	{

	}

	private void HeavyHit()
	{

	}

	private void Jump()
	{

	}

	Vector3 origine;
	Vector3 jumpVector = new Vector3(0f, 4f, 0f);

	private void Update()
	{
		if (_rigidbody.position.y < -3f)
		{
			_rigidbody.position = origine;
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			_rigidbody.AddForce(jumpVector * 10f, ForceMode.Impulse);
		}

	}

	bool isMoving = false;

	Vector3 newVelocity;
	Vector3 direction;
	float speedMove = 4f;
	float speedForce = 15f;

	Vector2 tempVector2;
	Vector3 tempVector3;

	void FixedUpdate()
	{
		if (!isControlled || PauseManager.Instance.IsPause)
		{
			return;
		}

		//valeur du joystic
		tempVector3 = new Vector3(Input.GetAxis("Horizontal_" + playerID), 0f, Input.GetAxis("Vertical_" + playerID));

		isMoving = Mathf.Abs(tempVector3.x) > 0.1f || Mathf.Abs(tempVector3.z) > 0.1f;

		if (isMoving)
		{
			direction = tempVector3.z * ground.forward;
			direction.x = tempVector3.x;

			Move();

			_rigidbody.velocity = newVelocity;
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

	private void OnCollisionEnter(Collision collision)
	{

	}

	private void OnTriggerEnter(Collider other)
	{

	}
}
