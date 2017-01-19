using UnityEngine;

/// <summary>
/// Class for easings calculus
/// </summary>
public static class Easing
{
	private const float PI = Mathf.PI;
	private const float HalfPI = (PI / 2);

	/// <summary>
	/// Calculate the Ease from a pourcent
	/// </summary>
	/// <param name="linearStep">Pourcent on the ease</param>
	/// <param name="part">Easing Part</param>
	/// <param name="type">Easing Type</param>
	/// <returns>A easing float</returns>
	public static float Ease(float linearStep, EasingPart part = EasingPart.NoEase, EasingType type = EasingType.Linear)
	{
		switch(part)
		{
			case EasingPart.NoEase:
				return linearStep;
			case EasingPart.EaseIn:
				return EaseIn(linearStep, type);
			case EasingPart.EaseOut:
				return EaseOut(linearStep, type);
			case EasingPart.EaseInOut:
				return EaseInOut(linearStep, type);
			default:
				return linearStep;
		}
	}

	/// <summary>
	/// Calculate the Ease position between two Vector2
	/// </summary>
	/// <param name="linearStep">Pourcent on the ease</param>
	/// <param name="part">Easing Part</param>
	/// <param name="type">Easing Type</param>
	/// <returns>A easing Vector2</returns>
	public static Vector2 EaseVector2(Vector2 from, Vector2 to, float linearStep, EasingPart part = EasingPart.NoEase, EasingType type = EasingType.Linear)
	{
		return Vector2.LerpUnclamped(from, to, Ease(linearStep, part, type));
	}

	/// <summary>
	/// Calculate the Ease position between two Vector3
	/// </summary>
	/// <param name="linearStep">Pourcent on the ease</param>
	/// <param name="part">Easing Part</param>
	/// <param name="type">Easing Type</param>
	/// <returns>A easing Vector3</returns>
	public static Vector3 EaseVector3(Vector3 from, Vector3 to, float linearStep, EasingPart part = EasingPart.NoEase, EasingType type = EasingType.Linear)
	{
		return Vector3.LerpUnclamped(from, to, Ease(linearStep, part, type));
	}

	/// <summary>
	/// Calculate the Ease position between two Vector4
	/// </summary>
	/// <param name="linearStep">Pourcent on the ease</param>
	/// <param name="part">Easing Part</param>
	/// <param name="type">Easing Type</param>
	/// <returns>A easing Vector4</returns>
	public static Vector4 EaseVector4(Vector4 from, Vector4 to, float linearStep, EasingPart part = EasingPart.NoEase, EasingType type = EasingType.Linear)
	{
		return Vector4.LerpUnclamped(from, to, Ease(linearStep, part, type));
	}

	/// <summary>
	/// Calculate the Ease position between two Color32
	/// </summary>
	/// <param name="linearStep">Pourcent on the ease</param>
	/// <param name="part">Easing Part</param>
	/// <param name="type">Easing Type</param>
	/// <returns>A easing Color32</returns>
	public static Color32 EaseColor32(Color32 from, Color32 to, float linearStep, EasingPart part = EasingPart.NoEase, EasingType type = EasingType.Linear)
	{
		return Color32.LerpUnclamped(from, to, Ease(linearStep, part, type));
	}

	/// <summary>
	/// Calculate the Ease position between two Colors
	/// </summary>
	/// <param name="linearStep">Pourcent on the ease</param>
	/// <param name="part">Easing Part</param>
	/// <param name="type">Easing Type</param>
	/// <returns>A easing Color</returns>
	public static Color EaseColor(Color from, Color to, float linearStep, EasingPart part = EasingPart.NoEase, EasingType type = EasingType.Linear)
	{
		return Color.LerpUnclamped(from, to, Ease(linearStep, part, type));
	}

	/// <summary>
	/// Calculate a Ease In from a pourcent
	/// </summary>
	/// <param name="linearStep">Pourcent on the ease</param>
	/// <param name="type">Easing Type</param>
	public static float EaseIn(float linearStep, EasingType type)
	{
		switch(type)
		{
			case EasingType.Step:
				return linearStep < .5f ? 0 : 1;
			case EasingType.Linear:
				return linearStep;
			case EasingType.Sine:
				return Sine.EaseIn(linearStep);
			case EasingType.Quadratic:
				return Power.EaseIn(linearStep, 2);
			case EasingType.Cubic:
				return Power.EaseIn(linearStep, 3);
			case EasingType.Quartic:
				return Power.EaseIn(linearStep, 4);
			case EasingType.Quintic:
				return Power.EaseIn(linearStep, 5);
			case EasingType.Elastic:
				return Elastic.EaseIn(linearStep);
			default:
				return linearStep;
		}
	}

