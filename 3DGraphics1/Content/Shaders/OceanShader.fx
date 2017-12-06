float4x4 World;
float4x4 View;
float4x4 Projection;

float4 AmbientColor = float4(0, 0, 1, 1);
float AmbientIntensity = 1;

float3 EyePos;

float time = 0;

float FogEnabled;
float FogStart;
float FogEnd;
float4 FogColor;

textureCUBE cubeTex;
samplerCUBE CubeTextureSampler = sampler_state
{
    Texture = <cubeTex>;
    MinFilter = anisotropic;
    MagFilter = anisotropic;
    MipFilter = anisotropic;
    AddressU = wrap;
    AddressV = wrap;
};

float textureLerp;

texture2D normalTex;
sampler2D NormalTextureSampler = sampler_state
{
    Texture = <normalTex>;
    MinFilter = anisotropic;
    MagFilter = anisotropic;
    MipFilter = anisotropic;
    AddressU = wrap;
    AddressV = wrap;
};

texture2D normalTex2;
sampler2D NormalTextureSampler2 = sampler_state
{
    Texture = <normalTex2>;
    MinFilter = anisotropic;
    MagFilter = anisotropic;
    MipFilter = anisotropic;
    AddressU = wrap;
    AddressV = wrap;
};

struct VertexShaderInput
{
    float3 Position : POSITION0;
    float3 normal : NORMAL0;
    float2 texCoord : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
    float2 texCoord : TEXCOORD0;
    float3 worldPos : TEXCOORD1;
};

float ComputeFogFactor(float d)
{
    return clamp((d - FogStart) / (FogEnd - FogStart), 0, 1) * FogEnabled;
}

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(float4(input.Position, 1), World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);

    output.texCoord = input.texCoord * 100;
    output.worldPos = worldPosition.xyz;
    output.FogFactor = ComputeFogFactor(length(CameraPosition - worldPosition));
    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    float3 diffuseColor = float4(0,0,1,1);

    float4 normalTexture1 = tex2D(NormalTextureSampler, input.texCoord + float2(0, 0));
    float4 normalTexture2 = tex2D(NormalTextureSampler2, input.texCoord + float2(time, 0));
    float4 normalTexture = (textureLerp * normalTexture1) + ((1 - textureLerp) * normalTexture2);

    float3 normal = ((normalTexture) * 2) - 1;
    normal.xyz = normal.xzy;
    normal = normalize(normal);

    float3 cubeTexCoords = reflect(input.worldPos - EyePos,normal);

    float3 cubeTex = texCUBE(CubeTextureSampler,cubeTexCoords).rgb;

    return float4((normalTexture*1.0) + (diffuseColor*0.0),1);
}

technique Ocean
{
    pass Pass1
    {
        VertexShader = compile vs_4_0 VertexShaderFunction();
        PixelShader = compile ps_4_0 PixelShaderFunction();
    }
}