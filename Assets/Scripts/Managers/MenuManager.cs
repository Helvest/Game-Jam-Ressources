using UnityEngine;

public class MenuManager : MonoBehaviour
{
	public void LoadScene(string sceneName)
	{
		LoadingScreen.instance.Load(sceneName);
	}

	public void LoadScene(int sceneNumber)
	{
		LoadingScreen.instance.Load(sceneNumber);
	}
}
