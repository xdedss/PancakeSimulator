
float4 pancake_Warp(float4 posWorld) {
	float extraY = (0.8 - posWorld.y);
	if (extraY > 0) posWorld.y -= min(extraY * extraY, 0.8);
	return posWorld;
}

float4 pancake_WarpVertex(float4 vertex) {
	return vertex;
	
	vertex = mul(unity_ObjectToWorld, vertex);
	vertex = pancake_Warp(vertex);
	return mul(unity_WorldToObject, vertex);
}