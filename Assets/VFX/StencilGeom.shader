Shader "CustomRenderTexture/StencilGeom"
{
    Properties
    {
        _StencilID ("Stencil ID", int) = 1
    }

     SubShader
     {
        Tags
        {
            "RenderType" = "Opaque"
            "RenderPipeline" = "UniversalPipeline"
            "Queue" = "Geometry"
        }
        Pass
        {
            Blend Zero One
            ZWrite Off
            
            Stencil
            {
                Ref [_StencilID]
                Comp Always
                Pass Replace
                Fail Keep
            }
        }
    }
}
