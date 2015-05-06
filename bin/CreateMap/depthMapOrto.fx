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
float4x4 dmo_g_mImageMatrix;
float4   dmo_g_mDepthLimits;
float	 dmo_g_imWidth;
float	 dmo_g_imHeight;
float	 dmo_g_nx;
float	 dmo_g_ny;
float	 dmo_g_px;
float	 dmo_g_py;
float	 dmo_g_partw;
float	 dmo_g_parth;

//--------------------------------------------------------------------------------------
// Name: VS
// Type: Vertex Shader Fragment
// Desc: Projection transform
//--------------------------------------------------------------------------------------
void dmo_VS( float4 vPosObject: POSITION,
                 float3 vNormalObject: NORMAL,
                 float2 vTexCoordIn: TEXCOORD0,
                 out float4 vPosProj: POSITION,
                 out float2 vTexCoordOut: TEXCOORD0,
                 out float4 vColorOut: COLOR0
                )
{
    vPosProj = mul(dmo_g_mImageMatrix,vPosObject);
    float d = vPosProj[2]/dmo_g_mDepthLimits[2];
	vColorOut = float4(d,d,d,1.0f);
	
	float x = (vPosProj[0]/dmo_g_mDepthLimits[0])*dmo_g_imWidth;
	float y = (vPosProj[1]/dmo_g_mDepthLimits[1])*dmo_g_imHeight;
	float z = vPosProj[2]/dmo_g_mDepthLimits[2];
	x = (x-dmo_g_px*dmo_g_partw)/dmo_g_partw;
	y = 1.0f-(y-dmo_g_py*dmo_g_parth)/dmo_g_parth;

	//float x = vPosProj[0]/dmo_g_mDepthLimits[0];
	//float y = 1.0f-vPosProj[1]/dmo_g_mDepthLimits[1];
	//float z = vPosProj[2]/dmo_g_mDepthLimits[2];
	
    //Direct3D can use the w-component of a vertex that has been transformed by the world, view, and projection matrices to perform depth-based 
    //calculations in depth-buffer or fog effects. Computations such as these require that your projection matrix normalize w to be equivalent to world-space z. 
	vPosProj[0] = ((x-0.5f)*2.0f);
	vPosProj[1] = ((y-0.5f)*2.0f);    
	vPosProj[2] = z;
    vPosProj[3] = 1.0f;
}


//--------------------------------------------------------------------------------------
// Name: PS
// Type: Pixel Shader
// Desc: 
//--------------------------------------------------------------------------------------
void dmo_PS( float2 vTexCoord: TEXCOORD0,
         float4 vColorIn: COLOR0,
         out float4 vColorOut: COLOR0)
{  
    vColorOut = vColorIn;
	//vColorOut = float4(0.0f,1.0f,0.0f,1.0f);
   
}


//--------------------------------------------------------------------------------------
// Renders scene to render target
//--------------------------------------------------------------------------------------
technique dmo_GenerateDepthMap
{
    pass p0
    {
        VertexShader = compile vs_2_0 dmo_VS();
        PixelShader  = compile ps_2_0 dmo_PS();
    }
}
