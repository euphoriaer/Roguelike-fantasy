
Shader "HeroGo/Particle/Additive"
{
	Properties
	{
		_TintColor("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex("Particle Texture", 2D) = "white" {}
		_InvFade("Soft Particles Factor", Range(0.01,3.0)) = 1.0
	}

	Category
	{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "PreviewType" = "Plane" }
		Blend SrcAlpha One
		Cull Off 
		Lighting Off
        ZWrite Off 

		Fog{ Mode Off }

		BindChannels
		{
			Bind "Color", color
			Bind "Vertex", vertex
			Bind "TexCoord", texcoord
		}

		SubShader
		{
			Pass
			{
				SetTexture[_MainTex]
				{
					constantColor[_TintColor]
					combine constant * primary
				}
				SetTexture [_MainTex] {
				combine texture * previous DOUBLE
			}
			}
		}
	}
}

