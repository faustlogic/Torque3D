
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX
//
// SOUL NUKE (lighting)
//
// Copyright (C) Faust Logic, Inc.
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

datablock LightFlareData(SN_Light1_flare_FLARE)
{
  overallScale = 1.0;
  flareEnabled = true;
  flareTexture = %mySpellDataPath @ "/Shared/lights/sp2_falloffFlare";
  elementRect[0] = "0 0 256 256";
  elementDist[0] = 0.0;
  elementScale[0] = 30.0;
  elementTint[0] = "1 1 1";
  elementRotate[0] = false;
  elementUseLightColor[0] = true;
};

datablock afxT3DPointLightData(SN_Light1_flare_CE)
{
  radius = 10;
  color = "1 1 1";
  brightness = 1;
  castShadows = false;
  localRenderViz = false;
  flareType = SN_Light1_flare_FLARE;
  flareScale = 1.0;
  flareScale = "$$ %%._expScale";
};

// flare line-of-sight...
datablock afxXM_AimData(SN_Flare_aim_XM)
{
  aimZOnly = true;  
};
datablock afxXM_LocalOffsetData(SN_Flare_offset_XM)
{
  localOffset = "0 1 0";
};

datablock afxEffectWrapperData(SN_Light1_EW)
{
  effect = SN_Light1_flare_CE;
  posConstraint = "mine";
  lifetime = 0.3;
  fadeInTime  = 0.3;
  fadeOutTime = 0.10;
  xfmModifiers[0] = SN_Flare_aim_XM;
  xfmModifiers[1] = SN_Flare_offset_XM;
  posConstraint2 = "camera"; // aim
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// EXPLOSION LIGHT

datablock afxXM_WorldOffsetData(SN_Light_2_offset_XM)
{
  worldOffset = "0 0 4";
};

datablock afxT3DPointLightData(SN_Light_2_CE)
{
  radius = "$$ 35 * %%._expScale";
  color = "0.094 2.88 2.22";
  brightness = 1;
  castShadows = false; // T3D Off (temporarily)
  localRenderViz = false;
};

datablock afxEffectWrapperData(SN_Light2_EW)
{
  effect = SN_Light_2_CE;
  posConstraint = "mine";
  delay = 0.05;
  fadeInTime  = "$$ 0.15 * %%._expScale";
  fadeOutTime = 0.60;
  lifetime = "$$ 0.15 * %%._expScale";
  xfmModifiers[0] = SN_Light_2_offset_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

function SN_add_Lighting_FX(%effect_data)
{
  %effect_data.addEffect(SN_Light1_EW);
  %effect_data.addEffect(SN_Light2_EW);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
