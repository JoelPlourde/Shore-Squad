Shader "MIA Studios/Skybox"
{
	Properties
	{
		[Header(General)]
		_SkyIntensity("Sky Intensity", Float) = 0.8
		_StarDistance("Star Distance", Range(100.0, 600.0)) = 500.0

		[Header(Day)]
		_SkyColor1("Top Color", Color) = (0.37, 0.52, 0.73, 0)
		_SkyColor2("Horizon Color", Color) = (0.89, 0.96, 1, 0)
		_SunColor("Sun Color", Color) = (1, 0.99, 0.87, 1)
		_SunIntensity("Sun Intensity", Range(0.0, 20.0)) = 10.0

		[Header(Night)]
		_SkyColor3("Top Color", Color) = (0,0,0,0)
		_SkyColor4("Horizon Color", Color) = (0.89, 0.96, 1, 0)
		_MoonColor("Moon Color", Color) = (1, 0.99, 0.87, 1)
		_MoonIntensity("Moon Intensity", Range(0.0, 20.0)) = 10.0

		[Header(Stars)]
		_StarsDensity("Stars", Range(0,20)) = 9
		_StarsIntensity("Stars Intensity", Range(0.0, 15.0)) = 12
	}

		CGINCLUDE

#include "UnityCG.cginc"

			struct vertexInput
		{
			float4 vertex : POSITION;
			float3 texcoord : TEXCOORD0;
		};

		struct vertexOutput
		{
			float4 vertex : SV_POSITION;
			float3 texcoord : TEXCOORD0;
		};

		half _StarsDensity;

		vertexOutput vert(vertexInput v)
		{
			vertexOutput o;
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.texcoord = v.texcoord * _StarsDensity;

			float2 lightVec = abs(normalize(_WorldSpaceLightPos0.xy + float3(180, 0, 0)));
			float3x3 m = float3x3(
				1, 0, 0,
				0, lightVec.x, lightVec.y,
				0, -lightVec.y, lightVec.x
				);
			o.texcoord = mul(o.texcoord.xyz, m) * _StarsDensity;

			return o;
		}

		half random(half3 st)
		{
			return frac(sin(dot(st.xyz, float3(12.9898, 78.233, 1)))*43758.5453123);
		}

		half _SkyIntensity;
		half3 _SunColor;
		half3 _MoonColor;
		half _SunIntensity;
		half _MoonIntensity;
		half _StarsIntensity;
		half3 _SkyColor1;
		half3 _SkyColor2;
		half3 _SkyColor3;
		half3 _SkyColor4;
		half _StarDistance;

		half4 frag(vertexOutput i) : COLOR
		{
			half3 v = normalize(i.texcoord);

			half p = 1 - pow(min(1, 1 - v.y), pow(0.5, v.x*v.z));

			half2 lightVec = normalize(_WorldSpaceLightPos0.xyz);
			half y = saturate(lightVec.y);

			half3 color_day = (_SkyColor1 * p + _SkyColor2 * (1 - p));
			half3 color_night = (_SkyColor3 * p + _SkyColor4 * (1 - p));
			half3 color_sun = _SunColor * min(pow(max(0, dot(v, -_WorldSpaceLightPos0.xyz)), _StarDistance), 1) * _SunIntensity;
			half3 color_moon = _MoonColor * min(pow(max(0, dot(v, _WorldSpaceLightPos0.xyz)), _StarDistance), 1) * _MoonIntensity;

			half rand = random(round(i.texcoord));
			half2 temp_color = saturate(half2(rand - 0.997, rand - 0.99));
			half3 color_star = ((temp_color.x + temp_color.y * 5)*(saturate(1 - color_moon.b * 5)) * _StarsIntensity) * (max(0.5 - max(0,y), 0));

			return half4((color_night * (1 - y) + color_day * y) * _SkyIntensity + color_sun + color_moon + (color_star * _StarsIntensity), 0);
		}
			ENDCG

			SubShader
		{

			Tags{ "RenderType" = "Skybox" "Queue" = "Background" }
				Pass
			{
				ZWrite Off
				Cull Off
				Fog { Mode Off }
				CGPROGRAM
				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma vertex vert
				#pragma fragment frag
				ENDCG
			}
		}
}