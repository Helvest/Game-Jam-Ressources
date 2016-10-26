﻿using UnityEngine;

//Script unique pour la camera
public class Camera2D : MonoBehaviour
{
	[HideInInspector]
	public bool IFollowA = true;
	[HideInInspector]
	public Point2D pointA;
	[HideInInspector]
	public Point2D pointB;

	private Point2D point;
	private Point2D pointPre;
	private Camera2DLogic[] targets;

	private Vector3 nPosition;

	private bool isTransition = false;
	private EasingPart easingPart;
	private EasingType easingType;

	float easeTime;
	float easeTimer;

	public void SetTargetWishEase(Transform targetTransforme, float time = 1, EasingPart part = EasingPart.NoEase, EasingType type = EasingType.Linear)
	{
		isTransition = true;

		easingPart = part;
		easingType = type;
		easeTime = time;
		easeTimer = 0;

		SetTarget(targetTransforme);
	}

	public void SetTarget(Transform targetTransforme)
	{
		targets = targetTransforme.GetComponents<Camera2DLogic>();

		if(targets.Length == 0)
		{
			Debug.Log("Aucun script Camera2DLogic dans la cible");
		}
		else
		{
			IFollowA = !IFollowA;

			if(IFollowA)
			{
				point = pointA;
			}
			else
			{
				point = pointB;
			}

			if(!isTransition)
			{
				point.position = targetTransforme.position;
			}
			else
			{
				pointPre.position = transform.position;
			}

			point.decalage = Vector2.zero;

			foreach(Camera2DLogic i in targets)
			{
				point.decalage += i.decalage;
			}
		}
	}

	private void Awake()
	{
#if UNITY_EDITOR
		isPlay = true;
		player = null;
#endif
		pointA = new Point2D();
		pointB = new Point2D();
		pointPre = new Point2D();
	}

	private void LateUpdate()
	{
#if UNITY_EDITOR
		if(point == null)
		{
			EditorSetTarget();
			return;
		}
#endif

		foreach(Camera2DLogic i in targets)
		{
			i.UpdatePoint(ref point);
		}

		if(isTransition)
		{
			easeTimer += Time.deltaTime;

			if(easeTimer < easeTime)
			{
				nPosition = Easing.EaseVector2(pointPre.CameraPosition, point.CameraPosition, easeTimer / easeTime, easingPart, easingType);
			}
			else
			{
				isTransition = false;
			}
		}
		else
		{
			nPosition = point.CameraPosition;
		}

		nPosition.z = transform.position.z;
		transform.position = nPosition;
	}

#if UNITY_EDITOR
	private bool isPlay = false;
	private Transform player;
	private Vector3 playerPosition;

	void EditorSetTarget()
	{
		pointA = new Point2D();
		pointB = new Point2D();
		pointPre = new Point2D();

		if(IFollowA)
		{
			point = pointA;
		}
		else
		{
			point = pointB;
		}

		player = GameObject.FindGameObjectWithTag("Player").transform;

		if(player != null)
		{
			playerPosition = player.position;
			SetTarget(player);
		}
	}

	void OnDrawGizmos()
	{
		if(!isPlay)
		{
			if(player == null || !playerPosition.Equals(player.position))
			{
				EditorSetTarget();
			}

			LateUpdate();
		}
	}
#endif
}