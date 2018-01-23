using System;
using UnityEngine;

namespace Helvest.SimpleLUT
{
	[ExecuteInEditMode]
	public class SimpleLUT : MonoBehaviour
	{
		//[Tooltip("Shader to use for the lookup.")]
		//private Shader Shader;

		[SerializeField,
		Tooltip("Texture to use for the lookup. Make sure it has read/write enabled and mipmaps disabled. The height must equal the square root of the width.")]
		private Texture2D LookupTexture;
		private Texture2D previousTexture;

		[SerializeField]
		private Texture2D[] LookupTextures = new Texture2D[0];

		[SerializeField,
		Range(0f, 1f),
		Tooltip("Lerp between original (0) and corrected color (1)")]
		private float amount = 1.0f;
		public float Amount
		{
			get
			{
				return amount;
			}

			set
			{
				amount = value;
				material.SetFloat("_Amount", amount);
			}
		}

		[SerializeField,
		Tooltip("Tint color, applied to the final pixel")]
		private Color tintColor = Color.white;
		public Color TintColor
		{
			get
			{
				return tintColor;
			}

			set
			{
				tintColor = value;
				material.SetColor("_TintColor", tintColor);
			}
		}

		[SerializeField,
		Range(0.0f, 360.0f),
		Tooltip("Hue")]
		private float hue = 0.0f;
		public float Hue
		{
			get
			{
				return hue;
			}

			set
			{
				hue = value;
				material.SetFloat("_Hue", hue);
			}
		}

		[SerializeField,
		Range(-1f, 1f),
		Tooltip("Saturation")]
		private float saturation = 0.0f;
		public float Saturation
		{
			get
			{
				return saturation;
			}

			set
			{
				saturation = value;
				material.SetFloat("_Saturation", saturation + 1.0f);
			}
		}

		[SerializeField,
		Range(-1f, 1f),
		Tooltip("Brightness")]
		private float brightness = 0.0f;
		public float Brightness
		{
			get
			{
				return brightness;
			}

			set
			{
				brightness = value;
				material.SetFloat("_Brightness", brightness + 1.0f);
			}
		}

		[SerializeField,
		Range(-1f, 1f),
		Tooltip("Contrast")]
		private float contrast = 0.0f;
		public float Contrast
		{
			get
			{
				return contrast;
			}

			set
			{
				contrast = value;
				material.SetFloat("_Contrast", contrast + 1.0f);
			}
		}

		[SerializeField,
		Range(0f, 1f),
		Tooltip("Sharpness")]
		private float sharpness = 0.0f;
		public float Sharpness
		{
			get
			{
				return sharpness;
			}

			set
			{
				sharpness = value;
				float actualSharpness = sharpness * 4.0f * 0.2f;
				material.SetFloat("_SharpnessCenterMultiplier", 1.0f + 4.0f * actualSharpness);
				material.SetFloat("_SharpnessEdgeMultiplier", actualSharpness);
			}
		}

		private Material material;
		private Texture3D converted3DLut = null;
		private int lutSize;
		private int colorSpace;

		private string previousLutName = string.Empty;

		public void SetLutNull()
		{
			if (previousTexture)
			{
				previousTexture = LookupTexture = null;
				previousLutName = string.Empty;

				Convert(LookupTexture);
			}
		}

		public void SetLut(string lutName)
		{
			lutName = lutName.ToLower();

			if (previousLutName == lutName)
			{
				return;
			}

			Texture2D newLookupTexture = null;

			int Length = LookupTextures.Length;
			for (int i = 0; i < Length; i++)
			{
				if (LookupTextures[i].name.ToLower() == lutName)
				{
					newLookupTexture = LookupTextures[i];
				}
			}

			if (newLookupTexture)
			{
				previousTexture = LookupTexture = newLookupTexture;
				previousLutName = LookupTexture.name.ToLower();

				Convert(LookupTexture);
			}
		}

		private void CreateMaterial()
		{
			if (material != null)
			{
				return;
			}

			Shader shader = Shader.Find("Helvest/SimpleLUT");

			if (!shader)
			{
				Debug.LogError("Shader Helvest/SimpleLUT not find");
				return;
			}

			material = new Material(shader);
			material.hideFlags = HideFlags.DontSave;

			material.SetFloat("_Amount", amount);
			material.SetColor("_TintColor", tintColor);
			material.SetFloat("_Hue", hue);
			material.SetFloat("_Saturation", saturation + 1.0f);
			material.SetFloat("_Brightness", brightness + 1.0f);
			material.SetFloat("_Contrast", contrast + 1.0f);
			float actualSharpness = sharpness * 4.0f * 0.2f;
			material.SetFloat("_SharpnessCenterMultiplier", 1.0f + 4.0f * actualSharpness);
			material.SetFloat("_SharpnessEdgeMultiplier", actualSharpness);
		}

