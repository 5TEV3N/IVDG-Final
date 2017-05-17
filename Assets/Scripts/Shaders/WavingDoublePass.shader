﻿Shader "Custom/WavingDoublePass" {
	Properties{
		_WavingTint("Fade Color", Color) = (.7,.6,.5, 0)
		_MainTex("Base (RGB) Alpha (A)", 2D) = "white" {}
	_WaveAndDistance("Wave and distance", Vector) = (12, 3.6, 1, 1)
		_Cutoff("Cutoff", float) = 0.5
		_Cutoff2("Cutoff2", float) = 0.5
	}

		SubShader{
		Tags{
		"Queue" = "Geometry+200"
		"IgnoreProjector" = "True"
		"RenderType" = "Grass"
	}
		Cull Off
		LOD 200
		ColorMask RGB

		CGPROGRAM
#pragma surface surf CelShadingForward fullforwardshadows vertex:WavingGrassVert addshadow alphatest:_Cutoff2
#pragma exclude_renderers flash
#include "TerrainEngine.cginc"

		sampler2D _MainTex;
	fixed _Cutoff;

	struct Input {
		float2 uv_MainTex;
		fixed4 color : COLOR;
	};


	half4 LightingCelShadingForward(SurfaceOutput s, half3 lightDir, half atten) {
		half NdotL = dot(s.Normal, lightDir);
		if (NdotL <= 0.3) NdotL = 0.2;
		if (NdotL > 0.3 && NdotL < 0.9) NdotL = 0.6;
		//if (NdotL <= 0.0) NdotL = 0;
		if (NdotL >= 0.9) NdotL = 1;
		//else NdotL = 1;
		half4 c;
		c.rgb = s.Albedo * _LightColor0.rgb * (NdotL * atten * 1.5);
		c.a = s.Alpha;
		return c;
	}


	void surf(Input IN, inout SurfaceOutput o) {
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * IN.color;
		fixed4 d = tex2D(_MainTex, IN.uv_MainTex);
		o.Albedo = c.rgb;
		//    o.Alpha = c.a;
		o.Alpha = d.a;
		clip(o.Alpha - _Cutoff);
		//    o.Alpha *= IN.color.a;
	}
	ENDCG
	}

		SubShader{
		Tags{
		"Queue" = "Geometry+200"
		"IgnoreProjector" = "True"
		"RenderType" = "Grass"
	}
		Cull Off
		LOD 200
		ColorMask RGB

		Pass{
		Tags{ "LightMode" = "Vertex" }
		Material{
		Diffuse(1,1,1,1)
		Ambient(1,1,1,1)
	}
		Lighting On
		ColorMaterial AmbientAndDiffuse
		AlphaTest Greater[_Cutoff]
		SetTexture[_MainTex]{ combine texture * primary DOUBLE, texture }
	}
		Pass{
		Tags{ "LightMode" = "VertexLMRGBM" }
		AlphaTest Greater[_Cutoff]
		BindChannels{
		Bind "Vertex", vertex
		Bind "texcoord1", texcoord0 // lightmap uses 2nd uv
		Bind "texcoord", texcoord1 // main uses 1st uv
	}
		SetTexture[unity_Lightmap]{
		matrix[unity_LightmapMatrix]
		combine texture * texture alpha DOUBLE
	}
		SetTexture[_MainTex]{ combine texture * previous QUAD, texture }
	}
	}

		Fallback Off
}