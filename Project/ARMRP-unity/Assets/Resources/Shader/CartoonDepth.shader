// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Coolnark/CartoonDepth" {
	Properties{
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_SSSTex("SSS (RGB)", 2D) = "white" {}
		/*
		_ILMTex("ILM (RGB)", 2D) = "white" {}
		_SplashTex("splash (RGB)", 2D) = "white" {}
		_SplashU ("splash U",range(-0.5,1.5)) = 0
		_SplashWidth ("Model width",float) = 1
		*/
		
	    [Toggle(_DiSSOLVE)]_Dissolve("溶解", int) = 0
		_DissolveEdgeColor("溶解色", Color) = (1, 1, 1, 1)//溶解色
        _DissolveMap("DissolveMap", 2D) = "white" {} 
        _DissolveClip("溶解值", Range(0,1)) = 0.5 
        _ColorFactor("溶解色强度", Range(0,1)) = 0.825
        
		_SSSGate ("冷暖色分界",range(-0.5,0.5)) = 0
		_HighLightGate ("高亮分界",range(1,3)) = 1
		_DarkLightGate ("深暗分界",range(-1,1)) = 0
		_HighLightEnhance ("高亮增强",range(0.15,2)) = 0.6
		_ShadowContrast("Shadow Contrast", Range(0, 20)) = 1
		_DarkenInnerLineColor("Darken Inner Line Color", Range(0, 1)) = 0.2
		_LightDirection("Light Direction", Vector) = (0,0.986,0.167)
		_CameraDirection("Camera Direction", Vector) = (0,0.3420201,-0.9396926)
		_PointLightColor("Point Light Color", Color) = (0,0,0,0)
		_RimColor("Rim Light Color", Color) = (0,0,0,0)
		_RimPower ("Rim power",range(1,10)) = 2//边缘强度
		_RimGate ("Rim gate",range(0,1)) = 0.2//边缘光阀值，低于这个阀值的不再显示
		_Color("Vector Color", Color) = (1,1,1,1)
		_SrcBlend("Src Blend", int) = 1
		_DstBlend("Dst Blend", int) = 0
		_ZWrite("Z Write", int) = 1
		_ZOffset("Z Offset", float) = 0
		_ColorMask ("Color Mask", int) = 15
	    
	    //[Toggle(_ALPHABLEND_ON)]_alphablend_on("alpha blend", int) = 0
	    //[Toggle(_ALPHABLENDWithHeight_ON)]_alphablendwithheight_on("alpha blend with height", int) = 0
	    //[Toggle(_SPLASH_ON)]_splash_on("splash from left to right", int) = 0
	    
	    [Toggle(_RIM_LIGHT)]_rim_light("边缘光效果", int) = 0
	    [Toggle(_TURN_STONE)]_turn_stone("石化", int) = 0
	    _StatueDegree("石化值", range(0, 1)) = 0.5
		_GrayScale("石化强度", Float) = 1 //石化强度
    }

    SubShader 
    {
		Tags {"Queue"="Geometry+10" "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "ShaderModel"="4.5"}
		LOD 500
	    Pass {
		Name "CelShading"
		Cull Back
		ZWrite [_ZWrite]
		Blend [_SrcBlend] [_DstBlend]
		Offset [_ZOffset],[_ZOffset]
		HLSLPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #include "Packages/com.unity.render-pipelines.universal/Shaders/UnlitInput.hlsl"
		//#pragma shader_feature_local_fragment _ALPHABLEND_ON
		//#pragma shader_feature_local_fragment _ALPHABLENDWithHeight_ON
		//#pragma shader_feature_local_fragment _SPLASH_ON
		#pragma shader_feature _TURN_STONE
		#pragma multi_compile _ _DiSSOLVE

        sampler2D _MainTex;
        sampler2D _SSSTex;
        
        //sampler2D _ILMTex;
        //sampler2D _SplashTex;
        /*
        uniform float _SplashU;  // 横向纹理偏移
        uniform float _SplashWidth;  // 横向纹理偏移
        */
        uniform float _SSSGate;  // 冷暖色分界
        uniform float _HighLightGate;  // 高亮分界
        uniform float _DarkLightGate;  // 深暗分界
        uniform float _HighLightEnhance;  // 高亮增强
    
        uniform float4 _MainTex_ST;
        uniform float _ShadowContrast;
        uniform float _DarkenInnerLineColor;
        uniform half3 _LightDirection;
        uniform half3 _CameraDirection;
        uniform float4 _PointLightColor;
        uniform float4 _RimColor;
        uniform float _RimPower;
        uniform float _RimGate;
        uniform float4 _Color;
        uniform int _SrcBlend;
        uniform int _DstBlend;
        uniform int _ZWrite;
        uniform float _ZOffset;
        uniform float _GrayScale;
        half _StatueDegree;
        half _Brightness;
        
        sampler2D _DissolveMap;
        half3 _DissolveEdgeColor;
        half _DissolveClip;
        half _ColorFactor;
            
        struct Attributes {
            float4 vertex : POSITION;
            float3 normal : NORMAL;
            float2 texcoord : TEXCOORD0;
            float4 color : COLOR;
            UNITY_VERTEX_INPUT_INSTANCE_ID
        };
                
        struct Varyings {
            float4 pos : POSITION;
            float3 normal : NORMAL;
            float4 color : COLOR;
            float2 tex : TEXCOORD0;
            float2 tex1 : TEXCOORD1;
            
            /*
            #if _SPLASH_ON
                float2 splashtex : TEXCOORD2;
            #endif
            */
            float4 pointLightColor : COLOR1;
            UNITY_VERTEX_INPUT_INSTANCE_ID
            UNITY_VERTEX_OUTPUT_STEREO
        };
                
         inline float DecodeFloatRGBA( float4 enc )
        {
            float4 kDecodeDot = float4(1.0, 1/255.0, 1/65025.0, 1/16581375.0);
            return dot( enc, kDecodeDot );
        }

        Varyings vert(Attributes input) 
        {
            Varyings output = (Varyings)0;
            UNITY_SETUP_INSTANCE_ID(input);
            UNITY_TRANSFER_INSTANCE_ID(input, output);
            UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
            output.pos = TransformObjectToHClip(input.vertex);
            output.normal = TransformObjectToWorldNormal(normalize(input.normal));// mul(input.normal, (float3x3)unity_WorldToObject);
            output.tex = input.texcoord;// TRANSFORM_TEX(input.texcoord, _MainTex);
            /*
            #if _SPLASH_ON
                output.splashtex.x = input.vertex.z/_SplashWidth + _SplashU;
                output.splashtex.y = 0.5;
            #endif
            */
            output.color = input.color;
            half dotRN = saturate(dot(output.normal, normalize(_CameraDirection)));
            output.tex1.x = pow((1-dotRN) ,_RimPower);
            /*
            #if _ALPHABLENDWithHeight_ON
                float4 worldpos = mul(unity_ObjectToWorld, input.vertex);
                output.tex1.y = worldpos.y;
            #endif
            */
            half diff = max(0.2, dot(output.normal, _LightDirection));
            output.pointLightColor = _PointLightColor * diff;// _PointLightColor * diff * atten + _Color;
            return output;
        }
    
        half4 frag(Varyings input) :COLOR 
        { 
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
				half NdotL = dot(_LightDirection, input.normal);
				NdotL = NdotL*0.5 + 0.5;
				half4 o;
				half4 c = tex2D(_MainTex, input.tex);
				half4 cSSS = tex2D(_SSSTex, input.tex);
				half4 cILM = cSSS;
				half3 BrightColor = c.rgb * _Color  + input.pointLightColor.rgb ;//+ rimColor;
				half3 ShadowColor = c.rgb * cSSS.rgb * _Color + input.pointLightColor.rgb;// + rimColor;
				half vertColor = (input.color.r - 0.5) * _ShadowContrast + 0.5;
				NdotL = NdotL - 1 + cILM.g * vertColor;
				half SpecularSize =  1- cILM.b;
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
					if (NdotL*_HighLightGate   > SpecularSize && cILM.r >= 0.5f) 
					{
						o.rgb =BrightColor * (cILM.r + _HighLightEnhance);
					}
					else
					{
						o.rgb = BrightColor;// + c.a * 0.3;
					}
					o.a = c.a;
				}
				half clampedLineColor = cILM.a;
				clampedLineColor = max(clampedLineColor,_DarkenInnerLineColor);
				o.rgb = o.rgb * clampedLineColor;
				/*
                #if _ALPHABLEND_ON
                    o.a = _Color.a;
                #endif
                #if _ALPHABLENDWithHeight_ON
                    half heightCoaf = max(0, 1 - input.tex1.y/3);
                    o.a = heightCoaf;
                #endif
                #if _SPLASH_ON
                    fixed4 cSplash = tex2D(_SplashTex, input.splashtex);
                    o.rgb = cSplash.rgb * cSplash.a + o.rgb *(1 - cSplash.a);
                #endif
                #if _RIM_LIGHT
                    //o = lerp(o,_RimColor,step(_RimGate,input.color.b)*input.color.b);
                #endif
                */
                half  val = step(_RimGate,input.tex1.x) * input.tex1.x;
                o.rgb = o.rgb + _RimColor * val;
                #if _TURN_STONE
                    half stone = o.r * 0.3 + o.g * 0.587 + o.b * 0.114;
                    o.rgb = lerp(o.rgb, stone *_GrayScale, _StatueDegree) ;
                #endif
                #if _DiSSOLVE 
                    half4 cutoutSource = tex2D(_DissolveMap,  input.tex);
                    clip(cutoutSource.r - _DissolveClip*1.001 );
                    float percentage = _DissolveClip / cutoutSource.r;
                    half3 edgeColor = _DissolveEdgeColor.rgb;
                    cutoutSource.a = FastSign(percentage -  _ColorFactor);
                    o.rgb = lerp(o.rgb, edgeColor, saturate(cutoutSource.a));
                #endif
				return o;  	
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
