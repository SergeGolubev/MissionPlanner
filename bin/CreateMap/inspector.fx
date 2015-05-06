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
float3   g_vCameraPosition : CameraPosition;
float4x4 g_mWorld : World;          // World matrix
float4x4 g_mWorldViewProjection : WorldViewProjection; // World * View * Projection matrix

float3 LightPosition;  
float3 LightDiffuseColor; // intensity multiplier  
float3 LightSpecularColor; // intensity multiplier  
float  LightDistanceSquared;  
float3 DiffuseColor;  
float3 AmbientLightColor;  
float3 SpecularColor;  
float  SpecularPower;  


//--------------------------------------------------------------------------------------
// Name: VS
// Type: Vertex Shader Fragment
// Desc: Projection transform
//--------------------------------------------------------------------------------------
void VS( float4 vPosObject: POSITION,
         float3 vNormalObject: NORMAL,
         out float4 vPosProj: POSITION,
         out float3 Normal: TEXCOORD0,
         out float3 WorldPos: COLOR0
       )
{
    // Transform the position into world space for lighting, and projected space
    // for display
    float4 vPosWorld = mul( vPosObject, g_mWorld );
    vPosProj = mul( vPosObject, g_mWorldViewProjection );

    // Transform the normal into world space for lighting
    Normal = mul( vNormalObject, (float3x3)g_mWorld );
    
	WorldPos.x = vPosObject.x;
	WorldPos.y = vPosObject.y;
	WorldPos.z = vPosObject.z;
}


//--------------------------------------------------------------------------------------
// Name: PS
// Type: Pixel Shader
// Desc: 
//--------------------------------------------------------------------------------------
void PS( float3 Normal: TEXCOORD0,
         float3 WorldPos: COLOR0,
         out float4 vColorOut: COLOR0)
{
	// Get light direction for this fragment  
	//float3 lightDir = normalize(WorldPos - LightPosition);
	float3 lightDir = normalize(WorldPos - g_vCameraPosition);
	
	// Note: Non-uniform scaling not supported  
	float diffuseLighting = saturate(dot(Normal, -lightDir));  
   
	// Introduce fall-off of light intensity  
	//diffuseLighting *= (LightDistanceSquared / dot(LightPosition - WorldPos, LightPosition - WorldPos));  

	// Using Blinn half angle modification for perofrmance over correctness  
	float3 h = normalize(normalize(g_vCameraPosition - WorldPos) - lightDir);  
	float specLighting = pow(saturate(dot(h, Normal)), SpecularPower);  

	/*
	vColorOut = float4(saturate(  
		AmbientLightColor +  
		(texel.xyz * DiffuseColor * LightDiffuseColor * diffuseLighting * 0.6) + // Use light diffuse vector as intensity multiplier  
		(SpecularColor * LightSpecularColor * specLighting * 0.5) // Use light specular vector as intensity multiplier  
	), texel.w);  
	*/
	vColorOut = float4(saturate(  
		//texel.xyz * float3(1.0f,1.0f,1.0f) * diffuseLighting
		float3(1.0f,1.0f,1.0f) * diffuseLighting 
		//texel.xyz * diffuseLighting
	), 1.0f);  
   
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
