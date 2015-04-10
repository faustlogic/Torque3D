
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX
//
// Player (Materials)
//
// Copyright (C) Faust Logic, Inc.
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

new Material(SpaceOrcEye_MTL)
{
   diffuseMap[0] = "orc_ID6_eye";
   emissive[0] = true;
   glow[0] = true;
};

// Note - Setting the cubemap is currently problematic with macos
if ($platform $= "macos") // AFX OFF
{
   new Material(SpaceOrc_MTL)
   {
      diffuseMap[0] = "Orc_Material";
      bumpTex[0] = "Orc_Material_normal";
      //cubemap = AFX_DaySky_Cubemap;
      pixelSpecular[0] = true;
      specular[0] = "0.5 0.5 0.5 0.5";
      specularPower[0] = 8.0;
   };

   new Material(SpaceOrc_Night_MTL)
   {
      mapTo = "night.body";
      diffuseMap[0] = "Orc_Material";
      bumpTex[0] = "Orc_Material_normal";
      //cubemap = AFX_NightSky_Cubemap;
      pixelSpecular[0] = true;
      specular[0] = "0.5 0.5 0.5 0.5";
      specularPower[0] = 8.0;
   };
}
else
{
   new Material(SpaceOrc_MTL)
   {
      diffuseMap[0] = "Orc_Material";
      bumpTex[0] = "Orc_Material_normal";
      cubemap = AFX_DaySky_Cubemap;
      pixelSpecular[0] = true;
      specular[0] = "0.5 0.5 0.5 0.5";
      specularPower[0] = 8.0;
   };

   new Material(SpaceOrc_Night_MTL)
   {
      mapTo = "night.body";
      diffuseMap[0] = "Orc_Material";
      bumpTex[0] = "Orc_Material_normal";
      cubemap = AFX_NightSky_Cubemap;
      pixelSpecular[0] = true;
      specular[0] = "0.5 0.5 0.5 0.5";
      specularPower[0] = 8.0;
   };
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

