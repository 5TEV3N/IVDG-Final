Shader "Hidden/VacuumShaders/DirectX 11 Low Poly/Unlit_Transparent_ZWrite_Diffuse" 
{
	Properties 
	{ 
		[VacuumShadersShaderType] _SHADER_TYPE_LABEL("", float) = 0
		[VacuumShadersRenderingMode] _RENDERING_MODE_LABEL("", float) = 0

		[VacuumShadersLabel] _VERTEX_LABEL("Low Poly", float) = 0
		[Enum(Triangle,0,Quad,1)] _SamplingType("Sampling Type", Float) = 0
		_MainTex ("Texture #1", 2D) = "white" {}
		[VacuumShadersUVScroll] _MainTex_Scroll("    ", vector) = (0, 0, 0, 0)		
		  
		[VacuumShadersSecondVertexTexture] _SecondTextureID("", Float) = 0
		[HideInInspector] _SecondTex_BlendType("", Float) = 0
		[HideInInspector] _SecondTex_AlphaOffset("", Range(-1, 1)) = 0
		[HideInInspector] _SecondTex ("", 2D) = "white" {}
		[HideInInspector] _SecondTex_Scroll("", vector) = (0, 0, 0, 0) 
		 
		[VacuumShadersLabel] _PIXEL_LABEL("Fragment", float) = 0
		_Color ("Tint Color", Color) = (1,1,1,1)	
		[VacuumShadersToggleSimple] _VertexColor("Mesh Vertex Color", Float) = 0

		[VacuumShadersPixelTexture] _PixelTextureID("", Float) = 0
		[HideInInspector] _PixelTex_BlendType("Blend Type", Float) = 0
		[HideInInspector] _PixelTex_AlphaOffset("", Range(-1, 1)) = 0
		[HideInInspector] _PixelTex ("  Texture", 2D) = "white" {}
		[HideInInspector] _PixelTex_Scroll("    ", vector) = (0, 0, 0, 0)

		[VacuumShadersLargeLabel] _ALPHA_LABEL(" Alpha", float) = 0
		[VacuumShadersToggleSimple] _AlphaFromVertex("    Use Low Poly Alpha", Float) = 0
		[VacuumShadersToggleZWrite] _ZWrite_LABEL("    ZWrite", float) = 0

		[VacuumShadersReflection] _REFLECTION_LABEL("Reflective", float) = 0
		
		[VacuumShadersLabel] _Dsiplace_LABEL("Displace", float) = 0	
		[VacuumShadersDisplaceType] _DisplaceType("", Float) = 0
		[HideInInspector] _DisplaceTex_1 ("", 2D) = "gray" {}
		[HideInInspector] _DisplaceTex_1_Scroll("", vector) = (0, 0, 0, 0)
		[HideInInspector] _DisplaceTex_2 ("", 2D) = "gray" {}
		[HideInInspector] _DisplaceTex_2_Scroll("", vector) = (0, 0, 0, 0)
        [HideInInspector] _DisplaceBlendType ("Blend Type", Float) = 1
		[HideInInspector] _DisplaceStrength ("", float) = 1

		[HideInInspector] _DisplaceDirection("", Range(0, 360)) = 45
		[HideInInspector] _DisplaceScriptSynchronize("", Float) = 0
		[HideInInspector] _DisplaceSpeed("", Float) = 1
		[HideInInspector] _DisplaceAmplitude ("", Float) = 0.5
		[HideInInspector] _DisplaceFrequency("", Float) = 0.2
		[HideInInspector] _DisplaceNoiseCoef("", Float) = -0.5		

		//PaperCraft
		[VacuumShadersLabel] _PAPERCRAFT_LABEL("Wireframe", float) = 0
		[VacuumShadersPaperCraft] _PaperCrat("", float) = 0
		[HideInInspector] _V_WIRE_FixedSize("Fixed Size", float) = 0
		[HideInInspector] _V_WIRE_Size("Wire Size", Float) = 0.5
		[HideInInspector] _V_WIRE_Color("Wire Color", color) = (0, 0, 0, 1)
	} 

	SubShader 
	{
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PaperCraft" = "Off" }
		LOD 200
	
		UsePass "Hidden/VacuumShaders/DirectX 11 Low Poly/ZWrite/MAIN"


	ZWrite Off
	Blend SrcAlpha OneMinusSrcAlpha 

	// ---- forward rendering base pass:
	Pass 
	{
		Name "Main"

		CGPROGRAM
		// compile directives
		#pragma vertex vert_surf
		#pragma geometry geom
		#pragma fragment frag_surf
		#include "Assets/VacuumShaders/DirectX 11 Low Poly Shader/Shaders/cginc/Platform.cginc"
		#pragma multi_compile_fog
		#include "HLSLSupport.cginc"
		#include "UnityCG.cginc"


		struct v2f_surf 
		{
		  float4 pos : SV_POSITION;
		  float2 pixelTexUV : TEXCOORD0;
		  fixed4 color : COLOR0;
		  UNITY_FOG_COORDS(1)
		  float3 mass : TEXCOORD2;
		  float3 worldPos : TEXCOORD3;
		};


		#pragma shader_feature _ V_LP_SECOND_TEXTURE_ON
		#pragma shader_feature _ V_LP_PIXEL_TEXTURE_ON
		#pragma shader_feature _ V_LP_DISPLACE_PARAMETRIC V_LP_DISPLACE_TEXTURE
		#define V_LP_TRANSPARENT
		#define V_GEOMETRY_SAVE_LOWPOLY_COLOR
//		#include "Assets/VacuumShaders/DirectX 11 Low Poly Shader/Shaders/cginc/PaperCraft.cginc"
		#include "Assets/VacuumShaders/DirectX 11 Low Poly Shader/Shaders/cginc/Core.cginc"


		// vertex shader
		v2f_surf vert_surf (appdata_full v) {
		 
		 SET_UP_LOW_POLY_DATA(v)

		  o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

		  UNITY_TRANSFER_FOG(o,o.pos); // pass fog coordinates to pixel shader
		  return o;
		}


		// fragment shader
		fixed4 frag_surf (v2f_surf IN) : SV_Target 
		{

		  fixed4 lowpolyColor = GetLowpolyPixelColor(IN.pixelTexUV, IN.color);

		//PaperCraft
//		MakePaperCraft(IN.mass, IN.worldPos, lowpolyColor);


		  UNITY_APPLY_FOG(IN.fogCoord, lowpolyColor); // apply fog
 
		  return lowpolyColor;
		}

		ENDCG

		}


	}

	Fallback "Unlit/Transparent Cutout"
}
