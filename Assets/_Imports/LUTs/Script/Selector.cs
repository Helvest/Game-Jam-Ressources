using System.Collections;
using System.Collections.Generic;
using Helvest.SimpleLUT;
using UnityEngine;
using UnityEngine.UI;

public class Selector : MonoBehaviour
{
	[SerializeField]
	private RawImage RawImage;

	[SerializeField]
	private Text LUTname;

	[SerializeField]
	private Text LUTonOff;

	[SerializeField]
	private Texture2D[] LUTs;

	[SerializeField]
	private Texture[] Images;

	private SimpleLUT SimpleLUT;

	private void Awake()
	{
		SimpleLUT = GetComponent<SimpleLUT>();
	}

	[SerializeField]
	private Slider SliderAmount;
	public void ChangeAmount()
	{
		SimpleLUT.Amount = SliderAmount.value;
	}

	[SerializeField]
	private Slider SliderHue;
	public void ChangeHue()
	{
		SimpleLUT.Hue = SliderHue.value;
	}

	[SerializeField]
	private Slider SliderSliderSaturation;
	public void ChangeSaturation()
	{
		SimpleLUT.Saturation = SliderSliderSaturation.value;
	}

	[SerializeField]
	private Slider SliderBrightness;
	public void ChangeBrightness()
	{
		SimpleLUT.Brightness = SliderBrightness.value;
	}

	[SerializeField]
	private Slider SliderContrast;
	public void ChangeContrast()
	{
		SimpleLUT.Contrast = SliderContrast.value;
	}

	[SerializeField]
	private Slider SliderSharpness;
	public void ChangeSharpness()
	{
		SimpleLUT.Sharpness = SliderSharpness.value;
	}

	public void ActiveLUT()
	{
		SliderAmount.value = SimpleLUT.Amount = 1;
		SliderHue.value = SimpleLUT.Hue = 0;
		SliderSliderSaturation.value = SimpleLUT.Saturation = 0;
		SliderBrightness.value = SimpleLUT.Brightness = 0;
		SliderContrast.value = SimpleLUT.Contrast = 0;
		SliderSharpness.value = SimpleLUT.Sharpness = 0;
	}

	private int actualLUT = -1;
	public void ChangeLUT(bool isNext)
	{
		actualLUT += isNext ? 1 : -1;

		if (actualLUT == -1)
		{
			SimpleLUT.SetLutNull();
			LUTname.text = "";
			return;
		}
		else if (actualLUT == -2)
		{
			actualLUT = LUTs.Length - 1;
		}
		else if (actualLUT == LUTs.Length)
		{
			actualLUT = 0;
		}

		SimpleLUT.SetLut(LUTs[actualLUT].name);
		LUTname.text = LUTs[actualLUT].name;
	}

	private int actualImage = 0;
	public void ChangeImage(bool isNext)
	{
		actualImage += isNext ? 1 : -1;

		if (actualImage == -1)
		{
			actualImage = Images.Length - 1;
		}
		else if (actualImage == Images.Length)
		{
			actualImage = 0;
		}

		RawImage.texture = Images[actualImage];
	}

}
