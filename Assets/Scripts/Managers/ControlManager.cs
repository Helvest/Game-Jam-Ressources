using UnityEngine;

/// <summary>
/// Manage advanced inputs and PlayerScript scripts
/// </summary>
public class ControlManager : Singleton<ControlManager>
{

	protected override void Awake()
	{
		base.Awake();

		for(int i = 0; i < 4; i++)
		{
			upDoubleTimer[i] =
			downDoubleTimer[i] =
			rightDoubleTimer[i] =
			leftDoubleTimer[i] =
			0;

			upWasPress[i] =
			downWasPress[i] =
			rightWasPress[i] =
			leftWasPress[i] =
			false;
		}
	}

	private static PlayerScript[] PlayerScriptCharacters = new PlayerScript[0];

	/// <summary>
	/// Add PlayerScript objets on the liste of actifs PlayerScripts
	/// </summary>
	public static void AddPlayerScriptCharacters(PlayerScript PlayerScript)
	{
		int ccLength = PlayerScriptCharacters.Length;
		//check is player is already in
		for(int i = 0; i < ccLength; i++)
		{
			if(PlayerScriptCharacters[i] == PlayerScript)
			{
				return;
			}
		}

		//expand array
		PlayerScript[] tempsArray = new PlayerScript[ccLength + 1];

		for(int i = 0; i < ccLength; i++)
		{
			tempsArray[i] = PlayerScriptCharacters[i];
		}

		PlayerScriptCharacters = tempsArray;

		//add PlayerScript
		PlayerScriptCharacters[ccLength] = PlayerScript;

		PlayerScript.isControlled = true;
	}

	/// <summary>
	/// Remove PlayerScript objets on the liste of actifs PlayerScripts
	/// </summary>
	public static void RemovePlayerScriptCharacters(PlayerScript PlayerScript)
	{
		int ccLength = PlayerScriptCharacters.Length;
		int index = -1;

		//check is player is in
		for(int i = 0; i < ccLength; i++)
		{
			if(PlayerScriptCharacters[i] == PlayerScript)
			{
				index = i;
				break;
			}
		}

		if(index == -1)
		{
			return;
		}

		//shrink array
		PlayerScript[] tempsArray = new PlayerScript[ccLength - 1];

		//remove PlayerScript
		for(int i = 0; i < ccLength; i++)
		{
			if(i < index)
			{
				tempsArray[i] = PlayerScriptCharacters[i];
			}
			else if(i > index)
			{
				tempsArray[i - 1] = PlayerScriptCharacters[i - 1];
			}
		}

		PlayerScriptCharacters = tempsArray;

		PlayerScript.isControlled = false;
	}

	[SerializeField]
	private float upDoubleTime = 0.175f;
	private static float[] upDoubleTimer = new float[4];
	private static bool[] upWasPress = new bool[4];

	[SerializeField]
	private float downDoubleTime = 0.175f;
	private static float[] downDoubleTimer = new float[4];
	private static bool[] downWasPress = new bool[4];

	[SerializeField]
	private float rightDoubleTime = 0.175f;
	private static float[] rightDoubleTimer = new float[4];
	private static bool[] rightWasPress = new bool[4];

	[SerializeField]
	private float leftDoubleTime = 0.175f;
	private static float[] leftDoubleTimer = new float[4];
	private static bool[] leftWasPress = new bool[4];

	private static PlayerScript PlayerScript;
	private static int playerID;

