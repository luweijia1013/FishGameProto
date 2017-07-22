using UnityEngine;
using System.Collections;

[ExecuteInEditMode]

public class MotionBlurEffect : MonoBehaviour
{

	#region Variables
	public Shader CurShader;//着色器实例
	private Vector4 ScreenResolution;//屏幕分辨率
	private Material CurMaterial;//当前的材质

	[Range(5, 50)]
	public float IterationNumber = 15;
	[Range(-0.5f, 0.5f)]
	public float Intensity = 0.125f;


	#endregion


	//-------------------------材质的get&set----------------------------
	#region MaterialGetAndSet
	Material material
	{
		get
		{
			if (CurMaterial == null)
			{
				CurMaterial = new Material(CurShader);
				CurMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return CurMaterial;
		}
	}
	#endregion

	void Start()
	{

	}

	//-------------------------------------【OnRenderImage()函数】------------------------------------  
	// 说明：此函数在当完成所有渲染图片后被调用，用来渲染图片后期效果
	//--------------------------------------------------------------------------------------------------------
	void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
	{
		if (CurShader != null)
		{
			//设置Shader中的外部变量
			material.SetFloat("_IterationNumber", IterationNumber);
			material.SetFloat("_Value", Intensity);
			material.SetVector("_ScreenResolution", new Vector4(sourceTexture.width, sourceTexture.height, 0.0f, 0.0f));

			//拷贝源纹理到目标渲染纹理，加上我们的材质效果
			Graphics.Blit(sourceTexture, destTexture, material);
		}
		//着色器实例为空，直接拷贝屏幕上的效果。此情况下是没有实现屏幕特效的
		else
		{
			//直接拷贝源纹理到目标渲染纹理
			Graphics.Blit(sourceTexture, destTexture);
		}

	}
		

	void Update()
	{

		//找到对应的Shader文件
		#if UNITY_EDITOR
		if (Application.isPlaying != true)
		{
			CurShader = Shader.Find("MotionBlurShader");

		}
		#endif
	}


	//-----------------------------------------【OnDisable()函数】---------------------------------------  
	// 说明：当对象变为不可用或非激活状态时此函数便被调用  
	//--------------------------------------------------------------------------------------------------------
	void OnDisable()
	{
		if (CurMaterial)
		{
			DestroyImmediate(CurMaterial);
		}
	}
}