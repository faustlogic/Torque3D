
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// WANDERING WISPS -- [reusable element]
//
// Releases wisp effects to wander along randomly selected paths. 
// May be configured to call a custom function as each wisp reaches the 
// end of its path.
//
// Uses a special wisp-like free-targeting selectron when available.
// 
//    instantiation function:
//      startWanderingWisps(%xfm, %n_wisps, %life_range, %onWispEndFunc)
//                  xfm -- transform used to anchor the effect. (Where the wisps originate.)
//             %n_wisps -- the number of wisps to generate (1-21)
//          %life_range -- single value or range-pair indicating time each wisp spends
//                         completing its path. Shorter lifetimes lead to faster wisps
//                         and vice-versa.
//       %onWispEndFunc -- a callback function to be called when each wisp reaches the
//                         end of it's path. 
//                             Func(%datablock, %spell, %constraint, %xfm, %opaque_data)
//
//    internal parameters:
//      onWispEndFunc
//      _wispLife[##]
//      _wispI[##]
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
$spell_reload = isObject(WanderingWispsSpell);
if ($spell_reload)
{
  // mark datablocks so we can detect which are reloaded this script
  markDataBlocks();
  // reset data path from previously saved value
  %mySpellDataPath = WanderingWispsSpell.spellDataPath;
}
else
{
  // set data path from default plus containing folder name
  %mySpellDataPath = $afxSpellDataPath @ "/" @ $afxAutoloadScriptFolder;
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

$WW_WispPath_Scale = 3.0;

//
// These are end positions which have been carefully copied from the paths in
// the datafile ww_wisp_paths.txt. It's important that the values and the
// array ordering match with the paths in ww_wisp_paths.txt. These are used on
// the server to pass the final path positions into the onWispEnd() function.
// Since the actual paths are used client-side while these end-positons are used
// on the server, it was easiest to copy these values rather than load the paths.
//
$WW_WispPath_End[0] = "10.073 18.956 0.0";
$WW_WispPath_End[1] = "-6.997 -14.721 0.0";
$WW_WispPath_End[2] = "-8.310 13.704 0.0";
$WW_WispPath_End[3] = "9.398 -5.104 0.0";
$WW_WispPath_End[4] = "-8.629 5.400 0.0";
$WW_WispPath_End[5] = "10.199 5.163 0.0";
$WW_WispPath_End[6] = "8.757 -1.434 0.0";
$WW_WispPath_End[7] = "-2.829 18.310 0.0";
$WW_WispPath_End[8] = "13.807 -9.752 0.0";
$WW_WispPath_End[9] = "-7.601 8.161 0.0";
$WW_WispPath_End[10] = "2.097 11.033 0.0";
$WW_WispPath_End[11] = "2.619 -10.043 0.0";
$WW_WispPath_End[12] = "11.870 9.690 0.0";
$WW_WispPath_End[13] = "-15.472 -1.426 0.0";
$WW_WispPath_End[14] = "-12.596 -5.423 0.0";
$WW_WispPath_End[15] = "-5.011 4.735 0.0";
$WW_WispPath_End[16] = "-8.657 1.935 0.0";
$WW_WispPath_End[17] = "16.997 -3.144 0.0";
$WW_WispPath_End[18] = "18.169 6.525 0.0";
$WW_WispPath_End[19] = "-3.253 7.546 0.0";
$WW_WispPath_End[20] = "-8.571 -6.872 0.0";

// apply scale to each path end-point. 
for (%i = 0; %i < 21; %i++)
  $WW_WispPath_End[%i] = VectorScale($WW_WispPath_End[%i], $WW_WispPath_Scale);

// used in substitution statement to retrieve end-point values
function WW_getWispEndPoint(%idx)
{
  return $WW_WispPath_End[%idx];
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

// selects a path using a group-index (##)
datablock afxPathData(WW_WispPath_00_Path)
{
  lifetime = "$$ %%._wispLife[##]";
  points = "$$ WW_getWispPath(%%._wispI[##])";
};
datablock afxXM_PathConformData(WW_Wander_path_00_XM)
{
  paths = "WW_WispPath_00_Path";
  pathMult = $WW_WispPath_Scale;
};

// used to position the wisps just above the ground
datablock afxXM_GroundConformData(WW_LineMagic_ground_XM)
{
  height = 0.1;
};

// wisp head mooring (client) used for positioning the wisp flames
datablock afxXM_LocalOffsetData(WW_PathEnd_offset_00_XM)
{
  localOffset = "$$ WW_getWispEndPoint(%%._wispI[##])";
};
datablock afxMooringData(WW_WispMooring_CE)
{
  networking = $AFX::CLIENT_ONLY;
  displayAxisMarker = false;
};
datablock afxEffectWrapperData(WW_WispMooring_00_EW)
{
  effect = WW_WispMooring_CE;
  constraint = "anchor";
  effectName = "$$ GroundMagic_MooringA_##";
  isConstraintSrc = true;
  delay = "$$ ##*0.7";
  lifetime = "$$ %%._wispLife[##]";
  xfmModifiers[0] = WW_Wander_path_00_XM;
  xfmModifiers[1] = WW_LineMagic_ground_XM;
};

// wisp trail line-magic, these particles create the long lines that
// the wisps leave behind
datablock ParticleData(WW_LineMagic_P)
{
  textureName          = %mySpellDataPath @ "/WW/particles/ww_line_magic";
  dragCoeffiecient     = 0.0;
  windCoefficient      = 0.0;
  gravityCoefficient   = 0.0;
  inheritedVelFactor   = 0.0;
  lifetimeMS           = 8000;
  lifetimeVarianceMS   = 1600;
  blendType            = premultalpha;
  spinRandomMin        = 0;
  spinRandomMax        = 0;
  colors[0]            = "1.0 1.0 1.0 0.0";
    colors[1]            = 8/255 SPC 245/255 SPC 189/255 SPC 0.0;
    colors[2]            = 8/255 SPC 245/255 SPC 189/255 SPC 0.0;
    colors[3]            = 255/255 SPC 186/255 SPC 71/255 SPC 0.0;
    colors[4]            = 211/255 SPC 45/255  SPC  2/255 SPC 0.0;
    colors[5]            = "0.0 0.0 0.0 0.5";
    colors[6]            = "0.0 0.0 0.0 0.5";
    colors[7]            = "0.0 0.0 0.0 0.0";
  sizeBias             = 0.05;
  sizes[0]             = 5.0;
    sizes[1]             = 1.5; 
    sizes[2]             = 1.0; 
    sizes[3]             = 1.0; 
    sizes[4]             = 1.0; 
    sizes[5]             = 2.0;
    sizes[6]             = 2.0;
    sizes[7]             = 2.0;
  times[0]             = 0.0;
    times[1]             = 0.05;
    times[2]             = 0.08;
    times[3]             = 0.15;
    times[4]             = 0.3;
    times[5]             = 0.35;
    times[6]             = 0.9;
    times[7]             = 1.0;
};
datablock ParticleEmitterData(WW_LineMagic_E)
{
  ejectionPeriodMS = 30;
  periodVarianceMS = 0;
  ejectionVelocity = 0.0;
  velocityVariance = 0.0;
  particles = WW_LineMagic_P;
  fadeAlpha = true;
  fadeColor = true;
  blendStyle = "PREMULTALPHA";
  softnessDistance = 0.01; // disable soft-particles
};
datablock afxEffectWrapperData(WW_LineMagic_00_EW)
{
  effect = WW_LineMagic_E;
  constraint = "$$ \"#effect.GroundMagic_MooringA_##\"";
  delay = "$$ ##*0.7";
  fadeInTime = 1.0;
  fadeOutTime = 0.0;
  lifetime = "$$ %%._wispLife[##]";
  forcedBBox = "-50.0 -50.0 -20.0 50.0 50.0 20.0";
  sortPriority = 120;
};

// wisp magic-fire particles, these particles create the flames
// of the wisps
datablock ParticleData(WW_MagicFire_P)
{
  textureName          = %mySpellDataPath @ "/WW/particles/ww_wisp_fire";
  dragCoeffiecient     = 0.5;
  inheritedVelFactor   = 0.00;
  lifetimeMS           = 500;
  lifetimeVarianceMS   = 300;
  spinRandomMin        = -300;
  spinRandomMax        = 300;
  colors[0]            = "0.5 0.5 0.5 1.0";
    colors[1]            = "1.0 1.0 1.0 1.0";
    colors[2]            = "0.0 0.0 0.0 0.0";
  sizeBias             = 0.6;
  sizes[0]             = 1.0;
    sizes[1]             = 0.5; 
    sizes[2]             = 0.1;
  times[0]             = 0.0;
    times[1]             = 0.5;
    times[2]             = 1.0;
  constrainPos         = true;
};
datablock afxParticleEmitterDiscData(WW_MagicFire_E)
{
  ejectionPeriodMS      = 8;
  periodVarianceMS      = 3;
  ejectionVelocity      = 1.3;
  velocityVariance      = 0.5;
  particles             = "WW_MagicFire_P";
  vector                = "0 0 1";
  vectorIsWorld         = true;
  radiusMin             = 0.0;
  radiusMax             = 0.15;
  blendStyle            = "ADDITIVE";
};
datablock afxEffectWrapperData(WW_MagicFire_00_EW : WW_LineMagic_00_EW)
{
  effect = WW_MagicFire_E;
  lifetime = "$$ %%._wispLife[##] + 1";
  sortPriority = 0;
};

// wisp magic-fire bounce zodiac
datablock afxZodiacData(WW_MagicFireBounce_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/WW/zodiacs/ww_magic_fire_bounce";
  radius = 1.0;
  startAngle = 0.0;
  rotationRate = 0.0;
  color = "1 1 1 1";
  blend = additive;
  interiorHorizontalOnly = true;
};
datablock afxEffectWrapperData(WW_MagicFireBounce_00_EW : WW_MagicFire_00_EW)
{
  effect = WW_MagicFireBounce_CE;
};

// wisp end script
datablock afxXM_LocalOffsetData(WW_PathEnd_offset_00_XM)
{
  localOffset = "$$ WW_getWispEndPoint(%%._wispI[##])";
};
datablock afxScriptEventData(WW_EndScript_CE)
{
  methodName = "onWispEnd";
};
datablock afxEffectWrapperData(WW_EndScript_00_EW)
{
  effect = WW_EndScript_CE;
  constraint = "anchor";
  delay = "$$ ##*0.7 + %%._wispLife[##]";
  xfmModifiers[0] = WW_PathEnd_offset_00_XM;
  xfmModifiers[1] = WW_LineMagic_ground_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// AUDIO 

switch$ (afxGetEngine())
{
  case "TGE":
    exec("./audio/ww_audio_sub.cs");
  case "TGEA":
    exec("./audio/ww_audio_sub.cs");
 case "T3D":
    exec("./audio/ww_audio_sub.cs");
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// GROUPS

// wandering wisps group, this group combines all of the effects that
// make up a single wisp. The path it follows is slected by the
// group-index.
datablock afxEffectGroupData(WW_WanderingWisps_EG)
{
  assignIndices = true;
  count = "$$ %%._n_wisps";
  addEffect = WW_WispMooring_00_EW;
  addEffect = WW_LineMagic_00_EW;
  addEffect = WW_MagicFire_00_EW;
  addEffect = WW_MagicFireBounce_00_EW;
  addEffect = WW_EndScript_00_EW;
};

WW_add_group_Audio_FX(WW_WanderingWisps_EG);

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// WANDERING WISPS EFFECTRON
//

datablock afxEffectronData(WanderingWispsEffectron)
{
  echoPacketUsage = 20;
  execOnNewClients = true;
  clientScriptFile = %mySpellDataPath @ "/WW/WW_client.cs";
  clientInitFunction = "WW_clientInit";

  addEffect = WW_WanderingWisps_EG;
};

//
// This script-event method is called as each wisp reaches the end of its path. 
// If the effectron's dynamic onWispEndFunc field is specified, the function
// by that name will be called.
//
function WanderingWispsEffectron::onWispEnd(%this, %chor, %cons, %xfm, %data)
{  
  if (%chor.onWispEndFunc !$= "")
    call(%chor.onWispEndFunc, %this, %chor, %cons, %xfm, %data);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// INSTANTIATION FUNCTION

function startWanderingWisps(%xfm, %n_wisps, %life_range, %onWispEndFunc)
{
  if (%n_wisps > 21)
  {
    warn("startWanderingWisps() -- %n_wisps of" SPC %n_wisps SPC "is too large... clamped to 21.");
    %n_wisps = 21;
  }

  %wisp_effectron = startEffectron(WanderingWispsEffectron, %xfm, "anchor");
  %wisp_effectron._n_wisps = %n_wisps;

  // initialize lifetimes
  %wc = getWordCount(%life_range);
  if (%wc > 1)
  {
    %life_min = getWord(%life_range, 0); 
    %life_max = getWord(%life_range, 1);
  }
  else if (%wc == 1)
  {
    %life_min = %life_range; 
    %life_max = %life_range;
  }
  else
  {
    %life_min = 11; 
    %life_max = 25;
  }
  for (%i = 0; %i < %n_wisps; %i++)
    %wisp_effectron._wispLife[%i] = getRandom(%life_min, %life_max);

  // pick path ordering
  %idx_string = "0";
  for (%i = 1; %i < %n_wisps; %i++)
    %idx_string = %idx_string SPC %i;
  %end_idx = %n_wisps-1;
  for (%i = 0; %i < %n_wisps-1; %i++)
  {
    %pick_word_idx = getRandom(%end_idx);
    %wisp_effectron._wispI[%i] = getWord(%idx_string, %pick_word_idx);
    %idx_string = removeWord(%idx_string, %pick_word_idx);
    %end_idx--;
  }
  %wisp_effectron._wispI[%i] = %idx_string;
  
  %wisp_effectron.onWispEndFunc = %onWispEndFunc;
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// WANDERING WISPS SPELL
//    a simple spell that allows placing of the wisp starting
//    point using a free-targeting selectron.
//

datablock afxMagicSpellData(WanderingWispsSpell)
{
  castingDur = 0;
};

datablock afxRPGMagicSpellData(WanderingWispsSpell_RPG)
{
  spellName = "Wandering Wisps";
  desc = "Releases wisp effects to wander along randomly selected paths. " @
         "May be configured to call a custom function as each wisp reaches the " @
         "end of its path." @
         "\n\n" @
         "Uses a special wisp-like free-targeting selectron when available." @
         "\n\n" @
         "[reusable element]";
  sourcePack = "Spell Pack 2";
  iconBitmap = %mySpellDataPath @ "/WW/icons/ww";
  manaCost = 10;
  castingDur = 0;
  target = "free";
  freeTargetStyle = 1;
};

function WanderingWispsSpell::onLaunch(%this, %spell, %caster, %target, %missile)
{
  Parent::onLaunch(%this, %spell, %caster, %target, %missile);

  // fix anchor at free-target location with caster's transform
  %anchor = moveTransformAbs(%caster.getTransform(), %spell.freeTarget);

  startWanderingWisps(%anchor, 2, 25);
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
  WanderingWispsSpell.scriptFile = $afxAutoloadScriptFile;
  WanderingWispsSpell.spellDataPath = %mySpellDataPath;
  if (isFunction(addDemoSpellbookSpell))
  {
    addDemoSpellbookSpell(WanderingWispsSpell, WanderingWispsSpell_RPG);
  }
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