	void Update()
	{
		if(PauseManager.IsPause)
		{
			return;
		}

		//parcour la liste des joueurs
		for(int i = 0; i < PlayerScriptCharacters.Length; i++)
		{
			PlayerScript = PlayerScriptCharacters[i];

			if(!PlayerScript)
			{
				RemovePlayerScriptCharacters(PlayerScript);
				continue;
			}
			else if(!PlayerScript.isControlled)
			{
				RemovePlayerScriptCharacters(PlayerScript);
				continue;
			}

			playerID = PlayerScript.playerID;

			//ActionA
			if(Input.GetButtonDown("ActionA_" + playerID))
			{
				PlayerScript.UseActionA_Press();
			}

			if(Input.GetButtonUp("ActionA_" + playerID))
			{
				PlayerScript.UseActionA_Release();
			}

			//ActionB
			if(Input.GetButtonDown("ActionB_" + playerID))
			{
				PlayerScript.UseActionB_Press();
			}

			if(Input.GetButtonUp("ActionB_" + playerID))
			{
				PlayerScript.UseActionB_Release();
			}

			//ActionX
			if(Input.GetButtonDown("ActionX_" + playerID))
			{
				PlayerScript.UseActionX_Press();
			}

			if(Input.GetButtonUp("ActionX_" + playerID))
			{
				PlayerScript.UseActionX_Release();
			}

			//ActionY
			if(Input.GetButtonDown("ActionY_" + playerID))
			{
				PlayerScript.UseActionY_Press();
			}

			if(Input.GetButtonUp("ActionY_" + playerID))
			{
				PlayerScript.UseActionY_Release();
			}

			//Up
			if(upWasPress[i])
			{
				upDoubleTimer[i] -= Time.deltaTime;

				if(Input.GetButtonDown("DPadY_" + playerID) && Input.GetAxisRaw("DPadY_" + playerID) == 1)
				{
					PlayerScript.UpDouble();
					upWasPress[i] = false;
				}
				else if(upDoubleTimer[i] <= 0)
				{
					PlayerScript.Up();
					upWasPress[i] = false;
				}
			}
			else
			{
				upWasPress[i] = Input.GetButtonDown("DPadY_" + playerID) && Input.GetAxisRaw("DPadY_" + playerID) == 1;
				upDoubleTimer[i] = upDoubleTime;
			}

			//Down
			if(downWasPress[i])
			{
				downDoubleTimer[i] -= Time.deltaTime;

				if(Input.GetButtonDown("DPadY_" + playerID) && Input.GetAxisRaw("DPadY_" + playerID) == -1)
				{
					PlayerScript.DownDouble();
					downWasPress[i] = false;
				}
				else if(downDoubleTimer[i] <= 0)
				{
					PlayerScript.Down();
					downWasPress[i] = false;
				}
			}
			else
			{
				downWasPress[i] = Input.GetButtonDown("DPadY_" + playerID) && Input.GetAxisRaw("DPadY_" + playerID) == -1;
				downDoubleTimer[i] = downDoubleTime;
			}

			//Right
			if(rightWasPress[i])
			{
				rightDoubleTimer[i] -= Time.deltaTime;

				if(Input.GetButtonDown("DPadX_" + playerID) && Input.GetAxisRaw("DPadX_" + playerID) == 1)
				{
					PlayerScript.RightDouble();
					rightWasPress[i] = false;
				}
				else if(rightDoubleTimer[i] <= 0)
				{
					PlayerScript.Right();
					rightWasPress[i] = false;
				}
			}
			else
			{
				rightWasPress[i] = Input.GetButtonDown("DPadX_" + playerID) && Input.GetAxisRaw("DPadX_" + playerID) == 1;
				rightDoubleTimer[i] = rightDoubleTime;
			}

			//Left
			if(leftWasPress[i])
			{
				leftDoubleTimer[i] -= Time.deltaTime;

				if(Input.GetButtonDown("DPadX_" + playerID) && Input.GetAxisRaw("DPadX_" + playerID) == -1)
				{
					PlayerScript.LeftDouble();
					leftWasPress[i] = false;
				}
				else if(leftDoubleTimer[i] <= 0)
				{
					PlayerScript.Left();
					leftWasPress[i] = false;
				}
			}
			else
			{
				leftWasPress[i] = Input.GetButtonDown("DPadX_" + playerID) && Input.GetAxisRaw("DPadX_" + playerID) == -1;
				leftDoubleTimer[i] = leftDoubleTime;
			}
		}
	}
}

/// <summary>
/// Liste of all possible inputs
/// </summary>
public enum InputListe
{
	None,
	ActionA,
	ActionB,
	ActionX,
	ActionY,
	Up,
	UpDouble,
	Down,
	DownDouble,
	Righ,
	RightDouble,
	Left,
	LeftDouble,
}