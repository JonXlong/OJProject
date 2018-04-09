// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.30 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.30;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:True,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True;n:type:ShaderForge.SFN_Final,id:4795,x:32724,y:32693,varname:node_4795,prsc:2|emission-9634-OUT;n:type:ShaderForge.SFN_Tex2d,id:4299,x:31914,y:32690,ptovrint:False,ptlb:noduuu,ptin:_noduuu,varname:node_4299,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:28c7aad1372ff114b90d330f8a2dd938,ntxv:0,isnm:False|UVIN-8951-UVOUT;n:type:ShaderForge.SFN_Panner,id:8951,x:31711,y:32744,varname:node_8951,prsc:2,spu:0,spv:0.3|UVIN-3263-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:3263,x:31676,y:32917,varname:node_3263,prsc:2,uv:0;n:type:ShaderForge.SFN_Multiply,id:9634,x:32185,y:32578,varname:node_9634,prsc:2|A-9934-RGB,B-4299-RGB,C-3661-RGB;n:type:ShaderForge.SFN_Color,id:9934,x:31887,y:32293,ptovrint:False,ptlb:node_9934,ptin:_node_9934,varname:node_9934,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Tex2d,id:3661,x:31971,y:33021,ptovrint:False,ptlb:node_3661,ptin:_node_3661,varname:node_3661,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:e6e7abfd01541984db62a8d677d6b2d7,ntxv:2,isnm:False|UVIN-8611-UVOUT;n:type:ShaderForge.SFN_Panner,id:8611,x:31760,y:33109,varname:node_8611,prsc:2,spu:0,spv:0.5|UVIN-8608-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:8608,x:31540,y:33069,varname:node_8608,prsc:2,uv:0;proporder:4299-9934-3661;pass:END;sub:END;*/

Shader "Shader Forge/uv_Shader" {
    Properties {
        _noduuu ("noduuu", 2D) = "white" {}
        _node_9934 ("node_9934", Color) = (1,1,1,1)
        _node_3661 ("node_3661", 2D) = "black" {}
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _noduuu; uniform float4 _noduuu_ST;
            uniform float4 _node_9934;
            uniform sampler2D _node_3661; uniform float4 _node_3661_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                UNITY_FOG_COORDS(1)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float4 node_5622 = _Time + _TimeEditor;
                float2 node_8951 = (i.uv0+node_5622.g*float2(0,0.3));
                float4 _noduuu_var = tex2D(_noduuu,TRANSFORM_TEX(node_8951, _noduuu));
                float2 node_8611 = (i.uv0+node_5622.g*float2(0,0.5));
                float4 _node_3661_var = tex2D(_node_3661,TRANSFORM_TEX(node_8611, _node_3661));
                float3 emissive = (_node_9934.rgb*_noduuu_var.rgb*_node_3661_var.rgb);
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG_COLOR(i.fogCoord, finalRGBA, fixed4(0.5,0.5,0.5,1));
                return finalRGBA;
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
