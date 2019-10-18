using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RedShift : MonoBehaviour
{
	Material RedshiftMat;

	Camera cam;

	public void Start(){
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		if (RedshiftMat == null) {
			RedshiftMat = new Material(Shader.Find("Hidden/RedShift"));
		}

		if(cam == null){
			cam = this.GetComponent<Camera>();
            cam.depthTextureMode = cam.depthTextureMode | DepthTextureMode.Depth;
		}

		Graphics.Blit(src, dst, RedshiftMat);
	}
}
