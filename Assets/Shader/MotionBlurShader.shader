// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "MotionBlurShader" 
{
	Properties
	{
		_MainTex("Main Texture(RGB)", 2D) = "white" {}
		_IterationNumber("IterationNumber", Int)=16
		//_Value("Intensity",)
		//_Value2
		//_Value3 
	}

	SubShader
	{	
		Pass
		{
			//设置深度测试模式:渲染所有像素.等同于关闭透明度测试（AlphaTest Off）
			ZTest Always

			CGPROGRAM


			//编译指令:告知编译器顶点和片段着色函数的名称
			#pragma vertex vert
			#pragma fragment frag

			//包含辅助CG头文件
			#include "UnityCG.cginc"

			//外部变量的声明
			uniform sampler2D _MainTex;
			uniform float _Value;
			uniform int _IterationNumber;

			//顶点输入结构
			struct vertexInput
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			//顶点输出结构
			struct vertexOutput
			{
				half2 texcoord : TEXCOORD0;
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
			};


			//--------------------------------【顶点着色函数】-----------------------------
			// 输入：顶点输入结构体
			// 输出：顶点输出结构体
			//---------------------------------------------------------------------------------
			vertexOutput vert(vertexInput Input)
			{
				//【1】声明一个输出结构对象
				vertexOutput Output;

				//【2】填充此输出结构
				//输出的顶点位置为模型视图投影矩阵乘以顶点位置，也就是将三维空间中的坐标投影到了二维窗口
				Output.vertex = UnityObjectToClipPos(Input.vertex);
				//输出的纹理坐标也就是输入的纹理坐标
				Output.texcoord = Input.texcoord;
				//输出的颜色值也就是输入的颜色值
				Output.color = Input.color;

				//【3】返回此输出结构对象
				return Output;
			}

			//--------------------------------【片段着色函数】-----------------------------
			// 输入：顶点输出结构体
			// 输出：float4型的颜色值
			//---------------------------------------------------------------------------------
			float4 frag(vertexOutput i) : COLOR
			{
				//【1】设置中心坐标
				float2 center = float2(0.5, 0.5);
				//【2】获取纹理坐标的x，y坐标值
				float2 uv = i.texcoord.xy;
				//【3】纹理坐标按照中心位置进行一个偏移
				uv -= center;
				//【4】初始化一个颜色值
				float4 color = float4(0.0, 0.0, 0.0, 0.0);
				//【5】将Value乘以一个系数
				_Value *= 0.085;
				//【6】设置坐标缩放比例的值
				float scale = 1;

				for (int j = 1; j < _IterationNumber; ++j)
				{
					color += tex2D(_MainTex, uv * scale + center);
					scale = 1 + (float(j * _Value));
				}

				for (int j = 1; j < _IterationNumber; ++j)
				{
					color += tex2D(_MainTex, uv * scale + center);
					scale = 1 - (float(j * _Value));
				}

				color /= 2*(float)_IterationNumber;
				//float t = saturate(length(uv)*3);  
				//return lerp(tex2D(_MainTex,uv+center), color, t);
				return  color;
			}

			ENDCG
		}
	}
}