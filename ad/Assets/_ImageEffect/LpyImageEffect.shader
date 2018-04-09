// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/LpyImageEffect" {
	Properties 
    {
        _MainTex ("Main Tex", 2D) = "white" {}
        _MaskColor ("Mask Color", Color) = (1, 1, 1, 1)
         
        _Speed ("Speed", float) = 2
        _MaskLimit ("MaskLimit", float) = 0.8
    }
     
    SubShader 
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
         
        Pass 
        {
            Tags { "LightMode"="ForwardBase" }
            ZTest off
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
             
            CGPROGRAM
            #pragma vertex vert  
            #pragma fragment frag
            #include "UnityCG.cginc"
            #define PI 3.14159265358979  
             
             
            sampler2D _MainTex;
            fixed3 _MaskColor;
            float _Speed;
            float _MaskLimit;
             
            struct a2v 
            {  
                float4 vertex : POSITION; 
                float3 texcoord : TEXCOORD0;
            };  
             
            struct v2f 
            {  
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };  
             
            v2f vert (a2v v) 
            {  
                v2f o;  
                o.pos = UnityObjectToClipPos(v.vertex);  
                o.uv = v.texcoord;
                return o;
            }  
             
            fixed4 frag (v2f i) : SV_Target 
            {
                fixed4 c = tex2D(_MainTex, i.uv);
                 
                float isMask = sin(_Time.y*_Speed  -i.uv.x*2*PI );
                isMask = step(_MaskLimit,isMask);
                 
                c.rgb += _MaskColor*isMask;
                return c;
            }
            ENDCG
        }  
    }
    FallBack "Transparent/VertexLit"
}
