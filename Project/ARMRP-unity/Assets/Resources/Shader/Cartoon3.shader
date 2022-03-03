// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Coolnark/Cartoon3" {
	Properties{
		//_Color("Color", Color) = (1,1,1,1)
		// http://wiki.unity3d.com/index.php?title=Outlined_Diffuse_3

		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_SSSTex("SSS (RGB)", 2D) = "white" {}
		_ILMTex("ILM (RGB)", 2D) = "white" {}
		_SplashTex("splash (RGB)", 2D) = "white" {}
		_CutoffTex("溶解 (alpha)", 2D) = "white" {}
		_SplashU ("splash U",range(-0.5,1.5)) = 0
		_SplashWidth ("Model width",float) = 1

		_SSSGate ("冷暖色分界",range(-0.5,0.5)) = 0
		_HighLightGate ("高亮分界",range(1,3)) = 1
		_DarkLightGate ("深暗分界",range(-1,1)) = 0

		_HighLightEnhance ("高亮增强",range(0.25,2)) = 0.8


		_ShadowContrast("Shadow Contrast", Range(0, 20)) = 1
		_DarkenInnerLineColor("Darken Inner Line Color", Range(0, 1)) = 0.2

		_LightDirection("Light Direction", Vector) = (0,0.986,0.167)
		_HighLightDirection("High Light Direction", Vector) = (-0.73,0.352,-0.587)
		_CameraDirection("Camera Direction", Vector) = (0,0.3420201,-0.9396926)
		//_PointLightPosition("Point Light Position", Vector) = (0,0,1)
		_PointLightColor("Point Light Color", Color) = (0,0,0,0)
		_RimColor("Rim Light Color", Color) = (0,0,0,0)
		_RimPower ("Rim power",range(1,10)) = 2//边缘强度
		_RimGate ("Rim gate",range(0,1)) = 0.2//边缘光阀值，低于这个阀值的不再显示
		_Color("Vector Color", Color) = (1,1,1,1)
		_SrcBlend("Src Blend", int) = 1
		_DstBlend("Dst Blend", int) = 0
		_ZWrite("Z Write", int) = 1
		_ZOffset("Z Offset", float) = 0
		//_StencilComp("Stencil Comp", int) = 8
		//_StencilPass("Stencil Pass", int) = 0
		_ColorMask ("Color Mask", int) = 15

	    [Toggle(_ALPHABLEND_ON)]_alphablend_on("alpha blend", int) = 0
	    [Toggle(_ALPHABLENDWithHeight_ON)]_alphablendwithheight_on("alpha blend with height", int) = 0
	    [Toggle(_SPLASH_ON)]_splash_on("splash from left to right", int) = 0
	    [Toggle(_TURN_STONE)]_turn_stone("石化", int) = 0
	    [Toggle(_REAL_LIGHT)]_real_light("真实光照", int) = 0
	    [Toggle(_RIM_LIGHT)]_rim_light("边缘光效果", int) = 0
	    [Toggle(_CUTOFF_ON)]_cutoff_on("溶解效果", int) = 0
		_Cutoff ("cutoff",range(0,1)) = 0
		_GrayScale("GrayScale", Float) = 1
}


CGINCLUDE
#include "UnityCG.cginc"
	sampler2D _MainTex;
	sampler2D _SSSTex;
	sampler2D _ILMTex;
	sampler2D _SplashTex;
	sampler2D _CutoffTex;
	uniform float _SplashU;  // 横向纹理偏移
	uniform float _SplashWidth;  // 横向纹理偏移
	uniform float _SSSGate;  // 冷暖色分界
	uniform float _HighLightGate;  // 高亮分界
	uniform float _DarkLightGate;  // 深暗分界
	uniform float _HighLightEnhance;  // 高亮增强
	uniform float _Cutoff;  

	struct appdata {
		float4 vertex : POSITION;
		float3 normal : NORMAL;
		float2 texcoord : TEXCOORD0;
		float4 color : COLOR;
	};

	uniform float4 _MainTex_ST;
	uniform float _ShadowContrast;
	uniform float _DarkenInnerLineColor;
	uniform half3 _LightDirection;
	uniform half3 _HighLightDirection;
	uniform half3 _CameraDirection;
	//uniform half3 _PointLightPosition;
	uniform float4 _PointLightColor;
	uniform float4 _RimColor;
	uniform float _RimPower;
	uniform float _RimGate;
	uniform float4 _Color;
	uniform int _SrcBlend;
	uniform int _DstBlend;
	uniform int _ZWrite;
	uniform float _ZOffset;
	uniform	float _GrayScale;
	//uniform int _StencilComp;
	//uniform int _StencilPass;