		private void OnEnable()
		{
			if (GetComponent<Camera>() == null)
			{
				Debug.LogError("This script must be attached to a Camera");
			}

			colorSpace = QualitySettings.activeColorSpace == ColorSpace.Linear ? 1 : 0;

			mainTexID = Shader.PropertyToID("_MainTex");
			imageWidthFactorID = Shader.PropertyToID("_ImageWidthFactor");
			imageHeightFactorID = Shader.PropertyToID("_ImageHeightFactor");

			CreateMaterial();

			if (LookupTexture != previousTexture)
			{
				previousTexture = LookupTexture;
				Convert(LookupTexture);
			}
		}

		private void OnDestroy()
		{
			if (converted3DLut != null)
			{
				DestroyImmediate(converted3DLut);
			}
			converted3DLut = null;
		}

		private bool ValidDimensions(Texture2D tex2d)
		{
			if (tex2d == null)
			{
				return false;
			}

			int h = tex2d.height;
			if (h != Mathf.FloorToInt(Mathf.Sqrt(tex2d.width)))
			{
				return false;
			}
			return true;
		}

		private bool Convert(Texture2D lookupTexture)
		{
			if (converted3DLut != null)
			{
				DestroyImmediate(converted3DLut);
			}

			if (lookupTexture == null)
			{
				int dim = 16;
				Color[] newC = new Color[dim * dim * dim];
				float oneOverDim = 1.0f / (1.0f * dim - 1.0f);

				for (int i = 0; i < dim; i++)
				{
					for (int j = 0; j < dim; j++)
					{
						for (int k = 0; k < dim; k++)
						{
							newC[i + (j * dim) + (k * dim * dim)] = new Color((i * 1.0f) * oneOverDim, (j * 1.0f) * oneOverDim, (k * 1.0f) * oneOverDim, 1.0f);
						}
					}
				}

				converted3DLut = new Texture3D(dim, dim, dim, TextureFormat.ARGB32, false);
				converted3DLut.SetPixels(newC);
				converted3DLut.Apply();
				lutSize = converted3DLut.width;
				converted3DLut.wrapMode = TextureWrapMode.Clamp;
			}
			else
			{
				if (lookupTexture.mipmapCount > 1)
				{
					Debug.LogError("Lookup texture must not have mipmaps");
					return false;
				}

				try
				{
					int dim = lookupTexture.width * lookupTexture.height;
					dim = lookupTexture.height;

					if (!ValidDimensions(lookupTexture))
					{
						Debug.LogError("Lookup texture dimensions must be a power of two. The height must equal the square root of the width.");
						return false;
					}

					Color[] c = lookupTexture.GetPixels();
					Color[] newC = new Color[c.Length];

					int i, j, k;
					for (i = 0; i < dim; i++)
					{
						for (j = 0; j < dim; j++)
						{
							for (k = 0; k < dim; k++)
							{
								newC[i + (j * dim) + (k * dim * dim)] = c[k * dim + i + (dim - j - 1) * dim * dim];
							}
						}
					}

					converted3DLut = new Texture3D(dim, dim, dim, TextureFormat.ARGB32, false);
					converted3DLut.SetPixels(newC);
					converted3DLut.Apply();
					lutSize = converted3DLut.width;
					converted3DLut.wrapMode = TextureWrapMode.Clamp;
				}
				catch (Exception ex)
				{
					Debug.LogError("Unable to convert texture to LUT texture, make sure it is read/write. Error: " + ex);
				}
			}

			material.SetTexture("_ClutTex", converted3DLut);
			material.SetFloat("_Scale", (lutSize - 1) / (1.0f * lutSize));
			material.SetFloat("_Offset", 1.0f / (2.0f * lutSize));

			return true;
		}

		private Vector4 sourceWidth = Vector4.zero;
		private Vector4 sourceHeight = Vector4.zero;

		private int mainTexID, imageWidthFactorID, imageHeightFactorID;

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (converted3DLut == null || material == null)
			{
				Graphics.Blit(source, destination);
				return;
			}

			/*material.SetTexture(mainTexID, source);
			sourceWidth.x = 1.0f / source.width;
			material.SetVector(imageWidthFactorID, sourceWidth);
			sourceHeight.y = 1.0f / source.height;
			material.SetVector(imageHeightFactorID, sourceHeight);*/

			material.SetTexture(mainTexID, source);
			sourceWidth.x = 1.0f / source.width;
			material.SetVector(imageWidthFactorID, sourceWidth);
			sourceHeight.y = 1.0f / source.height;
			material.SetVector(imageHeightFactorID, sourceHeight);

			Graphics.Blit(source, destination, material, colorSpace);
		}
	}
}
