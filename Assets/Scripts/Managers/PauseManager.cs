﻿using UnityEngine;

public class PauseManager : MonoBehaviour
{
	private static PauseManager instance;

	private PauseManager() { }

	public static PauseManager Instance
	{
		get
		{
			if(instance == null)
			{
				instance = new PauseManager();
			}
			return instance;
		}
	}

	[SerializeField]
	private CanvasGroup canvasGroup;

	//[SerializeField]
	//private EventSystem eventSystem;

	private static bool isPause = false;
	public static bool IsPause
	{
		get
		{
			return isPause;
		}

		set
		{
			if(isPause == value)
			{
				return;
			}

			if(value)
			{
				Cursor.visible = true;
				Time.timeScale = 0;

				if(instance.canvasGroup)
				{
					instance.canvasGroup.alpha = 1;
					instance.canvasGroup.gameObject.SetActive(true);
				}
			}
			else
			{
				Cursor.visible = false;
				Time.timeScale = 1;

				if(instance.canvasGroup)
				{
					instance.canvasGroup.alpha = 0;
					instance.canvasGroup.gameObject.SetActive(false);
				}
			}

			isPause = value;
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
	}

	void Update()
	{
		if(GameManager.State == GameManager.States.InGame)
		{
			if(Input.GetButtonDown("Pause"))
			{
				IsPause = !IsPause;
			}
		}
		else
		{
			IsPause = false;
		}
	}

}
