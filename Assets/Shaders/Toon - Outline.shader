Shader "Shore/Toon - Outline"
{
	Properties
	{
		_OutlineColor("Outline Color", Color) = (1,1,1,1)
		_OutlineWidth("Outline Width", Range(1., 5.)) = 1.
	}
	CGINCLUDE
	#include "UnityCG.cginc"
	struct appdata
	{
		float4 vertex : POSITION;
	};
	struct v2f
	{
		float4 pos : POSITION;
		float4 color : COLOR;
	};
	ENDCG
	SubShader
	{
		Tags{ "Queue" = "Geometry+1" }
		//Cull Back
		Pass
		{
			ZWrite Off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			float _OutlineWidth;
			float4 _OutlineColor;
			v2f vert(appdata v)
			{
				v.vertex.xyz *= _OutlineWidth;
				v2f o;
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;
			}
			half4 frag(v2f i) : COLOR
			{
				return _OutlineColor;
			}
			ENDCG
		}
	}
	Fallback "VertexLit"
}