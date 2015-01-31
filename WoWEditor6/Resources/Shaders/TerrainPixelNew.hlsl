﻿struct PixelInput
{
	float4 position : SV_Position;
	float3 normal : NORMAL0;
	float2 texCoord : TEXCOORD0;
	float2 texCoordAlpha : TEXCOORD1;
	float4 color : COLOR0;
	float depth : TEXCOORD2;
	float3 worldPosition : TEXCOORD3;
};

SamplerState alphaSampler : register(s1);
SamplerState colorSampler : register(s0);

Texture2D alphaTexture : register(t0);
Texture2D texture0 : register(t1);
Texture2D texture1 : register(t2);
Texture2D texture2 : register(t3);
Texture2D texture3 : register(t4);

float3 sunDirection = float3(1, 1, -1);

cbuffer GlobalParamsBuffer : register(b0)
{
	float4 ambientLight;
	float4 diffuseLight;
	float4 fogColor;
	float4 fogParams;
	float4 mousePosition;
	float4 brushParams;
};

cbuffer TextureScaleParams : register(b2)
{
	float4 texScales;
};

float4 applyBrush(float4 color, float3 worldPos) {
	float3 dirVec = worldPos - mousePosition.xyz;
	float dsq = dot(dirVec.xy, dirVec.xy);
	float innerRadius = brushParams.x * brushParams.x;
	float outerRadius = brushParams.y * brushParams.y;
	float facInner = step(dsq, innerRadius) * step(innerRadius * 0.95, dsq);
	float facOuter = step(dsq, outerRadius) * step(outerRadius * 0.95, dsq);
	float angle = atan2(dirVec.y, dirVec.x) + 3.141592;
	angle = degrees(angle);
	angle = fmod(angle, 36.0f);

	float fac = step(0.1, (facInner + facOuter) / 2);
	fac *= step(angle, 18.0f);
	float4 brushColor = float4(1, 1, 1, 1);
	brushColor.rgb -= color.rgb;

	return fac * brushColor + (1 - fac) * color;
}

float3 getDiffuseLight(float3 normal) {
	float light = dot(normal, normalize(-float3(-1, 1, -1)));
	if (light < 0.0)
		light = 0.0;
	if (light > 0.5)
		light = 0.5 + (light - 0.5) * 0.65;

	float3 diffuse = diffuseLight.rgb * light;
	diffuse += ambientLight.rgb;
	diffuse = saturate(diffuse);
	return diffuse;
}

float4 main(PixelInput input) : SV_Target{
	float4 alpha = alphaTexture.Sample(alphaSampler, input.texCoordAlpha);
	float4 c0 = texture0.Sample(colorSampler, input.texCoord.yx * texScales.x);
	float4 c1 = texture1.Sample(colorSampler, input.texCoord.yx * texScales.y);
	float4 c2 = texture2.Sample(colorSampler, input.texCoord.yx * texScales.z);
	float4 c3 = texture3.Sample(colorSampler, input.texCoord.yx * texScales.a);

	float4 color = (1.0 - (alpha.g + alpha.b + alpha.a)) * c0;
	color += alpha.g * c1;
	color += alpha.b * c2;
	color += alpha.a * c3;

	color.rgb *= input.color.bgr * 2;
	color.rgb *= getDiffuseLight(input.normal);

	float fogDepth = input.depth - fogParams.x;
	fogDepth /= (fogParams.y - fogParams.x);
	float fog = 1.0f - pow(saturate(fogDepth), 1.5);

	color.rgb = (1.0 - fog) * fogColor.rgb + fog * color.rgb;

	color = applyBrush(color, input.worldPosition);

	return color;
}