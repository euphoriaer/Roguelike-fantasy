Shader "HeroGo/Effect/AlphaBlendEffectEx" {
    Properties {
        
		[Enum(Alpha Blend,10,Addtive,1)] _DestBlend("Dest Blend Mode", Float) = 1
        _TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
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
		[Space(5)]
		[MaterialToggle(_SoftDissolveMode)] _SoftDissolveMode("SoftDissolve CVS_Y", Float) = 0
		_EdgeWidth ("Edge Width", Range(0,1)) = 0
		_SoftEdge ("Soft Edge", Range(0.51, 1)) = 0.51

		[Header(__________________________________________________)]
		[Header(VertexAnimation)]
		[Toggle(_VertexAnimation)] _VertexAnimation("Vertex Animation Off/On", Float) = 0
		 _Wind("Wind",Vector) = (1,1,1,1)
	     _Speed("Wind Speed",Vector) = (0,0,0,0)
	     _WindEdgeFlutter("WindEdge Flutter", float) = 0.5
		 [Space(10)]
	     _WindEdgeFlutterFreqScale("WindEdge Flutter Freq",float) = 0.5
		
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "RenderPipeline" = "UniversalPipeline"
        }
        Pass {
			Blend SrcAlpha [_DestBlend]
			Cull Off
			ZWrite Off 


            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#pragma shader_feature_local_fragment _ UesZW
			#pragma shader_feature_local_fragment _ _MaskToggle
            #pragma shader_feature_local_fragment _ _FresnelToggle
			#pragma shader_feature_local_fragment _ _DissolveMode
			#pragma shader_feature_local_fragment _ _SoftDissolveMode
			#pragma shader_feature_local_fragment _ _Distortion_Dissolve
			#pragma shader_feature_local_fragment _ CVS_ZW
			#pragma shader_feature_local_fragment _ _VertexAnimation
            #include "Packages/com.unity.render-pipelines.universal/Shaders/UnlitInput.hlsl"


             sampler2D _MainTex;  
             half4 _MainTex_ST;
             half _MainSpeedV;
             half _MainSpeedU;
			 half4 _TintColor;
			 half _Intensity;
			 half _AlphaCutoff;
             half _Exp;
             half _FresnelInt;
			 half4 _FresnelColor;
            
             sampler2D _MaskTex;  
             half4 _MaskTex_ST;
			 half _MaskUVSpeedMode;
			 half _MaskSpeedU;
             half _MaskSpeedV;
			 half _MaskAlphaCutoff;
            
             half _DistortionSpeedU;
             half _DistortionSpeedV;
             sampler2D _DistortionTex;  
             half4 _DistortionTex_ST;
             half _DistortionIntensity;

             half _Dissolve;
             half _EdgeHighlight;
             half4 _EdgeColor;

			 half _EdgeWidth;
			 half _SoftEdge;  

			 half4 _Wind;
			 half _WindEdgeFlutter;
	         half _WindEdgeFlutterFreqScale;
	         half4	_Speed;
            
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

            float4 SmoothCurve( float4 x ) {
               return x * x *( 3.0 - 2.0 * x );
            }
            float4 TriangleWave( float4 x ) {
               return abs( frac( x + 0.5 ) * 2.0 - 1.0 );
            }
            float4 SmoothTriangleWave( float4 x ) {
               return SmoothCurve( TriangleWave( x ) );
            }
                
		 #ifdef _VertexAnimation
			inline float4 AnimateVertex2(float4 pos,float4 worldpos, float3 normal, float4 animParams,float4 wind,float2 time)
            {	
	            float fDetailAmp = 0.1f;
	            float fBranchAmp = 0.3f;
	
	            // Phases (object, vertex, branch)
	            float fObjPhase = dot(worldpos.xyz, 1);
	            float fBranchPhase = fObjPhase + animParams.x;
	            float fVtxPhase = dot(pos.xyz, animParams.y + fBranchPhase);
	            // x is used for edges; y is used for branches
	            //旗子本身的摆动用了y,也就是植物的茎,旗子边上的摆动用了x,也就是叶子的边
	            float2 vWavesIn = time  + float2(fVtxPhase, fBranchPhase );
	
	            //将两个频率映射成四个
	            float4 vWaves = (frac( vWavesIn.xxyy * float4(1.975, 0.793, 0.375, 0.193) ) * 2.0 - 1.0);
	            //使用了三角波,就是为了不用sin,cos为了一点效率
	            vWaves = SmoothTriangleWave( vWaves );
	            //在做一些游戏效果的时候经常需要用到正弦波,它平滑又有周期性,但是sin的计算开销略大.作为优化,在GPU Gem上看到一个用三次函数平滑三角波得到近似正弦波的方法不错
	            
                //将两个频率合并，为了看上去没有规律
	            float2 vWavesSum = vWaves.xz + vWaves.yw;
	            // Edge (xz) and branch bending (y)
	            //融合的部分,animParams.y叶子的权重从顶点色的绿色获得,fDetailAmp振幅, normal法线叶子是按法线方向飘动的
	            float3 bend = animParams.y * fDetailAmp * normal.xyz;
	            bend.y = animParams.w * fBranchAmp;
	            pos.xyz += ((vWavesSum.xyx * bend) + (wind.xyz * vWavesSum.y * animParams.w)) * wind.w; 
	            pos.xyz =pos.xyz + animParams.z * wind.xyz;
	            //pos.x = sin(pos.x+_Time.x *3.14) * animParams.w;
	            return pos;
            }
         #endif

            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
				o.uv1 = v.texcoord1;
                o.vertexColor = v.vertexColor;
                o.normalDir = TransformObjectToWorldNormal(v.normal);
				o.posWorld = mul(unity_ObjectToWorld, v.vertex);

          #ifdef _VertexAnimation
				float4	wind;
				float  bendingFact	= v.vertexColor.a;
				_Wind.x = sin((_Time.x * 30 + o.posWorld.x) * _Speed.x) * 0.06 * _Speed.x;
				wind.xyz = mul((float3x3)unity_WorldToObject, _Wind.xyz);
                wind.w	= _Wind.w  * bendingFact;
				float4 windParams = float4(0, _WindEdgeFlutter, bendingFact.xx);
				float windTime = _Time.y * float2(_WindEdgeFlutterFreqScale, 1);
				float4 mdlPos = AnimateVertex2(v.vertex, o.posWorld, v.normal, windParams, wind, windTime);				
				o.pos = TransformObjectToHClip( mdlPos );
          #else
		        o.pos = TransformObjectToHClip( v.vertex );
          #endif
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
                float4 baseColor = _TintColor * i.vertexColor;
                float3 mainColor = ((baseColor.rgb*_MainTex_var.rgb)) * _Intensity;

//Fresnel--------------------------
                #ifdef _FresnelToggle
					float faceSign = ( facing >= 0 ? 1 : -1 );
					i.normalDir = normalize(i.normalDir) * faceSign;
					float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                    float strenth = pow(1.0-saturate(dot(viewDirection, -abs(i.normalDir))),_Exp);
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
					half finalAlpha = saturate(alpha * MaskAlpha);
				#else
					float alpha = saturate(((_MainTex_var.a * baseColor.a) - _AlphaCutoff) * _Intensity + (fresnelAlpha * fresnelIntensity));
					half finalAlpha = alpha;
				#endif

				#ifdef _Distortion_Dissolve

				#ifdef _SoftDissolveMode
				float softWidth = i.uv1.g;
				#else
				float softWidth = _EdgeWidth;
				#endif


				half4 col = half4(mainColor.rgb + edgeColor + frescolor.rgb, finalAlpha - smoothstep(_SoftEdge, (1.0 - _SoftEdge), saturate(((_DistortionTex_var.a + 1.0) + (saturate((softWidth)) * -2)))));
				col.a = saturate(col.a);
				return col;
                #else
                    half4 col = half4(mainColor.rgb + edgeColor + frescolor.rgb, finalAlpha);
                    col.a = saturate(col.a);
				    return col;
			    #endif
			
			}
            ENDHLSL
        }
        
        Pass
        {
            Name "DepthOnly"
            Tags{"LightMode" = "DepthOnly"}
            ZWrite On
            ColorMask 0
            HLSLPROGRAM
            #pragma only_renderers gles gles3 glcore d3d11
            #pragma vertex DepthOnlyVertex
            #pragma fragment DepthOnlyFragment
            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing
            #include "Packages/com.unity.render-pipelines.universal/Shaders/UnlitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/DepthOnlyPass.hlsl"
            ENDHLSL
        }
    }
}
