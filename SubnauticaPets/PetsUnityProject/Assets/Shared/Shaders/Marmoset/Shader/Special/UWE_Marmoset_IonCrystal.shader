Shader "UWE/Marmoset/IonCrystal" {
	Properties {
		[HideInInspector] _Color ("Diffuse Color", Vector) = (1,1,1,1)
		[MarmoToggle(null,null,BoldLabel)] _EnableMisc ("Misc", Float) = 1
		[Enum(TwoSided,0,OneSidedReverseWEIRD,1,OneSided,2)] _MyCullVariable ("Culling", Float) = 2
		[Toggle] _ZWrite ("ZWrite", Float) = 1
		_ZOffset ("Depth Offset", Float) = 0
		[OptionalProperty(MARMO_ALPHA_CLIP FX_BUILDING)] _Cutoff ("Alpha Cutoff", Range(0, 1)) = 0
		[MarmoToggle(null,null,BoldLabel)] _EnableLighting ("Lighting", Float) = 1
		_IBLreductionAtNight ("IBL reduction at night", Range(0, 1)) = 0.99
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
		[OptionalProperty(MARMO_EMISSION FX_IONCRYSTAL)] _GlowStrength ("glow Strength by Day", Float) = 1
		[OptionalProperty(MARMO_EMISSION FX_IONCRYSTAL)] _GlowStrengthNight ("glow Strength by night", Float) = 1
		[OptionalProperty(MARMO_EMISSION FX_IONCRYSTAL)] _EmissionLM ("glow Diffuse Emission Strength by Day", Float) = 0
		[OptionalProperty(MARMO_EMISSION FX_IONCRYSTAL)] _EmissionLMNight ("glow Diffuse Emission Strength by night", Float) = 0
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