﻿Shader "Custom/CelShadingForward" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf CelShadingForward fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		fixed4 _Color;

		struct Input {
			float2 uv_MainTex;
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
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
		FallBack "Diffuse"
}