ENDCG

SubShader 
{
	//Tags {"Queue" = "Geometry+100" }
	//CGPROGRAM
	//#pragma surface surfA Lambert

	////sampler2D _MainTex;
	//fixed4 _Color;

	//struct Input {
	//	float2 uv_MainTex;
	//};

	//void surfA(Input IN, inout SurfaceOutput o) {
	//	//fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
	//	half4 c2 = half4(1, 0, 1, 1);
	//	//return c2;
	//	// fixed4 c = (1, 0, 0, 1);
	//	o.Albedo = c2.rgb;
	//	o.Alpha = c2.a;
	//}
	//ENDCG

	// note that a vertex shader is specified here but its using the one above
		Tags {
		"Queue"="Geometry+10" 
		"RenderType" = "Opaque"
		}

	pass {
		Name "CelShading"
		cull Back
		ZWrite [_ZWrite]
		//stencil{
		//	Ref 2
		//	Comp [_StencilComp]
		//	Pass [_StencilPass]
		//}	

		ColorMask [_ColorMask]
		Blend [_SrcBlend] [_DstBlend]
		Offset [_ZOffset],[_ZOffset]
		CGPROGRAM
		#pragma vertex vertCel
		#pragma fragment fragCel
		#pragma shader_feature _ALPHABLEND_ON
		#pragma shader_feature _ALPHABLENDWithHeight_ON
		#pragma shader_feature _SPLASH_ON
		#pragma shader_feature _TURN_STONE
		#pragma shader_feature _REAL_LIGHT
		#pragma shader_feature _RIM_LIGHT
		#pragma shader_feature _CUTOFF_ON

		struct v2fCel {
			float4 pos : POSITION;
			float3 normal : NORMAL;
			float4 color : COLOR;
			float2 tex : TEXCOORD0;
			float2 tex1 : TEXCOORD1;
#if _SPLASH_ON
			float2 splashtex : TEXCOORD2;
#endif
			float4 pointLightColor : COLOR1;
			//float2 pointLightLuminosity : TEXCOORD1;
			//float3 pointLightDir : POSITION1;
		};

			v2fCel vertCel(appdata v) {
				// just make a copy of incoming vertex data but scaled according to normal direction
				v2fCel o;

				o.pos = UnityObjectToClipPos(v.vertex);
				//o.normal = float4(v.normal,1.0);

				o.normal = UnityObjectToWorldNormal(normalize(v.normal));// mul(v.normal, (float3x3)unity_WorldToObject);
				o.tex = v.texcoord;// TRANSFORM_TEX(v.texcoord, _MainTex);
#if _SPLASH_ON
				o.splashtex.x = v.vertex.z/_SplashWidth + _SplashU;
				o.splashtex.y = 0.5;
#endif
				o.color = v.color;

				//float3 V = _CameraDirection;//WorldSpaceViewDir(v.vertex);
				//o.color.a = saturate(dot(normalize(v.normal),(V)));
				//half dotR = saturate(dot(o.normal,(_CameraDirection)));
				//o.color.a = pow((1-dotR) ,_RimPower);
#if _RIM_LIGHT
				half dotRN = saturate(dot(o.normal,normalize(_CameraDirection)));
				o.tex1.x = pow((1-dotRN) ,_RimPower);
#endif


#if _ALPHABLENDWithHeight_ON
				float4 worldpos = mul(unity_ObjectToWorld, v.vertex);
				o.tex1.y = worldpos.y;
#endif
				float diff = max(0.2, dot(o.normal, _LightDirection));
				o.pointLightColor = _PointLightColor * diff;// _PointLightColor * diff * atten + _Color;
				return o;
			}

			half4 fragCel(v2fCel i) :COLOR { 
#if _CUTOFF_ON
				fixed4 cCutoff = tex2D(_CutoffTex, i.tex);
				clip(cCutoff.a - _Cutoff);
#endif
				half NdotL = dot(_LightDirection, i.normal);
				NdotL = NdotL*0.5 + 0.5;
#if _REAL_LIGHT
				return NdotL;
#endif
				fixed4 o;
				fixed4 c = tex2D(_MainTex, i.tex);

				fixed4 cSSS = tex2D(_SSSTex, i.tex);
				fixed4 cILM = tex2D(_ILMTex, i.tex);


				half3 BrightColor = c.rgb * _Color  + i.pointLightColor.rgb ;//+ rimColor;
				half3 ShadowColor = c.rgb * cSSS.rgb * _Color + i.pointLightColor.rgb;// + rimColor;
				float vertColor = (i.color.r - 0.5) * _ShadowContrast + 0.5;
				//float ShadowThreshold = cILM.g * vertColor;
				//ShadowThreshold = 1 - ShadowThreshold; // flip black / white


				//return vertColor;
				NdotL = NdotL - 1 + cILM.g * vertColor;
				//return NdotL - _SSSGate;
				//return NdotL;
				half SpecularSize =  1-cILM.b;
				//return SpecularSize;
				if (NdotL < _SSSGate) 
				{			
					if ( NdotL < - SpecularSize - _DarkLightGate && cILM.r < 0.5f) 
					{
						o.rgb = ShadowColor *(cILM.r + 0.5);
					}
					else
					{
						o.rgb = ShadowColor;
					}
					o.a = 0;
				}
				else
				{
					if (
					NdotL*_HighLightGate   > SpecularSize &&
					cILM.r >= 0.5f) 
					{
						o.rgb =BrightColor * (cILM.r + _HighLightEnhance);
					}
					else
					{
						o.rgb = BrightColor;// + c.a * 0.3;
					}
					o.a = c.a;
				}
		
				// add inner lines
				float clampedLineColor = cILM.a;
				clampedLineColor = max(clampedLineColor,_DarkenInnerLineColor);
				half3 InnerLineColor = half3(clampedLineColor, clampedLineColor, clampedLineColor);
				o.rgb = o.rgb * InnerLineColor;
#if _ALPHABLEND_ON
				o.a = _Color.a;
#endif
#if _ALPHABLENDWithHeight_ON
				half heightCoaf = max(0, 1 - i.tex1.y/3);
				o.a = heightCoaf;
#endif
#if _SPLASH_ON
				fixed4 cSplash = tex2D(_SplashTex, i.splashtex);
				o.rgb = cSplash.rgb * cSplash.a + o.rgb *(1 - cSplash.a);
#endif
#if _RIM_LIGHT
    			half NdotHighL = dot(_HighLightDirection, i.normal);
				//return NdotHighL;
	    		if (NdotHighL > 0)
					o.rgb = o.rgb + _RimColor*step(_RimGate,i.tex1.x);//*i.tex1.x;
#endif
#if _TURN_STONE
				half stone = o.r * 0.3 + o.g * 0.52 + o.b * 0.18;
				o.r = o.g = o.b = stone * _GrayScale;
#endif
				//return float4(ShadowColor ,1.0);
				//return float4(ShadowColor *(cILM.r + 0.5) ,1.0);
				//return float4(BrightColor ,1.0);
				//return float4(BrightColor * (cILM.r + 0.5),1.0);
				//half s = step(i.color.b,0.5);
				//return o*s + rimColor*(1-s); 	
				return o;  	

			}

		ENDCG

	}

	}

	FallBack "Diffuse"
}
