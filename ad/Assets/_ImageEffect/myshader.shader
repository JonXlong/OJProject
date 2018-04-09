Shader "Custom/myshader" {
	Properties{
	_MainColor("主颜色",color)=(1,.5,.1,1)
	_SpecilColor("漫反射颜色",color)=(1,1,1,1)
	_EmissColor("自发光颜色",color)=(1,1,1,1)
	_ShiRang("反射强度",Range(0.1,1))=0.7
	    //纹理  
              _MainTex("基本纹理",2D)="White"{TexGen SphereMap}  
	}
	SubShader {
	pass{
		//SetColor(_MainColor,color)
		Material{
		Diffuse[_MainColor]
		Ambient[_MainColor]
		  Shininess [_ShiRang]  
                //高光颜色  
                Specular [_SpecilColor]  
                //自发光颜色  
                Emission [_EmissColor]   
		//Ambient(0.9,0.5,0.4,1)
		//SetColor[_MainColor]
		}
			  Lighting On  
            //开启独立镜面反射  
            SeparateSpecular On  
            //设置纹理并进行纹理混合  
            SetTexture [_MainTex]   
            {  
                Combine texture * primary DOUBLE, texture * primary  
            }  
	//	Lighting on
	}
	}
	Fallback"Diffuse"
}
