using UnityEngine;
using System.Collections;


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

	/*
	void Start()
	{

	}

	void Update()
	{

	}
	*/
}