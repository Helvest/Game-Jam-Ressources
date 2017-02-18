using UnityEngine;

/// <summary>
/// Manage states of the game
/// </summary>
public class GameManager : Singleton<GameManager>
{
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

			switch(value)
			{
				case States.Other:
					break;
				case States.InMenu:
					Cursor.visible = true;
					break;
				case States.InGame:
					Cursor.visible = false;
					break;
				default:
					break;
			}

			state = value;
		}
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