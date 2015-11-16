
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// SHARDS OF VESUVIUS SPELL
//
//  Inspired by the dying words of Pliny the Elder himself, this incantation conjures 
//  a small but powerful fragment of the legendary eruption.
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
$spell_reload = isObject(ShardsOfVesuviusSpell);
if ($spell_reload)
{
  // mark datablocks so we can detect which are reloaded this script
  markDataBlocks();
  // reset data path from previously saved value
  %mySpellDataPath = ShardsOfVesuviusSpell.spellDataPath;
}
else
{
  // set data path from default plus containing folder name
  %mySpellDataPath = $afxSpellDataPath @ "/" @ $afxAutoloadScriptFolder;
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// Big Fiery Blast
//    Happens in three stages... each blast stage occuring a
//    little higher than the last but with a smaller spread.

datablock afxXM_LocalOffsetData(SoV_FieryBlast_offset_XM)
{
  localOffset = "0 0 -2";
};

// 3 kinds of particles varied in size
datablock ParticleData(SoV_FieryBlast_MED_P)
{
  textureName          = %mySpellDataPath @ "/Shared/particles/sp2_fireball_A";
  dragCoeffiecient     = 6.0; 
  gravityCoefficient   = 0.04;
  windCoefficient      = 0.0;
  inheritedVelFactor   = 0.00;
  lifetimeMS           = 1300;
  lifetimeVarianceMS   = 300;
  spinRandomMin        = -250; 
  spinRandomMax        = 250;
  colors[0]            = "1.0 1.0 1.0 0.0";
    colors[1]            = "1.0 1.0 1.0 0.3";
    colors[2]            = "1.0 1.0 1.0 0.6";
    colors[3]            = "0.0 0.0 0.0 0.0";
  sizeBias             = 1.0;
  sizes[0]             = 2;
    sizes[1]             = 5;
    sizes[2]             = 6;
    sizes[3]             = 5;
  times[0]             = 0.0;
    times[1]             = 0.2;
    times[2]             = 0.66;
    times[3]             = 1.0;
};
datablock ParticleData(SoV_FieryBlast_SM_P : SoV_FieryBlast_MED_P)
{
  sizeBias             = 0.5;
};
datablock ParticleData(SoV_FieryBlast_LG_P : SoV_FieryBlast_MED_P)
{
  sizeBias             = 1.5;
};

// 3 emitter fx varied in delay, sortPriority, ejectionOffset and spreadMax
datablock afxParticleEmitterConeData(SoV_FieryBlast_Stage1_E)
{
  ejectionPeriodMS      = 4;
  periodVarianceMS      = 2;
  ejectionVelocity      = 17.0*0.2;
  velocityVariance      = 4.0 *0.5;
  particles             = "SoV_FieryBlast_MED_P SoV_FieryBlast_SM_P SoV_FieryBlast_SM_P SoV_FieryBlast_LG_P";
  vector                = "0 0 1";
  forcedBBox            = "-25.0 -25.0 0.0 25.0 25.0 25.0";
  blendStyle            = "PREMULTALPHA";
  spreadMin             = 0.0;
  spreadMax             = 150;
  ejectionOffset        = 6.0;
  spreadMax             = 50;
};
datablock afxEffectWrapperData(SoV_FieryBlast_Stage1_EW)
{
  effect = SoV_FieryBlast_Stage1_E;
  constraint = "freeTarget";
  delay = 0.3;
  lifetime = 0.3;
  fadeinTime = 0.0;
  fadeOutTime = 0.0;
  forcedBBox = "-30.0 -30.0 0.0 30.0 30.0 35.0";
  sortPriority = 5;
  xfmModifiers[0] = SoV_FieryBlast_offset_XM;
};

datablock afxParticleEmitterConeData(SoV_FieryBlast_Stage2_E : SoV_FieryBlast_Stage1_E)
{
  spreadMax = 100;
  ejectionOffset = 9.0;
};
datablock afxEffectWrapperData(SoV_FieryBlast_Stage2_EW : SoV_FieryBlast_Stage1_EW)
{
  effect = SoV_FieryBlast_Stage2_E;
  delay = 0.6;
  sortPriority = 6;
};

datablock afxParticleEmitterConeData(SoV_FieryBlast_Stage3_E : SoV_FieryBlast_Stage1_E)
{
  spreadMax = 50;
  ejectionOffset = 12.0;
};
datablock afxEffectWrapperData(SoV_FieryBlast_Stage3_EW : SoV_FieryBlast_Stage1_EW)
{
  effect = SoV_FieryBlast_Stage3_E;
  delay = 0.9;
  sortPriority = 7;
};

//~~~~~~~~~~~~~~~~~~~~//

datablock afxParticlePoolData(SoV_SmokePool)
{
  poolType = "two-pass";
  baseColor = "$$ (%%._mood $= \"day\") ? \"0.1 0.08 0.075 0.9\" : \"0.07 0.045 0.041 0.9\"";
  blendWeight = "$$ (%%._mood $= \"day\") ? 0.5 : 0.6";
};

//~~~~~~~~~~~~~~~~~~~~//

// smoke particles (7)

datablock ParticleData(SoV_Smoke_D_2pass_P)
{
  textureName          = "$$ \"" @ %mySpellDataPath @ "/Shared/particles/sp2_dust_2x2_\" @ " @ "%%._mood";
  textureExtName       = %mySpellDataPath @ "/Shared/particles/sp2_dust_2x2_white";
  textureCoords[0]     = "0.5 0.5";
    textureCoords[1]     = "0.5 1.0";
    textureCoords[2]     = "1.0 1.0";
    textureCoords[3]     = "1.0 0.5";
  dragCoeffiecient     = 6.0;
  gravityCoefficient   = 0.04;
  windCoefficient      = 0.0;
  inheritedVelFactor   = 0.00;
  randomizeSpinDir     = true;
  lifetimeMS           = 10000*1.0;
  lifetimeVarianceMS   = 1000*1.0;
  spinRandomMin        = 30;
  spinRandomMax        = 50;
  spinBias             = 0.5;
  sizeBias             = 1.0;
  colors[0]            = "1 1 1 0.0";
    colors[1]            = "1 1 1 0.8";
    colors[2]            = "1 1 1 0.7";
    colors[3]            = "1 1 1 0.5";
    colors[4]            = "1 1 1 0.0";
  sizes[0]             = 5;
    sizes[1]             = 6;
    sizes[2]             = 12;
    sizes[3]             = 18;
    sizes[4]             = 20;
  times[0]             = 0.0;
    times[1]             = 0.05;
    times[2]             = 0.33;
    times[3]             = 0.66;
    times[4]             = 1.0;
};

datablock ParticleData(SoV_Smoke_C_2pass_P : SoV_Smoke_D_2pass_P)
{
  textureCoords[0]     = "0.0 0.5";
    textureCoords[1]     = "0.0 1.0";
    textureCoords[2]     = "0.5 1.0";
    textureCoords[3]     = "0.5 0.5";
  lifetimeMS           = 10000*1.2;
  lifetimeVarianceMS   = 1000*1.2;
  spinBias             = 0.6;
  sizeBias             = 0.75;
  colors[0]            = "1 1 1 0";
    colors[1]            = "1 1 1 1";
    colors[2]            = "1 1 1 1";
    colors[3]            = "1 1 1 1";
    colors[4]            = "1 1 1 0";
  times[0]             = 0.0;
    times[1]             = 0.1;
    times[2]             = 0.6;
    times[3]             = 0.85;
    times[4]             = 1.0;
};

datablock ParticleData(SoV_Smoke_B_2pass_P : SoV_Smoke_C_2pass_P)
{
  textureCoords[0]     = "0.5 0.0";
    textureCoords[1]     = "0.5 0.5";
    textureCoords[2]     = "1.0 0.5";
    textureCoords[3]     = "1.0 0.0";
  spinBias             = 0.75;
  sizeBias             = 0.55;
};

datablock ParticleData(SoV_Smoke_A_2pass_P : SoV_Smoke_B_2pass_P)
{
  textureCoords[0]     = "0.0 0.0";
    textureCoords[1]     = "0.0 0.5";
    textureCoords[2]     = "0.5 0.5";
    textureCoords[3]     = "0.5 0.0";
};

datablock ParticleData(SoV_Smoke_C_SM_2pass_P : SoV_Smoke_C_2pass_P)
{
  sizeBias             = 0.35;
  sizes[0]             = 7.86;
};
datablock ParticleData(SoV_Smoke_B_SM_2pass_P : SoV_Smoke_B_2pass_P)
{
  sizeBias             = 0.25;
  sizes[0]             = 11;
};
datablock ParticleData(SoV_Smoke_A_SM_2pass_P : SoV_Smoke_A_2pass_P)
{
  sizeBias             = 0.25;
  sizes[0]             = 11;
};

// smoke emitters (2)

datablock afxParticleEmitterConeData(SoV_Smoke_2pass_E)
{
  ejectionOffset        = 6.0;
  vector                = "0 0 1";
  spreadMin             = 0.0;
  spreadMax             = 120.0;
  poolData              = SoV_SmokePool;
  poolDepthFade         = true;
  poolRadialFade        = true;
  ejectionPeriodMS      = 30;
  periodVarianceMS      = 15;
  ejectionVelocity      = 17.0*0.18;
  velocityVariance      = 4.0*0.18;
  particles             = "SoV_Smoke_C_2pass_P SoV_Smoke_C_SM_2pass_P" SPC
                          "SoV_Smoke_B_2pass_P SoV_Smoke_B_SM_2pass_P" SPC 
                          "SoV_Smoke_A_2pass_P SoV_Smoke_A_SM_2pass_P";
};
datablock afxParticleEmitterConeData(SoV_Smoke_LG_2pass_E : SoV_Smoke_2pass_E)
{
  ejectionPeriodMS      = 40;
  periodVarianceMS      = 20;
  ejectionVelocity      = 17.0*0.155;
  velocityVariance      = 4.0*0.155;
  particles             = "SoV_Smoke_D_2pass_P";
};

// smoke effects (2)

datablock afxXM_LocalOffsetData(SoV_Smoke_offset_XM)
{
  localOffset = "0 0 -2";
};

datablock afxEffectWrapperData(SoV_Smoke_2pass_EW)
{
  effect = SoV_Smoke_2pass_E;
  constraint = "freeTarget";
  delay = 0.0;
  lifetime = 2.0;
  fadeinTime = 0.0;
  fadeOutTime = 0.0;
  forcedBBox = "-30.0 -30.0 0.0 30.0 30.0 35.0";
  sortPriority = 100; // used by the smoke ParticlePool
  xfmModifiers[0] = SoV_Smoke_offset_XM;
};

datablock afxEffectWrapperData(SoV_Smoke_LG_2pass_EW : SoV_Smoke_2pass_EW)
{
  effect = SoV_Smoke_LG_2pass_E;
  delay = 2.0;
  lifetime = 1.0;
  forcedBBox = "-29.0 -29.0 0.0 29.0 29.0 34.0";  // must differ from other emitter to avoid flickering
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// ROCK DEBRIS

datablock afxXM_RandomRotData(SoV_RockDebris_random_rot_XM)
{
  axis = "0 0 1";
  thetaMin = 40;
  thetaMax = 80;
};

datablock DebrisData(SoV_RockDebris_00_CE)
{
  shapeFile = %mySpellDataPath @ "/SoV/models/sov_rockC.dts";
  shapeFile = "$$ \"" @ %mySpellDataPath @ "/SoV/models/\" @ SoV_pickRockModel()";
  elasticity = 0.6;
  friction = 0.5;
  numBounces = 2;
  bounceVariance = 2;
  minSpinSpeed = 200;
  maxSpinSpeed = 700;
  lifetime = 3;
  lifetimeVariance = 0.5;
  velocity = 15;
  velocityVariance = 5.0;
  useRadiusMass = true;
  baseRadius = 0.3;
  gravModifier = 0.8;
  terminalVelocity = 20;
};

datablock afxEffectWrapperData(SoV_RockDebris_00_EW)
{
  effect = SoV_RockDebris_00_CE;
  constraint = "freeTarget";
  delay = "$$ SoV_pickRockDelay()";
  xfmModifiers[0] = SoV_RockDebris_random_rot_XM;
};

datablock afxEffectGroupData(SoV_RockDebris_00_EG)
{
  count = 45;
  count = "$$ %%._numRocks";
  addEffect = SoV_RockDebris_00_EW;
};

/*
// caster performs a throwing animation 
datablock afxAnimClipData(SoV_Casting_Clip_CE)
{
  clipName = "throw";
  rate = 0.7;
};
datablock afxEffectWrapperData(SoV_Casting_Clip_EW)
{
  effect = SoV_Casting_Clip_CE;
  constraint = "caster";
  delay = 0;
  lifetime = 2.0; 
};
*/

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// AUDIO AND LIGHTING

switch$ (afxGetEngine())
{
  case "TGE":
    exec("./lighting/sov_lighting_tge_sub.cs");
    exec("./audio/sov_audio_sub.cs");
  case "TGEA":
    exec("./lighting/sov_lighting_tgea_sub.cs");
    exec("./audio/sov_audio_sub.cs");
 case "T3D":
    exec("./lighting/sov_lighting_t3d_sub.cs");
    exec("./audio/sov_audio_sub.cs");
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// exec sub-script
exec("./special/sov_comets_sub.cs");

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// SHARDS OF VESUVIUS SPELL
//

datablock afxMagicSpellData(ShardsOfVesuviusSpell)
{
  echoPacketUsage = 20;
  castingDur = 0.0;
  clientScriptFile = %mySpellDataPath @ "/SoV/sov_client.cs";
  clientInitFunction = "SoV_clientInit";

    // big fiery explosion //
  addCastingEffect = SoV_FieryBlast_Stage1_EW;
  addCastingEffect = SoV_FieryBlast_Stage2_EW;
  addCastingEffect = SoV_FieryBlast_Stage3_EW;
    // dark lingering smoke clouds //
  addCastingEffect = SoV_Smoke_2pass_EW;
  addCastingEffect = SoV_Smoke_LG_2pass_EW;
    // rock debris //
  addCastingEffect = SoV_RockDebris_00_EG;

  //addCastingEffect = SoV_Casting_Clip_EW;
};

// add effects from sub-scripts
SoV_add_Comet_FX(ShardsOfVesuviusSpell);
SoV_add_Lighting_FX(ShardsOfVesuviusSpell);
SoV_add_Audio_FX(ShardsOfVesuviusSpell);

datablock afxRPGMagicSpellData(ShardsOfVesuviusSpell_RPG)
{
  spellName = "Shards of Vesuvius";
  desc = "Inspired by the dying words of Pliny the Elder himself," SPC
         "this incantation conjures a small but powerful fragment of the"   SPC
         "legendary eruption.\n" @
         "\n" @
         "<font:Arial Italic:14>adaptation from Fire in the Sky: <font:Arial:14>Jeff Faust\n" @
         "<font:Arial Italic:14>sound design: <font:Arial:14>Matt Pacyga";
  sourcePack = "Spell Pack 2";
  iconBitmap = %mySpellDataPath @ "/SoV/icons/sov";
  manaCost = 10;
  castingDur = ShardsOfVesuviusSpell.castingDur;
  target = "free";
  canTargetSelf = true;
};

function ShardsOfVesuviusSpell::onActivate(%this, %spell, %caster, %target)
{
  Parent::onActivate(%this, %spell, %caster, %target);
  if (isObject(%target))
    %spell.addConstraint(%target.getTransform(), "freeTarget");

  %spell._mood = (MissionInfo.isNightMission) ? "night" : "day";

  %spell._numRocks = getRandom(40,50);
  SoV_add_Comet_Params(%spell, 0.0);
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
  ShardsOfVesuviusSpell.scriptFile = $afxAutoloadScriptFile;
  ShardsOfVesuviusSpell.spellDataPath = %mySpellDataPath;
  if (isFunction(addDemoSpellbookSpell))
    addDemoSpellbookSpell(ShardsOfVesuviusSpell, ShardsOfVesuviusSpell_RPG);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
