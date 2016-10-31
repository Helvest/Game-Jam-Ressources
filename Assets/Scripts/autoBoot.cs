using UnityEngine;
using UnityEngine.SceneManagement;

public class autoBoot : MonoBehaviour
{
	[SerializeField]
	private string sceneMenu = "Menu";

	void LateUpdate()
	{
		SceneManager.LoadScene(sceneMenu);
	}
}
