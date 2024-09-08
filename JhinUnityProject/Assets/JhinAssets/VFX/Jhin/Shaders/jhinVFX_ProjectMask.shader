// Upgrade NOTE: upgraded instancing buffer 'SeroRoninVFXjhinVFX_ProjectMask' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SeroRonin/VFX/jhinVFX_ProjectMask"
{
	Properties
	{
		_MaskBaseTex("MaskBaseTex", 2D) = "white" {}
		_MaskEmissionTex("MaskEmissionTex", 2D) = "white" {}
		_MaskGlitchTex("MaskGlitchTex", 2D) = "white" {}
		_MaskStaticTex("MaskStaticTex", 2D) = "white" {}
		_MaskFrame("Mask Frame", Float) = 0
		_GlitchActive("Glitch Active", Float) = -1
		_GlitchOffset("GlitchOffset", Vector) = (0.5,0.6,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha , One One
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.5
		#pragma multi_compile_instancing
		#pragma surface surf Standard keepalpha noshadow 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _MaskBaseTex;
		uniform sampler2D _MaskEmissionTex;
		uniform float4 _MaskEmissionTex_ST;
		uniform sampler2D _MaskGlitchTex;
		uniform float4 _MaskGlitchTex_ST;
		uniform sampler2D _MaskStaticTex;
		uniform float4 _MaskStaticTex_ST;

		UNITY_INSTANCING_BUFFER_START(SeroRoninVFXjhinVFX_ProjectMask)
			UNITY_DEFINE_INSTANCED_PROP(float4, _MaskBaseTex_ST)
#define _MaskBaseTex_ST_arr SeroRoninVFXjhinVFX_ProjectMask
			UNITY_DEFINE_INSTANCED_PROP(float2, _GlitchOffset)
#define _GlitchOffset_arr SeroRoninVFXjhinVFX_ProjectMask
			UNITY_DEFINE_INSTANCED_PROP(float, _MaskFrame)
#define _MaskFrame_arr SeroRoninVFXjhinVFX_ProjectMask
			UNITY_DEFINE_INSTANCED_PROP(float, _GlitchActive)
#define _GlitchActive_arr SeroRoninVFXjhinVFX_ProjectMask
		UNITY_INSTANCING_BUFFER_END(SeroRoninVFXjhinVFX_ProjectMask)


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 _MaskBaseTex_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_MaskBaseTex_ST_arr, _MaskBaseTex_ST);
			float2 uv_MaskBaseTex = i.uv_texcoord * _MaskBaseTex_ST_Instance.xy + _MaskBaseTex_ST_Instance.zw;
			float4 tex2DNode142 = tex2D( _MaskBaseTex, uv_MaskBaseTex );
			o.Albedo = tex2DNode142.rgb;
			float _MaskFrame_Instance = UNITY_ACCESS_INSTANCED_PROP(_MaskFrame_arr, _MaskFrame);
			float2 uv_MaskEmissionTex = i.uv_texcoord * _MaskEmissionTex_ST.xy + _MaskEmissionTex_ST.zw;
			// *** BEGIN Flipbook UV Animation vars ***
			// Total tiles of Flipbook Texture
			float fbtotaltiles7 = 2.0 * 2.0;
			// Offsets for cols and rows of Flipbook Texture
			float fbcolsoffset7 = 1.0f / 2.0;
			float fbrowsoffset7 = 1.0f / 2.0;
			// Speed of animation
			float fbspeed7 = _Time[ 1 ] * 0.0;
			// UV Tiling (col and row offset)
			float2 fbtiling7 = float2(fbcolsoffset7, fbrowsoffset7);
			// UV Offset - calculate current tile linear index, and convert it to (X * coloffset, Y * rowoffset)
			// Calculate current tile linear index
			float fbcurrenttileindex7 = round( fmod( fbspeed7 + 0.0, fbtotaltiles7) );
			fbcurrenttileindex7 += ( fbcurrenttileindex7 < 0) ? fbtotaltiles7 : 0;
			// Obtain Offset X coordinate from current tile linear index
			float fblinearindextox7 = round ( fmod ( fbcurrenttileindex7, 2.0 ) );
			// Multiply Offset X by coloffset
			float fboffsetx7 = fblinearindextox7 * fbcolsoffset7;
			// Obtain Offset Y coordinate from current tile linear index
			float fblinearindextoy7 = round( fmod( ( fbcurrenttileindex7 - fblinearindextox7 ) / 2.0, 2.0 ) );
			// Reverse Y to get tiles from Top to Bottom
			fblinearindextoy7 = (int)(2.0-1) - fblinearindextoy7;
			// Multiply Offset Y by rowoffset
			float fboffsety7 = fblinearindextoy7 * fbrowsoffset7;
			// UV Offset
			float2 fboffset7 = float2(fboffsetx7, fboffsety7);
			// Flipbook UV
			half2 fbuv7 = uv_MaskEmissionTex * fbtiling7 + fboffset7;
			// *** END Flipbook UV Animation vars ***
			float4 tex2DNode10 = tex2D( _MaskEmissionTex, fbuv7 );
			float fbtotaltiles41 = 2.0 * 2.0;
			float fbcolsoffset41 = 1.0f / 2.0;
			float fbrowsoffset41 = 1.0f / 2.0;
			float fbspeed41 = _Time[ 1 ] * 0.0;
			float2 fbtiling41 = float2(fbcolsoffset41, fbrowsoffset41);
			float fbcurrenttileindex41 = round( fmod( fbspeed41 + 1.0, fbtotaltiles41) );
			fbcurrenttileindex41 += ( fbcurrenttileindex41 < 0) ? fbtotaltiles41 : 0;
			float fblinearindextox41 = round ( fmod ( fbcurrenttileindex41, 2.0 ) );
			float fboffsetx41 = fblinearindextox41 * fbcolsoffset41;
			float fblinearindextoy41 = round( fmod( ( fbcurrenttileindex41 - fblinearindextox41 ) / 2.0, 2.0 ) );
			fblinearindextoy41 = (int)(2.0-1) - fblinearindextoy41;
			float fboffsety41 = fblinearindextoy41 * fbrowsoffset41;
			float2 fboffset41 = float2(fboffsetx41, fboffsety41);
			half2 fbuv41 = uv_MaskEmissionTex * fbtiling41 + fboffset41;
			float2 temp_output_1_0_g11 = float2( 0,0 );
			float temp_output_6_0_g11 = _SinTime.w;
			float temp_output_16_0_g11 = (temp_output_1_0_g11).y;
			float YVal31_g11 = ( ( 10.0 * cos( ( ( UNITY_PI * (temp_output_1_0_g11).x ) + ( UNITY_PI * temp_output_6_0_g11 ) ) ) * sin( ( ( temp_output_16_0_g11 * UNITY_PI ) + ( 5.0 / 3.0 ) + ( temp_output_6_0_g11 * UNITY_PI ) ) ) ) + temp_output_16_0_g11 );
			float2 temp_cast_1 = (abs( ( 1.0 / ( ( YVal31_g11 * 1.0 ) / 1.0 ) ) )).xx;
			float simplePerlin2D141 = snoise( temp_cast_1 );
			simplePerlin2D141 = simplePerlin2D141*0.5 + 0.5;
			float cos132 = cos( ( simplePerlin2D141 / 25.0 ) );
			float sin132 = sin( ( simplePerlin2D141 / 25.0 ) );
			float2 rotator132 = mul( fbuv41 - float2( 0.75,0.75 ) , float2x2( cos132 , -sin132 , sin132 , cos132 )) + float2( 0.75,0.75 );
			float fbtotaltiles43 = 2.0 * 2.0;
			float fbcolsoffset43 = 1.0f / 2.0;
			float fbrowsoffset43 = 1.0f / 2.0;
			float fbspeed43 = _Time[ 1 ] * 0.0;
			float2 fbtiling43 = float2(fbcolsoffset43, fbrowsoffset43);
			float fbcurrenttileindex43 = round( fmod( fbspeed43 + 2.0, fbtotaltiles43) );
			fbcurrenttileindex43 += ( fbcurrenttileindex43 < 0) ? fbtotaltiles43 : 0;
			float fblinearindextox43 = round ( fmod ( fbcurrenttileindex43, 2.0 ) );
			float fboffsetx43 = fblinearindextox43 * fbcolsoffset43;
			float fblinearindextoy43 = round( fmod( ( fbcurrenttileindex43 - fblinearindextox43 ) / 2.0, 2.0 ) );
			fblinearindextoy43 = (int)(2.0-1) - fblinearindextoy43;
			float fboffsety43 = fblinearindextoy43 * fbrowsoffset43;
			float2 fboffset43 = float2(fboffsetx43, fboffsety43);
			half2 fbuv43 = uv_MaskEmissionTex * fbtiling43 + fboffset43;
			float2 break109 = fbuv43;
			float2 temp_output_1_0_g10 = float2( 0,0 );
			float2 break19_g9 = float2( 0,1 );
			float temp_output_1_0_g9 = _SinTime.w;
			float sinIn7_g9 = sin( temp_output_1_0_g9 );
			float sinInOffset6_g9 = sin( ( temp_output_1_0_g9 + 1.0 ) );
			float lerpResult20_g9 = lerp( break19_g9.x , break19_g9.y , frac( ( sin( ( ( sinIn7_g9 - sinInOffset6_g9 ) * 91.2228 ) ) * 43758.55 ) ));
			float temp_output_6_0_g10 = ( lerpResult20_g9 + sinIn7_g9 );
			float temp_output_16_0_g10 = (temp_output_1_0_g10).y;
			float YVal31_g10 = ( ( 10.0 * cos( ( ( UNITY_PI * (temp_output_1_0_g10).x ) + ( UNITY_PI * temp_output_6_0_g10 ) ) ) * sin( ( ( temp_output_16_0_g10 * UNITY_PI ) + ( 5.0 / 3.0 ) + ( temp_output_6_0_g10 * UNITY_PI ) ) ) ) + temp_output_16_0_g10 );
			float temp_output_91_0 = abs( ( 1.0 / ( ( YVal31_g10 * 1.0 ) / 1.0 ) ) );
			float4 appendResult111 = (float4(break109.x , ( break109.y + ( temp_output_91_0 / 750.0 ) ) , 0.0 , 0.0));
			float fbtotaltiles45 = 2.0 * 2.0;
			float fbcolsoffset45 = 1.0f / 2.0;
			float fbrowsoffset45 = 1.0f / 2.0;
			float fbspeed45 = _Time[ 1 ] * 0.0;
			float2 fbtiling45 = float2(fbcolsoffset45, fbrowsoffset45);
			float fbcurrenttileindex45 = round( fmod( fbspeed45 + 3.0, fbtotaltiles45) );
			fbcurrenttileindex45 += ( fbcurrenttileindex45 < 0) ? fbtotaltiles45 : 0;
			float fblinearindextox45 = round ( fmod ( fbcurrenttileindex45, 2.0 ) );
			float fboffsetx45 = fblinearindextox45 * fbcolsoffset45;
			float fblinearindextoy45 = round( fmod( ( fbcurrenttileindex45 - fblinearindextox45 ) / 2.0, 2.0 ) );
			fblinearindextoy45 = (int)(2.0-1) - fblinearindextoy45;
			float fboffsety45 = fblinearindextoy45 * fbrowsoffset45;
			float2 fboffset45 = float2(fboffsetx45, fboffsety45);
			half2 fbuv45 = uv_MaskEmissionTex * fbtiling45 + fboffset45;
			float2 break101 = fbuv45;
			float4 appendResult106 = (float4(( break101.x + ( temp_output_91_0 / 100.0 ) ) , break101.y , 0.0 , 0.0));
			float4 temp_output_85_0 = ( _MaskFrame_Instance == 0.0 ? tex2DNode10 : ( _MaskFrame_Instance == 1.0 ? tex2D( _MaskEmissionTex, rotator132 ) : ( _MaskFrame_Instance == 2.0 ? tex2D( _MaskEmissionTex, appendResult111.xy ) : ( _MaskFrame_Instance == 3.0 ? tex2D( _MaskEmissionTex, appendResult106.xy ) : tex2DNode10 ) ) ) );
			float4 blendOpSrc147 = tex2DNode142;
			float4 blendOpDest147 = temp_output_85_0;
			float _GlitchActive_Instance = UNITY_ACCESS_INSTANCED_PROP(_GlitchActive_arr, _GlitchActive);
			float2 _GlitchOffset_Instance = UNITY_ACCESS_INSTANCED_PROP(_GlitchOffset_arr, _GlitchOffset);
			float2 uv_MaskGlitchTex = i.uv_texcoord * _MaskGlitchTex_ST.xy + _MaskGlitchTex_ST.zw;
			float2 panner127 = ( 1.0 * _GlitchOffset_Instance + uv_MaskGlitchTex);
			float2 uv_MaskStaticTex = i.uv_texcoord * _MaskStaticTex_ST.xy + _MaskStaticTex_ST.zw;
			float2 break175 = uv_MaskStaticTex;
			float4 appendResult176 = (float4(( break175.x * 10 ) , ( break175.y * 4 ) , 0.0 , 0.0));
			float2 panner159 = ( _Time.y * float2( 0,0.2 ) + appendResult176.xy);
			float2 panner181 = ( _Time.y * float2( 0,0.7 ) + appendResult176.xy);
			o.Emission = ( ( saturate( 2.0f*blendOpDest147*blendOpSrc147 + blendOpDest147*blendOpDest147*(1.0f - 2.0f*blendOpSrc147) )) * ( ( temp_output_85_0.a * ( _GlitchActive_Instance == 0.0 ? 1.0 : ( tex2D( _MaskGlitchTex, panner127 ).a + 0.0 ) ) ) * (float4( 0.5,0.5,0.5,0 ) + (( tex2D( _MaskStaticTex, ( ( temp_output_91_0 / 500.0 ) + panner159 ) ) * (float4( 0.5,0.5,0.5,0 ) + (tex2D( _MaskStaticTex, ( panner181 + ( temp_output_91_0 / 600.0 ) ) ) - float4( 0,0,0,0 )) * (float4( 1,1,1,1 ) - float4( 0.5,0.5,0.5,0 )) / (float4( 1,1,1,1 ) - float4( 0,0,0,0 ))) ) - float4( 0,0,0,0 )) * (float4( 1,1,1,1 ) - float4( 0.5,0.5,0.5,0 )) / (float4( 1,1,1,1 ) - float4( 0,0,0,0 ))) ) ).rgb;
			o.Metallic = 1.0;
			o.Smoothness = 0.9;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18935
781;73;1174;916;-426.1932;1904.003;1;True;False
Node;AmplifyShaderEditor.SinTimeNode;96;-2237.064,-495.0531;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;2;-2221.897,-1017.968;Inherit;True;Property;_MaskEmissionTex;MaskEmissionTex;1;0;Create;True;0;0;0;False;0;False;a2068a9ea9e575c49b30f9f4f0d426b8;a2068a9ea9e575c49b30f9f4f0d426b8;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TextureCoordinatesNode;8;-1871.07,-1113.193;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;92;-2070.168,-401.8412;Inherit;False;Noise Sine Wave;-1;;9;a6eff29f739ced848846e3b648af87bd;0;2;1;FLOAT;0;False;2;FLOAT2;0,1;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;91;-1835.93,-472.2845;Inherit;False;CoolWave;-1;;10;a4ec317493edf3b439fcd463a40eca0d;0;6;35;FLOAT;10;False;4;FLOAT;1;False;6;FLOAT;0;False;3;FLOAT;1;False;1;FLOAT2;0,0;False;2;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCFlipBookUVAnimation;45;-1536,-768;Inherit;False;0;0;6;0;FLOAT2;0,0;False;1;FLOAT;2;False;2;FLOAT;2;False;3;FLOAT;0;False;4;FLOAT;3;False;5;FLOAT;0;False;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TexturePropertyNode;167;-1408,0;Inherit;True;Property;_MaskStaticTex;MaskStaticTex;3;0;Create;True;0;0;0;False;0;False;8c37af5f6c26d96428e939f67d902381;8c37af5f6c26d96428e939f67d902381;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.BreakToComponentsNode;101;-1280,-768;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.TFHCFlipBookUVAnimation;43;-1536,-1024;Inherit;False;0;0;6;0;FLOAT2;0,0;False;1;FLOAT;2;False;2;FLOAT;2;False;3;FLOAT;0;False;4;FLOAT;2;False;5;FLOAT;0;False;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleDivideOpNode;107;-1280,-672;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;100;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;168;-1152,0;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BreakToComponentsNode;175;-896,0;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleDivideOpNode;113;-1280,-928;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;750;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;138;-1840,-704;Inherit;False;CoolWave;-1;;11;a4ec317493edf3b439fcd463a40eca0d;0;6;35;FLOAT;10;False;4;FLOAT;1;False;6;FLOAT;0;False;3;FLOAT;1;False;1;FLOAT2;0,0;False;2;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;109;-1280,-1024;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleAddOpNode;93;-1024,-800;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleNode;177;-768,64;Inherit;False;4;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;106;-832,-768;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.WireNode;136;-1846.574,-718.138;Inherit;False;1;0;SAMPLER2D;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TFHCFlipBookUVAnimation;7;-1536,-1536;Inherit;False;0;0;6;0;FLOAT2;0,0;False;1;FLOAT;2;False;2;FLOAT;2;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.ScaleNode;172;-768,0;Inherit;False;10;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;135;-1803.773,-1412.24;Inherit;False;1;0;SAMPLER2D;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;141;-1789.146,-847.1282;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;108;-1024,-960;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;176;-608,0;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TimeNode;165;-640,384;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleDivideOpNode;134;-1280,-1152;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;25;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;10;-640,-1536;Inherit;True;Property;_TextureSample1;Texture Sample 1;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;111;-832,-1024;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;46;-640,-768;Inherit;True;Property;_TextureSample3;Texture Sample 3;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCFlipBookUVAnimation;41;-1536,-1280;Inherit;False;0;0;6;0;FLOAT2;0,0;False;1;FLOAT;2;False;2;FLOAT;2;False;3;FLOAT;0;False;4;FLOAT;1;False;5;FLOAT;0;False;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;81;-551.6782,-1651.359;Inherit;False;InstancedProperty;_MaskFrame;Mask Frame;4;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;114;-1408,-512;Inherit;True;Property;_MaskGlitchTex;MaskGlitchTex;2;0;Create;True;0;0;0;False;0;False;6052590aed7737041950eac485bf603a;6052590aed7737041950eac485bf603a;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.PannerNode;181;-380.8997,258.011;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0.7;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;44;-640,-1024;Inherit;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleDivideOpNode;184;-1264,288;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;600;False;1;FLOAT;0
Node;AmplifyShaderEditor.Compare;88;-256,-768;Inherit;False;0;4;0;FLOAT;0;False;1;FLOAT;3;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector2Node;128;-1152,-256;Inherit;False;InstancedProperty;_GlitchOffset;GlitchOffset;6;0;Create;True;0;0;0;False;0;False;0.5,0.6;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RotatorNode;132;-1280,-1280;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0.75,0.75;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;130;-1152,-128;Inherit;False;Constant;_Float3;Float 3;4;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;118;-1152,-384;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;185;-176,256;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;173;-1264,192;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;500;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;159;-384,0;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0.2;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Compare;87;-128,-1024;Inherit;False;0;4;0;FLOAT;2;False;1;FLOAT;2;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PannerNode;127;-896,-288;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;42;-640,-1280;Inherit;True;Property;_TextureSample2;Texture Sample 2;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;119;-640,-512;Inherit;True;Property;_TextureSample4;Texture Sample 4;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;182;0,256;Inherit;True;Property;_TextureSample7;Texture Sample 6;7;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Compare;86;0,-1280;Inherit;False;0;4;0;FLOAT;0;False;1;FLOAT;1;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;174;-176,0;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;116;-256,-512;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Compare;85;128,-1552;Inherit;False;0;4;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;122;0,-592;Inherit;False;InstancedProperty;_GlitchActive;Glitch Active;5;0;Create;True;0;0;0;True;0;False;-1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;186;384,256;Inherit;False;5;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,1;False;3;COLOR;0.5,0.5,0.5,0;False;4;COLOR;1,1,1,1;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;171;0,0;Inherit;True;Property;_TextureSample6;Texture Sample 6;7;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;183;512,0;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.BreakToComponentsNode;125;256,-1280;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.Compare;126;0,-512;Inherit;True;0;4;0;FLOAT;2;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;192;128,-1984;Inherit;True;Property;_MaskBaseTex;MaskBaseTex;0;0;Create;True;0;0;0;False;0;False;d39a40aec07a07645b4b29f1ebdb863b;d39a40aec07a07645b4b29f1ebdb863b;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;124;416,-1280;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;180;768,-128;Inherit;False;5;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,1;False;3;COLOR;0.5,0.5,0.5,0;False;4;COLOR;1,1,1,1;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;142;128,-1792;Inherit;True;Property;_TextureSample5;Texture Sample 5;0;0;Create;True;0;0;0;False;0;False;-1;d39a40aec07a07645b4b29f1ebdb863b;d39a40aec07a07645b4b29f1ebdb863b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendOpsNode;147;512,-1536;Inherit;False;SoftLight;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;162;704,-1280;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;193;1024,-1328;Inherit;False;Constant;_Float1;Float 0;6;0;Create;True;0;0;0;False;0;False;0.9;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;145;1024,-1536;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;148;1024,-1408;Inherit;False;Constant;_Float0;Float 0;6;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;1;1280,-1792;Float;False;True;-1;3;ASEMaterialInspector;0;0;Standard;SeroRonin/VFX/jhinVFX_ProjectMask;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;False;0;False;Opaque;;Geometry;All;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;4;1;False;-1;1;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;8;2;2;0
WireConnection;92;1;96;4
WireConnection;91;6;92;0
WireConnection;45;0;8;0
WireConnection;101;0;45;0
WireConnection;43;0;8;0
WireConnection;107;0;91;0
WireConnection;168;2;167;0
WireConnection;175;0;168;0
WireConnection;113;0;91;0
WireConnection;138;6;96;4
WireConnection;109;0;43;0
WireConnection;93;0;101;0
WireConnection;93;1;107;0
WireConnection;177;0;175;1
WireConnection;106;0;93;0
WireConnection;106;1;101;1
WireConnection;136;0;2;0
WireConnection;7;0;8;0
WireConnection;172;0;175;0
WireConnection;135;0;2;0
WireConnection;141;0;138;0
WireConnection;108;0;109;1
WireConnection;108;1;113;0
WireConnection;176;0;172;0
WireConnection;176;1;177;0
WireConnection;134;0;141;0
WireConnection;10;0;135;0
WireConnection;10;1;7;0
WireConnection;111;0;109;0
WireConnection;111;1;108;0
WireConnection;46;0;136;0
WireConnection;46;1;106;0
WireConnection;41;0;8;0
WireConnection;181;0;176;0
WireConnection;181;1;165;2
WireConnection;44;0;2;0
WireConnection;44;1;111;0
WireConnection;184;0;91;0
WireConnection;88;0;81;0
WireConnection;88;2;46;0
WireConnection;88;3;10;0
WireConnection;132;0;41;0
WireConnection;132;2;134;0
WireConnection;118;2;114;0
WireConnection;185;0;181;0
WireConnection;185;1;184;0
WireConnection;173;0;91;0
WireConnection;159;0;176;0
WireConnection;159;1;165;2
WireConnection;87;0;81;0
WireConnection;87;2;44;0
WireConnection;87;3;88;0
WireConnection;127;0;118;0
WireConnection;127;2;128;0
WireConnection;127;1;130;0
WireConnection;42;0;2;0
WireConnection;42;1;132;0
WireConnection;119;0;114;0
WireConnection;119;1;127;0
WireConnection;182;0;167;0
WireConnection;182;1;185;0
WireConnection;86;0;81;0
WireConnection;86;2;42;0
WireConnection;86;3;87;0
WireConnection;174;0;173;0
WireConnection;174;1;159;0
WireConnection;116;0;119;4
WireConnection;85;0;81;0
WireConnection;85;2;10;0
WireConnection;85;3;86;0
WireConnection;186;0;182;0
WireConnection;171;0;167;0
WireConnection;171;1;174;0
WireConnection;183;0;171;0
WireConnection;183;1;186;0
WireConnection;125;0;85;0
WireConnection;126;0;122;0
WireConnection;126;3;116;0
WireConnection;124;0;125;3
WireConnection;124;1;126;0
WireConnection;180;0;183;0
WireConnection;142;0;192;0
WireConnection;147;0;142;0
WireConnection;147;1;85;0
WireConnection;162;0;124;0
WireConnection;162;1;180;0
WireConnection;145;0;147;0
WireConnection;145;1;162;0
WireConnection;1;0;142;0
WireConnection;1;2;145;0
WireConnection;1;3;148;0
WireConnection;1;4;193;0
ASEEND*/
//CHKSM=7186BBD9BA92F635B0C4153AE66D4DA7D3305DED