using UnityEngine;

/// <summary>
/// Manage the objects of the scene
/// </summary>
public class LevelManager : Singleton<LevelManager>
{
	public Transform playerTrans, mainCameraTrans;

	public PlayerScript playerScript;
	public CameraScript cameraScript;

	public Vector3 lastSavedPosition;

	protected override void OnAwake()
	{
		GameManager.Instance.State = GameManager.States.InGame;
		playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
		mainCameraTrans = Camera.main.transform;
	}

	private void Start()
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

	public void Respawn()
	{
		playerTrans.position = lastSavedPosition;
	}
}
