// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Coin"
{
	Properties
	{
		_Smoothness("Smoothness", Float) = 0
		_Color0("Color 0", Color) = (0.8196079,0.627451,0.3098039,0)
		_Metallic("Metallic", Float) = 0
		_FlashWidth("FlashWidth", Range( 0 , 1)) = 0.8588235
		_Color2("Color 2", Color) = (0,0,0,0)
		_Freq("Freq", Float) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float3 worldPos;
		};

		uniform float4 _Color0;
		uniform float4 _Color2;
		uniform float _Freq;
		uniform float _FlashWidth;
		uniform float _Metallic;
		uniform float _Smoothness;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Albedo = _Color0.rgb;
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float mulTime26 = _Time.y * 3.0;
			o.Emission = ( _Color2 * (0.0 + (saturate( ( sin( ( ( ase_vertex3Pos.x + ase_vertex3Pos.y + mulTime26 ) * _Freq ) ) - _FlashWidth ) ) - 0.0) * (1.0 - 0.0) / (( 1.0 - _FlashWidth ) - 0.0)) ).rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17200
0;389;1294;430;750.7139;-128.8042;1;True;False
Node;AmplifyShaderEditor.CommentaryNode;20;-706.7299,125.1033;Inherit;False;358;324;Nodos a usar;2;12;5;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-653.0594,72.73351;Inherit;False;Constant;_Float0;Float 0;5;0;Create;True;0;0;False;0;3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;26;-527.0593,75.58108;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;5;-656.7299,175.1033;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;25;-373.0593,199.5811;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-364.3994,313.5143;Inherit;False;Property;_Freq;Freq;5;0;Create;True;0;0;False;0;0;2.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;-220.3994,209.5143;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-631.7494,349.2821;Inherit;False;Property;_FlashWidth;FlashWidth;3;0;Create;True;0;0;False;0;0.8588235;0.7203046;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;24;-81.82396,225.0757;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;34;31.60059,230.5143;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;28;225.0407,253.5811;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;38;-101.4505,438.0969;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;37;350.4421,271.5274;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;18;177.5598,52.93763;Inherit;False;Property;_Color2;Color 2;4;0;Create;True;0;0;False;0;0,0,0,0;0.6313726,0.4922109,0.1921569,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;2;8.493759,-33.7739;Inherit;False;Property;_Smoothness;Smoothness;0;0;Create;True;0;0;False;0;0;0.85;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;1;-328.647,-199.6457;Inherit;False;Property;_Color0;Color 0;1;0;Create;True;0;0;False;0;0.8196079,0.627451,0.3098039,0;0.9150943,0.6334964,0.2374066,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;474.5506,151.6418;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;3;28.47902,-103.8034;Inherit;False;Property;_Metallic;Metallic;2;0;Create;True;0;0;False;0;0;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;604.5361,-165.831;Float;False;True;2;ASEMaterialInspector;0;0;Standard;Coin;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;26;0;29;0
WireConnection;25;0;5;1
WireConnection;25;1;5;2
WireConnection;25;2;26;0
WireConnection;30;0;25;0
WireConnection;30;1;31;0
WireConnection;24;0;30;0
WireConnection;34;0;24;0
WireConnection;34;1;12;0
WireConnection;28;0;34;0
WireConnection;38;0;12;0
WireConnection;37;0;28;0
WireConnection;37;2;38;0
WireConnection;16;0;18;0
WireConnection;16;1;37;0
WireConnection;0;0;1;0
WireConnection;0;2;16;0
WireConnection;0;3;3;0
WireConnection;0;4;2;0
ASEEND*/
//CHKSM=CE97D56ACDF71B7FF53B71C36766AD10B73C94D8