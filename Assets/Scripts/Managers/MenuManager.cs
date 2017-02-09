using UnityEngine;

/// <summary>
/// Manage menu and boutons functions
/// </summary>
public class MenuManager : Singleton<MenuManager>
{
	protected override void Awake()
	{
		base.Awake();
		GameManager.State = GameManager.States.InMenu;
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
