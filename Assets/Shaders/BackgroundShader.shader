Shader "Custom/BackgroundShader"
{
    Properties
    {
//        _Color ("Color", Color) = (1,1,1,1)
//        _MainTex ("Albedo (RGB)", 2D) = "white" {}
//        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Speed ("Speed", float) = 0.01
        _Scale ("Scale", float) = 100
        _SubScale ("SubScale", float) = 1
        _CutoffMargin ("CutoffMargin", Range(0,1)) = 0.1
        _Multiple ("Multiple", float) = 2
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        // #pragma surface surf Standard fullforwardshadows
        #pragma surface surf Lambert vertex:vert
        
        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        // sampler2D _MainTex;
        // half _Glossiness;
        // fixed4 _Color;
        float _Speed;
        float _Scale;
        float _SubScale;
        half _CutoffMargin;
        float _Multiple;
        
        struct Input
        {
            float2 uv_MainTex;
            float3 localPos;
            float3 localCamPos;
        };



        void vert (inout appdata_full v, out Input o) {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            o.localPos = v.vertex.xyz;
            o.localCamPos = mul(unity_ObjectToWorld, float4(_WorldSpaceCameraPos, 1));
        }

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)
        
        float hash12(float2 p) {
            float3 p3  = frac(float3(p.xyx) * .1031);
            p3 += dot(p3, p3.yzx + 19.19);
            return frac((p3.x + p3.y) * p3);
        }

        float noise(float x, float y)
        {
            return hash12(float2(x, y));
        }
        float noise(float2 p)
        {
            return hash12(p);
        }


        float easeInOut(float t)
        {
            return (t*t) / (2 * (t*t - t) + 1);
        }
        
        void surf (Input IN, inout SurfaceOutput o)
        {
            // Albedo comes from a texture tinted by color
            //fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            fixed4 c;
            float color;
            fixed3 localCamPosInvVert;
            localCamPosInvVert.xy = IN.localCamPos.xy;
            localCamPosInvVert.z = - IN.localCamPos.z;
            fixed3 pos = _Speed * localCamPosInvVert + IN.localPos;

            
            float2 posRound = floor(pos.xz * _Scale) / _SubScale;
            // color = noise(posRound);
            //blur
            float sum = 0;
            int size = 2;
            int n = 0;
            for(int x = -size ; x <= size ; x++)
                for(int y = -size ; y <= size ; y++)
                {
                    //int i = (x+2)+(y+2)*5;
                    float realX = posRound.x + x;
                    float realY = posRound.y + y;
                    sum += noise(realX, realY);
                    n++;
                }
            int sizeInAxis = size * 2 + 1;
            color = sum / (sizeInAxis * sizeInAxis);

            //dots
            float colorX = pow(3 * color - 2, 0.1);
            color = 1 - easeInOut(colorX);
            if(color < _CutoffMargin)
                color = 0;
            color *= _Multiple;
            //color = pow(color, 3);
            
            
            
            c.rgb = color;
            o.Albedo = c.rgb;
            o.Alpha = 1;
            
        }
        
        ENDCG
    }
    FallBack "Diffuse"
}
