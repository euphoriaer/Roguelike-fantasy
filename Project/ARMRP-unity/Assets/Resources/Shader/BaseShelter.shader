
Shader "Custom/BaseShelter" {
   Properties{
		
		_Color("Outline Color", Color) = (0, 0, 0, 0)
        _Alpha("alpha", Range(0,1)) = 1
    }

    SubShader 
    {
		Tags {"Queue" = "Transparent" "RenderType" = "Transparent" "ShaderModel"="4.5"}
		LOD 500
	    Pass {
	    Tags {"LightMode" = "UniversalForward" }
		Cull Back
		//Blend DstColor OneMinusSrcAlpha   
		//Blend DstColor OneMinusDstColor    
		Blend DstColor OneMinusDstColor
		BlendOp Add
		HLSLPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #include "Packages/com.unity.render-pipelines.universal/Shaders/UnlitInput.hlsl"

        half4 _Color;
        half _Alpha;
        struct Attributes {
            float4 vertex : POSITION;
            UNITY_VERTEX_INPUT_INSTANCE_ID
        };
                
        struct Varyings {
            float4 pos : POSITION;
            UNITY_VERTEX_INPUT_INSTANCE_ID
            UNITY_VERTEX_OUTPUT_STEREO
        };
                
        Varyings vert(Attributes input) 
        {
            Varyings output = (Varyings)0;
            UNITY_SETUP_INSTANCE_ID(input);
            UNITY_TRANSFER_INSTANCE_ID(input, output);
            UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
            output.pos = TransformObjectToHClip(input.vertex);
            return output;
        }
    
        half4 frag(Varyings input) :COLOR 
        { 
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
                half4 c = _Color;
                c.a = _Alpha;
                return c;	
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
