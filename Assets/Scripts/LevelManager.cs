using UnityEngine;

/// <summary>
/// Manage the objects of the scene
/// </summary>
public class LevelManager : MonoBehaviour
{
	Transform player, mainCamera;
	Camera2D camera2D;

	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
		mainCamera = Camera.main.transform;

		camera2D = mainCamera.GetComponent<Camera2D>();
		camera2D.SetTarget(player);
	}
	/*
	void Update()
	{

	}*/
}
