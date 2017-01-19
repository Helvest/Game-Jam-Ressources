using UnityEngine;

/// <summary>
/// Script with functions activate by inputs
/// </summary>
public class Controllable : MonoBehaviour
{
	/// <summary>
	/// Optimisation for call transform
	/// </summary>
	protected Transform _transform;

	public bool isControlled = false;

	[Range(1,4)]
	public int playerID = 1;

	/// <summary></summary>
	protected virtual void Awake()
	{
		_transform = transform;
	}

	/// <summary></summary>
	protected virtual void Start()
	{
		ControlManager.AddControllableCharacters(this);
	}

	/// <summary></summary>
	public virtual void UseActionA_Press() { }
	/// <summary></summary>
	public virtual void UseActionA_Release() { }
	/// <summary></summary>
	public virtual void UseActionB_Press() { }
	/// <summary></summary>
	public virtual void UseActionB_Release() { }
	/// <summary></summary>
	public virtual void UseActionX_Press() { }
	/// <summary></summary>
	public virtual void UseActionX_Release() { }
	/// <summary></summary>
	public virtual void UseActionY_Press() { }
	/// <summary></summary>
	public virtual void UseActionY_Release() { }
	/// <summary></summary>
	public virtual void Up() { }
	/// <summary></summary>
	public virtual void UpDouble() { }
	/// <summary></summary>
	public virtual void Down() { }
	/// <summary></summary>
	public virtual void DownDouble() { }
	/// <summary></summary>
	public virtual void Right() { }
	/// <summary></summary>
	public virtual void RightDouble() { }
	/// <summary></summary>
	public virtual void Left() { }
	/// <summary></summary>
	public virtual void LeftDouble() { }

}
