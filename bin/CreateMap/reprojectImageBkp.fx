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
texture  g_pImage;
float	 g_imWidth;
float	 g_imHeight;

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


//--------------------------------------------------------------------------------------
// Name: VS
// Type: Vertex Shader Fragment
// Desc: Projection transform
//--------------------------------------------------------------------------------------
void VS( float4 vPosObject: POSITION,
                 float3 vNormalObject: NORMAL,
                 float2 vTexCoordIn: TEXCOORD0,
                 out float4 vPosProj: POSITION,
                 out float2 vTexCoordOut: TEXCOORD0,
                 out float4 vColorOut: COLOR0
                )
{
    vPosProj = mul(vPosObject, g_mRCProjViewMatrix);
    vPosProj[1] = -vPosProj[1];
	vColorOut = float4(1.0f,0.0f,0.0f,1.0f);
    float4 tcPix = mul(g_mTCProjMatrix, vPosObject);
	vTexCoordOut[0] = ((tcPix[0]/tcPix[3])/tcPix[2])/g_imWidth;
	vTexCoordOut[1] = ((tcPix[1]/tcPix[3])/tcPix[2])/g_imHeight;
}


//--------------------------------------------------------------------------------------
// Name: PS
// Type: Pixel Shader
// Desc: 
//--------------------------------------------------------------------------------------
void PS( float2 vTexCoord: TEXCOORD0,
         float4 vColorIn: COLOR0,
         out float4 vColorOut: COLOR0)
{ 
    vColorOut = vColorIn;
    vColorOut.rgb = tex2D(ImageSampler, vTexCoord);
    vColorOut.a = 1.0f;
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
