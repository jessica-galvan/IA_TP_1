// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SecurityCam"
{
	Properties
	{
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Alpha("Alpha", Range( 0 , 1)) = 0
		_Noise("Noise", Float) = 5
		_NoiseIntensity("NoiseIntensity", Range( 0 , 1)) = 0.4160894
		_ScreenResolution("ScreenResolution", Vector) = (16,9,0,0)

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Opaque" }
	LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend Off
		AlphaToMask Off
		Cull Back
		ColorMask RGBA
		ZWrite On
		ZTest LEqual
		Offset 0 , 0
		
		
		
		Pass
		{
			Name "Unlit"
			Tags { "LightMode"="ForwardBase" }
			CGPROGRAM

			

			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			//only defining to not throw compilation error over Unity 5.5
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"


			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 worldPos : TEXCOORD0;
				#endif
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform sampler2D _TextureSample0;
			uniform float _Alpha;
			uniform float _Noise;
			uniform float2 _ScreenResolution;
			uniform float _NoiseIntensity;
			float3 mod3D289( float3 x ) { return x - floor( x / 289.0 ) * 289.0; }
			float4 mod3D289( float4 x ) { return x - floor( x / 289.0 ) * 289.0; }
			float4 permute( float4 x ) { return mod3D289( ( x * 34.0 + 1.0 ) * x ); }
			float4 taylorInvSqrt( float4 r ) { return 1.79284291400159 - r * 0.85373472095314; }
			float snoise( float3 v )
			{
				const float2 C = float2( 1.0 / 6.0, 1.0 / 3.0 );
				float3 i = floor( v + dot( v, C.yyy ) );
				float3 x0 = v - i + dot( i, C.xxx );
				float3 g = step( x0.yzx, x0.xyz );
				float3 l = 1.0 - g;
				float3 i1 = min( g.xyz, l.zxy );
				float3 i2 = max( g.xyz, l.zxy );
				float3 x1 = x0 - i1 + C.xxx;
				float3 x2 = x0 - i2 + C.yyy;
				float3 x3 = x0 - 0.5;
				i = mod3D289( i);
				float4 p = permute( permute( permute( i.z + float4( 0.0, i1.z, i2.z, 1.0 ) ) + i.y + float4( 0.0, i1.y, i2.y, 1.0 ) ) + i.x + float4( 0.0, i1.x, i2.x, 1.0 ) );
				float4 j = p - 49.0 * floor( p / 49.0 );  // mod(p,7*7)
				float4 x_ = floor( j / 7.0 );
				float4 y_ = floor( j - 7.0 * x_ );  // mod(j,N)
				float4 x = ( x_ * 2.0 + 0.5 ) / 7.0 - 1.0;
				float4 y = ( y_ * 2.0 + 0.5 ) / 7.0 - 1.0;
				float4 h = 1.0 - abs( x ) - abs( y );
				float4 b0 = float4( x.xy, y.xy );
				float4 b1 = float4( x.zw, y.zw );
				float4 s0 = floor( b0 ) * 2.0 + 1.0;
				float4 s1 = floor( b1 ) * 2.0 + 1.0;
				float4 sh = -step( h, 0.0 );
				float4 a0 = b0.xzyw + s0.xzyw * sh.xxyy;
				float4 a1 = b1.xzyw + s1.xzyw * sh.zzww;
				float3 g0 = float3( a0.xy, h.x );
				float3 g1 = float3( a0.zw, h.y );
				float3 g2 = float3( a1.xy, h.z );
				float3 g3 = float3( a1.zw, h.w );
				float4 norm = taylorInvSqrt( float4( dot( g0, g0 ), dot( g1, g1 ), dot( g2, g2 ), dot( g3, g3 ) ) );
				g0 *= norm.x;
				g1 *= norm.y;
				g2 *= norm.z;
				g3 *= norm.w;
				float4 m = max( 0.6 - float4( dot( x0, x0 ), dot( x1, x1 ), dot( x2, x2 ), dot( x3, x3 ) ), 0.0 );
				m = m* m;
				m = m* m;
				float4 px = float4( dot( x0, g0 ), dot( x1, g1 ), dot( x2, g2 ), dot( x3, g3 ) );
				return 42.0 * dot( m, px);
			}
			

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				o.ase_texcoord1.xy = v.ase_texcoord.xy;
				o.ase_texcoord2 = v.vertex;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.zw = 0;
				float3 vertexValue = float3(0, 0, 0);
				#if ASE_ABSOLUTE_VERTEX_POS
				vertexValue = v.vertex.xyz;
				#endif
				vertexValue = vertexValue;
				#if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);

				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				#endif
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 finalColor;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 WorldPosition = i.worldPos;
				#endif
				float2 texCoord70 = i.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float4 tex2DNode1 = tex2D( _TextureSample0, ( ceil( ( texCoord70 * 65.30461 ) ) / 65.30461 ) );
				float4 color5 = IsGammaSpace() ? float4(0.2428572,1,0,1) : float4(0.04806329,1,0,1);
				float4 lerpResult6 = lerp( tex2DNode1 , ( tex2DNode1 * color5 ) , _Alpha);
				float4 appendResult66 = (float4(_ScreenResolution.x , 0.0 , _ScreenResolution.y , 0.0));
				float mulTime46 = _Time.y * 6.0;
				float simplePerlin3D40 = snoise( ( ( float4( i.ase_texcoord2.xyz , 0.0 ) * _Noise * appendResult66 ) + float4( ( mulTime46 * float3(0,1,0) ) , 0.0 ) ).xyz );
				simplePerlin3D40 = simplePerlin3D40*0.5 + 0.5;
				float4 temp_cast_3 = (( simplePerlin3D40 * _NoiseIntensity )).xxxx;
				
				
				finalColor = ( ( lerpResult6 - temp_cast_3 ) - float4( 0,0,0,0 ) );
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18900
0;514;1474;485;2266.535;-84.86546;1.708219;True;True
Node;AmplifyShaderEditor.CommentaryNode;69;-1690.877,28.74746;Inherit;False;1135.165;572.2938;Pixel Effect;5;77;76;75;74;70;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;70;-1621.876,79.71202;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;74;-1547.512,356.8529;Inherit;True;Constant;_Float0;Float 0;8;0;Create;True;0;0;0;False;0;False;65.30461;0;0;200;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;64;-937.7416,676.8224;Inherit;False;1815.373;785.7371;Noise;12;43;46;45;60;40;44;38;37;39;59;66;68;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;75;-1202.834,112.1179;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;68;-860.4346,986.3041;Inherit;True;Property;_ScreenResolution;ScreenResolution;7;0;Create;True;0;0;0;False;0;False;16,9;16,9;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PosVertexDataNode;37;-838.4965,741.4412;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;39;-799.9626,884.7979;Inherit;False;Property;_Noise;Noise;5;0;Create;True;0;0;0;False;0;False;5;2.16;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CeilOpNode;76;-941.5402,111.3389;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;66;-620.3795,951.338;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.Vector3Node;43;-592.4163,1273.782;Inherit;False;Constant;_ChangeInY;ChangeInY;6;0;Create;True;0;0;0;False;0;False;0,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleTimeNode;46;-544.5012,1171.369;Inherit;False;1;0;FLOAT;6;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;-555.5608,734.0762;Inherit;False;3;3;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;45;-252.4437,1100.02;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;77;-769.4055,107.9958;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;65;-503.2794,18.07066;Inherit;False;1027.158;603.4641;Tint;5;1;6;7;2;5;;1,1,1,1;0;0
Node;AmplifyShaderEditor.ColorNode;5;-401.0833,358.2808;Inherit;False;Constant;_Color0;Color 0;1;0;Create;True;0;0;0;False;0;False;0.2428572,1,0,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-451.4322,108.9801;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;8fc5c195eea2064479e4542bafaa58ae;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;44;-338.2347,844.1691;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-137.4211,525.3511;Inherit;False;Property;_Alpha;Alpha;1;0;Create;True;0;0;0;False;0;False;0;0.126;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;40;-130.5325,744.1592;Inherit;True;Simplex3D;True;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;60;214.8397,887.6645;Inherit;False;Property;_NoiseIntensity;NoiseIntensity;6;0;Create;True;0;0;0;False;0;False;0.4160894;0.259;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;2;-118.9413,272.3476;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;6;230.0085,199.6873;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;59;632.1393,767.9495;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;62;867.8451,449.5246;Inherit;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;14;1682.481,1346.718;Inherit;False;2934.143;1595.937;Noise;21;33;9;20;19;27;23;18;26;15;22;17;10;24;25;28;12;21;11;13;16;35;;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector2Node;16;2362.328,1940.664;Inherit;False;Property;_Speed;Speed;4;0;Create;True;0;0;0;False;0;False;0,0;5,5;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.NegateNode;17;2867.788,1898.539;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;23;3312.898,2041.744;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;20;3773.188,1668.711;Inherit;True;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;2216.846,1775.678;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SaturateNode;35;4202.727,1476.094;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;24;2791.427,2254.528;Inherit;True;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;25;2775.262,2584.321;Inherit;True;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;63;1056.202,451.1647;Inherit;False;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;19;3308.655,1788.666;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;18;3005.388,1893.74;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;33;3935.39,1427.149;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;10;2376.857,1744.538;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;11;1897.29,1779.926;Inherit;False;Property;_NoiseTIling;NoiseTIling;2;0;Create;True;0;0;0;False;0;False;99.99999;15.5;0;25;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;22;3009.63,2146.818;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;13;1892.568,1894.004;Inherit;False;Property;_ScreenProportion;ScreenProportion;3;0;Create;True;0;0;0;False;0;False;16,9;16,9;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.NoiseGeneratorNode;9;3398.204,1451.831;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;27;3296.733,2371.537;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;15;2978.304,1611.84;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NegateNode;21;2641.769,2302.369;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NegateNode;28;2608.339,2636.765;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;26;2993.466,2476.611;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;1341.609,361.0521;Float;False;True;-1;2;ASEMaterialInspector;100;1;SecurityCam;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;False;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque=RenderType;True;2;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;False;0
WireConnection;75;0;70;0
WireConnection;75;1;74;0
WireConnection;76;0;75;0
WireConnection;66;0;68;1
WireConnection;66;2;68;2
WireConnection;38;0;37;0
WireConnection;38;1;39;0
WireConnection;38;2;66;0
WireConnection;45;0;46;0
WireConnection;45;1;43;0
WireConnection;77;0;76;0
WireConnection;77;1;74;0
WireConnection;1;1;77;0
WireConnection;44;0;38;0
WireConnection;44;1;45;0
WireConnection;40;0;44;0
WireConnection;2;0;1;0
WireConnection;2;1;5;0
WireConnection;6;0;1;0
WireConnection;6;1;2;0
WireConnection;6;2;7;0
WireConnection;59;0;40;0
WireConnection;59;1;60;0
WireConnection;62;0;6;0
WireConnection;62;1;59;0
WireConnection;17;0;16;0
WireConnection;23;0;22;0
WireConnection;20;0;9;0
WireConnection;20;1;19;0
WireConnection;20;2;23;0
WireConnection;20;3;27;0
WireConnection;12;0;11;0
WireConnection;12;1;13;0
WireConnection;35;0;33;0
WireConnection;24;0;16;1
WireConnection;24;1;21;0
WireConnection;25;0;28;0
WireConnection;25;1;16;2
WireConnection;63;0;62;0
WireConnection;19;0;18;0
WireConnection;18;0;10;0
WireConnection;18;2;17;0
WireConnection;33;0;20;0
WireConnection;10;0;12;0
WireConnection;22;0;10;0
WireConnection;22;2;24;0
WireConnection;9;0;15;0
WireConnection;27;0;26;0
WireConnection;15;0;10;0
WireConnection;15;2;16;0
WireConnection;21;0;16;2
WireConnection;28;0;16;1
WireConnection;26;0;10;0
WireConnection;26;2;25;0
WireConnection;0;0;63;0
ASEEND*/
//CHKSM=56BA64AE490C798718C4FDB6DE25116AE5A6C9D5