// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ToonWater"
{
	Properties
	{
		_Water("Water", 2D) = "white" {}
		_DeepthScale("DeepthScale", Range( 0 , 1)) = 0.4114811
		_WaterTiling("WaterTiling", Range( 0 , 10)) = 3.451827
		_FlowTiling("FlowTiling", Range( 0 , 1)) = 3.451827
		_WaterSpeed("WaterSpeed", Vector) = (0,0,0,0)
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_Distortion("Distortion", Range( 0 , 1)) = 0.1624063
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma surface surf Standard alpha:fade keepalpha addshadow fullforwardshadows exclude_path:deferred 
		struct Input
		{
			float2 uv_texcoord;
			float4 screenPos;
		};

		uniform sampler2D _Water;
		uniform float2 _WaterSpeed;
		uniform sampler2D _TextureSample1;
		uniform float _FlowTiling;
		uniform float _WaterTiling;
		uniform float _Distortion;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _DeepthScale;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 color49 = IsGammaSpace() ? float4(0.2784314,0.7294118,0.7254902,1) : float4(0.06301003,0.4910209,0.48515,1);
			float2 temp_cast_0 = (_FlowTiling).xx;
			float2 uv_TexCoord32 = i.uv_texcoord * temp_cast_0;
			float2 temp_cast_1 = (_WaterTiling).xx;
			float2 uv_TexCoord4 = i.uv_texcoord * temp_cast_1;
			float4 lerpResult31 = lerp( tex2D( _TextureSample1, uv_TexCoord32 ) , float4( uv_TexCoord4, 0.0 , 0.0 ) , _Distortion);
			float2 panner1 = ( 1.0 * _Time.y * _WaterSpeed + lerpResult31.rg);
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth39 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth39 = abs( ( screenDepth39 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( 6.0 ) );
			float4 lerpResult37 = lerp( color49 , tex2D( _Water, panner1 ) , pow( distanceDepth39 , _DeepthScale ));
			o.Emission = lerpResult37.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
0;501.1429;1680;652.4286;1587.435;784.4804;1.938073;True;False
Node;AmplifyShaderEditor.CommentaryNode;29;-1505.703,-755.5955;Inherit;False;1566.738;382.0203;Water Wave Flow Effect;5;30;33;31;28;32;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-1471.928,-687.1552;Inherit;False;Property;_FlowTiling;FlowTiling;3;0;Create;True;0;0;0;False;0;False;3.451827;0.11272;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;9;-947.5816,-354.1546;Inherit;False;952.5812;415.8426;Movement;4;1;4;5;11;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;32;-1115.544,-721.3305;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;5;-895.6265,-228.1962;Inherit;False;Property;_WaterTiling;WaterTiling;2;0;Create;True;0;0;0;False;0;False;3.451827;3.76224;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;4;-538.5564,-255.8112;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;30;-731.6191,-499.9263;Inherit;False;Property;_Distortion;Distortion;6;0;Create;True;0;0;0;False;0;False;0.1624063;0.5386284;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;28;-779.5032,-705.5955;Inherit;True;Property;_TextureSample1;Texture Sample 1;5;0;Create;True;0;0;0;False;0;False;-1;None;94c976211f7b99b46a309c1024cae8a2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;35;-143.8328,164.8969;Inherit;False;710.0369;251.6162;DepthFade;4;41;39;38;36;;1,1,1,1;0;0
Node;AmplifyShaderEditor.LerpOp;31;-377.5607,-641.4687;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;38;-115.2707,224.4969;Inherit;False;Constant;_Float1;Float 1;0;0;Create;True;0;0;0;False;0;False;6;6.6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;11;-521.1016,-114.5178;Inherit;False;Property;_WaterSpeed;WaterSpeed;4;0;Create;True;0;0;0;False;0;False;0,0;0.01,0.02;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;1;-81.6989,-190.3851;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DepthFade;39;114.8822,205.3002;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;36;54.268,312.7448;Inherit;False;Property;_DeepthScale;DeepthScale;1;0;Create;True;0;0;0;False;0;False;0.4114811;0.424;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;41;387.2223,223.6317;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;49;255.9912,-55.51253;Inherit;False;Constant;_Color0;Color 0;7;0;Create;True;0;0;0;False;0;False;0.2784314,0.7294118,0.7254902,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;3;178.0255,-254.0681;Inherit;True;Property;_Water;Water;0;0;Create;True;0;0;0;False;0;False;-1;None;3394991baf929de4b9e2a158da89e970;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;37;677.5339,100.5937;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;973.343,4.85298;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;ToonWater;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;2;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;32;0;33;0
WireConnection;4;0;5;0
WireConnection;28;1;32;0
WireConnection;31;0;28;0
WireConnection;31;1;4;0
WireConnection;31;2;30;0
WireConnection;1;0;31;0
WireConnection;1;2;11;0
WireConnection;39;0;38;0
WireConnection;41;0;39;0
WireConnection;41;1;36;0
WireConnection;3;1;1;0
WireConnection;37;0;49;0
WireConnection;37;1;3;0
WireConnection;37;2;41;0
WireConnection;0;2;37;0
ASEEND*/
//CHKSM=26037BD61753DF057600F60A13FBB5C3E0675FF8