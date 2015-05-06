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
float4x4 g_mImageMatrix;
float4x4 g_mRCProjMatrix;
float4x4 g_mRCCamMatrix;
float4x4 g_mRCImMatrix;
float4   g_mCameraCentre;
float4   g_mDepthLimits;
float	 g_imWidth;
float	 g_imHeight;
float	 g_nx;
float	 g_ny;
float	 g_px;
float	 g_py;
float	 g_partw;
float	 g_parth;

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
    float d = 
	sqrt((g_mCameraCentre[0]-vPosObject[0])*
		 (g_mCameraCentre[0]-vPosObject[0])+
		 (g_mCameraCentre[1]-vPosObject[1])*
		 (g_mCameraCentre[1]-vPosObject[1])+
		 (g_mCameraCentre[2]-vPosObject[2])*
		 (g_mCameraCentre[2]-vPosObject[2]));

    vPosProj = mul(g_mRCProjMatrix,vPosObject);
		 
    if (vPosProj[2]<0.0f) {
		d = -d;
    };   
    d = (d-g_mDepthLimits[1])/(g_mDepthLimits[0]-g_mDepthLimits[1]);
	vColorOut = float4(d,d,d,1.0f);
		 

	float x = vPosProj[0]/vPosProj[2];
	float y = vPosProj[1]/vPosProj[2];
	float z = vPosProj[2];
	
	x = (x-g_px*g_partw)/g_partw;
	y = 1.0f-(y-g_py*g_parth)/g_parth;
	
		
    //Direct3D can use the w-component of a vertex that has been transformed by the world, view, and projection matrices to perform depth-based 
    //calculations in depth-buffer or fog effects. Computations such as these require that your projection matrix normalize w to be equivalent to world-space z. 
	vPosProj[0] = ((x-0.5f)*2.0f)*z;
	vPosProj[1] = ((y-0.5f)*2.0f)*z;    
	//vPosProj[2] = ((z-g_mDepthLimits[1])/(g_mDepthLimits[0]-g_mDepthLimits[1]))*g_mDepthLimits[1];
	vPosProj[2] = d*z;
    vPosProj[3] = z;
    
    
    /*
	vPosProj[0] = ((x-0.5f)*2.0f);
	vPosProj[1] = ((y-0.5f)*2.0f);    
	vPosProj[2] = d;
    vPosProj[3] = 1.0f;
	*/

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
