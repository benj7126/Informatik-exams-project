Shader "xBarShader"
{
    Properties
    {
        _p ("PercentLeft", Range(0.0, 1.0)) = 0.5
        _shadeSTR ("Black n White str", Range(0.0, 1.0)) = 0.1
        _fColor ("Foreground color", Color) = (1, 1, 1, 1)
        _bColor ("Background color", Color) = (1, 1, 1, 1)
        _tColor ("Transform color", Color) = (1, 1, 1, 1)
        [MaterialToggle]_f ("Flip", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Transparrent" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float _p;
            float _shadeSTR;
            float4 _fColor;
            float4 _bColor;
            float4 _tColor;

            float _f;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = float2(v.uv.x*2-1, v.uv.y);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                i.uv.x -=i.uv.x*_f*2;
                
                float _ip = abs(_p-1);
                if (i.uv.x > 0.5)
                    i.uv.x = abs(i.uv.x-1);
                if (abs(i.uv.y*2-1) < abs(_p-1))
                    return float4(_fColor.r*_ip+i.uv.x*_shadeSTR + _tColor.r*_p, _fColor.g*_ip+i.uv.x*_shadeSTR + _tColor.g*_p, _fColor.b*_ip+i.uv.x*_shadeSTR + _tColor.b*_p, 1);
                
                return float4(_bColor.r+i.uv.x*_shadeSTR, _bColor.g+i.uv.x*_shadeSTR, _bColor.b+i.uv.x*_shadeSTR, 0);
            }
            ENDCG
        }
    }
}
