
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// RING OF FIRE SPELL
//
//  Encircle yourself with an incinerating wall of fire.
//  Love is a burning thing, And it makes a fiery ring.
//
//    parameters:
//      _anim           (boolean)     default: true
//      _ringDur        (float)       default: 3.1 or 10.6
//      _ringRadius     (float)       default: 11.63 
//      _castDur        (float)       default: 0.0
//      _flavor         (string)      default: "swirl"   ["swirl", "spider"]
//
//    internal:
//      _swirl          (boolean)
//      _spider         (boolean)
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
$spell_reload = isObject(RingOfFireSpell);
if ($spell_reload)
{
  // mark datablocks so we can detect which are reloaded this script
  markDataBlocks();
  // reset data path from previously saved value
  %mySpellDataPath = RingOfFireSpell.spellDataPath;
}
else
{
  // set data path from default plus containing folder name
  %mySpellDataPath = $afxSpellDataPath @ "/" @ $afxAutoloadScriptFolder;

  afxExecPrerequisite("sp2_shared_fx.cs", $afxAutoloadScriptFolder);
  afxExecPrerequisite("up_in_flames.cs", $afxAutoloadScriptFolder);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// RING OF FIRE

//~~~~~~~~~~~~~~~~~~~~//
// Misc.

datablock afxXM_AimData(RoF_Aim_XM)
{
  aimZOnly = true;
};

//~~~~~~~~~~~~~~~~~~~~//
// Caster Animation

datablock afxAnimClipData(RoF_Casting_Clip_CE)
{
  clipName = "rof";
  ignoreCorpse = true;
  lockAnimation = true;
  rate = 1.0;
};
//
datablock afxEffectWrapperData(RoF_Casting_Clip_EW)
{
  effectEnabled = "$$ %%._anim";
  effect = RoF_Casting_Clip_CE;
  constraint = caster;
  lifetime = (140/30)-(10/30);
  delay = 0.0;
};

//~~~~~~~~~~~~~~~~~~~~//
// Script Event

datablock afxScriptEventData(RoF_FirstRingBlastScript_CE)
{
  methodName = "FirstRingBlast";
};
datablock afxEffectWrapperData(RoF_FirstRingBlastScript_EW)
{
  effect = RoF_FirstRingBlastScript_CE;
  constraint = "RingAnchor";
  delay = 3.1;
};

//~~~~~~~~~~~~~~~~~~~~//
// Fire Particles

datablock ParticleData(RoF_Fire_A_SM_P : SP2_Fire_B_P)
{
  sizeBias = 0.2;
};

datablock ParticleEmitterData(RoF_Fire_SM_E : SP2_Fire_E)
{
  particles         = "RoF_Fire_A_SM_P";
};

datablock afxXM_GroundConformData(RoF_RingFire_Ground_XM)
{
  height = 0.25;
};

//~~~~~~~~~~~~~~~~~~~~//
// Client Mooring

datablock afxMooringData(RoF_ClientMooring_CE)
{
  networking = $AFX::CLIENT_ONLY;
  displayAxisMarker = false;
};

//~~~~~~~~~~~~~~~~~~~~//
// Ring Fire Zodiac

datablock afxZodiacData(RoF_RingFire_Zode_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/RoF/zodiacs/rof_fire_spot";
  radius = 0.5;
  startAngle = 0;
  rotationRate = 0.0;  
  color = "1.0 1.0 1.0 1.0";
  blend = additive;
};

//~~~~~~~~~~~~~~~~~~~~//
// Ring Burns Zodiacs

datablock afxZodiacData(RoF_RingBurns_Halo_CE)
{  
  texture = %mySpellDataPath @ "/RoF/zodiacs/rof_halo_ring";
  radius = 11.0*1.3;
  startAngle = 0;
  rotationRate = 0.0;
  color = "1.0 1.0 1.0 1.0";
  blend = additive;
  showOnInteriors = false;
};

datablock afxZodiacData(RoF_RingBurns_Burn_CE)
{  
  texture = %mySpellDataPath @ "/RoF/zodiacs/rof_burn_ring";
  radius = 10.0*1.3;
  startAngle = 0;
  rotationRate = 0.0;  
  color = "0 0 0 1";
  blend = normal;
  showOnInteriors = true;
  useGradientRange = true;
  gradientRange = "0 45";
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// SWIRL FLAVOR

// Inner Ring
datablock afxZodiacData(RoF_FlowerZode_Embers_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/RoF/zodiacs/rof_ember_flower";
  radius = 2.7;
  startAngle = 0.0;
  rotationRate = 0.0;
  color = "1.0 1.0 1.0 1.0";
  blend = additive;
  interiorHorizontalOnly = true;
};
datablock afxEffectWrapperData(ROF_FlowerZode_Embers_EW) // SWIRL
{
  effect = RoF_FlowerZode_Embers_CE;
  constraint = "RingAnchor";
  delay = 0;
  fadeInTime = 0.75;
  fadeOutTime = 1.0;
  lifetime = 0.75;
};

datablock afxZodiacData(ROF_FlowerZode_Burn_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/RoF/zodiacs/rof_burn_flower";
  radius = 2.7;
  startAngle = 0.0;
  rotationRate = 0.0;
  color = "0 0 0 0.75";
  blend = normal;
  interiorHorizontalOnly = true;
};
datablock afxEffectWrapperData(ROF_FlowerZode_Burn_EW) // SWIRL
{
  effect = ROF_FlowerZode_Burn_CE;
  constraint = "RingAnchor";
  delay = 0.75;
  fadeInTime  = 1.0;
  fadeOutTime = 3.0;
  lifetime = 5.0;
};

datablock afxZodiacData(RoF_CirclesZode_Burn_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/RoF/zodiacs/rof_burn_circles";
  radius = 2.0;
  startAngle = 0;
  rotationRate = 0.0;  
  color = "0 0 0 1";
  blend = normal;
};
datablock afxEffectWrapperData(RoF_CirclesZode_Burn_EW)
{
  effect = RoF_CirclesZode_Burn_CE;
  constraint = "RingAnchor";
  delay = 2.5- 1.0;
  lifetime = 6.0; //5.0;
  fadeInTime = 0.5;
  fadeOutTime = 2.0;  
};

datablock afxZodiacData(RoF_SwirlZode_Burn_CE)
{  
  texture = %mySpellDataPath @ "/RoF/zodiacs/rof_burn14_swirl";
  radius = 10.0*1.3;
  startAngle = 12;
  rotationRate = 0.0;  
  color = "0 0 0 1";
  blend = normal;
  showOnInteriors = false;
};
datablock afxEffectWrapperData(RoF_SwirlZode_Burn_EW)
{
  effect = RoF_SwirlZode_Burn_CE;
  constraint = "RingAnchor";
  delay = 2.55;
  lifetime = 5.0;
  fadeInTime = 0.5;
  fadeOutTime = 2.0;  
  scaleFactor = "$$ %%._ringRadius/11.63";
};

datablock afxZodiacData(RoF_SwirlZode_Embers_CE)
{  
  texture = %mySpellDataPath @ "/RoF/zodiacs/rof_ember14_swirl";
  radius = 10.0*1.3;
  startAngle = 12;
  rotationRate = 0.0;  
  color = "1.0 1.0 1.0 1.0";
  blend = additive;
  showOnInteriors = false;
};
datablock afxEffectWrapperData(RoF_SwirlZode_Embers_EW)
{
  effect = RoF_SwirlZode_Embers_CE;
  constraint = "RingAnchor";
  delay = 2.86;
  lifetime = 0.5;
  fadeInTime = 0.5;
  fadeOutTime = 1.5;
  scaleFactor = "$$ %%._ringRadius/11.63";
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// Ring Fire

// moorings

datablock afxXM_SpinData(RoF_FireRing_spinA_0_XM)
{
  spinAxis = "0 0 1";
  spinAngle = "$$ (360.0/14)*##";
  spinRate = 120;
  delay = "$$ 2.0 - (0.1*##)";
  fadeInTime = 1.0;
};
datablock afxXM_LocalOffsetData(RoF_FireRing_Radius0_offset_XM)
{
  localOffset = "0 1.5 0";  
};
datablock afxXM_LocalOffsetData(RoF_FireRing_Radius1_offset_0_XM)
{
  localOffset = "$$ 0 SPC (%%._ringRadius - 1.5) SPC 0";
  delay = "$$ 2.0 - (0.1*##)";
  fadeInTime = 0.6;
};

datablock afxEffectWrapperData(RoF_RingFire_Mooring_Swirl_00_EW)
{
  effect = RoF_ClientMooring_CE;
  effectName = "$$ Swirl_Mooring##";
  isConstraintSrc = true;
  constraint = "RingAnchor";
  delay = "$$ 0.5 + (0.1*##)";
  lifetime = "$$ %%._ringDur";
  xfmModifiers[0] = RoF_FireRing_spinA_0_XM;
  xfmModifiers[1] = RoF_FireRing_Radius0_offset_XM;  
  xfmModifiers[2] = RoF_FireRing_Radius1_offset_0_XM;
  xfmModifiers[3] = RoF_RingFire_Ground_XM;
};

// starter fires

datablock afxEffectWrapperData(RoF_RingFire_Starters_Swirl_00_EW)
{
  effect = RoF_Fire_SM_E;
  constraint = "$$ \"#effect.Swirl_Mooring##\"";
  delay = "$$ 0.5 + (0.1*##)";
  lifetime = "$$ 2.0 - (0.1*##)";
  fadeInTime = 0.25;
  fadeOutTime = 0.25;
};

// flames

datablock afxEffectWrapperData(RoF_RingFire_Flames_Swirl_00_EW)
{
  effect = SP2_Fire_E;
  constraint = "$$ \"#effect.Swirl_Mooring##\"";
  delay = 2.5;
  lifetime = "$$ %%._ringDur - 2.6";
  fadeInTime = 0.6;
  fadeOutTime = 0.25;
};

//~~~~~~~~~~~~~~~~~~~~//

datablock afxXM_ScaleData(Rof_FireBounce_scale_0_XM)
{
  scale = "4 4 4";
  delay = "$$ 2.0 - (0.1*##)";
  fadeInTime = 0.5*0.6;
};
datablock afxEffectWrapperData(RoF_RingFire_Zode_Swirl_00_EW)
{
  effect = RoF_RingFire_Zode_CE;
  constraint = "$$ \"#effect.Swirl_Mooring##\"";
  delay = "$$ 0.5 + (0.1*##)";
  lifetime = "$$ 2.0 - (0.1*##) + 0.48";
  fadeInTime = 0.2;
  fadeOutTime = 0.25;
  xfmModifiers[0] = Rof_FireBounce_scale_0_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// Ring Burns

datablock afxEffectWrapperData(RoF_RingBurns_Halo_Swirl_EW)
{
  effect = RoF_RingBurns_Halo_CE;
  constraint = "RingAnchor";
  delay = 3.1; 
  lifetime = "$$ %%._ringDur - 3.6";
  fadeInTime = 1.0;
  fadeOutTime = 1.0;  
  scaleFactor = "$$ %%._ringRadius/11.63";
};

datablock afxEffectWrapperData(RoF_RingBurns_Burn_Swirl_EW)
{
  effect = RoF_RingBurns_Burn_CE;
  constraint = "RingAnchor";
  delay = "$$ %%._ringDur - 0.5";
  lifetime = 10;
  fadeInTime = 0.5;
  fadeOutTime = 6.0;
  residueLifetime = 2.5;
  scaleFactor = "$$ %%._ringRadius/11.63";
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// SPIDER FLAVOR

// Inner Ring

datablock afxZodiacData(RoF_SpiderZode_Burn_CE)
{  
  texture = %mySpellDataPath @ "/RoF/zodiacs/rof_burn14_spider";
  radius = 10.0*1.3;
  startAngle = 0;
  rotationRate = 0.0;  
  color = "0 0 0 1";
  blend = normal;
  showOnInteriors = false;
};
datablock afxEffectWrapperData(RoF_SpiderZode_Burn_EW)
{
  effect = RoF_SpiderZode_Burn_CE;
  constraint = "RingAnchor";
  delay = 1.0;
  lifetime = 10.0;
  fadeInTime = 0.5;
  fadeOutTime = 2.0;
};

datablock afxZodiacData(RoF_SpiderZode_Embers_CE)
{  
  texture = %mySpellDataPath @ "/RoF/zodiacs/rof_ember14_spider";
  radius = 10.0*1.3;
  startAngle = 0;
  rotationRate = 0.0;  
  color = "1.0 1.0 1.0 1.0";
  blend = additive;
  showOnInteriors = false;
};
datablock afxEffectWrapperData(RoF_SpiderZode_Embers_EW)
{
  effect = RoF_SpiderZode_Embers_CE;
  constraint = "RingAnchor";
  delay = 1.0;
  lifetime = 0.8;
  fadeInTime = 0.8;
  fadeOutTime = 1.5;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// Ring Fire

datablock afxPathData(RoF_FireRing_Spider_00_Path)
{
  points = "$$ RoF_getFirePath(##)";
  lifetime = 1.0;
};

datablock afxXM_SpinData(RoF_RingFire_Spin_Spider_XM)
{
  spinAxis = "0 0 1";
  spinAngle = 0;
  spinRate = 120;
  delay = 0.8;
  fadeInTime = 0.7;
};

datablock afxXM_PathConformData(RoF_RingFire_Path_Spider_00_XM)
{
  paths = "RoF_FireRing_Spider_00_Path";
  pathMult = 0.26*1.3;
};

// moorings

datablock afxEffectWrapperData(RoF_RingFire_Mooring_Spider_00_EW)
{
  effect = RoF_ClientMooring_CE;
  effectName = "$$ Spider_Mooring##";
  isConstraintSrc = true;
  constraint = "RingAnchor";
  delay = 0.7;
  lifetime = "$$ %%._ringDur";
  xfmModifiers[0] = RoF_RingFire_Spin_Spider_XM;
  xfmModifiers[1] = RoF_RingFire_Path_Spider_00_XM;
  xfmModifiers[2] = RoF_RingFire_Ground_XM;
};

// flames

datablock afxEffectWrapperData(RoF_RingFire_Flames_Spider_00_EW)
{
  effect = SP2_Fire_E;
  constraint = "$$ \"#effect.Spider_Mooring##\"";
  delay = 0.7;
  lifetime = "$$ %%._ringDur + 0.5";
  fadeInTime = 1.0;
  fadeOutTime = 0.25;
};

// fire zode

datablock afxEffectWrapperData(RoF_RingFire_Zode_Spider_00_EW)
{
  effect = RoF_RingFire_Zode_CE;
  constraint = "$$ \"#effect.Spider_Mooring##\"";
  delay = 0.7;
  lifetime = "$$ %%._ringDur - 0.25";
  fadeInTime = 0.2;
  fadeOutTime = 0.25;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// Ring Burns

datablock afxEffectWrapperData(RoF_RingBurns_Halo_Spider_EW)
{
  effect = RoF_RingBurns_Halo_CE;
  constraint = "RingAnchor";
  delay = "$$ %%._ringDur - 0.8"; 
  lifetime = 1.8;
  fadeInTime = 1.0;
  fadeOutTime = 1.0; 
};

datablock afxEffectWrapperData(RoF_RingBurns_Burn_Spider_EW)
{
  effect = RoF_RingBurns_Burn_CE;
  constraint = "RingAnchor";
  delay = "$$ %%._ringDur + 0.7";
  lifetime = 10;
  fadeInTime = 0.5;
  fadeOutTime = 2.0;
};

datablock afxZodiacData(RoF_RingBurns_Embers_Spider_CE)
{  
  texture = %mySpellDataPath @ "/RoF/zodiacs/rof_ember_ring";
  radius = 10.0*1.3;
  startAngle = 0;
  rotationRate = 0.0;  
  color = "1.0 1.0 1.0 1.0";
  blend = additive;
  showOnInteriors = false;
};
datablock afxEffectWrapperData(RoF_RingBurns_Embers_Spider_EW)
{
  effect = RoF_RingBurns_Embers_Spider_CE;
  constraint = "RingAnchor";
  delay = "$$ %%._ringDur + 6.3";
  lifetime = 0.5;
  fadeInTime = 0.5;
  fadeOutTime = 0.8;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// AUDIO AND LIGHTING

switch$ (afxGetEngine())
{
  case "TGE":
    exec("./lighting/rof_lighting_tge_sub.cs");
    exec("./audio/rof_audio_sub.cs");
  case "TGEA":
    exec("./lighting/rof_lighting_tgea_sub.cs");
    exec("./audio/rof_audio_sub.cs");
 case "T3D":
    exec("./lighting/rof_lighting_t3d_sub.cs");
    exec("./audio/rof_audio_sub.cs");
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// GROUPS

// Inner Ring Group (swirl)
datablock afxEffectGroupData(RoF_InnerRing_Swirl_EG)
{
  groupEnabled = "$$ %%._swirl == true";
  count = 1;
    // inner ring zodiacs //
  addEffect = ROF_FlowerZode_Burn_EW;
  addEffect = ROF_FlowerZode_Embers_EW;
  addEffect = RoF_CirclesZode_Burn_EW;
  addEffect = RoF_SwirlZode_Burn_EW;
  addEffect = RoF_SwirlZode_Embers_EW;
    // inner ring lights //
  addEffect = $RoF_CastingLight;
  addEffect = $RoF_FireballRingLight;
  addEffect = $RoF_FireBurstLight;
};

// Ring Fire Group (swirl)
datablock afxEffectGroupData(RoF_RingFire_Swirl_EG)
{
  groupEnabled = "$$ %%._swirl == true";
  count = 14;
  assignIndices = true;
  addEffect = RoF_RingFire_Mooring_Swirl_00_EW;
  addEffect = RoF_RingFire_Starters_Swirl_00_EW;
  addEffect = RoF_fireIgniters_SND_00_EW;
  addEffect = RoF_RingFire_Flames_Swirl_00_EW;
  addEffect = $RoF_RingFire_Light_Swirl_00;
  addEffect = RoF_RingFire_Zode_Swirl_00_EW;
};

// Ring Burns Group (swirl)
datablock afxEffectGroupData(RoF_RingBurns_Swirl_EG)
{
  groupEnabled = "$$ %%._swirl == true";
  count = 1;
  addEffect = RoF_RingBurns_Halo_Swirl_EW;
  addEffect = RoF_RingBurns_Burn_Swirl_EW;
};

// Inner Ring Group (spider)
datablock afxEffectGroupData(RoF_InnerRing_Spider_EG)
{
  groupEnabled = "$$ %%._spider == true";
  count = 1;
  addEffect = RoF_SpiderZode_Burn_EW;
  addEffect = RoF_SpiderZode_Embers_EW;
};

// Ring Fire Group (spider)
datablock afxEffectGroupData(RoF_RingFire_Spider_EG)
{
  groupEnabled = "$$ %%._spider == true";
  count = 14;
  assignIndices = true;
  addEffect = RoF_RingFire_Mooring_Spider_00_EW;
  addEffect = RoF_RingFire_Flames_Spider_00_EW;
  addEffect = RoF_RingFire_Zode_Spider_00_EW;
};

// Ring Burns Group (spider)
datablock afxEffectGroupData(RoF_RingBurns_Spider_EG)
{
  groupEnabled = "$$ %%._spider == true";
  count = 1;
  addEffect = RoF_RingBurns_Halo_Spider_EW;
  addEffect = RoF_RingBurns_Burn_Spider_EW;
  addEffect = RoF_RingBurns_Embers_Spider_EW;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// RING OF FIRE SPELL
//

datablock afxMagicSpellData(RingOfFireSpell)
{
  echoPacketUsage = 20;
  clientScriptFile = %mySpellDataPath @ "/RoF/rof_client.cs";
  clientInitFunction = "RoF_clientInit";

  castingDur = "$$ %%._castDur";
  
    // caster animation //
  addCastingEffect = RoF_Casting_Clip_EW;

    // assign burn script //
  addCastingEffect = RoF_FirstRingBlastScript_EW;

    // swirl variation //
  addCastingEffect = RoF_InnerRing_Swirl_EG;
  addCastingEffect = RoF_RingFire_Swirl_EG;
  addCastingEffect = RoF_RingBurns_Swirl_EG; 

    // spider variation //
  addCastingEffect = RoF_InnerRing_Spider_EG;
  addCastingEffect = RoF_RingFire_Spider_EG;
  addCastingEffect = RoF_RingBurns_Spider_EG;

    // default params
  _castDur = 0;
  _anim = 1;
  _casterAnchor = 0;
  _ringDur = 10.6;
  _ringRadius = 11.63;
  _flavor = "swirl";
};

// sounds added via sub-script functions //
RoF_add_Audio_FX(RingOfFireSpell);

datablock afxRPGMagicSpellData(RingOfFireSpell_RPG)
{
  spellName = "Ring of Fire";
  desc = "Encircle yourself with an incinerating wall of fire.\n" @
         "<font:Arial:6>\n" @
         "<just:center><font:Arial Italic:14>Love is a burning thing, And it makes a fiery ring.<just:left>\n" @
         "\n" @
         "<font:Arial Italic:14>spell design: <font:Arial:14>Matthew Durante\n" @
         "<font:Arial Italic:14>sound design: <font:Arial:14>Matt Pacyga\n" @ 
         "<font:Arial Italic:14>spell concept: <font:Arial:14>Jeff Faust";
  sourcePack = "Spell Pack 2";
  iconBitmap = %mySpellDataPath @ "/RoF/icons/rof";
  manaCost = 10;
  castingDur = 4.0;
  _castDur = 4.0;
  _casterAnchor = 1;
};

datablock afxRPGMagicSpellData(RingOfFireSpell2_RPG)
{
  spellName = "Ring of Fire 2";
  desc = "";
  sourcePack = "Spell Pack 2";
  iconBitmap = %mySpellDataPath @ "/RoF/icons/rof";
  manaCost = 10;
  castingDur = 0;
  _castDur = 0;
  _anim = 0;
  _casterAnchor = 1;
  _ringDur = 3.1;
  _ringRadius = 11.63;
  _flavor = "spider";
};

function RingOfFireSpell::onActivate(%this, %spell, %caster, %target)
{
  Parent::onActivate(%this, %spell, %caster, %target);

  if (%spell._casterAnchor)
    %spell.addConstraint(%caster.getTransform(), "RingAnchor");

  %spell._spider = (%spell._flavor $= "spider");
  %spell._swirl = !%spell._spider;

  /* This is for testing radius and duration variations
  if (%spell._swirl)
  {
    %spell._ringDur = 10.6*(1.0 + getRandomF(-1.0,1.0)*0.2);
    %spell._ringRadius = 11.63*(1.0 + getRandomF(-1.0,1.0)*0.2);
  }
  */
}

//
// This script starts up the ring-monitor which detects when objects cross
// the ring and sets them on fire.
//
function RingOfFireSpell::FirstRingBlast(%this, %spell, %caster, %cons, %xfm, %data)
{
  %pos = getWords(%xfm, 0, 2);

  %effectron = new afxEffectron() 
  {
    datablock = RoF_RingMonitorEffectron;
    _ringRadius = %spell._ringRadius;
    _ringDur = %spell._ringDur;
  };
  %effectron.addConstraint(%pos, "RingAnchor");

  // if caster is close to center the inside[] list is initialized with caster 
  // so that he avoids initial burn
  if (VectorDist(%caster.getPosition(), %pos) < 5.0)
  {
    %effectron.inside[0] = %caster;
    %effectron.n_inside = 1;
  }
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

exec("./special/rof_burner_sub.cs");

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

if ($spell_reload)
{
  // Removes then adds all reloaded datablocks
  touchDataBlocks();
}
else
{
  // save script filename and data path for reloads
  RingOfFireSpell.scriptFile = $afxAutoloadScriptFile;
  RingOfFireSpell.spellDataPath = %mySpellDataPath;
  if (isFunction(addDemoSpellbookSpell))
  {
    addDemoSpellbookSpell(RingOfFireSpell, RingOfFireSpell_RPG);
    // for testing, uncomment below to add the variation used by Fire in the Sky
    //addDemoSpellbookSpell(RingOfFireSpell, RingOfFireSpell2_RPG);
  }
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//