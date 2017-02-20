Shader "Custom/VertexColorAndRim" {
	Properties {
		_MainColor ("Base Color", Color) = (1,1,1,1)
		_VColor ("Vertex White", Color) = (1,1,1,1)
		_RimColor ("Rim Color", Color) = (1,0,1,1)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		//sampler2D _MainTex;
		half4 _MainColor;
		half4 _VColor;
		half4 _RimColor;
		
		struct Input {
			//float2 uv_MainTex;
			fixed4 color : COLOR;
			float3 viewDir;
		};


		void surf (Input IN, inout SurfaceOutput o) {
			// our color is composed of vertex colors multiplied with base color
			
			half4 c = _MainColor * (1-_MainColor.a) + (IN.color * _VColor) * _MainColor.a; // tex2D (_MainTex, IN.uv_MainTex);
			
			half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
          	
          	o.Emission = _RimColor.rgb * pow (rim, _RimColor.a*3);
			
			
			o.Albedo = c.rgb;
			o.Alpha = c.a;
			
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
