// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ForceShield"
{
	Properties
	{
		_DepthDistance("DepthDistance", Range( 0 , 1)) = 0
		_Opacity("Opacity", Range( 0 , 1)) = 0.4618868
		_Tiling("Tiling", Range( 0 , 2)) = 3.451827
		_ShieldColor("ShieldColor", Color) = (0.1241545,0.3777947,0.5849056,0)
		_Scale("Scale", Range( 0 , 5)) = 0.4319889
		_FallOff("FallOff", Range( 0 , 20)) = 0
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_FresnelScale("FresnelScale", Range( 0 , 1)) = 0.1031419
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGINCLUDE
		#include "UnityCG.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
			float4 screenPos;
			float3 worldPos;
			float3 worldNormal;
			float3 viewDir;
		};

		uniform sampler2D _TextureSample0;
		uniform float _Tiling;
		uniform float _Opacity;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _DepthDistance;
		uniform float _Scale;
		uniform float _FallOff;
		uniform float _FresnelScale;
		uniform float4 _ShieldColor;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 color17 = IsGammaSpace() ? float4(0.6556604,1,0.9756937,0) : float4(0.387421,1,0.945595,0);
			float2 temp_cast_0 = (_Tiling).xx;
			float2 uv_TexCoord32 = i.uv_texcoord * temp_cast_0;
			float4 tex2DNode14 = tex2D( _TextureSample0, uv_TexCoord32 );
			float4 temp_output_18_0 = ( color17 * tex2DNode14 );
			float4 color21 = IsGammaSpace() ? float4(0.4469117,0.6307652,0.7830189,1) : float4(0.1681511,0.3556438,0.5754442,1);
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth1 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth1 = saturate( abs( ( screenDepth1 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _DepthDistance ) ) );
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV28 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode28 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV28, 5.0 ) );
			float fresnelNdotV22 = dot( ase_worldNormal, -i.viewDir );
			float fresnelNode22 = ( 0.0 + _FresnelScale * pow( 1.0 - fresnelNdotV22, 5.0 ) );
			float temp_output_27_0 = ( saturate( fresnelNode28 ) * fresnelNode22 );
			float temp_output_6_0 = ( _Opacity + pow( ( ( 1.0 - distanceDepth1 ) * _Scale ) , _FallOff ) + temp_output_27_0 );
			float4 lerpResult34 = lerp( temp_output_18_0 , color21 , temp_output_6_0);
			float4 lerpResult20 = lerp( ( temp_output_18_0 + ( _ShieldColor * ( 1.0 - tex2DNode14 ) ) ) , color21 , temp_output_6_0);
			o.Emission = ( lerpResult34 + temp_output_27_0 + lerpResult20 ).rgb;
			o.Alpha = saturate( temp_output_6_0 );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float4 screenPos : TEXCOORD3;
				float3 worldNormal : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.screenPos = ComputeScreenPos( o.pos );
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.viewDir = worldViewDir;
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
				surfIN.screenPos = IN.screenPos;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
