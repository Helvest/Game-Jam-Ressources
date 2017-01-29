using UnityEngine;

/// <summary>
/// Manage the objects of the scene
/// </summary>
public class LevelManager : MonoBehaviour
{
	public static Transform player, mainCamera;
	public static Camera3D camera3D;

	void Start()
	{
		GameManager.State = GameManager.States.InGame;

		player = GameObject.FindGameObjectWithTag("Player").transform;
		mainCamera = Camera.main.transform;

		camera3D = mainCamera.GetComponent<Camera3D>();
		camera3D.SetTarget(player);
	}
	/*
	void Update()
	{

	}*/
}
