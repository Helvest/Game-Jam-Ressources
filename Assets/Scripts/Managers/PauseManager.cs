﻿using UnityEngine;

/// <summary>
/// Manage the pause system and canvas in pause state
/// </summary>
public class PauseManager : Singleton<PauseManager>
{
	/// <summary>
	/// CanvasGroup who appart in pause
	/// </summary>
	[SerializeField]
	private CanvasGroup canvasGroup;

	private static bool isPause = false;
	/// <summary>
	/// On/Off pause state
	/// </summary>
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
