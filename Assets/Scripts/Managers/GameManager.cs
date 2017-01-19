using UnityEngine;

/// <summary>
/// Manage states of the game
/// </summary>
public class GameManager : MonoBehaviour
{
	private static GameManager instance;

	private GameManager(){}

	/// <summary>
	/// Instance of the singeton GameManager
	/// </summary>
	public static GameManager Instance
	{
		get
		{
			if(instance == null)
			{
				instance = new GameManager();
			}
			return instance;
		}
	}

	/// <summary>
	/// Possible states of the game
	/// </summary>
	public enum States
	{
		Other,
		InMenu,
		InGame,
		InLoading
	}

	private static States state = States.InMenu;
	/// <summary>
	/// Actual state of the game
	/// Make the transition from one state to another
	/// </summary>
	public static States State
	{
		get
		{
			return state;
		}

		set
		{
			if(state == value)
			{
				return;
			}

			switch(state)
			{
				case States.Other:
					break;
				case States.InMenu:
					//Cursor.visible = false;
					break;
				case States.InGame:
					break;
				default:
					break;
			}

			switch(value)
			{
				case States.Other:
					break;
				case States.InMenu:
					//Cursor.visible = true;
					break;
				case States.InGame:
					break;
				default:
					break;
			}

			state = value;
		}
	}

	void Awake()
	{
		if(instance)
		{
			Destroy(gameObject);
			return;
		}

		instance = this;
		DontDestroyOnLoad(gameObject);
	}

	/// <summary>
	/// Load Scene with the loader
	/// </summary>
	public void LoadScene(string sceneName)
	{
		LoadingScreen.instance.Load(sceneName);
	}

	/// <summary>
	/// Load Scene with the loader
	/// </summary>
	public void LoadScene(int sceneNumber)
	{
		LoadingScreen.instance.Load(sceneNumber);
	}

}