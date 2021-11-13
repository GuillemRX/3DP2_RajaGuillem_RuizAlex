Shader "Custom/DepthMask"
{
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        
        ColorMask 0
        ZWrite On
        
        Pass {}
    }
}
