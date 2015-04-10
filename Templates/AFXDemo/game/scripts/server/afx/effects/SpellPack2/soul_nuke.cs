//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// SOUL NUKE
//
//    Detonates a colorful magic explosion that forms a small mushroom cloud shape.
//
//    parameters:
//      _expScale      (float)       default=1.0
//      _mood          (string)      default="day"
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
$spell_reload = isObject(SoulNukeSpell);
if ($spell_reload)
{
  // mark datablocks so we can detect which are reloaded this script
  markDataBlocks();
  // reset data path from previously saved value
  %mySpellDataPath = SoulNukeSpell.spellDataPath;
}
else
{
  // set data path from default plus containing folder name
  %mySpellDataPath = $afxSpellDataPath @ "/" @ $afxAutoloadScriptFolder;

  afxExecPrerequisite("sp2_shared_fx.cs", $afxAutoloadScriptFolder);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// EXPLOSION FLASH

//
// -- particles + light flare
//

datablock ParticleData(SN_Fire_S_P)
{
  textureName          = %mySpellDataPath @ "/SN/particles/sn_fire_candy";
  dragCoeffiecient     = 0.5;
  inheritedVelFactor   = 0.00;
  lifetimeMS           = 225;
  lifetimeVarianceMS   = 50;
  spinRandomMin        = 0;
  spinRandomMax        = 0;
  colors[0]            = "0.0 0.0 0.0 0.0";
    colors[1]            = "1.0 1.0 1.0 1.0";
    colors[2]            = "0.0 0.0 0.0 0.0";
  sizeBias             = "$$ 3*%%._expScale";
  sizes[0]             = 0;
    sizes[1]             = 1.5;
    sizes[2]             = 0;
  times[0]             = 0.0;
    times[1]             = 0.5;
    times[2]             = 1.0;
};
datablock ParticleData(SN_Fire_M_P : SN_Fire_S_P)
{
  sizeBias = "$$ 4.5*%%._expScale";
};
datablock ParticleData(SN_Fire_L_P : SN_Fire_S_P)
{
  sizeBias = "$$ 6.5*%%._expScale";
};

datablock afxParticleEmitterDiscData(SN_Fire_E)
{
  ejectionPeriodMS  = 1;
  periodVarianceMS  = 0;
  ejectionVelocity  = 0;
  velocityVariance  = 0;
  particles         = "SN_Fire_S_P SN_Fire_M_P SN_Fire_L_P";
  vector            = "0 0 1";
  radiusMin         = 0.5;
  radiusMax         = 2.5;
  blendStyle        = "ADDITIVE";
};

datablock afxXM_WorldOffsetData(SN_Fire_offset_XM)
{
  worldOffset = "0 0 0.3";
};

datablock afxEffectWrapperData(SN_Fire_EW)
{
  effect = SN_Fire_E;
  posConstraint = "mine";
  lifetime = 0.03;
  xfmModifiers[0] = SN_Fire_offset_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// DUST PLUME

datablock ParticleData(SN_SmokeA_P)
{
  textureName          = "$$ \"" @ %mySpellDataPath @ "/Shared/particles/sp2_dust_2x2_\" @ " @ "%%._mood";
  textureCoords[0] = "0.0 0.0";
    textureCoords[1] = "0.0 0.5";
    textureCoords[2] = "0.5 0.5";
    textureCoords[3] = "0.5 0.0";
  dragCoeffiecient     = 1;
  gravityCoefficient   = 0.35;
  inheritedVelFactor   = 0.00;
  lifetimeMS           = 2500;
  lifetimeVarianceMS   = 250;
  randomizeSpinDir     = true;
  spinRandomMin        = 100.0;
  spinRandomMax        = 150.0;  
  colors[0]            = "0.74 0.07 0.73 0.0";
    colors[1]            = "0.03 0.96 0.74 0.0";
    colors[2]            = "0.20 0.20 0.20 1.0";
    colors[3]            = "0.70 0.70 0.70 1.0";
    colors[4]            = "0.00 0.00 0.00 0.0";
  sizeBias             = "$$ %%._expScale";
  sizes[0]             = 3.0;
    sizes[1]             = 1.3;
    sizes[2]             = 5.0;
    sizes[3]             = 6.0;
    sizes[4]             = 9.0;
  times[0]             = 0.0;
    times[1]             = 0.05;
    times[2]             = 0.2;
    times[3]             = 0.7;
    times[4]             = 1.0;
};
datablock ParticleData(SN_SmokeB_P : SN_SmokeA_P)
{
  textureCoords[0] = "0.5 0.0";
    textureCoords[1] = "0.5 0.5";
    textureCoords[2] = "1.0 0.5";
    textureCoords[3] = "1.0 0.0";
};

datablock ParticleData(SN_SmokeA_Fill_P)
{
  textureName          = "$$ \"" @ %mySpellDataPath @ "/Shared/particles/sp2_dust_2x2_\" @ " @ "%%._mood";
  textureCoords[0] = "0.0 0.0";
    textureCoords[1] = "0.0 0.5";
    textureCoords[2] = "0.5 0.5";
    textureCoords[3] = "0.5 0.0";
  dragCoeffiecient     = 1;
  gravityCoefficient   = 0.35;
  inheritedVelFactor   = 0.00;
  lifetimeMS           = 2500 * 0.2;
  lifetimeVarianceMS   = 250 * 0.2;
  randomizeSpinDir     = true;
  spinRandomMin        = 100.0;
  spinRandomMax        = 150.0;
  colors[0]            = 189/255 SPC 17/255  SPC  187/255 SPC 0.0;
    colors[1]            = 8/255 SPC 245/255 SPC 189/255 SPC 0.0;
    colors[2]            = "0.0 0.0 0.0 0.0";  
  sizeBias             = "$$ %%._expScale";
  sizes[0]             = 3.0;
    sizes[1]             = 1.3;
    sizes[2]             = 5.0;
  times[0]             = 0.0;
    times[1]             = 0.25;
    times[2]             = 1.0;
};

datablock ParticleData(SN_SmokeB_Fill_P : SN_SmokeA_Fill_P)
{
  textureCoords[0] = "0.5 0.0";
    textureCoords[1] = "0.5 0.5";
    textureCoords[2] = "1.0 0.5";
    textureCoords[3] = "1.0 0.0";
};

datablock ParticleData(SN_SmokeC_P : SN_SmokeA_P)
{
  gravityCoefficient   = 0;
  lifetimeMS           = 1500;
  colors[0]            = "0.0 0.0 0.0 0.0";
    colors[1]            = "0.7 0.7 0.7 1.0";
    colors[2]            = (0.7*0.7) SPC (0.7*0.7) SPC (0.7*0.7) SPC (1.0*0.7);
    colors[3]            = (0.7*0.2) SPC (0.7*0.2) SPC (0.7*0.2) SPC (1.0*0.2);
    colors[4]            = "0.0 0.0 0.0 0.0";
  sizeBias             = "$$ %%._expScale";
  sizes[0]             = 2;
    sizes[1]             = 4;
    sizes[2]             = 7;
    sizes[3]             = 15;
    sizes[4]             = 30;
};

datablock ParticleData(SN_SmokeD_P : SN_SmokeB_P)
{
  gravityCoefficient   = 0;
  lifetimeMS           = 1500;
  colors[0]            = "0.0 0.0 0.0 0.0";
    colors[1]            = "0.7 0.7 0.7 1.0";
    colors[2]            = (0.7*0.7) SPC (0.7*0.7) SPC (0.7*0.7) SPC (1.0*0.7);
    colors[3]            = (0.7*0.2) SPC (0.7*0.2) SPC (0.7*0.2) SPC (1.0*0.2);
    colors[4]            = "0.0 0.0 0.0 0.0";
  sizeBias             = "$$ %%._expScale";
  sizes[0]             = 2;
    sizes[1]             = 4;
    sizes[2]             = 7;
    sizes[3]             = 15;
    sizes[4]             = 30; 
};

// The concentric profile path for the disc emitter
//  -- x-values scale the width of the particle volume
//  -- z-values scales the height of the particle volume
//  -- y-values are ignored
datablock afxPathData(SN_Smoke_path)
{
  points = "0 0 0"    SPC     // Note -- A default value with the same number of points as 
           "-1 0 4"   SPC     // there are values in the times list below is currently needed
           "-1 0 8"   SPC     // to avoid a console warning.
           "0.3 0 12" SPC
           "2 0 14"   SPC
           "4 0 12";
  points = "$$ SN_computePlumePoints(%%._expScale)";
  times = 0.0      SPC
          0.2*0.14 SPC
          0.4*0.14 SPC
          0.6*0.14 SPC
          0.8*0.14 SPC
          1.0*0.20;
  lifetime = 0.20;    
  concentric = true;
};
datablock afxPathData(SN_Smoke_path2)
{
  points = "$$ SN_computePlumePoints_Fill(%%._expScale)";
  lifetime = 0.4; 
  concentric = true;
};

datablock afxParticleEmitterDiscData(SN_Smoke_E)
{
  ejectionPeriodMS  = 2;
  periodVarianceMS  = 1;
  ejectionVelocity  = "$$ 3.5 * %%._expScale";
  velocityVariance  = "$$ 1.5 * %%._expScale";
  particles         = "SN_SmokeA_P SN_SmokeB_P";
  vector            = "0 0 1"; 
  radiusMin         = 0;
  radiusMax         = "$$ 1.5 * %%._expScale";
  blendStyle        = "PREMULTALPHA";
  pathsTransform    = "SN_Smoke_path";
  overrideAdvance   = true;
};
datablock afxParticleEmitterDiscData(SN_Smoke_Fill_E : SN_Smoke_E)
{
  pathsTransform = "SN_Smoke_path2";
  particles = "SN_SmokeA_Fill_P SN_SmokeB_Fill_P";
};

datablock afxEffectWrapperData(SN_Plume_EW)
{
  effect = SN_Smoke_E;
  posConstraint = "mine";
  lifetime = "$$ 0.2 * %%._expScale";
  delay = 0.10;
  sortPriority = 10;
};

datablock afxEffectWrapperData(SN_Plume_Fill_Night_EW : SN_Plume_EW)
{
  effectEnabled = "$$ %%._mood $= night";
  effect = SN_Smoke_Fill_E;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// SECONDARY DUST

//
// -- dust ball + dust ring
//

datablock afxParticleEmitterConeData(SN_Dust_E)
{
  ejectionPeriodMS  = 10;
  periodVarianceMS  = 3;
  ejectionVelocity  = "$$ 5*1.5 * %%._expScale";
  velocityVariance  = "$$ 2*1.5 * %%._expScale";
  particles         = "SN_SmokeA_P SN_SmokeB_P";
  vector            = "0 0 1"; 
  spreadMin         = 20;
  spreadMax         = 140;
  blendStyle        = "PREMULTALPHA";
};

datablock afxEffectWrapperData(SN_Dust_EW)
{
  effect = SN_Dust_E;
  constraint = "mine";
  delay = 0.03;
  lifetime = "$$ 0.15 * %%._expScale";
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock afxParticleEmitterVectorData(SN_DustRing2_E)
{
  ejectionOffset    = 0;
  ejectionPeriodMS  = 20;
  periodVarianceMS  = 5;
  ejectionVelocity  = "$$ 15 * %%._expScale";
  velocityVariance  = "$$ 2  * %%._expScale";
  particles         = "SN_SmokeC_P SN_SmokeD_P";
  blendStyle = "PREMULTALPHA";
  vector = "0.0 1.0 0.0";
};

datablock afxXM_SpinData(SN_DustRing2_spin_00_XM)
{
  spinAngle = "$$ (##*60)";
};

datablock afxXM_LocalOffsetData(SN_DustRing2_offset_XM)
{  
  fadeInTime = 1.0;
  localOffset = "$$ \"0\" SPC (15.0*%%._expScale) SPC \"0\"";
};

datablock afxXM_GroundConformData(SN_DustRing2_ground_XM)
{
  height = 1.0;
};

datablock afxXM_OscillateData(SN_DustRing2_oscillate_XM)
{
  mask  = $afxXfmMod::ORI;
  min   = "-30"; 
  max   = "30"; 
  axis  = "0 0 1";
  speed = 20;   
};

datablock afxEffectWrapperData(SN_DustRing2_00_EW)
{
  effect = SN_DustRing2_E;
  constraint = "mine";
  lifetime = 1.0;
  delay = 0.2; 
  xfmModifiers[0] = SN_DustRing2_spin_00_XM; 
    xfmModifiers[1] = SN_DustRing2_oscillate_XM;
    xfmModifiers[2] = SN_DustRing2_offset_XM;
    xfmModifiers[3] = SN_DustRing2_ground_XM;
};

datablock afxEffectGroupData(SN_DustRing_EG)
{
  assignIndices = true;
  count = 6;  
  addEffect = SN_DustRing2_00_EW;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// ROCK DEBRIS

datablock DebrisData(SN_RockDebris_ABC_CE)
{
  shapeFile = "$$ \"" @ %mySpellDataPath @ "/SN/models/\" @ SN_pickRockModel()";
  elasticity = 0.6;
  friction = 0.5;
  numBounces = 2;
  bounceVariance = 2;
  explodeOnMaxBounce = false;
  staticOnMaxBounce = false;
  snapOnMaxBounce = false;
  minSpinSpeed = 200;
  maxSpinSpeed = 700;
  render2D = false;
  lifetime = 3;
  lifetimeVariance = 0.5;
  velocity = 15;
  velocityVariance = 5.0;
  fade = true;
  useRadiusMass = true;
  baseRadius = 0.3;
  gravModifier = 0.8;
  terminalVelocity = 20;
  ignoreWater = true;
};

datablock afxXM_RandomRotData(SN_RockDebris_random_rot_XM)
{
  axis = "0 0 1";
  thetaMin = 30;
  thetaMax = 60;
};

datablock afxEffectWrapperData(SN_RockDebris_00_EW)
{
  effect = SN_RockDebris_ABC_CE;
  constraint = "mine";
  delay = "$$ getRandomF(0.0, 0.5)";
  xfmModifiers[0] = SN_RockDebris_random_rot_XM;
};
datablock afxEffectGroupData(SN_RockDebris_00_EG)
{
  count = "$$ getRandom(16,22)";
  addEffect = SN_RockDebris_00_EW;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// SHOCKWAVE

datablock afxZodiacData(SN_ShockWave_CE)
{  
  texture = %mySpellDataPath @ "/SN/zodiacs/sn_shockwave_candy";
  radius = 0.1;
  startAngle = 0.0;
  rotationRate = 0.0;
  color = "1.0 1.0 1.0 0.6";
  blend = additive;
  growthRate = 225;
  showOnInteriors = false;
};
//
datablock afxEffectWrapperData(SN_ShockWave_EW)
{
  effect = SN_ShockWave_CE;
  posConstraint = "mine";
  delay = 0.05;
  fadeInTime = 0.0;
  fadeOutTime = "$$ 0.7  * %%._expScale";
  lifetime    = "$$ 0.25 * %%._expScale";
};


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// BURN ZODE

datablock afxZodiacData(SN_BurntGroundZode_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/SN/zodiacs/sn_mineScorch";
  radius = "$$ 10 * %%._expScale";
  startAngle = "$$ getRandomF(0.0, 360.0)";
  rotationRate = 0;
  color = "0 0 0 0.85";
  blend = normal;
};
//
datablock afxEffectWrapperData(SN_BurntGroundZode_EW)
{
  effect = SN_BurntGroundZode_CE;
  constraint = "mine";
  delay = 0.4;
  fadeInTime = 0.5; 
  lifetime = 1.0;
  fadeOutTime = 5.0;
  residueLifetime = 20.0;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock afxAreaDamageData(SN_AreaDamage_CE)
{
  flavor = "soulmine";
  radius = 10;
  damage = 40;
  impulse = 1500;
  notifyDamageSource = true;
};
datablock afxEffectWrapperData(SN_AreaDamage_EW)
{
  effect = SN_AreaDamage_CE;
  constraint = "mine";
  delay = 0.1;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// SOUL NUKE IMPACT EFFECTRON
//

datablock afxAnimClipData(SN_KnockDown_CE)
{
  clipName = "death11";
  rate = 1.0;
  ignoreCorpse = true;
};
//
datablock afxEffectWrapperData(SN_KnockDown_EW)
{
  effect = SN_KnockDown_CE;
  constraint = "victim";
  delay = 0;
  lifetime = 2.3;
};

datablock afxAnimClipData(SN_StayDown_CE)
{
  clipName = "death11";
  posOffset = 0.8;
  rate = 0.001;
  ignoreCorpse = true;
};
//
datablock afxEffectWrapperData(SN_StayDown_EW)
{
  effect = SN_StayDown_CE;
  constraint = "victim";
  delay = 2.2;
  lifetime = "$$ %%._downTime";
};

datablock afxAnimClipData(SN_GetUp_CE)
{
  clipName = "death2";
  rate = -1;
  transitionTime = 0.5;
  ignoreCorpse = true;
};
//
datablock afxEffectWrapperData(SN_GetUp_EW)
{
  effect = SN_GetUp_CE;
  constraint = "victim";
  lifetime = 2.2;
  delay = "$$ 2.2 + %%._downTime - 0.1";
};

datablock afxAnimLockData(SN_AnimLock_CE)
{
  priority = 0;
};
//
datablock afxEffectWrapperData(SN_AnimLock_EW)
{
  effect = SN_AnimLock_CE;
  constraint = "victim";
  delay = 0.0;
  lifetime = "$$ 2.2 + %%._downTime + 2.2";
};

datablock afxEffectronData(SoulNukeImpactEffectron)
{
  addEffect = SN_AnimLock_EW;
  addEffect = SN_KnockDown_EW;
  addEffect = SN_StayDown_EW;
  addEffect = SN_GetUp_EW;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// AUDIO AND LIGHTING

switch$ (afxGetEngine())
{
  case "TGE":
    exec("./lighting/sn_lighting_tge_sub.cs");
    exec("./audio/sn_audio_sub.cs");
  case "TGEA":
    exec("./lighting/sn_lighting_tgea_sub.cs");
    exec("./audio/sn_audio_sub.cs");
 case "T3D":
    exec("./lighting/sn_lighting_t3d_sub.cs");
    exec("./audio/sn_audio_sub.cs");
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// SOUL NUKE EFFECTRON
//
datablock afxEffectronData(SoulNukeEffectron)
{
  echoPacketUsage = 20;
  clientScriptFile = %mySpellDataPath @ "/SN/sn_client.cs";
  clientInitFunction = "sn_clientInit";

  duration = 1.0;
  numLoops = 1;
  execOnNewClients = true;
  
    // explosion flash //
  addEffect = SN_Fire_EW;
    // dust plume //  
  addEffect = SN_Plume_EW;
  addEffect = SN_Plume_Fill_Night_EW;
    // rock debris //
  addEffect = SN_RockDebris_00_EG;
    // secondary dust //
  addEffect = SN_Dust_EW;
  addEffect = SN_DustRing_EG;
    // burn zode //
  addEffect = SN_BurntGroundZode_EW;      
    // shockwave //
    //   (note: added after burnt ground zode, so composited on top)
  addEffect = SN_ShockWave_EW;
  addEffect = SN_AreaDamage_EW;

    // default parameter settings //
  _expScale = 1.0; 
  _mood = "day";
};

// sounds and lights added via sub-script functions //
SN_add_Lighting_FX(SoulNukeEffectron);
SN_add_Audio_FX(SoulNukeEffectron);

function SoulNukeEffectron::onActivate(%this, %effectron)
{  
  if (isObject(theLevelInfo))
    %night_mission = theLevelInfo.isNightMission;
  else if (isObject(MissionInfo))
    %night_mission = MissionInfo.isNightMission;

  %effectron._mood = (%night_mission) ? "night" : "day"; 
}

// Notifies the Stomper when stomp damage is done to something. Could be used to assign
// response effects instead of the built-in impulse response.
function SoulNukeEffectron::onInflictedAreaDamage(%this, %effectron, %targetObject, %damage, %damageType, %position)
{
  %impact_effectron = startEffectron(SoulNukeImpactEffectron, %targetObject, "victim");
  if (!isObject(%impact_effectron))
    return;
    
  %impact_effectron._downTime = (%damage/40) * 2.0;

  //UAISK+AFX Interop Changes: Start
  if ($UAISK_Is_Available && %targetObject.isBot)
  {
    %dazed = 0.5;
    %targetObject.schedule(0.0, "doSpecialMove", (2.2 + %impact_effectron._downTime + 2.2 + %dazed)*1000);
  }
  //UAISK+AFX Interop Changes: End
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// SOUL NUKE SPELL
//

datablock afxMagicSpellData(SoulNukeSpell)
{
  castingDur = 0;
};

datablock afxRPGMagicSpellData(SoulNukeSpell_RPG)
{
  spellName = "Soul Nuke";
  desc = "Detonates a colorful magic explosion that forms a small " @
         "mushroom cloud shape." @
         "\n\n" @
         "[reusable element]";
  sourcePack = "Spell Pack 2";
  iconBitmap = %mySpellDataPath @ "/SN/icons/sn";
  manaCost = 10;
  castingDur = 0;
  target = "free";
};

function SoulNukeSpell::onLaunch(%this, %spell, %caster, %target, %missile)
{
  Parent::onLaunch(%this, %spell, %caster, %target, %missile);

  %anchor = %spell.freeTarget;

  %mine_explosion = startEffectron(SoulNukeEffectron, %anchor, "mine");
  %mine_explosion._expScale = getRandomF(0.9, 3);
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
  SoulNukeSpell.scriptFile = $afxAutoloadScriptFile;
  SoulNukeSpell.spellDataPath = %mySpellDataPath;
  if (isFunction(addDemoSpellbookSpell))
  {
    addDemoSpellbookSpell(SoulNukeSpell, SoulNukeSpell_RPG);
  }
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
