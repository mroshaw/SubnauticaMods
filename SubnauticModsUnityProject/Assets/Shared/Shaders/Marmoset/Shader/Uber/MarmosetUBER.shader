Shader "MarmosetUBER" {
	Properties {
		[HideInInspector] _Color ("Diffuse Color", Vector) = (1,1,1,1)
		[MarmoBlendEnum(BoldLabel)] _Mode ("Blend Mode", Float) = 0
		[HideInInspector] _Mode ("Blend Mode", Float) = 0
		[HideInInspector] _SrcBlend ("__src", Float) = 1
		[HideInInspector] _DstBlend ("__dst", Float) = 10
		[HideInInspector] _SrcBlend2 ("__src2", Float) = 1
		[HideInInspector] _DstBlend2 ("__dst2", Float) = 10
		[HideInInspector] _AddSrcBlend ("__Addsrc", Float) = 1
		[HideInInspector] _AddDstBlend ("__Adddst", Float) = 10
		[HideInInspector] _AddSrcBlend2 ("__Addsrc2", Float) = 1
		[HideInInspector] _AddDstBlend2 ("__Adddst2", Float) = 10
		[MarmoToggle(null,null,BoldLabel)] _EnableMisc ("Misc", Float) = 1
		[Enum(TwoSided,0,OneSidedReverseWEIRD,1,OneSided,2)] _MyCullVariable ("Culling", Float) = 2
		[Toggle] _ZWrite ("ZWrite", Float) = 1
		_ZOffset ("Depth Offset", Float) = 0
		[MarmoToggle(MARMO_ALPHA_CLIP,null)] _EnableCutOff ("Alpha Test", Float) = 0
		[OptionalProperty(MARMO_ALPHA_CLIP FX_BUILDING)] _Cutoff ("Alpha Cutoff", Range(0, 1)) = 0
		[MarmoToggle(UWE_DITHERALPHA,null)] _EnableDitherAlpha ("Dither Alpha", Float) = 0
		[MarmoToggle(UWE_VR_FADEOUT,null)] _EnableVrFadeOut ("VR Fade Out", Float) = 0
		[TextureFeature(null,UWE_VR_FADEOUT,null,true)] _VrFadeMask ("VR Fade Mask", 2D) = "white" {}
		[MarmoToggle(null,null,BoldLabel)] _EnableLighting ("Lighting", Float) = 1
		_IBLreductionAtNight ("IBL reduction at night", Range(0, 1)) = 0.99
		[MarmoToggle(MARMO_SIMPLE_GLASS,null)] _EnableSimpleGlass ("Simple Glass", Float) = 0
		[MarmoToggle(MARMO_VERTEX_COLOR,null)] _EnableVertexColor ("VertexColor", Float) = 0
		[MarmoToggle(UWE_SCHOOLFISH,null)] _EnableSchoolFish ("SchoolFish", Float) = 0
		[MarmoToggle(null,null,BoldLabel)] _EnableMainMaps ("MAIN MAP", Float) = 1
		[TextureFeature(null,null,_Color,true)] _MainTex ("Diffuse Map (RGB), Gloss (A)", 2D) = "white" {}
		[TextureFeature(MARMO_NORMALMAP)] _BumpMap ("Normal Map", 2D) = "bump" {}
		[MarmoEnum(None UWE_SIG MARMO_SPECMAP, null, BoldLabel, Off SIGGmap SpecularMap)] _MarmoSpecEnum ("SPECULAR:", Float) = 0
		[TextureFeature(none ,UWE_SIG,_SpecColor,true)] _SIGMap ("SIGGmap: Specular(R), Illum(G), Gloss(B), Glow(A)", 2D) = "black" {}
		[TextureFeature(None,MARMO_SPECMAP,_SpecColor,true)] _SpecTex ("Specular Map (RGB) Gloss(A)", 2D) = "white" {}
		[HideInInspector] _SpecColor ("spec Color", Vector) = (1,1,1,1)
		[OptionalProperty(UWE_SIG MARMO_SPECMAP)] _SpecInt ("spec Intensity", Float) = 1
		[OptionalProperty(UWE_SIG MARMO_SPECMAP)] _Shininess ("spec Sharpness", Range(2, 8)) = 4
		[OptionalProperty(UWE_SIG MARMO_SPECMAP)] _Fresnel ("spec Fresnel", Range(0, 1)) = 0
		[MarmoToggle(MARMO_EMISSION,null,BoldLabel)] _EnableGlow ("GLOW", Float) = 0
		[HideInInspector] _GlowColor ("Glow Color", Vector) = (1,1,1,1)
		[TextureFeature(null,MARMO_EMISSION,_GlowColor,true)] _Illum ("Glow(RGB) Diffuse Emission(A)", 2D) = "white" {}
		[MarmoToggle(GLOW_UV_FROM_VERTECCOLOR,MARMO_EMISSION)] _GlowUVfromVC ("UV from VertexColor", Float) = 0
		[OptionalProperty(MARMO_EMISSION FX_IONCRYSTAL)] _GlowStrength ("glow Strength by Day", Float) = 1
		[OptionalProperty(MARMO_EMISSION FX_IONCRYSTAL)] _GlowStrengthNight ("glow Strength by night", Float) = 1
		[OptionalProperty(MARMO_EMISSION FX_IONCRYSTAL)] _EmissionLM ("glow Diffuse Emission Strength by Day", Float) = 0
		[OptionalProperty(MARMO_EMISSION FX_IONCRYSTAL)] _EmissionLMNight ("glow Diffuse Emission Strength by night", Float) = 0
		[MarmoToggle(UWE_DETAILMAP,null,BoldLabel)] _EnableDetailMaps ("DETAIL MAPS", Float) = 0
		[TextureFeature(null,UWE_DETAILMAP,null,false)] _DetailDiffuseTex ("detail Diffuse", 2D) = "gray" {}
		[TextureFeature(null,UWE_DETAILMAP,null,false)] _DetailBumpTex ("detail Normalmap", 2D) = "bump" {}
		[TextureFeature(null,UWE_DETAILMAP,null,false)] _DetailSpecTex ("detail Specular", 2D) = "gray" {}
		[OptionalProperty(UWE_DETAILMAP)] _DetailIntensities ("detail Intensity Diffuse(X) Normal(Y) Specular(Z)", Vector) = (0.2,0.2,0.2,0)
		[MarmoToggle(UWE_LIGHTMAP,null,BoldLabel)] _EnableLightmap ("LIGHTMAP", Float) = 0
		[TextureFeature(null,UWE_LIGHTMAP,_LightmapStrength,false)] _Lightmap ("Lightmap", 2D) = "white" {}
		[HideInInspector] _LightmapStrength ("Lightmap Strength", Float) = 2.65
		[MarmoToggle(UWE_3COLOR,null,BoldLabel)] _Enable3Color ("3 COLORS", Float) = 0
		[TextureFeature(null, UWE_3COLOR, null, false)] _MultiColorMask ("Mask (R = Color, G = Color2, B = Color3", 2D) = "white" {}
		[OptionalProperty(UWE_3COLOR)] _Color2 ("diffuse Color2", Vector) = (1,1,1,1)
		[OptionalProperty(UWE_3COLOR)] _Color3 ("diffuse Color3", Vector) = (1,1,1,1)
		[OptionalProperty(UWE_3COLOR)] _SpecColor2 ("spec Color2", Vector) = (1,1,1,1)
		[OptionalProperty(UWE_3COLOR)] _SpecColor3 ("spec Color3", Vector) = (1,1,1,1)
		[MarmoEnum(None FX_DEFORM FX_BUILDING FX_BLEEDER FX_ANIMATEDGLOW, null, BoldLabel)] FX ("Effect:", Float) = 0
		[OptionalProperty(FX_DEFORM FX_MESMER FX_BLEEDER)] _DeformMap ("_DeformMap", 2D) = "white" {}
		[OptionalProperty(FX_DEFORM FX_MESMER FX_BLEEDER)] _DeformParams ("DeformMap Speed(X,Y), Strength(Z, W)", Vector) = (1,1,0.2,0.2)
		[OptionalProperty(FX_BLEEDER)] _FillSack ("_FillSack", Range(0, 1)) = 0
		[OptionalProperty(FX_PROPULSIONCANNON)] _OverlayStrength ("_OverlayStrength", Range(0, 1)) = 1
		[OptionalProperty(FX_PROPULSIONCANNON FX_MESMER)] _GlowScrollColor ("Glow Scroll Color", Vector) = (1,1,1,1)
		[OptionalProperty(FX_PROPULSIONCANNON)] _GlowScrollMask ("_GlowScrollMask", 2D) = "white" {}
		[OptionalProperty(FX_MESMER)] _Hypnotize ("Hypnotize", Range(0, 1)) = 1
		[OptionalProperty(FX_MESMER FX_BLEEDER)] _ScrollColor ("ScrollTex Color", Vector) = (1,1,1,1)
		[OptionalProperty(FX_MESMER FX_BLEEDER)] _ColorStrength ("Color Strength", Vector) = (1,1,1,1)
		[OptionalProperty(FX_MESMER FX_BLEEDER FX_PROPULSIONCANNON)] _ScrollTex ("Scroll Texture", 2D) = "white" {}
		[OptionalProperty(FX_ANIMATEDGLOW)] _GlowMask ("_GlowMask", 2D) = "white" {}
		[OptionalProperty(FX_ANIMATEDGLOW)] _GlowMaskSpeed ("_GlowMaskSpeed", Vector) = (1,1,1,1)
		[OptionalProperty(FX_MESMER FX_BLEEDER)] _ScrollTex2 ("Scroll Texture2", 2D) = "white" {}
		[OptionalProperty(FX_MESMER FX_BLEEDER FX_PROPULSIONCANNON)] _ScrollSpeed ("Scroll Speed", Vector) = (0.1,0.1,0,0)
		[OptionalProperty(FX_IONCRYSTAL)] _NoiseTex ("Noise Texture", 2D) = "white" {}
		[OptionalProperty(FX_IONCRYSTAL)] _DetailsColor ("_DetailsColor Color", Vector) = (1,1,1,1)
		[OptionalProperty(FX_IONCRYSTAL)] _SquaresColor ("Squares Color", Vector) = (1,1,1,1)
		[OptionalProperty(FX_IONCRYSTAL)] _SquaresTile ("SquaresTile", Float) = 30
		[OptionalProperty(FX_IONCRYSTAL)] _SquaresSpeed ("SquaresSpeed", Float) = 0.5
		[OptionalProperty(FX_IONCRYSTAL)] _SquaresIntensityPow ("_SquaresIntensityPow", Float) = 2
		[OptionalProperty(FX_IONCRYSTAL)] _NoiseSpeed ("_NoiseSpeed", Vector) = (1,1,1,1)
		[OptionalProperty(FX_IONCRYSTAL)] _FakeSSSparams ("SSS intensity(X), range(Y), power(Z)", Vector) = (1,1,1,1)
		[OptionalProperty(FX_IONCRYSTAL)] _FakeSSSSpeed ("_FakeSSSSpeed", Vector) = (1,1,1,1)
		[OptionalProperty( FX_BUILDING)] _BorderColor ("Border Color", Vector) = (1,1,1,1)
		[OptionalProperty(FX_BUILDING)] _Built ("Built Amount", Float) = 0
		[OptionalProperty(FX_BUILDING)] _BuildParams ("BuildParams Scale(X), DetailScale(Y), Frequency(Z),Speed(W)", Vector) = (1,1,5,4)
		[OptionalProperty(FX_BUILDING)] _BuildLinear ("BuildLinear", Float) = 0
		[OptionalProperty(FX_BUILDING)] _EmissiveTex ("Emissive texture", 2D) = "white" {}
		[OptionalProperty(FX_BUILDING)] _NoiseThickness ("Noise Thickness", Float) = 0.05
		[OptionalProperty(FX_BUILDING)] _NoiseStr ("Noise Strength", Float) = 0.2
		[MarmoEnum(None UWE_WAVING FX_SINWAVE FX_KELP FX_ROPE, null, BoldLabel)] FX_Vertex ("Vertex Waving:", Float) = 0
		[OptionalProperty(UWE_WAVING FX_ROPE FX_SINWAVE FX_KELP)] _Scale ("Amplitude Main(X,Y,Z), Rustling(W)", Vector) = (0.12,0.05,0.12,0.1)
		[OptionalProperty(UWE_WAVING FX_ROPE FX_SINWAVE FX_KELP)] _Frequency ("Frequency Main(X,Y,Z), Rustling(W)", Vector) = (0.6,0.5,0.75,0.8)
		[OptionalProperty(UWE_WAVING FX_ROPE FX_SINWAVE FX_KELP)] _Speed ("MainSpeed (X), Rustling (Y)", Vector) = (0.6,0.3,0,0)
		[TextureFeature(null, FX_KELP, null, false)] _AnimMask ("Animation Mask", 2D) = "white" {}
		[OptionalProperty(UWE_WAVING)] _ObjectUp ("Object Up Axis", Vector) = (0,0,1,0)
		[OptionalProperty(UWE_WAVING)] _WaveUpMin ("Influence Cutoff Y", Float) = 0
		[OptionalProperty(UWE_WAVING FX_ROPE)] _Fallof ("Influence _Fallof", Float) = 1
		[OptionalProperty(FX_ROPE)] _RopeGravity ("Gravity", Float) = 1
		[OptionalProperty(FX_KELP FX_BUILDING)] _minYpos ("_minYpos", Float) = 0.5
		[OptionalProperty(FX_KELP FX_BUILDING)] _maxYpos ("_maxYpos", Float) = 0.5
		[MarmoToggle(FX_BURST,null,BoldLabel)] _EnableBurst ("FX Burst", Float) = 0
		[TextureFeature(null, FX_BURST, null, false)] _DispTex ("Displacement Texture", 2D) = "gray" {}
		[OptionalProperty(FX_BURST)] _Displacement ("Displacement", Range(0, 5)) = 5
		[OptionalProperty(FX_BURST)] _BurstStrength ("Burst Strength", Range(0, 1)) = 0
		[OptionalProperty(FX_BURST)] _Range ("Range (min,max)", Vector) = (0,70,0,1)
		[OptionalProperty(FX_BURST)] _ClipRange ("ClipRange [0,1]", Float) = 0.6
		[MarmoToggle(UWE_INFECTION,null,BoldLabel)] _EnableInfection ("(Keep OFF - debug only) Infection", Float) = 0
		[MarmoToggle(UWE_PLAYERINFECTION,null, BoldLabel)] _EnablePlayerInfection ("(Keep OFF - debug only) Player Infection", Float) = 0
		[OptionalProperty(UWE_INFECTION UWE_PLAYERINFECTION)] _InfectionHeightStrength ("Infection Height Strength", Float) = 0
		[OptionalProperty(UWE_INFECTION UWE_PLAYERINFECTION)] _InfectionScale ("Infection Scale (X, Y, Z)", Vector) = (1,1,1,0)
		[OptionalProperty(UWE_INFECTION UWE_PLAYERINFECTION)] _InfectionOffset ("Infection Offset (X, Y, Z)", Vector) = (0,0,0,0)
		[OptionalProperty(UWE_PLAYERINFECTION)] _InfectionNoiseTex ("_InfectionNoiseTex", 2D) = "white" {}
		[OptionalProperty(UWE_PLAYERINFECTION)] _InfectionSpeed ("_InfectionSpeed", Vector) = (1,1,1,0)
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback "VertexLit"
}