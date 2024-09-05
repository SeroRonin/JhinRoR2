// Upgrade NOTE: upgraded instancing buffer 'SeroRoninVFXjhinVFX_UniversalShader' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SeroRonin/VFX/jhinVFX_UniversalShader"
{
	Properties
	{
		_Texture0("Texture 0", 2D) = "white" {}
		_AlphaUVMask("Alpha UV Mask", 2D) = "white" {}
		_NumCells("Num Cells", Vector) = (1,1,0,0)
		_SelectFrame("Select Frame", Vector) = (1,1,0,0)
		_EmissionScale("Emission Scale", Float) = 1
		[Header(Backface)]_BackfaceColorOptions("Use Backface | Hue Shift | Force Color", Vector) = (0,180,0,0)
		_BackfaceColor("Backface Color", Color) = (0,0,0,0)
		[Header(Vertex Stream (Particles))]_TexRGBWeight("TexRGB Weight", Range( 0 , 1)) = 1
		_UseAnimatedOffset("Use Animated Offset", Int) = 0
		_UseOpacityCurve("Use Opacity Curve", Int) = 0
		_UseRandomFrame("Use Random Frame", Int) = 0
		_UseAlphaCutoff("Use Alpha Cutoff", Int) = 0
		[HideInInspector] _texcoord4( "", 2D ) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] _texcoord3( "", 2D ) = "white" {}
		[HideInInspector] _texcoord2( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.5
		#pragma multi_compile_instancing
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			half ASEVFace : VFACE;
			float4 vertexColor : COLOR;
			float4 uv3_texcoord3;
			float2 uv_texcoord;
			float4 uv2_texcoord2;
			float4 uv4_texcoord4;
		};

		uniform float4 _BackfaceColorOptions;
		uniform sampler2D _Texture0;
		uniform sampler2D _AlphaUVMask;

		UNITY_INSTANCING_BUFFER_START(SeroRoninVFXjhinVFX_UniversalShader)
			UNITY_DEFINE_INSTANCED_PROP(float4, _BackfaceColor)
#define _BackfaceColor_arr SeroRoninVFXjhinVFX_UniversalShader
			UNITY_DEFINE_INSTANCED_PROP(float4, _AlphaUVMask_ST)
#define _AlphaUVMask_ST_arr SeroRoninVFXjhinVFX_UniversalShader
			UNITY_DEFINE_INSTANCED_PROP(float2, _NumCells)
#define _NumCells_arr SeroRoninVFXjhinVFX_UniversalShader
			UNITY_DEFINE_INSTANCED_PROP(float2, _SelectFrame)
#define _SelectFrame_arr SeroRoninVFXjhinVFX_UniversalShader
			UNITY_DEFINE_INSTANCED_PROP(float, _TexRGBWeight)
#define _TexRGBWeight_arr SeroRoninVFXjhinVFX_UniversalShader
			UNITY_DEFINE_INSTANCED_PROP(float, _EmissionScale)
#define _EmissionScale_arr SeroRoninVFXjhinVFX_UniversalShader
			UNITY_DEFINE_INSTANCED_PROP(int, _UseRandomFrame)
#define _UseRandomFrame_arr SeroRoninVFXjhinVFX_UniversalShader
			UNITY_DEFINE_INSTANCED_PROP(int, _UseAnimatedOffset)
#define _UseAnimatedOffset_arr SeroRoninVFXjhinVFX_UniversalShader
			UNITY_DEFINE_INSTANCED_PROP(int, _UseAlphaCutoff)
#define _UseAlphaCutoff_arr SeroRoninVFXjhinVFX_UniversalShader
			UNITY_DEFINE_INSTANCED_PROP(int, _UseOpacityCurve)
#define _UseOpacityCurve_arr SeroRoninVFXjhinVFX_UniversalShader
		UNITY_INSTANCING_BUFFER_END(SeroRoninVFXjhinVFX_UniversalShader)


		float3 HSVToRGB( float3 c )
		{
			float4 K = float4( 1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0 );
			float3 p = abs( frac( c.xxx + K.xyz ) * 6.0 - K.www );
			return c.z * lerp( K.xxx, saturate( p - K.xxx ), c.y );
		}


		float3 RGBToHSV(float3 c)
		{
			float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
			float4 p = lerp( float4( c.bg, K.wz ), float4( c.gb, K.xy ), step( c.b, c.g ) );
			float4 q = lerp( float4( p.xyw, c.r ), float4( c.r, p.yzx ), step( p.x, c.r ) );
			float d = q.x - min( q.w, q.y );
			float e = 1.0e-10;
			return float3( abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
		}

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float backfaceUseColor188 = _BackfaceColorOptions.x;
			float backfaceForceColor189 = _BackfaceColorOptions.z;
			float4 _BackfaceColor_Instance = UNITY_ACCESS_INSTANCED_PROP(_BackfaceColor_arr, _BackfaceColor);
			float backfaceHueShift190 = ( _BackfaceColorOptions.y / 360.0 );
			float _TexRGBWeight_Instance = UNITY_ACCESS_INSTANCED_PROP(_TexRGBWeight_arr, _TexRGBWeight);
			float _EmissionScale_Instance = UNITY_ACCESS_INSTANCED_PROP(_EmissionScale_arr, _EmissionScale);
			float4 temp_output_141_0 = ( i.vertexColor * _EmissionScale_Instance );
			int _UseRandomFrame_Instance = UNITY_ACCESS_INSTANCED_PROP(_UseRandomFrame_arr, _UseRandomFrame);
			float2 _NumCells_Instance = UNITY_ACCESS_INSTANCED_PROP(_NumCells_arr, _NumCells);
			float XCells159 = _NumCells_Instance.x;
			float YCells160 = _NumCells_Instance.y;
			float2 appendResult90 = (float2(( 1.0 / XCells159 ) , ( 1.0 / YCells160 )));
			float2 appendResult106 = (float2(round( i.uv3_texcoord3.x ) , round( i.uv3_texcoord3.y )));
			float dotResult4_g7 = dot( appendResult106 , float2( 12.9898,78.233 ) );
			float lerpResult10_g7 = lerp( 0.01 , XCells159 , frac( ( sin( dotResult4_g7 ) * 43758.55 ) ));
			float2 appendResult107 = (float2(round( i.uv3_texcoord3.z ) , round( i.uv3_texcoord3.w )));
			float dotResult4_g6 = dot( appendResult107 , float2( 12.9898,78.233 ) );
			float lerpResult10_g6 = lerp( 0.01 , YCells160 , frac( ( sin( dotResult4_g6 ) * 43758.55 ) ));
			float2 appendResult116 = (float2(( 1.0 - ( 1.0 / ceil( lerpResult10_g7 ) ) ) , ( 1.0 - ( 1.0 / ceil( lerpResult10_g6 ) ) )));
			float2 uv_TexCoord117 = i.uv_texcoord * appendResult90 + appendResult116;
			float2 appendResult51 = (float2(( 1.0 / XCells159 ) , ( 1.0 / YCells160 )));
			int _UseAnimatedOffset_Instance = UNITY_ACCESS_INSTANCED_PROP(_UseAnimatedOffset_arr, _UseAnimatedOffset);
			float2 _SelectFrame_Instance = UNITY_ACCESS_INSTANCED_PROP(_SelectFrame_arr, _SelectFrame);
			float2 appendResult25 = (float2(( 1.0 - ( 1.0 / _SelectFrame_Instance.x ) ) , ( 1.0 - ( 1.0 / _SelectFrame_Instance.y ) )));
			float2 appendResult153 = (float2(i.uv2_texcoord2.x , i.uv2_texcoord2.y));
			float2 uv_TexCoord6 = i.uv_texcoord * appendResult51 + ( (float)_UseAnimatedOffset_Instance == 0.0 ? appendResult25 : ( appendResult25 + appendResult153 ) );
			float4 TextureRGBA166 = tex2D( _Texture0, ( (float)_UseRandomFrame_Instance == 1.0 ? uv_TexCoord117 : uv_TexCoord6 ) );
			float layeredBlendVar225 = _TexRGBWeight_Instance;
			float4 layeredBlend225 = ( lerp( temp_output_141_0,( temp_output_141_0 * TextureRGBA166 ) , layeredBlendVar225 ) );
			float4 finalRGB227 = layeredBlend225;
			float3 hsvTorgb174 = RGBToHSV( finalRGB227.rgb );
			float3 hsvTorgb173 = HSVToRGB( float3(( backfaceHueShift190 + hsvTorgb174.x ),hsvTorgb174.y,hsvTorgb174.z) );
			o.Emission = ( backfaceUseColor188 == 1.0 ? ( i.ASEVFace == -1.0 ? ( backfaceForceColor189 == 1.0 ? _BackfaceColor_Instance : float4( hsvTorgb173 , 0.0 ) ) : finalRGB227 ) : finalRGB227 ).rgb;
			int _UseAlphaCutoff_Instance = UNITY_ACCESS_INSTANCED_PROP(_UseAlphaCutoff_arr, _UseAlphaCutoff);
			float temp_output_79_0 = ( TextureRGBA166.a * i.vertexColor.a );
			float ifLocalVar199 = 0;
			if( temp_output_79_0 <= i.uv4_texcoord4.x )
				ifLocalVar199 = 0.0;
			else
				ifLocalVar199 = temp_output_79_0;
			float layeredBlendVar201 = 0.0;
			float layeredBlend201 = ( lerp( ifLocalVar199,0.0 , layeredBlendVar201 ) );
			int _UseOpacityCurve_Instance = UNITY_ACCESS_INSTANCED_PROP(_UseOpacityCurve_arr, _UseOpacityCurve);
			float4 _AlphaUVMask_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_AlphaUVMask_ST_arr, _AlphaUVMask_ST);
			float2 uv_AlphaUVMask = i.uv_texcoord * _AlphaUVMask_ST_Instance.xy + _AlphaUVMask_ST_Instance.zw;
			o.Alpha = ( ( ( (float)_UseAlphaCutoff_Instance == 1.0 ? layeredBlend201 : temp_output_79_0 ) * ( (float)_UseOpacityCurve_Instance == 1.0 ? i.uv2_texcoord2.w : 1.0 ) ) * tex2D( _AlphaUVMask, uv_AlphaUVMask ).r );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Unlit alpha:fade keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.5
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
				float4 customPack1 : TEXCOORD1;
				float2 customPack2 : TEXCOORD2;
				float4 customPack3 : TEXCOORD3;
				float4 customPack4 : TEXCOORD4;
				float3 worldPos : TEXCOORD5;
				half4 color : COLOR0;
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
				o.customPack1.xyzw = customInputData.uv3_texcoord3;
				o.customPack1.xyzw = v.texcoord2;
				o.customPack2.xy = customInputData.uv_texcoord;
				o.customPack2.xy = v.texcoord;
				o.customPack3.xyzw = customInputData.uv2_texcoord2;
				o.customPack3.xyzw = v.texcoord1;
				o.customPack4.xyzw = customInputData.uv4_texcoord4;
				o.customPack4.xyzw = v.texcoord3;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.color = v.color;
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
				surfIN.uv3_texcoord3 = IN.customPack1.xyzw;
				surfIN.uv_texcoord = IN.customPack2.xy;
				surfIN.uv2_texcoord2 = IN.customPack3.xyzw;
				surfIN.uv4_texcoord4 = IN.customPack4.xyzw;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.vertexColor = IN.color;
				SurfaceOutput o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutput, o )
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
Version=18935
872;73;1083;960;3276.38;3275.994;3.327045;True;False
Node;AmplifyShaderEditor.CommentaryNode;164;-2496,-96;Inherit;False;1378.045;744.4274;Comment;17;125;6;51;152;50;25;52;74;62;31;37;82;159;160;83;222;223;Frame Selection;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;135;-2496,-768;Inherit;False;1638.759;653.3064;Uses particle system Vertex Stream on TEXCOORD1, generally StableRandom.xyzw to generate seed;22;117;90;116;114;115;89;88;113;112;111;110;109;108;107;106;104;102;103;105;101;161;162;Random Cell/Frame Selection;1,1,1,1;0;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;101;-2464,-480;Inherit;False;2;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;83;-2208,-32;Inherit;False;InstancedProperty;_NumCells;Num Cells;2;0;Create;True;0;0;0;False;0;False;1,1;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RoundOpNode;104;-2240,-320;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;159;-2048,-32;Inherit;False;XCells;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RoundOpNode;103;-2240,-256;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;160;-2048,48;Inherit;False;YCells;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RoundOpNode;102;-2240,-480;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RoundOpNode;105;-2240,-416;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;107;-2112,-320;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;106;-2112,-480;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;161;-2160,-656;Inherit;False;159;XCells;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;162;-2160,-560;Inherit;False;160;YCells;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;82;-2464,96;Inherit;False;InstancedProperty;_SelectFrame;Select Frame;3;0;Create;True;0;0;0;False;0;False;1,1;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.FunctionNode;108;-1952,-320;Inherit;False;Random Range;-1;;6;7b754edb8aebbfb4a9ace907af661cfc;0;3;1;FLOAT2;10,60;False;2;FLOAT;0.01;False;3;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;163;-2496,672;Inherit;False;489.8119;247.4882;Uses TEXCOORD1.xy, generally used for scrolling over lifetime;2;153;154;Offset;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;37;-2208,288;Inherit;False;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;109;-1952,-480;Inherit;False;Random Range;-1;;7;7b754edb8aebbfb4a9ace907af661cfc;0;3;1;FLOAT2;5,5;False;2;FLOAT;0.01;False;3;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;31;-2208,176;Inherit;False;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;62;-2048,176;Inherit;False;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CeilOpNode;110;-1760,-480;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CeilOpNode;111;-1760,-320;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;74;-2048,288;Inherit;False;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;154;-2464,736;Inherit;False;1;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;25;-1840,176;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;112;-1632,-320;Inherit;False;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;113;-1632,-480;Inherit;False;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;153;-2240,736;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;114;-1504,-320;Inherit;False;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;88;-1504,-672;Inherit;False;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;89;-1504,-576;Inherit;False;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;50;-1840,-32;Inherit;False;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;152;-1696,256;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;52;-1840,64;Inherit;False;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;115;-1504,-480;Inherit;False;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.IntNode;222;-1696,128;Inherit;False;InstancedProperty;_UseAnimatedOffset;Use Animated Offset;8;0;Create;True;0;0;0;True;0;False;0;0;False;0;1;INT;0
Node;AmplifyShaderEditor.Compare;223;-1488,128;Inherit;False;0;4;0;INT;0;False;1;FLOAT;0;False;2;FLOAT2;0,0;False;3;FLOAT2;1,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;116;-1312,-480;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;51;-1712,-32;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;90;-1312,-672;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.IntNode;119;-1024,0;Inherit;False;InstancedProperty;_UseRandomFrame;Use Random Frame;10;0;Create;True;0;0;0;True;0;False;0;0;False;0;1;INT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;6;-1328,-32;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;117;-1120,-592;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Compare;120;-768,0;Inherit;False;0;4;0;INT;0;False;1;FLOAT;1;False;2;FLOAT2;0,0;False;3;FLOAT2;1,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;8;-768,-192;Inherit;True;Property;_Texture0;Texture 0;0;0;Create;True;0;0;0;False;0;False;96024c08522e567458679a30cd70ab15;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.CommentaryNode;193;-2480,-1584;Inherit;False;2116.447;737.4417;;26;143;188;189;190;229;186;192;157;191;156;187;228;171;170;185;173;176;174;227;144;225;165;98;141;226;194;Color;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;1;-576,0;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;166;-256,0;Inherit;False;TextureRGBA;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;98;-2448,-976;Inherit;False;InstancedProperty;_EmissionScale;Emission Scale;4;0;Create;True;0;0;0;True;0;False;1;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;226;-2448,-1136;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;165;-2256,-1040;Inherit;False;166;TextureRGBA;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;141;-2256,-1136;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector4Node;186;-2448,-1520;Inherit;False;Property;_BackfaceColorOptions;Use Backface | Hue Shift | Force Color;5;1;[Header];Create;False;1;Backface;0;0;True;0;False;0,180,0,0;0,180,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;144;-2064,-1056;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;143;-2256,-1216;Inherit;False;InstancedProperty;_TexRGBWeight;TexRGB Weight;7;1;[Header];Create;True;1;Vertex Stream (Particles);0;0;True;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;217;-2496,944;Inherit;False;1509.481;713.8796;;19;215;216;168;205;204;79;203;78;84;85;80;202;198;201;199;200;220;219;218;Opacity;1,1,1,1;0;0
Node;AmplifyShaderEditor.LayeredBlendNode;225;-1936,-1136;Inherit;False;6;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;229;-2096,-1440;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;360;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;205;-2432,1328;Inherit;False;166;TextureRGBA;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;190;-1984,-1440;Inherit;False;backfaceHueShift;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;227;-1744,-1136;Inherit;False;finalRGB;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.BreakToComponentsNode;204;-2240,1328;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.VertexColorNode;168;-2240,1472;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;192;-1552,-1216;Inherit;False;190;backfaceHueShift;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RGBToHSVNode;174;-1552,-1136;Inherit;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;79;-2112,1328;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;203;-2112,1248;Inherit;False;Constant;_Float1;Float 1;1;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;200;-2112,1056;Inherit;False;3;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;189;-2096,-1344;Inherit;False;backfaceForceColor;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;176;-1296,-1216;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;199;-1904,1056;Inherit;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;157;-1168,-1344;Inherit;False;InstancedProperty;_BackfaceColor;Backface Color;6;0;Create;True;0;0;0;True;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LayeredBlendNode;201;-1728,1056;Inherit;False;6;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;78;-1760,1424;Inherit;False;1;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.IntNode;84;-1760,1280;Inherit;False;InstancedProperty;_UseOpacityCurve;Use Opacity Curve;9;0;Create;True;0;0;0;True;0;False;0;1;False;0;1;INT;0
Node;AmplifyShaderEditor.IntNode;198;-1732,981;Inherit;False;InstancedProperty;_UseAlphaCutoff;Use Alpha Cutoff;11;0;Create;True;0;0;0;True;0;False;0;0;False;0;1;INT;0
Node;AmplifyShaderEditor.GetLocalVarNode;191;-1168,-1424;Inherit;False;189;backfaceForceColor;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.HSVToRGBNode;173;-1168,-1136;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.FaceVariableNode;156;-912,-1408;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;228;-912,-1200;Inherit;False;227;finalRGB;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.Compare;185;-912,-1344;Inherit;False;0;4;0;FLOAT;0;False;1;FLOAT;1;False;2;COLOR;0,0,0,0;False;3;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.Compare;85;-1536,1280;Inherit;False;0;4;0;INT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;3;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;188;-2096,-1520;Inherit;False;backfaceUseColor;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Compare;202;-1536,1056;Inherit;False;0;4;0;INT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;3;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Compare;170;-720,-1344;Inherit;False;0;4;0;FLOAT;0;False;1;FLOAT;-1;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;215;-1344,1280;Inherit;True;Property;_AlphaUVMask;Alpha UV Mask;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;80;-1344,1056;Inherit;False;2;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;187;-720,-1424;Inherit;False;188;backfaceUseColor;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.StickyNoteNode;218;-2272,992;Inherit;False;346;237;Particle Alpha Cutoff;;1,1,1,1;Uses Vertex Stream, TEXCOORD3.x;0;0
Node;AmplifyShaderEditor.Compare;171;-528,-1344;Inherit;False;0;4;0;FLOAT;0;False;1;FLOAT;1;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StickyNoteNode;194;-2448,-1248;Inherit;False;150;100;Particle Color;;1,1,1,1;Uses Vertex Steam COLOR;0;0
Node;AmplifyShaderEditor.StickyNoteNode;125;-2464,-32;Inherit;False;150;100;New Note;;1,1,1,1;Starts from bottom Left corner??;0;0
Node;AmplifyShaderEditor.StickyNoteNode;219;-1888,1360;Inherit;False;309;234;Particle Opacity;;1,1,1,1;Uses Vertex Stream, TEXCOORD1.W;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;216;-1152,1056;Inherit;False;2;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StickyNoteNode;220;-2432,1472;Inherit;False;150;100;Particle Color;;1,1,1,1;Uses Vertex Steam COLOR;0;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,-128;Float;False;True;-1;3;ASEMaterialInspector;0;0;Unlit;SeroRonin/VFX/jhinVFX_UniversalShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;104;0;101;3
WireConnection;159;0;83;1
WireConnection;103;0;101;4
WireConnection;160;0;83;2
WireConnection;102;0;101;1
WireConnection;105;0;101;2
WireConnection;107;0;104;0
WireConnection;107;1;103;0
WireConnection;106;0;102;0
WireConnection;106;1;105;0
WireConnection;108;1;107;0
WireConnection;108;3;162;0
WireConnection;37;1;82;2
WireConnection;109;1;106;0
WireConnection;109;3;161;0
WireConnection;31;1;82;1
WireConnection;62;1;31;0
WireConnection;110;0;109;0
WireConnection;111;0;108;0
WireConnection;74;1;37;0
WireConnection;25;0;62;0
WireConnection;25;1;74;0
WireConnection;112;1;111;0
WireConnection;113;1;110;0
WireConnection;153;0;154;1
WireConnection;153;1;154;2
WireConnection;114;1;112;0
WireConnection;88;1;161;0
WireConnection;89;1;162;0
WireConnection;50;1;159;0
WireConnection;152;0;25;0
WireConnection;152;1;153;0
WireConnection;52;1;160;0
WireConnection;115;1;113;0
WireConnection;223;0;222;0
WireConnection;223;2;25;0
WireConnection;223;3;152;0
WireConnection;116;0;115;0
WireConnection;116;1;114;0
WireConnection;51;0;50;0
WireConnection;51;1;52;0
WireConnection;90;0;88;0
WireConnection;90;1;89;0
WireConnection;6;0;51;0
WireConnection;6;1;223;0
WireConnection;117;0;90;0
WireConnection;117;1;116;0
WireConnection;120;0;119;0
WireConnection;120;2;117;0
WireConnection;120;3;6;0
WireConnection;1;0;8;0
WireConnection;1;1;120;0
WireConnection;166;0;1;0
WireConnection;141;0;226;0
WireConnection;141;1;98;0
WireConnection;144;0;141;0
WireConnection;144;1;165;0
WireConnection;225;0;143;0
WireConnection;225;1;141;0
WireConnection;225;2;144;0
WireConnection;229;0;186;2
WireConnection;190;0;229;0
WireConnection;227;0;225;0
WireConnection;204;0;205;0
WireConnection;174;0;227;0
WireConnection;79;0;204;3
WireConnection;79;1;168;4
WireConnection;189;0;186;3
WireConnection;176;0;192;0
WireConnection;176;1;174;1
WireConnection;199;0;79;0
WireConnection;199;1;200;1
WireConnection;199;2;79;0
WireConnection;199;3;203;0
WireConnection;199;4;203;0
WireConnection;201;1;199;0
WireConnection;173;0;176;0
WireConnection;173;1;174;2
WireConnection;173;2;174;3
WireConnection;185;0;191;0
WireConnection;185;2;157;0
WireConnection;185;3;173;0
WireConnection;85;0;84;0
WireConnection;85;2;78;4
WireConnection;188;0;186;1
WireConnection;202;0;198;0
WireConnection;202;2;201;0
WireConnection;202;3;79;0
WireConnection;170;0;156;0
WireConnection;170;2;185;0
WireConnection;170;3;228;0
WireConnection;80;0;202;0
WireConnection;80;1;85;0
WireConnection;171;0;187;0
WireConnection;171;2;170;0
WireConnection;171;3;228;0
WireConnection;216;0;80;0
WireConnection;216;1;215;1
WireConnection;0;2;171;0
WireConnection;0;9;216;0
ASEEND*/
//CHKSM=6B29CD4BDCF661273E5AD8BAA5716D09DD7F55FA