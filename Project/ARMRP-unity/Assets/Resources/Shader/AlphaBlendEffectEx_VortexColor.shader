Shader "HeroGo/Effect/AlphaBlendEffectEx_VortexColor" {
    Properties {
        
		[Enum(Alpha Blend,10,Addtive,1)] _DestBlend("Dest Blend Mode", Float) = 1
        _DyeColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex("Main Tex", 2D) = "white" {}
	    _Intensity("    Intensity", Float) = 1
		[MaterialToggle(UesZW)] _UesZW("    CVS_ZW", Float) = 0
        _MainSpeedU ("    Main Speed U", Float ) = 0
        _MainSpeedV ("    Main Speed V", Float ) = 0
		_AlphaCutoff("    Alpha Cutoff", Range(0, 1)) = 0
		[Header(__________________________________________________)]

		[Header(Fresnel)]
        [Toggle(_FresnelToggle)] _FresnelToggle ("Ues Fresnel Off/On", Float ) = 0
        _Exp ("    Exp", Float ) = 1
        _FresnelInt ("    Fresnel Int", Float ) = 1
		_FresnelColor("    Fres Col", Color) = (1,1,1,1)
		[Header(__________________________________________________)]

		[Header(Mask)]
		[MaterialToggle(_MaskToggle)] _MaskToggle("Ues Mask Off/On", Float) = 0
		_MaskTex("    Mask Tex", 2D) = "white" {}
		[MaterialToggle(CVS_ZW)] _MaskUVSpeedMode("    CVS_ZW", Float) = 0
        _MaskSpeedU ("    MaskSpeedU", Float ) = 0
        _MaskSpeedV ("    MaskSpeedV", Float ) = 0
		_MaskAlphaCutoff("    Mask Alpha Cutoff", Range(0, 1)) = 0
		[Header(__________________________________________________)]

		[Header(Distortion)]
		[MaterialToggle(_Distortion_Dissolve)] _DD("Ues Distortion & Dissolve Off/On", Float) = 0
		_DistortionTex("    Distortion Tex", 2D) = "white" {}
        _DistortionIntensity ("    Distortion Intensity", Range(-2, 2)) = 0
        _DistortionSpeedU ("    Distortion Speed U", Range(-2, 2)) = 0
        _DistortionSpeedV ("    Distortion Speed V", Range(-2, 2)) = 0
		[Header(_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ )]

		[Header(Dissolve(UES DistortionTex))]
        _Dissolve ("    Dissolve", Range(0, 5)) = 0
		[MaterialToggle(_DissolveMode)] _DissolveMode("    CVS_X", Float) = 0
        _EdgeHighlight ("    Edge Highlight", Float ) = 0
        _EdgeColor ("    Edge Color", Color) = (1,1,1,1)
		
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
			Blend SrcAlpha [_DestBlend]
			Cull Off
			ZWrite Off 



            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
			#pragma multi_compile _ UesZW
			#pragma multi_compile _ _MaskToggle
            #pragma multi_compile _ _FresnelToggle
			#pragma multi_compile _ _DissolveMode
			#pragma multi_compile _ _Distortion_Dissolve
			#pragma multi_compile _ CVS_ZW
            #pragma target 2.0


             sampler2D _MainTex;  float4 _MainTex_ST;
             float _MainSpeedV;
             float _MainSpeedU;
			 float4 _DyeColor;
			 float _Intensity;
			 float _AlphaCutoff;
             float _Exp;
             float _FresnelInt;
			 float4 _FresnelColor;
            
             sampler2D _MaskTex;  float4 _MaskTex_ST;
			 fixed _MaskUVSpeedMode;
			 float _MaskSpeedU;
             float _MaskSpeedV;
			 float _MaskAlphaCutoff;
            
             float _DistortionSpeedU;
             float _DistortionSpeedV;
             sampler2D _DistortionTex;  float4 _DistortionTex_ST;
             float _DistortionIntensity;

             float _Dissolve;
             float _EdgeHighlight;
             float4 _EdgeColor;
            
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
				float4 uv1 : TEXCOORD1;
                float4 posWorld : TEXCOORD2;
                float3 normalDir : TEXCOORD3;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
				o.uv1 = v.texcoord1;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);

				o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                
                
//Distortion----------------------
			#ifdef _Distortion_Dissolve
				float2 uvDistortion = (float2((_Time.g*_DistortionSpeedU),(_Time.g*_DistortionSpeedV))+i.uv0);
                float4 _DistortionTex_var = tex2D(_DistortionTex,TRANSFORM_TEX(uvDistortion, _DistortionTex));
				#ifdef _DissolveMode
				float _DissolveMode_var = i.uv1.r;
				#else
				float _DissolveMode_var = _Dissolve;
				#endif
                float dissolveHigh = step(_DissolveMode_var,(_DistortionTex_var.a+_EdgeHighlight));
                float edge = (dissolveHigh-step(_DissolveMode_var,_DistortionTex_var.a));
                float3 edgeColor = edge*_EdgeColor.rgb;
				clip(((dissolveHigh+edge)*_EdgeColor.a) - 0.5);

				float2 _distUV = float2(_DistortionTex_var.r, _DistortionTex_var.g)*_DistortionIntensity;

			#else
				float3 edgeColor = 0;
				float2 _distUV = 0;
			#endif

//MainTex--------------------------
				#ifdef UesZW
				float2 mainUVspeed = i.uv1.zw + i.uv0;
				#else
				float2 mainUVspeed = float2(_MainSpeedU * _Time.g,_MainSpeedV * _Time.g) + i.uv0;
				#endif
				float2 uvMain = (_distUV + mainUVspeed);
				float4 _MainTex_var = tex2D(_MainTex, TRANSFORM_TEX(uvMain, _MainTex));

//BaseAndMain----------------------
                float4 baseColor = _DyeColor * i.vertexColor;
                float3 mainColor = ((baseColor.rgb*_MainTex_var.rgb)) * _Intensity;

//Fresnel--------------------------
                #ifdef _FresnelToggle
					float faceSign = ( facing >= 0 ? 1 : -1 );
					i.normalDir = normalize(i.normalDir) * faceSign;
					float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                    float strenth = pow(1.0-saturate(dot(viewDirection, i.normalDir)),_Exp);
					float fresnelAlpha = baseColor.a * strenth;
                    float fresnelIntensity = _FresnelInt;
					float4 frescolor = strenth * _FresnelColor * fresnelIntensity;
                #else
                    float strenth = 0;
					float fresnelAlpha = 0;
                    float fresnelIntensity = 1;
					float4 frescolor = 0;
                #endif

//uesMaskTex--------------------------	
				#ifdef _MaskToggle
					#ifdef CVS_ZW
					float2 uvMask = i.uv1.zw + i.uv0;
					#else
					float2 uvMask = float2((_MaskSpeedU*_Time.g), (_Time.g*_MaskSpeedV)) + i.uv0;
					#endif

					float4 _MaskTex_var = tex2D(_MaskTex,TRANSFORM_TEX(uvMask, _MaskTex));
				
					float MaskAlpha = saturate((_MaskTex_var.a - _MaskAlphaCutoff) * _Intensity);
					//end uesMask

					float alpha = saturate(((_MainTex_var.a * baseColor.a) - _AlphaCutoff) * _Intensity + (fresnelAlpha * fresnelIntensity));
					fixed finalAlpha = saturate(alpha * MaskAlpha);
				#else
					float alpha = saturate(((_MainTex_var.a * baseColor.a) - _AlphaCutoff) * _Intensity + (fresnelAlpha * fresnelIntensity));
					fixed finalAlpha = alpha;
				#endif

				return fixed4(mainColor.rgb + edgeColor + frescolor.rgb, finalAlpha);
            }
            ENDCG
        }
    }
}
