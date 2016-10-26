using UnityEngine;

public static class Easing
{
	//linearStep = %

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

	public static Vector2 EaseVector2(Vector2 from, Vector2 to, float linearStep, EasingPart part = EasingPart.NoEase, EasingType type = EasingType.Linear)
	{
		return Vector2.LerpUnclamped(from, to, Ease(linearStep, part, type));
	}

	public static Vector3 EaseVector3(Vector3 from, Vector3 to, float linearStep, EasingPart part = EasingPart.NoEase, EasingType type = EasingType.Linear)
	{
		return Vector3.LerpUnclamped(from, to, Ease(linearStep, part, type));
	}

	public static Vector4 EaseVector4(Vector4 from, Vector4 to, float linearStep, EasingPart part = EasingPart.NoEase, EasingType type = EasingType.Linear)
	{
		return Vector4.LerpUnclamped(from, to, Ease(linearStep, part, type));
	}

	public static Color32 EaseColor32(Color32 from, Color32 to, float linearStep, EasingPart part = EasingPart.NoEase, EasingType type = EasingType.Linear)
	{
		return Color32.LerpUnclamped(from, to, Ease(linearStep, part, type));
	}

	public static Color EaseColor(Color from, Color to, float linearStep, EasingPart part = EasingPart.NoEase, EasingType type = EasingType.Linear)
	{
		return Color.LerpUnclamped(from, to, Ease(linearStep, part, type));
	}

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

	public static float EaseInOut(float linearStep, EasingType easeInType, EasingType easeOutType)
	{
		return linearStep < 0.5 ? EaseInOut(linearStep, easeInType) : EaseInOut(linearStep, easeOutType);
	}

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
			return Mathf.Sin(s * MathHelper.HalfPi - MathHelper.HalfPi) + 1;
		}
		public static float EaseOut(float s)
		{
			return Mathf.Sin(s * MathHelper.HalfPi);
		}
		public static float EaseInOut(float s)
		{
			return (Mathf.Sin(s * MathHelper.Pi - MathHelper.HalfPi) + 1) / 2;
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
			s = p / (2 * Mathf.PI) * Mathf.Asin(1);
			return -(Mathf.Pow(2, 10 * (t -= 1)) * Mathf.Sin((t - s) * (2 * Mathf.PI) / p));
		}

		public static float EaseOut(float t)
		{
			if(t == 0 || t == 1)
				return t;

			p = .3f;
			s = p / (2 * Mathf.PI) * Mathf.Asin(1);
			return Mathf.Pow(2, -10 * t) * Mathf.Sin((t - s) * (2 * Mathf.PI) / p) + 1;
		}

		public static float EaseInOut(float t)
		{
			if(t == 0 || t == 1)
				return t;

			p = .3f;
			s = p / (2 * Mathf.PI) * Mathf.Asin(1);

			if(t < .5f)
			{
				t *= 2;
				return -.5f * (Mathf.Pow(2, 10 * (t -= 1)) * Mathf.Sin((t - s) * (2 * Mathf.PI) / p));
			}
			t -= 0.5f;
			t *= 2;
			return .5f * Mathf.Pow(2, -10 * t) * Mathf.Sin((t - s) * (2 * Mathf.PI) / p) + 1;
		}
	}
}

public enum EasingPart
{
	NoEase,
	EaseIn,
	EaseOut,
	EaseInOut
}

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

public static class MathHelper
{
	public const float Pi = Mathf.PI;
	public const float HalfPi = (Mathf.PI / 2);

	public static float Lerp(float from, float to, float step)
	{
		return ((to - from) * step + from);
	}
}
