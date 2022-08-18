Shader "Amazing Assets/Curved World/Unlit"
{           
	Properties   
	{  

[HideInInspector][CurvedWorldBendSettings]	  _CurvedWorldBendSettings("0,2|1|1", Vector) = (0, 0, 0, 0)


[HideInInspector][MaterialEnum(Front,2,Back,1,Both,0)] _Cull("Face Cull", Int) = 0
 


		[HideInInspector][CurvedWorldToggleFloat] _IncludeVertexColor("", float) = 0
		[HideInInspector] _Color ("Color (RGB)", Color) = (1, 1, 1, 1)
        [HideInInspector] _MainTex ("Base (RGB)", 2D) = "white" { }
		[HideInInspector][CurvedWorldUVScroll] _MainTex_Scroll("", vector) = (0, 0, 0, 0)
        [HideInInspector] _Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5

		[HideInInspector][KeywordEnum(None, By Main Alpha, By Secondary Alpha, Multiple, Additive, By Vertex Alpha)] _TextureMix ("", Float) = 0
		[HideInInspector] _SecondaryTex ("", 2D) = "white" { }
		[HideInInspector][CurvedWorldUVScroll] _SecondaryTex_Scroll("", vector) = (0, 0, 0, 0)
		[HideInInspector] _SecondaryTex_Blend("", Range(-1, 1)) = 0
		
		[HideInInspector] _NormalMapStrength("", float) = 1
		[HideInInspector][Normal] _NormalMap("", 2D) = "bump" {}
		[HideInInspector][CurvedWorldUVScroll] _NormalMap_Scroll ("", vector) = (0, 0, 0, 0)
		[HideInInspector][Normal] _SecondaryNormalMap("", 2D) = "bump"{}
		[HideInInspector][CurvedWorldUVScroll] _SecondaryNormalMap_Scroll ("", vector) = (0, 0, 0, 0)

		[HideInInspector] _ReflectionColor("", color) = (1, 1, 1, 1)
		[HideInInspector] _ReflectionMaskOffset("", Range(-1, 1)) = 0
		[HideInInspector] _ReflectionCubeMap("", Cube) = ""{}	
		[HideInInspector] _ReflectionFresnelBias("", Range(-1, 1)) = 0

		[HideInInspector][HDR] _RimColor("", color) = (1, 1, 1, 1)
		[HideInInspector] _RimBias("", Range(-1, 1)) = 0

		[HideInInspector][HDR] _EmissionColor("", color) = (0, 0, 0, 0)
		[HideInInspector] _EmissionMap("", 2D) = "white"{}
		[HideInInspector][CurvedWorldUVScroll] _EmissionMap_Scroll ("", vector) = (0, 0, 0, 0)

		[HideInInspector] _MatcapMap ("", 2D) = "white"{}
		[HideInInspector] _MatcapIntensity ("", float) = 1
		[HideInInspector][Enum(Multiply,0,Additive,1)] _MatcapBlendMode ("", float) = 1

        // Blending state
        [HideInInspector] _Mode ("__mode", Float) = 0.0
        [HideInInspector] _SrcBlend ("__src", Float) = 1.0
        [HideInInspector] _DstBlend ("__dst", Float) = 0.0
        [HideInInspector] _ZWrite ("__zw", Float) = 1.0
	}

	   
	SubShader 
	{
		Tags { "RenderType"="CurvedWorld_Opaque" }
		LOD 100		     
		            
		Cull[_Cull]     

		//PassName "Unlit"
		Pass       
	    {                 
			Name "Unlit"

			Blend [_SrcBlend] [_DstBlend]
        	ZWrite [_ZWrite]

			CGPROGRAM           
			#pragma vertex vert   
	    	#pragma fragment frag  			         
			#pragma multi_compile_instancing     			                          
			#pragma multi_compile_fog      
			#pragma shader_feature_local _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
			  			
			#pragma shader_feature_local _ _TEXTUREMIX_BY_MAIN_ALPHA _TEXTUREMIX_BY_SECONDARY_ALPHA _TEXTUREMIX_MULTIPLE _TEXTUREMIX_ADDITIVE _TEXTUREMIX_BY_VERTEX_ALPHA
			#pragma shader_feature_local _NORMALMAP
			#pragma shader_feature_local _REFLECTION
			#pragma shader_feature_local _RIM
			#pragma shader_feature_local _EMISSION
			#pragma shader_feature_local _MATCAP

			
#pragma shader_feature_local CURVEDWORLD_BEND_TYPE_CLASSICRUNNER_X_POSITIVE CURVEDWORLD_BEND_TYPE_CLASSICRUNNER_Z_POSITIVE
#define CURVEDWORLD_BEND_ID_1
#pragma shader_feature_local CURVEDWORLD_DISABLED_ON
#pragma shader_feature_local CURVEDWORLD_NORMAL_TRANSFORMATION_ON

			#include "Unlit.cginc" 
			   
			      
			ENDCG    
			        
		} //Pass "Unlit"

		//PassName "ScenePickingPass"
		Pass
        {
            Name "ScenePickingPass"
            Tags { "LightMode" = "Picking" }

            BlendOp Add
            Blend One Zero
            ZWrite On
            Cull Off

            CGPROGRAM
			#include "HLSLSupport.cginc"
			#include "UnityShaderVariables.cginc"
			#include "UnityShaderUtilities.cginc"


            #pragma target 3.0

            #pragma shader_feature_local _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
            #pragma multi_compile_instancing

            #pragma vertex vertEditorPass
            #pragma fragment fragScenePickingPass


#pragma shader_feature_local CURVEDWORLD_BEND_TYPE_CLASSICRUNNER_X_POSITIVE CURVEDWORLD_BEND_TYPE_CLASSICRUNNER_Z_POSITIVE
#define CURVEDWORLD_BEND_ID_1
#pragma shader_feature_local CURVEDWORLD_DISABLED_ON


            #include "../../Core/SceneSelection.cginc" 
            ENDCG
        }	//Pass "ScenePickingPass"		

		//PassName "SceneSelectionPass"
		Pass
        {
            Name "SceneSelectionPass"
            Tags { "LightMode" = "SceneSelectionPass" }

            BlendOp Add
            Blend One Zero
            ZWrite On
            Cull Off

            CGPROGRAM
			#include "HLSLSupport.cginc"
			#include "UnityShaderVariables.cginc"
			#include "UnityShaderUtilities.cginc"


            #pragma target 3.0

            #pragma shader_feature_local _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
            #pragma multi_compile_instancing

            #pragma vertex vertEditorPass
            #pragma fragment fragSceneHighlightPass


#pragma shader_feature_local CURVEDWORLD_BEND_TYPE_CLASSICRUNNER_X_POSITIVE CURVEDWORLD_BEND_TYPE_CLASSICRUNNER_Z_POSITIVE
#define CURVEDWORLD_BEND_ID_1
#pragma shader_feature_local CURVEDWORLD_DISABLED_ON


            #include "../../Core/SceneSelection.cginc" 
            ENDCG
        }	//Pass "SceneSelectionPass"		
		  
	} //SubShader
	    

	Fallback "Hidden/Amazing Assets/Curved World/Fallback/Unlit"
	CustomEditor "AmazingAssets.CurvedWorldEditor.UnlitShaderGUI"
} //Shader
