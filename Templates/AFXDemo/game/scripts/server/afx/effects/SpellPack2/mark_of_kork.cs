
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// MARK OF KORK SPELL
//  
//  Kork brands the terrain with some taunting orcish graffiti. Brought to you
//  by The Letter K.
//
// This spell also implements effect variations used by the Soul Miner's Slaughter
// spell.
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

//
// Here we test if the script is being reloaded or if this is the
// first time the script has executed this mission.
//
$spell_reload = isObject(MarkOfKorkSpell);
if ($spell_reload)
{
  // mark datablocks so we can detect which are reloaded this script
  markDataBlocks();
  // reset data path from previously saved value
  %mySpellDataPath = MarkOfKorkSpell.spellDataPath;
}
else
{
  // set data path from default plus containing folder name
  %mySpellDataPath = $afxSpellDataPath @ "/" @ $afxAutoloadScriptFolder;

  // Mark of Kork and Soul Miner's Slaughter use elements from these other scripts.
  afxExecPrerequisite("sp2_shared_fx.cs", $afxAutoloadScriptFolder);
  afxExecPrerequisite("clusters_of_fire.cs", $afxAutoloadScriptFolder);
  afxExecPrerequisite("soul_nuke.cs", $afxAutoloadScriptFolder);
  afxExecPrerequisite("the_letter_k.cs", $afxAutoloadScriptFolder);
  afxExecPrerequisite("dread_bull_portent.cs", $afxAutoloadScriptFolder);
  afxExecPrerequisite("wandering_wisps.cs", $afxAutoloadScriptFolder);
  afxExecPrerequisite("soul_mine_solo.cs", $afxAutoloadScriptFolder);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// GLOBALS

$MoK_DisplayMoorings = false;

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// SPELLCASTER ANIMATION

// This clip: something appears in the spellcaster's hand
// startling him somewhat. He procedes to grind whatever it
// is into the ground.

datablock afxAnimClipData(MoK_Casting_Clip_CE)
{
  clipName = "mok";
  ignoreCorpse = true;
  rate = 1.0;
  lockAnimation = true;
};
datablock afxEffectWrapperData(MoK_Casting_Clip_EW)
{
  effect = MoK_Casting_Clip_CE;
  constraint = caster;
  lifetime = 240/30;
};

/* not used currently
datablock afxAnimLockData(MoK_AnimLock_CE)
{
  priority = 0;
};
datablock afxEffectWrapperData(MoK_AnimLock_EW)
{
  effect = MoK_AnimLock_CE;
  constraint = caster;
  lifetime = 120/30;
};
*/

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// MOORINGS

//
//  - moorings frozen to negate caster rotation 
//

datablock afxXM_FreezeData(MoK_freeze_XM)
{
  mask = $afxXfmMod::ALL;
};

datablock afxXM_GroundConformData(MoK_Mooring_ground_XM)
{
  height = 0;
};

// Client-side Mooring
datablock afxMooringData(MoK_Mooring_CE)
{
  networking = $AFX::CLIENT_ONLY;
  displayAxisMarker = $MoK_DisplayMoorings;
};

// Server-side Mooring (ghosted to clients)
datablock afxMooringData(MoK_Mooring_Server_CE)
{
  networking = $AFX::GHOSTABLE;
  displayAxisMarker = $MoK_DisplayMoorings;
};

// Caster Left Hand Mooring (Client)
//  -- Wrapper delay causes the mooring to appear when the orc's hand
//      touches the ground, which is the position we want to capture.
datablock afxEffectWrapperData(MoK_HandMooring_EW) // MoK and SMS
{
  effect = MoK_Mooring_CE;
  constraint = "caster.Bip01 L Hand";
  effectName = "Hand_Mooring";
  isConstraintSrc = true;
  delay = (120/30);
  lifetime = 40;
  xfmModifiers[0] = MoK_freeze_XM;
  xfmModifiers[1] = MoK_Mooring_ground_XM;
};

// Caster Mooring (Client)
datablock afxEffectWrapperData(MoK_CasterMooring_EW)
{
  effect = MoK_Mooring_CE;
  constraint = caster;
  effectName = "Caster_Mooring";
  isConstraintSrc = true;
  delay = (120/30);
  lifetime = 40;
  xfmModifiers[0] = MoK_freeze_XM;
  xfmModifiers[1] = MoK_Mooring_ground_XM;
};

// Caster Mooring (Server)
datablock afxEffectWrapperData(MoK_CasterMooringServer_EW)
{
  effect = MoK_Mooring_Server_CE;
  constraint = caster;
  effectName = "Caster_Mooring_Server";
  isConstraintSrc = true;
  ghostIsConstraintSrc = true;
  delay = (120/30);
  lifetime = 60;
  xfmModifiers[0] = MoK_freeze_XM;
  xfmModifiers[1] = MoK_Mooring_ground_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// CASTING HAND MAGIC

//
// -- intensified magic appears when hand touches ground
// -- colored light + short flash light with flare
//

datablock ParticleData(MoK_HandMagic_SM_P)
{
  textureName          = "$$ \"" @ %mySpellDataPath @ "/MoK/particles/\" @ %%._flav @ _handMagic";
  dragCoeffiecient     = 0.5;
  inheritedVelFactor   = 0.00;
  lifetimeMS           = 500;
  lifetimeVarianceMS   = 120;
  spinRandomMin        = -300;
  spinRandomMax        = 300;
  colors[0]            = "0.5 0.5 0.5 1.0";
    colors[1]            = "1.0 1.0 1.0 1.0";
    colors[2]            = "0.0 0.0 0.0 0.0";
  sizeBias             = 1.0;
  sizes[0]             = (1.0*0.1);
    sizes[1]             = (0.5*0.1); 
    sizes[2]             = (0.1*0.1);
  times[0]             = 0.0;
    times[1]             = 0.5;
    times[2]             = 1.0;
};
datablock ParticleData(MoK_HandMagic_MED_P : MoK_HandMagic_SM_P)
{
  sizeBias             = 2.0;
};
datablock ParticleData(MoK_HandMagic_LG_P : MoK_HandMagic_SM_P)
{
  sizeBias             = 6.0;
};

datablock afxParticleEmitterDiscData(MoK_HandMagic_E)
{
  ejectionPeriodMS      = 5;
  periodVarianceMS      = 2;
  ejectionVelocity      = 1.3;
  velocityVariance      = 0.3;
  particles             = "MoK_HandMagic_SM_P MoK_HandMagic_MED_P MoK_HandMagic_LG_P";
  vector                = "0 0 1";
  vectorIsWorld         = true;
  radiusMin             = 0.0;
  radiusMax             = 0.15;
  blendStyle            = "additive";
};

datablock afxParticleEmitterDiscData(MoK_HandMagic_Intense_E : MoK_HandMagic_E)
{
  ejectionVelocity      = 2.5*2;
  velocityVariance      = 1.0*2;
};

//~~~~~~~~~~~~~~~~~~~~//

datablock afxXM_LocalOffsetData(MoK_Hand_Palm_offset_XM)
{
  localOffset = "0.2 0.03 0.0";  // approximate above-palm position
};
datablock afxEffectWrapperData(MoK_HandMagic_lfhand_EW) // MoK and SMS
{
  effect = MoK_HandMagic_E;
  // Using a constraint allows the localOffset to work correctly, but
  //  also cause the emitter to exist in a new rotational space, which is a 
  //  problem...  (vectorIsWorld fix)
  constraint = "caster.Bip01 L Hand";
  delay = 40/30;
  lifetime = (220-40)/30 -0.2;
  fadeInTime = 0.15;
  fadeOutTime = 0.2;
  xfmModifiers[0] = MoK_Hand_Palm_offset_XM;
};

datablock afxEffectWrapperData(MoK_HandMagic_Intense_lfhand_EW) // MoK and SMS
{
  effect = MoK_HandMagic_Intense_E;
  constraint = "caster.Bip01 L Hand";
  delay = 120/30;
  lifetime = (210-120)/30 -0.2;
  fadeInTime = 0.0;
  fadeOutTime = 0.2;
  xfmModifiers[0] = MoK_Hand_Palm_offset_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// SPINNING MARK FLARE ZODES

//
// -- spinning zodiacs where hand presses ground
//

datablock afxZodiacData(MoK_MarkFlareZodeA_CE : SHARED_ZodiacBase_CE)
{  
  texture = "$$ \"" @ %mySpellDataPath @ "/MoK/zodiacs/\" @ %%._flav @ _markFlareA";
  radius = 4.0;
  startAngle = 0.0;
  rotationRate = 300.0;
  color = "1.0 1.0 1.0 1.0";
  blend = additive;
  growInTime = 0.75;
};
datablock afxZodiacData(MoK_MarkFlareZodeB_CE : MoK_MarkFlareZodeA_CE)
{
  texture = "$$ \"" @ %mySpellDataPath @ "/MoK/zodiacs/\" @ %%._flav @ _markFlareB";
  rotationRate = -300.0;
};

datablock afxEffectWrapperData(MoK_MarkFlareZodeA_EW) // MoK and SMS
{
  effect = MoK_MarkFlareZodeA_CE;
  constraint = "caster.Bip01 L Hand";
  delay = 120/30;
  fadeInTime = 0.20;
  fadeOutTime = 0.20;
  lifetime = (90/30)-0.20;
};
datablock afxEffectWrapperData(MoK_MarkFlareZodeB_EW : MoK_MarkFlareZodeA_EW) // MoK and SMS
{
  effect = MoK_MarkFlareZodeB_CE;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// CASTING HAND BEAM

//
// -- orc beams appear where orc's hand touches the ground
//

datablock afxModelData(MoK_OrcBeams_CE)
{
  shapeFile = "$$ \"" @ %mySpellDataPath @ "/MoK/models/\" @ %%._flav @ \"_orcBeams.dts\"";
  sequence = "beam";
  sequenceRate = 1.0;
  forceOnMaterialFlags =  $MaterialFlags::Additive;
  useVertexAlpha = true;
  textureFiltering = false; // otherwise, even at close-range, filtering makes these beams all blurry...
};
//
datablock afxXM_LocalOffsetData(MoK_OrcBeams_offset_XM)
{
  localOffset = "-0.2 0 0";
};
//
datablock afxXM_SpinData(MoK_OrcBeams_spin_XM)
{
  spinAxis = "0 0 1";
  spinRate = 300;
};
//
datablock afxEffectWrapperData(MoK_OrcBeams_EW)
{
  effect = MoK_OrcBeams_CE;
  posConstraint = "#effect.Hand_Mooring"; // no orient...
  scaleFactor = 40.0;
  delay = 120/30;
  fadeInTime = 0.20;
  fadeOutTime = 0.20;
  lifetime = (90/30)-0.20;
  xfmModifiers[1] = MoK_OrcBeams_spin_XM;
  xfmModifiers[0] = MoK_OrcBeams_offset_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock ParticleData(MoK_OrcBeamSparksA_P)
{
  textureName          = %mySpellDataPath @ "/MoK/particles/mok_sparks";
  dragCoeffiecient     = 0;
  gravityCoefficient   = 2;
  windCoefficient      = 0;
  inheritedVelFactor   = 0.00;
  lifetimeMS           = 800;
  lifetimeVarianceMS   = 250;
  useInvAlpha          = false;
  spinRandomMin        = 0.0;
  spinRandomMax        = 0.0;
  colors[0]            = "1 1 0.9 1";
    colors[1]            = "1 1 0.9 1";
    colors[2]            = "1 1 0.9 1";
    colors[3]            = "1 1 0.9 0";
  sizeBias             = 2.5;
  sizes[0]             = 0.50; 
    sizes[1]             = 0.50;
    sizes[2]             = 0.40;
    sizes[3]             = 0.30;
  times[0]             = 0.0;
    times[1]             = 0.3;
    times[2]             = 0.7;
    times[3]             = 1.0;
};
datablock ParticleData(MoK_OrcBeamSparksB_P : MoK_OrcBeamSparksA_P)
{
  sizeBias = 1.5;
};
datablock ParticleData(MoK_OrcBeamSparksC_P : MoK_OrcBeamSparksA_P)
{
  sizeBias = 0.5;
};

datablock afxParticleEmitterConeData(MoK_OrcBeamSparks_E)
{
  ejectionPeriodMS      = 10;
  periodVarianceMS      = 3;
  ejectionVelocity      = 15*0.5;
  velocityVariance      = 10*0.5;
  particles             = "MoK_OrcBeamSparksA_P MoK_OrcBeamSparksB_P MoK_OrcBeamSparksC_P";
  vector                = "0 0 1"; 
  spreadMin             = 60;
  spreadMax             = 135;
  blendStyle            = "ADDITIIVE"; 
};
datablock afxEffectWrapperData(MoK_OrcBeamSparks_EW)
{
  effectEnabled = "$$ %%._sparks";
  effect = MoK_OrcBeamSparks_E;
  posConstraint = "#effect.Hand_Mooring"; // no orient...
  delay = 120/30;
  fadeInTime = 0.20;
  fadeOutTime = 0.20;
  lifetime = (90/30)-0.20;
  xfmModifiers[0] = MoK_OrcBeams_offset_XM;
};

datablock afxParticleEmitterConeData(MoK_OrcBeamSparks2_E : MoK_OrcBeamSparks_E)
{
  ejectionPeriodMS      = 3; 
  periodVarianceMS      = 1; 
  ejectionVelocity      = 15*0.75;
  velocityVariance      = 10*0.75;
};
datablock afxEffectWrapperData(MoK_OrcBeamSparks2_EW : MoK_OrcBeamSparks_EW)
{
  effectEnabled = "$$ %%._sparks";
  effect = MoK_OrcBeamSparks2_E;
  delay = 180/30;
  fadeInTime = 0;
  fadeOutTime = 0;
  lifetime = (30/30)-0.2;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// GIANT HAND BEAM
//
//  This is a glowing radiance effect in the shape of a hand
//  outline. Since it is currently done with an animated dts model
//  it does not work well on uneven terrain. A procedural version
//  of the same effect, but constructed from a path outline of the
//  hand and conformed to the ground would be an excellent
//  improvement.
//

datablock afxModelData(MoK_HandBeams_CE)
{
  shapeFile = "$$ \""  @  %mySpellDataPath  @  "/MoK/models/\" @ %%._flav @ \"_handBeams.dts\"";
  sequence = "beam";
  sequenceRate = 1.0;
  forceOnMaterialFlags = $MaterialFlags::Additive | $MaterialFlags::SelfIlluminating;
  useVertexAlpha = true;
  textureFiltering = false; // otherwise, even at close-range, filtering makes these beams all blurry...
};

datablock afxXM_LocalOffsetData(MoK_HandBeams_offset_XM)
{
  localOffset = "0.25 2.50 0";
};

datablock afxXM_SpinData(MoK_HandBeams_spin_XM)
{
  spinAxis = "0 0 1";
  spinRate = 0;
  spinAngle = 8;
};

datablock afxEffectWrapperData(MoK_HandBeams_EW)
{
  effect = MoK_HandBeams_CE;
  constraint = "#effect.Caster_Mooring";
  scaleFactor = 45.5;
  delay = 4.75;
  fadeInTime = 0.20;
  fadeOutTime = 0.20;
  lifetime = (60/30)-0.20;
  xfmModifiers[0] = MoK_HandBeams_offset_XM;
  xfmModifiers[1] = MoK_HandBeams_spin_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// GIANT HAND GLOW ZODES

//
// -- pulsating, glowing hand zode accomplished with four separate zodes
//

datablock afxZodiacData(MoK_HandGlowZode_VIS_CE : SHARED_ZodiacBase_CE)
{  
  texture = "$$ \"" @ %mySpellDataPath @ "/MoK/zodiacs/\" @ %%._flav @ _handGlow";
  radius = 6.2;
  startAngle = 10;
  rotationRate = 0.0;
  color = "$$ %%._sms ? \"1 1 1 1\" : \"1 1 1 0.5\"";
  blend = additive;
  trackOrientConstraint = true;
};
datablock afxZodiacData(MoK_HandGlowZodeOver_VIS_CE : MoK_HandGlowZode_VIS_CE)
{  
  color = "1 1 1 1";
};

//~~~~~~~~~~~~~~~~~~~~//

datablock afxXM_LocalOffsetData(MoK_BurnZode_offset_XM)
{
  localOffset = "0.25 2.7 0";
};

//~~~~~~~~~~~~~~~~~~~~//

datablock afxEffectWrapperData(MoK_HandGlowZode_VIS_EW)
{
  effect = MoK_HandGlowZode_VIS_CE;
  constraint = "#effect.Caster_Mooring";
  xfmModifiers[0] = MoK_BurnZode_offset_XM;
  delay = 5;
  lifetime = 8.0;
  fadeInTime = 0;
  fadeOutTime = 0;
  visibilityKeys = "0:0  1:1 2:1  2.75:0.48  3.5:0.7 4:0.7  4.81:0.28  5.5:0.5  6.31:0.25 6.7:0.28 7.7:0";
};
datablock afxEffectWrapperData(MoK_HandGlowZodeOver_VIS_EW : MoK_HandGlowZode_VIS_EW)
{
  effectEnabled = "$$ !%%._sms";
  effect = MoK_HandGlowZodeOver_VIS_CE;  
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// GIANT HAND BURN ZODE

//
// -- burn mark left by glowing hand zodes
//

datablock afxZodiacData(MoK_BurnZode_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/MoK/zodiacs/mok_handBurn";
  radius = 6.2;
  startAngle = 10;
  rotationRate = 0.0;
  color = "0 0 0 1.0";
  blend = normal;
  trackOrientConstraint = true;
};

datablock afxEffectWrapperData(MoK_BurnZode_EW)
{
  effect = MoK_BurnZode_CE;
  constraint = "#effect.Caster_Mooring";
  delay = (120/30)+(15/30)+(15/30);
  fadeInTime = 1.0;
  fadeOutTime = 2.0;
  lifetime = 10.0;
  xfmModifiers[0] = MoK_BurnZode_offset_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// BURNING GROUND ZODE

%MoK_BurningGround_delay = (120/30)+2.4;
$MoK_BurningGround_delay = (120/30)+2.4;
datablock afxZodiacData(MoK_BurningGroundZode_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/MoK/zodiacs/mok_groundBurning";
  radius = 16;
  startAngle = 0;
  rotationRate = 0;
  color = "1 1 1 1";
  blend = additive;
  trackOrientConstraint = true;
};
//
datablock afxEffectWrapperData(MoK_BurningGroundZode_EW)
{
  effect = MoK_BurningGroundZode_CE;
  constraint = "#effect.Hand_Mooring";

  delay = %MoK_BurningGround_delay; //+0.9;
  fadeInTime = 1.0;
  fadeOutTime = 0.5;
  lifetime = 1.0;

  xfmModifiers[0] = MoK_BurnZode_offset_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// BURNT GROUND ZODES

datablock afxZodiacData(MoK_BurntGroundZodeA_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/MoK/zodiacs/mok_groundCharred";
  radius = 20;
  startAngle = 0;
  rotationRate = 0;
  color = "0 0 0 1.0";
  blend = normal; //subractive;
};
datablock afxZodiacData(MoK_BurntGroundZodeB_CE : MoK_BurntGroundZodeA_CE)
{  
  radius = 24;
  startAngle = 212;
};
datablock afxEffectWrapperData(MoK_BurntGroundZodeA_EW)
{
  effect = MoK_BurntGroundZodeA_CE;
  constraint = "#effect.Hand_Mooring";
  delay = (120/30)+2.8; //2.0;
  fadeInTime = 1.0;
  lifetime = 1.0;
  fadeOutTime = 5.0;
  residueLifetime = 20.0;

  xfmModifiers[0] = MoK_BurnZode_offset_XM;
};
datablock afxEffectWrapperData(MoK_BurntGroundZodeB_EW : MoK_BurntGroundZodeA_EW)
{
  effect = MoK_BurntGroundZodeB_CE;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// SIMMERING GROUND ZODES

datablock afxZodiacData(MoK_SimmeringGroundZode_123_CE : SHARED_ZodiacBase_CE)
{  
  texture = "$$ \"" @ %mySpellDataPath @ "/MoK/zodiacs/mok_groundSimmer_\" @ mFloor(1 + ##/10)";
  radius = 16;
  startAngle = 0;
  rotationRate = 0;
  color = "1 1 1 1";
  blend = additive;
  trackOrientConstraint = true;
};

datablock afxEffectWrapperData(MoK_SimmeringGroundZode_00_EW)
{
  effect = MoK_SimmeringGroundZode_123_CE;
  constraint = "#effect.Hand_Mooring";
  delay = "$$ 7 + getRandomF(-1.0,1.0) + (## % 10)*1.3 + getRandomF(-1.0,1.0)";
  lifetime    = 1;
  fadeInTime  = 0.3;
  fadeOutTime  = 0.3;
  lifetimeBias = "$$ getRandomF(0.85, 2.2)";
  xfmModifiers[0] = MoK_BurnZode_offset_XM;
};
datablock afxEffectGroupData(Mok_SimmeringGroundZode_EG)
{
  assignIndices = true;
  count = 30;
  addEffect = MoK_SimmeringGroundZode_00_EW;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// SYMBOL GLOW ZODES -- MAIN SPELL

//
// -- glow version: border glow, short
// -- hot version: quick burn-in zode
//


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// GROUND FIRES

datablock afxScriptEventData(MoK_CueGroundFires_CE)
{
  methodName = "CueGroundFires";   // name of method in afxMagicSpellData subclass
};
datablock afxEffectWrapperData(MoK_CueGroundFires_EW)
{
  effect = MoK_CueGroundFires_CE;
  constraint = "#effect.Caster_Mooring_Server";
  delay = %MoK_BurningGround_delay;
};

$MoK_Fire_area_x = 10.0;
$MoK_Fire_area_y = 10.0;
$MoK_Fire_areacaster_x_min = -3.0;
$MoK_Fire_areacaster_x_max =  3.0;
$MoK_Fire_areacaster_y_min =  0.0;
$MoK_Fire_areacaster_y_max =  5.2;

function MoK_calcFirePos(%count, %i, %data)
{
  // assign quadrants
  if (%i < 12)
    %quadrant = (%i % 4)+1;
  else
    %quadrant = getRandom(1,4);

  %x_dim = 10; %y_dim = 10;

  switch (%quadrant)
  {
  case 1:
    %min_x = 0;
    %max_x = %x_dim;
    %min_y = 0;
    %max_y = %y_dim;
  case 2:
    %min_x = -%x_dim;
    %max_x = 0;
    %min_y = 0;
    %max_y = %y_dim;
  case 3:
    %min_x = -%x_dim;
    %max_x = 0;
    %min_y = -%y_dim;
    %max_y = 0;
  case 4:
    %min_x = 0;
    %max_x = %x_dim;
    %min_y = -%y_dim;
    %max_y = 0;
  }

  while (true)
  {
    %pos_x = getRandomF(%min_x, %max_x);
    %pos_y = getRandomF(%min_y, %max_y);

    if ( ( !((%pos_x <  3.0) && (%pos_y < 5.2)) ) ||
         ( !((%pos_x > -3.0) && (%pos_y < 5.2)) ) ||
         ( !((%pos_x > -3.0) && (%pos_y > 0.0)) ) ||
         ( !((%pos_x <  3.0) && (%pos_y > 0.0)) ) )
    {
      break;
    }
  }

  return (mFloatLength(%pos_x,1) SPC mFloatLength(%pos_y,1) SPC "0");
}

datablock afxXM_LocalOffsetData(MoK_K_offset_XM)
{
  localOffset = "-0.15 1.8 0.0";
};
datablock afxScriptEventData(MoK_CueKSymbol_CE)
{
  methodName = "CueKSymbol";   // name of method in afxMagicSpellData subclass
};
datablock afxEffectWrapperData(MoK_CueKSymbol_EW)
{
  effectEnabled = "$$ !%%._sms";
  effect = MoK_CueKSymbol_CE;
  constraint = "#effect.Caster_Mooring_Server";
  delay = 5.0;
  xfmModifiers[0] = MoK_K_offset_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// SYMBOL-LAUNCHING SCRIPTS

//
// -- Launches effectron that creates the symbol 
// -- symbols are currently mine generators
//

datablock afxScriptEventData(SMS_CueWisps_CE)
{
  methodName = "CueWisps";   // name of method in afxMagicSpellData subclass
};

%SMS_CueDreadBull_time = (120/30)+(15/30)+(15/30);
%SMS_CueWisps_time = %SMS_CueDreadBull_time + 6; 

datablock afxXM_LocalOffsetData(SMS_Wisps_offset_XM)
{
  localOffset = "-0.4 1.2 0.0";
  localOffset = "-0.3 1.3 0.0";
};
datablock afxEffectWrapperData(SMS_CueWisps_EW)
{
  effectEnabled = "$$ %%._sms";
  effect = SMS_CueWisps_CE;
  constraint = "#effect.Caster_Mooring_Server";
  delay = %SMS_CueWisps_time;
  xfmModifiers[0] = SMS_Wisps_offset_XM;
};

datablock afxXM_LocalOffsetData(SMS_DreadBull_offset_XM)
{
  localOffset = "-0.3 1.3 0.0";
};
datablock afxScriptEventData(SMS_CueDreadBull_CE)
{
  methodName = "CueDreadBull";   // name of method in afxMagicSpellData subclass
};
datablock afxEffectWrapperData(SMS_CueDreadBull_EW)
{
  effectEnabled = "$$ %%._sms";
  effect = SMS_CueDreadBull_CE;
  constraint = "#effect.Caster_Mooring_Server";
  delay = %SMS_CueDreadBull_time;
  xfmModifiers[0] = SMS_DreadBull_offset_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// HANDPRINT EFFECTS

datablock afxEffectGroupData(MoK_HandPrintFX_EG)
{
  groupEnabled = "$$ %%._hand == true";
  addEffect = MoK_HandGlowZode_VIS_EW;
  addEffect = MoK_BurnZode_EW;
  addEffect = MoK_HandGlowZodeOver_VIS_EW;
  addEffect = MoK_HandBeams_EW;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// AUDIO AND LIGHTING

switch$ (afxGetEngine())
{
  case "TGE":
    exec("./lighting/mok_lighting_tge_sub.cs");
    exec("./audio/mok_audio_sub.cs");
  case "TGEA":
    exec("./lighting/mok_lighting_tgea_sub.cs");
    exec("./audio/mok_audio_sub.cs");
 case "T3D":
    exec("./lighting/mok_lighting_t3d_sub.cs");
    exec("./audio/mok_audio_sub.cs");
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// MARK OF KORK SPELL
//

datablock afxMagicSpellData(MarkOfKorkSpell)
{
  echoPacketUsage = 20;
  castingDur = "$$ %%._castDur";
  
    // spellcaster animation //
  addCastingEffect = MoK_Casting_Clip_EW;
  //addCastingEffect = MoK_AnimLock_EW;
    // moorings //
  addCastingEffect = MoK_HandMooring_EW;
  addCastingEffect = MoK_CasterMooring_EW;
  addCastingEffect = MoK_CasterMooringServer_EW;
    // casting hand magic //
  addCastingEffect = MoK_HandMagic_lfhand_EW;
  addCastingEffect = MoK_HandMagic_Intense_lfhand_EW;
    // sparks (_sparks) //
  addCastingEffect = MoK_OrcBeamSparks_EW;
  addCastingEffect = MoK_OrcBeamSparks2_EW;
    // casting hand beam (_flav) //
  addCastingEffect = MoK_OrcBeams_EW;

    // burnt ground zodes //
    //  (note: these zodiacs are added here so that all subsequent zodes
    //          will be composited over the burnt ground)
  addCastingEffect = MoK_BurntGroundZodeA_EW;
  addCastingEffect = MoK_BurntGroundZodeB_EW;

  addCastingEffect = MoK_HandPrintFX_EG;
  
    // simmering ground zodes //
  addCastingEffect = Mok_SimmeringGroundZode_EG;
    // burning ground zode //
  addCastingEffect = MoK_BurningGroundZode_EW;

    // spinning mark flare zodes //
    //  (note: to ensure compositing over other zodiacs, these flare zodes
    //          are all the way down here)
  addCastingEffect = MoK_MarkFlareZodeA_EW;
  addCastingEffect = MoK_MarkFlareZodeB_EW;
    // ground fires //
  addCastingEffect = MoK_CueGroundFires_EW;
    // K Brand //
  addCastingEffect = MoK_CueKSymbol_EW;
    // symbol-launching scripts //
  addCastingEffect = SMS_CueWisps_EW;
  addCastingEffect = SMS_CueDreadBull_EW;

  _castDur = 0.5;
  _sms = 0;
  _flav = "mok";
  _sparks = 1;
  _hand = 0;
};

// sounds and lights added via sub-script functions //
MoK_add_Lighting_FX(MarkOfKorkSpell);
MoK_add_Audio_FX(MarkOfKorkSpell);

datablock afxRPGMagicSpellData(MarkOfKorkSpell_RPG)
{
  spellName = "Mark of Kork";
  desc = "Kork brands the terrain with some taunting orcish graffiti.\n" @
         "<font:Arial:6>\n" @
         "<font:Arial:14><just:center>Brought to you by <font:Arial Italic:16>The Letter K<just:left>.\n" @
         "\n" @
         "<font:Arial Italic:14>spell design and concept: <font:Arial:14>Matthew Durante\n" @
         "<font:Arial Italic:14>sound design: <font:Arial:14>Matt Pacyga;";
  sourcePack = "Spell Pack 2";
  iconBitmap = %mySpellDataPath @ "/MoK/icons/mok";
  target = "nothing";
  manaCost = 10;
  castingDur = 0.5;

  _castDur = 0.5;
  _sms = 0;
  _flav = "mok";
  _sparks = 1;
  _hand = 1;
};

// script methods

function SMS_PlantMine(%dblock, %spell, %cons, %xfm, %data)
{  
  %etron = startEffectron(SoulMineEffectron, %xfm, "mine");
}

function MarkOfKorkSpell::CueWisps(%this, %spell, %caster, %constraint, %xfm, %data)
{
  startWanderingWisps(%xfm, 11, "11 25", SMS_PlantMine);
}

function MarkOfKorkSpell::CueDreadBull(%this, %spell, %caster, %constraint, %xfm, %data)
{
  %bull_effectron = new afxEffectron()
  {
    datablock = DreadBullPortentEffectron;
    _n_pulses = 21;
    _shake = 1;
  };
  %bull_effectron.addConstraint(%xfm, "anchor");
}

function MarkOfKorkSpell::CueGroundFires(%this, %spell, %caster, %constraint, %xfm, %data)
{
  startClustersOfFire(%xfm, 20, "", MoK_calcFirePos);
}

function MarkOfKorkSpell::CueKSymbol(%this, %spell, %caster, %constraint, %xfm, %data)
{
  %effectron = startEffectron(TheLetterKEffectron, %xfm, "anchor");
}

function MarkOfKorkSpell::onActivate(%this, %spell, %caster, %target)
{
  Parent::onActivate(%this, %spell, %caster, %target);
  
  // set _simmerT[0-9] 
  %time = $MoK_BurningGround_delay+0.5;
  for (%i = 0; %i < 10; %i++)
  {
    %spell._simmerT[%i] = %time;
    %time += 1.3 + getRandomF(-1,1);
  }

  for (%j = 0; %j < 10; %j++)
  {
    %spell.simmerLifetimes_A[%j] = 2.2;
    %spell.simmerLifetimes_B[%j] = 2.2;
    %spell.simmerLifetimes_C[%j] = 2.2;
  }
}
  
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

if ($spell_reload)
{
  // Removes then adds all reloaded datablocks
  touchDataBlocks();
}
else
{
  // save script filename and data path for reloads
  MarkOfKorkSpell.scriptFile = $afxAutoloadScriptFile;
  MarkOfKorkSpell.spellDataPath = %mySpellDataPath;
  if (isFunction(addDemoSpellbookSpell))
  {
    addDemoSpellbookSpell(MarkOfKorkSpell, MarkOfKorkSpell_RPG);
  }
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
