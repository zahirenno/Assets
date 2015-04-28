Shader "Custom/pshh" {
Properties {
 _MainTex ("", 2D) = "white" {}
}
 
SubShader {
 
ZTest Always Cull Off ZWrite Off Fog { Mode Off } //Rendering settings
 
 Pass{
  CGPROGRAM
  #pragma vertex vert
  #pragma fragment frag
  #include "UnityCG.cginc" 
  //we include "UnityCG.cginc" to use the appdata_img struct
    
  struct v2f {
   float4 pos : POSITION;
   half2 uv : TEXCOORD0;
  };
   
  //Our Vertex Shader 
  v2f vert (appdata_img v){
   v2f o;
   o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
   o.uv = MultiplyUV (UNITY_MATRIX_TEXTURE0, v.texcoord.xy);
   return o; 
  }
    
  sampler2D _MainTex; //Reference in Pass is necessary to let us use this variable in shaders
    
  //Our Fragment Shader
  fixed4 frag (v2f i) : COLOR{
   fixed4 orgCol = tex2D(_MainTex, i.uv); //Get the orginal rendered color 
     
   float st = 1.0f / 800.0f;
   
   fixed4 coll = -1.0f * tex2D(_MainTex, i.uv - (st, st))
   				-2.0f * tex2D(_MainTex, i.uv - (0.0f, st))
   				-1.0f * tex2D(_MainTex, i.uv - (st, -st))
   				+1.0f * tex2D(_MainTex, i.uv - (-st, -st))
   				+2.0f * tex2D(_MainTex, i.uv - (0.0f, -st))
   				+1.0f * tex2D(_MainTex, i.uv - (st, -st));
    coll = 1.0f - coll;
    coll.a = 1.0f; 
    
    
    
   //Make changes on the color
   
 
   
   fixed4 col = orgCol * coll;
     
   return col;
  }
  ENDCG
 }
} 
 FallBack "Diffuse"
}