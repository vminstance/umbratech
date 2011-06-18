// Effect file for
//  UMBRA ENGINE

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

VertexToPixelBlock TexturedVS( float4 inPos : POSITION0, float4 inColor : COLOR0)
{
    VertexToPixelBlock Output = (VertexToPixelBlock)0;
    float4x4 preViewProjection = mul(xView, xProjection);
    float4x4 preWorldViewProjection = mul(xWorld, preViewProjection);
    float4 realPosition = float4(inPos.x, inPos.y, inPos.z, 1);
    Output.Position			= mul(realPosition, preWorldViewProjection);
    Output.TextureCoords	= float2(inPos.w / 16, inColor.w / 16);
    Output.Color			= float4((float)inColor.x / 255, (float)inColor.y / 255, (float)inColor.z / 255, 1);
    Output.OriginalPos		= inPos;
    Output.ScreenPos		= float2(Output.Position.x, Output.Position.y);
    bool IsTransparent = false;
    for(int i = 0; i < 4; i++)
	{
        if(	inPos.w == (int)(xTranslucentBlocks[i] % 16)		&& inColor.w == (int)(xTranslucentBlocks[i] / 16)		||
			inPos.w == (int)(xTranslucentBlocks[i] % 16)		&& inColor.w == (int)(xTranslucentBlocks[i] / 16) + 1	||
			inPos.w == (int)(xTranslucentBlocks[i] % 16)	+ 1 && inColor.w == (int)(xTranslucentBlocks[i] / 16)		||
			inPos.w == (int)(xTranslucentBlocks[i] % 16)	+ 1 && inColor.w == (int)(xTranslucentBlocks[i] / 16) + 1
			)
		{
            IsTransparent = true;
        }


	}

	if(IsTransparent)
	{
        Output.Position = float4(-1,-1,-1,-1);
    }



    return Output;
}






PixelToFrame TexturedPS(VertexToPixelBlock PSIn) 
{
    PixelToFrame Output = (PixelToFrame)0;
    float depth = length(PSIn.OriginalPos - xCameraPos);
    Output.Color = tex2D(TextureSampler, PSIn.TextureCoords);
    Output.Color.r *= saturate(PSIn.Color.r);
    Output.Color.g *= saturate(PSIn.Color.g);
    Output.Color.b *= saturate(PSIn.Color.b);
    Output.Color.rgb *= xFaceLightCoef;
    float2 positionScreen = PSIn.ScreenPos.xy * 0.5f + 0.5f;
    positionScreen.x = positionScreen.x * xViewPort.x;
    positionScreen.y = (1.0f - positionScreen.y) * xViewPort.y;
    float distanceFromCenter = sqrt((pow((positionScreen.x - xViewPort.x / 2), 2) + pow((positionScreen.y - xViewPort.y / 2), 2)) / pow(depth,1.5)) / 500;
    if(xFlashEnabled)
	{
        if(distanceFromCenter < 0.8)
		{
            float depthCoef = pow(0.95, depth);
            // falloff
			Output.Color.rgb = max(Output.Color.rgb * 1.5 * depthCoef / xFaceLightCoef, Output.Color.rgb);
        }

		else if(distanceFromCenter < 1)
		{
            float depthCoef = pow(0.95, depth);
            // falloff
			float fade = (distanceFromCenter - 0.8) * 5;
            Output.Color.rgb = lerp(max(Output.Color.rgb * 1.5 * depthCoef / xFaceLightCoef, Output.Color.rgb), Output.Color.rgb, fade);
        }

	}
	if(xIsUnderWater)
	{
		float timeCoef = float(xTime) / 10000;
		Output.Color.rg /= 2.1 + (sin(PSIn.OriginalPos.x * 1.5) * cos(PSIn.OriginalPos.z * 1.5)) / 4 * sin(PSIn.OriginalPos.x * timeCoef) * cos(PSIn.OriginalPos.y * timeCoef);

		float amount = saturate((depth)/(min(xFogEnd / 2, 10)));
		Output.Color = lerp(Output.Color, xFogColor, amount);
	}
	else if(xFogEnabled)
	{
			float amount = saturate((depth-xFogStart)/(xFogEnd-xFogStart));
			Output.Color = lerp(Output.Color, xFogColor, amount);
    }



    return Output;
}






VertexToPixelBlock TexturedVSAlpha( float4 inPos : POSITION0, float4 inColor : COLOR0)
{
    VertexToPixelBlock Output = (VertexToPixelBlock)0;
    float4x4 preViewProjection = mul (xView, xProjection);
    float4x4 preWorldViewProjection = mul (xWorld, preViewProjection);
    float4 realPosition = float4(inPos.x, inPos.y, inPos.z, 1);
    Output.Position			= mul(realPosition, preWorldViewProjection);
    Output.TextureCoords	= float2(inPos.w / 16, inColor.w / 16);
    Output.Color			= float4((float)inColor.x / 255, (float)inColor.y / 255, (float)inColor.z / 255, 1);
    Output.OriginalPos		= inPos;
    bool IsTransparent = false;
    for(int i = 0; i < 4; i++)
	{
        if(	inPos.w == (int)(xTranslucentBlocks[i] % 16)		&& inColor.w == (int)(xTranslucentBlocks[i] / 16)		||
			inPos.w == (int)(xTranslucentBlocks[i] % 16)		&& inColor.w == (int)(xTranslucentBlocks[i] / 16) + 1	||
			inPos.w == (int)(xTranslucentBlocks[i] % 16)	+ 1 && inColor.w == (int)(xTranslucentBlocks[i] / 16)		||
			inPos.w == (int)(xTranslucentBlocks[i] % 16)	+ 1 && inColor.w == (int)(xTranslucentBlocks[i] / 16) + 1
			)
		{
            IsTransparent = true;
        }


	}

	if(IsTransparent)
	{
        Output.Color.a *= 0.5;
    }


	else
	{
        Output.Position = float4(-1,-1,-1,-1);
    }


	return Output;
}


technique Voxel
{
    pass Pass0
	{
        VertexShader = compile vs_3_0 TexturedVS();
        PixelShader  = compile ps_3_0 TexturedPS();
    }


	pass Pass1
	{
        VertexShader = compile vs_3_0 TexturedVSAlpha();
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