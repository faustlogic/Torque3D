
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// STANDARD SELECTRON (Core Tech)
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

$AFX_VERSION = (isFunction(afxGetVersion)) ? afxGetVersion() : 1.02;
$MIN_REQUIRED_VERSION = 1.12;

// Test version requirements for this script
if ($AFX_VERSION < $MIN_REQUIRED_VERSION)
{
  error("AFX script " @ fileName($afxAutoloadScriptFile) @ " is not compatible with AFX versions older than " @ $MIN_REQUIRED_VERSION @ ".");
  return;
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

$thisEngine = afxGetEngine();

// style numbers
$AFX_Default_Style = 0;

%mySelectronDataPath = $afxSpellDataPath @ "/" @ $afxAutoloadScriptFolder @ "/SELE";

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// AFX DEFAULT Selectron Style
//
//    This is the default selectron used in the AFX demo.
//    It's a rippling yellow ring with a preliminary glow
//    plus sfx on selection and deselection.
//

// Initial Glow A1 (clockwise)
datablock afxZodiacData(AFX_Default_glow_Zode_CE : SHARED_SelectronZodiac_CE)
{  
  texture = %mySelectronDataPath @ "/afx_default/DEF_SELE_glow.png";
  radius = 2.0;//1.5;
  startAngle = -60.0*0.5;
  rotationRate = 60.0;
  color = "1.0 1.0 1.0 1.0";
  blend = additive;
};
datablock afxEffectWrapperData(AFX_Default_glow_Zode_EW)
{
  effect = AFX_Default_glow_Zode_CE;
  posConstraint = selected;
  lifetime = 0.7;
  fadeInTime = 0.50;
  fadeOutTime = 0.20;
  delay = 0;
  effectName = "MainGlowZode";
  isConstraintSrc = true;
  xfmModifiers[0] = SHARED_AltitudeConform_XM;
};

// Initial Glow A2 (counter-clockwise)
datablock afxZodiacData(AFX_Default_glow2_Zode_CE : AFX_Default_glow_Zode_CE)
{  
  startAngle = 60.0*0.5;
  rotationRate = -60.0;
};
datablock afxEffectWrapperData(AFX_Default_glow2_Zode_EW)
{
  effect = AFX_Default_glow2_Zode_CE;
  posConstraint = "#effect.MainGlowZode";
  borrowAltitudes = true;
  lifetime = 0.7;
  fadeInTime = 0.50;
  fadeOutTime = 0.20;
  delay = 0;
};

// Main Zodiac A1 (clockwise)
datablock afxZodiacData(AFX_Default_Zode1_CE : SHARED_SelectronZodiac_CE)
{  
  texture = %mySelectronDataPath @ "/afx_default/DEF_SELE_zode.png";
  radius = 2.0;//1.5;
  startAngle = 0.0;
  rotationRate = 60.0;
  color = "1.0 1.0 1.0 1.0";
  blend = additive;
  distanceMax = ($thisEngine $= "T3D") ? 75 : 200;
  distanceFalloff = ($thisEngine $= "T3D") ? 30 : 180;
};
datablock afxEffectWrapperData(AFX_Default_Zode1_EW)
{
  effect = AFX_Default_Zode1_CE;
  posConstraint = selected;
  fadeInTime = 0.20;
  fadeOutTime = 0.20;
  delay = 0.5;
  effectName = "MainZode";
  isConstraintSrc = true;
  xfmModifiers[0] = SHARED_AltitudeConform_XM;
};

// Main Zodiac A2 (counter-clockwise)
datablock afxZodiacData(AFX_Default_Zode2_CE : AFX_Default_Zode1_CE)
{  
  rotationRate = -60.0;
};
datablock afxEffectWrapperData(AFX_Default_Zode2_EW)
{
  effect = AFX_Default_Zode2_CE;
  posConstraint = "#effect.MainZode";
  borrowAltitudes = true;
  fadeInTime = 0.20;
  fadeOutTime = 0.20;
  delay = 0.5;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// Selectron Sounds

// Shared Audio Description
datablock SFXDescription(AFX_Default_Snd_AD)
{
  volume             = 1.0;
  isLooping          = false;
  is3D               = true;
  ReferenceDistance  = 50.0;
  MaxDistance        = 180.0;
  channel            = $SimAudioType;
};

// Selection Sound
datablock SFXProfile(AFX_Default_Select_Snd_CE)
{
   //fileName = %mySelectronDataPath @ "/afx_default/DEF_SELE_select_snd";
   fileName = %mySelectronDataPath @ "/afx_default/DEF_SELE_select_snd.ogg";
   description = AFX_Default_Snd_AD;
   preload = true;
};
datablock afxEffectWrapperData(AFX_Default_Select_Snd_EW)
{
  effect = AFX_Default_Select_Snd_CE;
  constraint = "selected";
  lifetime = 1.483;
  scaleFactor = 0.5;
};

// Deselection Sound
datablock SFXProfile(AFX_Default_Deselect_Snd_CE)
{
   //fileName = %mySelectronDataPath @ "/afx_default/DEF_SELE_deselect_snd";
   fileName = %mySelectronDataPath @ "/afx_default/DEF_SELE_deselect_snd.ogg";
   description = AFX_Default_Snd_AD;
   preload = true;
};
datablock afxEffectWrapperData(AFX_Default_Deselect_Snd_Ew)
{
  effect = AFX_Default_Deselect_Snd_CE;
  constraint = "selected";
  lifetime = 0.269;
  scaleFactor = 0.5;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// LIGHTING

if ($thisEngine $= "T3D")
{
  datablock afxT3DSpotLightData(AFX_Default_RevealLight_CE)
  {
    range = 8;
    color = "2.5 2.5 2.5";
    brightness = 1;
    castShadows = false;
    localRenderViz = false;
  };

  datablock afxXM_LocalOffsetData(AFX_Default_RevealLight_offset_XM)
  {
    localOffset = "0 2 -4";
  };
  datablock afxXM_SpinData(AFX_Default_RevealLight_spin1_XM)
  {
    spinAxis = "0 0 1";
    spinAngle = 0;
    spinRate = -30;
  };
  datablock afxXM_SpinData(AFX_Default_RevealLight_spin2_XM : AFX_Default_RevealLight_spin1_XM)
  {
    spinAngle = 120;
  };
  datablock afxXM_SpinData(AFX_Default_RevealLight_spin3_XM : AFX_Default_RevealLight_spin1_XM)
  {
    spinAngle = 240;
  };
  datablock afxXM_AimData(AFX_Default_RevealLight_aim_XM)
  {
    aimZOnly = false;
  };
  
  datablock afxEffectWrapperData(AFX_Default_RevealLight_1_EW)
  {
    effect = AFX_Default_RevealLight_CE;
    posConstraint = selected;
    posConstraint2 = "selected.#center";
    lifetime = 0.7;
    fadeInTime = 0.50;
    fadeOutTime = 0.20;
    xfmModifiers[0] = AFX_Default_RevealLight_spin1_XM;
    xfmModifiers[1] = AFX_Default_RevealLight_offset_XM;
    xfmModifiers[2] = AFX_Default_RevealLight_aim_XM;
  };
  datablock afxEffectWrapperData(AFX_Default_RevealLight_2_EW : AFX_Default_RevealLight_1_EW)
  {
    xfmModifiers[0] = AFX_Default_RevealLight_spin2_XM;
  };
  datablock afxEffectWrapperData(AFX_Default_RevealLight_3_EW : AFX_Default_RevealLight_1_EW)
  {
    xfmModifiers[0] = AFX_Default_RevealLight_spin3_XM;
  };
}
else
{
  $isTGEA = ($thisEngine $= "TGEA");

  // main zode reveal light
  datablock afxLightData(AFX_Default_RevealLight_CE)
  {
    type = "Spot";
    radius = ($isTGEA) ? 8 : 4;    
    direction = "0 -0.313 0.95";
    sgCastsShadows = false;
    color = "2.5 2.5 2.5";
    lightIlluminationMask = $AFX::ILLUM_DTS | $AFX::ILLUM_DIF; // TGEA (ignored by TGE)
    sgLightingModelName = ($isTGEA) ? "Original Advanced" : "Near Linear";
  };

  datablock afxXM_LocalOffsetData(AFX_Default_RevealLight_offset_XM)
  {
    localOffset = "0 2 -4";
  };
  datablock afxXM_SpinData(AFX_Default_RevealLight_spin1_XM)
  {
    spinAxis = "0 0 1";
    spinAngle = 0;
    spinRate = -30;
  };
  datablock afxXM_SpinData(AFX_Default_RevealLight_spin2_XM : AFX_Default_RevealLight_spin1_XM)
  {
    spinAngle = 120;
  };
  datablock afxXM_SpinData(AFX_Default_RevealLight_spin3_XM : AFX_Default_RevealLight_spin1_XM)
  {
    spinAngle = 240;
  };

  datablock afxEffectWrapperData(AFX_Default_RevealLight_1_EW)
  {
    effect = AFX_Default_RevealLight_CE;
    posConstraint = selected;
    lifetime = 0.7;
    fadeInTime = 0.50;
    fadeOutTime = 0.20;
    xfmModifiers[0] = AFX_Default_RevealLight_spin1_XM;
    xfmModifiers[1] = AFX_Default_RevealLight_offset_XM;
  };
  datablock afxEffectWrapperData(AFX_Default_RevealLight_2_EW : AFX_Default_RevealLight_1_EW)
  {
    xfmModifiers[0] = AFX_Default_RevealLight_spin2_XM;
  };
  datablock afxEffectWrapperData(AFX_Default_RevealLight_3_EW : AFX_Default_RevealLight_1_EW)
  {
    xfmModifiers[0] = AFX_Default_RevealLight_spin3_XM;
  };
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// AFX DEFAULT Selectron 
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock afxSelectronData(AFX_Default_SELE)
{
  selectionTypeStyle = $AFX_Default_Style;
  selectionTypeMask = $TypeMasks::PlayerObjectType | $TypeMasks::CorpseObjectType | $TypeMasks::VehicleObjectType;

  mainDur = $AFX::INFINITE_TIME;

  addSelectEffect = AFX_Default_glow_Zode_EW;
  addSelectEffect = AFX_Default_glow2_Zode_EW;
  addSelectEffect = AFX_Default_RevealLight_1_EW;
  addSelectEffect = AFX_Default_RevealLight_2_EW;
  addSelectEffect = AFX_Default_RevealLight_3_EW;

  addMainEffect = AFX_Default_Zode1_EW;
  addMainEffect = AFX_Default_Zode2_EW;

  // sounds
  addSelectEffect = AFX_Default_Select_Snd_EW;
  addDeselectEffect = AFX_Default_Deselect_Snd_Ew;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

datablock afxEffectWrapperData(AFX_FreeTargeting_Zode1_EW)
{
  effect = AFX_Default_Zode1_CE;
  posConstraint = "freeTarget";
  fadeInTime = 0.20;
  fadeOutTime = 0.20;
  delay = 0.5;
  effectName = "MainZode";
  isConstraintSrc = true;
  xfmModifiers[0] = SHARED_AltitudeConform_XM;
};

datablock afxEffectWrapperData(AFX_FreeTargeting_Zode2_EW)
{
  effect = AFX_Default_Zode2_CE;
  posConstraint = "#effect.MainZode";
  borrowAltitudes = true;
  fadeInTime = 0.20;
  fadeOutTime = 0.20;
  delay = 0.5;
};

datablock afxSelectronData(AFX_FreeTargeting_SELE)
{
  selectionTypeStyle = 0;
  selectionTypeMask = 0;

  mainDur = $AFX::INFINITE_TIME;

  addMainEffect = AFX_FreeTargeting_Zode1_EW;
  addMainEffect = AFX_FreeTargeting_Zode2_EW;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

// Add styles to the demo's selectron manager. (This is only
// needed to allow selectron cycling using the 't' key.)
// 
addDemoSelectronStyle("AFX DEFAULT",  $AFX_Default_Style);

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
