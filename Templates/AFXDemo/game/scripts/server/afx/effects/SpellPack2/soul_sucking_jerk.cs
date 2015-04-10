//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// SOUL SUCKING JERK SPELL
//
//    Your foe has too much Soul, so yank out some of his spare Soul Substance,
//    a useful reagent.
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

//
// Here we test if the script is being reloaded or if this is the
// first time the script has executed this mission.
//
$spell_reload = isObject(SoulSuckingJerkSpell);
if ($spell_reload)
{
  // mark datablocks so we can detect which are reloaded this script
  markDataBlocks();
  // reset data path from previously saved value
  %mySpellDataPath = SoulSuckingJerkSpell.spellDataPath;
}
else
{
  // set data path from default plus containing folder name
  %mySpellDataPath = $afxSpellDataPath @ "/" @ $afxAutoloadScriptFolder;
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// GLOBALS

datablock afxAnimClipData(SSJ_Casting_clip_CE)
{
  clipName = "ssj";
  ignoreCorpse = true;
  rate = 1.0;
};
//
datablock afxEffectWrapperData(SSJ_Casting_clip_EW)
{
  effect = SSJ_Casting_clip_CE;
  constraint = caster;
  lifetime = 120/30;
  delay = 0;
};

datablock afxAnimClipData(SSJ_CastingHold_clip_CE)
{
  clipName = "ssj_hold";
  ignoreCorpse = true;
  rate = 1.0;
  transitionTime = 1.0; // for non-blend, to hide weird transition
};
//
datablock afxEffectWrapperData(SSJ_CastingHold_clip_EW)
{
  effect = SSJ_CastingHold_clip_CE;
  constraint = caster;
};

datablock afxAnimClipData(SSJ_CastingGrab_clip_CE)
{
  clipName = "ssj_grab";
  ignoreCorpse = true;
  rate = 1.0;
  transitionTime = 0.1;
};
//
datablock afxEffectWrapperData(SSJ_CastingGrab_clip_EW)
{
  effect = SSJ_CastingGrab_clip_CE;
  constraint = caster;
  lifetime = 75/30;
  delay = 0;
};

datablock afxAnimLockData(SSJ_AnimLock_CE)
{
  priority = 0;
};
//
datablock afxEffectWrapperData(SSJ_AnimLock_EW)
{
  effect = SSJ_AnimLock_CE;
  constraint = caster;
  lifetime = 3.33;
  delay = 0;
};

datablock afxModelData(SSJ_SkeletalHand_RT_CE)
{
  shapeFile = %mySpellDataPath @ "/SSJ/models/ssj_skeleHand_RT.dts";
  useVertexAlpha = true; // TGE (ignored by TGEA)
};
//
datablock afxEffectWrapperData(SSJ_SkeletalHand_RT_EW)
{
  effect = SSJ_SkeletalHand_RT_CE;
  constraint = "caster.Mount0";
  effectName = "SkeletalHand_RT";
  isConstraintSrc = true;
  lifetime = $AFX::INFINITE_TIME;
  delay = 1.0;
  fadeInTime = 1.0;
  fadeOutTime = 1.0;
};

datablock afxAnimClipData(SSJ_SkeletalHand_clip1_CE)
{
  clipName = "seq1";  
  rate = 1.0;
};
//
datablock afxEffectWrapperData(SSJ_SkeletalHand_clip1_EW)
{
  effect = SSJ_SkeletalHand_clip1_CE;
  constraint = "#effect.SkeletalHand_RT";
  lifetime = 50/30;
  delay = 1.0;
};

datablock afxAnimClipData(SSJ_SkeletalHand_clip2_CE)
{
  clipName = "seq2";  
  rate = 1.3;
};
//
datablock afxEffectWrapperData(SSJ_SkeletalHand_clip2_EW)
{
  effect = SSJ_SkeletalHand_clip2_CE;
  constraint = "#effect.SkeletalHand_RT";
  //lifetime = $AFX::INFINITE_TIME; CRASH!!!!! JTF Note: check this
  lifetime = 20*((40/30)*(1.0/1.3));
  delay = 1.0+(50/30) +(0/30);
};

datablock afxAnimClipData(SSJ_SkeletalHand_clip3_CE)
{
  clipName = "seq3";  
  rate = 1.0;
};
//
datablock afxEffectWrapperData(SSJ_SkeletalHand_clip3_EW)
{
  effect = SSJ_SkeletalHand_clip3_CE;
  constraint = "#effect.SkeletalHand_RT";
  lifetime = (145-90)/30;
  delay = 0;
};

datablock afxPhraseEffectData(SSJ_SkeletalHand_clip3_phrase_effect_CE)
{
  triggerMask = 0x800000; // BIT(23)
  ignoreChoreographerTriggers = false;
  ignoreConstraintTriggers = true;
  ignorePlayerTriggers = true;
  addEffect = SSJ_SkeletalHand_clip3_EW;
};
datablock afxEffectWrapperData(SSJ_SkeletalHand_clip3_phrase_effect_EW)
{
  effect = SSJ_SkeletalHand_clip3_phrase_effect_CE;
  constraint = "#effect.SkeletalHand_RT";
};

datablock ParticleData(SSJ_HandBoneGlow_PalmA_P)
{
  textureName          = %mySpellDataPath @ "/SSJ/particles/ssj_hand_glow";
  dragCoeffiecient     = 0;
  gravityCoefficient   = 0;
  inheritedVelFactor   = 0.00;
  lifetimeMS           = (7/30)*1000;
  lifetimeVarianceMS   = 0;
  spinRandomMin        = 0;
  spinRandomMax        = 0;
  colors[0]            = "1.0 1.0 1.0 0.0";
    colors[1]            = "1.0 1.0 1.0 0.2";
    colors[2]            = "1.0 1.0 1.0 0.2";
    colors[3]            = "1.0 1.0 1.0 0.0";
  sizes[0]             = 0.7;
    sizes[1]             = 1.2;
    sizes[2]             = 1.4;
    sizes[3]             = 1.6;
  times[0]             = 0.0;
    times[1]             = 0.2;
    times[2]             = 0.8;
    times[3]             = 1.0;
};
datablock ParticleData(SSJ_HandBoneGlow_PalmB_P : SSJ_HandBoneGlow_PalmA_P)
{
  lifetimeMS           = (6/30)*1000;
};
datablock ParticleData(SSJ_HandBoneGlow_PalmC_P : SSJ_HandBoneGlow_PalmA_P)
{ 
  lifetimeMS           = (5/30)*1000;
};

datablock ParticleData(SSJ_HandBoneGlow_A_P : SSJ_HandBoneGlow_PalmA_P)
{
  lifetimeMS           = (5/30)*1000;
  colors[0]            = "1.0 1.0 1.0 0.0";
  colors[1]            = "1.0 1.0 1.0 0.5";
  colors[2]            = "1.0 1.0 1.0 0.5";
  colors[3]            = "1.0 1.0 1.0 0.0";
  sizes[0]             = 0.6;
  sizes[1]             = 0.6;
  sizes[2]             = 0.6;
  sizes[3]             = 0.6;
};
datablock ParticleData(SSJ_HandBoneGlow_B_P : SSJ_HandBoneGlow_A_P)
{ 
  lifetimeMS           = (4/30)*1000;
};
datablock ParticleData(SSJ_HandBoneGlow_C_P : SSJ_HandBoneGlow_A_P)
{
  lifetimeMS           = (3/30)*1000;
};
datablock ParticleData(SSJ_HandBoneGlow_D_P : SSJ_HandBoneGlow_A_P)
{
  lifetimeMS           = (2/30)*1000;
};

// Hand Smoke Particles
datablock ParticleData(SSJ_HandBoneSmokeA_P)
{
  textureName          = %mySpellDataPath @ "/SSJ/particles/ssj_hand_smoke";
  dragCoeffiecient     = 0.1;
  gravityCoefficient   = 0;
  windCoefficient      = 0;
  inheritedVelFactor   = 0.00;
  lifetimeMS           = 2000;
  lifetimeVarianceMS   = 200;
  spinRandomMin        = -300.0;
  spinRandomMax        = 300.0;
  colors[0]            = "0.2 0.2 0.2 0.2";
    colors[1]            = "0.05 0.05 0.05 0.05";
    colors[2]            = "0.02 0.02 0.02 0.02";
    colors[3]            = "0.0 0.0 0.0 0.0";
  sizes[0]             = 0.2;
    sizes[1]             = 0.7*1.5;
    sizes[2]             = 1.3*2;
    sizes[3]             = 2.0*2;
  times[0]             = 0.0;
    times[1]             = 0.4;
    times[2]             = 0.7;
    times[3]             = 1.0;
};
datablock ParticleData(SSJ_HandBoneSmokeB_P : SSJ_HandBoneSmokeA_P)
{
  lifetimeMS           = 2000*2;
  lifetimeVarianceMS   = 200 *2;
  sizes[0]             = 0.2;
    sizes[1]             = 0.7*2.5;
    sizes[2]             = 1.3*4;
    sizes[3]             = 2.0*4;
};
datablock ParticleData(SSJ_HandBoneSmokeC_P : SSJ_HandBoneSmokeA_P)
{
  lifetimeMS           = 2000*0.6;
  lifetimeVarianceMS   = 200 *0.6;
  sizes[0]             = 0.2;
    sizes[1]             = 0.7*3.0;
    sizes[2]             = 1.3*6;
    sizes[3]             = 2.0*6;
};

datablock afxParticleEmitterVectorData(SSJ_BoneGlow_PalmA_E)
{
  ejectionOffset    = 0;
  ejectionPeriodMS  = 20;
  periodVarianceMS  = 0;
  ejectionVelocity  = 5;
  velocityVariance  = 0;
  particles         = "SSJ_HandBoneGlow_PalmA_P SSJ_HandBoneGlow_PalmB_P SSJ_HandBoneGlow_PalmC_P";
  blendStyle = "ADDITIVE";
  vector = "0.0 1.0 0.0";
  fadeColor = true;
};
datablock afxParticleEmitterVectorData(SSJ_BoneGlow_PalmB_E : SSJ_BoneGlow_PalmA_E)
{
  particles         = "SSJ_HandBoneGlow_PalmB_P";
};
datablock afxParticleEmitterVectorData(SSJ_BoneGlow_PalmC_E : SSJ_BoneGlow_PalmA_E)
{
  particles         = "SSJ_HandBoneGlow_PalmC_P";
};

datablock afxParticleEmitterVectorData(SSJ_BoneGlow_A_E : SSJ_BoneGlow_PalmA_E)
{   
  particles         = "SSJ_HandBoneGlow_A_P";
};
datablock afxParticleEmitterVectorData(SSJ_BoneGlow_B_E : SSJ_BoneGlow_A_E)
{   
  particles         = "SSJ_HandBoneGlow_B_P";
};
datablock afxParticleEmitterVectorData(SSJ_BoneGlow_C_E : SSJ_BoneGlow_A_E)
{
  particles         = "SSJ_HandBoneGlow_C_P";
};
datablock afxParticleEmitterVectorData(SSJ_BoneGlow_D_E : SSJ_BoneGlow_A_E)
{
  particles         = "SSJ_HandBoneGlow_D_P";
};

// vector emitter pointing straight up
datablock afxParticleEmitterVectorData(SSJ_HandSmokeA_E)
{
  ejectionPeriodMS = 20;
  periodVarianceMS = 0;
  ejectionVelocity = 1.0;
  velocityVariance = 1.0;
  particles        = SSJ_HandBoneSmokeA_P;
  vector = "0.0 0.0 1.0";
  fadeColor = true;
  fadeAlpha = true;
  blendStyle = "PREMULTALPHA";
};
datablock afxParticleEmitterVectorData(SSJ_HandSmokeB_E : SSJ_HandSmokeA_E)
{
  particles        = SSJ_HandBoneSmokeB_P;
};
datablock afxParticleEmitterVectorData(SSJ_HandSmokeC_E : SSJ_HandSmokeA_E)
{
  particles        = SSJ_HandBoneSmokeC_P;
};

datablock afxEffectWrapperData(SSJ_BoneGlow_PALM_A_EW : SSJ_SkeletalHand_RT_EW)
{
  effect = SSJ_BoneGlow_PalmB_E;
  constraint = "#effect.SkeletalHand_RT.mount_baseA";
  effectName = "";
  isConstraintSrc = false;
};
datablock afxEffectWrapperData(SSJ_BoneGlow_PALM_B_EW : SSJ_BoneGlow_PALM_A_EW)
{
  effect = SSJ_BoneGlow_PalmA_E;
  constraint = "#effect.SkeletalHand_RT.mount_baseB";
};
datablock afxEffectWrapperData(SSJ_BoneGlow_PALM_C_EW : SSJ_BoneGlow_PALM_A_EW)
{
  effect = SSJ_BoneGlow_PalmB_E;
  constraint = "#effect.SkeletalHand_RT.mount_baseC";
};
datablock afxEffectWrapperData(SSJ_BoneGlow_PALM_D_EW : SSJ_BoneGlow_PALM_A_EW)
{
  effect = SSJ_BoneGlow_PalmC_E;
  constraint = "#effect.SkeletalHand_RT.mount_baseD";
};

datablock afxEffectWrapperData(SSJ_BoneGlow_IDX_A_EW : SSJ_BoneGlow_PALM_A_EW)
{
  effect = SSJ_BoneGlow_B_E;
  constraint = "#effect.SkeletalHand_RT.mount_idxA";  
};
datablock afxEffectWrapperData(SSJ_BoneGlow_IDX_B_EW : SSJ_BoneGlow_IDX_A_EW)
{
  effect = SSJ_BoneGlow_C_E;
  constraint = "#effect.SkeletalHand_RT.mount_idxB";
};
datablock afxEffectWrapperData(SSJ_BoneGlow_IDX_C_EW : SSJ_BoneGlow_IDX_A_EW)
{
  effect = SSJ_BoneGlow_D_E;
  constraint = "#effect.SkeletalHand_RT.mount_idxC";
};

datablock afxEffectWrapperData(SSJ_BoneGlow_MID_A_EW : SSJ_BoneGlow_PALM_A_EW)
{
  effect = SSJ_BoneGlow_B_E;
  constraint = "#effect.SkeletalHand_RT.mount_midA";  
};
datablock afxEffectWrapperData(SSJ_BoneGlow_MID_B_EW : SSJ_BoneGlow_MID_A_EW)
{
  effect = SSJ_BoneGlow_C_E;
  constraint = "#effect.SkeletalHand_RT.mount_midB";
};
datablock afxEffectWrapperData(SSJ_BoneGlow_MID_C_EW : SSJ_BoneGlow_MID_A_EW)
{
  effect = SSJ_BoneGlow_D_E;
  constraint = "#effect.SkeletalHand_RT.mount_midC";
};

datablock afxEffectWrapperData(SSJ_BoneGlow_RNG_A_EW : SSJ_BoneGlow_PALM_A_EW)
{
  effect = SSJ_BoneGlow_B_E;
  constraint = "#effect.SkeletalHand_RT.mount_rngA";  
};
datablock afxEffectWrapperData(SSJ_BoneGlow_RNG_B_EW : SSJ_BoneGlow_RNG_A_EW)
{
  effect = SSJ_BoneGlow_C_E;
  constraint = "#effect.SkeletalHand_RT.mount_rngB";
};
datablock afxEffectWrapperData(SSJ_BoneGlow_RNG_C_EW : SSJ_BoneGlow_RNG_A_EW)
{
  effect = SSJ_BoneGlow_D_E;
  constraint = "#effect.SkeletalHand_RT.mount_rngC";
};

datablock afxEffectWrapperData(SSJ_BoneGlow_PNK_A_EW : SSJ_BoneGlow_PALM_A_EW)
{
  effect = SSJ_BoneGlow_C_E;
  constraint = "#effect.SkeletalHand_RT.mount_pnkA";  
};
datablock afxEffectWrapperData(SSJ_BoneGlow_PNK_B_EW : SSJ_BoneGlow_PNK_A_EW)
{
  effect = SSJ_BoneGlow_D_E;
  constraint = "#effect.SkeletalHand_RT.mount_pnkB";
};
datablock afxEffectWrapperData(SSJ_BoneGlow_PNK_C_EW : SSJ_BoneGlow_PNK_A_EW)
{
  effect = SSJ_BoneGlow_D_E;
  constraint = "#effect.SkeletalHand_RT.mount_pnkC";
};

datablock afxEffectWrapperData(SSJ_BoneGlow_THM_A_EW : SSJ_BoneGlow_PALM_A_EW)
{
  effect = SSJ_BoneGlow_A_E;
  constraint = "#effect.SkeletalHand_RT.mount_thmA";  
};
datablock afxEffectWrapperData(SSJ_BoneGlow_THM_B_EW : SSJ_BoneGlow_THM_A_EW)
{
  effect = SSJ_BoneGlow_C_E;
  constraint = "#effect.SkeletalHand_RT.mount_thmB";
};
datablock afxEffectWrapperData(SSJ_BoneGlow_THM_C_EW : SSJ_BoneGlow_THM_A_EW)
{
  effect = SSJ_BoneGlow_D_E;
  constraint = "#effect.SkeletalHand_RT.mount_thmC";
};


datablock afxXM_OscillateData(SSJ_BoneSmoke_oscillate_XM)
{
  mask = $afxXfmMod::ORI;
  axis  = "$$ getRandom() SPC getRandom() SPC 0"; 
  min   = "$$ getRandomF(-80.0, 0.0)";
  max   = "$$ getRandomF(0.0, 80.0)";
  speed = "$$ getRandomF(0.3, 8.0)";
};

datablock afxEffectWrapperData(SSJ_BoneSmoke_THM_A_EW : SSJ_SkeletalHand_RT_EW)
{
  effect = SSJ_HandSmokeA_E;
  posConstraint = "#effect.SkeletalHand_RT.mount_thmA";
  constraint = "";
  effectName = "";
  isConstraintSrc = false;
  
  xfmModifiers[0] = SSJ_BoneSmoke_oscillate_XM;
};
datablock afxEffectWrapperData(SSJ_BoneSmoke_MID_A_EW : SSJ_BoneSmoke_THM_A_EW)
{
  posConstraint = "#effect.SkeletalHand_RT.mount_midA";  
};
datablock afxEffectWrapperData(SSJ_BoneSmoke_PNK_A_EW : SSJ_BoneSmoke_THM_A_EW)
{
  posConstraint = "#effect.SkeletalHand_RT.mount_pnkA";  
};


datablock afxEffectWrapperData(SSJ_BoneSmoke_THM_C_EW : SSJ_BoneSmoke_THM_A_EW)
{
  posConstraint = "#effect.SkeletalHand_RT.mount_thmC";
};
datablock afxEffectWrapperData(SSJ_BoneSmoke_IDX_C_EW : SSJ_BoneSmoke_THM_A_EW)
{
  posConstraint = "#effect.SkeletalHand_RT.mount_idxC";
};
datablock afxEffectWrapperData(SSJ_BoneSmoke_MID_C_EW : SSJ_BoneSmoke_THM_A_EW)
{
  posConstraint = "#effect.SkeletalHand_RT.mount_midC";  
};
datablock afxEffectWrapperData(SSJ_BoneSmoke_RNG_C_EW : SSJ_BoneSmoke_THM_A_EW)
{
  posConstraint = "#effect.SkeletalHand_RT.mount_rngC";
};
datablock afxEffectWrapperData(SSJ_BoneSmoke_PNK_C_EW : SSJ_BoneSmoke_THM_A_EW)
{
  posConstraint = "#effect.SkeletalHand_RT.mount_pnkC";  
};

datablock afxEffectWrapperData(SSJ_BoneSmoke_PALM_B_EW : SSJ_BoneSmoke_THM_A_EW)
{
  posConstraint = "#effect.SkeletalHand_RT.mount_baseB";  
};

//~~~~~~~~~~~~~~~~~~~~//

datablock afxXM_AimData(SSJ_LockOnFlare_Aim_XM)
{
  aimZOnly = true;
};

datablock afxModelData(SSJ_LockOnFlare_CE)
{
  shapeFile = %mySpellDataPath @ "/SSJ/models/ssj_lockOn_flare.dts";
  forceOnMaterialFlags = $MaterialFlags::Additive | $MaterialFlags::SelfIlluminating;
  alphaMult = 1.0; //0.5;
  sequence = "flare";
  sequenceRate = 1.0;
};
//
datablock afxEffectWrapperData(SSJ_LockOnFlare_EW)
{
  effect = SSJ_LockOnFlare_CE;
  posConstraint = target;
  posConstraint2 = camera;
  lifetime = (20/30)-(10/30);
  delay = 1.0+(35/30)+0.2;
  fadeOutTime = 10/30;
  xfmModifiers[0] = SSJ_LockOnFlare_Aim_XM;
};

//~~~~~~~~~~~~~~~~~~~~//

datablock afxAnimClipData(SSJ_LockOnTargetAnim_CE)
{
  clipName = "ssj_lock";
  ignoreCorpse = true;
  rate = 1.0;
  lockAnimation = true;
};
//
datablock afxEffectWrapperData(SSJ_LockOnTargetAnim_EW)
{
  effect = SSJ_LockOnTargetAnim_CE;
  constraint = "target";
  delay    = 1.0+(35/30)+0.2;
  lifetime = 3.3;
};

datablock afxXM_LocalOffsetData(SSJ_LockOn_SkeletalHand_offsetA_XM)
{
  localOffset = "0 0 1.25";
};
datablock afxXM_LocalOffsetData(SSJ_LockOn_SkeletalHand_offsetB_XM)
{
  localOffset = "0 0 0.75";
};
datablock afxXM_LocalOffsetData(SSJ_LockOn_SkeletalHand_offsetC_XM)
{
  localOffset = "0 0 0.25";
};

datablock afxXM_SpinData(SSJ_LockOn_SkeletalHand_spinB_XM)
{
  spinAxis = "0 0 1";
  spinRate = 0;
  spinAngle = 360*0.33;
};
datablock afxXM_SpinData(SSJ_LockOn_SkeletalHand_spinC_XM)
{
  spinAxis = "0 0 1";
  spinRate = 0;
  spinAngle = 360*0.66;
};

$SSJ_LockOn_SkeletalHand_A_delay = 1.0+(35/30)+0.3;
$SSJ_LockOn_SkeletalHand_B_delay = 1.0+(35/30)+0.6;
$SSJ_LockOn_SkeletalHand_C_delay = 1.0+(35/30)+0.75;

$SSJ_LockOn_SkeletalHand_A_lifetime = 40/30;
$SSJ_LockOn_SkeletalHand_B_lifetime = 38/30;
$SSJ_LockOn_SkeletalHand_C_lifetime = 36/30;

$SSJ_LockOn_SkeletalHand_Slice_delay_offset = 28/30;

datablock afxModelData(SSJ_LockOn_SkeletalHand_CE)
{
  shapeFile = %mySpellDataPath @ "/SSJ/models/ssj_lockOn_SkeleHand.dts";
  forceOnMaterialFlags = $MaterialFlags::Additive | $MaterialFlags::SelfIlluminating;
  useVertexAlpha = true; // TGE (ignored by TGEA)
  sequence = "swipe";
};
//
datablock afxEffectWrapperData(SSJ_LockOn_SkeletalHand_A_EW)
{
  effect = SSJ_LockOn_SkeletalHand_CE;
  constraint = target;
  effectName = "LockOn_SkeletalHand_A";
  isConstraintSrc = true;
  lifetime = $SSJ_LockOn_SkeletalHand_A_lifetime-(10/30);
  delay = $SSJ_LockOn_SkeletalHand_A_delay;
  fadeInTime = 10/30;
  fadeOutTime = 10/30;
  
  xfmModifiers[0] = SSJ_LockOn_SkeletalHand_offsetA_XM;
};
datablock afxEffectWrapperData(SSJ_LockOn_SkeletalHand_B_EW : SSJ_LockOn_SkeletalHand_A_EW)
{
  effectName = "LockOn_SkeletalHand_B";
  lifetime = $SSJ_LockOn_SkeletalHand_B_lifetime-(10/30);
  delay = $SSJ_LockOn_SkeletalHand_B_delay;
  xfmModifiers[0] = SSJ_LockOn_SkeletalHand_offsetB_XM;
  xfmModifiers[1] = SSJ_LockOn_SkeletalHand_spinB_XM;
};
datablock afxEffectWrapperData(SSJ_LockOn_SkeletalHand_C_EW : SSJ_LockOn_SkeletalHand_A_EW)
{
  effectName = "LockOn_SkeletalHand_C";
  lifetime = $SSJ_LockOn_SkeletalHand_C_lifetime-(10/30);
  delay = $SSJ_LockOn_SkeletalHand_C_delay;
  xfmModifiers[0] = SSJ_LockOn_SkeletalHand_offsetC_XM;
  xfmModifiers[1] = SSJ_LockOn_SkeletalHand_spinC_XM;
};

datablock ParticleData(SSJ_LockOnHand_Slice_P)
{
  textureName          = %mySpellDataPath @ "/SSJ/particles/ssj_slice";
  dragCoeffiecient     = 0;
  gravityCoefficient   = 0;
  inheritedVelFactor   = 0.00;
  lifetimeMS           = (7/30)*1000;
  lifetimeVarianceMS   = 0;
  spinRandomMin        = 0;
  spinRandomMax        = 0;
  colors[0]            = "1.0 1.0 1.0 0.0";
    colors[1]            = "1.0 1.0 1.0 0.1";
    colors[2]            = "1.0 1.0 1.0 0.1";
    colors[3]            = "1.0 1.0 1.0 0.0";
  sizes[0]             = 0;
    sizes[1]             = 1.4*1.0;
    sizes[2]             = 1.4*1.0;
    sizes[3]             = 0;
  times[0]             = 0.0;
    times[1]             = 0.33;
    times[2]             = 0.66;
    times[3]             = 1.0;
};

datablock afxParticleEmitterVectorData(SSJ_LockOn_SkeletalHand_Slice_E)
{
  ejectionOffset    = 0;
  ejectionPeriodMS  = 2;
  periodVarianceMS  = 0;
  ejectionVelocity  = 0;
  velocityVariance  = 0;
  particles         = "SSJ_LockOnHand_Slice_P";
  blendStyle = "ADDITIVE";
  vector = "0.0 1.0 0.0";
  fadeColor = true;
};

datablock afxEffectWrapperData(SSJ_LockOn_SkeletalHand_Slice_IDX_A_EW)
{
  effect = SSJ_LockOn_SkeletalHand_Slice_E;
  constraint = "#effect.LockOn_SkeletalHand_A.mount_idxC";
  lifetime = $SSJ_LockOn_SkeletalHand_A_lifetime -(3/30) - $SSJ_LockOn_SkeletalHand_Slice_delay_offset;
  delay = $SSJ_LockOn_SkeletalHand_A_delay+$SSJ_LockOn_SkeletalHand_Slice_delay_offset;
  fadeInTime = 3/30;
  fadeOutTime = 3/30;
};
datablock afxEffectWrapperData(SSJ_LockOn_SkeletalHand_Slice_MID_A_EW : SSJ_LockOn_SkeletalHand_Slice_IDX_A_EW)
{
  constraint = "#effect.LockOn_SkeletalHand_A.mount_midC";
};
datablock afxEffectWrapperData(SSJ_LockOn_SkeletalHand_Slice_RNG_A_EW : SSJ_LockOn_SkeletalHand_Slice_IDX_A_EW)
{
  constraint = "#effect.LockOn_SkeletalHand_A.mount_rngC";
};
datablock afxEffectWrapperData(SSJ_LockOn_SkeletalHand_Slice_PNK_A_EW : SSJ_LockOn_SkeletalHand_Slice_IDX_A_EW)
{
  constraint = "#effect.LockOn_SkeletalHand_A.mount_pnkC";
};
datablock afxEffectWrapperData(SSJ_LockOn_SkeletalHand_Slice_THM_A_EW : SSJ_LockOn_SkeletalHand_Slice_IDX_A_EW)
{
  constraint = "#effect.LockOn_SkeletalHand_A.mount_thmC";
};

datablock afxEffectWrapperData(SSJ_LockOn_SkeletalHand_Slice_IDX_B_EW)
{
  effect = SSJ_LockOn_SkeletalHand_Slice_E;
  constraint = "#effect.LockOn_SkeletalHand_B.mount_idxC";
  lifetime = $SSJ_LockOn_SkeletalHand_B_lifetime - (3/30) - $SSJ_LockOn_SkeletalHand_Slice_delay_offset;
  delay = $SSJ_LockOn_SkeletalHand_B_delay+$SSJ_LockOn_SkeletalHand_Slice_delay_offset;
  fadeInTime = 3/30;
  fadeOutTime = 3/30;
};
datablock afxEffectWrapperData(SSJ_LockOn_SkeletalHand_Slice_MID_B_EW : SSJ_LockOn_SkeletalHand_Slice_IDX_B_EW)
{
  constraint = "#effect.LockOn_SkeletalHand_B.mount_midC";
};
datablock afxEffectWrapperData(SSJ_LockOn_SkeletalHand_Slice_RNG_B_EW : SSJ_LockOn_SkeletalHand_Slice_IDX_B_EW)
{
  constraint = "#effect.LockOn_SkeletalHand_B.mount_rngC";
};
datablock afxEffectWrapperData(SSJ_LockOn_SkeletalHand_Slice_PNK_B_EW : SSJ_LockOn_SkeletalHand_Slice_IDX_B_EW)
{
  constraint = "#effect.LockOn_SkeletalHand_B.mount_pnkC";
};
datablock afxEffectWrapperData(SSJ_LockOn_SkeletalHand_Slice_THM_B_EW : SSJ_LockOn_SkeletalHand_Slice_IDX_B_EW)
{
  constraint = "#effect.LockOn_SkeletalHand_B.mount_thmC";
};

datablock afxEffectWrapperData(SSJ_LockOn_SkeletalHand_Slice_IDX_C_EW)
{
  effect = SSJ_LockOn_SkeletalHand_Slice_E;
  constraint = "#effect.LockOn_SkeletalHand_C.mount_idxC";
  lifetime = $SSJ_LockOn_SkeletalHand_C_lifetime -(3/30) - $SSJ_LockOn_SkeletalHand_Slice_delay_offset;
  delay = $SSJ_LockOn_SkeletalHand_C_delay+$SSJ_LockOn_SkeletalHand_Slice_delay_offset;
  fadeInTime = 3/30;
  fadeOutTime = 3/30;
};
datablock afxEffectWrapperData(SSJ_LockOn_SkeletalHand_Slice_MID_C_EW : SSJ_LockOn_SkeletalHand_Slice_IDX_C_EW)
{
  constraint = "#effect.LockOn_SkeletalHand_C.mount_midC";
};
datablock afxEffectWrapperData(SSJ_LockOn_SkeletalHand_Slice_RNG_C_EW : SSJ_LockOn_SkeletalHand_Slice_IDX_C_EW)
{
  constraint = "#effect.LockOn_SkeletalHand_C.mount_rngC";
};
datablock afxEffectWrapperData(SSJ_LockOn_SkeletalHand_Slice_PNK_C_EW : SSJ_LockOn_SkeletalHand_Slice_IDX_C_EW)
{
  constraint = "#effect.LockOn_SkeletalHand_C.mount_pnkC";
};
datablock afxEffectWrapperData(SSJ_LockOn_SkeletalHand_Slice_THM_C_EW : SSJ_LockOn_SkeletalHand_Slice_IDX_C_EW)
{
  constraint = "#effect.LockOn_SkeletalHand_C.mount_thmC";
};

datablock ParticleData(SSJ_LockOnHand_SliceGlow_P)
{
  textureName          = %mySpellDataPath @ "/SSJ/particles/ssj_hand_slice_glow";
  dragCoeffiecient     = 0;
  gravityCoefficient   = 0;
  inheritedVelFactor   = 0.00;
  lifetimeMS           = (7/30)*1000;
  lifetimeVarianceMS   = 0;
  spinRandomMin        = 0;
  spinRandomMax        = 0;
  colors[0]            = "0.0 0.0 0.0 0.0"; 
    colors[1]            = "0.2 0.2 0.2 0.2";
    colors[2]            = "0.2 0.2 0.2 0.2";
    colors[3]            = "0.0 0.0 0.0 0.0";
  sizes[0]             = 2.0*3.0;
    sizes[1]             = 3.0*3.0;
    sizes[2]             = 3.0*3.0;
    sizes[3]             = 2.0*3.0;
  times[0]             = 0.0;
    times[1]             = 0.2;
    times[2]             = 0.8;
    times[3]             = 1.0;
};

datablock ParticleEmitterData(SSJ_LockOn_SkeletalHand_SliceGlow_E)
{
  ejectionOffset    = 0;
  ejectionPeriodMS  = 5;
  periodVarianceMS  = 0;
  ejectionVelocity  = 1.1;
  velocityVariance  = 1;
  particles         = "SSJ_LockOnHand_SliceGlow_P";
  blendStyle = "ADDITIVE";
  fadeColor = true;
};

datablock afxEffectWrapperData(SSJ_LockOn_SkeletalHand_SliceGlow_A_EW)
{
  effect = SSJ_LockOn_SkeletalHand_SliceGlow_E;
  constraint = "#effect.LockOn_SkeletalHand_A.mount_midA";
  
  lifetime = $SSJ_LockOn_SkeletalHand_A_lifetime-(10/30);
  delay = $SSJ_LockOn_SkeletalHand_A_delay;
  fadeInTime = 10/30;
  fadeOutTime = 10/30;
};
datablock afxEffectWrapperData(SSJ_LockOn_SkeletalHand_SliceGlow_B_EW : SSJ_LockOn_SkeletalHand_SliceGlow_A_EW)
{
  effect = SSJ_LockOn_SkeletalHand_SliceGlow_E;
  constraint = "#effect.LockOn_SkeletalHand_B.mount_midA";
  
  lifetime = $SSJ_LockOn_SkeletalHand_B_lifetime-(10/30);
  delay = $SSJ_LockOn_SkeletalHand_B_delay;  
};
datablock afxEffectWrapperData(SSJ_LockOn_SkeletalHand_SliceGlow_C_EW : SSJ_LockOn_SkeletalHand_SliceGlow_A_EW)
{
  effect = SSJ_LockOn_SkeletalHand_SliceGlow_E;
  constraint = "#effect.LockOn_SkeletalHand_C.mount_midA";
  
  lifetime = $SSJ_LockOn_SkeletalHand_C_lifetime-(10/30);
  delay = $SSJ_LockOn_SkeletalHand_C_delay;  
};

//
// The other important attribute used here is hover.  Hover
// influences the missiles guidance system, allowing it to hover at a
// specified altitude (hoverAltitude) until the missile is within a
// certain distance to the target (hoverAttackDistance).  Once there,
// it hovers dramatically for an additional time (hoverTime in ms)
// before plunging down to strike the target.  A gradient is provided
// (hoverAttackGradient) allowing the missile to ease-in and out as
// it enters the attacking range.
//

datablock afxMagicMissileData(SSJ_SoulMissile)
{
  muzzleVelocity        = 9;
  velInheritFactor      = 0;
  lifetime              = 100000;
  isBallistic           = true;
  ballisticCoefficient  = 1.0;
  gravityMod            = 0.05;
  isGuided              = true;
  precision             = 30;
  trackDelay            = 7;
  hasLight    = false;
  launchOffset = "0.0 0.0 2.0";
  launchAimPitch = -70.0;
  reverseTargeting = true;
  hoverAltitude       = 20;
  hoverAttackDistance = 15;
  hoverAttackGradient = 10;
  hoverTime           = 0;
  wiggleAxis       = "0 0 1" SPC "0 1 0";
  wiggleMagnitudes = "0.2 0.3";
  wiggleSpeeds     = "2.5 1.5";
};

datablock afxZodiacPlaneData(SSJ_SoulRingPlaneA_CE)
{
  texture = %mySpellDataPath @ "/SSJ/zodiacs/ssj_soul_ring";
  
  rotationRate = 180;
  doubleSided = true;
  radius = 0.75;
  blend = "additive";
  faceDir = "forward";
  useFullTransform = true;
};
datablock afxZodiacPlaneData(SSJ_SoulRingPlaneB_CE : SSJ_SoulRingPlaneA_CE)
{
  rotationRate = -180;
};

datablock afxXM_AimData(SSJ_SoulRing_Aim_XM)
{
  aimZOnly = false;
};
// intersection planes are causing rendering artifacts
datablock afxXM_LocalOffsetData(SSJ_SoulRing_offset_XM)
{
  localOffset = "0 0.01 0";
};

datablock afxEffectWrapperData(SSJ_SoulCircleA_EW)
{
  effect = SSJ_SoulRingPlaneA_CE;
  posConstraint = missile;
  posConstraint2 = camera;  // caster
  delay = 0;
  fadeInTime = 0.5;
  fadeOutTime = 0.5;
  xfmModifiers[0] = SSJ_SoulRing_Aim_XM;
};
datablock afxEffectWrapperData(SSJ_SoulCircleB_EW : SSJ_SoulCircleA_EW)
{
  effect = SSJ_SoulRingPlaneB_CE;
  xfmModifiers[1] = SSJ_SoulRing_offset_XM;
};

$SSJ_Soul_Scale = 0.5; // for EG

datablock ParticleData(SSJ_SoulRay_A_P)
{
  textureName          = %mySpellDataPath @ "/SSJ/particles/ssj_soul_ray";
  dragCoeffiecient     = 0;
  windCoefficient      = 0;
  gravityCoefficient   = 0; 
  inheritedVelFactor   = 0.00;
  lifetimeMS           = 1.0*1000;
  lifetimeVarianceMS   = 0;
  spinRandomMin        = 0;
  spinRandomMax        = 0;
  colors[0]            = "1.0 1.0 1.0 0.0";
    colors[1]            = "1.0 1.0 1.0 1.0"; 
    colors[2]            = "1.0 1.0 1.0 1.0";
    colors[3]            = "1.0 1.0 1.0 0.0";
  sizes[0]             = 0.0;
    sizes[1]             = 1*1.5 *$SSJ_Soul_Scale;
    sizes[2]             = 2*1.5 *$SSJ_Soul_Scale;
    sizes[3]             = 3*1.5 *$SSJ_Soul_Scale;
  times[0]             = 0.0;
    times[1]             = 0.33;
    times[2]             = 0.66;
    times[3]             = 1.0;
  constrainPos         = true;
};
//
datablock afxParticleEmitterConeData(SSJ_SoulRays_E)
{
  vector      = "0 1 0"; 
  spreadMin   = 180.0;
  spreadMax   = 180.0;
  ejectionPeriodMS = 10;
  periodVarianceMS = 1;
  ejectionVelocity = 2 *$SSJ_Soul_Scale;
  velocityVariance = 0;  
  particles        = "SSJ_SoulRay_A_P";
  blendStyle = "ADDITIVE";  
  fadeColor = true;
  ejectionOffset = 0.4;
  orientParticles = true;
};

datablock afxEffectWrapperData(SSJ_SoulRays_EW)
{
  effect = SSJ_SoulRays_E;
  posConstraint = missile;
  posConstraint2 = camera;
  delay = 0;
  fadeInTime = 0.5;
  fadeOutTime = 0.5;
  xfmModifiers[0] = SSJ_SoulRing_Aim_XM;
};

//~~~~~~~~~~~~~~~~~~~~//

datablock ParticleData(SSJ_SoulStar_A_P)
{
  textureName          = %mySpellDataPath @ "/SSJ/particles/ssj_soul_star_2x2";
  textureCoords[0] = "0.0 0.0";
    textureCoords[1] = "0.0 0.5";
    textureCoords[2] = "0.5 0.5";
    textureCoords[3] = "0.5 0.0";
  dragCoeffiecient     = 0;
  gravityCoefficient   = 0;
  inheritedVelFactor   = 0.00;
  lifetimeMS           = (5/30)*1000;
  lifetimeVarianceMS   = (2/30);
  spinRandomMin        = 0;
  spinRandomMax        = 0;
  colors[0]            = "1.0 1.0 1.0 0.0";
    colors[1]            = "1.0 1.0 1.0 1.0"; 
    colors[2]            = "1.0 1.0 1.0 1.0";
    colors[3]            = "1.0 1.0 1.0 0.0";
  sizes[0]             = 2.0;
    sizes[1]             = 2.0;
    sizes[2]             = 2.0;
    sizes[3]             = 2.0;
  times[0]             = 0.0;
    times[1]             = 0.33;
    times[2]             = 0.66;
    times[3]             = 1.0;
};
datablock ParticleData(SSJ_SoulStar_B_P : SSJ_SoulStar_A_P)
{
  textureCoords[0] = "0.5 0.0";
    textureCoords[1] = "0.5 0.5";
    textureCoords[2] = "1.0 0.5";
    textureCoords[3] = "1.0 0.0";
};
datablock ParticleData(SSJ_SoulStar_C_P : SSJ_SoulStar_A_P)
{
  textureCoords[0] = "0.0 0.5";
    textureCoords[1] = "0.0 1.0";
    textureCoords[2] = "0.5 1.0";
    textureCoords[3] = "0.5 0.5";
  sizes[1]             = 1.0*0.5;
  sizes[2]             = 1.0*0.5;
};

datablock afxPathData(SSJ_SoulCircle_A_Path)
{
  points = "0 0 4" SPC
           "-1.381612513 0 3.930482489" SPC
           "-3.043366229 0 2.760841557" SPC
           "-4.023796027 0 1" SPC
           "-4.023796027 0 -1" SPC
           "-3.043366229 0 -2.760841557" SPC
           "-1.381612513 0 -3.930482489" SPC
           "0 0 -4";
  mult = 0.25;  
};
datablock afxPathData(SSJ_SoulCircle_B_Path)
{
  points = "0 0 4" SPC
           "1.381612513 0 3.930482489" SPC
           "3.043366229 0 2.760841557" SPC
           "4.023796027 0 1" SPC
           "4.023796027 0 -1" SPC
           "3.043366229 0 -2.760841557" SPC
           "1.381612513 0 -3.930482489" SPC
           "0 0 -4";
  mult = 0.25;  
};

datablock afxXM_LocalOffsetData(tmp_offset_XM)
{
  localOffset = "0 0 4";
};

datablock afxParticleEmitterPathData(SSJ_SoulCircle_E)
{
  ejectionPeriodMS = 10;
  periodVarianceMS = 3;
  ejectionVelocity = 2.5; 
  velocityVariance = 0.75;
  particles        = "SSJ_SoulStar_A_P SSJ_SoulStar_B_P SSJ_SoulStar_C_P";
  pathOrigin  = "origin"; // origin point vector tangent
  paths = "SSJ_SoulCircle_A_Path SSJ_SoulCircle_B_Path";
  fadeAlpha = true;
};

datablock afxEffectWrapperData(SSJ_SoulCircleSparkles_EW)
{
  effect = SSJ_SoulCircle_E;
  posConstraint = missile;
  orientConstraint = caster;
  delay = 0;
  fadeInTime = 0.5;
  fadeOutTime = 0.5;
};


$SSJ_Wing_FlapSpeed = 0.66; //1.0;
datablock afxModelData(SSJ_Wing_SkeletalHand_RT_CE)
{
  shapeFile = %mySpellDataPath @ "/SSJ/models/ssj_wingHand_RT.dts";
  forceOnMaterialFlags = $MaterialFlags::Additive | $MaterialFlags::SelfIlluminating;
  useVertexAlpha = true; // TGE (ignored by TGEA)
  sequence = "flap";  
  sequenceRate = 2.0*$SSJ_Wing_FlapSpeed;
};
datablock afxModelData(SSJ_Wing_SkeletalHand_LF_CE : SSJ_Wing_SkeletalHand_RT_CE)
{
  shapeFile = %mySpellDataPath @ "/SSJ/models/ssj_wingHand_LF.dts";
};

datablock afxXM_SpinData(SSJ_Wing_SkeletalHand_spin_XM)
{
  spinAxis = "0 0 1";
  spinRate = 0;
  spinAngle = 90;
};

$SSJ_Wing_SkeletalHands_seperation = 2.0;
datablock afxXM_LocalOffsetData(SSJ_Wing_SkeletalHand_offset1_XM)
{
  localOffset = "0" SPC ($SSJ_Wing_SkeletalHands_seperation*0.5) SPC "0";
  fadeInTime = 1.0;
};
datablock afxXM_LocalOffsetData(SSJ_Wing_SkeletalHand_offset2_XM)
{
  localOffset = "0" SPC ($SSJ_Wing_SkeletalHands_seperation*-0.5) SPC "0";
  fadeInTime = 1.0;
};

datablock afxXM_ScaleData(SSJ_Wing_SkeletalHand_scale_XM)
{
  scale = "-1 -1 -1";
  lifetime = 0;
  fadeOutTime = 1.0;
};

datablock afxXM_WaveScalarData(SSJ_Wing_SkeletalHand_oscillate_XM)
{
  a = -1.0;
  b = 2.0;
  parameter = "pos";
  op = "add";
  axis = "0 0 1";
  waveform = "sine";
  speed = 1.0*$SSJ_Wing_FlapSpeed;
  fadeInTime = 1.0;
};

datablock afxEffectWrapperData(SSJ_Wing_SkeletalHand_RT_EW)
{
  effect = SSJ_Wing_SkeletalHand_RT_CE;
  constraint = missile;
  effectName = "Wing_SkeletalHand_RT";
  isConstraintSrc = true;
  delay = 0;
  fadeInTime = 1.0;
  fadeOutTime = 0.5;
  scaleFactor = 1.4;
  xfmModifiers[0] = SSJ_Wing_SkeletalHand_spin_XM;
    xfmModifiers[1] = SSJ_Wing_SkeletalHand_offset1_XM;
    xfmModifiers[2] = SSJ_Wing_SkeletalHand_oscillate_XM;
    xfmModifiers[3] = SSJ_Wing_SkeletalHand_scale_XM;
};
datablock afxEffectWrapperData(SSJ_Wing_SkeletalHand_LF_EW : SSJ_Wing_SkeletalHand_RT_EW)
{
  effect = SSJ_Wing_SkeletalHand_LF_CE;
  effectName = "Wing_SkeletalHand_LF";
  xfmModifiers[1] = SSJ_Wing_SkeletalHand_offset2_XM;
};

datablock ParticleData(SSJ_Wing_Slice_P : SSJ_LockOnHand_Slice_P)
{
  lifetimeMS           = (30/30)*1000;
  lifetimeVarianceMS   = 0;
  colors[0]            = "1.0 1.0 1.0 0.1";
    colors[1]            = "0.3 0.3 0.3 0.05";
    colors[2]            = "0.1 0.1 0.1 0.02";
    colors[3]            = "0.0 0.0 0.0 0.0";
};
datablock afxParticleEmitterVectorData(SSJ_Wing_SkeletalHand_Slice_E : SSJ_LockOn_SkeletalHand_Slice_E)
{
  particles = "SSJ_Wing_Slice_P";
  fadeAlpha = true;
};

datablock afxEffectWrapperData(SSJ_Wing_SkeletalHand_Slice_IDX_RT_EW)
{
  effect = SSJ_Wing_SkeletalHand_Slice_E;
  constraint = "#effect.Wing_SkeletalHand_RT.mount_idxC";
  delay = 0;
  fadeInTime = 1.0;
  fadeOutTime = 0.5;
};
datablock afxEffectWrapperData(SSJ_Wing_SkeletalHand_Slice_MID_RT_EW : SSJ_Wing_SkeletalHand_Slice_IDX_RT_EW)
{
  constraint = "#effect.Wing_SkeletalHand_RT.mount_midC";
};
datablock afxEffectWrapperData(SSJ_Wing_SkeletalHand_Slice_RNG_RT_EW : SSJ_Wing_SkeletalHand_Slice_IDX_RT_EW)
{
  constraint = "#effect.Wing_SkeletalHand_RT.mount_rngC";
};
datablock afxEffectWrapperData(SSJ_Wing_SkeletalHand_Slice_PNK_RT_EW : SSJ_Wing_SkeletalHand_Slice_IDX_RT_EW)
{
  constraint = "#effect.Wing_SkeletalHand_RT.mount_pnkC";
};

datablock afxEffectWrapperData(SSJ_Wing_SkeletalHand_Slice_IDX_LF_EW)
{
  effect = SSJ_Wing_SkeletalHand_Slice_E;
  constraint = "#effect.Wing_SkeletalHand_LF.mount_idxC";
  delay = 0;
  fadeInTime = 0.5;
  fadeOutTime = 0.5;
};
datablock afxEffectWrapperData(SSJ_Wing_SkeletalHand_Slice_MID_LF_EW : SSJ_Wing_SkeletalHand_Slice_IDX_LF_EW)
{
  constraint = "#effect.Wing_SkeletalHand_LF.mount_midC";
};
datablock afxEffectWrapperData(SSJ_Wing_SkeletalHand_Slice_RNG_LF_EW : SSJ_Wing_SkeletalHand_Slice_IDX_LF_EW)
{
  constraint = "#effect.Wing_SkeletalHand_LF.mount_rngC";
};
datablock afxEffectWrapperData(SSJ_Wing_SkeletalHand_Slice_PNK_LF_EW : SSJ_Wing_SkeletalHand_Slice_IDX_LF_EW)
{
  constraint = "#effect.Wing_SkeletalHand_LF.mount_pnkC";
};

datablock ParticleData(SSJ_Wing_HandBoneSmoke_P : SSJ_HandBoneSmokeA_P)
{  
  inheritedVelFactor   = 1.00;
  lifetimeMS           = 2000*0.5;
  lifetimeVarianceMS   = 200*0.5;
  colors[0]            = "0.15 0.15 0.15 0.15";
    colors[1]            = "0.05 0.05 0.05 0.05";
    colors[2]            = "0.02 0.02 0.02 0.02";
    colors[3]            = "0.0 0.0 0.0 0.0";  
  sizes[0]             = 0.2;
    sizes[1]             = 0.7*1.5*3;
    sizes[2]             = 1.3*2*3;
    sizes[3]             = 2.0*2*3;
};
datablock afxParticleEmitterVectorData(SSJ_Wing_HandSmokeA_E : SSJ_HandSmokeA_E)
{
  ejectionPeriodMS = 80;
  periodVarianceMS = 40;
  ejectionVelocity = 2.0;
  velocityVariance = 2.0;
  particles        = SSJ_Wing_HandBoneSmoke_P;
};

datablock afxEffectWrapperData(SSJ_Wing_BoneSmoke_THM_RT_EW)
{
  effect = SSJ_Wing_HandSmokeA_E;
  posConstraint = "#effect.Wing_SkeletalHand_RT.mount_thmA";
  fadeInTime = 1.0;
  fadeOutTime = 1.0;
  xfmModifiers[0] = SSJ_BoneSmoke_oscillate_XM;
};
datablock afxEffectWrapperData(SSJ_Wing_BoneSmoke_IDX_RT_EW : SSJ_Wing_BoneSmoke_THM_RT_EW)
{
  posConstraint = "#effect.Wing_SkeletalHand_RT.mount_idxA";
};
datablock afxEffectWrapperData(SSJ_Wing_BoneSmoke_MID_RT_EW : SSJ_Wing_BoneSmoke_THM_RT_EW)
{
  posConstraint = "#effect.Wing_SkeletalHand_RT.mount_midA";
};
datablock afxEffectWrapperData(SSJ_Wing_BoneSmoke_RNG_RT_EW : SSJ_Wing_BoneSmoke_THM_RT_EW)
{
  posConstraint = "#effect.Wing_SkeletalHand_RT.mount_rngA";
};
datablock afxEffectWrapperData(SSJ_Wing_BoneSmoke_PNK_RT_EW : SSJ_Wing_BoneSmoke_THM_RT_EW)
{
  posConstraint = "#effect.Wing_SkeletalHand_RT.mount_pnkA";
};

datablock afxEffectWrapperData(SSJ_Wing_BoneSmoke_THM_LF_EW : SSJ_Wing_BoneSmoke_THM_RT_EW)
{
  posConstraint = "#effect.Wing_SkeletalHand_LF.mount_thmA";
};
datablock afxEffectWrapperData(SSJ_Wing_BoneSmoke_IDX_LF_EW : SSJ_Wing_BoneSmoke_THM_LF_EW)
{
  posConstraint = "#effect.Wing_SkeletalHand_LF.mount_idxA";
};
datablock afxEffectWrapperData(SSJ_Wing_BoneSmoke_MID_LF_EW : SSJ_Wing_BoneSmoke_THM_LF_EW)
{
  posConstraint = "#effect.Wing_SkeletalHand_LF.mount_midA";
};
datablock afxEffectWrapperData(SSJ_Wing_BoneSmoke_RNG_LF_EW : SSJ_Wing_BoneSmoke_THM_LF_EW)
{
  posConstraint = "#effect.Wing_SkeletalHand_LF.mount_rngA";
};
datablock afxEffectWrapperData(SSJ_Wing_BoneSmoke_PNK_LF_EW : SSJ_Wing_BoneSmoke_THM_LF_EW)
{
  posConstraint = "#effect.Wing_SkeletalHand_LF.mount_pnkA";
};

datablock afxEffectWrapperData(SSJ_Wing_SkeletalHand_Glow_RT_EW)
{
  effect = SSJ_LockOn_SkeletalHand_SliceGlow_E;
  constraint = "#effect.Wing_SkeletalHand_RT.mount_midA";
  fadeInTime = 1.0;
  fadeOutTime = 0.5;
};
datablock afxEffectWrapperData(SSJ_Wing_SkeletalHand_Glow_LF_EW : SSJ_Wing_SkeletalHand_Glow_RT_EW)
{
  constraint = "#effect.Wing_SkeletalHand_LF.mount_midA";
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// AUDIO AND LIGHTING

switch$ (afxGetEngine())
{
  case "TGE":
    exec("./lighting/ssj_lighting_tge_sub.cs");
    exec("./audio/ssj_audio_sub.cs");
  case "TGEA":
    exec("./lighting/ssj_lighting_tgea_sub.cs");
    exec("./audio/ssj_audio_sub.cs");
 case "T3D":
    exec("./lighting/ssj_lighting_t3d_sub.cs");
    exec("./audio/ssj_audio_sub.cs");
}

datablock afxEffectWrapperData(SSJ_SoulRays_Catch_EW)
{
  effect = SSJ_SoulRays_E;
  posConstraint = "caster.Mount0";
  posConstraint2 = camera;
  lifetime = 1.25;
  delay = 0;
  fadeInTime = 0.25;
  fadeOutTime = 0.25;
  xfmModifiers[0] = SSJ_SoulRing_Aim_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// effectron
datablock afxEffectronData(SSJ_SkeletalHandEffectron)
{
  duration = $AFX::INFINITE_TIME;

  addEffect = SSJ_SkeletalHand_RT_EW; 

  addEffect = SSJ_SkeletalHand_clip1_EW;
  addEffect = SSJ_SkeletalHand_clip2_EW;
  addEffect = SSJ_SkeletalHand_clip3_phrase_effect_EW;

  addEffect = SSJ_BoneGlow_PALM_A_EW;
  addEffect = SSJ_BoneGlow_PALM_B_EW;
  addEffect = SSJ_BoneGlow_PALM_C_EW;
  addEffect = SSJ_BoneGlow_PALM_D_EW;
  
  addEffect = SSJ_BoneGlow_IDX_A_EW;
  addEffect = SSJ_BoneGlow_IDX_B_EW;
  addEffect = SSJ_BoneGlow_IDX_C_EW;
  addEffect = SSJ_BoneGlow_MID_A_EW;
  addEffect = SSJ_BoneGlow_MID_B_EW;
  addEffect = SSJ_BoneGlow_MID_C_EW;
  addEffect = SSJ_BoneGlow_RNG_A_EW;
  addEffect = SSJ_BoneGlow_RNG_B_EW;
  addEffect = SSJ_BoneGlow_RNG_C_EW;
  addEffect = SSJ_BoneGlow_PNK_A_EW;
  addEffect = SSJ_BoneGlow_PNK_B_EW;
  addEffect = SSJ_BoneGlow_PNK_C_EW;
  addEffect = SSJ_BoneGlow_THM_A_EW;
  addEffect = SSJ_BoneGlow_THM_B_EW;
  addEffect = SSJ_BoneGlow_THM_C_EW;

  addEffect = SSJ_BoneSmoke_THM_A_EW;
  addEffect = SSJ_BoneSmoke_IDX_C_EW;
  addEffect = SSJ_BoneSmoke_MID_A_EW;
  addEffect = SSJ_BoneSmoke_RNG_C_EW;
  addEffect = SSJ_BoneSmoke_PNK_A_EW;
  addEffect = SSJ_BoneSmoke_THM_C_EW;
  addEffect = SSJ_BoneSmoke_MID_C_EW;
  addEffect = SSJ_BoneSmoke_PNK_C_EW;
  addEffect = SSJ_BoneSmoke_PALM_B_EW;
};

SSJ_add_hand_Lighting_FX(SSJ_SkeletalHandEffectron);
SSJ_add_hand_Audio_FX(SSJ_SkeletalHandEffectron);

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// spell

datablock afxMagicSpellData(SoulSuckingJerkSpell)
{
  castingDur = 4.0; 
  allowMovementInterrupts = false;

    // magic missile //
  missile = SSJ_SoulMissile;

  addCastingEffect = SSJ_AnimLock_EW;
  addCastingEffect = SSJ_Casting_clip_EW;
  addCastingEffect = SSJ_LockOnFlare_EW;
  addCastingEffect = SSJ_LockOnTargetAnim_EW;
  addCastingEffect = SSJ_LockOn_SkeletalHand_A_EW;
  addCastingEffect = SSJ_LockOn_SkeletalHand_B_EW;
  addCastingEffect = SSJ_LockOn_SkeletalHand_C_EW;
  addCastingEffect = SSJ_LockOn_SkeletalHand_Slice_IDX_A_EW;
  addCastingEffect = SSJ_LockOn_SkeletalHand_Slice_MID_A_EW;
  addCastingEffect = SSJ_LockOn_SkeletalHand_Slice_RNG_A_EW;
  addCastingEffect = SSJ_LockOn_SkeletalHand_Slice_PNK_A_EW;
  addCastingEffect = SSJ_LockOn_SkeletalHand_Slice_THM_A_EW;
  addCastingEffect = SSJ_LockOn_SkeletalHand_Slice_IDX_B_EW;
  addCastingEffect = SSJ_LockOn_SkeletalHand_Slice_MID_B_EW;
  addCastingEffect = SSJ_LockOn_SkeletalHand_Slice_RNG_B_EW;
  addCastingEffect = SSJ_LockOn_SkeletalHand_Slice_PNK_B_EW;
  addCastingEffect = SSJ_LockOn_SkeletalHand_Slice_THM_B_EW;
  addCastingEffect = SSJ_LockOn_SkeletalHand_Slice_IDX_C_EW;
  addCastingEffect = SSJ_LockOn_SkeletalHand_Slice_MID_C_EW;
  addCastingEffect = SSJ_LockOn_SkeletalHand_Slice_RNG_C_EW;
  addCastingEffect = SSJ_LockOn_SkeletalHand_Slice_PNK_C_EW;
  addCastingEffect = SSJ_LockOn_SkeletalHand_Slice_THM_C_EW;
  addCastingEffect = SSJ_LockOn_SkeletalHand_SliceGlow_A_EW;
  addCastingEffect = SSJ_LockOn_SkeletalHand_SliceGlow_B_EW;
  addCastingEffect = SSJ_LockOn_SkeletalHand_SliceGlow_C_EW;
  
  addDeliveryEffect = SSJ_CastingHold_clip_EW;
  addDeliveryEffect = SSJ_SoulCircleA_EW;
  addDeliveryEffect = SSJ_SoulCircleB_EW;
  addDeliveryEffect = SSJ_SoulRays_EW;
  addDeliveryEffect = SSJ_Wing_SkeletalHand_RT_EW;
  addDeliveryEffect = SSJ_Wing_SkeletalHand_LF_EW;
  addDeliveryEffect = SSJ_Wing_SkeletalHand_Slice_IDX_RT_EW;
  addDeliveryEffect = SSJ_Wing_SkeletalHand_Slice_MID_RT_EW;
  addDeliveryEffect = SSJ_Wing_SkeletalHand_Slice_RNG_RT_EW;
  addDeliveryEffect = SSJ_Wing_SkeletalHand_Slice_PNK_RT_EW;
  addDeliveryEffect = SSJ_Wing_SkeletalHand_Slice_IDX_LF_EW;
  addDeliveryEffect = SSJ_Wing_SkeletalHand_Slice_MID_LF_EW;
  addDeliveryEffect = SSJ_Wing_SkeletalHand_Slice_RNG_LF_EW;
  addDeliveryEffect = SSJ_Wing_SkeletalHand_Slice_PNK_LF_EW;
  addDeliveryEffect = SSJ_Wing_SkeletalHand_Glow_RT_EW;
  addDeliveryEffect = SSJ_Wing_SkeletalHand_Glow_LF_EW;
  
  addImpactEffect = SSJ_CastingGrab_clip_EW;
  addImpactEffect = SSJ_SoulRays_Catch_EW;
};

// sounds and lights added via sub-script functions //
SSJ_add_Lighting_FX(SoulSuckingJerkSpell);
SSJ_add_Audio_FX(SoulSuckingJerkSpell);

datablock afxRPGMagicSpellData(SoulSuckingJerkSpell_RPG)
{
  spellName = "Soul Sucking Jerk";
  desc = "Your foe has too much Soul, so yank out some of his spare Soul Substance, a useful reagent.\n" @
         "\n" @
         "<font:Arial Italic:14>spell design: <font:Arial:14>Matthew Durante\n" @
         "<font:Arial Italic:14>sound design: <font:Arial:14>Matt Pacyga\n" @ 
         "<font:Arial Italic:14>spell concept: <font:Arial:14>Jeff Faust";
  sourcePack = "Spell Pack 2";
  iconBitmap = %mySpellDataPath @ "/SSJ/icons/ssj";
  target = "enemy";
  canTargetSelf = false;
  manaCost = 10;
  castingDur = SoulSuckingJerkSpell.castingDur;
 
  _rayCnt = 4;
};

function SoulSuckingJerkSpell::readyToCast(%this, %caster, %target)
{
  if (!Parent::readyToCast(%this, %caster, %target))
    return false;

  // check if caster already has the skeletal-hand effectron attached,
  // if so, abort the spell cast.
  if (isObject(%caster) && isObject(%caster.ssj_skeletalHand_fx))
  {
    if (isObject(%caster.client))
      DisplayScreenMessage(%caster.client, "You're unable to concentrate.");
    return false;
  }

  return true;
}

function SoulSuckingJerkSpell::onActivate(%this, %spell, %caster, %target)
{
  Parent::onActivate(%this, %spell, %caster, %target);

  // startup skeletal-hand effectron on the caster
  if (isObject(%caster) && !isObject(%caster.ssj_skeletalHand_fx))
    %caster.ssj_skeletalHand_fx = startEffectron(SSJ_SkeletalHandEffectron, %caster, "caster");
}

// The onImpact() will happen when the flying soul hits something...
// If it's the caster, consider it a catch, otherwise a miss.
// Given this logic, you cannot catch a soul belonging to someone else's SSJ spell.
function SoulSuckingJerkSpell::onImpact(%this, %spell, %caster, %impObj, %impPos, %impNorm)
{
  Parent::onImpact(%this, %spell, %caster, %impObj, %impPos, %impNorm);

  // display success or failure message to user's HUD
  if (isObject(%caster.client))
  {
    if (%impObj == %caster)
      DisplayScreenMessage(%caster.client, "You've collected soul substance.");
    else
      DisplayScreenMessage(%caster.client, "You missed.");
  }  
  
  // trigger grasping hand clip by setting a choreographer trigger and
  // schedule removal of hand effect
  if (isObject(%caster) && isObject(%caster.ssj_skeletalHand_fx))
  {
    %caster.ssj_skeletalHand_fx.setTriggerBit(23); // see SSJ_SkeletalHand_clip3_phrase_effect_CE.triggerMask
    %caster.ssj_skeletalHand_fx.schedule(3000, "interrupt");
  }

  // clear the hand effectron reference
  %caster.ssj_skeletalHand_fx = "";
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
  SoulSuckingJerkSpell.scriptFile = $afxAutoloadScriptFile;
  SoulSuckingJerkSpell.spellDataPath = %mySpellDataPath;
  if (isFunction(addDemoSpellbookSpell))
    addDemoSpellbookSpell(SoulSuckingJerkSpell, SoulSuckingJerkSpell_RPG);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//


