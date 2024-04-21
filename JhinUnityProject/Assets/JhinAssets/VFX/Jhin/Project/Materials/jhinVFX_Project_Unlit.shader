// Upgrade NOTE: upgraded instancing buffer 'SeroRoninVFXjhinVFX_Project_Unlit' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SeroRonin/VFX/jhinVFX_Project_Unlit"
{
	Properties
	{
		_Texture0("Texture 0", 2D) = "white" {}
		_UseAlphaCurve("Use Alpha Curve", Int) = 0
		_UseTexRGB("Use Tex RGB", Int) = 0
		_GlowBrightness("Glow Brightness", Float) = 1
		_MixColorwithTex("Mix Color with Tex", Int) = 0
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
			float4 vertexColor : COLOR;
			float4 uv_texcoord;
		};

		uniform int _MixColorwithTex;
		uniform float _GlowBrightness;
		uniform sampler2D _Texture0;
		uniform int _UseAlphaCurve;

		UNITY_INSTANCING_BUFFER_START(SeroRoninVFXjhinVFX_Project_Unlit)
			UNITY_DEFINE_INSTANCED_PROP(float4, _Texture0_ST)
#define _Texture0_ST_arr SeroRoninVFXjhinVFX_Project_Unlit
			UNITY_DEFINE_INSTANCED_PROP(int, _UseTexRGB)
#define _UseTexRGB_arr SeroRoninVFXjhinVFX_Project_Unlit
		UNITY_INSTANCING_BUFFER_END(SeroRoninVFXjhinVFX_Project_Unlit)

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			int _UseTexRGB_Instance = UNITY_ACCESS_INSTANCED_PROP(_UseTexRGB_arr, _UseTexRGB);
			float4 temp_output_80_0 = ( i.vertexColor * _GlowBrightness );
			float4 _Texture0_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture0_ST_arr, _Texture0_ST);
			float2 uv_Texture0 = i.uv_texcoord * _Texture0_ST_Instance.xy + _Texture0_ST_Instance.zw;
			float4 tex2DNode1 = tex2D( _Texture0, uv_Texture0 );
			o.Emission = ( (float)_UseTexRGB_Instance == 1.0 ? ( (float)_MixColorwithTex == 1.0 ? ( temp_output_80_0 * tex2DNode1 ) : tex2DNode1 ) : temp_output_80_0 ).rgb;
			o.Alpha = ( i.vertexColor.a * ( (float)_UseAlphaCurve == 1.0 ? i.uv_texcoord.z : 1.0 ) * tex2DNode1.a );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Unlit alpha:fade keepalpha fullforwardshadows exclude_path:deferred 

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
				o.customPack1.xyzw = customInputData.uv_texcoord;
				o.customPack1.xyzw = v.texcoord;
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
				surfIN.uv_texcoord = IN.customPack1.xyzw;
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
487;81;1217;759;1502.83;646.5906;2.174072;True;False
Node;AmplifyShaderEditor.TexturePropertyNode;8;-569.605,94.06843;Inherit;True;Property;_Texture0;Texture 0;0;0;Create;True;0;0;0;False;0;False;171e1f34bc1f8d8459eaed8f9234d087;f7d8d86f8163777498e75d101b665195;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.VertexColorNode;77;-552.663,-163.2246;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;81;-556.2205,-244.9735;Inherit;False;Property;_GlowBrightness;Glow Brightness;3;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-298.8997,96.20002;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;80;-352,-256;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;82;-556.6018,333.247;Inherit;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;94;-225,-184;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.IntNode;84;-554.1494,495.2322;Inherit;False;Property;_UseAlphaCurve;Use Alpha Curve;1;0;Create;True;0;0;0;False;0;False;0;0;False;0;1;INT;0
Node;AmplifyShaderEditor.IntNode;92;-64,-340;Inherit;False;Property;_MixColorwithTex;Mix Color with Tex;4;0;Create;True;0;0;0;False;0;False;0;0;False;0;1;INT;0
Node;AmplifyShaderEditor.Compare;83;-253.1494,426.2322;Inherit;False;0;4;0;INT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;3;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Compare;93;-64,-260;Inherit;False;0;4;0;INT;0;False;1;FLOAT;1;False;2;COLOR;0,0,0,0;False;3;COLOR;1,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.IntNode;118;133.3563,-336.4431;Inherit;False;InstancedProperty;_UseTexRGB;Use Tex RGB;2;0;Create;True;0;0;0;False;0;False;0;0;False;0;1;INT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;79;15.50001,299.8999;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Compare;90;128,-256;Inherit;False;0;4;0;INT;0;False;1;FLOAT;1;False;2;COLOR;0,0,0,0;False;3;COLOR;1,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;384,-128;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;SeroRonin/VFX/jhinVFX_Project_Unlit;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;ForwardOnly;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;5;4;False;-1;1;False;-1;1;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;1;0;8;0
WireConnection;80;0;77;0
WireConnection;80;1;81;0
WireConnection;94;0;80;0
WireConnection;94;1;1;0
WireConnection;83;0;84;0
WireConnection;83;2;82;3
WireConnection;93;0;92;0
WireConnection;93;2;94;0
WireConnection;93;3;1;0
WireConnection;79;0;77;4
WireConnection;79;1;83;0
WireConnection;79;2;1;4
WireConnection;90;0;118;0
WireConnection;90;2;93;0
WireConnection;90;3;80;0
WireConnection;0;2;90;0
WireConnection;0;9;79;0
ASEEND*/
//CHKSM=7FFAE876CDF836D4682AE981C7643E320DD78454