//UNITY_SHADER_NO_UPGRADE


//Based off workshop 4 code
Shader "Unlit/WaveShader"
{
    Properties
    {   
        _PointLightColor("Point Light Color", Color) = (0, 0, 0)
        _PointLightPosition("Point Light Position", Vector) = (0.0, 0.0, 0.0)
        _MainTex ("Texture", 2D) = "white" {}
        _Amplitude ("Amplitude", Range(-10,10)) = 0.3
        _Wavelength("Wavelength", Range(-10,10)) = 0.5
        _Speed ("Speed", Range(0,10)) = 0.1
    }

    SubShader
    {
        Pass
        {
            //Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            uniform float3 _PointLightColor;
            uniform float3 _PointLightPosition;
            uniform sampler2D _MainTex; 

            struct vertIn
            {
                float4 vertex : POSITION;
                float4 uv : TEXCOORD0;
                float4 normal : NORMAL;
                float4 color : COLOR;
            };

            struct vertOut
            {
                float4 vertex : SV_POSITION;
                float4 uv : TEXCOORD0;
                float4 color : COLOR;
                float3 worldNormal : TEXCOORD1;
            };

            float _Amplitude;
            float _Wavelength;
            float _Speed;

            
            // Implementation of the vertex shader
            vertOut vert(vertIn v)
            {
                
                float numWaves = 2 * UNITY_PI/_Wavelength;
                float waveY = sin((v.vertex.xz + _Time.y * _Speed)*numWaves);
                v.vertex.y += waveY * (_Amplitude/numWaves);

                float waveZ = cos((v.vertex.zx + _Time.y * _Speed)*numWaves);
                v.vertex.y += waveZ * (_Amplitude/numWaves);

                // wave behaviour
                //numWaves = 2 * UNITY_PI/_Wavelength;
                //sin((vertex + _Time.y * _Speed)*numWaves)* (_Amplitude/num_waves)
                //v.vertex += displacement;
                

                vertOut o;

                float4 worldVertex = mul(unity_ObjectToWorld, v.vertex);
                float3 worldNormal = normalize(mul(transpose((float3x3)unity_WorldToObject), v.normal.xyz));

                o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
                o.color = v.color;

                o.uv = worldVertex;
                o.worldNormal = worldNormal;

                
                return o;

            }
            
            

            
            // Implementation of the fragment shader
            fixed4 frag(vertOut v) : SV_Target
            {
                float3 interpNormal = normalize(v.worldNormal);

                // Calculate ambient RGB intensities
                float Ka = 0.5; //ambience
                float Kd = 0.5; //diffuse
                float Ks = 0.2; //specular
                float fAtt = 0.7; //colour
                float specN = 200; // Values>>1 give tighter highlights


                float3 amb = v.color.rgb * UNITY_LIGHTMODEL_AMBIENT.rgb * Ka;
                
                

                // Calculate diffuse RBG reflections, we save the results of L.N because we will use it again
                // (when calculating the reflected ray in our specular component)

                float3 L = normalize(_PointLightPosition - v.uv.xyz);
                float LdotN = dot(L, interpNormal);
                float3 dif = fAtt * _PointLightColor.rgb * Kd * v.color.rgb * saturate(LdotN);

                // Calculate specular reflections
                float3 V = normalize(_WorldSpaceCameraPos - v.uv.xyz);
                // Using classic reflection calculation:
                //float3 R = normalize((2.0 * LdotN * interpNormal) - L);
                //float3 spe = fAtt * _PointLightColor.rgb * Ks * pow(saturate(dot(V, R)), specN);
                // Using Blinn-Phong approximation:
                // We usually need a higher specular power when using Blinn-Phong
                float3 H = normalize(V + L);
                float3 spe = fAtt * _PointLightColor.rgb * Ks * pow(saturate(dot(interpNormal, H)), specN);

                fixed4 col = tex2D(_MainTex, v.uv);

                col.rgb *= (amb.rgb + dif.rgb + spe.rgb) *10;
                //col.rgb = (255.0, 255.0, 0.0);
                
                col.a = v.color.a;
                
                return col;
            }
            
            ENDCG
        }
    }
}