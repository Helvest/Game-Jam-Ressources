using UnityEngine;

/// <summary>
/// Point for 2D position and adjustment
/// </summary>
public class Point2D
{
	/// <summary>
	/// Position of the point
	/// </summary>
	public Vector2 position = Vector2.zero;
	/// <summary>
	/// offset of the point
	/// </summary>
	public Vector2 offset = Vector2.zero;

	/// <summary>
	/// Create a default point
	/// </summary>
	public Point2D(){}

	/// <summary>
	/// Create a point and set position
	/// </summary>
	public Point2D(float X, float Y)
	{
		position = new Vector2(X, Y);
	}

	/// <summary>
	/// Create a point and set position and offset
	/// </summary>
	public Point2D(float X, float Y, float OX, float OY)
	{
		position = new Vector2(X, Y);
		offset = new Vector2(OX, OY);
	}

	/// <summary>
	/// Create a point and set position
	/// </summary>
	public Point2D(Vector2 _position)
	{
		position = _position;
	}

	/// <summary>
	/// Create a point and set position and offset
	/// </summary>
	public Point2D(Vector2 _position, Vector2 _offset)
	{
		position = _position;
		offset = _offset;
	}

	/// <summary>
	/// Create a point and set position
	/// </summary>
	public Point2D(Vector3 _position)
	{
		position = _position;
	}

	/// <summary>
	/// Create a point and set position and offset
	/// </summary>
	public Point2D(Vector3 _position, Vector3 _offset)
	{
		position = _position;
		offset = _offset;
	}

	/// <summary>
	/// Absolute position of the point for the camera
	/// </summary>
	public Vector2 CameraPosition
	{
		get
		{
			return position + offset;
		}
	}

	/// <summary>
	/// Create a default point
	/// </summary>
	public static Point2D zero
	{
		get
		{
			return new Point2D();
		}
	}

}
