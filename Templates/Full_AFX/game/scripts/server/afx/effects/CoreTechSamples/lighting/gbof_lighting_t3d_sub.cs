
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// GREAT BALL OF FIRE (lighting)
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

$GBoF_ZodiacRevealLighting = true;
$GBoF_MainZodiacLighting = true;
$GBoF_PortalFlameLighting = true;
$GBoF_FireballLighting = true;
$GBoF_ImpactLighting = true;

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// Zodiac Reveal Lighting (white)
//
// 3 rotating white spot-lights placed below ground-level and
// pointed up and in toward the spellcaster. These coincide with
// the glowing white reveal zodiac that occurs at the beginning
// of the spell so that it looks like the white zodiac is
// illuminatiing the spellcaster from below.
//
if ($GBoF_ZodiacRevealLighting)
{
  datablock afxXM_LocalOffsetData(GBoF_MainZodeRevealLight_offset_XM)
  {
    localOffset =  "0 2 -4";
  };
  datablock afxXM_SpinData(GBoF_MainZodeRevealLight_spin1_XM)
  {
    spinAxis = "0 0 1";
    spinRate = -30;
    spinAngle = 0;
  };
  datablock afxXM_SpinData(GBoF_MainZodeRevealLight_spin2_XM : GBoF_MainZodeRevealLight_spin1_XM)
  {
    spinAngle = 120;
  };
  datablock afxXM_SpinData(GBoF_MainZodeRevealLight_spin3_XM : GBoF_MainZodeRevealLight_spin1_XM)
  {
    spinAngle = 240;
  };
  datablock afxXM_AimData(GBoF_MainZodeRevealLight_aim_XM)
  {
    aimZOnly = false;
  };

  datablock afxT3DSpotLightData(GBoF_MainZodeRevealLight_CE)
  {
    range = 7;
    color = "1 1 1";
    brightness = 2;
    castShadows = false;
    localRenderViz = false;
  };

  datablock afxEffectWrapperData(GBoF_MainZodeRevealLight_1_EW : GBoF_Zode_Reveal_EW)
  {
    effect = GBoF_MainZodeRevealLight_CE;
    posConstraint = "caster";
    posConstraint2 = "caster.#center";
    lifetime = 0.75;
    fadeInTime = 0.5;
    fadeOutTime = 0.25;
    propagateTimeFactor = true;
    xfmModifiers[0] = GBoF_MainZodeRevealLight_spin1_XM;
    xfmModifiers[1] = GBoF_MainZodeRevealLight_offset_XM;
    xfmModifiers[2] = GBoF_MainZodeRevealLight_aim_XM;
  };
  datablock afxEffectWrapperData(GBoF_MainZodeRevealLight_2_EW : GBoF_MainZodeRevealLight_1_EW)
  {
    xfmModifiers[0] = GBoF_MainZodeRevealLight_spin2_XM;
  };
  datablock afxEffectWrapperData(GBoF_MainZodeRevealLight_3_EW : GBoF_MainZodeRevealLight_1_EW)
  {
    xfmModifiers[0] = GBoF_MainZodeRevealLight_spin3_XM;
  };
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// Main Zodiac Lighting (orange)
//
// An orange point-light placed below ground-level representing
// illumination coming from the spells main zodiacs.
//

if ($GBoF_MainZodiacLighting)
{
  // this offset defines the lights distance beneath the terrain  
  datablock afxXM_LocalOffsetData(GBoF_CastingZodeLight_offset_XM)
  {
    localOffset = "0 0 -4";
  };

  datablock afxT3DPointLightData(GBoF_CastingZodeLight_CE)
  {
    radius = 7;
    color = "1.0 0.5 0.0";
    brightness = 3.0;
    castShadows = false;
    localRenderViz = false;
  };

  datablock afxEffectWrapperData(GBoF_CastingZodeLight_EW : GBoF_Zode1_EW)
  {
    effect = GBoF_CastingZodeLight_CE;
    xfmModifiers[0] = GBoF_CastingZodeLight_offset_XM;
  };
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// Portal Flame Lighting (orange-yellow)
//
// 5 point-lights with flares representing illumination from
// the 5 small portal flames. These lights are constrained
// to the particle flames in the main effect. 
// 
if ($GBoF_PortalFlameLighting)
{
  datablock LightFlareData(GBoF_FirePortalLight_FLARE)
  {
    overallScale = 1.0;
    flareEnabled = true;
    flareTexture = %mySpellDataPath @ "/GBoF/lights/GBoF_firePortalFlare";
    elementRect[0] = "0 0 128 128";
    elementDist[0] = 0.0;
    elementScale[0] = 2;
    elementTint[0] = "1 1 1";
    elementRotate[0] = true;
    elementUseLightColor[0] = true;
  };

  datablock LightAnimData(GBoF_FirePortalLight_ANI)
  {
    flicker = true;
    chanceTurnOn = 0.9;
    chanceTurnOff = 0.5;
    animEnabled = true;
    minBrightness = 0.5;
    maxBrightness = 1.0;
  };

  datablock afxT3DPointLightData(GBoF_FirePortalLight1_CE)
  {
    radius = 0.8;
    color = "1.0 0.6 0.0";
    brightness = 0.1;
    flareType = GBoF_FirePortalLight_FLARE;
    flareScale = 1;
    animate = true;
    animationType = GBoF_FirePortalLight_ANI;
    animationPeriod = 0.1;
    castShadows = false;
    localRenderViz = false;
  };
  datablock afxT3DPointLightData(GBoF_FirePortalLight2_CE : GBoF_FirePortalLight1_CE)
  {
     animationPeriod = 0.2;
  };
  datablock afxT3DPointLightData(GBoF_FirePortalLight3_CE : GBoF_FirePortalLight1_CE)
  {
     animationPeriod = 0.31;
  };
  datablock afxT3DPointLightData(GBoF_FirePortalLight4_CE : GBoF_FirePortalLight1_CE)
  {
     animationPeriod = 0.15;
  };
  datablock afxT3DPointLightData(GBoF_FirePortalLight5_CE : GBoF_FirePortalLight1_CE)
  {
     animationPeriod = 0.25;
  };

  datablock afxEffectWrapperData(GBoF_FirePortalLight1_EW)
  {
    effect = GBoF_FirePortalLight1_CE;
    posConstraint = "#effect.PortalFlame1";
    delay = 2.0;
    lifetime = 3.5;
    fadeInTime  = 0.5;
    fadeOutTime = 0.25;
  };
  datablock afxEffectWrapperData(GBoF_FirePortalLight2_EW : GBoF_FirePortalLight1_EW)
  {
    effect = GBoF_FirePortalLight2_CE;
    posConstraint = "#effect.PortalFlame2";
  };
  datablock afxEffectWrapperData(GBoF_FirePortalLight3_EW : GBoF_FirePortalLight1_EW)
  {
    effect = GBoF_FirePortalLight3_CE;
    posConstraint = "#effect.PortalFlame3";
  };
  datablock afxEffectWrapperData(GBoF_FirePortalLight4_EW : GBoF_FirePortalLight1_EW)
  {
    effect = GBoF_FirePortalLight4_CE;
    posConstraint = "#effect.PortalFlame4";
  };
  datablock afxEffectWrapperData(GBoF_FirePortalLight5_EW : GBoF_FirePortalLight1_EW)
  {
    effect = GBoF_FirePortalLight5_CE;
    posConstraint = "#effect.PortalFlame5";
  };
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// Fireball Lighting (yellow and white)
//
// This light represents light from the huge fireball. Note that
// the parameters match those of the light belonging to the missile.
//
// The fireball light is simulated using two lights, each casting 
// dynamic shadows and animated with a path.  The path gives the
// lights a fast wiggle which is very visible in the shadow.  Two
// lights are used to create an effective color on the groundplane:
// the larger-radius yellow light followed by a smaller white light
// that whitens the center.
//
if ($GBoF_FireballLighting)
{
  datablock afxPathData(GBoF_FireBallLight_TLK_1_Path)
  {
    points = " 0    0    0"   SPC
             " 0.1 -0.3  0.8" SPC
             "-0.3  0.2 -0.6" SPC
             " 0.0 -0.6  0.4" SPC
             " -0.7 0.4 -0.8" SPC
             " 0    0    0";
    lifetime = 0.25;
    loop = cycle;
    mult = 0.65;
  };
  //
  datablock afxXM_PathConformData(GBoF_FireBallLight_TLK_1_path_XM)
  {
    paths = "GBoF_FireBallLight_TLK_1_Path";
  };

  datablock afxT3DPointLightData(GBoF_FireBallLight_1_CE)
  {
    radius = 7.0;
    color = "1.0 0.9 0.0";
    brightness = 0.5;
    castShadows = false; // T3D Off (temporarily)
    localRenderViz = false;
  };

  datablock afxEffectWrapperData(GBoF_FireBallLight_1_EW)
  {
    effect = GBoF_FireBallLight_1_CE;
    posConstraint = "caster";
    delay    = 4.6;
    lifetime = 1.4;
    fadeInTime  = 0.5;
    xfmModifiers[0] = GBoF_FireBall_Offset_XM;
    xfmModifiers[1] = GBoF_FireBallLight_TLK_1_path_XM;
  };

  //~~~~~~~~~~~~~~~~~~~~//

  datablock afxPathData(GBoF_FireBallLight_TLK_2_Path)
  {
    points = " 0    0    0"   SPC
             " 0.4  0.7 -0.3" SPC
             "-0.3  0.0  0.4" SPC
             " 0.2  0.4 -0.8" SPC
             "-0.4 -0.8  0.5" SPC
             " 0    0    0";
    lifetime = 0.20;
    loop = cycle;
    mult = 0.65;
  };
  //
  datablock afxXM_PathConformData(GBoF_FireBallLight_TLK_2_path_XM)
  {
    paths = "GBoF_FireBallLight_TLK_2_Path";
  };

  datablock afxT3DPointLightData(GBoF_FireBallLight_2_CE)
  {
    radius = 5.0;
    color = "1.0 1.0 1.0";
    brightness = 2.0;
    castShadows = false; // T3D Off (temporarily)
    localRenderViz = false;
  };

  datablock afxEffectWrapperData(GBoF_FireBallLight_2_EW)
  {
    effect = GBoF_FireBallLight_2_CE;
    posConstraint = "caster";  
    delay    = 4.6;
    lifetime = 1.4;
    fadeInTime  = 0.5;
    xfmModifiers[0] = GBoF_FireBall_Offset_XM;
    xfmModifiers[1] = GBoF_FireBallLight_TLK_2_path_XM;
  };

  // same lights constrained to the missile

  datablock afxEffectWrapperData(GBoF_FireBallMissileLight_1_EW)
  {
    effect = GBoF_FireBallLight_1_CE;
    posConstraint = "missile";  
    xfmModifiers[0] = GBoF_FireBallLight_TLK_1_path_XM;
  };

  datablock afxEffectWrapperData(GBoF_FireBallMissileLight_2_EW)
  {
    effect = GBoF_FireBallLight_2_CE;
    posConstraint = "missile";  
    xfmModifiers[0] = GBoF_FireBallLight_TLK_2_path_XM;
  };
}

if ($GBoF_ImpactLighting)
{
  datablock afxXM_LocalOffsetData(GBoF_ImpactFlare_offset_XM)
  {
    localOffset = "0 0 1.5";
  };

  // Impact Lights 1 (A and B) -- occur on any impact

  datablock LightFlareData(GBoF_ImpactLight1_FLARE)
  {
    overallScale = 1.0;
    flareEnabled = true;
    flareTexture = %mySpellDataPath @ "/GBoF/lights/GBoF_lightFalloffMono";
    elementRect[0] = "0 0 256 256";
    elementDist[0] = 0.0;
    elementScale[0] = 20;
    elementTint[0] = "1 1 1";
    elementRotate[0] = false;
    elementUseLightColor[0] = true;
  };

  datablock afxT3DPointLightData(GBoF_ImpactLight1_flare_CE)
  {
     radius = 10;
     color = "1.0 1.0 1.0";
     brightness = 10.0;
     castShadows = false;
     localRenderViz = false;
     flareType = GBoF_ImpactLight1_FLARE;
     flareScale = 1.0;
  };

  datablock afxEffectWrapperData(GBoF_ImpactLight1A_EW)
  {
    effect = GBoF_ImpactLight1_flare_CE;
    posConstraint = "impactPoint";  
    delay    = 0.0;
    lifetime = 0.25;
    fadeInTime  = 0.25;
    fadeOutTime = 0.75;
    execConditions[0] = $AFX::IMPACTED_SOMETHING;
    xfmModifiers[0] = GBoF_ImpactFlare_offset_XM;
  };

  //~~~~~~~~~~~~~~~~~~~~//

  datablock LightFlareData(GBoF_ImpactLight2_FLARE)
  {
    overallScale = 1.0;
    flareEnabled = true;
    flareTexture = %mySpellDataPath @ "/GBoF/lights/GBoF_corona";
    elementRect[0] = "0 0 128 128";
    elementDist[0] = 0.0;
    elementScale[0] = 20;
    elementTint[0] = "1 1 1";
    elementRotate[0] = false;
    elementUseLightColor[0] = true;
  };

  datablock afxT3DPointLightData(GBoF_ImpactLight2_flare_CE)
  {
     radius = 18;
     color = "1.0 0.6 0.0";
     brightness = 1.0;
     castShadows = false;
     localRenderViz = false;
     flareType = GBoF_ImpactLight2_FLARE;
     flareScale = 1.0;
  };

  datablock afxEffectWrapperData(GBoF_ImpactLight1B_EW : GBoF_ImpactLight1A_EW)
  {
    effect = GBoF_ImpactLight2_flare_CE;
    delay = 0.05;
  };

  // Impact Lights 2 (A and B) -- occur only on non-corpse player impacts

  datablock afxEffectWrapperData(GBoF_ImpactLight2A_EW : GBoF_ImpactLight1A_EW)
  {
    delay = 0.4;
    execConditions[0] = $AFX::IMPACTED_PRIMARY;
  };

  datablock afxEffectWrapperData(GBoF_ImpactLight2B_EW : GBoF_ImpactLight1B_EW)
  {
    delay = 0.45;
    execConditions[0] = $AFX::IMPACTED_PRIMARY;
  };

  // Impact Lights 3 (A and B) -- occur only on non-corpse player impacts

  datablock afxT3DPointLightData(GBoF_ImpactLightBig1_flare_CE : GBoF_ImpactLight1_flare_CE)
  {
     radius = 20;
     flareScale = 2.0;
  };

  datablock afxEffectWrapperData(GBoF_ImpactLight3A_EW : GBoF_ImpactLight1A_EW)
  {
    effect = GBoF_ImpactLightBig1_flare_CE;
    delay = 1.2;
    lifetime = 0.5;
    fadeInTime = 0.25;
    fadeOutTime = 1.5;
    execConditions[0] = $AFX::IMPACTED_PRIMARY;
  };

  datablock afxT3DPointLightData(GBoF_ImpactLightBig2_flare_CE : GBoF_ImpactLight2_flare_CE)
  {
     radius = 36.0;
     flareScale = 2.0;
  };

  datablock afxEffectWrapperData(GBoF_ImpactLight3B_EW : GBoF_ImpactLight1B_EW)
  {
    effect = GBoF_ImpactLightBig2_flare_CE;
    delay = 1.3;
    lifetime = 0.5;
    fadeInTime = 0.25;
    fadeOutTime = 1.5;
    execConditions[0] = $AFX::IMPACTED_PRIMARY;
  };
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

// add effects to main spell datablock
function GBoF_add_Lighting_FX(%spell_data)
{
   if ($GBoF_ZodiacRevealLighting)
   {
      %spell_data.addCastingEffect(GBoF_MainZodeRevealLight_1_EW);
      %spell_data.addCastingEffect(GBoF_MainZodeRevealLight_2_EW);
      %spell_data.addCastingEffect(GBoF_MainZodeRevealLight_3_EW);
   }

   if ($GBoF_MainZodiacLighting)
   {
      %spell_data.addCastingEffect(GBoF_CastingZodeLight_EW);
   }

   if ($GBoF_PortalFlameLighting)
   {
      %spell_data.addCastingEffect(GBoF_FirePortalLight1_EW);
      %spell_data.addCastingEffect(GBoF_FirePortalLight2_EW);
      %spell_data.addCastingEffect(GBoF_FirePortalLight3_EW);
      %spell_data.addCastingEffect(GBoF_FirePortalLight4_EW);
      %spell_data.addCastingEffect(GBoF_FirePortalLight5_EW);
   }

   if ($GBoF_FireballLighting)
   {
      %spell_data.addCastingEffect(GBoF_FireBallLight_1_EW);
      %spell_data.addCastingEffect(GBoF_FireBallLight_2_EW);
      %spell_data.addDeliveryEffect(GBoF_FireBallMissileLight_1_EW);
      %spell_data.addDeliveryEffect(GBoF_FireBallMissileLight_2_EW);
   }

   if ($GBoF_ImpactLighting)
   {
      %spell_data.addImpactEffect(GBoF_ImpactLight1A_EW);
      %spell_data.addImpactEffect(GBoF_ImpactLight1B_EW);
      %spell_data.addImpactEffect(GBoF_ImpactLight2A_EW);
      %spell_data.addImpactEffect(GBoF_ImpactLight2B_EW);
      %spell_data.addImpactEffect(GBoF_ImpactLight3A_EW);
      %spell_data.addImpactEffect(GBoF_ImpactLight3B_EW);
   }
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
