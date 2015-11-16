//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// BOLT FROM THE BLUE
//
//  Crack the sky and smite an unsuspecting foe with a frightening thunderbolt
//  of lethal energy. Galileo figaro-magnifico!
//
//    parameters:
//      _scale          (float)       default=1.0     clamped:(0.2,1.0)
//      _castDur        (float)       default=0.0 
//      _scorchedEarth  (boolean)     default=false
//      _targeted       (boolean)     default=false
//      _casterAnchor   (boolean)     default=false
//  
//    internal:
//      _skyboltsLife
//      _skyboltsGlow.angle[##]
//      _skyboltsGlow.start[##]
//      _skyboltsGlow.end[##]
//
//    non-default BFB settings:
//      _castDur        0.5
//      _scorchedEarth  true
//      _targeted       true
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
$spell_reload = isObject(BoltFromTheBlueSpell);
if ($spell_reload)
{
  // mark datablocks so we can detect which are reloaded this script
  markDataBlocks();
  // reset data path from previously saved value
  %mySpellDataPath = BoltFromTheBlueSpell.spellDataPath;
}
else
{
  // set data path from default plus containing folder name
  %mySpellDataPath = $afxSpellDataPath @ "/" @ $afxAutoloadScriptFolder;
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// SKY LIGHTNING BALL (particles)
//  A ball of pulsing energy at the source of the lightning bolt.
//

datablock ParticleData(BFB_SkyLightningBall_A_P)
{
  textureName          = %mySpellDataPath @ "/BFB/particles/bfb_lightning_ball_2x2";
  textureCoords[0] = "0.0 0.0";
    textureCoords[1] = "0.0 0.5";
    textureCoords[2] = "0.5 0.5";
    textureCoords[3] = "0.5 0.0";
  dragCoeffiecient     = 0;
  gravityCoefficient   = 0;
  inheritedVelFactor   = 0.00;
  lifetimeMS           = 150;
  lifetimeVarianceMS   = 50;
  spinSpeed            = 0;
  spinRandomMin        = 0;
  spinRandomMax        = 0;
  angle                = 0;
  angleVariance        = 180;
  colors[0]            = "0.0 0.0 0.0 0.0";
    colors[1]            = "1.0 1.0 1.0 0.8";
    colors[2]            = "1.0 1.0 1.0 0.8";
    colors[3]            = "0.0 0.0 0.0 0.0";
  sizeBias             = "$$ 1.0 + %%._scale";
  sizes[0]             = 0;
    sizes[1]             = 6;
    sizes[2]             = 3;
    sizes[3]             = 0;
  times[0]             = 0.0;
    times[1]             = 0.25;
    times[2]             = 0.75;
    times[3]             = 1.0;
};
datablock ParticleData(BFB_SkyLightningBall_B_P : BFB_SkyLightningBall_A_P)
{
  textureCoords[0] = "0.5 0.0";
    textureCoords[1] = "0.5 0.5";
    textureCoords[2] = "1.0 0.5";
    textureCoords[3] = "1.0 0.0";
};
datablock ParticleData(BFB_SkyLightningBall_C_P : BFB_SkyLightningBall_A_P)
{
  textureCoords[0] = "0.0 0.5";
    textureCoords[1] = "0.0 1.0";
    textureCoords[2] = "0.5 1.0";
    textureCoords[3] = "0.5 0.5";
};

datablock ParticleEmitterData(BFB_SkyLightningBall_E)
{
  thetaMin          = 0.0;
  thetaMax          = 180.0;
  ejectionOffset    = 0.8;
  ejectionPeriodMS  = 60;
  periodVarianceMS  = 20;
  ejectionVelocity  = 0;
  velocityVariance  = 0;
  particles         = "BFB_SkyLightningBall_A_P BFB_SkyLightningBall_B_P BFB_SkyLightningBall_C_P";
  blendStyle        = "ADDITIVE";
  fadeSize          = true;
};

// this sets the height of the lightning source, high in the sky
datablock afxXM_LocalOffsetData(BFB_SkyLightningBall_offset_XM)
{  
  localOffset = "0 0" SPC 51*0.7;
};

datablock afxEffectWrapperData(BFB_SkyLightningBall_EW)
{
  effect = BFB_SkyLightningBall_E;
  constraint = anchor;
  delay      = 0; 
  lifetime = "$$ 0.45 + %%._scale*0.7";
  fadeInTime = 0.15;
  fadeOutTime = 0.15;
  xfmModifiers[0] = BFB_SkyLightningBall_offset_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// SKY LIGHTNING BOLTS (geometry)
//

datablock afxModelData(BFB_SkyLightning_CE)
{
  shapeFile = %mySpellDataPath @ "/BFB/models/bfb_sky_lightning.dts";
  forceOnMaterialFlags = $MaterialFlags::Additive | $MaterialFlags::SelfIlluminating;
  sequence = "bolt";
  sequenceRate = 1.5;
};

datablock afxEffectWrapperData(BFB_SkyLightning_EW)
{
  effect = BFB_SkyLightning_CE;
  constraint = anchor;
  delay      = 0.2; 
  lifetime = "$$ %%._skyboltsLife";
  fadeInTime = 0.15;
  fadeOutTime = 0.05;
  scaleFactor = 0.7;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// SKY LIGHTNING GLOW (particles)
//

// 8 Path variations for the glow particles

datablock afxPathData(BFB_CastingLightning_00_path)
{
  points = "$$ BFB_getGlowPath(##)";
  mult = 0.7;
};

//~~~~~~~~~~~~~~~~~~~~//

datablock ParticleData(BFB_LightningGlow_P)
{
  textureName          = %mySpellDataPath @ "/Shared/particles/sp2_phosphor_white";
  dragCoeffiecient     = 0;
  gravityCoefficient   = 0;
  inheritedVelFactor   = 0.00;
  lifetimeMS           = (5/30)*1000;
  lifetimeVarianceMS   = 0;
  spinRandomMin        = 0;
  spinRandomMax        = 0;
  colors[0]            = "0.24 0.69 0.78 0.01";
   colors[1]            = "0.24 0.69 0.78 0.02";
   colors[2]            = "0.24 0.69 0.78 0.04";
   colors[3]            = "0.24 0.69 0.78 0.05";
  sizeBias             = 1.0;
  sizes[0]             = 0;   
   sizes[1]             = 30;
   sizes[2]             = 12;
   sizes[3]             = 0;
  times[0]             = 0.0;
   times[1]             = 0.2;
   times[2]             = 0.8;
   times[3]             = 1.0;
};

datablock afxParticleEmitterPathData(BFB_CastingLightningGlow_00_E)
{
  ejectionPeriodMS = 2;
  periodVarianceMS = 1;
  ejectionVelocity = 0;
  velocityVariance = 0;
  particles        = "BFB_LightningGlow_P";
  pathOrigin       = "vector";
  paths            = BFB_CastingLightning_00_path;
  fadeSize         = true;
  fadeAlpha        = true;
  vector           = "0 0 1";
  blendStyle       = "ADDITIVE";
};

datablock afxXM_SpinData(BFB_CastingLightningGlow_00_spin_XM)
{
  spinAxis  = "0 0 1";
  spinAngle = "$$ %%._skyboltsGlow.angle[##]";
  spinRate  = 0;
};

datablock afxEffectWrapperData(BFB_CastingLightningGlow_00_EW)
{
  effectEnabled = "$$ %%._skyboltsLife > %%._skyboltsGlow.start[##]";
  effect = BFB_CastingLightningGlow_00_E;
  constraint = anchor;
  delay      = "$$ 0.2 + %%._skyboltsGlow.start[##]";
  lifetime   = "$$ %%._skyboltsGlow.end[##] - %%._skyboltsGlow.start[##]";
  fadeOutTime = 0.2;
  xfmModifiers[0] = BFB_CastingLightningGlow_00_spin_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// SCORCHED EARTH (zodiac)
//

datablock afxZodiacData(BFB_LightningBurnZodiacA_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/BFB/zodiacs/bfb_lightning_scorch";
  radius = "$$ 2.0 + 7.0*%%._scale";
  color = "1 1 1 1";
  blend = normal;
};
//
datablock afxEffectWrapperData(BFB_LightningBurnZodiacA_EW)
{
  effectEnabled = "$$ %%._scorchedEarth";
  effect = BFB_LightningBurnZodiacA_CE;
  constraint = anchor;
  lifetime    = 0.0;
  residueLifetime = 5.0;
  delay       = 0.3;
  fadeInTime  = 0.5;
  fadeOutTime = 1.0;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// TARGET REACTION (animation)
//

datablock afxAnimClipData(BFB_Electrocute_CE)
{
  clipName = "bfb_hit2";
  rate = 1.0;
  ignoreCorpse = true;
};
//
datablock afxEffectWrapperData(BFB_Electrocute_EW)
{
  effectEnabled = "$$ %%._targeted == true";
  effect = BFB_Electrocute_CE;
  constraint = "anchor";
  delay = 0.1;
  lifetime = 1.5;
  lifetime = "$$ 0.5 + %%._skyboltsLife";
};

datablock afxAnimClipData(BFB_Knees_CE)
{
  clipName = "bfb_hit";
  rate = 1;
  ignoreCorpse = true;
  lockAnimation = true;
};
//
datablock afxEffectWrapperData(BFB_Knees_EW)
{
  effectEnabled = "$$ %%._targeted == true";
  effect = BFB_Knees_CE;
  constraint = "anchor";
  delay = 1.5;
  delay = "$$ 0.5 + %%._skyboltsLife";
  lifetime = 1.5;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// CASTER GESTURE (animation)
//

datablock afxAnimClipData(BFB_HeadClip1_CE)
{
  clipName = "head"; // this is a blend sequence
  posOffset = 0.5;
  rate = -0.5;
  ignoreFirstPerson = true;
};
//
datablock afxEffectWrapperData(BFB_HeadClip1_EW)
{
  effect = BFB_HeadClip1_CE;
  constraint = "caster";
  delay = 0;
  lifetime = 2.0;
};

datablock afxAnimClipData(BFB_HeadClip2_CE)
{
  clipName = "head"; // this is a blend sequence
  posOffset = 0.55;
  rate = 0.05;
  ignoreFirstPerson = true;
};
//
datablock afxEffectWrapperData(BFB_HeadClip2_EW)
{
  effect = BFB_HeadClip2_CE;
  constraint = "caster";
  delay = 0;
  lifetime = 2;
};

datablock afxAnimClipData(BFB_Summon_Clip_CE)
{
  clipName = "summon";
  rate = 1.0;
};
//
datablock afxEffectWrapperData(BFB_Summon_Clip_EW)
{
  effect = BFB_Summon_Clip_CE;
  constraint = "caster";
  lifetime = 1.5;
  delay = 0.5;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// IMPACT LIGHTNING (geometry)
//

datablock afxXM_LocalOffsetData(BFB_ImpactLightning_offset_XM)
{  
  localOffset = "0 0 1.5";
  fadeInTime = 1.0;
  fadeOutTime = 1.0;
  lifetime = 1.0;
  lifetime = "$$ %%._skyboltsLife";
};
datablock afxModelData(BFB_ImpactLightning_CE)
{
  shapeFile = %mySpellDataPath @ "/BFB/models/bfb_impact_lightning.dts";
  forceOnMaterialFlags = $MaterialFlags::Additive | $MaterialFlags::SelfIlluminating;
  sequence = "bolt";
  sequenceRate = 1.0;
};

datablock afxEffectWrapperData(BFB_ImpactLightning_EW)
{
  effectEnabled = "$$ %%._targeted == true";
  effect = BFB_ImpactLightning_CE;
  constraint = "anchor";
  lifetime = 1.0;
  lifetime = "$$ %%._skyboltsLife";
  fadeOutTime = 0.3;
  delay = 0.2;
  xfmModifiers[0] = BFB_ImpactLightning_offset_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// AUDIO AND LIGHTING

switch$ (afxGetEngine())
{
  case "TGE":
    exec("./lighting/bfb_lighting_tge_sub.cs");
    exec("./audio/bfb_audio_sub.cs");
  case "TGEA":
    exec("./lighting/bfb_lighting_tgea_sub.cs");
    exec("./audio/bfb_audio_sub.cs");
 case "T3D":
    exec("./lighting/bfb_lighting_t3d_sub.cs");
    exec("./audio/bfb_audio_sub.cs");
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// GROUPS

datablock afxEffectGroupData(BFB_SkyLightningGlow_EG)
{
  assignIndices = true;
  count = 16;
  addEffect = BFB_CastingLightningGlow_00_EW;
};

BFB_add_group_Lighting_FX(BFB_SkyLightningGlow_EG);

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// BOLT FROM THE BLUE SPELL
//

datablock afxMagicSpellData(BoltFromTheBlueSpell)
{
  echoPacketUsage = 20;
  castingDur = "$$ %%._castDur";
  
  clientScriptFile = %mySpellDataPath @ "/BFB/bfb_client.cs";
  clientInitFunction = "BFB_clientInit";

  addCastingEffect = BFB_HeadClip1_EW;
  addCastingEffect = BFB_HeadClip2_EW;
  addCastingEffect = BFB_Summon_Clip_EW;

  addImpactEffect = BFB_SkyLightningBall_EW;
  
  addImpactEffect = BFB_SkyLightning_EW;
  addImpactEffect = BFB_SkyLightningGlow_EG;
  addImpactEffect = BFB_LightningBurnZodiacA_EW;

  addImpactEffect = BFB_Electrocute_EW;
  addImpactEffect = BFB_Knees_EW;
  addImpactEffect = BFB_ImpactLightning_EW;

    // default parameter settings //
  _scale = 1.0; 
  _castDur = 0.0;
  _scorchedEarth = 0;
  _targeted = 0;
  _casterAnchor = 0;  
};

// sounds added via sub-script functions //
BFB_add_Audio_FX(BoltFromTheBlueSpell);

datablock afxRPGMagicSpellData(BoltFromTheBlueSpell_RPG)
{
  spellName = "Bolt from the Blue";
  desc = "Crack the sky and smite an unsuspecting foe with a frightening thunderbolt " @
         "of lethal energy.\n" @
         "<font:Arial:6>\n" @
         "<just:center><font:Arial Italic:16>Galileo figaro-magnifico!<just:left>\n" @
         "\n" @
         "<font:Arial Italic:14>spell design: <font:Arial:14>Matthew Durante\n" @
         "<font:Arial Italic:14>sound design: <font:Arial:14>Matt Pacyga\n" @ 
         "<font:Arial Italic:14>spell concept: <font:Arial:14>Jeff Faust";
  sourcePack = "Spell Pack 2";
  iconBitmap = %mySpellDataPath @ "/BFB/icons/bfb";
  manaCost = 10;
  castingDur = 0.5;
  target = "enemy";
  canTargetSelf = true;
  directDamage = 50.0;
  areaDamage = 20;
  areaDamageRadius = 15;
  areaDamageImpulse = 500;
  
  _castDur = 0.5;
  _scale =  1.0;
  _scorchedEarth = 1;
  _casterAnchor = 0;  
  _targeted = 1;
};

function BoltFromTheBlueSpell::onActivate(%this, %spell, %caster, %target)
{
  Parent::onActivate(%this, %spell, %caster, %target);
   
  if (%spell._targeted)
    %spell.addConstraint(%target, "anchor");
  else if (%spell._casterAnchor)
    %spell.addConstraint(%caster.getTransform(), "anchor");
    
  %spell._scale = mClamp(%spell._scale, 0.2, 1.0);
  %spell._skyboltsLife = 0.15 + 0.85*%spell._scale;
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
  BoltFromTheBlueSpell.scriptFile = $afxAutoloadScriptFile;
  BoltFromTheBlueSpell.spellDataPath = %mySpellDataPath;
  if (isFunction(addDemoSpellbookSpell))
  {
    addDemoSpellbookSpell(BoltFromTheBlueSpell, BoltFromTheBlueSpell_RPG);
  }
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//