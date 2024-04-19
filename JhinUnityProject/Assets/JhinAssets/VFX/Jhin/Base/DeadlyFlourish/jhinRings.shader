// Upgrade NOTE: upgraded instancing buffer 'SeroRoninVFXjhin_Rings' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SeroRonin/VFX/jhin_Rings"
{
	Properties
	{
		_Texture0("Texture 0", 2D) = "white" {}
		_FrameChoice("FrameChoice", Int) = 0
		_Color0("Color 0", Color) = (0,0,0,0)
		_TexRGBWeight("TexRGBWeight", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma multi_compile_instancing
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float4 uv_texcoord;
		};

		uniform sampler2D _Texture0;
		uniform int _FrameChoice;

		UNITY_INSTANCING_BUFFER_START(SeroRoninVFXjhin_Rings)
			UNITY_DEFINE_INSTANCED_PROP(float4, _Color0)
#define _Color0_arr SeroRoninVFXjhin_Rings
			UNITY_DEFINE_INSTANCED_PROP(float, _TexRGBWeight)
#define _TexRGBWeight_arr SeroRoninVFXjhin_Rings
		UNITY_INSTANCING_BUFFER_END(SeroRoninVFXjhin_Rings)

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float _TexRGBWeight_Instance = UNITY_ACCESS_INSTANCED_PROP(_TexRGBWeight_arr, _TexRGBWeight);
			float4 _Color0_Instance = UNITY_ACCESS_INSTANCED_PROP(_Color0_arr, _Color0);
			float2 appendResult11 = (float2(0.5 , 0.5));
			float2 appendResult25 = (float2(( (float)_FrameChoice < 2.0 ? 0.0 : 0.5 ) , ( (float)( _FrameChoice % 2 ) == 1.0 ? 0.5 : 0.0 )));
			float2 uvs_TexCoord6 = i.uv_texcoord;
			uvs_TexCoord6.xy = i.uv_texcoord.xy * appendResult11 + appendResult25;
			float4 tex2DNode2 = tex2D( _Texture0, uvs_TexCoord6.xy );
			float layeredBlendVar52 = _TexRGBWeight_Instance;
			float4 layeredBlend52 = ( lerp( _Color0_Instance,tex2DNode2 , layeredBlendVar52 ) );
			o.Emission = layeredBlend52.rgb;
			float ifLocalVar3 = 0;
			if( tex2DNode2.a <= i.uv_texcoord.z )
				ifLocalVar3 = 0.0;
			else
				ifLocalVar3 = tex2DNode2.a;
			float layeredBlendVar33 = i.uv_texcoord.w;
			float layeredBlend33 = ( lerp( ifLocalVar3,0.0 , layeredBlendVar33 ) );
			o.Alpha = layeredBlend33;
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
				float4 customPack1 : TEXCOORD1;
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
				o.customPack1.xyzw = customInputData.uv_texcoord;
				o.customPack1.xyzw = v.texcoord;
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
				surfIN.uv_texcoord = IN.customPack1.xyzw;
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
481;73;1227;769;2020.486;-24.80014;1;True;True
Node;AmplifyShaderEditor.IntNode;49;-1669.716,511.8025;Inherit;False;Property;_FrameChoice;FrameChoice;1;0;Create;True;0;0;0;False;0;False;0;0;False;0;1;INT;0
Node;AmplifyShaderEditor.SimpleRemainderNode;30;-1467.2,513.6951;Inherit;False;2;0;INT;0;False;1;INT;2;False;1;INT;0
Node;AmplifyShaderEditor.Compare;21;-1325,323;Inherit;False;4;4;0;INT;0;False;1;FLOAT;2;False;2;FLOAT;0;False;3;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.Compare;31;-1225.2,562.6951;Inherit;False;0;4;0;INT;0;False;1;FLOAT;1;False;2;FLOAT;0.5;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;11;-1215.612,77.77618;Inherit;False;FLOAT2;4;0;FLOAT;0.5;False;1;FLOAT;0.5;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;25;-1040.638,501.6956;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;1;-923.5744,2.132141;Inherit;True;Property;_Texture0;Texture 0;0;0;Create;True;0;0;0;False;0;False;056c897dcc65ee342ba435059500acb1;056c897dcc65ee342ba435059500acb1;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TextureCoordinatesNode;6;-891.3767,236.2136;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;5;-604.3767,463.2136;Inherit;False;Constant;_Float1;Float 1;1;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-578.5744,14.13214;Inherit;True;Property;_TextureSample0;Texture Sample 0;2;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexCoordVertexDataNode;32;-607.3262,294.4753;Inherit;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ConditionalIfNode;3;-219.3767,182.2136;Inherit;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;51;-484.299,-276.0441;Inherit;False;InstancedProperty;_TexRGBWeight;TexRGBWeight;3;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;50;-497.299,-165.0441;Inherit;False;InstancedProperty;_Color0;Color 0;2;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;7;-1637.581,332.0049;Inherit;False;Random Range;-1;;1;7b754edb8aebbfb4a9ace907af661cfc;0;3;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT;3;False;1;FLOAT;0
Node;AmplifyShaderEditor.LayeredBlendNode;52;-46.29895,-98.04413;Inherit;False;6;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LayeredBlendNode;33;-33.32617,204.4753;Inherit;False;6;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FloorOpNode;8;-1642.639,446.8009;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;10;-1635.391,240.7308;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;178,13;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;SeroRonin/VFX/jhin_Rings;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;30;0;49;0
WireConnection;21;0;49;0
WireConnection;31;0;30;0
WireConnection;25;0;21;0
WireConnection;25;1;31;0
WireConnection;6;0;11;0
WireConnection;6;1;25;0
WireConnection;2;0;1;0
WireConnection;2;1;6;0
WireConnection;3;0;2;4
WireConnection;3;1;32;3
WireConnection;3;2;2;4
WireConnection;3;3;5;0
WireConnection;3;4;5;0
WireConnection;7;1;10;0
WireConnection;52;0;51;0
WireConnection;52;1;50;0
WireConnection;52;2;2;0
WireConnection;33;0;32;4
WireConnection;33;1;3;0
WireConnection;8;0;7;0
WireConnection;0;2;52;0
WireConnection;0;9;33;0
ASEEND*/
//CHKSM=7E4A638E03FE496E1DCD5116C3F8524ECA93B1AE