	/// <summary>
	/// Calculate a Ease Out from a pourcent
	/// </summary>
	/// <param name="linearStep">Pourcent on the ease</param>
	/// <param name="type">Easing Type</param>
	public static float EaseOut(float linearStep, EasingType type)
	{
		switch(type)
		{
			case EasingType.Step:
				return linearStep < .5f ? 0 : 1;
			case EasingType.Linear:
				return linearStep;
			case EasingType.Sine:
				return Sine.EaseOut(linearStep);
			case EasingType.Quadratic:
				return Power.EaseOut(linearStep, 2);
			case EasingType.Cubic:
				return Power.EaseOut(linearStep, 3);
			case EasingType.Quartic:
				return Power.EaseOut(linearStep, 4);
			case EasingType.Quintic:
				return Power.EaseOut(linearStep, 5);
			case EasingType.Elastic:
				return Elastic.EaseOut(linearStep);
			default:
				return linearStep;
		}

	}

	/// <summary>
	/// Calculate a Ease InOut from a pourcent
	/// </summary>
	/// <param name="linearStep">Pourcent on the ease</param>
	/// <param name="easeInType">Easing Type for the In</param>
	/// <param name="easeOutType">Easing Type for the Out</param>
	public static float EaseInOut(float linearStep, EasingType easeInType, EasingType easeOutType)
	{
		return linearStep < 0.5 ? EaseInOut(linearStep, easeInType) : EaseInOut(linearStep, easeOutType);
	}

	/// <summary>
	/// Calculate a Ease InOut from a pourcent
	/// </summary>
	/// <param name="linearStep">Pourcent on the ease</param>
	/// <param name="type">Easing Type</param>
	public static float EaseInOut(float linearStep, EasingType type)
	{
		switch(type)
		{
			case EasingType.Step:
				return linearStep < .5f ? 0 : 1;
			case EasingType.Linear:
				return linearStep;
			case EasingType.Sine:
				return Sine.EaseInOut(linearStep);
			case EasingType.Quadratic:
				return Power.EaseInOut(linearStep, 2);
			case EasingType.Cubic:
				return Power.EaseInOut(linearStep, 3);
			case EasingType.Quartic:
				return Power.EaseInOut(linearStep, 4);
			case EasingType.Quintic:
				return Power.EaseInOut(linearStep, 5);
			case EasingType.Elastic:
				return Elastic.EaseInOut(linearStep);
			default:
				return linearStep;
		}
	}

	static class Sine
	{
		public static float EaseIn(float s)
		{
			return Mathf.Sin(s * HalfPI - HalfPI) + 1;
		}
		public static float EaseOut(float s)
		{
			return Mathf.Sin(s * HalfPI);
		}
		public static float EaseInOut(float s)
		{
			return (Mathf.Sin(s * PI - HalfPI) + 1) / 2;
		}
	}

	static class Power
	{
		public static float EaseIn(float s, int power)
		{
			return Mathf.Pow(s, power);
		}
		public static float EaseOut(float s, int power)
		{
			var sign = power % 2 == 0 ? -1 : 1;
			return (sign * (Mathf.Pow(s - 1, power) + sign));
		}
		public static float EaseInOut(float s, int power)
		{
			s *= 2;
			if(s < 1)
				return EaseIn(s, power) / 2;
			var sign = power % 2 == 0 ? -1 : 1;
			return (float)(sign / 2.0 * (Mathf.Pow(s - 2, power) + sign * 2));
		}
	}

	static class Elastic
	{
		private static float s, p;

		public static float EaseIn(float t)
		{
			if(t == 0 || t == 1)
				return t;

			p = .3f;
			s = p / (2 * PI) * Mathf.Asin(1);
			return -(Mathf.Pow(2, 10 * (t -= 1)) * Mathf.Sin((t - s) * (2 * PI) / p));
		}

		public static float EaseOut(float t)
		{
			if(t == 0 || t == 1)
				return t;

			p = .3f;
			s = p / (2 * PI) * Mathf.Asin(1);
			return Mathf.Pow(2, -10 * t) * Mathf.Sin((t - s) * (2 * PI) / p) + 1;
		}

		public static float EaseInOut(float t)
		{
			if(t == 0 || t == 1)
				return t;

			p = .3f;
			s = p / (2 * PI) * Mathf.Asin(1);

			if(t < .5f)
			{
				t *= 2;
				return -.5f * (Mathf.Pow(2, 10 * (t -= 1)) * Mathf.Sin((t - s) * (2 * PI) / p));
			}
			t -= 0.5f;
			t *= 2;
			return .5f * Mathf.Pow(2, -10 * t) * Mathf.Sin((t - s) * (2 * PI) / p) + 1;
		}
	}
}

/// <summary>
/// Liste of possible part with ease
/// </summary>
public enum EasingPart
{
	NoEase,
	EaseIn,
	EaseOut,
	EaseInOut
}

/// <summary>
/// Liste of possible type of ease
/// </summary>
public enum EasingType
{
	Step,
	Linear,
	Sine,
	Quadratic,
	Cubic,
	Quartic,
	Quintic,
	Elastic
}
