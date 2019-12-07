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

		if (RedshiftMat == null) {
			RedshiftMat = new Material(Shader.Find("Custom/RedShift"));
		}

		if(cam == null){
			cam = this.GetComponent<Camera>();
      		cam.depthTextureMode = cam.depthTextureMode | DepthTextureMode.Depth;
		}

		Graphics.Blit(src, dst, RedshiftMat);
	}
}
