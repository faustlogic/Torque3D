//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// DUST RING SPELL
//
//  Stirs up a cloud of dust that radiates outward in a ring shape.
//
//    Parameters:
//      _castDur          (float)
//      _scale            (float)   -- overall effect scale
//      _radiusStart      (float)
//      _radiusEnd        (float)
//      _lifetime         (float)
//      _particleScale    (float)
//      _casterAnchor     (bool)
//      _fixedAnchor      (bool)
//      _ejectionPeriodMS (integer) -- higher values decreases cloud density
//      _dustOvershoot    (bool)    --
//      _spread           (float)   -- describes arc of coverage, 360 produces complete ring
//      _mood             (string)  -- "day", "night"
//
//    Internal Params:
//      _emitterCnt       (integer) -- derived from _spread
//      _emitterSpread    (float)   -- derived from _spread and _emitterCnt
//      _dustOffset       (vector)  -- derived from _radiusStart, _radiusEnd, and _dustOvershoot
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
$spell_reload = isObject(RingOfDustSpell);
if ($spell_reload)
{
  // mark datablocks so we can detect which are reloaded this script
  markDataBlocks();
  // reset data path from previously saved value
  %mySpellDataPath = RingOfDustSpell.spellDataPath;
}
else
{
  // set data path from default plus containing folder name
  %mySpellDataPath = $afxSpellDataPath @ "/" @ $afxAutoloadScriptFolder;
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock ParticleData(RoD_SmokeA_P)
{
  textureName          = "$$ \"" @ %mySpellDataPath @ "/Shared/particles/sp2_dust_2x2_\" @ " @ "%%._mood";
  textureCoords[0]     = "0.0 0.0";
    textureCoords[1]     = "0.0 0.5";
    textureCoords[2]     = "0.5 0.5";
    textureCoords[3]     = "0.5 0.0";
  dragCoefficient      = 0.0; //1;
  gravityCoefficient   = 0;
  inheritedVelFactor   = 0.0;
  lifetimeMS           = 2000;
  lifetimeVarianceMS   = 750; 
  randomizeSpinDir     = true;
  spinRandomMin        = 100.0;
  spinRandomMax        = 150.0; 
  colors[0]            = "0.0 0.0 0.0 0.0";
    colors[1]            = "$$ %%._color";
    colors[2]            = "$$ ColorScale(%%._color,0.7)";
    colors[3]            = "$$ ColorScale(%%._color,0.2)";
    colors[4]            = "0.0 0.0 0.0 0.0";
  sizeBias             = "$$ %%._scale * %%._particleScale";
  sizes[0]             = 10;
    sizes[1]             = 15;
    sizes[2]             = 20;
    sizes[3]             = 25;
    sizes[4]             = 30;
  times[0]             = 0.0;
    times[1]             = 0.05;
    times[2]             = 0.15;
    times[3]             = 0.3;
    times[4]             = 1.0;
};

datablock ParticleData(RoD_SmokeB_P : RoD_SmokeA_P)
{
  textureCoords[0] = "0.5 0.0";
    textureCoords[1] = "0.5 0.5";
    textureCoords[2] = "1.0 0.5";
    textureCoords[3] = "1.0 0.0";
};

datablock afxParticleEmitterVectorData(RoD_DustRing_00_E)
{
  ejectionOffset    = 0;
  ejectionPeriodMS  = "$$ %%._ejectionPeriodMS";
  periodVarianceMS  = "$$ mFloor( 0.25 * %%._ejectionPeriodMS )";
  ejectionVelocity  = "$$ 15 * %%._scale"; // 15
  velocityVariance  = "$$ 2  * %%._scale";
  particles         = "RoD_SmokeA_P RoD_SmokeB_P";
  blendStyle        = "PREMULTALPHA";
  vector            = "$$ %%._dustOvershoot ? \"0.0 1.0 0.0\" : \"0.0 -1.0 0.0\"";
  fadeVelocity      = true;
  softnessDistance  = 5.0;
};

//~~~~~~~~~~~~~~~~~~~~//

datablock afxXM_SpinData(RoD_DustRing_spin00_XM)
{
  spinAngle = "$$ (##*%%._emitterSpread) - (%%._spread*0.5)";
  spinRate  = 0.0;
};

datablock afxXM_OscillateData(RoD_DustRing_oscillate_XM)
{
  mask  = $afxXfmMod::ORI;
  min = 0;
  max = "$$ %%._emitterSpread";
  axis  = "0 0 1";
  speed = 20;   
};

datablock afxXM_LocalOffsetData(RoD_DustRing_offset_XM)
{
  localOffset = "$$ %%._dustOffset";
  fadeInTime = "$$ (%%._lifetime+0.5)*%%._scale";
};

// initial radius
datablock afxXM_LocalOffsetData(RoD_DustRing_offset0_XM)
{
  localOffset = "$$ \"0\" SPC (%%._radiusStart*%%._scale) SPC \"0\"";
};

datablock afxXM_GroundConformData(RoD_DustRing_ground_XM)
{
  height = "$$ 10.0 * %%._scale * %%._particleScale";
  fadeInTime = "$$ (%%._lifetime*%%._scale)*(1.5/2.5)";
};

//~~~~~~~~~~~~~~~~~~~~//

datablock afxEffectWrapperData(RoD_DustRing_00_EW)
{
  effect = RoD_DustRing_00_E;
  constraint = "anchor";
  lifetime    = "$$ %%._lifetime * %%._scale";
  fadeInTime  = "$$ %%._lifetime * %%._scale";
  
  xfmModifiers[0] = RoD_DustRing_spin00_XM;  
  xfmModifiers[1] = RoD_DustRing_oscillate_XM;
  xfmModifiers[2] = RoD_DustRing_offset_XM;
  xfmModifiers[3] = RoD_DustRing_offset0_XM;
  xfmModifiers[4] = RoD_DustRing_ground_XM;
};

//~~~~~~~~~~~~~~~~~~~~//

datablock afxEffectGroupData(RoD_DustRing_EG)
{
  assignIndices = true;
  count = "$$ %%._emitterCnt";  
  addEffect = RoD_DustRing_00_EW;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// RING OF DUST SPELL
//

datablock afxMagicSpellData(RingOfDustSpell)
{
  echoPacketUsage = 20;
  castingDur = "$$ %%._castDur";

  addCastingEffect = RoD_DustRing_EG;

  _castDur = 0;
  _fixedAnchor = 1;
  _color = "0.7 0.7 0.7 1.0";
};

datablock afxRPGMagicSpellData(RingOfDustSpell_RPG)
{
  spellName = "Ring of Dust";
  desc = "Stirs up a cloud of dust that radiates outward in a ring shape." @
         "\n\n" @
         "[reusable element]";
  sourcePack = "Spell Pack 2";
  iconBitmap = %mySpellDataPath @ "/RoD/icons/rod";
  manaCost = 10;
  castingDur = 0;
  
  _castDur = 0;
  _scale =  1.0; 
  _radiusStart = 5.0;
  _radiusEnd   = 40.0;
  _lifetime    = 2;
  _spread      = 360;
  _particleScale = 0.4;
  _ejectionPeriodMS = 30;
  _dustOvershoot = true;
  _casterAnchor = 1;
  _fixedAnchor = 1;
  _color = "0.7 0.7 0.7 1.0";
};

function RingOfDustSpell::onActivate(%this, %spell, %caster, %target)
{
  Parent::onActivate(%this, %spell, %caster, %target);
  
  // set "anchor" constraint to the caster's transform
  if (%spell._casterAnchor)
  {
    if (%spell._fixedAnchor)
      %spell.addConstraint(%caster.getTransform(), "anchor");
    else
      %spell.addConstraint(%caster, "anchor");
  }

  %night_mission = false;
  if (isObject(theLevelInfo))
    %night_mission = theLevelInfo.isNightMission;
  else if (isObject(MissionInfo))
    %night_mission = MissionInfo.isNightMission;
  %spell._mood = (%night_mission) ? "night" : "day"; 

  %dust_offset = (%spell._radiusEnd-%spell._radiusStart)*%spell._scale;
  if (%spell._dustOvershoot)
    %dust_offset *= (90.0/75.0);
  %spell._dustOffset = "0" SPC %dust_offset SPC "0";
 
  if (%spell._spread > 360.0)
    %spell._spread = 360;

  %spell._emitterCnt = mCeil(%spell._spread/60);
  %spell._emitterSpread = (%spell._emitterCnt != 0) ? (%spell._spread/%spell._emitterCnt) : 0;
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
  RingOfDustSpell.scriptFile = $afxAutoloadScriptFile;
  RingOfDustSpell.spellDataPath = %mySpellDataPath;
  if (isFunction(addDemoSpellbookSpell))
  {
    addDemoSpellbookSpell(RingOfDustSpell, RingOfDustSpell_RPG);
  }
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//