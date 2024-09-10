// Upgrade NOTE: upgraded instancing buffer 'SeroRoninVFXjhinVFX_SoulFighterFlameShader' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SeroRonin/VFX/jhinVFX_SoulFighterFlameShader"
{
	Properties
	{
		_Texture0("Texture 0", 2D) = "white" {}
		_EmissionScale("Emission Scale", Float) = 1
		[Header(Backface)]_BackfaceColorOptions("Use Backface | Hue Shift | Force Color", Vector) = (0,180,0,0)
		_BackfaceColor("Backface Color", Color) = (0,0,0,0)
		[Header(Vertex Stream (Particles))]_TexRGBWeight("TexRGB Weight", Range( 0 , 1)) = 1
		_UseAnimatedOffset("Use Animated Offset", Int) = 0
		_UseOpacityCurve("Use Opacity Curve", Int) = 0
		_UseRandomFrame("Use Random Frame", Int) = 0
		_UseAlphaCutoff("Use Alpha Cutoff", Int) = 0
		_UVR("UV R", Vector) = (1,2,0,0)
		_UVG("UV G", Vector) = (1,1,0,0)
		_UVB("UV B", Vector) = (4,1.5,0,0)
		_ScrollR("Scroll R", Vector) = (1,1,0,0)
		_ScrollG("Scroll G", Vector) = (0,1,0,0)
		_ScrollB("Scroll B", Vector) = (1,0.8,0,0)
		_Texture1("Texture 1", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Off
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.5
		#pragma multi_compile_instancing
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _BackfaceColorOptions;
		uniform sampler2D _Texture0;
		uniform float4 _Texture0_ST;
		uniform sampler2D _Texture1;
		uniform float4 _Texture1_ST;

		UNITY_INSTANCING_BUFFER_START(SeroRoninVFXjhinVFX_SoulFighterFlameShader)
			UNITY_DEFINE_INSTANCED_PROP(float4, _BackfaceColor)
#define _BackfaceColor_arr SeroRoninVFXjhinVFX_SoulFighterFlameShader
			UNITY_DEFINE_INSTANCED_PROP(float2, _ScrollR)
#define _ScrollR_arr SeroRoninVFXjhinVFX_SoulFighterFlameShader
			UNITY_DEFINE_INSTANCED_PROP(float2, _UVR)
#define _UVR_arr SeroRoninVFXjhinVFX_SoulFighterFlameShader
			UNITY_DEFINE_INSTANCED_PROP(float2, _ScrollG)
#define _ScrollG_arr SeroRoninVFXjhinVFX_SoulFighterFlameShader
			UNITY_DEFINE_INSTANCED_PROP(float2, _UVG)
#define _UVG_arr SeroRoninVFXjhinVFX_SoulFighterFlameShader
			UNITY_DEFINE_INSTANCED_PROP(float2, _ScrollB)
#define _ScrollB_arr SeroRoninVFXjhinVFX_SoulFighterFlameShader
			UNITY_DEFINE_INSTANCED_PROP(float2, _UVB)
#define _UVB_arr SeroRoninVFXjhinVFX_SoulFighterFlameShader
			UNITY_DEFINE_INSTANCED_PROP(int, _UseAnimatedOffset)
#define _UseAnimatedOffset_arr SeroRoninVFXjhinVFX_SoulFighterFlameShader
			UNITY_DEFINE_INSTANCED_PROP(int, _UseRandomFrame)
#define _UseRandomFrame_arr SeroRoninVFXjhinVFX_SoulFighterFlameShader
			UNITY_DEFINE_INSTANCED_PROP(float, _EmissionScale)
#define _EmissionScale_arr SeroRoninVFXjhinVFX_SoulFighterFlameShader
			UNITY_DEFINE_INSTANCED_PROP(float, _TexRGBWeight)
#define _TexRGBWeight_arr SeroRoninVFXjhinVFX_SoulFighterFlameShader
			UNITY_DEFINE_INSTANCED_PROP(int, _UseOpacityCurve)
#define _UseOpacityCurve_arr SeroRoninVFXjhinVFX_SoulFighterFlameShader
			UNITY_DEFINE_INSTANCED_PROP(int, _UseAlphaCutoff)
#define _UseAlphaCutoff_arr SeroRoninVFXjhinVFX_SoulFighterFlameShader
		UNITY_INSTANCING_BUFFER_END(SeroRoninVFXjhinVFX_SoulFighterFlameShader)

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			int _UseAnimatedOffset_Instance = UNITY_ACCESS_INSTANCED_PROP(_UseAnimatedOffset_arr, _UseAnimatedOffset);
			int _UseRandomFrame_Instance = UNITY_ACCESS_INSTANCED_PROP(_UseRandomFrame_arr, _UseRandomFrame);
			float _EmissionScale_Instance = UNITY_ACCESS_INSTANCED_PROP(_EmissionScale_arr, _EmissionScale);
			float _TexRGBWeight_Instance = UNITY_ACCESS_INSTANCED_PROP(_TexRGBWeight_arr, _TexRGBWeight);
			float4 _BackfaceColor_Instance = UNITY_ACCESS_INSTANCED_PROP(_BackfaceColor_arr, _BackfaceColor);
			int _UseOpacityCurve_Instance = UNITY_ACCESS_INSTANCED_PROP(_UseOpacityCurve_arr, _UseOpacityCurve);
			int _UseAlphaCutoff_Instance = UNITY_ACCESS_INSTANCED_PROP(_UseAlphaCutoff_arr, _UseAlphaCutoff);
			float2 _ScrollR_Instance = UNITY_ACCESS_INSTANCED_PROP(_ScrollR_arr, _ScrollR);
			float2 uv_Texture0 = i.uv_texcoord * _Texture0_ST.xy + _Texture0_ST.zw;
			float2 _UVR_Instance = UNITY_ACCESS_INSTANCED_PROP(_UVR_arr, _UVR);
			float2 panner231 = ( _Time.y * _ScrollR_Instance + ( uv_Texture0 * _UVR_Instance ));
			float4 tex2DNode235 = tex2D( _Texture0, panner231 );
			float2 _ScrollG_Instance = UNITY_ACCESS_INSTANCED_PROP(_ScrollG_arr, _ScrollG);
			float2 _UVG_Instance = UNITY_ACCESS_INSTANCED_PROP(_UVG_arr, _UVG);
			float2 panner240 = ( 1.0 * _Time.y * _ScrollG_Instance + ( uv_Texture0 * _UVG_Instance ));
			float4 tex2DNode234 = tex2D( _Texture0, panner240 );
			float2 _ScrollB_Instance = UNITY_ACCESS_INSTANCED_PROP(_ScrollB_arr, _ScrollB);
			float2 _UVB_Instance = UNITY_ACCESS_INSTANCED_PROP(_UVB_arr, _UVB);
			float2 panner239 = ( _Time.y * _ScrollB_Instance + ( uv_Texture0 * _UVB_Instance ));
			float4 tex2DNode241 = tex2D( _Texture0, panner239 );
			float4 appendResult254 = (float4(tex2DNode235.r , tex2DNode234.g , tex2DNode241.a , 0.0));
			float2 uv_Texture1 = i.uv_texcoord * _Texture1_ST.xy + _Texture1_ST.zw;
			float4 temp_output_216_0 = ( appendResult254 * tex2D( _Texture1, uv_Texture1 ) );
			o.Alpha = temp_output_216_0.x;
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
1038;73;920;916;2572.095;1761.172;3.987038;True;False
Node;AmplifyShaderEditor.TexturePropertyNode;8;-1176.67,598.4613;Inherit;True;Property;_Texture0;Texture 0;0;0;Create;True;0;0;0;False;0;False;1608000747f47ed4580e36aa55ff5e04;96024c08522e567458679a30cd70ab15;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.Vector2Node;246;-608,768;Inherit;False;InstancedProperty;_UVR;UV R;11;0;Create;True;0;0;0;True;0;False;1,2;1,2;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;232;-832,768;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;247;-608,1024;Inherit;False;InstancedProperty;_UVG;UV G;12;0;Create;True;0;0;0;True;0;False;1,1;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;248;-665.4204,1248;Inherit;False;InstancedProperty;_UVB;UV B;13;0;Create;True;0;0;0;True;0;False;4,1.5;1.5,0.5;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;243;-448,1120;Inherit;False;InstancedProperty;_ScrollG;Scroll G;15;0;Create;True;0;0;0;True;0;False;0,1;0,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleTimeNode;233;-989.7276,308.8806;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;244;-448,1344;Inherit;False;InstancedProperty;_ScrollB;Scroll B;16;0;Create;True;0;0;0;True;0;False;1,0.8;0.3,1.15;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;242;-448,864;Inherit;False;InstancedProperty;_ScrollR;Scroll R;14;0;Create;True;0;0;0;True;0;False;1,1;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;251;-448,1248;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;250;-448,1024;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;249;-448,768;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;240;-256,1024;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;231;-256,768;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;260;22.73788,1320.787;Inherit;True;Property;_Texture1;Texture 1;17;0;Create;True;0;0;0;False;0;False;4f668dc813670734c965af73768d7ed3;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.PannerNode;239;-256,1248;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;235;53.52666,645.5145;Inherit;True;Property;_TextureSample2;Texture Sample 2;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;262;304.0711,1392.404;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;234;41.09621,878.2802;Inherit;True;Property;_TextureSample1;Texture Sample 1;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;241;39.46215,1112.499;Inherit;True;Property;_TextureSample3;Texture Sample 3;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;193;-2480,-1584;Inherit;False;2116.447;737.4417;;26;143;188;189;190;229;186;192;157;191;156;187;228;171;170;185;173;176;174;227;144;225;165;98;141;226;194;Color;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;217;-2496,944;Inherit;False;1509.481;713.8796;;17;168;205;204;79;203;78;84;85;80;202;198;201;199;200;220;219;218;Opacity;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;163;-2496,672;Inherit;False;489.8119;247.4882;Uses TEXCOORD1.xy, generally used for scrolling over lifetime;2;153;154;Offset;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;164;-2496,-96;Inherit;False;1378.045;744.4274;Comment;17;125;6;51;152;50;25;52;74;62;31;37;82;159;160;83;222;223;Frame Selection;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;135;-2496,-768;Inherit;False;1638.759;653.3064;Uses particle system Vertex Stream on TEXCOORD1, generally StableRandom.xyzw to generate seed;22;117;90;116;114;115;89;88;113;112;111;110;109;108;107;106;104;102;103;105;101;161;162;Random Cell/Frame Selection;1,1,1,1;0;0
Node;AmplifyShaderEditor.DynamicAppendNode;254;507.0305,886.0181;Inherit;True;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;261;523.2745,1298.482;Inherit;True;Property;_TextureSample4;Texture Sample 4;18;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;192;-1552,-1216;Inherit;False;190;backfaceHueShift;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.StickyNoteNode;125;-2464,-32;Inherit;False;150;100;New Note;;1,1,1,1;Starts from bottom Left corner??;0;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;88;-1504,-672;Inherit;False;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;157;-1168,-1344;Inherit;False;InstancedProperty;_BackfaceColor;Backface Color;5;0;Create;True;0;0;0;True;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexCoordVertexDataNode;154;-2464,736;Inherit;False;1;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector4Node;186;-2448,-1520;Inherit;False;Property;_BackfaceColorOptions;Use Backface | Hue Shift | Force Color;4;1;[Header];Create;False;1;Backface;0;0;True;0;False;0,180,0,0;0,180,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Compare;171;-528,-1344;Inherit;False;0;4;0;FLOAT;0;False;1;FLOAT;1;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;203;-2112,1248;Inherit;False;Constant;_Float1;Float 1;1;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LayeredBlendNode;225;-1936,-1136;Inherit;False;6;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.IntNode;198;-1732,981;Inherit;False;InstancedProperty;_UseAlphaCutoff;Use Alpha Cutoff;10;0;Create;True;0;0;0;True;0;False;0;0;False;0;1;INT;0
Node;AmplifyShaderEditor.ColorNode;253;-532.8868,-296.9078;Inherit;False;Constant;_Color0;Color 0;18;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FaceVariableNode;156;-912,-1408;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;191;-1168,-1424;Inherit;False;189;backfaceForceColor;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;108;-1952,-320;Inherit;False;Random Range;-1;;6;7b754edb8aebbfb4a9ace907af661cfc;0;3;1;FLOAT2;10,60;False;2;FLOAT;0.01;False;3;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RoundOpNode;104;-2240,-320;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-576,0;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;166;-256,0;Inherit;False;TextureRGBA;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;98;-2448,-976;Inherit;False;InstancedProperty;_EmissionScale;Emission Scale;3;0;Create;True;0;0;0;True;0;False;1;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;37;-2208,288;Inherit;False;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;114;-1504,-320;Inherit;False;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;176;-1296,-1216;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;113;-1632,-480;Inherit;False;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;89;-1504,-576;Inherit;False;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;51;-1712,-32;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;141;-2256,-1136;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;161;-2160,-656;Inherit;False;159;XCells;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;227;-1744,-1136;Inherit;False;finalRGB;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.Compare;170;-720,-1344;Inherit;False;0;4;0;FLOAT;0;False;1;FLOAT;-1;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;188;-2096,-1520;Inherit;False;backfaceUseColor;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Compare;85;-1536,1280;Inherit;False;0;4;0;INT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;3;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;50;-1840,-32;Inherit;False;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;229;-2096,-1440;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;360;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCGrayscale;264;864,688;Inherit;True;0;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;205;-2432,1328;Inherit;False;166;TextureRGBA;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;162;-2160,-560;Inherit;False;160;YCells;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RoundOpNode;105;-2240,-416;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.IntNode;222;-1696,128;Inherit;False;InstancedProperty;_UseAnimatedOffset;Use Animated Offset;7;0;Create;True;0;0;0;True;0;False;0;0;False;0;1;INT;0
Node;AmplifyShaderEditor.DynamicAppendNode;116;-1312,-480;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;143;-2256,-1216;Inherit;False;InstancedProperty;_TexRGBWeight;TexRGB Weight;6;1;[Header];Create;True;1;Vertex Stream (Particles);0;0;True;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;144;-2064,-1056;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StickyNoteNode;194;-2448,-1248;Inherit;False;150;100;Particle Color;;1,1,1,1;Uses Vertex Steam COLOR;0;0
Node;AmplifyShaderEditor.ConditionalIfNode;199;-1904,1056;Inherit;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;109;-1952,-480;Inherit;False;Random Range;-1;;7;7b754edb8aebbfb4a9ace907af661cfc;0;3;1;FLOAT2;5,5;False;2;FLOAT;0.01;False;3;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Compare;202;-1536,1056;Inherit;False;0;4;0;INT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;3;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StickyNoteNode;220;-2432,1472;Inherit;False;150;100;Particle Color;;1,1,1,1;Uses Vertex Steam COLOR;0;0
Node;AmplifyShaderEditor.LayeredBlendNode;201;-1728,1056;Inherit;False;6;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;107;-2112,-320;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.VertexColorNode;226;-2448,-1136;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;168;-2240,1472;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;90;-1312,-672;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;80;-1344,1056;Inherit;False;2;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;83;-2208,-32;Inherit;False;InstancedProperty;_NumCells;Num Cells;1;0;Create;True;0;0;0;False;0;False;1,1;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TexCoordVertexDataNode;200;-2112,1056;Inherit;False;3;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;159;-2048,-32;Inherit;False;XCells;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;165;-2256,-1040;Inherit;False;166;TextureRGBA;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;252;541.7606,577.1925;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;153;-2240,736;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;6;-1328,-32;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Compare;223;-1488,128;Inherit;False;0;4;0;INT;0;False;1;FLOAT;0;False;2;FLOAT2;0,0;False;3;FLOAT2;1,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CeilOpNode;111;-1760,-320;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RoundOpNode;103;-2240,-256;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;115;-1504,-480;Inherit;False;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RoundOpNode;102;-2240,-480;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;117;-1120,-592;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.IntNode;119;-1024,0;Inherit;False;InstancedProperty;_UseRandomFrame;Use Random Frame;9;0;Create;True;0;0;0;True;0;False;0;0;False;0;1;INT;0
Node;AmplifyShaderEditor.Compare;185;-912,-1344;Inherit;False;0;4;0;FLOAT;0;False;1;FLOAT;1;False;2;COLOR;0,0,0,0;False;3;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;216;861.7903,919.5994;Inherit;True;2;2;0;FLOAT4;1,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;106;-2112,-480;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.HSVToRGBNode;173;-1168,-1136;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.BreakToComponentsNode;204;-2240,1328;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.RegisterLocalVarNode;189;-2096,-1344;Inherit;False;backfaceForceColor;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;152;-1696,256;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;160;-2048,48;Inherit;False;YCells;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;190;-1984,-1440;Inherit;False;backfaceHueShift;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;79;-2112,1328;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;228;-912,-1200;Inherit;False;227;finalRGB;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;31;-2208,176;Inherit;False;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;78;-1760,1424;Inherit;False;1;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RGBToHSVNode;174;-1552,-1136;Inherit;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleDivideOpNode;52;-1840,64;Inherit;False;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StickyNoteNode;219;-1888,1360;Inherit;False;309;234;Particle Opacity;;1,1,1,1;Uses Vertex Stream, TEXCOORD1.W;0;0
Node;AmplifyShaderEditor.IntNode;84;-1760,1280;Inherit;False;InstancedProperty;_UseOpacityCurve;Use Opacity Curve;8;0;Create;True;0;0;0;True;0;False;0;1;False;0;1;INT;0
Node;AmplifyShaderEditor.GetLocalVarNode;187;-720,-1424;Inherit;False;188;backfaceUseColor;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;74;-2048,288;Inherit;False;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Compare;120;-768,0;Inherit;False;0;4;0;INT;0;False;1;FLOAT;1;False;2;FLOAT2;0,0;False;3;FLOAT2;1,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;112;-1632,-320;Inherit;False;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;25;-1840,176;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CeilOpNode;110;-1760,-480;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;62;-2048,176;Inherit;False;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StickyNoteNode;218;-2272,992;Inherit;False;346;237;Particle Alpha Cutoff;;1,1,1,1;Uses Vertex Stream, TEXCOORD3.x;0;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;101;-2464,-480;Inherit;False;2;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;82;-2464,96;Inherit;False;InstancedProperty;_SelectFrame;Select Frame;2;0;Create;True;0;0;0;False;0;False;1,1;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;924.409,-53.11116;Float;False;True;-1;3;ASEMaterialInspector;0;0;Unlit;SeroRonin/VFX/jhinVFX_SoulFighterFlameShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;232;2;8;0
WireConnection;251;0;232;0
WireConnection;251;1;248;0
WireConnection;250;0;232;0
WireConnection;250;1;247;0
WireConnection;249;0;232;0
WireConnection;249;1;246;0
WireConnection;240;0;250;0
WireConnection;240;2;243;0
WireConnection;231;0;249;0
WireConnection;231;2;242;0
WireConnection;231;1;233;0
WireConnection;239;0;251;0
WireConnection;239;2;244;0
WireConnection;239;1;233;0
WireConnection;235;0;8;0
WireConnection;235;1;231;0
WireConnection;262;2;260;0
WireConnection;234;0;8;0
WireConnection;234;1;240;0
WireConnection;241;0;8;0
WireConnection;241;1;239;0
WireConnection;254;0;235;1
WireConnection;254;1;234;2
WireConnection;254;2;241;4
WireConnection;261;0;260;0
WireConnection;261;1;262;0
WireConnection;88;1;161;0
WireConnection;171;0;187;0
WireConnection;171;2;170;0
WireConnection;171;3;228;0
WireConnection;225;0;143;0
WireConnection;225;1;141;0
WireConnection;225;2;144;0
WireConnection;108;1;107;0
WireConnection;108;3;162;0
WireConnection;104;0;101;3
WireConnection;1;0;8;0
WireConnection;1;1;231;0
WireConnection;166;0;1;0
WireConnection;37;1;82;2
WireConnection;114;1;112;0
WireConnection;176;0;192;0
WireConnection;176;1;174;1
WireConnection;113;1;110;0
WireConnection;89;1;162;0
WireConnection;51;0;50;0
WireConnection;51;1;52;0
WireConnection;141;0;226;0
WireConnection;141;1;98;0
WireConnection;227;0;225;0
WireConnection;170;0;156;0
WireConnection;170;2;185;0
WireConnection;170;3;228;0
WireConnection;188;0;186;1
WireConnection;85;0;84;0
WireConnection;85;2;78;4
WireConnection;50;1;159;0
WireConnection;229;0;186;2
WireConnection;264;0;216;0
WireConnection;105;0;101;2
WireConnection;116;0;115;0
WireConnection;116;1;114;0
WireConnection;144;0;141;0
WireConnection;144;1;165;0
WireConnection;199;0;79;0
WireConnection;199;1;200;1
WireConnection;199;2;79;0
WireConnection;199;3;203;0
WireConnection;199;4;203;0
WireConnection;109;1;106;0
WireConnection;109;3;161;0
WireConnection;202;0;198;0
WireConnection;202;2;201;0
WireConnection;202;3;79;0
WireConnection;201;1;199;0
WireConnection;107;0;104;0
WireConnection;107;1;103;0
WireConnection;90;0;88;0
WireConnection;90;1;89;0
WireConnection;80;0;202;0
WireConnection;80;1;85;0
WireConnection;159;0;83;1
WireConnection;252;0;235;1
WireConnection;252;1;234;2
WireConnection;252;2;241;3
WireConnection;153;0;154;1
WireConnection;153;1;154;2
WireConnection;6;0;51;0
WireConnection;6;1;223;0
WireConnection;223;0;222;0
WireConnection;223;2;25;0
WireConnection;223;3;152;0
WireConnection;111;0;108;0
WireConnection;103;0;101;4
WireConnection;115;1;113;0
WireConnection;102;0;101;1
WireConnection;117;0;90;0
WireConnection;117;1;116;0
WireConnection;185;0;191;0
WireConnection;185;2;157;0
WireConnection;185;3;173;0
WireConnection;216;0;254;0
WireConnection;216;1;261;0
WireConnection;106;0;102;0
WireConnection;106;1;105;0
WireConnection;173;0;176;0
WireConnection;173;1;174;2
WireConnection;173;2;174;3
WireConnection;204;0;205;0
WireConnection;189;0;186;3
WireConnection;152;0;25;0
WireConnection;152;1;153;0
WireConnection;160;0;83;2
WireConnection;190;0;229;0
WireConnection;79;0;204;3
WireConnection;79;1;168;4
WireConnection;31;1;82;1
WireConnection;174;0;227;0
WireConnection;52;1;160;0
WireConnection;74;1;37;0
WireConnection;120;0;119;0
WireConnection;120;2;117;0
WireConnection;120;3;6;0
WireConnection;112;1;111;0
WireConnection;25;0;62;0
WireConnection;25;1;74;0
WireConnection;110;0;109;0
WireConnection;62;1;31;0
WireConnection;0;9;216;0
ASEEND*/
//CHKSM=5E56D16A8D8FF4C651330848B8F749B29458AEFF