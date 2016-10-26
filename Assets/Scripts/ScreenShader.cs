using UnityEngine;

[ExecuteInEditMode]
public class ScreenShader : MonoBehaviour
{
	public Material TransitionMaterial;

	void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		if(TransitionMaterial != null)
			Graphics.Blit(src, dst, TransitionMaterial);
	}
}
