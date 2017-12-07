#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_3
#define PS_SHADERMODEL ps_4_0_level_9_3
#endif

float4x4 World;
float4x4 View;
float4x4 Projection;
float4x4 WorldInverseTranspose;

float4 AmbientColor = float4(1, 1, 1, 1);
float AmbientIntensity = 0.1;
float3 DiffuseLightDirection = float3(0, 1, 0.2);
float4 DiffuseColor = float4(1, 1, 1, 1);
float DiffuseIntensity = 20.0;
float Shininess = 1.0;
float4 SpecularColor = float4(1, 1, 1, 1);
float SpecularIntensity = 0.5;
float3 CameraPosition = float3(1, 1, 0);

float Time = 0;	
float TextureLerp = 0.3;

texture ModelTexture1;
sampler2D textureSampler1 = sampler_state {
	Texture = (ModelTexture1);
	MagFilter = None;
	MinFilter = None;
	AddressU = Mirror;
	AddressV = Mirror;
};

texture ModelTexture2;
sampler2D textureSampler2 = sampler_state {
	Texture = (ModelTexture2);
	MagFilter = None;
	MinFilter = None;
	AddressU = Mirror;
	AddressV = Mirror;
};

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float4 Normal : NORMAL0;
	float2 TextureCoordinate : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float4 Color : COLOR0;
	float3 Normal : TEXCOORD0;
	float4 PositionWorld : TEXCOORD1;
	float2 TextureCoordinate : TEXCOORD2;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;

	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);

	float4 normal = mul(input.Normal, WorldInverseTranspose);
	float lightIntensity = dot(normal, DiffuseLightDirection);
	output.Color = saturate(DiffuseColor * DiffuseIntensity * lightIntensity);
	output.PositionWorld = worldPosition;
	output.Normal = normal;
	output.TextureCoordinate = input.TextureCoordinate * 5;

	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float3 light = normalize(DiffuseLightDirection);
    float3 normal = normalize(input.Normal);
    float3 r = normalize(2 * dot(light, normal) * normal - light);
	float3 v = normalize(CameraPosition - input.PositionWorld);

    float dotProduct = dot(r, v);
    float4 specular = SpecularIntensity * SpecularColor * max(pow(dotProduct, Shininess), 0);


	float4 texture1 = tex2D(textureSampler1, input.TextureCoordinate + float2(0, 0));
	float4 texture2 = tex2D(textureSampler2, input.TextureCoordinate + float2(Time, 0));
	float4 textureColor = (TextureLerp * texture1) + ((1 - TextureLerp) * texture2);
	textureColor.a = 0.3;
	//float4 textureColor = tex2D(textureSampler1, input.TextureCoordinate);
	float4 color = saturate(textureColor * 5 * input.Color + AmbientColor * AmbientIntensity + specular);
	//color.a = 1;
	return color;
}

technique Specular
{
	pass Pass1
	{
		AlphaBlendEnable = TRUE;
		DestBlend = INVSRCALPHA;
		SrcBlend = SRCALPHA;
		VertexShader = compile VS_SHADERMODEL VertexShaderFunction();
		PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
		//AlphaBlendEnable = FALSE;

	}
}