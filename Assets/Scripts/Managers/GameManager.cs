using UnityEngine;

public class GameManager : MonoBehaviour
{
	private static GameManager instance;

	private GameManager(){}

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

	public enum States
	{
		Other,
		InMenu,
		InGame,
		InLoading
	}

	private static States state = States.InMenu;
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

	public void LoadScene(string sceneName)
	{
		LoadingScreen.instance.Load(sceneName);
	}

	public void LoadScene(int sceneNumber)
	{
		LoadingScreen.instance.Load(sceneNumber);
	}

}