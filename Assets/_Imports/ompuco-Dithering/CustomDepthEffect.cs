//dithering & bit depth effects
//by ompu co (Sam Blye) 2018
//please visit https://www.patreon.com/ompuco

using UnityEngine;
[ExecuteInEditMode][ImageEffectAllowedInSceneView]
public class CustomDepthEffect : MonoBehaviour
{

    public Material precisionMat;
	[Range(1,256)]
	public int colorDepth = 1;

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {	
		precisionMat.SetInt("cDepth",colorDepth);
        Graphics.Blit(src, dest, precisionMat);
    }
}
