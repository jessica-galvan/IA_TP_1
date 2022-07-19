// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ToonLightning"
{
	Properties
	{
		_Step_1("Step_1", Range( 0 , 1)) = 0.9647059
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_ShadowIntesity0("ShadowIntesity 0", Range( 0 , 0.5)) = 0
		_Step_3("Step_3", Range( 0 , 1)) = 0.3810108
		_Step_3Intensity("Step_3: Intensity", Range( 0 , 1)) = 0.931218
		_Step_2("Step_2", Range( 0 , 1)) = 0.655049
		_Step_2Intensity("Step_2: Intensity", Range( 0 , 1)) = 0.6375453
		_HatchingMax("HatchingMax", Range( 0 , 1)) = 0.4662862
		_HatchingMin("HatchingMin", Range( 0 , 1)) = 0.712074
		_Texturee("Texturee", 2D) = "white" {}
		_HatchingIntensity("HatchingIntensity", Range( 0 , 1)) = 1
		_HatchingTiling("HatchingTiling", Range( 0 , 1)) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "UnityCG.cginc"
		#include "UnityShaderVariables.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float3 worldNormal;
			float3 worldPos;
			float2 uv_texcoord;
		};

		struct SurfaceOutputCustomLightingCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			Input SurfInput;
			UnityGIInput GIData;
		};

		uniform float _Step_1;
		uniform float _Step_2;
		uniform float _Step_2Intensity;
		uniform float _Step_3;
		uniform float _Step_3Intensity;
		uniform float _ShadowIntesity0;
		uniform sampler2D _Texturee;
		uniform float4 _Texturee_ST;
		uniform float _HatchingMin;
		uniform float _HatchingMax;
		uniform float _HatchingIntensity;
		uniform sampler2D _TextureSample0;
		uniform float _HatchingTiling;

		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			#ifdef UNITY_PASS_FORWARDBASE
			float ase_lightAtten = data.atten;
			if( _LightColor0.a == 0)
			ase_lightAtten = 0;
			#else
			float3 ase_lightAttenRGB = gi.light.color / ( ( _LightColor0.rgb ) + 0.000001 );
			float ase_lightAtten = max( max( ase_lightAttenRGB.r, ase_lightAttenRGB.g ), ase_lightAttenRGB.b );
			#endif
			#if defined(HANDLE_SHADOWS_BLENDING_IN_GI)
			half bakedAtten = UnitySampleBakedOcclusion(data.lightmapUV.xy, data.worldPos);
			float zDist = dot(_WorldSpaceCameraPos - data.worldPos, UNITY_MATRIX_V[2].xyz);
			float fadeDist = UnityComputeShadowFadeDistance(data.worldPos, zDist);
			ase_lightAtten = UnityMixRealtimeAndBakedShadows(data.atten, bakedAtten, UnityComputeShadowFade(fadeDist));
			#endif
			float3 ase_worldNormal = i.worldNormal;
			float3 ase_worldPos = i.worldPos;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float dotResult4 = dot( ase_worldNormal , ase_worldlightDir );
			float temp_output_6_0 = (dotResult4*0.5 + 0.5);
			float temp_output_18_0 = ( ( 1.0 - step( temp_output_6_0 , _Step_1 ) ) + saturate( ( ( 1.0 - step( temp_output_6_0 , _Step_2 ) ) - _Step_2Intensity ) ) + saturate( ( ( 1.0 - step( temp_output_6_0 , _Step_3 ) ) - _Step_3Intensity ) ) );
			float2 uv_Texturee = i.uv_texcoord * _Texturee_ST.xy + _Texturee_ST.zw;
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			float smoothstepResult30 = smoothstep( _HatchingMin , _HatchingMax , temp_output_6_0);
			float2 temp_cast_1 = (_HatchingTiling).xx;
			float2 uv_TexCoord36 = i.uv_texcoord * temp_cast_1;
			float4 temp_cast_2 = (( ( 1.0 - smoothstepResult30 ) * _HatchingIntensity * tex2D( _TextureSample0, uv_TexCoord36 ).r )).xxxx;
			c.rgb = ( ( saturate( ( saturate( temp_output_18_0 ) + saturate( ( ( 1.0 - ceil( temp_output_18_0 ) ) * _ShadowIntesity0 ) ) ) ) * tex2D( _Texturee, uv_Texturee ) * float4( ( ase_lightColor.rgb + ase_lightColor.a + saturate( ( ase_lightAtten + 0.0 ) ) ) , 0.0 ) ) - temp_cast_2 ).rgb;
			c.a = 1;
			return c;
		}

		inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
		{
			s.GIData = data;
		}

		void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
		{
			o.SurfInput = i;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardCustomLighting keepalpha fullforwardshadows 

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
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float3 worldNormal : TEXCOORD3;
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
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
				SurfaceOutputCustomLightingCustom o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputCustomLightingCustom, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
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
0;527;1474;472;778.5103;-446.8083;3.421853;True;True
Node;AmplifyShaderEditor.CommentaryNode;5;-1541.6,-86.67243;Inherit;False;614.6068;479.8558;Direccion de la Luz;3;2;3;4;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;9;-1023.722,469.2081;Inherit;False;588.7603;277.6256;Half Lambert;2;8;6;;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;3;-1493.738,125.9452;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldNormalVector;2;-1491.6,-36.67244;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.CommentaryNode;17;-99.0652,-417.8419;Inherit;False;875.4553;408.6612;Step 2;6;20;22;21;15;19;16;;1,1,1,1;0;0
Node;AmplifyShaderEditor.DotProductOpNode;4;-1169.645,75.46906;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;52;-215.2776,85.40456;Inherit;False;1026.145;414.8965;Step 3;6;56;57;58;55;54;53;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-991.7949,613.8237;Inherit;False;Constant;_HalfLambert;HalfLambert;0;0;Create;True;0;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;6;-793.9205,510.2239;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;57;-173.6725,224.6405;Inherit;False;Property;_Step_3;Step_3;3;0;Create;True;0;0;0;False;0;False;0.3810108;0.4185804;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-73.39206,-244.4455;Inherit;False;Property;_Step_2;Step_2;5;0;Create;True;0;0;0;False;0;False;0.655049;0.5942487;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;53;96.18672,132.3761;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;16;61.71013,-370.8703;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;14;29.29971,-915.4714;Inherit;False;588.7999;409.4;Step_1;3;13;10;11;;1,1,1,1;0;0
Node;AmplifyShaderEditor.OneMinusNode;54;250.431,141.6805;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;11;34.80459,-620.9416;Inherit;False;Property;_Step_1;Step_1;0;0;Create;True;0;0;0;False;0;False;0.9647059;0.9657854;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;58;84.35227,366.3813;Inherit;False;Property;_Step_3Intensity;Step_3: Intensity;4;0;Create;True;0;0;0;False;0;False;0.931218;0.6806892;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;15;49.87567,-136.8647;Inherit;False;Property;_Step_2Intensity;Step_2: Intensity;6;0;Create;True;0;0;0;False;0;False;0.6375453;0.9578906;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;19;215.9546,-361.5659;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;55;457.8647,227.4955;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;21;423.3888,-275.7507;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;10;166.3611,-862.1313;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;22;626.589,-256.5507;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;56;621.5158,246.6955;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;13;411.3345,-856.5841;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;18;880.0738,-396.264;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;29;1036.113,-139.9569;Inherit;False;672.5478;512.4604;Shadow Intensity;5;27;26;25;24;23;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CeilOpNode;23;1072.93,-61.7961;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;24;1253.182,-49.55478;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;46;1019.204,409.6816;Inherit;False;891.9639;414.0805;Color e intensity;5;40;44;43;42;41;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;26;1089.28,252.6815;Inherit;False;Property;_ShadowIntesity0;ShadowIntesity 0;2;0;Create;True;0;0;0;False;0;False;0;0;0;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.LightAttenuation;41;1058.163,565.1782;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;42;1074.806,699.7171;Inherit;False;Constant;_LightAttenuation;LightAttenuation;2;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;1357.659,108.9027;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;39;453.2571,888.4984;Inherit;False;1462.586;646.0863;HatchingEffect;9;38;37;36;35;34;32;31;30;59;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SaturateNode;50;1504.531,-365.9919;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;510.1463,958.1303;Inherit;False;Property;_HatchingMin;HatchingMin;8;0;Create;True;0;0;0;False;0;False;0.712074;0.344;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;43;1370.853,642.6118;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;32;502.5287,1050.026;Inherit;False;Property;_HatchingMax;HatchingMax;7;0;Create;True;0;0;0;False;0;False;0.4662862;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;37;535.3494,1355.114;Inherit;False;Property;_HatchingTiling;HatchingTiling;11;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;27;1526.315,78.84715;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;30;900.7292,1025.234;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LightColorNode;40;1281.261,453.0496;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleAddOpNode;49;1885.23,-90.65571;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;36;876.0379,1348.851;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;44;1561.706,597.5285;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;45;1737.751,496.5387;Inherit;False;3;3;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OneMinusNode;59;1228.322,1035.331;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;38;1170.63,1141.13;Inherit;False;Property;_HatchingIntensity;HatchingIntensity;10;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;48;1826.541,164.2377;Inherit;True;Property;_Texturee;Texturee;9;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;51;2119.541,19.55531;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;35;1192.838,1302.452;Inherit;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;0;False;0;False;-1;None;0e68e1ab5a61e904bb0b2dfc0a3812e9;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;2331.349,171.9278;Inherit;True;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;1523.32,1019.685;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;60;2581.692,355.3573;Inherit;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2874.682,-9.270838;Float;False;True;-1;2;ASEMaterialInspector;0;0;CustomLighting;ToonLightning;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;4;0;2;0
WireConnection;4;1;3;0
WireConnection;6;0;4;0
WireConnection;6;1;8;0
WireConnection;6;2;8;0
WireConnection;53;0;6;0
WireConnection;53;1;57;0
WireConnection;16;0;6;0
WireConnection;16;1;20;0
WireConnection;54;0;53;0
WireConnection;19;0;16;0
WireConnection;55;0;54;0
WireConnection;55;1;58;0
WireConnection;21;0;19;0
WireConnection;21;1;15;0
WireConnection;10;0;6;0
WireConnection;10;1;11;0
WireConnection;22;0;21;0
WireConnection;56;0;55;0
WireConnection;13;0;10;0
WireConnection;18;0;13;0
WireConnection;18;1;22;0
WireConnection;18;2;56;0
WireConnection;23;0;18;0
WireConnection;24;0;23;0
WireConnection;25;0;24;0
WireConnection;25;1;26;0
WireConnection;50;0;18;0
WireConnection;43;0;41;0
WireConnection;43;1;42;0
WireConnection;27;0;25;0
WireConnection;30;0;6;0
WireConnection;30;1;31;0
WireConnection;30;2;32;0
WireConnection;49;0;50;0
WireConnection;49;1;27;0
WireConnection;36;0;37;0
WireConnection;44;0;43;0
WireConnection;45;0;40;1
WireConnection;45;1;40;2
WireConnection;45;2;44;0
WireConnection;59;0;30;0
WireConnection;51;0;49;0
WireConnection;35;1;36;0
WireConnection;47;0;51;0
WireConnection;47;1;48;0
WireConnection;47;2;45;0
WireConnection;34;0;59;0
WireConnection;34;1;38;0
WireConnection;34;2;35;1
WireConnection;60;0;47;0
WireConnection;60;1;34;0
WireConnection;0;13;60;0
ASEEND*/
//CHKSM=F138CB7E117F28AE733E497438D21513D89EA6DE