using UnityEngine;

/// <summary>
/// Script for set camera 2D comportement on a object
/// Comportement: camera is fixed on a position relative to a object
/// </summary>
public class Camera2DLogic : MonoBehaviour
{
	/// <summary>
	/// Axe of comportement
	/// </summary>
	[SerializeField]
	protected EnumCameraPlan plan = EnumCameraPlan.XY;
	/// <summary>
	/// 
	/// </summary>
	public Vector2 offset = Vector2.zero;

	/// <summary>
	/// Optimisation for call transform
	/// </summary>
    protected Transform _transform;

	/// <summary>
	/// </summary>
    virtual protected void Awake()
    {
        _transform = transform;
    }

	/// <summary>
	/// Update camera position with the defined comportement
	/// </summary>
	/// <param name="point2D">Object for 2D position and adjustment</param>
	virtual public void UpdatePoint(ref Point2D point2D)
	{
#if UNITY_EDITOR
		_transform = transform;
#endif
		switch(plan)
        {
            case EnumCameraPlan.XY:
                point2D.position = _transform.position;
                break;
            case EnumCameraPlan.X:
                point2D.position.x = _transform.position.x;
                break;
            case EnumCameraPlan.Y:
                point2D.position.y = _transform.position.y;
                break;
        }

		point2D.offset = offset;

	}
}
