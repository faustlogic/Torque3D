//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// THE LETTER K -- ELEMENT
//
//    Brands the ground with a fiery letter K.
//
//    parameters:
//      _shake 
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
$spell_reload = isObject(TheLetterKSpell);
if ($spell_reload)
{
  // mark datablocks so we can detect which are reloaded this script
  markDataBlocks();
  // reset data path from previously saved value
  %mySpellDataPath = TheLetterKSpell.spellDataPath;
}
else
{
  // set data path from default plus containing folder name
  %mySpellDataPath = $afxSpellDataPath @ "/" @ $afxAutoloadScriptFolder;

  afxExecPrerequisite("sp2_shared_fx.cs", $afxAutoloadScriptFolder);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

// this offset centers the K symbol
datablock afxXM_LocalOffsetData(LK_K_offset_XM)
{
  localOffset = "0.35 0 0";
};

// these wave xmods shakes the K symbol when it appears
datablock afxXM_WaveScalarData(LK_K_wave1_XM)
{
  a = -0.02*2;
  b = 0.04*2;
  parameter = "pos";
  op = "add";
  axis = "1 0 0";
  axisIsLocal = true;
  waveform = "sine";
  speed = 70/6.3;
  speedVariance = 0.0;
  offDutyT = 0.5;
  fadeInTime = 0.3;
  lifetime = 1.5;
  fadeOutTime = 1.0;
};
datablock afxXM_WaveScalarData(LK_K_wave2_XM : LK_K_wave1_XM)
{
  axis = "0 1 0";
  speed = 60/6.3;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock afxZodiacData(LK_Glowing_K_Zode_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/LK/zodiacs/lk_glowing_K";
  radius = 1.7;
  startAngle = 0;
  rotationRate = 0.0;
  color = "1 1 1 1.0";
  blend = additive;
  trackOrientConstraint = true;
};
datablock afxEffectWrapperData(LK_Glowing_K_EW)
{
  effect = LK_Glowing_K_Zode_CE;
  constraint = "anchor";
  delay = 0;
  fadeInTime = 1.0;
  fadeOutTime = 0.5;
  lifetime = 2.0;
  xfmModifiers[0] = LK_K_offset_XM;
  xfmModifiers[1] = LK_K_wave1_XM;
  xfmModifiers[2] = LK_K_wave2_XM;
  // these subs keep-or-clear the wave xmods according to _shake
  xfmModifiers[1] = "$$ (%%._shake == true) ? \"~~\" : \"~0\"";
  xfmModifiers[2] = "$$ (%%._shake == true) ? \"~~\" : \"~0\"";
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock afxZodiacData(LK_Hot_K_Zode_CE : LK_Glowing_K_Zode_CE)
{  
  texture = %mySpellDataPath @ "/LK/zodiacs/lk_hot_K";
  color = "1 1 1 1";
  blend = additive;
};
datablock afxEffectWrapperData(LK_Hot_K_EW)
{
  effect = LK_Hot_K_Zode_CE;
  constraint = "anchor";
  delay = 2.0;
  fadeInTime = 0.5;
  fadeOutTime = 5.0;
  lifetime = 2.0;
  xfmModifiers[0] = LK_K_offset_XM;
};

%LK_K_Pulse_dur = 2.1;
datablock afxEffectWrapperData(LK_Hot_K_Pulse_1_EW)
{
  effect = LK_Hot_K_Zode_CE;
  constraint = "anchor";
  delay = 2.5;
  fadeInTime = %LK_K_Pulse_dur*0.5;
  fadeOutTime = %LK_K_Pulse_dur*0.5;
  lifetime = %LK_K_Pulse_dur*0.5;
  xfmModifiers[0] = LK_K_offset_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock afxZodiacData(LK_Hot_60_K_Zode_CE : LK_Hot_K_Zode_CE)
{  
  color = "1 1 1 0.6";
};
datablock afxEffectWrapperData(LK_Hot_K_Pulse_2_EW : LK_Hot_K_Pulse_1_EW)
{
  effect = LK_Hot_60_K_Zode_CE;
  delay = 4.5;
};

datablock afxZodiacData(LK_Hot_30_K_Zode_CE : LK_Hot_K_Zode_CE)
{  
  color = "1 1 1 0.3";
};
datablock afxEffectWrapperData(LK_Hot_K_Pulse_3_EW : LK_Hot_K_Pulse_1_EW)
{
  effect = LK_Hot_30_K_Zode_CE;
  delay = 6.0;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock afxZodiacData(LK_Simmering_K_123_CE : LK_Glowing_K_Zode_CE)
{
  texture = "$$ \"" @ %mySpellDataPath @ "/LK/zodiacs/lk_embers_K_\" @ mFloor(1 + ##/6)";
  color = "1 1 1 1.0";
  blend = additive;
};

datablock afxEffectWrapperData(LK_Simmering_K_00_EW)
{
  effect = LK_Simmering_K_123_CE;
  constraint = "anchor";
  delay       = "$$ 5 + 2*(## % 6) + (##/6)*0.6 + getRandomF(0.0,0.6)";
  lifetime    = "$$ getRandomF(0.4, 0.8)*2";
  fadeInTime  = 0.4*2;
  fadeOutTime = "$$ getRandomF(0.5, 1.0)*2";
  xfmModifiers[0] = LK_K_offset_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock afxZodiacData(LK_Burnt_K_Zode_CE : LK_Glowing_K_Zode_CE)
{  
  texture = %mySpellDataPath @ "/LK/zodiacs/lk_burnt_K";
  color = "1 1 1 1.0";
  blend = normal;
};
//
datablock afxEffectWrapperData(LK_Burnt_K_EW)
{
  effect = LK_Burnt_K_Zode_CE;
  constraint = "anchor";
  delay       = 9.0;
  lifetime    = 1.5;
  fadeInTime  = 5.0;
  fadeOutTime = 3.0;
  residueLifetime = 15.0;
  xfmModifiers[0] = LK_K_offset_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// "K" FORMING PATHS
//

%LK_K_outline_scale = 0.11;

datablock afxPathData(LK_K_outline_1_Path)
{
  points = "-5.524609621 -10.29485488 0" SPC
           "-8.852687705 -0.08874874893 0" SPC
           "-12.22514016 12.6910711 0";
  mult = %LK_K_outline_scale;  
};

datablock afxPathData(LK_K_outline_2_Path)
{
  points = "-7.521456471 11.13796799 0" SPC
           "-6.01272774 5.591171182 0" SPC
           "-4.770245255 0.7099899914 0" SPC
           "-1.087172174 6.389909923 0" SPC
           "2.906521527 13.44543546 0";
  mult = %LK_K_outline_scale;
};

datablock afxPathData(LK_K_outline_3_Path)
{
  points = "4.060255263 9.629239258 0" SPC
           "-1.397792796 1.553103106 0" SPC
           "-2.152157161 -0.1331231234 0" SPC
           "4.548373382 -1.242482485 0" SPC
           "10.18391894 -5.635545557 0";
  mult = %LK_K_outline_scale;
};

datablock afxPathData(LK_K_outline_4_Path)
{
  points = "-0.865300302 -12.06982985 0" SPC
           "-2.817772778 -7.011151165 0" SPC
           "-3.439014021 -3.328078085 0" SPC
           "0.9096746765 -5.236176187 0" SPC
           "7.388333348 -9.362993012 0" SPC
           "8.763938956 -13.66730733 0";
  mult = %LK_K_outline_scale;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock afxParticleEmitterPathData(LK_SymbolOutline_E)
{
  ejectionPeriodMS  = 3;
  periodVarianceMS  = 1;
  ejectionVelocity  = 0.5;
  velocityVariance  = 0.25;
  particles         = "SP2_GroundFire_LG_P SP2_GroundFire_MED_P SP2_GroundFire_SM_P";
  pathOrigin        = "vector";
  paths             = "LK_K_outline_1_Path LK_K_outline_2_Path" SPC
                      "LK_K_outline_3_Path LK_K_outline_4_Path";
  fadeSize          = true;
  fadeAlpha         = true;
  groundConform     = true;
  vector            = "0 0 1";
  softnessDistance  = 0.01; // disable soft-particles
};

datablock afxEffectWrapperData(LK_SymbolOutline_EW)
{
  effect = LK_SymbolOutline_E;
  constraint = "anchor";
  delay = 2;
  lifetime = 14.0 - 2;
  fadeOutTime = 3.0;
  fadeInTime = 0.3;
  xfmModifiers[0] = LK_K_offset_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock afxPathData(LK_SymbolOutlineTendril_1A_Path : LK_K_outline_1_Path)
{  
  lifetime = "$$ getRandomF(1.0, 3.0)";
  loop = oscillate;
  timeOffset = "$$ getRandomF(0.0, 2.0)";
};
datablock afxPathData(LK_SymbolOutlineTendril_2A_Path : LK_K_outline_2_Path)
{
  lifetime = "$$ getRandomF(1.0, 3.0)";
  loop = oscillate;
  timeOffset = "$$ getRandomF(0.0, 2.0)";
};
datablock afxPathData(LK_SymbolOutlineTendril_3A_Path : LK_K_outline_3_Path)
{
  lifetime = "$$ getRandomF(1.0, 3.0)";
  loop = oscillate;
  timeOffset = "$$ getRandomF(0.0, 2.0)";
};
datablock afxPathData(LK_SymbolOutlineTendril_4A_Path : LK_K_outline_4_Path)
{
  lifetime = "$$ getRandomF(1.0, 3.0)";
  loop = oscillate;
  timeOffset = "$$ getRandomF(0.0, 2.0)";
};

datablock afxXM_PathConformData(LK_SymbolOutline_TendrilSmoke_path_1A_XM)
{
  paths = "LK_SymbolOutlineTendril_1A_Path";
};
datablock afxXM_PathConformData(LK_SymbolOutline_TendrilSmoke_path_2A_XM)
{
  paths = "LK_SymbolOutlineTendril_2A_Path";
};
datablock afxXM_PathConformData(LK_SymbolOutline_TendrilSmoke_path_3A_XM)
{
  paths = "LK_SymbolOutlineTendril_3A_Path";
};
datablock afxXM_PathConformData(LK_SymbolOutline_TendrilSmoke_path_4A_XM)
{
  paths = "LK_SymbolOutlineTendril_4A_Path";
};

// Tendril Smoke Particles
datablock ParticleData(LK_SymbolOutline_TendrilSmokeRND_P : SP2_TendrilSmokeA_P)
{
  lifetimeMS           = "$$ 2000*getRandomF(0.6,2.0)";
  lifetimeVarianceMS   = "$$ 200*getRandomF(0.6,2.0)";
  colors[0]            = "0.0 0.0 0.0 0.0";
    colors[1]            = "0.5 0.5 0.5 0.02"; // very subtle
    colors[2]            = "1.0 1.0 1.0 0.02";
    colors[3]             = "0.0 0.0 0.0 0.0";
  sizes[0]             = 0.2;
    sizes[1]             = 0.7*1.5;
    sizes[2]             = 1.3*2;
    sizes[3]             = 2.0*2;
  times[0]             = 0.0;
    times[1]             = 0.4;
    times[2]             = 0.7;
    times[3]             = 1.0;
};
datablock afxParticleEmitterVectorData(LK_SymbolOutline_TendrilSmokeRND_E : SP2_TendrilSmokeA_E)
{
  particles = LK_SymbolOutline_TendrilSmokeRND_P;
  fadeColor = true;
  fadeAlpha = true;
};

datablock afxXM_GroundConformData(LK_SymbolOutline_TendrilSmoke_ground_XM)
{
  height = 0.2;
};

datablock afxXM_WaveScalarData(LK_TendrilSmoke_wave_XM)
{
  waveform = "sine";
  parameter = "ori";
  speed = "$$ getRandomF(0.1, 2)";
  a = "$$ getRandomF(-80.0,0)";
  b = "$$ getRandomF(0,80.0)";
  op = "mult";
  axis  = "$$ getRandomDir(\"0 0 1\", 90, 90)";
  offDutyT = 0.5;
};

datablock afxEffectWrapperData(LK_SymbolOutline_TendrilSmoke_A_EW)
{
  effect = LK_SymbolOutline_TendrilSmokeRND_E;
  constraint = "anchor";  
  delay = 3;
  fadeInTime = 2.0;
  fadeOutTime = 4.0;
  lifetime = 16 - 2;
  xfmModifiers[0] = LK_K_offset_XM;
  xfmModifiers[1] = LK_SymbolOutline_TendrilSmoke_path_1A_XM;  
  xfmModifiers[2] = LK_SymbolOutline_TendrilSmoke_ground_XM;
  xfmModifiers[3] = LK_TendrilSmoke_wave_XM;
  xfmModifiers[4] = LK_TendrilSmoke_wave_XM;
};
datablock afxEffectWrapperData(LK_SymbolOutline_TendrilSmoke_B_EW : LK_SymbolOutline_TendrilSmoke_A_EW)
{
  xfmModifiers[1] = LK_SymbolOutline_TendrilSmoke_path_2A_XM;
};
datablock afxEffectWrapperData(LK_SymbolOutline_TendrilSmoke_C_EW : LK_SymbolOutline_TendrilSmoke_A_EW)
{
  xfmModifiers[1] = LK_SymbolOutline_TendrilSmoke_path_3A_XM;
};
datablock afxEffectWrapperData(LK_SymbolOutline_TendrilSmoke_D_EW : LK_SymbolOutline_TendrilSmoke_A_EW)
{
  xfmModifiers[1] = LK_SymbolOutline_TendrilSmoke_path_4A_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// AUDIO AND LIGHTING

switch$ (afxGetEngine())
{
  case "TGE":
    exec("./lighting/lk_lighting_tge_sub.cs");
    //exec("./audio/lk_audio_sub.cs");
  case "TGEA":
    exec("./lighting/lk_lighting_tgea_sub.cs");
    //exec("./audio/lk_audio_sub.cs");
 case "T3D":
    exec("./lighting/lk_lighting_t3d_sub.cs");
    //exec("./audio/lk_audio_sub.cs");
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// GROUPS

datablock afxEffectGroupData(LK_Simmering_K_EG)
{
  assignIndices = true;
  count = 18;
  addEffect = LK_Simmering_K_00_EW;
};

datablock afxEffectGroupData(LK_TendrilSmoke_K_EG)
{
  addEffect = LK_SymbolOutline_TendrilSmoke_A_EW;
  addEffect = LK_SymbolOutline_TendrilSmoke_A_EW;
  addEffect = LK_SymbolOutline_TendrilSmoke_B_EW;
  addEffect = LK_SymbolOutline_TendrilSmoke_B_EW;
  addEffect = LK_SymbolOutline_TendrilSmoke_C_EW;
  addEffect = LK_SymbolOutline_TendrilSmoke_C_EW;
  addEffect = LK_SymbolOutline_TendrilSmoke_D_EW;
  addEffect = LK_SymbolOutline_TendrilSmoke_D_EW;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// THE LETTER K EFFECTRON
//   This is the main part of the Letter K element
//   which is implemented using an effectron.
//
datablock afxEffectronData(TheLetterKEffectron)
{
  echoPacketUsage = 20;
  execOnNewClients = true;

  duration = 1.0;
  numLoops = 1;
  
    // symbol glow zodes -- main spell //
  addEffect = LK_Glowing_K_EW;
  addEffect = LK_Hot_K_EW;

  addEffect = LK_Hot_K_Pulse_1_EW;
    addEffect = LK_Hot_K_Pulse_2_EW;
    addEffect = LK_Hot_K_Pulse_3_EW;
  
  addEffect = LK_Burnt_K_EW;

  addEffect = LK_Simmering_K_EG;

    // burning "K" shape //
  addEffect = LK_SymbolOutline_EW;
  addEffect = LK_TendrilSmoke_K_EG;

    // param defaults //
  _shake = 0;
};

LK_add_Lighting_FX(TheLetterKEffectron);
//LK_add_Audio_FX(TheLetterKEffectron);

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// THE LETTER K SPELL
//   This is a simple spell front-end to the main effectron,
//   TheLetterKEffectron. It's purpose is to place a 
//   free-targeting demo of The Letter K in the spellbank
//   interface.
//
datablock afxMagicSpellData(TheLetterKSpell)
{
  castingDur = 0;
};

datablock afxRPGMagicSpellData(TheLetterKSpell_RPG)
{
  spellName = "The Letter K";
  desc = "Brands the ground with a fiery letter K." @
         "\n\n" @
         "[reusable element]";
  sourcePack = "Spell Pack 2";
  iconBitmap = %mySpellDataPath @ "/MoK/icons/mok";
  manaCost = 10;
  castingDur = 0;
  target = "free";
};

function TheLetterKSpell::onLaunch(%this, %spell, %caster, %target, %missile)
{
  Parent::onLaunch(%this, %spell, %caster, %target, %missile);

  %anchor = %spell.freeTarget SPC getWords(%caster.getTransform(), 3);

  %effectron = startEffectron(TheLetterKEffectron, %anchor, "anchor");
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
  TheLetterKSpell.scriptFile = $afxAutoloadScriptFile;
  TheLetterKSpell.spellDataPath = %mySpellDataPath;
  if (isFunction(addDemoSpellbookSpell))
  {
    addDemoSpellbookSpell(TheLetterKSpell, TheLetterKSpell_RPG);
  }
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
