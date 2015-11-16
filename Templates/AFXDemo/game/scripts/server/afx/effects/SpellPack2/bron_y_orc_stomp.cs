//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// BRON-Y-ORC STOMP SPELL
//
//    Walking your blue-eyed Merle or charging the enemy, this spell adds
//    a little spice to your step. Kork says, Move Fool!
//
//    parameters:
//      _mood          (string)       default="day"    ("day", "night")
//      _useVoices     (bool)         default=true
//      _doShakes      (bool)         default=true
//
//    internal:
//      _triggerScaleMag[##]
//      _triggerScale[##]
//      _triggerAngle[##]
//      _triggerRandomSnd[##]
//      _triggerScaleLAND
//      _triggerAngleLAND
//      _triggerRandomSndLAND
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
$MIN_REQUIRED_VERSION = 2.0;

// Test version requirements for this script
if ($AFX_VERSION < $MIN_REQUIRED_VERSION)
{
  error("AFX script " @ fileName($afxAutoloadScriptFile) @ " is not compatible with AFX versions older than " @ $MIN_REQUIRED_VERSION @ ".");
  return;
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

exec("./special/byos_cam_sub.cs");

//
// Here we test if the script is being reloaded or if this is the
// first time the script has executed this mission.
//
$spell_reload = isObject(BronYOrcStompSpell);
if ($spell_reload)
{
  // mark datablocks so we can detect which are reloaded this script
  markDataBlocks();
  // reset data path from previously saved value
  %mySpellDataPath = BronYOrcStompSpell.spellDataPath;
}
else
{
  // set data path from default plus containing folder name
  %mySpellDataPath = $afxSpellDataPath @ "/" @ $afxAutoloadScriptFolder;
  afxExecPrerequisite("ring_of_dust.cs", $afxAutoloadScriptFolder);
}


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// GLOBALS

$BYOS_MovementSpeedBias = 1.0;

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock afxXM_FreezeData(BYOS_freeze_XM)
{
  mask = $afxXfmMod::ALL;
};

datablock afxXM_LocalOffsetData(BYOS_StepOffset_LF_XM)
{
  localOffset = "-0.08 0.0 0.0";
};

datablock afxXM_LocalOffsetData(BYOS_StepOffset_RT_XM)
{
  localOffset = "0.08 0.0 0.0";
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// FOOTSTEP TRIGGERED ELEMENTS

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// BLENDED RUNNING ANIMATION (animation)
//
//  Three blend animations that cause the orc to crouch down
//  somewhat when combined with the running animation. They
//  vary in the amount of crouch and are selected according 
//  to the randomly selected scale for a specific trigger.
//
//  note - If there's a way to control the weight of a 
//  particular blend, this could probably be done with a
//  single blend with variable weighting.
//

datablock afxAnimClipData(BYOS_RunningA_clip_CE)
{
  clipName = "byos_run1";
  ignoreCorpse = true;
  rate = 2.0 * $BYOS_MovementSpeedBias;
};
datablock afxEffectWrapperData(BYOS_RunningA_clip_EW)
{
  effectEnabled = "$$ %%._triggerScaleMag[##] == 2";
  effect = BYOS_RunningA_clip_CE;
  constraint = target;
  lifetime = (15/30)*(1/(2.0*$BYOS_MovementSpeedBias));
  delay = 0.0;
};

datablock afxAnimClipData(BYOS_RunningB_clip_CE : BYOS_RunningA_clip_CE)
{
  clipName = "byos_run2";
};
datablock afxEffectWrapperData(BYOS_RunningB_clip_EW : BYOS_RunningA_clip_EW)
{
  effectEnabled = "$$ %%._triggerScaleMag[##] == 1";
  effect = BYOS_RunningB_clip_CE;
};

datablock afxAnimClipData(BYOS_RunningC_clip_CE : BYOS_RunningA_clip_CE)
{
  clipName = "byos_run3";
};
datablock afxEffectWrapperData(BYOS_RunningC_clip_EW : BYOS_RunningA_clip_EW)
{
  effectEnabled = "$$ %%._triggerScaleMag[##] == 0";
  effect = BYOS_RunningC_clip_CE;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// STOMP ZODIACS (zodiac)
//
//  Each foot stomp leaves behind three layered zodiacs, each
//  of which varies in size according to the random trigger scale.
//
//  -- The bottom layer is a crater of cracks that persists for 
//     a short period as residue.
//  -- On top of that is short-lived cracks with a colorful glow.
//  -- The top layer is a growing shockwave ring.
//

// glowing cracks

datablock afxZodiacData(BYOS_Step_Cracks_Hot_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/BYOS/zodiacs/byos_cracks_step_hot";
  radius = "$$ 5 * %%._triggerScale[##]";
  startAngle = "$$ %%._triggerAngle[##]";
  color = "1 1 1 1";
  blend = additive;
};
datablock afxEffectWrapperData(BYOS_Step_Cracks_Hot_LF_EW)
{
  effect = BYOS_Step_Cracks_Hot_CE; 
  posConstraint = "target.Bip01 L Foot";
  orientConstraint = "target";
  lifetime = 0.1;
  fadeOutTime = 0.3;
  xfmModifiers[0] = BYOS_StepOffset_LF_XM;
  xfmModifiers[1] = BYOS_freeze_XM;
};
datablock afxEffectWrapperData(BYOS_Step_Cracks_Hot_RT_EW : BYOS_Step_Cracks_Hot_LF_EW)
{
  posConstraint = "target.Bip01 R Foot";
  xfmModifiers[0] = BYOS_StepOffset_RT_XM;
  xfmModifiers[1] = BYOS_freeze_XM;
};

// residue cracks

datablock afxZodiacData(BYOS_Step_Cracks_CE : BYOS_Step_Cracks_Hot_CE)
{  
  texture = %mySpellDataPath @ "/BYOS/zodiacs/byos_cracks_step_black";
  blend = subtractive;
};
datablock afxEffectWrapperData(BYOS_Step_Cracks_LF_EW : BYOS_Step_Cracks_Hot_LF_EW)
{
  effect = BYOS_Step_Cracks_CE;
  lifetime = 0.0;
  fadeOutTime = 0.5;
  residueLifetime = 2.5;
};
datablock afxEffectWrapperData(BYOS_Step_Cracks_RT_EW : BYOS_Step_Cracks_LF_EW)
{
  posConstraint = "target.Bip01 R Foot";
  xfmModifiers[0] = BYOS_StepOffset_RT_XM;
  xfmModifiers[1] = BYOS_freeze_XM;
};

// stomp shockwave

datablock afxZodiacData(BYOS_StompWave_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/BYOS/zodiacs/byos_stomp_wave";
  radius = 1;
  startAngle = 0.0;
  startAngle = "$$ getRandomF(0.0, 360.0)";
  growthRate = "$$ 12.0 * %%._triggerScale[##]";
  color = "1.0 1.0 1.0 0.5";
  blend = additive;
};
datablock afxEffectWrapperData(BYOS_Step_StompWave_LF_EW)
{
  effect = BYOS_StompWave_CE;
  posConstraint = "target.Bip01 L Foot";
  orientConstraint = "target";
  lifetime = 0.1;
  fadeInTime = 0.1;
  fadeOutTime = "$$ 0.4 * %%._triggerScale[##]";
  xfmModifiers[0] = BYOS_StepOffset_LF_XM;
  xfmModifiers[1] = BYOS_freeze_XM;
};
datablock afxEffectWrapperData(BYOS_Step_StompWave_RT_EW : BYOS_Step_StompWave_LF_EW)
{
  posConstraint = "target.Bip01 R Foot";
  xfmModifiers[0] = BYOS_StepOffset_RT_XM;
  xfmModifiers[1] = BYOS_freeze_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// STOMP PARTICLE BURST (particles)
//
//  This is a small flash explosion that occurs at the foot
//  location with each step.
//  

datablock ParticleData(BYOS_ExplosionBlast_P)
{
  textureName          = %mySpellDataPath @ "/BYOS/particles/byos_flash";
  dragCoeffiecient     = 6.0; 
  gravityCoefficient   = 0.04;
  windCoefficient      = 0.0;
  inheritedVelFactor   = 0.00;
  lifetimeMS           = 150;
  lifetimeVarianceMS   = 50;
  spinRandomMin        = 0; 
  spinRandomMax        = 0;
  colors[0]            = "1.0 1.0 1.0 0.0";
    colors[1]            = "1.0 1.0 1.0 0.3";
    colors[2]            = "1.0 1.0 1.0 0.6";
    colors[3]            = "0.0 0.0 0.0 0.0"; 
  sizeBias             = "$$ 0.2 * %%._triggerScale[##]";
  sizes[0]             = 2;
    sizes[1]             = 4;
    sizes[2]             = 6;
    sizes[3]             = 4;
  times[0]             = 0.0;
    times[1]             = 0.2;
    times[2]             = 0.66;
    times[3]             = 1.0;
};

datablock ParticleEmitterData(BYOS_ExplosionBlast_E)
{
  ejectionPeriodMS      = 5;
  periodVarianceMS      = 2;
  ejectionVelocity      = 1.3;
  velocityVariance      = 0.3;
  particles             = "BYOS_ExplosionBlast_P";
  blendStyle            = "PREMULTALPHA";
};

datablock afxEffectWrapperData(BYOS_ExplosionBlast_LF_EW)
{
  effect = BYOS_ExplosionBlast_E;
  posConstraint = "target.Bip01 L Foot";
  lifetime    = 0.1;
  delay       = 0;
  fadeInTime  = 0;
  fadeOutTime = 0;
  xfmModifiers[0] = BYOS_StepOffset_LF_XM;
};
datablock afxEffectWrapperData(BYOS_ExplosionBlast_RT_EW : BYOS_ExplosionBlast_LF_EW)
{
  posConstraint = "target.Bip01 R Foot";
  xfmModifiers[0] = BYOS_StepOffset_RT_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// STOMP PARTICLE DUST (particles)

datablock ParticleData(BYOS_Dust_P)
{
  textureName          = "$$ \"" @ %mySpellDataPath @ "/Shared/particles/sp2_dust_\" @ " @ "%%._mood";
  dragCoeffiecient     = 1;
  gravityCoefficient   = 0; 
  inheritedVelFactor   = 0.00;
  lifetimeMS           = "$$ 1000 * %%._triggerScale[##]";
  lifetimeVarianceMS   = "$$  250 * %%._triggerScale[##]";
  randomizeSpinDir     = true;
  spinRandomMin        = 100.0;
  spinRandomMax        = 150.0;   
  colors[0]            = "0.0 0.0 0.0 0.0";
    colors[1]            = "0.7 0.7 0.7 1.0";
    colors[2]            = "0.7 0.7 0.7 1.0";
    colors[3]            = (0.7*0.5) SPC (0.7*0.5) SPC (0.7*0.5) SPC (1.0*0.5);
    colors[4]            = "0.0 0.0 0.0 0.0"; 
  sizeBias             = "$$ %%._triggerScale[##]";
  sizes[0]             = 2;
    sizes[1]             = 4;
    sizes[2]             = 6;
    sizes[3]             = 8;
    sizes[4]             = 10;
  times[0]             = 0.0;
    times[1]             = 0.2;
    times[2]             = 0.3;
    times[3]             = 0.5;
    times[4]             = 1.0;
};

datablock afxParticleEmitterConeData(BYOS_ExplosionDust_E)
{
  ejectionPeriodMS      = 2;
  periodVarianceMS      = 1;
  ejectionVelocity      = 5;
  ejectionVelocity      = "$$ 10.0 * %%._triggerScale[##]";
  velocityVariance      = 0.0;
  velocityVariance      = "$$ 2.0 * %%._triggerScale[##]";
  particles             = "BYOS_Dust_P";
  ejectionOffset        = 1.0;
  ejectionOffset        = "$$ 2.0 * %%._triggerScale[##]";
  vector                = "0 0 1";
  spreadMin             = 180;
  spreadMax             = 180;
  blendStyle            = "PREMULTALPHA";
};

datablock afxEffectWrapperData(BYOS_ExplosionDust_LF_EW)
{
  effectEnabled = "$$ %%._dust";
  effect = BYOS_ExplosionDust_E;
  posConstraint = "target.Bip01 L Foot";
  orientConstraint = "target";
  lifetime    = 0.05;
  delay       = 0;
  fadeInTime  = 0.05;
  fadeOutTime = 0.0;
  xfmModifiers[0] = BYOS_StepOffset_LF_XM;
  xfmModifiers[1] = BYOS_freeze_XM;
};
datablock afxEffectWrapperData(BYOS_ExplosionDust_RT_EW : BYOS_ExplosionDust_LF_EW)
{
  posConstraint = "target.Bip01 R Foot";
  xfmModifiers[0] = BYOS_StepOffset_RT_XM;
  xfmModifiers[1] = BYOS_freeze_XM;
};



//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// STOMP SHAKE (camera shake)

datablock afxCameraShakeData(BYOS_CamShake_CE)
{
   frequency = "0.0 0.0 10.0";
   amplitude = "$$ \"0 0 \" @ (0.1*%%._triggerScale[##])";
   radius  = 100.0;
   falloff = 5.0;
};

datablock afxEffectWrapperData(BYOS_CamShake_LF_EW)
{
  effectEnabled = "$$ %%._doShakes";
  effect = BYOS_CamShake_CE;
  posConstraint = "target.Bip01 L Foot";
  lifetime    = 1.0;
  delay       = 0;
  fadeInTime  = 0;
  fadeOutTime = 0;
  xfmModifiers[0] = BYOS_StepOffset_LF_XM;
};
datablock afxEffectWrapperData(BYOS_CamShake_RT_EW : BYOS_CamShake_LF_EW)
{
  posConstraint = "target.Bip01 R Foot";
  xfmModifiers[0] = BYOS_StepOffset_RT_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// JUMP TRIGGERED ELEMENTS

// BLENDED JUMP ANIMATION (animation)

datablock afxAnimClipData(BYOS_Jumping_clip_CE)
{
  clipName = "byos_jump";
  ignoreCorpse = true;
  rate = 1.0;
};
datablock afxEffectWrapperData(BYOS_Jumping_JUMP_clip_EW)
{
  effect = BYOS_Jumping_clip_CE;
  constraint = target;
  lifetime = (25/30);
  delay = 0.2;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// JUMP LANDING TRIGGERED ELEMENTS (client)

// BLENDED LANDING ANIMATION (animation)

datablock afxAnimClipData(BYOS_Landing_clip_CE)
{
  clipName = "byos_land";
  ignoreCorpse = true;
  rate = 1.0;
};
datablock afxEffectWrapperData(BYOS_Landing_LAND_clip_EW)
{
  effect = BYOS_Landing_clip_CE;
  constraint = target;
  lifetime = (30/30);
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// JUMP LANDING GROUND ZODIACS (zodiac)

datablock afxZodiacData(BYOS_Landing_Cracks_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/BYOS/zodiacs/byos_cracks_land_black";
  radius = "$$ 8.0 * %%._triggerScaleLAND";
  startAngle = "$$ %%._triggerAngleLAND";
  color = "1.0 1.0 1.0 1.0";
  blend = subtractive;
};
datablock afxZodiacData(BYOS_Landing_Cracks_Hot_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/BYOS/zodiacs/byos_cracks_land_hot";
  radius = "$$ 8.0 * %%._triggerScaleLAND";
  startAngle = "$$ %%._triggerAngleLAND";
  color = "1.0 1.0 1.0 1.0";
  blend = additive;
};

datablock afxEffectWrapperData(BYOS_Landing_Cracks_EW)
{
  effect = BYOS_Landing_Cracks_CE;
  constraint = target;
  lifetime = 0.5;
  fadeOutTime = 0.5;
  fadeInTime = 0.1;
  residueLifetime = 5.0;
  xfmModifiers[0] = BYOS_freeze_XM;
};
datablock afxEffectWrapperData(BYOS_Landing_Cracks_Hot_EW : BYOS_Landing_Cracks_EW)
{
  effect = BYOS_Landing_Cracks_Hot_CE;
  lifetime = 0.2;
  fadeOutTime = 0.5;
  residueLifetime = 0;
};

datablock afxZodiacData(BYOS_Landing_StompWave_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/BYOS/zodiacs/byos_stomp_wave";
  radius = 1;
  startAngle = "$$ getRandomF(0.0, 360.0)";
  growthRate = "$$ 12.0*2 * %%._triggerScaleLAND";
  color = "1.0 1.0 1.0 0.5";
  blend = additive;
};
//
datablock afxEffectWrapperData(BYOS_Landing_StompWave_EW)
{
  effect = BYOS_Landing_StompWave_CE;
  constraint = target;
  lifetime = 0.1;
  fadeInTime = 0.1;
  fadeOutTime = 0.4;
  fadeOutTime = "$$ 0.4 * %%._triggerScaleLAND";
  xfmModifiers[0] = BYOS_freeze_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// JUMP LANDING PARTICLE EFFECTS (particles)

datablock ParticleData(BYOS_ExplosionBlast_LAND_P : BYOS_ExplosionBlast_P)
{
  sizeBias = "$$ 0.8 * %%._triggerScaleLAND";
};

datablock ParticleEmitterData(BYOS_ExplosionBlast_LAND_E)
{
  ejectionPeriodMS      = 5;
  periodVarianceMS      = 2;
  ejectionVelocity      = 1.3;
  velocityVariance      = 0.3;
  particles             = "BYOS_ExplosionBlast_LAND_P";
  blendStyle = "PREMULTALPHA";
};

datablock afxEffectWrapperData(BYOS_ExplosionBlast_LAND_EW)
{
  effect = BYOS_ExplosionBlast_LAND_E;
  posConstraint = "target";
  lifetime    = 0.1;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// JUMP LANDING CAMERA SHAKE (camera shake)

datablock afxCameraShakeData(BYOS_Landing_CamShake_CE)
{
   frequency = "0.0 0.0 10.0";
   //amplitude = "$$ \"0 0 \" @ (0.1*%%._triggerScale[##])";
   amplitude = "0.0 0.0 0.5";
   radius  = 150.0;
   falloff = 5.0;
};

datablock afxEffectWrapperData(BYOS_Landing_CamShake_EW)
{
  effectEnabled = "$$ %%._doShakes";
  effect = BYOS_Landing_CamShake_CE;
  posConstraint = "target";
  lifetime    = 1.5;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// LANDING TRIGGERED ELEMENTS (server)

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// DUST EFFECT TRIGGER (script)

// This script-event calls the script that will launch Dust Ring
datablock afxScriptEventData(BYOS_launchDustRing_CE)
{
  methodName = "launchDustRing";
  scriptData = 1.0;
};
//
datablock afxEffectWrapperData(BYOS_launchDustRing_EW)
{
  effectEnabled = "$$ %%._dust";
  effect = BYOS_launchDustRing_CE;
  constraint = target;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// DEAL DAMAGE (damage)

datablock afxAreaDamageData(BYOS_AreaJumpDamage_CE)
{
  flavor = "jump";
  radius = 20;
  damage = 40;
  impulse = 5000;
  notifyDamageSource = false;
  excludeConstraintObject = true;
};
datablock afxEffectWrapperData(BYOS_AreaJumpDamage_EW)
{
  effect = BYOS_AreaJumpDamage_CE;
  constraint = "target";
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// OPENING ELEMENTS

datablock afxAnimClipData(BYOS_Casting_clip_CE)
{
  clipName = "byos";
  ignoreCorpse = true;
};
datablock afxEffectWrapperData(BYOS_Casting_clip_EW)
{
  effectEnabled = "$$ %%._useVoices == true";
  effect = BYOS_Casting_clip_CE;
  posConstraint = target;
  delay = 0;  
  lifetime = 75/30;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// OTHER ELEMENTS

datablock afxFootSwitchData(BYOS_OverrideFootstepFX_CE)
{
  overrideAll = true;
};
datablock afxEffectWrapperData(BYOS_OverrideFootstepFX_EW)
{
  effect = BYOS_OverrideFootstepFX_CE;
  posConstraint = target;
}; 

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// AUDIO AND LIGHTING

switch$ (afxGetEngine())
{
  case "TGE":
    exec("./lighting/byos_lighting_tge_sub.cs");
    exec("./audio/byos_audio_sub.cs");
  case "TGEA":
    exec("./lighting/byos_lighting_tgea_sub.cs");
    exec("./audio/byos_audio_sub.cs");
 case "T3D":
    exec("./lighting/byos_lighting_t3d_sub.cs");
    exec("./audio/byos_audio_sub.cs");
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// PHRASE EFFECTS

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// Triggered Phrase Effects (one for each foot)

datablock afxPhraseEffectData(BYOS_phrase_effect_LF_CE)
{
  triggerMask = $AFX::PLAYER_LF_FOOT_C_TRIGGER;
  onTriggerCommand = "BYOS_pickStepParams(%%,##)";

    // blended crouch clips //
  addEffect = BYOS_RunningA_clip_EW;
  addEffect = BYOS_RunningB_clip_EW;
  addEffect = BYOS_RunningC_clip_EW;
    // stomp zodiacs //
  addEffect = BYOS_Step_Cracks_LF_EW;
  addEffect = BYOS_Step_Cracks_Hot_LF_EW;
  addEffect = BYOS_Step_StompWave_LF_EW;
    // other stomp effects //
  addEffect = BYOS_ExplosionBlast_LF_EW;
  addEffect = BYOS_ExplosionDust_LF_EW;
  addEffect = BYOS_CamShake_LF_EW;
};
datablock afxEffectWrapperData(BYOS_phrase_effect_LF_EW)
{
  effect = BYOS_phrase_effect_LF_CE;
  posConstraint = target;
  groupIndex = 0;
};

datablock afxPhraseEffectData(BYOS_phrase_effect_RT_CE)
{
  triggerMask = $AFX::PLAYER_RT_FOOT_C_TRIGGER;
  onTriggerCommand = "BYOS_pickStepParams(%%,##)";

    // blended crouch clips //
  addEffect = BYOS_RunningA_clip_EW;
  addEffect = BYOS_RunningB_clip_EW;
  addEffect = BYOS_RunningC_clip_EW;
    // stomp zodiacs //
  addEffect = BYOS_Step_Cracks_RT_EW;
  addEffect = BYOS_Step_Cracks_Hot_RT_EW;
  addEffect = BYOS_Step_StompWave_RT_EW;
    // other stomp effects //
  addEffect = BYOS_ExplosionBlast_RT_EW;
  addEffect = BYOS_ExplosionDust_RT_EW;
  addEffect = BYOS_CamShake_RT_EW;
};  
datablock afxEffectWrapperData(BYOS_phrase_effect_RT_EW)
{
  effect = BYOS_phrase_effect_RT_CE;
  posConstraint = target;
  groupIndex = 1;
};

BYOS_add_footstep_Lighting_FX(BYOS_phrase_effect_LF_CE, BYOS_phrase_effect_RT_CE);
BYOS_add_footstep_Audio_FX(BYOS_phrase_effect_LF_CE, BYOS_phrase_effect_RT_CE);

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// jump trigger

datablock afxPhraseEffectData(BYOS_phrase_effect_S_JUMP_CE)
{
  addEffect = BYOS_Jumping_JUMP_clip_EW;
  triggerMask = $AFX::PLAYER_JUMP_S_TRIGGER;
};  
datablock afxEffectWrapperData(BYOS_phrase_effect_S_JUMP_EW)
{
  effect = BYOS_phrase_effect_S_JUMP_CE;
  constraint = target;
  fadeOutTime = 1.0;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// client-side landing trigger

datablock afxPhraseEffectData(BYOS_phrase_effect_LAND_CE)
{
  onTriggerCommand = "BYOS_pickLandParams(%%)";
  triggerMask = $AFX::PLAYER_LANDING_C_TRIGGER;

  addEffect = BYOS_Landing_LAND_clip_EW;
  addEffect = BYOS_Landing_Cracks_EW;
  addEffect = BYOS_Landing_Cracks_Hot_EW;
  addEffect = BYOS_Landing_StompWave_EW;
  addEffect = BYOS_ExplosionBlast_LAND_EW;
  addEffect = BYOS_Landing_CamShake_EW;
};  
datablock afxEffectWrapperData(BYOS_phrase_effect_LAND_EW)
{
  effect = BYOS_phrase_effect_LAND_CE;
  posConstraint = target;
  fadeOutTime = 2.5;
};

BYOS_add_landing_Lighting_FX(BYOS_phrase_effect_LAND_CE);
BYOS_add_landing_Audio_FX(BYOS_phrase_effect_LAND_CE);

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// client-side landing trigger

datablock afxPhraseEffectData(BYOS_phrase_effect_S_LAND_CE)
{
  addEffect = BYOS_launchDustRing_EW;
  addEffect = BYOS_AreaJumpDamage_EW;
  triggerMask = $AFX::PLAYER_LANDING_S_TRIGGER;
};  
datablock afxEffectWrapperData(BYOS_phrase_effect_S_LAND_EW)
{
  effect = BYOS_phrase_effect_S_LAND_CE;
  posConstraint = target;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// BRON-Y-ORC STOMP SPELL

datablock afxMagicSpellData(BronYOrcStompSpell)
{
  castingDur = 1.0;
  clientScriptFile = %mySpellDataPath @ "/BYOS/byos_client.cs";
  clientInitFunction = "BYOS_clientInit";

  lingerDur = 20.0;
  allowMovementInterrupts = false;

    // opening //
  addCastingEffect = BYOS_Casting_clip_EW;
    // footstep effects //
  addLingerEffect = BYOS_phrase_effect_LF_EW;
  addLingerEffect = BYOS_phrase_effect_RT_EW;
    // jump effects //
  addLingerEffect = BYOS_phrase_effect_S_JUMP_EW;
    // landing effects //
  addLingerEffect = BYOS_phrase_effect_LAND_EW;
  addLingerEffect = BYOS_phrase_effect_S_LAND_EW;
    // other //
  addLingerEffect = BYOS_OverrideFootstepFX_EW;

    // default parameter settings //
  _useVoices = 1; 
  _mood = "day";
  _doShakes = 1;
  _dust = 1;
  _cine = 0;
};

// sounds added via sub-script functions //
BYOS_add_Audio_FX(BronYOrcStompSpell);

datablock afxRPGMagicSpellData(BronYOrcStompSpell_RPG)
{
  spellName = "Bron-Y-Orc Stomp";
  desc = "Walking your blue-eyed Merle or charging the enemy, " @
         "this spell adds a little spice to your step.\n" @ 
         "<font:Arial:6>\n" @
         "<just:center><font:Arial:14>Kork says, <font:Arial Italic:14>Move Fool!<just:left>\n" @
         "\n" @
         "<font:Arial Italic:14>spell design and concept: <font:Arial:14>Matthew Durante\n" @
         "<font:Arial Italic:14>sound design: <font:Arial:14>Matthew Durante";
  sourcePack = "Spell Pack 2";
  iconBitmap = %mySpellDataPath @ "/BYOS/icons/byos";
  target = "friend";
  canTargetSelf = true;
  manaCost = 10;
  castingDur = BronYOrcStompSpell.castingDur;

  //_cine = 1;
  //_doShakes = 0;
};

// if spell is already active on the target, prevent cast
function BronYOrcStompSpell::readyToCast(%this, %caster, %target)
{
  if (!Parent::readyToCast(%this, %caster, %target))
    return false;

  // prevent cast if target is already affected by this spell
  %alreadyStomping = isObject(%target) && %target.isStomping;

  if (%alreadyStomping && isObject(%caster.client))
      DisplayScreenMessage(%caster.client, "The target is already enchanted with this spell.");

  return !%alreadyStomping;
}
function BronYOrcStompSpell::onActivate(%this, %spell, %caster, %target)
{
  Parent::onActivate(%this, %spell, %caster, %target);

  %target.isStomping = true;
  
  %night_mission = false;
  if (isObject(theLevelInfo))
    %night_mission = theLevelInfo.isNightMission;
  else if (isObject(MissionInfo))
    %night_mission = MissionInfo.isNightMission;
  %spell._mood = (%night_mission) ? "night" : "day"; 

  //UAISK+AFX Interop Changes: Start
  if ($UAISK_Is_Available && %target.isBot)
  {
    //See how long until spell stops
    %time = $Sim::Time + BronYOrcStompSpell.lingerDur;
    schedule(BronYOrcStompSpell.castingDur*1000, 0, "randMoveOffsetPos", %target, %time);
  }
  //UAISK+AFX Interop Changes: End
}

function BronYOrcStompSpell::onImpact(%this, %spell, %caster, %impObj, %impPos, %impNorm)
{
  %spell.stomptron = startEffectron(BYOS_StompEffectron, %impObj, "stomper");
  %spell.stomptron.stomper_guy = %impObj;
  %impObj.setMovementSpeedBias($BYOS_MovementSpeedBias);

  if (%spell._cine)
  {
    %effe = new afxEffectron() 
    {
      datablock = BYOS_CineCam_Effe;
    };
    %effe.addConstraint(%impObj, "camAnchor");
    %effe.addConstraint(%impObj, "camCOI");
    %effe.addExplicitClient(%caster.client);
  }
}

function BronYOrcStompSpell::onDeactivate(%this, %spell)
{
  if (isObject(%spell.stomptron))
  {
    %spell.stomptron.stomper_guy.setMovementSpeedBias(1.0);
    %spell.stomptron.stomper_guy.isStomping = false;
    %spell.stomptron.interrupt();
  }
}

function BronYOrcStompSpell::launchDustRing(%this, %spell, %caster, %consObj, %xfm, %data)
{
  // %data is scale

  %eb_spell = new afxMagicSpell() {
    datablock = RingOfDustSpell;
    caster = %caster; 
    _castDur = 0;
    _scale =  1.0; //%data; 
    _radiusStart = 5.0;
    _radiusEnd = 40.0;
    _lifetime = 3;
    _spread = 360;
    _particleScale = 0.3;
    _ejectionPeriodMS = 20;
    _dustOvershoot = true;
    _casterAnchor = 0;  
  };

  %eb_spell.addConstraint(%xfm, "anchor");
}

// If the above BYOS_AreaJumpDamage_CE.notifyDamageSource is set to true, this callout will
// occur whenever a jump landing does damage to something. It could be used to assign custom
// response effects to replace or augment the built-in impulse response.
function BronYOrcStompSpell::onInflictedAreaDamage(%this, %effectron, %targetObject, %damage, %damageType, %position)
{
  echo("onInflictedAreaDamage to" SPC %targetObject.getShapeName() SPC %damage SPC %damageType SPC %position);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// BYOS STOMP EFFECTRON
//
//  While BYOS is active, this effectron is attached to the stomping player. While the
//  stomper player is moving, it triggers area damage around the stomper at regular
//  intervals. This is not done with footstep triggers because they occur client-side
//  and damage effects must happen on the server. Instead, the effectron is configured 
//  to loop and perform an AreaDamage effect every half second.
//

datablock afxXM_LocalOffsetData(BYOS_AreaStomp_offset_XM)
{
  localOffset = "0.0 1.0 0.0";
};
datablock afxAreaDamageData(BYOS_AreaStompDamage_CE)
{
  flavor = "stomp";
  radius = 4;
  damage = 10;
  impulse = 2000;
  notifyDamageSource = false;
  excludeConstraintObject = true;
};
datablock afxEffectWrapperData(BYOS_AreaStompDamage_EW)
{
  effectEnabled = "$$ (isObject(%%.stomper_guy)) ? (%%.stomper_guy.getSpeed() > 0.5) : false";
  effect = BYOS_AreaStompDamage_CE;
  constraint = "stomper";
  xfmModifiers[0] = BYOS_AreaStomp_offset_XM;
};

datablock afxEffectronData(BYOS_StompEffectron)
{
  duration = 0.5;
  numLoops = 1000;
  execOnNewClients = true;
  addEffect = BYOS_AreaStompDamage_EW;
};

// If the above BYOS_AreaStompDamage_CE.notifyDamageSource is set to true, this callout will
// occur whenever a stomp does damage to something. It could be used to assign custom
// response effects to replace or augment the built-in impulse response.
function BYOS_StompEffectron::onInflictedAreaDamage(%this, %effectron, %targetObject, %damage, %damageType, %position)
{
  echo("onInflictedAreaDamage to" SPC %targetObject.getShapeName() SPC %damage SPC %damageType SPC %position);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

if ($spell_reload)
{
  // Removes then adds all reloaded datablocks
  touchDataBlocks();
}
else
{
  // save script filename and data path for reloads
  BronYOrcStompSpell.scriptFile = $afxAutoloadScriptFile;
  BronYOrcStompSpell.spellDataPath = %mySpellDataPath;
  if (isFunction(addDemoSpellbookSpell))
    addDemoSpellbookSpell(BronYOrcStompSpell, BronYOrcStompSpell_RPG);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

