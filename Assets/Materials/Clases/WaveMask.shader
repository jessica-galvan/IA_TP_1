// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "WaveMask"
{
	Properties
	{
		_EdgeLength ( "Edge length", Range( 2, 50 ) ) = 15
		_Distance("Distance", Vector) = (0,0,0,0)
		_radius("radius", Float) = 1
		_FallOff("FallOff", Float) = 0
		_High("High", Float) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "Tessellation.cginc"
		#pragma target 4.6
		#pragma surface surf Standard alpha:fade keepalpha addshadow fullforwardshadows exclude_path:deferred vertex:vertexDataFunc tessellate:tessFunction 
		struct Input
		{
			float3 worldPos;
		};

		uniform float3 _Distance;
		uniform float _radius;
		uniform float _FallOff;
		uniform float _High;
		uniform float _EdgeLength;

		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityEdgeLengthBasedTess (v0.vertex, v1.vertex, v2.vertex, _EdgeLength);
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float3 temp_output_14_0 = ( sin( ( _Time.y + pow( ( distance( _Distance , ase_worldPos ) / _radius ) , _FallOff ) ) ) * float3(0,1,0) * _High );
			v.vertex.xyz += temp_output_14_0;
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 color17 = IsGammaSpace() ? float4(1,0.0235849,0.0235849,0) : float4(1,0.001825457,0.001825457,0);
			float3 ase_worldPos = i.worldPos;
			float3 temp_output_14_0 = ( sin( ( _Time.y + pow( ( distance( _Distance , ase_worldPos ) / _radius ) , _FallOff ) ) ) * float3(0,1,0) * _High );
			o.Albedo = ( color17 * float4( temp_output_14_0 , 0.0 ) ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
0;514;1474;485;621.5823;55.856;1.431485;True;True
Node;AmplifyShaderEditor.WorldPosInputsNode;1;-837.4886,90.85349;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.Vector3Node;2;-838.4886,-86.14651;Inherit;False;Property;_Distance;Distance;5;0;Create;True;0;0;0;False;0;False;0,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DistanceOpNode;3;-558.4886,36.85349;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-607.5222,185.1198;Inherit;False;Property;_radius;radius;6;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;4;-373.5222,38.1198;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-304.4914,204.1506;Inherit;False;Property;_FallOff;FallOff;7;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;6;-167.5222,36.1198;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;12;-167.8595,-113.928;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;10;45.13921,-1.790596;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;11;297.3961,-4.642849;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;16;282.1872,222.2704;Inherit;False;Constant;_Vector0;Vector 0;3;0;Create;True;0;0;0;False;0;False;0,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;13;302.9653,406.5643;Inherit;False;Property;_High;High;8;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;562.7998,73.14769;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;17;619.1307,-181.8712;Inherit;False;Constant;_Color0;Color 0;5;0;Create;True;0;0;0;False;0;False;1,0.0235849,0.0235849,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;835.7128,-69.02454;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1101.007,-142.0927;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;WaveMask;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;3;0;2;0
WireConnection;3;1;1;0
WireConnection;4;0;3;0
WireConnection;4;1;5;0
WireConnection;6;0;4;0
WireConnection;6;1;7;0
WireConnection;10;0;12;0
WireConnection;10;1;6;0
WireConnection;11;0;10;0
WireConnection;14;0;11;0
WireConnection;14;1;16;0
WireConnection;14;2;13;0
WireConnection;18;0;17;0
WireConnection;18;1;14;0
WireConnection;0;0;18;0
WireConnection;0;11;14;0
ASEEND*/
//CHKSM=8758A43EA733D31F92A4A38116707F5F68D04B6D