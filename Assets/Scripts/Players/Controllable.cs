using UnityEngine;

public class Controllable : MonoBehaviour
{
	protected Transform _transform;

	public bool isControlled = false;

	[Range(1,4)]
	public int playerID = 1;

	protected virtual void Awake()
	{
		_transform = transform;
	}

	protected virtual void Start()
	{
		ControlManager.AddControllableCharacters(this);
	}

	public virtual void UseActionA_Press() { }

	public virtual void UseActionA_Release() { }

	public virtual void UseActionB_Press() { }

	public virtual void UseActionB_Release() { }

	public virtual void UseActionX_Press() { }

	public virtual void UseActionX_Release() { }

	public virtual void UseActionY_Press() { }

	public virtual void UseActionY_Release() { }

	public virtual void Up() { }

	public virtual void UpDouble() { }

	public virtual void Down() { }

	public virtual void DownDouble() { }

	public virtual void Right() { }

	public virtual void RightDouble() { }

	public virtual void Left() { }

	public virtual void LeftDouble() { }

}
