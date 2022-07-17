// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ToonWater"
{
	Properties
	{
		_Water("Water", 2D) = "white" {}
		_WaterTiling("WaterTiling", Range( 0 , 10)) = 3.451827
		_FlowTiling("FlowTiling", Range( 0 , 1)) = 3.451827
		_DepthAmount("DepthAmount", Range( 0 , 1)) = 0
		_WaterSpeed("WaterSpeed", Vector) = (0,0,0,0)
		_Whiteness("Whiteness", Range( 0 , 1)) = 0
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_Distortion("Distortion", Range( 0 , 1)) = 0.1624063
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
			float4 screenPosition17;
		};

		uniform sampler2D _Water;
		uniform float2 _WaterSpeed;
		uniform sampler2D _TextureSample1;
		uniform float _FlowTiling;
		uniform float _WaterTiling;
		uniform float _Distortion;
		uniform float _Whiteness;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _DepthAmount;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float3 vertexPos17 = ase_vertex3Pos;
			float4 ase_screenPos17 = ComputeScreenPos( UnityObjectToClipPos( vertexPos17 ) );
			o.screenPosition17 = ase_screenPos17;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 temp_cast_0 = (_FlowTiling).xx;
			float2 uv_TexCoord32 = i.uv_texcoord * temp_cast_0;
			float2 temp_cast_1 = (_WaterTiling).xx;
			float2 uv_TexCoord4 = i.uv_texcoord * temp_cast_1;
			float4 lerpResult31 = lerp( tex2D( _TextureSample1, uv_TexCoord32 ) , float4( uv_TexCoord4, 0.0 , 0.0 ) , _Distortion);
			float2 panner1 = ( 1.0 * _Time.y * _WaterSpeed + lerpResult31.rg);
			float4 ase_screenPos17 = i.screenPosition17;
			float4 ase_screenPosNorm17 = ase_screenPos17 / ase_screenPos17.w;
			ase_screenPosNorm17.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm17.z : ase_screenPosNorm17.z * 0.5 + 0.5;
			float screenDepth17 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm17.xy ));
			float distanceDepth17 = saturate( abs( ( screenDepth17 - LinearEyeDepth( ase_screenPosNorm17.z ) ) / ( _DepthAmount ) ) );
			o.Albedo = ( tex2D( _Water, panner1 ) + ( _Whiteness * ( 1.0 - distanceDepth17 ) ) ).rgb;
			o.Alpha = distanceDepth17;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows exclude_path:deferred vertex:vertexDataFunc 

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
				float4 customPack2 : TEXCOORD2;
				float3 worldPos : TEXCOORD3;
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
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.customPack2.xyzw = customInputData.screenPosition17;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
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
				surfIN.screenPosition17 = IN.customPack2.xyzw;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
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
0;501.1429;1680;652.4286;1174.493;307.9385;1.750174;False;False
Node;AmplifyShaderEditor.CommentaryNode;29;-1505.703,-755.5955;Inherit;False;1566.738;382.0203;Water Wave Flow Effect;5;30;33;31;28;32;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-1471.928,-687.1552;Inherit;False;Property;_FlowTiling;FlowTiling;3;0;Create;True;0;0;0;False;0;False;3.451827;0.11272;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;9;-947.5816,-354.1546;Inherit;False;952.5812;415.8426;Movement;4;1;4;5;11;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;32;-1115.544,-721.3305;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;5;-895.6265,-228.1962;Inherit;False;Property;_WaterTiling;WaterTiling;2;0;Create;True;0;0;0;False;0;False;3.451827;5.291652;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;14;-1109.232,855.1113;Inherit;False;695.065;357.6429;Depth (Saturate DepthFade!);3;17;16;15;;1,1,1,1;0;0
Node;AmplifyShaderEditor.PosVertexDataNode;15;-1057.023,902.5995;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;16;-1071.762,1079.328;Inherit;False;Property;_DepthAmount;DepthAmount;5;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;4;-538.5564,-255.8112;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;28;-779.5032,-705.5955;Inherit;True;Property;_TextureSample1;Texture Sample 1;8;0;Create;True;0;0;0;False;0;False;-1;None;94c976211f7b99b46a309c1024cae8a2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;30;-731.6191,-499.9263;Inherit;False;Property;_Distortion;Distortion;9;0;Create;True;0;0;0;False;0;False;0.1624063;0.5386284;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;31;-377.5607,-641.4687;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector2Node;11;-521.1016,-114.5178;Inherit;False;Property;_WaterSpeed;WaterSpeed;6;0;Create;True;0;0;0;False;0;False;0,0;0.05,0.07;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.DepthFade;17;-822.3928,949.1282;Inherit;False;True;True;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;24;137.6893,782.348;Inherit;False;564.7352;220.2738;Whiteness at the end effect;3;21;23;20;;1,1,1,1;0;0
Node;AmplifyShaderEditor.PannerNode;1;-81.6989,-190.3851;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;21;278.991,923.4808;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;23;190.0491,818.1897;Inherit;False;Property;_Whiteness;Whiteness;7;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;42;-612.295,149.4693;Inherit;False;820.4596;351.5866;Foam;6;48;47;46;45;44;43;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;3;178.0255,-254.0681;Inherit;True;Property;_Water;Water;0;0;Create;True;0;0;0;False;0;False;-1;None;3394991baf929de4b9e2a158da89e970;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;520.2285,838.0089;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;10;-631.1218,1134.234;Inherit;False;695.065;357.6429;Depth (Saturate DepthFade!);4;7;2;8;13;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;35;832.8561,-669.7817;Inherit;False;710.0369;251.6162;DepthFade;4;41;39;38;36;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;43;-590.544,366.7087;Inherit;False;Constant;_Float1;Float 0;1;0;Create;True;0;0;0;False;0;False;0.09762208;0;0;4;0;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;2;-344.283,1228.25;Inherit;False;True;True;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;22;784.0455,-171.6001;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.PosVertexDataNode;8;-578.9122,1181.722;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;13;-85.21982,1245.89;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;48;57.72089,237.0484;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;36;1030.957,-521.934;Inherit;False;Property;_DeepthScale1;DeepthScale;1;0;Create;True;0;0;0;False;0;False;0.4114811;0.424;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;39;1091.571,-629.3784;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;37;1654.222,-734.085;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;41;1363.911,-611.047;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;44;-563.6771,240.8621;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;46;-194.0513,322.6331;Inherit;False;Constant;_FoamColor1;FoamColor ;1;0;Create;True;0;0;0;False;0;False;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;47;-120.033,240.3514;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;45;-287.5632,237.9893;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-593.6512,1358.45;Inherit;False;Property;_DepthDistance;DepthDistance;4;0;Create;True;0;0;0;False;0;False;0;0.601;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;38;861.4181,-610.1818;Inherit;False;Constant;_DepthDistance1;DepthDistance;0;0;Create;True;0;0;0;False;0;False;6;6.6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;973.343,4.85298;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;ToonWater;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;0;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;32;0;33;0
WireConnection;4;0;5;0
WireConnection;28;1;32;0
WireConnection;31;0;28;0
WireConnection;31;1;4;0
WireConnection;31;2;30;0
WireConnection;17;1;15;0
WireConnection;17;0;16;0
WireConnection;1;0;31;0
WireConnection;1;2;11;0
WireConnection;21;0;17;0
WireConnection;3;1;1;0
WireConnection;20;0;23;0
WireConnection;20;1;21;0
WireConnection;2;1;8;0
WireConnection;2;0;7;0
WireConnection;22;0;3;0
WireConnection;22;1;20;0
WireConnection;13;0;2;0
WireConnection;48;0;47;0
WireConnection;48;1;46;0
WireConnection;39;0;38;0
WireConnection;37;2;41;0
WireConnection;41;0;39;0
WireConnection;41;1;36;0
WireConnection;47;0;45;0
WireConnection;45;0;44;0
WireConnection;45;1;43;0
WireConnection;0;0;22;0
WireConnection;0;9;17;0
ASEEND*/
//CHKSM=1AE3DF165EA5BA929C86F7E1119F27F0EBDC3451