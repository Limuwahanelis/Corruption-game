Shader "Shaders/Corruption"
{
    Properties
    {
       // _MainTex ("_MainTex", 2D) = "white" {}
        _Radius("Radius",float)=1
        _MainTex("Diffuse", 2D) = "white" {}
        _MaskTex("Mask", 2D) = "white" {}
        _NormalMap("Normal Map", 2D) = "bump" {}
        _Scale("Scale",float) = 1
        _ScaleMod("ScaleMod",float) = 1
        _GradientMod("GradientMod",float)=1
        _RotateSpeed("Speed",float)=1
        [IntRange] _StencilID("Stencil ID",Range(0,255))=0
        [Enum(CompareFunction)] _StencilComp ("Compare op", Int) = 0
        [Enum(StencilOp)] _SucessPass ("Success op", Int) = 0
        [Enum(StencilOp)] _FailPass ("Fail op", Int) = 0

    }

    SubShader
    {
        Tags {"Queue" = "Transparent" "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" }

        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        Cull Off
        ZWrite Off


        Pass
        {
              Stencil
             {
                 ref [_StencilID]
                 Comp [_StencilComp]
                 Pass [_SucessPass]
                 Fail [_FailPass]
             }
          //  Tags { "LightMode" = "Universal2D" }

                        



            HLSLPROGRAM
                       // This line defines the name of the vertex shader.
            #pragma vertex vert
            // This line defines the name of the fragment shader.
            #pragma fragment frag

            // The Core.hlsl file contains definitions of frequently used HLSL
            // macros and functions, and also contains #include references to other
            // HLSL files (for example, Common.hlsl, SpaceTransforms.hlsl, etc.).
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/InputData2D.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/SurfaceData2D.hlsl"
            // The structure definition defines which variables it contains.
            // This example uses the Attributes structure as an input structure in
            // the vertex shader.
            struct Attributes
            {
                // The positionOS variable contains the vertex positions in object
                // space.
                float4 positionOS   : POSITION;
                float2 uv           : TEXCOORD0;
            };

            struct Varyings
            {
                // The positions in this struct must have the SV_POSITION semantic.
                float4 positionOS   : TEXCOORD3;
                float4 positionHCS  : SV_POSITION;
                float2  uv          : TEXCOORD0;
                float4 positionWS   : TEXCOORD2;
            };
            
            float2 unity_gradientNoise_dir(float2 p)
            {
                p = p % 289;
                float x = (34 * p.x + 1) * p.x % 289 + p.y;
                x = (34 * x + 1) * x % 289;
                x = frac(x / 41) * 2 - 1;
                return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
            }
            
            float unity_gradientNoise(float2 p)
            {
                float2 ip = floor(p);
                float2 fp = frac(p);
                float d00 = dot(unity_gradientNoise_dir(ip), fp);
                float d01 = dot(unity_gradientNoise_dir(ip + float2(0, 1)), fp - float2(0, 1));
                float d10 = dot(unity_gradientNoise_dir(ip + float2(1, 0)), fp - float2(1, 0));
                float d11 = dot(unity_gradientNoise_dir(ip + float2(1, 1)), fp - float2(1, 1));
                fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
                return lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x);
            }
            
            void Unity_GradientNoise_float(float2 UV, float Scale, out float Out)
            {
                Out = unity_gradientNoise(UV * Scale) + 0.5;
            }
            void Unity_Rotate_Degrees_float(float2 UV, float2 Center, float Rotation, out float2 Out)
            {
                Rotation = Rotation * (3.1415926f/180.0f);
                UV -= Center;
                float s = sin(Rotation);
                float c = cos(Rotation);
                float2x2 rMatrix = float2x2(c, -s, s, c);
                rMatrix *= 0.5;
                rMatrix += 0.5;
                rMatrix = rMatrix * 2 - 1;
                UV.xy = mul(UV.xy, rMatrix);
                UV += Center;
                Out = UV;
            }
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Radius;
            float _Scale;
            float _RotateSpeed;
            float _ScaleMod;
            float _GradientMod;
            // The vertex shader definition with properties defined in the Varyings
            // structure. The type of the vert function must match the type (struct)
            // that it returns.
            Varyings vert(Attributes IN)
            {

                // Declaring the output object (OUT) with the Varyings struct.
                Varyings OUT;
                // The TransformObjectToHClip function transforms vertex positions
                // from object space to homogenous clip space.
                float4 worldPos= mul (unity_ObjectToWorld, IN.positionOS);
                float2 rot;
                Unity_Rotate_Degrees_float(IN.uv,float2(0.5,0.5),_Time.y*_RotateSpeed,rot);
                float value_Out_2_Float_x;
                Unity_GradientNoise_float(IN.uv,cos( _Scale+_Time.y/_ScaleMod),value_Out_2_Float_x);
                float value_Out_2_Float_y;
                Unity_GradientNoise_float(IN.uv,sin( _Scale+_Time.y/_ScaleMod),value_Out_2_Float_y);
                float2 newPos = IN.positionOS;
                float xPosMult=1;
                float yPosMult=1;
                value_Out_2_Float_x = value_Out_2_Float_x/_GradientMod;
                value_Out_2_Float_y = value_Out_2_Float_y/_GradientMod;
                float newPosX =  IN.positionOS.x;
                float newPosy =  IN.positionOS.y;
                float cosValue =cos(_Time.y)/100;
                float sinValue =sin(_Time.y)/100;
                 if(cos(_Time.y)>=0)
                 {
                    if(IN.positionOS.x>=0)newPosX = cosValue+ IN.positionOS.x+value_Out_2_Float_x;
                     else newPosX = -cosValue+ IN.positionOS.x-value_Out_2_Float_x;

                 }
                 else
                 {
                     if(IN.positionOS.x>=0)newPosX = -cosValue+ IN.positionOS.x+value_Out_2_Float_x;
                     else newPosX = cosValue+ IN.positionOS.x-value_Out_2_Float_x;
                }
                 if(sin(_Time.y)>=0)
                {
                    if(IN.positionOS.y>=0)newPosy = sinValue+ IN.positionOS.y+value_Out_2_Float_y;
                    else newPosy = -sinValue+ IN.positionOS.y-value_Out_2_Float_y;
                }
                else
                {
                    if(IN.positionOS.y>=0)newPosy = -sinValue+ IN.positionOS.y+value_Out_2_Float_y;
                    else newPosy = sinValue+ IN.positionOS.y-value_Out_2_Float_y;
                }
                newPos =float2(newPosX,newPosy);
                float3 pos =  float3(newPos,IN.positionOS.z);
                OUT.positionHCS =TransformObjectToHClip(pos);
                float4 basePos = OUT.positionHCS;
                float4 tmp = OUT.positionHCS;

                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                OUT.positionWS=worldPos;
                OUT.positionOS=IN.positionOS;
                // Returning the output.
                return OUT;
            }

            // The fragment shader definition.
            half4 frag(Varyings i) : SV_Target
            {
                half4 customColor = tex2D(_MainTex,i.uv);
                return customColor;
            }

            ENDHLSL
        }

    }

}
