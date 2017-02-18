using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
	[SerializeField]
	private bool dontDestroyOnLoad = true;

	//[SerializeField]
	//private T defaultObject;

	private static T _instance;

	/// <summary>
	/// The Singleton instance, if don't exist create a new one
	/// </summary>
	public static T instance
	{
		get
		{
			// search for an object of type T in the scene
			if(_instance == null)
			{
				_instance = FindObjectOfType<T>();

				if(_instance == null)
				{
					// if the defaultObject is set, Instanciate it in the scene and save it in the variable
					/*if(_instance.defaultObject != null)
					{
						_instance = Instantiate<T>(_instance.defaultObject);
					}
					// create a new object to populate the _instance
					else
					{*/
					GameObject go = new GameObject(typeof(T).Name);
					_instance = go.AddComponent<T>();
					//}
				}
			}

			return _instance;
		}
	}

	protected virtual void Awake()
	{
		if(_instance == null)
		{
			_instance = (T)this;
		}
		else
		{
			Destroy(gameObject);
			return;
		}

		if(dontDestroyOnLoad)
		{
			DontDestroyOnLoad(gameObject);
		}
	}
}
