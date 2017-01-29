using UnityEngine;

/// <summary>
/// Manage advanced inputs and Controllable scripts
/// </summary>
public class ControlManager : MonoBehaviour
{
	private static ControlManager instance;

	private ControlManager() { }
	/// <summary>
	/// Instance of the singeton ControlManager
	/// </summary>
	public static ControlManager Instance
	{
		get
		{
			if(instance == null)
			{
				instance = new ControlManager();
			}
			return instance;
		}
	}

	void Awake()
	{
		if(instance)
		{
			Destroy(this);
			return;
		}

		instance = this;
		DontDestroyOnLoad(gameObject);

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

	private static Controllable[] controllableCharacters = new Controllable[0];

	/// <summary>
	/// Add controllable objets on the liste of actifs controllables
	/// </summary>
	public static void AddControllableCharacters(Controllable controllable)
	{
		int ccLength = controllableCharacters.Length;
		//check is player is already in
		for(int i = 0; i < ccLength; i++)
		{
			if(controllableCharacters[i] == controllable)
			{
				return;
			}
		}

		//expand array
		Controllable[] tempsArray = new Controllable[ccLength + 1];

		for(int i = 0; i < ccLength; i++)
		{
			tempsArray[i] = controllableCharacters[i];
		}

		controllableCharacters = tempsArray;

		//add Controllable
		controllableCharacters[ccLength] = controllable;

		controllable.isControlled = true;
	}

	/// <summary>
	/// Remove controllable objets on the liste of actifs controllables
	/// </summary>
	public static void RemoveControllableCharacters(Controllable controllable)
	{
		int ccLength = controllableCharacters.Length;
		int index = -1;

		//check is player is in
		for(int i = 0; i < ccLength; i++)
		{
			if(controllableCharacters[i] == controllable)
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
		Controllable[] tempsArray = new Controllable[ccLength - 1];

		//remove Controllable
		for(int i = 0; i < ccLength; i++)
		{
			if(i < index)
			{
				tempsArray[i] = controllableCharacters[i];
			}
			else if(i > index)
			{
				tempsArray[i - 1] = controllableCharacters[i - 1];
			}
		}

		controllableCharacters = tempsArray;

		controllable.isControlled = false;
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

	private static Controllable controllable;
	private static int playerID;

	void Update()
	{
		if(PauseManager.IsPause)
		{
			return;
		}

		//parcour la liste des joueurs
		for(int i = 0; i < controllableCharacters.Length; i++)
		{
			controllable = controllableCharacters[i];

			if(!controllable)
			{
				RemoveControllableCharacters(controllable);
				continue;
			}
			else if(!controllable.isControlled)
			{
				RemoveControllableCharacters(controllable);
				continue;
			}

			playerID = controllable.playerID;

			//ActionA
			if(Input.GetButtonDown("ActionA_" + playerID))
			{
				controllable.UseActionA_Press();
			}

			if(Input.GetButtonUp("ActionA_" + playerID))
			{
				controllable.UseActionA_Release();
			}

			//ActionB
			if(Input.GetButtonDown("ActionB_" + playerID))
			{
				controllable.UseActionB_Press();
			}

			if(Input.GetButtonUp("ActionB_" + playerID))
			{
				controllable.UseActionB_Release();
			}

			//ActionX
			if(Input.GetButtonDown("ActionX_" + playerID))
			{
				controllable.UseActionX_Press();
			}

			if(Input.GetButtonUp("ActionX_" + playerID))
			{
				controllable.UseActionX_Release();
			}

			//ActionY
			if(Input.GetButtonDown("ActionY_" + playerID))
			{
				controllable.UseActionY_Press();
			}

			if(Input.GetButtonUp("ActionY_" + playerID))
			{
				controllable.UseActionY_Release();
			}

			//Up
			if(upWasPress[i])
			{
				upDoubleTimer[i] -= Time.deltaTime;

				if(Input.GetAxisRaw("DPadY_" + playerID) == 1)
				{
					controllable.UpDouble();
					upWasPress[i] = false;
				}
				else if(upDoubleTimer[i] <= 0)
				{
					controllable.Up();
					upWasPress[i] = false;
				}
			}
			else
			{
				upWasPress[i] = Input.GetAxisRaw("DPadY_" + playerID) == 1;
				upDoubleTimer[i] = upDoubleTime;
			}

			//Down
			if(downWasPress[i])
			{
				downDoubleTimer[i] -= Time.deltaTime;

				if(Input.GetAxisRaw("DPadY_" + playerID) == -1)
				{
					controllable.DownDouble();
					downWasPress[i] = false;
				}
				else if(downDoubleTimer[i] <= 0)
				{
					controllable.Down();
					downWasPress[i] = false;
				}
			}
			else
			{
				downWasPress[i] = Input.GetAxisRaw("DPadY_" + playerID) == -1;
				downDoubleTimer[i] = downDoubleTime;
			}

			//Right
			if(rightWasPress[i])
			{
				rightDoubleTimer[i] -= Time.deltaTime;

				if(Input.GetAxisRaw("DPadX_" + playerID) == 1)
				{
					controllable.RightDouble();
					rightWasPress[i] = false;
				}
				else if(rightDoubleTimer[i] <= 0)
				{
					controllable.Right();
					rightWasPress[i] = false;
				}
			}
			else
			{
				rightWasPress[i] = Input.GetAxisRaw("DPadX_" + playerID) == 1;
				rightDoubleTimer[i] = rightDoubleTime;
			}

			//Left
			if(leftWasPress[i])
			{
				leftDoubleTimer[i] -= Time.deltaTime;

				if(Input.GetAxisRaw("DPadX_" + playerID) == -1)
				{
					controllable.LeftDouble();
					leftWasPress[i] = false;
				}
				else if(leftDoubleTimer[i] <= 0)
				{
					controllable.Left();
					leftWasPress[i] = false;
				}
			}
			else
			{
				leftWasPress[i] = Input.GetAxisRaw("DPadX_" + playerID) == -1;
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