using UnityEngine;

/// <summary>
/// Manage the objects of the scene
/// </summary>
public class LevelManager : Singleton<LevelManager>
{
	public static Transform playerTrans, mainCameraTrans;

	public static PlayerScript playerScript;
	public static CameraScript cameraScript;

	public static Vector3 lastSavedPosition;

	protected override void Awake()
	{
		base.Awake();
		GameManager.State = GameManager.States.InGame;
		playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
		mainCameraTrans = Camera.main.transform;
	}

	void Start()
	{
		if(playerTrans)
		{
			lastSavedPosition = playerTrans.position;
		}

		cameraScript = mainCameraTrans.GetComponent<CameraScript>();
		if(cameraScript && playerTrans)
		{
			cameraScript.SetTarget(playerTrans);
		}
	}

	public static void Respawn()
	{
		playerTrans.position = lastSavedPosition;
	}
}
