//dithering & bit depth effects
//by ompu co (Sam Blye) 2018
//please visit https://www.patreon.com/ompuco

using UnityEngine;
[ExecuteInEditMode][ImageEffectAllowedInSceneView]
public class DitherEffect : MonoBehaviour
{

    public Material ditherMat;
	public float ditherPower = 32.0f;

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
		ditherMat.SetFloat("dPower", ditherPower);
        Graphics.Blit(src, dest, ditherMat);
    }
}
