
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// FLAME BROIL (lighting)
//
// Copyright (C) 2015 Faust Logic, Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
//
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

datablock afxXM_LocalOffsetData(FB_MainZodeRevealLight_offset_XM)
{
  localOffset = "0 2 -4";
};

datablock afxXM_AimData(FB_MainZodeRevealLight_aim_XM)
{
  aimZOnly = false;
};

datablock afxXM_SpinData(FB_MainZodeRevealLight_spin1_XM)
{
  spinAxis = "0 0 1";
  spinAngle = 0;
  spinRate = -30;
};
datablock afxXM_SpinData(FB_MainZodeRevealLight_spin2_XM : FB_MainZodeRevealLight_spin1_XM)
{
  spinAngle = 120;
};
datablock afxXM_SpinData(FB_MainZodeRevealLight_spin3_XM : FB_MainZodeRevealLight_spin1_XM)
{
  spinAngle = 240;
};

datablock afxT3DSpotLightData(FB_MainZodeRevealLight_CE)
{
  range = 7;
  color = "1.0 1.0 1.0";
  brightness = 1.25;
  castShadows = false;
  localRenderViz = false;
};

datablock afxEffectWrapperData(FB_MainZodeRevealLight_1_EW)
{
  effect = FB_MainZodeRevealLight_CE;
  posConstraint = caster;
  posConstraint2 = "caster.#center";
  lifetime = 0.75;
  fadeInTime = 0.5;
  fadeOutTime = 0.25;
  xfmModifiers[0] = FB_MainZodeRevealLight_spin1_XM;
    xfmModifiers[1] = FB_MainZodeRevealLight_offset_XM;
    xfmModifiers[2] = FB_MainZodeRevealLight_aim_XM;
};
datablock afxEffectWrapperData(FB_MainZodeRevealLight_2_EW : FB_MainZodeRevealLight_1_EW)
{
  xfmModifiers[0] = FB_MainZodeRevealLight_spin2_XM;
};
datablock afxEffectWrapperData(FB_MainZodeRevealLight_3_EW : FB_MainZodeRevealLight_1_EW)
{
  xfmModifiers[0] = FB_MainZodeRevealLight_spin3_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock afxXM_SpinData(FB_ZodeGlowRingLight_spin1_XM : FB_MainZodeRevealLight_spin1_XM)
{
  spinAngle = -60;
};
datablock afxXM_SpinData(FB_ZodeGlowRingLight_spin2_XM : FB_ZodeGlowRingLight_spin1_XM)
{
  spinAngle = 60;
};
datablock afxXM_SpinData(FB_ZodeGlowRingLight_spin3_XM : FB_ZodeGlowRingLight_spin1_XM)
{
  spinAngle = 180;
};

datablock afxT3DSpotLightData(FB_ZodeGlowRingLight_CE)
{
  range = 10;
  color = "1.0 1.0 1.0";
  brightness = 2.5;
  castShadows = false;
  localRenderViz = false;
};

datablock afxEffectWrapperData(FB_ZodeGlowRingLight_1_EW)
{
  effect = FB_ZodeGlowRingLight_CE;
  posConstraint = caster;
  posConstraint2 = "caster.#center";
  delay = 0.6;
  lifetime = 0.7;
  fadeInTime = 0.6;
  fadeOutTime = 0.25;
  xfmModifiers[0] = FB_ZodeGlowRingLight_spin1_XM;
  xfmModifiers[1] = FB_MainZodeRevealLight_offset_XM;
  xfmModifiers[2] = FB_MainZodeRevealLight_aim_XM;
};
datablock afxEffectWrapperData(FB_ZodeGlowRingLight_2_EW : FB_ZodeGlowRingLight_1_EW)
{
  xfmModifiers[0] = FB_ZodeGlowRingLight_spin2_XM;
};
datablock afxEffectWrapperData(FB_ZodeGlowRingLight_3_EW : FB_ZodeGlowRingLight_1_EW)
{
  xfmModifiers[0] = FB_ZodeGlowRingLight_spin3_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock afxXM_LocalOffsetData(FB_FireRingLight_offset2_XM)
{
  localOffset = "0 0 -2";
};

// ring-of-fire lights

datablock afxT3DPointLightData(FB_FireRingLight_CE)
{
  radius = 6;
  color = "1.0 0.5 0.0";
  brightness = 15;
  castShadows = false;
  localRenderViz = false;
};

datablock afxEffectWrapperData(FB_FireRingLight_1_EW)
{
  effect = FB_FireRingLight_CE;
  posConstraint = caster;
  delay = 1.35;
  fadeInTime  = 0.5;
  fadeOutTime = 0.5;
  lifetime = 1.6;
  xfmModifiers[0] = FB_FireRingLight_offset_XM;
    xfmModifiers[1] = FB_FireRingLight_1_path_XM;
};

datablock afxEffectWrapperData(FB_FireRingLight_2_EW : FB_FireRingLight_1_EW)
{
  xfmModifiers[1] = FB_FireRingLight_2_path_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock afxT3DSpotLightData(FB_FireRingSpotLight_CE)
{
  range = 10;
  color = "1.0 0.5 0.0";
  brightness = 2.5;
  castShadows = false;
  localRenderViz = false;
};

datablock afxEffectWrapperData(FB_FireRingSpotLight_1_EW)
{
  effect = FB_FireRingSpotLight_CE;
  posConstraint = caster;
  posConstraint2 = "caster.#center";
  delay = 1.35;
  fadeInTime  = 0.5;
  fadeOutTime = 0.5;
  lifetime = 1.6;
  xfmModifiers[0] = FB_MainZodeRevealLight_spin1_XM;
    xfmModifiers[1] = FB_MainZodeRevealLight_offset_XM;
    xfmModifiers[2] = FB_MainZodeRevealLight_aim_XM;
};
datablock afxEffectWrapperData(FB_FireRingSpotLight_2_EW : FB_FireRingSpotLight_1_EW)
{
  xfmModifiers[0] = FB_MainZodeRevealLight_spin2_XM;
};
datablock afxEffectWrapperData(FB_FireRingSpotLight_3_EW : FB_FireRingSpotLight_1_EW)
{
  xfmModifiers[0] = FB_MainZodeRevealLight_spin3_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

// this light coordinates with FB_FireballEmerge_EW to create
// a fireball in the caster's right hand.
//

datablock LightFlareData(FB_FireballLight1_FLARE)
{
  overallScale = 1.0;
  flareEnabled = true;
  flareTexture = %mySpellDataPath @ "/FB/lights/FB_firePortalFlare";
  elementRect[0] = "0 0 128 128";
  elementDist[0] = 0.0;
  elementScale[0] = 5.0;
  elementTint[0] = "1 1 1";
  elementRotate[0] = true;
  elementUseLightColor[0] = true;
};

datablock LightAnimData(FB_FireballLight1_ANI)
{
  animEnabled = true;
  minBrightness = 0.3;
  maxBrightness = 0.9;
};

datablock afxT3DPointLightData(FB_FireballLight1_CE)
{
  radius = 3.5;
  color = "1.0 0.6 0.0";
  brightness = 0.375;
  castShadows = false;
  localRenderViz = false;
  flareType = FB_FireballLight1_FLARE;
  flareScale = 1;
  animate = true;
  animationType = FB_FireballLight1_ANI;
  animationPeriod = 0.1;
  animationPhase = 0.0;
};

datablock afxT3DPointLightData(FB_FireballLight2_CE : FB_FireballLight1_CE)
{
  radius = 1.5;
  animationPeriod = 0.15;
};

datablock afxEffectWrapperData(FB_FireballRevealLight1_EW)
{
  effect = FB_FireballLight1_CE;
  posConstraint = "caster.Bip01 R Hand";  
  delay = 2.7-0.5;
  fadeInTime  = 0.2;
  fadeOutTime = 0.1;
  lifetime = 0.9+0.5;
};

datablock afxEffectWrapperData(FB_FireballRevealLight2_EW : FB_FireballRevealLight1_EW)
{
  effect = FB_FireballLight2_CE;
};

datablock afxEffectWrapperData(FB_FireballFlare1_EW)
{
  effect = FB_FireballLight1_CE;
  constraint = missile;
};
datablock afxEffectWrapperData(FB_FireballFlare2_EW : FB_FireballFlare1_EW)
{
  effect = FB_FireballLight2_CE;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock LightFlareData(FB_ImpactLight1_flare_FLARE)
{
  overallScale = 1.0;
  flareEnabled = true;
  flareTexture = %mySpellDataPath @ "/FB/lights/FB_lightFalloffMono";
  elementRect[0] = "0 0 256 256";
  elementDist[0] = 0.0;
  elementScale[0] = 20.0;
  elementTint[0] = "1 1 1";
  elementRotate[0] = false;
  elementUseLightColor[0] = true;
};

datablock afxT3DPointLightData(FB_ImpactLight1_flare_CE)
{
  radius = 10;
  color = "1.0 1.0 1.0";
  brightness = 10.0;
  castShadows = false;
  localRenderViz = false;
  flareType = FB_ImpactLight1_flare_FLARE;
  flareScale = 1;
};

// flare line-of-sight...
datablock afxXM_LocalOffsetData(FB_ImpactFlare_offset_XM)
{
  localOffset = "0 0 1.5";
};

datablock afxEffectWrapperData(FB_ImpactLight1_EW)
{
  effect = FB_ImpactLight1_flare_CE;
  posConstraint = "impactPoint";  
  delay    = 0.0;
  lifetime = 0.25;
  fadeInTime  = 0.25;
  fadeOutTime = 0.75;
  execConditions[0] = $AFX::IMPACTED_SOMETHING;
  xfmModifiers[0] = FB_ImpactFlare_offset_XM;
};

datablock LightFlareData(FB_ImpactLight2_flare_FLARE)
{
  overallScale = 1.0;
  flareEnabled = true;
  flareTexture = %mySpellDataPath @ "/FB/lights/FB_corona";
  elementRect[0] = "0 0 128 128";
  elementDist[0] = 0.0;
  elementScale[0] = 30.0;
  elementTint[0] = "1 1 1";
  elementRotate[0] = false;
  elementUseLightColor[0] = true;
};

datablock afxT3DPointLightData(FB_ImpactLight2_flare_CE)
{
  radius = 18;
  color = "1.0 0.2 0.0";
  brightness = 5.0;
  castShadows = false;
  localRenderViz = false;
  flareType = FB_ImpactLight2_flare_FLARE;
  flareScale = 1;
};

datablock afxEffectWrapperData(FB_ImpactLight2_EW : FB_ImpactLight1_EW)
{
  effect = FB_ImpactLight2_flare_CE;
  delay = 0.05;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

function FB_add_Lighting_FX(%spell_data)
{
  %spell_data.addCastingEffect(FB_MainZodeRevealLight_1_EW);
  %spell_data.addCastingEffect(FB_MainZodeRevealLight_2_EW);
  %spell_data.addCastingEffect(FB_MainZodeRevealLight_3_EW);

  %spell_data.addCastingEffect(FB_ZodeGlowRingLight_1_EW);
  %spell_data.addCastingEffect(FB_ZodeGlowRingLight_2_EW);
  %spell_data.addCastingEffect(FB_ZodeGlowRingLight_3_EW);

  %spell_data.addCastingEffect(FB_FireRingLight_1_EW);
  %spell_data.addCastingEffect(FB_FireRingLight_2_EW);

  %spell_data.addCastingEffect(FB_FireRingSpotLight_1_EW);
  %spell_data.addCastingEffect(FB_FireRingSpotLight_2_EW);
  %spell_data.addCastingEffect(FB_FireRingSpotLight_3_EW);

  %spell_data.addCastingEffect(FB_FireballRevealLight1_EW);
  %spell_data.addCastingEffect(FB_FireballRevealLight2_EW);

  %spell_data.addDeliveryEffect(FB_FireballFlare1_EW);
  %spell_data.addDeliveryEffect(FB_FireballFlare2_EW);

  %spell_data.addImpactEffect(FB_ImpactLight1_EW);
  %spell_data.addImpactEffect(FB_ImpactLight2_EW);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
