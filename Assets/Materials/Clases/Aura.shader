// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Aura"
{
	Properties
	{
		_EdgeLength ( "Edge length", Range( 2, 50 ) ) = 15
		_Ciclos("Ciclos", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "Tessellation.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float _Ciclos;
		uniform float _EdgeLength;


		struct Gradient
		{
			int type;
			int colorsLength;
			int alphasLength;
			float4 colors[8];
			float2 alphas[8];
		};


		Gradient NewGradient(int type, int colorsLength, int alphasLength, 
		float4 colors0, float4 colors1, float4 colors2, float4 colors3, float4 colors4, float4 colors5, float4 colors6, float4 colors7,
		float2 alphas0, float2 alphas1, float2 alphas2, float2 alphas3, float2 alphas4, float2 alphas5, float2 alphas6, float2 alphas7)
		{
			Gradient g;
			g.type = type;
			g.colorsLength = colorsLength;
			g.alphasLength = alphasLength;
			g.colors[ 0 ] = colors0;
			g.colors[ 1 ] = colors1;
			g.colors[ 2 ] = colors2;
			g.colors[ 3 ] = colors3;
			g.colors[ 4 ] = colors4;
			g.colors[ 5 ] = colors5;
			g.colors[ 6 ] = colors6;
			g.colors[ 7 ] = colors7;
			g.alphas[ 0 ] = alphas0;
			g.alphas[ 1 ] = alphas1;
			g.alphas[ 2 ] = alphas2;
			g.alphas[ 3 ] = alphas3;
			g.alphas[ 4 ] = alphas4;
			g.alphas[ 5 ] = alphas5;
			g.alphas[ 6 ] = alphas6;
			g.alphas[ 7 ] = alphas7;
			return g;
		}


		float4 SampleGradient( Gradient gradient, float time )
		{
			float3 color = gradient.colors[0].rgb;
			UNITY_UNROLL
			for (int c = 1; c < 8; c++)
			{
			float colorPos = saturate((time - gradient.colors[c-1].w) / ( 0.00001 + (gradient.colors[c].w - gradient.colors[c-1].w)) * step(c, (float)gradient.colorsLength-1));
			color = lerp(color, gradient.colors[c].rgb, lerp(colorPos, step(0.01, colorPos), gradient.type));
			}
			#ifndef UNITY_COLORSPACE_GAMMA
			color = half3(GammaToLinearSpaceExact(color.r), GammaToLinearSpaceExact(color.g), GammaToLinearSpaceExact(color.b));
			#endif
			float alpha = gradient.alphas[0].x;
			UNITY_UNROLL
			for (int a = 1; a < 8; a++)
			{
			float alphaPos = saturate((time - gradient.alphas[a-1].y) / ( 0.00001 + (gradient.alphas[a].y - gradient.alphas[a-1].y)) * step(a, (float)gradient.alphasLength-1));
			alpha = lerp(alpha, gradient.alphas[a].x, lerp(alphaPos, step(0.01, alphaPos), gradient.type));
			}
			return float4(color, alpha);
		}


		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityEdgeLengthBasedTess (v0.vertex, v1.vertex, v2.vertex, _EdgeLength);
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float mulTime73 = _Time.y * 10.0;
			v.vertex.xyz += ( sin( ( ( v.texcoord.xy.x * _Ciclos * 6.28318548202515 ) + mulTime73 ) ) * float3(0,0,1) * ( 0.1 / 10.0 ) );
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			Gradient gradient6 = NewGradient( 0, 2, 2, float4( 1, 0.6666667, 0.8875985, 0 ), float4( 0.1800908, 0.4027694, 0.6698113, 1 ), 0, 0, 0, 0, 0, 0, float2( 1, 0 ), float2( 1, 1 ), 0, 0, 0, 0, 0, 0 );
			float4 color54 = IsGammaSpace() ? float4(0.4906105,0.8872277,0.990566,0) : float4(0.2054531,0.7623742,0.9786729,0);
			float temp_output_52_0 = ( ceil( ( 0.06010872 - ( 1.0 - i.uv_texcoord.y ) ) ) + ceil( ( 0.06010872 - i.uv_texcoord.y ) ) );
			float4 lerpResult72 = lerp( SampleGradient( gradient6, sin( ( i.uv_texcoord.x * 6.28318548202515 ) ) ) , color54 , temp_output_52_0);
			o.Emission = lerpResult72.rgb;
			o.Alpha = saturate( ( temp_output_52_0 + sin( ( i.uv_texcoord.y * 80.0 ) ) ) );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows exclude_path:deferred vertex:vertexDataFunc tessellate:tessFunction 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 4.6
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
				vertexDataFunc( v );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
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
0;499;1474;500;19.51416;-768.2456;1;True;True
Node;AmplifyShaderEditor.CommentaryNode;23;-1262.917,70.78799;Inherit;False;277;212;U =  Circunferencia  V = Altura;1;26;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;26;-1203.736,123.1986;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;55;-807.2076,-639.3948;Inherit;False;1520.776;623.8043;Borde;8;50;52;44;48;46;45;47;54;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;59;-505.8154,11.04146;Inherit;False;719.8484;308.3549;Rayado;3;58;57;56;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;45;-755.8132,-435.8751;Inherit;False;Constant;_Float0;Float 0;6;0;Create;True;0;0;0;False;0;False;0.06010872;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;47;-645.4432,-546.212;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;68;-591.4633,363.6454;Inherit;False;776.2494;376.1782;Gradiente alrededor del aura;3;63;64;67;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;82;-592.1818,776.1185;Inherit;False;1112.995;536.6358;MOVIMIENTO;7;81;73;78;76;75;77;79;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;57;-474.4706,129.6507;Inherit;False;Constant;_Float1;Float 1;6;0;Create;True;0;0;0;False;0;False;80;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TauNode;77;-522.2422,1040.587;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;78;-503.8226,1162.325;Inherit;False;Constant;_Speed;Speed;2;0;Create;True;0;0;0;False;0;False;10;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;48;-345.3175,-561.2618;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;46;-455.9707,-321.0777;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TauNode;67;-509.3923,477.4263;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;76;-548.217,917.0919;Inherit;False;Property;_Ciclos;Ciclos;5;0;Create;True;0;0;0;False;0;False;0;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;27;241.7537,362.4156;Inherit;False;684.7115;382.1973;Gradiente;3;7;8;6;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;-283.3915,65.76682;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;64;-253.4385,462.215;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CeilOpNode;50;-93.75944,-543.3911;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CeilOpNode;44;-32.926,-333.8869;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;75;-303.3049,872.7891;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;73;-197.3825,1085.916;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;58;40.92616,68.79034;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;86;496.3093,1123.251;Inherit;False;Constant;_Amplitud;Amplitud;2;0;Create;True;0;0;0;False;0;False;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;81;17.63576,866.9466;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;71;385.8689,-27.40509;Inherit;False;449.3384;270.7534;Uno Borde con Lineas;2;70;69;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SinOpNode;63;-32.89697,441.7576;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GradientNode;6;281.6703,422.5791;Inherit;False;0;2;2;1,0.6666667,0.8875985,0;0.1800908,0.4027694,0.6698113,1;1,0;1,1;0;1;OBJECT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;52;217.024,-354.1139;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GradientSampleNode;7;617.816,439.6082;Inherit;True;2;0;OBJECT;;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;69;409.1724,19.62864;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;85;522.2762,970.406;Inherit;False;Constant;_DirUp;Dir Up;2;0;Create;True;0;0;0;False;0;False;0,0,1;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ColorNode;54;522.4634,-508.9404;Inherit;False;Constant;_Color0;Color 0;6;0;Create;True;0;0;0;False;0;False;0.4906105,0.8872277,0.990566,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleDivideOpNode;87;731.8352,991.67;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;79;290.5324,851.6606;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;70;670.2073,22.59493;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;83;966.7599,859.411;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.FresnelNode;8;279.4986,517.5234;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;72;955.352,-266.1943;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1746.213,173.8769;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;Aura;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;6;0;-1;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;47;0;26;2
WireConnection;48;0;45;0
WireConnection;48;1;47;0
WireConnection;46;0;45;0
WireConnection;46;1;26;2
WireConnection;56;0;26;2
WireConnection;56;1;57;0
WireConnection;64;0;26;1
WireConnection;64;1;67;0
WireConnection;50;0;48;0
WireConnection;44;0;46;0
WireConnection;75;0;26;1
WireConnection;75;1;76;0
WireConnection;75;2;77;0
WireConnection;73;0;78;0
WireConnection;58;0;56;0
WireConnection;81;0;75;0
WireConnection;81;1;73;0
WireConnection;63;0;64;0
WireConnection;52;0;50;0
WireConnection;52;1;44;0
WireConnection;7;0;6;0
WireConnection;7;1;63;0
WireConnection;69;0;52;0
WireConnection;69;1;58;0
WireConnection;87;0;86;0
WireConnection;79;0;81;0
WireConnection;70;0;69;0
WireConnection;83;0;79;0
WireConnection;83;1;85;0
WireConnection;83;2;87;0
WireConnection;72;0;7;0
WireConnection;72;1;54;0
WireConnection;72;2;52;0
WireConnection;0;2;72;0
WireConnection;0;9;70;0
WireConnection;0;11;83;0
ASEEND*/
//CHKSM=639054531795C164C7483F6018697CDA43A8E5FF