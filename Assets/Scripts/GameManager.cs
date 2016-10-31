using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
	private static GameManager instance;

	private GameManager()
	{
	}

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

	void Awake()
	{
		DontDestroyOnLoad(transform.gameObject);
	}

	public void LoadScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}

	public void LoadScene(int sceneNumber)
	{
		SceneManager.LoadScene(sceneNumber);
	}

	/*
	void Start()
	{

	}

	void Update()
	{

	}
	*/
}