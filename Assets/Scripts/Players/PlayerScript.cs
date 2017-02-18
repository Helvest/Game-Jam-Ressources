using UnityEngine;

/// <summary>
/// Script with functions activate by inputs
/// </summary>
public class PlayerScript : MonoBehaviour
{
	/// <summary>
	/// Optimisation for call transform
	/// </summary>
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
		ControlManager.AddPlayerScriptCharacters(this);
	}

	protected virtual void OnDestroy()
	{
		ControlManager.RemovePlayerScriptCharacters(this);
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
