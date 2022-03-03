// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Coolnark/TextureEdge" {
	Properties
    {
		//[HideInInspector]_Color("Albedo (RGB)", 2D) = "black" {}
		_Color("Outline Color", Color) = (0, 0, 0, 0)
        _Outline("Outline width", Range(.0, 0.1)) = 0.1
        //_DissolveColor("DissolveColor", Color) = (0, 0, 0, 0)
        //_DissolveEdgeColor("DissolveEdgeColor", Color) = (0, 0, 0, 0)
	    [Toggle(_DiSSOLVE)]_Dissolve("溶解", int) = 0
        _DissolveMap("DissolveMap", 2D) = "white" {}
        _DissolveClip("溶解值", Range(0,1)) = 0.5 
        _ColorFactor("溶解色强度", Range(0,1)) = 0.825
		_ShelterColor("ShelterColor", Color) = (0.8588, 0.84705, 0.84705)
		/*
        _RimPower ("Rim power",range(1,10)) = 2//边缘强度
		_RimIntensity ("RimIntensity", Float) = 2
		*/
    }
    
    SubShader
    {
        Pass
        {
            Name "Unlit"
            Tags {"RenderType" = "Opaque" "IgnoreProjector" = "True" "LightMode" = "UniversalForwardOnly" }
             LOD 100
            Cull Front
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // -------------------------------------
            // Unity defined keywords
            #pragma multi_compile_instancing
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
		    #pragma multi_compile _ _DiSSOLVE
            half _Outline;
            
            /*
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_ST;
            */
            half3 _Color;
            TEXTURE2D(_DissolveMap);
            SAMPLER(sampler_DissolveMap);
            half4 _DissolveMap_ST;
            half _DissolveClip;
            half _ColorFactor;
            half _DissolveEdge;
            
            //half3 _DissolveColor;
            //half3 _DissolveEdgeColor;
	
            struct Attributes
            {
                float4 positionOS       : POSITION;
                float2 uv               : TEXCOORD0;
		        float3 normal : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float2 uv        : TEXCOORD0;
                float4 vertex : POSITION;

                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };

            Varyings vert(Attributes input)
            {
                Varyings output = (Varyings)0;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
                output.uv = TRANSFORM_TEX(input.uv, _DissolveMap);
                output.vertex = TransformObjectToHClip(input.positionOS +input.normal * _Outline);
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
                half2 uv = input.uv;
                //half4 c = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
                #if _DiSSOLVE 
                    half4 cutoutSource = SAMPLE_TEXTURE2D(_DissolveMap, sampler_DissolveMap, uv);
                    clip(cutoutSource.r - _DissolveClip * 1.001);
                    float percentage = _DissolveClip / cutoutSource.r;
                    cutoutSource.a = FastSign(percentage -  _ColorFactor);
                    return half4(_Color, cutoutSource.a);
                #else
                    return half4(_Color, 1);
                #endif 
                //return half4(c.rgb, cutoutSource.a);
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
            #pragma target 2.0

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
    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}