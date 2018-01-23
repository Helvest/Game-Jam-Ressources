using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player3D_FPS : PlayerScript
{
	public Rigidbody _rigidbody { get; protected set; }

	[SerializeField]
	private float speedMove = 9;

	[SerializeField]
	private LayerMask interactableLayerMask;

	[SerializeField]
	private Transform Head;

	private List<ObjectListe> inventaire = new List<ObjectListe>();

	protected override void Awake()
	{
		base.Awake();
		_rigidbody = GetComponent<Rigidbody>();

		if (!Head)
		{
			Head = _transform.Find("Head");
		}
		if (!Head)
		{
			Debug.Log("No Head Transform Find !");
		}

	}

	private Vector3 direction;

	void FixedUpdate()
	{
		if (!isControlled || PauseManager.Instance.IsPause)
		{
			return;
		}

		//Game over
		/*	if(_rigidbody.position.y <= -10)
			{
				_rigidbody.velocity = Vector3.zero;
				LevelManager.Respawn();
				return;
			}*/

		direction = Input.GetAxis("Horizontal_" + playerID) * LevelManager.Instance.mainCameraTrans.right +
					Input.GetAxis("Vertical_" + playerID) * LevelManager.Instance.mainCameraTrans.forward;
		direction.y = 0;

		_rigidbody.velocity = direction.normalized * speedMove;

		Head.rotation = LevelManager.Instance.mainCameraTrans.rotation;
	}

	private void Update()
	{
		Aim();
	}

	[SerializeField]
	private int interactDistance = 20;
	private RaycastHit hit = new RaycastHit();

	private Interactable interactable;

	[SerializeField]
	private Animator Icon;

	[SerializeField]
	private GameObject Cross;

	private InteractableEnum lastType;

	private bool checkInteractable;

	private void Aim()
	{
		Physics.Linecast(LevelManager.Instance.mainCameraTrans.position, LevelManager.Instance.mainCameraTrans.position + LevelManager.Instance.mainCameraTrans.forward * interactDistance, out hit, interactableLayerMask);

		checkInteractable = false;

		if (hit.collider)
		{
			interactable = hit.collider.GetComponent<Interactable>();

			if (!interactable)
			{
				interactable = hit.collider.GetComponentInParent<Interactable>();
			}

			if (interactable)
			{
				checkInteractable = true;

				lastType = interactable.type;
				Cross.SetActive(false);

				//HUD affiche interact
				switch (lastType)
				{
					case InteractableEnum.Prendre:
						//Icon.Play("Icon_Prendre");
						break;
					case InteractableEnum.Interagir:
						/*	Interactable scriptInteract = interactable;
							bool check = false;
							if(scriptInteract.needObject)
							{
								foreach(ObjectListe item in scriptInteract.checkObjectListe)
								{
									if(inventaire.Contains(item))
									{
										check = true;
									}
								}
							}
							else
							{
								check = true;
							}

							if(!check)
							{
								Cross.SetActive(true);
							}

							Icon.Play("Icon_Interagir");
							*/
						break;
					case InteractableEnum.Poser:
						/*	InteractPoser scriptPoser = interactable;

							bool checkPoser;

							if(scriptPoser.objectInOrder)
							{
								checkPoser = true;
								for(int i = 0; i < scriptPoser.checkObjectListe.Length; i++)
								{
									if(scriptPoser.checkObjectListe[i] != ObjectListe.None)
									{
										if(!inventaire.Contains(scriptPoser.checkObjectListe[i]))
										{
											checkPoser = false;
										}
										break;
									}
								}
							}
							else
							{
								checkPoser = false;
								foreach(ObjectListe item in scriptPoser.checkObjectListe)
								{
									if(inventaire.Contains(item))
									{
										checkPoser = true;
										break;
									}
								}
							}

							if(!checkPoser)
							{
								Cross.SetActive(true);
							}

							Icon.Play("Icon_Poser");*/
						break;
				}


				//Input
				if (Input.GetButtonDown("ActionA_" + playerID))
				{
					interactable.Interagir();
				}
			}
		}

		if (checkInteractable && lastType != InteractableEnum.Nothing)
		{
			lastType = InteractableEnum.Nothing;
			//Icon.Play("Icon_Default");
			//Cross.SetActive(false);
		}

		if (updateInventory)
		{
			updateInventory = false;
			//HUDManager.Instance.UpdateHUD(inventaire);
		}
	}

	private bool updateInventory = false;

	public void Prendre(ObjectListe nameObject)
	{
		if (!inventaire.Contains(nameObject))
		{
			inventaire.Add(nameObject);
		}
		updateInventory = true;
		lastType = InteractableEnum.Nothing;
	}

	public bool Poser(ObjectListe nameObject)
	{
		bool test = inventaire.Contains(nameObject);

		if (test)
		{
			inventaire.Remove(nameObject);
		}
		updateInventory = true;
		lastType = InteractableEnum.Nothing;

		return test;
	}

	public bool Interagir(ObjectListe nameObject)
	{
		updateInventory = true;
		lastType = InteractableEnum.Nothing;

		return inventaire.Contains(nameObject);
	}

	/*
		public override void UseActionA_Press()
		{
		}

		public override void UseActionB_Press()
		{
		}

		public override void UseActionX_Press()
		{
		}

		public override void UseActionY_Press()
		{
		}
	*/

#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		//Head = transform.FindChild("Head");
		Gizmos.color = Color.red;
		if (LevelManager.Instance.mainCameraTrans)
		{
			Gizmos.DrawLine(LevelManager.Instance.mainCameraTrans.position, LevelManager.Instance.mainCameraTrans.position + LevelManager.Instance.mainCameraTrans.forward * interactDistance);
		}
	}
#endif

}

