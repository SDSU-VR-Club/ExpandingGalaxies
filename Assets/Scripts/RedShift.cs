using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RedShift : MonoBehaviour
{
	[SerializeField]
	Material DepthMat, RedshiftMat;

	Camera cam;

	[SerializeField]
	RenderTexture last;

	private void OnRenderImage(RenderTexture src, RenderTexture dst)
	{

		RenderTexture depth = RenderTexture.GetTemporary(src.descriptor);

		if(last == null){
			last = new RenderTexture(src.descriptor);
		}

		if (DepthMat == null) {
			DepthMat = new Material(Shader.Find("Custom/Depth"));
		}

		if (RedshiftMat == null) {
			RedshiftMat = new Material(Shader.Find("Custom/RedShift"));
		}

		if(cam == null){
			cam = this.GetComponent<Camera>();
      cam.depthTextureMode = cam.depthTextureMode | DepthTextureMode.Depth;
		}

		//Kinda inefficient to do it this way, but until I find a better way this is how it's done
		Graphics.Blit(src, depth, DepthMat);
		Graphics.Blit(depth, dst);
		RedshiftMat.SetTexture("_CurrentDepthTexture", depth);
		RedshiftMat.SetTexture("_LastDepthTexture", last);

		Graphics.Blit(src, dst, RedshiftMat);

		Graphics.CopyTexture(depth, last);
		depth.Release();
	}
}
