using UnityEngine;

/// <summary>
/// Manage the objects of the scene
/// </summary>
public class LevelManager : Singleton<LevelManager>
{
	public static Transform player, mainCamera;
	public static Camera3D camera3D;
	public static Vector3 lastSavedPosition;

	void Start()
	{
		GameManager.State = GameManager.States.InGame;

		player = GameObject.FindGameObjectWithTag("Player").transform;
		lastSavedPosition = player.position;

		mainCamera = Camera.main.transform;

		camera3D = mainCamera.GetComponent<Camera3D>();
		camera3D.SetTarget(player);
	}

	public static void Respawn()
	{
		player.position = lastSavedPosition;
	}
}
