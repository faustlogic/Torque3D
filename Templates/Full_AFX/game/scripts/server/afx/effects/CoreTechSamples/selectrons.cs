
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// SELECTRONS (Core Tech)
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

// style numbers
$AFX_Old_Style = 1;
$Like_WoW_Style = 2;
$Blue_Flower_Style = 3;
$Booming_Style = 4;
$SciFi_Style = 5;

%mySelectronDataPath = $afxSpellDataPath @ "/" @ $afxAutoloadScriptFolder @ "/SELE";

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// AFX OLD Selectron Style
//
//    This selectron style reproduces the plain red zodiac used for
//    AFX selections before selectrons.
//

datablock afxZodiacData(AFX_Old_Zode_CE : SHARED_SelectronZodiac_CE)
{  
  texture = %mySelectronDataPath @ "/afx_old/OLD_SELE_ring.png";
  radius = 1.5;
  startAngle = 0.0;
  rotationRate = 0.0;
  color = "1.0 0.0 0.0 0.5";
  blend = additive;
};
//
datablock afxEffectWrapperData(AFX_Old_Zode_EW)
{
  effect = AFX_Old_Zode_CE;
  posConstraint = selected;
  fadeInTime = 0.20;
  fadeOutTime = 0.20;
  xfmModifiers[0] = SHARED_AltitudeConform_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// AFX OLD Selectron
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock afxSelectronData(AFX_OldDefault_SELE)
{
  selectionTypeStyle = $AFX_Old_Style;
  selectionTypeMask = $TypeMasks::PlayerObjectType | $TypeMasks::CorpseObjectType | $TypeMasks::VehicleObjectType;
  mainDur = $AFX::INFINITE_TIME;
  addMainEffect = AFX_Old_Zode_EW;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// LIKE WOW Selectron Style
//
//    This selectron style reproduces some of the behavior of the 
//    selections found in World of Warcraft. While visually, they
//    only approximate the ones in WoW, they do implement type
//    specific coloring and continuous orientation of the zodiac
//    relative to the camera position.
//

$AIObjectType_mask = 0x2000000; // BIT(25);

// used to point wide part of crescent toward camera
datablock afxXM_AimData(Like_WoW_aim_XM)
{
  aimZOnly = true;
};

// LIKE WOW Red (for NPCs)

datablock afxZodiacData(Like_WoW_RED_Zode_CE : SHARED_SelectronZodiac_CE)
{  
  texture = %mySelectronDataPath @ "/like_wow/WOW_SELE_crescent.png";
  radius = 1.5;
  startAngle = 180.0;
  rotationRate = 0.0;
  color = "1.0 0.0 0.0 0.5";
  blend = additive;
  trackOrientConstraint = true;
};
datablock afxEffectWrapperData(Like_WoW_RED_Zode_EW)
{
  effect = Like_WoW_RED_Zode_CE;
  constraint = selected;
  fadeInTime = 0.15;
  fadeOutTime = 0.15;
  posConstraint2 = camera; // aim
  xfmModifiers[0] = Like_WoW_aim_XM;
  xfmModifiers[1] = SHARED_AltitudeConform_XM;
};


// LIKE WOW White (for corpses)

datablock afxZodiacData(Like_WoW_WHITE_Zode_CE : Like_WoW_RED_Zode_CE)
{  
  color = "1.0 1.0 1.0 0.4";
};
datablock afxEffectWrapperData(Like_WoW_WHITE_Zode_EW : Like_WoW_RED_Zode_EW)
{
  effect = Like_WoW_WHITE_Zode_CE;
};
//


// LIKE WOW Purple (for players)
datablock afxZodiacData(Like_WoW_PURPLE_Zode_CE : Like_WoW_RED_Zode_CE)
{  
  color = "0.57 0.15 0.56 0.5";
};
datablock afxEffectWrapperData(Like_WoW_PURPLE_Zode_EW : Like_WoW_RED_Zode_EW)
{
  effect = Like_WoW_PURPLE_Zode_CE;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// The LIKE WOW Selectrons
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

// Ordering is important here... try to match corpses first since corpses also
// match AIs. Then try AIs since they will also match Players. Try Players last
// after corpse and AI tests have failed. 
//
datablock afxSelectronData(Like_WoW_WHITE_SELE)
{
  selectionTypeStyle = $Like_WoW_Style;

  // match corpses only
  selectionTypeMask = $TypeMasks::CorpseObjectType;

  mainDur = $AFX::INFINITE_TIME;
  addMainEffect = Like_WoW_WHITE_Zode_EW;
};

datablock afxSelectronData(Like_WoW_RED_SELE)
{
  selectionTypeStyle = $Like_WoW_Style;

  // match living (or undead) NPCs only
  selectionTypeMask = $AIObjectType_mask | $TypeMasks::VehicleObjectType;

  mainDur = $AFX::INFINITE_TIME;
  addMainEffect = Like_WoW_RED_Zode_EW;
};

datablock afxSelectronData(Like_WoW_PURPLE_SELE)
{
  selectionTypeStyle = $Like_WoW_Style;

  // match Players only (not NPCs)
  selectionTypeMask = $TypeMasks::PlayerObjectType;

  mainDur = $AFX::INFINITE_TIME;
  addMainEffect = Like_WoW_PURPLE_Zode_EW;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// EXPLODING Selectron Style

datablock afxZodiacData(Booming_Stain_CE : SHARED_SelectronZodiac_CE)
{  
  texture = %mySelectronDataPath @ "/exploding/EXP_SELE_stain.png";
  radius = 3.5;
  startAngle = 0.0;
  rotationRate = 0.0;
  color = "1.0 1.0 1.0 0.9";
};
//
datablock afxEffectWrapperData(Booming_Stain_EW)
{
  effect = Booming_Stain_CE;
  constraint = "selected";
  fadeInTime = 1.0;
  fadeOutTime = 1.0;
  xfmModifiers[0] = SHARED_AltitudeConform_XM;
};

datablock ParticleData(Booming_ExplosionSmoke_P)
{
   textureName          = %mySelectronDataPath @ "/exploding/smoke";
   dragCoeffiecient     = 100.0;
   gravityCoefficient   = 0;
   inheritedVelFactor   = 0.25;
   constantAcceleration = -0.30;
   lifetimeMS           = 1200;
   lifetimeVarianceMS   = 300;
   useInvAlpha =  true;
   spinRandomMin = -80.0;
   spinRandomMax =  80.0;

   colors[0]     = "1.0 1.0 1.0 1.0";
   colors[1]     = "1.0 0.5 0.0 1.0";
   colors[2]     = "0.4 0.4 0.4 1.0";
   colors[3]     = "0.0 0.0 0.0 0.0";

   sizes[0]      = 4.0;
   sizes[1]      = 6.0;
   sizes[2]      = 8.0;
   sizes[3]      = 10.0;

   times[0]      = 0.0;
   times[1]      = 0.33;
   times[2]      = 0.66;
   times[3]      = 1.0;
};
//
datablock ParticleData(Booming_ExplosionFire_P)
{
   textureName          = %mySelectronDataPath @ "/exploding/fireExplosion";
   dragCoeffiecient     = 100.0;
   gravityCoefficient   = 0;
   inheritedVelFactor   = 0.25;
   constantAcceleration = 0.1;
   lifetimeMS           = 600; //1200;
   lifetimeVarianceMS   = 150; //300;
   useInvAlpha =  false;
   spinRandomMin        = -900.0;
   spinRandomMax        = 900.0;
   colors[0]            = "1.0 1.0 1.0 1.0";
   colors[1]            = "1.0 1.0 0.0 1.0";
   colors[2]            = "1.0 0.0 0.0 1.0";
   colors[3]            = "1.0 0.0 0.0 0.0";
   sizes[0]             = 3.0;
   sizes[1]             = 5.0;
   sizes[2]             = 7.0;//2.2;
   sizes[3]             = 3.0; //1.2;
   times[0]             = 0.0;
   times[1]             = 0.2;
   times[2]             = 0.7;
   times[3]             = 1.0;
};
//
datablock ParticleEmitterData(Booming_ExplosionFire_E)
{
   ejectionPeriodMS = 10;
   periodVarianceMS = 0;
   ejectionVelocity = 0.8;
   velocityVariance = 0.5;
   thetaMin         = 0.0;
   thetaMax         = 180.0;
   lifetimeMS       = 250;
   particles        = "Booming_ExplosionFire_P";
};
//
datablock ParticleEmitterData(Booming_ExplosionSmoke_E)
{
   ejectionPeriodMS = 5;
   periodVarianceMS = 0;
   ejectionVelocity = 10;
   velocityVariance = 1.5;
   thetaMin         = 0.0;
   thetaMax         = 180.0;
   lifetimeMS       = 250;
   particles        = "Booming_ExplosionSmoke_P";
};
//
datablock ExplosionData(Booming_Explosion_CE)
{
   lifeTimeMS = 1200;

   // Volume particles
   particleEmitter = Booming_ExplosionFire_E;
   particleDensity = 20; //50;
   particleRadius = 3;

   // Point emission
   emitter[0] = Booming_ExplosionSmoke_E;
   emitter[1] = Booming_ExplosionSmoke_E;

   // Impulse
   impulseRadius = 10;
   impulseForce = 15;
};
//
datablock afxEffectWrapperData(Booming_Explosion_EW)
{
  effect = Booming_Explosion_CE;
  posConstraint = "selected";
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// BOOMING Selectron
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock afxSelectronData(Booming_SELE)
{
  selectionTypeStyle = $Booming_Style;
  selectionTypeMask = $TypeMasks::PlayerObjectType | $TypeMasks::CorpseObjectType | $TypeMasks::VehicleObjectType;

  mainDur = $AFX::INFINITE_TIME;
  addMainEffect = Booming_Stain_EW;
  addSelectEffect = Booming_Explosion_EW;
  addDeselectEffect = Booming_Explosion_EW;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// BLUE FLOWER Selectron Style

// this mooring anchors the zodiacs and measures
// their altitudes

datablock afxMooringData(BlueFlower_FixedZodeMooring_CE)
{
  networking = $AFX::CLIENT_ONLY;
  displayAxisMarker = false;
};
datablock afxEffectWrapperData(BlueFlower_FixedZodeMooring_EW)
{
  effect = BlueFlower_FixedZodeMooring_CE;
  posConstraint = selected;
  effectName = "FixedZodeMooring";
  isConstraintSrc = true;
  lifetime = 4.0;
  xfmModifiers[0] = SHARED_AltitudeConform_XM;
};

datablock afxZodiacData(BlueFlower_glow_Zode_CE : SHARED_SelectronZodiac_CE)
{  
  texture = %mySelectronDataPath @ "/blue_flower/BLUE_FLOWER_glow.png";
  radius = 4.0-0.5;
  growthRate = 0.75;
  color = "1.0 1.0 1.0 1.0";
  blend = additive;
};
//
datablock afxEffectWrapperData(BlueFlower_glow_Zode_EW)
{
  effect = BlueFlower_glow_Zode_CE;
  posConstraint = "#effect.FixedZodeMooring";
  borrowAltitudes = true;
  lifetime = 2.0;
  fadeInTime = 1.5;
  fadeOutTime = 0.20;
  delay = 0.25;
};

datablock afxZodiacData(BlueFlower_glowring1_Zode_CE : SHARED_SelectronZodiac_CE)
{  
  texture = %mySelectronDataPath @ "/blue_flower/BLUE_FLOWER_glowring.png";
  radius = 2.0;
  startAngle = 0;
  rotationRate = 81.0;
  color = "1.0 1.0 1.0 1.0";
  blend = additive;
};
//
datablock afxEffectWrapperData(BlueFlower_glowring1_Zode_EW)
{
  effect = BlueFlower_glowring1_Zode_CE;
  posConstraint = "#effect.FixedZodeMooring";
  borrowAltitudes = true;
  lifetime = 2.0+0.35;
  fadeInTime = 1.0;
  fadeOutTime = 0.20;
  delay = 0;
};

datablock afxZodiacData(BlueFlower_glowring2_Zode_CE : BlueFlower_glowring1_Zode_CE)
{  
  startAngle = 180;
  rotationRate = -123;
};
//
datablock afxEffectWrapperData(BlueFlower_glowring2_Zode_EW : BlueFlower_glowring1_Zode_EW)
{
  effect = BlueFlower_glowring2_Zode_CE;
};

datablock afxZodiacData(BlueFlower_rays1_Zode_CE : SHARED_SelectronZodiac_CE)
{  
  texture = %mySelectronDataPath @ "/blue_flower/BLUE_FLOWER_rays.png";
  radius = 3.0;
  startAngle = 0;
  rotationRate = 60.0;
  color = "0.5 0.5 0.5 0.5";
  blend = additive;
};
//
datablock afxEffectWrapperData(BlueFlower_rays1_Zode_EW)
{
  effect = BlueFlower_rays1_Zode_CE;
  posConstraint = "#effect.FixedZodeMooring";
  borrowAltitudes = true;
  lifetime = 2.0;
  fadeInTime = 1.0;
  fadeOutTime = 0.4;
  delay = 0;
};

datablock afxZodiacData(BlueFlower_rays2_Zode_CE : BlueFlower_rays1_Zode_CE)
{  
  startAngle = 180;
  rotationRate = -60.0;
};
//
datablock afxEffectWrapperData(BlueFlower_rays2_Zode_EW : BlueFlower_rays1_Zode_EW)
{
  effect = BlueFlower_rays2_Zode_CE;
};

%BlueFlower_main_radius3 = 2.4;
%BlueFlower_main_radius2 = 3.2;
%BlueFlower_main_radius1 = 4.0;

datablock afxZodiacData(BlueFlower_main_Zode_CE : SHARED_SelectronZodiac_CE)
{  
  texture = %mySelectronDataPath @ "/blue_flower/BLUE_FLOWER_zode.png";
  radius = %BlueFlower_main_radius3;
  startAngle = 0;
  rotationRate = 180.0;
  color = "0.9 0.9 0.9 0.9";
  blend = additive;
};
//
datablock afxEffectWrapperData(BlueFlower_main_Zode_EW)
{
  effect = BlueFlower_main_Zode_CE;
  posConstraint = selected;
  fadeInTime = 0.2;
  fadeOutTime = 0.3;
  delay = 2.6;
  xfmModifiers[0] = SHARED_AltitudeConform_XM;
};

datablock afxZodiacData(BlueFlower_mainGlow_Zode_CE : SHARED_SelectronZodiac_CE)
{  
  texture = %mySelectronDataPath @ "/blue_flower/BLUE_FLOWER_zodeGlow.png";
  radius = 1.8;
  startAngle = 0;
  rotationRate = 180.0;
  growthRate = 8.0;
  color = "1.0 1.0 1.0 1.0";
  blend = additive;
};
//
datablock afxEffectWrapperData(BlueFlower_mainGlow_Zode_EW)
{
  effect = BlueFlower_mainGlow_Zode_CE;
  posConstraint = "#effect.FixedZodeMooring";
  borrowAltitudes = true;
  lifetime = 0.15;
  fadeInTime = 0.05;
  fadeOutTime = 0.2;
  delay = 2.05;
};

datablock afxZodiacData(BlueFlower_mainGlow1_Zode_CE : SHARED_SelectronZodiac_CE)
{  
  texture = %mySelectronDataPath @ "/blue_flower/BLUE_FLOWER_zodeGlow.png";
  radius = %BlueFlower_main_radius1;
  startAngle = 0;
  rotationRate = 0.0;
  color = "1.0 1.0 1.0 1.0";
  blend = additive;
};
//
datablock afxEffectWrapperData(BlueFlower_mainGlow1_Zode_EW)
{
  effect = BlueFlower_mainGlow1_Zode_CE;
  posConstraint = "#effect.FixedZodeMooring";
  borrowAltitudes = true;
  lifetime = 0.2;
  fadeInTime = 0.3;
  fadeOutTime = 0.25;
  delay = 2.1;
};

datablock afxZodiacData(BlueFlower_mainGlow2_Zode_CE : BlueFlower_mainGlow1_Zode_CE)
{
  radius = %BlueFlower_main_radius2;
  rotationRate = 60.0;
};
//
datablock afxEffectWrapperData(BlueFlower_mainGlow2_Zode_EW : BlueFlower_mainGlow1_Zode_EW)
{
  effect = BlueFlower_mainGlow2_Zode_CE;
  delay = 2.3;
};

datablock afxZodiacData(BlueFlower_mainGlow3_Zode_CE : BlueFlower_mainGlow1_Zode_CE)
{
  radius = %BlueFlower_main_radius3;
  rotationRate = 120.0;
};
//
datablock afxEffectWrapperData(BlueFlower_mainGlow3_Zode_EW : BlueFlower_mainGlow1_Zode_EW)
{
  effect = BlueFlower_mainGlow3_Zode_CE;
  delay = 2.5;
};


datablock afxZodiacData(BlueFlower_main1_Zode_CE : BlueFlower_mainGlow1_Zode_CE)
{  
  texture = %mySelectronDataPath @ "/blue_flower/BLUE_FLOWER_zode.png";
  color = "0.9 0.9 0.9 0.9";
};
//
datablock afxEffectWrapperData(BlueFlower_main1_Zode_EW)
{
  effect = BlueFlower_main1_Zode_CE;
  posConstraint = "#effect.FixedZodeMooring";
  borrowAltitudes = true;
  lifetime = 0.3;
  fadeInTime = 0.2;
  fadeOutTime = 0.3;
  delay = 2.1+0.2;
};

datablock afxZodiacData(BlueFlower_main2_Zode_CE : BlueFlower_mainGlow2_Zode_CE)
{  
  texture = %mySelectronDataPath @ "/blue_flower/BLUE_FLOWER_zode.png";
  color = "0.9 0.9 0.9 0.9";
};
//
datablock afxEffectWrapperData(BlueFlower_main2_Zode_EW)
{
  effect = BlueFlower_main2_Zode_CE;
  posConstraint = "#effect.FixedZodeMooring";
  borrowAltitudes = true;
  lifetime = 0.3;
  fadeInTime = 0.2;
  fadeOutTime = 0.3;
  delay = 2.3+0.2;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// BLUE FLOWER Selectron
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock afxSelectronData(BlueFlower_SELE)
{
  selectionTypeMask = $TypeMasks::PlayerObjectType | $TypeMasks::CorpseObjectType | $TypeMasks::VehicleObjectType;
  selectionTypeStyle = $Blue_Flower_Style;

  mainDur = $AFX::INFINITE_TIME;

  addSelectEffect = BlueFlower_FixedZodeMooring_EW;
  addSelectEffect = BlueFlower_glow_Zode_EW;
  addSelectEffect = BlueFlower_rays1_Zode_EW;
  addSelectEffect = BlueFlower_rays2_Zode_EW;
  addSelectEffect = BlueFlower_glowring1_Zode_EW;
  addSelectEffect = BlueFlower_glowring2_Zode_EW;
  addSelectEffect = BlueFlower_mainGlow1_Zode_EW;
  addSelectEffect = BlueFlower_mainGlow2_Zode_EW;
  addSelectEffect = BlueFlower_mainGlow3_Zode_EW;
  addSelectEffect = BlueFlower_main1_Zode_EW;
  addSelectEffect = BlueFlower_main2_Zode_EW;
  
  addMainEffect = BlueFlower_main_Zode_EW;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// SCI-FI Selectron

// this mooring anchors the zodiacs and measures
// their altitudes

datablock afxMooringData(SCIFI_FixedZodeMooring_CE)
{
  networking = $AFX::CLIENT_ONLY;
  displayAxisMarker = false;
};
datablock afxEffectWrapperData(SCIFI_FixedZodeMooring_EW)
{
  effect = SCIFI_FixedZodeMooring_CE;
  constraint = selected;
  effectName = "FixedZodeMooring";
  isConstraintSrc = true;
  xfmModifiers[0] = SHARED_AltitudeConform_XM;
};

// this mooring copies what FixedZodeMooring does and
// then orients toward the camera

datablock afxXM_AimData(SCIFI_SelectPointer_aim_XM)
{
  aimZOnly = true;
};
datablock afxMooringData(SCIFI_AimedZodeMooring_CE)
{
  networking = $AFX::CLIENT_ONLY;
  displayAxisMarker = false;
};
datablock afxEffectWrapperData(SCIFI_AimedZodeMooring_EW)
{
  effect = SCIFI_AimedZodeMooring_CE;
  constraint = "#effect.FixedZodeMooring";
  borrowAltitudes = true;
  posConstraint2 = camera;
  effectName = "AimedZodeMooring";
  isConstraintSrc = true;
  xfmModifiers[0] = SCIFI_SelectPointer_aim_XM;
};

datablock afxXM_ScaleData(SCIFI_SelectTarget_scale_XM)
{
  scale = "1.5";
  delay = 0.0;
  lifetime = 0.0;
  fadeOutTime = 0.5;
};
//
datablock afxZodiacData(SCIFI_SelectTarget_Zode_CE : SHARED_SelectronZodiac_CE)
{  
  texture = %mySelectronDataPath @ "/scifi/SCIFI_SELE_Zode_B1.png";
  radius = 1.8;
  startAngle = 0.0;
  rotationRate = 0.0;
  color = "1.0 1.0 1.0 1.0";
  blend = additive;
};
//
datablock afxEffectWrapperData(SCIFI_SelectTarget_Zode_EW)
{
  effect = SCIFI_SelectTarget_Zode_CE;
  posConstraint = "#effect.FixedZodeMooring";
  borrowAltitudes = true;
  lifetime = 0.5;
  fadeInTime = 0.40;
  fadeOutTime = 0;
  delay = 0;
  xfmModifiers[0] = SCIFI_SelectTarget_scale_XM;
};

datablock afxZodiacData(SCIFI_SelectTargetSpin_Zode_CE : SCIFI_SelectTarget_Zode_CE)
{  
  startAngle = 0.0;
  rotationRate = 180.0;
};
//
datablock afxEffectWrapperData(SCIFI_SelectTargetSpin_Zode_EW)
{
  effect = SCIFI_SelectTargetSpin_Zode_CE;
  posConstraint = "#effect.FixedZodeMooring";
  borrowAltitudes = true;
  fadeInTime = 0;
  fadeOutTime = 0.20;
  delay = 0.5;
};

datablock afxZodiacData(SCIFI_SelectPointer1_Zode_CE : SHARED_SelectronZodiac_CE)
{  
  texture = %mySelectronDataPath @ "/scifi/SCIFI_SELE_Zode_B2.png";
  radius = 6.0*0.5;
  startAngle = 180.0;
  rotationRate = 0.0;
  color = "1.0 1.0 1.0 1.0";
  blend = additive;
  trackOrientConstraint = true;
};
//
datablock afxEffectWrapperData(SCIFI_SelectPointer1_Zode_EW)
{
  effect = SCIFI_SelectPointer1_Zode_CE;
  constraint = "#effect.AimedZodeMooring";
  borrowAltitudes = true;
  fadeInTime = 0.1;
  fadeOutTime = 0.2;
  delay = 1.0;
};

datablock afxZodiacData(SCIFI_SelectPointer2_Zode_CE : SCIFI_SelectPointer1_Zode_CE)
{  
  texture = %mySelectronDataPath @ "/scifi/SCIFI_SELE_Zode_B3.png";
  radius = 6.0;
};
//
datablock afxEffectWrapperData(SCIFI_SelectPointer2_Zode_EW : SCIFI_SelectPointer1_Zode_EW)
{
  effect = SCIFI_SelectPointer2_Zode_CE;
  delay = 1.2;
};

datablock afxZodiacData(SCIFI_SelectPointer3_Zode_CE : SCIFI_SelectPointer1_Zode_CE)
{  
  texture = %mySelectronDataPath @ "/scifi/SCIFI_SELE_Zode_B4.png";
  radius = 6.0;
};
//
datablock afxEffectWrapperData(SCIFI_SelectPointer3_Zode_EW : SCIFI_SelectPointer1_Zode_EW)
{
  effect = SCIFI_SelectPointer3_Zode_CE;
  delay = 1.4;
};

datablock afxZodiacData(SCIFI_SelectPointer4A_Zode_CE : SCIFI_SelectPointer1_Zode_CE)
{  
  texture = %mySelectronDataPath @ "/scifi/SCIFI_SELE_Zode_B5.png";
  radius = 6.0;
};
//
datablock afxEffectWrapperData(SCIFI_SelectPointer4A_Zode_EW : SCIFI_SelectPointer1_Zode_EW)
{
  effect = SCIFI_SelectPointer4A_Zode_CE;
  delay = 1.6;
  fadeOutTime = 0.1;
  lifetime = 0.4;
};


datablock afxEffectWrapperData(SCIFI_SelectPointer4B_Zode_EW : SCIFI_SelectPointer4A_Zode_EW)
{
  delay = 2.2;
};

datablock afxEffectWrapperData(SCIFI_SelectPointer4C_Zode_EW : SCIFI_SelectPointer4A_Zode_EW)
{
  delay = 2.8;
  lifetime = $AFX::INFINITE_TIME;
};

datablock afxZodiacData(SCIFI_SelectPointer5_Zode_CE : SHARED_SelectronZodiac_CE)
{  
  texture = %mySelectronDataPath @ "/scifi/SCIFI_SELE_Zode_B6.png";
  radius = 6.0*0.5;
  startAngle = 180.0;
  rotationRate = -120.0;
  color = "1.0 1.0 1.0 1.0";
  blend = additive;
  trackOrientConstraint = true;
};
//
datablock afxEffectWrapperData(SCIFI_SelectPointer5_Zode_EW)
{
  effect = SCIFI_SelectPointer5_Zode_CE;
  constraint = "#effect.AimedZodeMooring";
  borrowAltitudes = true;
  fadeInTime = 0.1;
  fadeOutTime = 0.2;
  delay = 1.4;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// SCI-FI Selectron
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock afxSelectronData(SCIFI_SELE)
{
  selectionTypeMask = $TypeMasks::PlayerObjectType | $TypeMasks::CorpseObjectType | $TypeMasks::VehicleObjectType;
  selectionTypeStyle = $SciFi_Style;

  mainDur = $AFX::INFINITE_TIME;

  addMainEffect = SCIFI_FixedZodeMooring_EW;
  addMainEffect = SCIFI_AimedZodeMooring_EW;

  addMainEffect = SCIFI_SelectTarget_Zode_EW;
  addMainEffect = SCIFI_SelectTargetSpin_Zode_EW;
  addMainEffect = SCIFI_SelectPointer1_Zode_EW;
  addMainEffect = SCIFI_SelectPointer2_Zode_EW;
  addMainEffect = SCIFI_SelectPointer3_Zode_EW;
  addMainEffect = SCIFI_SelectPointer4A_Zode_EW;
  addMainEffect = SCIFI_SelectPointer4B_Zode_EW;
  addMainEffect = SCIFI_SelectPointer4C_Zode_EW;
  addMainEffect = SCIFI_SelectPointer5_Zode_EW;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

// Add styles to the demo's selectron manager. (This is only
// needed to allow selectron cycling using the 't' key.)
// 
addDemoSelectronStyle("AFX OLD",      $AFX_Old_Style);
addDemoSelectronStyle("LIKE WOW",     $Like_WoW_Style);
addDemoSelectronStyle("BLUE FLOWER",  $Blue_Flower_Style);
addDemoSelectronStyle("EXPLODING",    $Booming_Style);
addDemoSelectronStyle("SCI-FI",       $SciFi_Style);

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
