Shader "Custom/Noise" {
	Properties{
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Emission("Emission", float) = 1
		_Brightness("Brightness", Range(0,2)) = 1
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
	}
		SubShader{
			Tags{ "RenderType" = "Opaque" "Queue" = "Geometry"}

			CGPROGRAM

			#pragma surface surf Standard fullforwardshadows
			#pragma target 3.0

		float _Emission;
		float _Brightness;
		half _Glossiness;
		half _Metallic;


		struct Input {
			float3 worldPos;
		};

		float rand3dTo1d(float3 value, float3 dotDir = float3(12.9898, 78.233, 37.719)) {
			//make value smaller to avoid artefacts
			float3 smallValue = sin(value);
			//get scalar value from 3d vector
			float random = dot(smallValue, dotDir);
			//make value more random by making it bigger and then taking the factional part
			random = frac(sin(random) * 143758.5453);
			return random;
		}

		float3 rand3dTo3d(float3 value) {
			return float3(
				rand3dTo1d(value, float3(12.989, 78.233, 37.719)),
				rand3dTo1d(value, float3(39.346, 11.135, 83.155)),
				rand3dTo1d(value, float3(73.156, 52.235, 09.151))
				);
		}

		void surf(Input i, inout SurfaceOutputStandard o) {

			float3 value = i.worldPos;
			o.Albedo = rand3dTo3d(value * _Time)* _Brightness;
			o.Emission = o.Albedo * _Emission* _Brightness;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
		}

		ENDCG
		}
			FallBack "Standard"
}