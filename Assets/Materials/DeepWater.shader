// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "DeepWater"
{
	Properties
	{
		_DeepthScale("DeepthScale", Range( 0 , 1)) = 0.4114811
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float4 screenPos;
		};

		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _DeepthScale;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 color1 = IsGammaSpace() ? float4(0.2342916,0.6132076,0.5962049,0) : float4(0.04481259,0.3341808,0.3141351,0);
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth2 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth2 = abs( ( screenDepth2 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( 6.0 ) );
			float4 lerpResult70 = lerp( float4( 0,0,0,0 ) , color1 , pow( distanceDepth2 , _DeepthScale ));
			o.Albedo = lerpResult70.rgb;
			float screenDepth54 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth54 = abs( ( screenDepth54 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( 1.0 ) );
			float4 color5 = IsGammaSpace() ? float4(1,1,1,0) : float4(1,1,1,0);
			o.Emission = ( saturate( step( distanceDepth54 , 0.09762208 ) ) * color5 ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17200
0;633.1429;1783;520.4286;1044.809;190.1151;1.659065;True;True
Node;AmplifyShaderEditor.CommentaryNode;64;-167.2348,416.5443;Inherit;False;820.4596;351.5866;Foam;6;72;60;5;55;57;54;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;36;-573.3449,86.29198;Inherit;False;710.0369;251.6162;DepthFade;4;74;76;3;2;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;57;-145.4839,633.7835;Inherit;False;Constant;_Float0;Float 0;1;0;Create;True;0;0;False;0;0.09762208;0;0;4;0;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;54;-118.617,507.9369;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-544.783,145.892;Inherit;False;Constant;_DepthDistance;DepthDistance;0;0;Create;True;0;0;False;0;6;6.6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;55;157.4968,505.0641;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;2;-314.63,126.6953;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;76;-375.2441,234.1394;Inherit;False;Property;_DeepthScale;DeepthScale;0;0;Create;True;0;0;False;0;0.4114811;0.424;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;5;251.0087,589.7078;Inherit;False;Constant;_FoamColor;FoamColor ;1;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;72;325.0271,507.4262;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;1;-25.81469,-76.7986;Inherit;False;Constant;_WaterColor;WaterColor;0;0;Create;True;0;0;False;0;0.2342916,0.6132076,0.5962049,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;74;-42.2899,145.0268;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;70;297.6668,-93.12401;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;60;502.781,504.1232;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;789.0971,179.3746;Float;False;True;2;ASEMaterialInspector;0;0;Standard;DeepWater;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;55;0;54;0
WireConnection;55;1;57;0
WireConnection;2;0;3;0
WireConnection;72;0;55;0
WireConnection;74;0;2;0
WireConnection;74;1;76;0
WireConnection;70;1;1;0
WireConnection;70;2;74;0
WireConnection;60;0;72;0
WireConnection;60;1;5;0
WireConnection;0;0;70;0
WireConnection;0;2;60;0
ASEEND*/
//CHKSM=B089E0F596DC480242E68504151D6A8F44E81246