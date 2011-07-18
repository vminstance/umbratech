// Effect file for
//  UMBRA ENGINE

#include <helper.fx>

struct VertexToPixelBlock
{
    float4 Position   	: POSITION0;
    float4 Color        : COLOR0;
    float2 TextureCoords: TEXCOORD0;
    float4 OriginalPos  : TEXCOORD1;
    float2 ScreenPos	: TEXCOORD2;
}

;
struct PixelToFrame
{
    float4 Color	: COLOR0;
}

;
//------- Constants --------
float4x4 xView;
float4x4 xProjection;
float4x4 xWorld;
float4 xTranslucentBlocks;
float2 xViewPort;
bool xIsUnderWater;
int xTime;
//------- Texture Samplers --------

texture xTexture;
sampler TextureSampler = sampler_state 
{
    texture = <xTexture>;
    magfilter = POINT;
    minfilter = POINT;
    mipfilter = POINT;
    AddressU = mirror;
    AddressV = mirror;
}

;
//------- Fog --------

float4 xFogColor	: FOGCOLOR;
float xFogStart		: FOGNEAR;
float xFogEnd		: FOGFAR;
bool xFogEnabled	: FOGENABLED;
float3 xCameraPos	: CAMERAPOS;
float xFaceLightCoef: FACELIGHTCOEF;
bool xFlashEnabled	: FLASHENABLED;
//------- Technique: Voxel --------



VertexToPixelBlock TexturedVS( float inData : POSITION )
{
    VertexToPixelBlock Output = (VertexToPixelBlock)0;
    float4x4 preViewProjection = mul(xView, xProjection);
    float4x4 preWorldViewProjection = mul(xWorld, preViewProjection);

    float index = (inData % 65536);
	float4 cornerPosition = GetCornerPosition(floor(inData / 65536));
    float4 realPosition = float4(index % 32, floor(index / 32) % 32, floor(index / 1024) % 32, 0);

	realPosition += cornerPosition;

    Output.Position			= mul(realPosition, preWorldViewProjection);
    Output.TextureCoords	= float2(0, 0);
    Output.Color			= float4(0.5, 0.5, 0.5, 1);
    Output.ScreenPos		= float2(Output.Position.x, Output.Position.y);
    return Output;
}


PixelToFrame TexturedPS(VertexToPixelBlock PSIn) 
{
    PixelToFrame Output = (PixelToFrame)0;
    Output.Color = tex2D(TextureSampler, PSIn.TextureCoords);
    Output.Color.r *= saturate(PSIn.Color.r);
    Output.Color.g *= saturate(PSIn.Color.g);
    Output.Color.b *= saturate(PSIn.Color.b);
    return Output;
}


technique Voxel
{
    pass Pass0
	{
        VertexShader = compile vs_3_0 TexturedVS();
        PixelShader  = compile ps_3_0 TexturedPS();
    }
}





struct VertexToPixelCursor
{
    float4 Position   	: POSITION0;
    float4 Color        : COLOR0;
}

;
//------- Technique: Cursor --------

VertexToPixelCursor CursorVS( float4 inPos : POSITION0, float4 inColor : COLOR0)
{
    VertexToPixelCursor Output = (VertexToPixelCursor)0;
    float4x4 preViewProjection = mul (xView, xProjection);
    float4x4 preWorldViewProjection = mul (xWorld, preViewProjection);
    float4 realPosition = float4(inPos.x, inPos.y, inPos.z, 1);
    Output.Position			= mul(realPosition, preWorldViewProjection);
    Output.Color			= float4((float)inColor.x / 255, (float)inColor.y / 255, (float)inColor.z / 255, 1);
    return Output;
}




PixelToFrame CursorPS(VertexToPixelCursor PSIn) 
{
    PixelToFrame Output = (PixelToFrame)0;
    Output.Color = PSIn.Color;
    return Output;
}






technique Cursor
{
    pass Pass0
	{
        VertexShader = compile vs_3_0 CursorVS();
        PixelShader  = compile ps_3_0 CursorPS();
    }


}


// Helper functions
