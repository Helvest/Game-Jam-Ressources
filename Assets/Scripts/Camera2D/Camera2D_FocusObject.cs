using UnityEngine;

/// <summary>
/// Script for set camera 2D comportement on a object
/// Comportement: camera is fixed on a position between the objects and a Vector2 position
/// </summary>
public class Camera2D_FocusObject : Camera2DLogic
{
	/// <summary>
	/// Position that camera will use for focus between it and a object
	/// </summary>
	public Vector2 positionToFocus = Vector2.zero;

	[SerializeField]
	private float posPourcent = 0.5f;
	[SerializeField]
	private float CameraSpeed = 1.0F;
	[SerializeField]
	private float distanceMin = 0.1F;

	/// <summary>
	/// Update camera position with the defined comportement
	/// </summary>
	/// <param name="point2D">Object for 2D position and adjustment</param>
	public override void UpdatePoint(ref Point2D point2D)
	{
		if (plan == EnumCameraPlan.X || plan == EnumCameraPlan.XY)
		{
			point2D.position.x = Calcul(_transform.position.x, point2D.position.x, positionToFocus.x);
		}
		if (plan == EnumCameraPlan.Y || plan == EnumCameraPlan.XY)
		{
			point2D.position.y = Calcul(_transform.position.y, point2D.position.y, positionToFocus.y);
		}
	}

	private float Calcul(float transPosition, float point, float focusPosition)
	{
		float objectif = Mathf.LerpUnclamped(transPosition, focusPosition, posPourcent);

		if (Mathf.Abs(objectif - point) <= distanceMin)
			return objectif;
		
		return Mathf.Lerp(point, objectif, CameraSpeed);
	}

#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		if (plan != EnumCameraPlan.NO)
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(Vector2.Lerp(transform.position, positionToFocus, 0.5f), 0.1f);
			Debug.DrawLine(
				transform.position,
				positionToFocus,
				Color.yellow
			);
		}
	}
#endif
}
