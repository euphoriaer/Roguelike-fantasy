Shader "Custom/UIScroll" {
    Properties
    {
        [MainTexture] _MainTex("Albedo", 2D) = "white" {}
        [MainColor] _Color("基础颜色", Color) = (1,1,1,1) 
        _MainSpeedU ("    Main Speed U", Float ) = 0
        _MainSpeedV ("    Main Speed V", Float ) = 0
        [Toggle(_Twinkle)]_Twinkle("闪烁", int) = 0
        _TwinkleSpeed("闪烁速度", Float) = 6
    }
        SubShader
        {
            
            Pass
            {
                Tags{"RenderPipeline" = "UniversalPipeline"
                    "RenderType" = "Transparent"
                    "IgnoreProjector" = "True"
                    "Queue" = "Transparent"}
                LOD 300
                Blend SrcAlpha OneMinusSrcAlpha
                ZWrite Off
                Cull Back
                Name "Unlit"
                HLSLPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma multi_compile_instancing
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                #pragma shader_feature_local_fragment _Twinkle

                half4 _Color;
                TEXTURE2D(_MainTex);
                SAMPLER(sampler_MainTex);
                half4 _MainTex_ST;
                half _MainSpeedV;
                half _MainSpeedU;
                half _TwinkleSpeed;

                struct Attributes
                {
                    float4 positionOS       : POSITION;
                    float2 uv               : TEXCOORD0;
                    UNITY_VERTEX_INPUT_INSTANCE_ID
                };

                struct Varyings
                {
                    float4 vertex : POSITION;
                    float2 uv        : TEXCOORD0;
                    float3 positionWS               : TEXCOORD1;
                    UNITY_VERTEX_INPUT_INSTANCE_ID
                    UNITY_VERTEX_OUTPUT_STEREO
                };

                Varyings vert(Attributes input)
                {
                    Varyings output = (Varyings)0;
                    UNITY_SETUP_INSTANCE_ID(input);
                    UNITY_TRANSFER_INSTANCE_ID(input, output);
                    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
                    output.uv = TRANSFORM_TEX(input.uv, _MainTex) + frac(float2 (_MainSpeedU, _MainSpeedV) * _Time.y); ;
                    output.vertex = TransformObjectToHClip(input.positionOS);
                    output.positionWS = mul(unity_ObjectToWorld, input.positionOS);
                    return output;
                }


                half4 frag(Varyings input) : SV_Target
                {
                    UNITY_SETUP_INSTANCE_ID(input);
                    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
                    half4 c = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);
                    #if _Twinkle 
                            c *= 2 * cos(_Time.y * _TwinkleSpeed) + 0.4;
                     #endif
                    c *= _Color;
                    return c;
                }

                ENDHLSL
            }
        }
            FallBack "Hidden/Universal Render Pipeline/FallbackError"
}
