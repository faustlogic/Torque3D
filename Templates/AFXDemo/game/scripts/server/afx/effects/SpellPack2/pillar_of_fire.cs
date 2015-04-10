//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// PILLAR OF FIRE -- [reusable element]
//
//    Kindles an upsurge of fire and smoke forming a column or pillar shape.
//
//    parameters:
//      _scale          (float)     default=1.0  
//      _height         (float)     default=13.0
//      _upsurgeDur     (float)     default=0.6
//      _mood           (string)    default="day"
//      _dust           (bool)      default=false
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
$spell_reload = isObject(PillarOfFireSpell);
if ($spell_reload)
{
  // mark datablocks so we can detect which are reloaded this script
  markDataBlocks();
  // reset data path from previously saved value
  %mySpellDataPath = PillarOfFireSpell.spellDataPath;
}
else
{
  // set data path from default plus containing folder name
  %mySpellDataPath = $afxSpellDataPath @ "/" @ $afxAutoloadScriptFolder;
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// EXPLOSION

datablock afxParticlePoolData(PoF_Explosion_Pool)
{
  poolType = "normal";  
};

datablock ParticleData(PoF_ExplosionBlast_P)
{
  textureName          = "$$ \"" @ %mySpellDataPath @ "/PoF/particles/pof_parts_2x2_\" @ " @ "%%._mood";
  textureCoords[0]     = "0.0 0.5";
    textureCoords[1]     = "0.0 1.0";
    textureCoords[2]     = "0.5 1.0";
    textureCoords[3]     = "0.5 0.5";
  dragCoeffiecient     = 6.0; 
  gravityCoefficient   = 0.04;
  windCoefficient      = 0.0;
  inheritedVelFactor   = 0.00;
  lifetimeMS           = 1300*0.6;
  lifetimeVarianceMS   = 300*0.6;
  spinRandomMin        = -250; 
  spinRandomMax        = 250;
  colors[0]            = "1.0 1.0 1.0 0.0";
    colors[1]            = "1.0 1.0 1.0 0.3";
    colors[2]            = "1.0 1.0 1.0 0.6";
    colors[3]            = "0.0 0.0 0.0 0.0";
  sizes[0]             = 2;
    sizes[1]             = 5;
    sizes[2]             = 6;
    sizes[3]             = 5;
  times[0]             = 0.0;
    times[1]             = 0.2;
    times[2]             = 0.66;
    times[3]             = 1.0;
};

datablock afxParticleEmitterDiscData(PoF_ExplosionBlast_E)
{
  ejectionPeriodMS      = 5;
  periodVarianceMS      = 2;
  ejectionVelocity      = 1.3;
  velocityVariance      = 0.3;
  particles             = "PoF_ExplosionBlast_P";
  vector                = "0 0 1";
  radiusMin             = 0.0;
  radiusMax             = 4.0;
  blendStyle            = "PREMULTALPHA";
  poolData              = PoF_Explosion_Pool;
};

datablock afxEffectWrapperData(PoF_ExplosionBlast_EW)
{
  effect = PoF_ExplosionBlast_E;
  constraint = anchor;
  lifetime    = 0.2;
};

//~~~~~~~~~~~~~~~~~~~~//

datablock ParticleData(PoF_ExplosionFireballM_P)
{
  textureName          = "$$ \"" @ %mySpellDataPath @ "/PoF/particles/pof_parts_2x2_\" @ " @ "%%._mood";
  textureCoords[0]     = "0.5 0.5";
    textureCoords[1]     = "0.5 1.0";
    textureCoords[2]     = "1.0 1.0";
    textureCoords[3]     = "1.0 0.5";
  dragCoeffiecient     = 6.0; 
  gravityCoefficient   = -0.5;
  windCoefficient      = 0.0;
  inheritedVelFactor   = 0.00;
  lifetimeMS           = 900;
  lifetimeVarianceMS   = 200;
  spinRandomMin        = -250; 
  spinRandomMax        = 250;
  colors[0]            = "1.0 1.0 1.0 0.0";
    colors[1]            = "1.0 1.0 1.0 0.3";
    colors[2]            = "1.0 1.0 1.0 0.6";
    colors[3]            = "0.0 0.0 0.0 0.6";
    colors[4]            = "0.0 0.0 0.0 0.0";
  sizeBias             = 1.0;
  sizes[0]             = 2;
    sizes[1]             = 5;
    sizes[2]             = 6;
    sizes[3]             = 8;
    sizes[4]             = 10;
  times[0]             = 0.0;
    times[1]             = 0.1;
    times[2]             = 0.4;
    times[3]             = 0.6;
    times[4]             = 1.0;
};
datablock ParticleData(PoF_ExplosionFireballS_P : PoF_ExplosionFireballM_P)
{
  sizeBias = 0.5;
};
/* Large currently unused
datablock ParticleData(PoF_ExplosionFireballL_P : PoF_ExplosionFireballM_P)
{
  sizeBias = 1.5;
};
*/

datablock afxParticleEmitterConeData(PoF_Explosion_E)
{
  ejectionPeriodMS      = 4;
  periodVarianceMS      = 2;
  ejectionVelocity      = 2.0;
  velocityVariance      = 0.5;
  particles             = "PoF_ExplosionFireballM_P PoF_ExplosionFireballS_P PoF_ExplosionFireballS_P PoF_ExplosionFireballS_P";
  vector                = "0 0 1";
  spreadMin             = 0.0;
  spreadMax             = 150;
  blendStyle            = "PREMULTALPHA";
  fadeOffset            = false;
  fadeSize              = true;
  poolData              = PoF_Explosion_Pool;
};

datablock afxXM_LocalOffsetData(PoF_Explosion_offset_XM)
{
  localOffset = "$$ \"0 0\" SPC %%._height";
  lifetime    = "$$ %%._upsurgeDur";
  fadeInTime  = "$$ %%._upsurgeDur";
  fadeInEase  = "0.0 0.0";
};

datablock afxEffectWrapperData(PoF_Explosion_EW)
{
  effect = PoF_Explosion_E;
  constraint = anchor;
  delay       = 0.08;
  lifetime    = "$$ %%._upsurgeDur";
  fadeInTime  = "$$ %%._upsurgeDur";
  xfmModifiers[0] = PoF_Explosion_offset_XM;
};

//~~~~~~~~~~~~~~~~~~~~//

datablock ParticleData(PoF_DustA_P)
{
  textureName          = "$$ \"" @ %mySpellDataPath @ "/PoF/particles/pof_parts_2x2_\" @ " @ "%%._mood";
  textureCoords[0]     = "0.0 0.0";
    textureCoords[1]     = "0.0 0.5";
    textureCoords[2]     = "0.5 0.5";
    textureCoords[3]     = "0.5 0.0";
  dragCoeffiecient     = 1;
  gravityCoefficient   = 0;
  inheritedVelFactor   = 0.00;
  lifetimeMS           = 2000;
  ifetimeVarianceMS    = 750;
  randomizeSpinDir     = true;
  spinRandomMin        = 100.0;
  spinRandomMax        = 150.0;   
  colors[0]            = "0.0 0.0 0.0 0.0";
    colors[1]            = "0.7 0.7 0.7 1.0";
    colors[2]            = "0.7 0.7 0.7 1.0";
    colors[3]            = (0.7*0.5) SPC (0.7*0.5) SPC (0.7*0.5) SPC (1.0*0.5);
    colors[4]            = "0.0 0.0 0.0 0.0";   
  sizeBias             = "$$ %%._scale";
  sizes[0]             = 2;
    sizes[1]             = 4;
    sizes[2]             = 6;
    sizes[3]             = 8;
    sizes[4]             = 10;
  times[0]             = 0.0;
    times[1]             = 0.05;
    times[2]             = 0.15;
    times[3]             = 0.3;
    times[4]             = 1.0;
};
datablock ParticleData(PoF_DustB_P : PoF_DustA_P)
{
  textureCoords[0] = "0.5 0.0";
    textureCoords[1] = "0.5 0.5";
    textureCoords[2] = "1.0 0.5";
    textureCoords[3] = "1.0 0.0";
};

datablock afxParticleEmitterConeData(PoF_ExplosionDust_E)
{
  ejectionPeriodMS      = 4;
  periodVarianceMS      = 2;
  ejectionVelocity      = 10.0;
  velocityVariance      = 3.0;
  particles             = "PoF_DustA_P PoF_DustB_P";
  vector                = "0 0 1";
  spreadMin             = 150;
  spreadMax             = 180;
  blendStyle            = "PREMULTALPHA";  
  poolData              = PoF_Explosion_Pool;
};

datablock afxEffectWrapperData(PoF_Explosion_Dust_EW)
{
  effectEnabled = "$$ %%._dust";
  effect = PoF_ExplosionDust_E;
  constraint = anchor;
  delay = 0.2;
  lifetime = 0.3;
  fadeInTime = 0;
  fadeOutTime = 0;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// AUDIO AND LIGHTING

switch$ (afxGetEngine())
{
  case "TGE":
    exec("./lighting/pof_lighting_tge_sub.cs");
    //exec("./audio/pof_audio_sub.cs");
  case "TGEA":
    exec("./lighting/pof_lighting_tgea_sub.cs");
    //exec("./audio/pof_audio_sub.cs");
 case "T3D":
    exec("./lighting/pof_lighting_t3d_sub.cs");
    //exec("./audio/pof_audio_sub.cs");
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
//PILLAR OF FIRE EFFECTRON
//

datablock afxEffectronData(PillarOfFireEffectron)
{
  echoPacketUsage = 20;
  duration = 1.0;
  numLoops = 1;
  
  addEffect = PoF_ExplosionBlast_EW;
  addEffect = PoF_Explosion_EW;
  addEffect = PoF_Explosion_Dust_EW;

    // default parameter settings //
  _scale =  1;
  _height = 13.0;
  _upsurgeDur = 0.6;
  _dust = 0;
};

// sounds and lights added via sub-script functions //
PoF_add_Lighting_FX(PillarOfFireEffectron);
//PoF_add_Audio_FX(PillarOfFireEffectron);

function PillarOfFireEffectron::onActivate(%this, %effectron)
{  
  %effectron._mood = (MissionInfo.isNightMission) ? "night" : "day"; 
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// PILLAR OF FIRE SPELL
//

datablock afxMagicSpellData(PillarOfFireSpell)
{
  castingDur = 0;
};

datablock afxRPGMagicSpellData(PillarOfFireSpell_RPG)
{
  spellName = "Pillar of Fire";
  desc = "Kindles an upsurge of fire and smoke forming a column or pillar shape." @
         "\n\n" @
         "[reusable element]";
  sourcePack = "Spell Pack 2";
  iconBitmap = %mySpellDataPath @ "/PoF/icons/pof";
  manaCost = 10;
  castingDur = 0;
  target = "free";
};

function PillarOfFireSpell::onLaunch(%this, %spell, %caster, %target, %missile)
{
  Parent::onLaunch(%this, %spell, %caster, %target, %missile);

  %anchor = %spell.freeTarget;

  %pillar_of_fire = startEffectron(PillarOfFireEffectron, %anchor, "anchor");
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
  PillarOfFireSpell.scriptFile = $afxAutoloadScriptFile;
  PillarOfFireSpell.spellDataPath = %mySpellDataPath;
  if (isFunction(addDemoSpellbookSpell))
  {
    addDemoSpellbookSpell(PillarOfFireSpell, PillarOfFireSpell_RPG);
  }
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//