0;585.7143;1684;567.8571;1279.557;598.546;2.077588;True;True
Node;AmplifyShaderEditor.CommentaryNode;37;-1124.939,294.0425;Inherit;False;1722.334;424.9343;Depth;10;2;3;6;7;10;11;12;8;13;1;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;38;-2039.038,-1284.743;Inherit;False;2836.759;705.6949;Hexagonal Texture;9;19;18;17;15;5;16;14;32;33;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;2;-1062.482,438.0511;Inherit;False;Property;_DepthDistance;DepthDistance;0;0;Create;True;0;0;0;False;0;False;0;0.583;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;35;-1160.642,-411.9482;Inherit;False;878.629;477.7963;Fresnel;7;31;26;22;27;29;28;25;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-1989.232,-1169.533;Inherit;False;Property;_Tiling;Tiling;2;0;Create;True;0;0;0;False;0;False;3.451827;1.46;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;1;-776.0052,331.8879;Inherit;False;True;True;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;25;-1139.165,-206.1718;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TextureCoordinatesNode;32;-1648.68,-1186.511;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;14;-1341.096,-986.4036;Inherit;True;Property;_TextureSample0;Texture Sample 0;6;0;Create;True;0;0;0;False;0;False;-1;None;5a584b56505218940a9bcf0ce87bdb09;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NegateNode;26;-895.287,-157.6129;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-1088.118,-31.4999;Inherit;False;Property;_FresnelScale;FresnelScale;7;0;Create;True;0;0;0;False;0;False;0.1031419;0.644;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;28;-943.351,-356.7239;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-532.6539,496.949;Inherit;False;Property;_Scale;Scale;4;0;Create;True;0;0;0;False;0;False;0.4319889;1.26;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;8;-495.5583,359.0025;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;16;-872.3666,-743.0201;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-371.8535,606.4488;Inherit;False;Property;_FallOff;FallOff;5;0;Create;True;0;0;0;False;0;False;0;8.2;0;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;22;-710.6559,-166.8135;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-209.2536,406.549;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;29;-661.9028,-312.7347;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;5;-715.1929,-915.0073;Inherit;False;Property;_ShieldColor;ShieldColor;3;0;Create;True;0;0;0;False;0;False;0.1241545,0.3777947,0.5849056,0;0.1241545,0.3777947,0.5849056,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;17;-680.7253,-1224.974;Inherit;False;Constant;_Color0;Color 0;6;0;Create;True;0;0;0;False;0;False;0.6556604,1,0.9756937,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-427.3411,-809.3609;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-264.7528,-1078.633;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;39;-93.91092,-433.7458;Inherit;False;1128.686;487.7355;White Border when in contact with floor;3;21;20;34;;1,1,1,1;0;0
Node;AmplifyShaderEditor.PowerNode;10;-54.55334,492.349;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-451.3568,-209.9355;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-42.14983,352.5715;Inherit;False;Property;_Opacity;Opacity;1;0;Create;True;0;0;0;False;0;False;0.4618868;0.332;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;21;178.0485,-327.9664;Inherit;False;Constant;_Color1;Color 1;6;0;Create;True;0;0;0;False;0;False;0.4469117,0.6307652,0.7830189,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;19;325.2043,-975.6653;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;6;282.1998,443.9868;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;34;656.8102,-350.0645;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;20;780.9429,-113.1522;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;24;1156.551,-235.4783;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;3;438.5356,464.7579;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1498.151,-213.1616;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;ForceShield;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;1;0;2;0
WireConnection;32;0;33;0
WireConnection;14;1;32;0
WireConnection;26;0;25;0
WireConnection;8;0;1;0
WireConnection;16;0;14;0
WireConnection;22;4;26;0
WireConnection;22;2;31;0
WireConnection;12;0;8;0
WireConnection;12;1;13;0
WireConnection;29;0;28;0
WireConnection;15;0;5;0
WireConnection;15;1;16;0
WireConnection;18;0;17;0
WireConnection;18;1;14;0
WireConnection;10;0;12;0
WireConnection;10;1;11;0
WireConnection;27;0;29;0
WireConnection;27;1;22;0
WireConnection;19;0;18;0
WireConnection;19;1;15;0
WireConnection;6;0;7;0
WireConnection;6;1;10;0
WireConnection;6;2;27;0
WireConnection;34;0;18;0
WireConnection;34;1;21;0
WireConnection;34;2;6;0
WireConnection;20;0;19;0
WireConnection;20;1;21;0
WireConnection;20;2;6;0
WireConnection;24;0;34;0
WireConnection;24;1;27;0
WireConnection;24;2;20;0
WireConnection;3;0;6;0
WireConnection;0;2;24;0
WireConnection;0;9;3;0
ASEEND*/
//CHKSM=DB7BC7E5C5BAFA545FAE6BD35BE9944704BF8FB7