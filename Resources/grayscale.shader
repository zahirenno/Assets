Shader "Custom/GrayScale" {
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
     
   //Make changes on the color
   float avg = (orgCol.r + orgCol.g + orgCol.b)/3.0f;
   fixed4 col = fixed4(avg, avg, avg, 1);
     
   return col;
  }
  ENDCG
 }
} 
 FallBack "Diffuse"
}