//--------------------------------------------------------------------------------------
// File: BasicHLSL.fx
//
// The effect file for the BasicHLSL sample.  
// 
// Copyright (c) Microsoft Corporation. All rights reserved.
//--------------------------------------------------------------------------------------


//--------------------------------------------------------------------------------------
// Global variables
//--------------------------------------------------------------------------------------
float4x4 g_mRCProjViewMatrix;
float4x4 g_mTCProjMatrix;
float4x4 g_mRCProjMatrix;
//texture  g_pImage;
float	 g_imWidth;
float	 g_imHeight;
float	 g_nx;
float	 g_ny;
float	 g_px;
float	 g_py;
float	 g_partw;
float	 g_parth;

float3 CameraPos;  
float3 LightPosition;  
float3 LightDiffuseColor; // intensity multiplier  
float3 LightSpecularColor; // intensity multiplier  
float LightDistanceSquared;  
float3 DiffuseColor;  
float3 AmbientLightColor;  
float3 SpecularColor;  
float SpecularPower;  
float4 DepthLimits;


/*
//--------------------------------------------------------------------------------------
// Texture samplers
//--------------------------------------------------------------------------------------
sampler ImageSampler = 
sampler_state
{
    Texture = <g_pImage>;
    MipFilter = LINEAR;
    MinFilter = LINEAR;
    MagFilter = LINEAR;
    AddressU = CLAMP;
    AddressV = CLAMP;
};
*/

//--------------------------------------------------------------------------------------
// Name: VS
// Type: Vertex Shader Fragment
// Desc: Projection transform
//--------------------------------------------------------------------------------------
void VS( float4 vPosObject: POSITION,
         float3 vNormalObject: NORMAL,
         float2 vTexCoordIn: TEXCOORD0,
         out float4 vPosProj: POSITION,
         out float3 Normal: TEXCOORD0,
         out float2 vTexCoordOut: TEXCOORD1,
         out float3 WorldPos: COLOR0
       )
{
    //vPosProj = mul(vPosObject, g_mRCProjViewMatrix);
    //vPosProj[1] = -vPosProj[1];
    
    
    float d = 
	sqrt((CameraPos[0]-vPosObject[0])*
		 (CameraPos[0]-vPosObject[0])+
		 (CameraPos[1]-vPosObject[1])*
		 (CameraPos[1]-vPosObject[1])+
		 (CameraPos[2]-vPosObject[2])*
		 (CameraPos[2]-vPosObject[2]))/DepthLimits[0];
      
      
    vPosProj = mul(g_mRCProjMatrix,vPosObject);
	float x = vPosProj[0]/vPosProj[2];
	float y = vPosProj[1]/vPosProj[2];
	
	x = (x-g_px*g_partw)/g_partw;
	y = 1.0f-(y-g_py*g_parth)/g_parth;
	
	vPosProj[0] = (x-0.5f)*2.0f;
	vPosProj[1] = (y-0.5f)*2.0f;    
	vPosProj[2] = d;
    vPosProj[3] = 1.0f;
    
    
            
    //vPosProj = mul(g_mRCProjMatrix,vPosObject);
	//vPosProj[0] = ((     (vPosProj[0]/vPosProj[2])/g_imWidth )-0.5f)*2.0f;
	//vPosProj[1] = ((1.0f-(vPosProj[1]/vPosProj[2])/g_imHeight)-0.5f)*2.0f;    
	//vPosProj[2] = d;
    //vPosProj[3] = 1.0f;

	Normal = vNormalObject;
	WorldPos.x = vPosObject.x;
	WorldPos.y = vPosObject.y;
	WorldPos.z = vPosObject.z;
    float4 tcPix = mul(g_mTCProjMatrix, vPosObject);
	vTexCoordOut[0] = ((tcPix[0]/tcPix[3])/tcPix[2])/g_imWidth;
	vTexCoordOut[1] = ((tcPix[1]/tcPix[3])/tcPix[2])/g_imHeight;
}


//--------------------------------------------------------------------------------------
// Name: PS
// Type: Pixel Shader
// Desc: 
//--------------------------------------------------------------------------------------
void PS( float3 Normal: TEXCOORD0,
		 float2 vTexCoord: TEXCOORD1,
         float3 WorldPos: COLOR0,
         out float4 vColorOut: COLOR0)
{
	vColorOut[0] = vTexCoord[0];
	vColorOut[1] = vTexCoord[1];
	vColorOut[2] = 1.0f;
	vColorOut[3] = 1.0f;
}


//--------------------------------------------------------------------------------------
// Renders scene to render target
//--------------------------------------------------------------------------------------
technique GenerateDepthMap
{
    pass p0
    {
        VertexShader = compile vs_2_0 VS();
        PixelShader  = compile ps_2_0 PS();
    }
}
