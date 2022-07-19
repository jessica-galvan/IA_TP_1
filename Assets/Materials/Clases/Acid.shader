// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Acid"
{
	Properties
	{
		_TessValue( "Max Tessellation", Range( 1, 32 ) ) = 15
		_Scale("Scale", Float) = 0
		_Height("Height", Float) = 1
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 4.6
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc tessellate:tessFunction 
		struct Input
		{
			float3 worldPos;
		};

		uniform float _Height;
		uniform float _Scale;
		uniform float _TessValue;


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		float4 tessFunction( )
		{
			return _TessValue;
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float2 appendResult19 = (float2(ase_worldPos.x , ase_worldPos.z));
			float mulTime24 = _Time.y * -0.2;
			float simplePerlin2D16 = snoise( ( appendResult19 + mulTime24 )*_Scale );
			simplePerlin2D16 = simplePerlin2D16*0.5 + 0.5;
			v.vertex.xyz += ( float3(0,1,0) * _Height * simplePerlin2D16 );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 color8 = IsGammaSpace() ? float4(0.3005355,0.764151,0.01802243,0) : float4(0.07350438,0.5448383,0.001394925,0);
			float4 color9 = IsGammaSpace() ? float4(0.06004083,0.2641509,0.008721969,0) : float4(0.004900483,0.05672633,0.000675075,0);
			float3 ase_worldPos = i.worldPos;
			float2 appendResult19 = (float2(ase_worldPos.x , ase_worldPos.z));
			float mulTime24 = _Time.y * -0.2;
			float simplePerlin2D16 = snoise( ( appendResult19 + mulTime24 )*_Scale );
			simplePerlin2D16 = simplePerlin2D16*0.5 + 0.5;
			float4 lerpResult7 = lerp( color8 , color9 , simplePerlin2D16);
			o.Emission = lerpResult7.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17200
0;389;1294;430;1699.508;97.4991;1.593125;True;False
Node;AmplifyShaderEditor.CommentaryNode;14;-1189.016,207.4903;Inherit;False;252;479.9106;Nodos a usar;1;2;;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldPosInputsNode;2;-1139.016,257.4903;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;25;-1114.737,412.587;Inherit;False;Constant;_Float0;Float 0;3;0;Create;True;0;0;False;0;-0.2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;24;-965.9615,382.9092;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;19;-917.1163,273.0168;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;22;-782.7614,272.1093;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-875.8734,498.9157;Inherit;False;Property;_Scale;Scale;5;0;Create;True;0;0;False;0;0;0.69;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;8;-556.4872,-135.1898;Inherit;False;Constant;_Color0;Color 0;1;0;Create;True;0;0;False;0;0.3005355,0.764151,0.01802243,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;9;-560.2872,37.21021;Inherit;False;Constant;_Color1;Color 1;1;0;Create;True;0;0;False;0;0.06004083,0.2641509,0.008721969,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;6;-363.1227,508.5648;Inherit;False;Property;_Height;Height;6;0;Create;True;0;0;False;0;1;0.49;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;5;-365.8157,347.4327;Inherit;False;Constant;_DireccionY;DireccionY;0;0;Create;True;0;0;False;0;0,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.NoiseGeneratorNode;16;-642.9808,371.2567;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;7;-245.238,-34.18591;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-185.8599,354.939;Inherit;False;3;3;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;6;ASEMaterialInspector;0;0;Standard;Acid;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;1;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;24;0;25;0
WireConnection;19;0;2;1
WireConnection;19;1;2;3
WireConnection;22;0;19;0
WireConnection;22;1;24;0
WireConnection;16;0;22;0
WireConnection;16;1;20;0
WireConnection;7;0;8;0
WireConnection;7;1;9;0
WireConnection;7;2;16;0
WireConnection;21;0;5;0
WireConnection;21;1;6;0
WireConnection;21;2;16;0
WireConnection;0;2;7;0
WireConnection;0;11;21;0
ASEEND*/
//CHKSM=D6FE602DDEA850E5B5379B13A6F5F526BBA46116