
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// GREEN LEGS AND SCRAM SPELL
//
//    To flee, or not flee: that IS the question.  When choosing the former over
//    the latter, this spell gets you out of harm's way, FAST! 
//    from the makers of Spirit of Fox (In Socks)
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

//
// Here we verify AFX version requirements for the script.
//
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
$spell_reload = isObject(GreenLegsAndScramSpell);
if ($spell_reload)
{
  // mark datablocks so we can detect which are reloaded this script
  markDataBlocks();
  // reset data path from previously saved value
  %mySpellDataPath = GreenLegsAndScramSpell.spellDataPath;
}
else
{
  // set data path from default plus containing folder name
  %mySpellDataPath = $afxSpellDataPath @ "/" @ $afxAutoloadScriptFolder;
}


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock ParticleData(GLaS_Smoke_P)
{
   textureName = %mySpellDataPath @ "/GLaS/particles/glas_smoke";
   dragCoefficient = 0.0;
   gravityCoefficient = -0.01;
   inheritedVelFactor = 1.00;
   lifetimeMS = 800;
   lifetimeVarianceMS = 100;
   useInvAlpha = false;
   spinRandomMin = -30.0;
   spinRandomMax = 30.0;
   colors[0] = "0.0 0.4 0.0 0.5";
    colors[1] = "0.0 0.5 0.0 0.2";
    colors[2] = "0.0 0.6 0.0 0.0";
   sizes[0] = 0.2;
    sizes[1] = 0.25;
    sizes[2] = 0.3;
   times[0] = 0.0;
    times[1] = 0.5;
    times[2] = 1.0;
};

datablock ParticleEmitterData(GLaS_Smoke_E)
{
   ejectionPeriodMS = 10;
   periodVarianceMS = 0;
   ejectionVelocity = 0.25;
   velocityVariance = 0.10;
   thetaMin = 0.0;
   thetaMax = 90.0;  
   particles = GLaS_Smoke_P;
};

datablock afxEffectWrapperData(GLaS_Smoke_rt_foot_EW)
{
  effect = GLaS_Smoke_E;
  delay = 0.5;
  constraint = "target.Bip01 R Foot";
};

datablock afxEffectWrapperData(GLaS_Smoke_lf_foot_EW)
{
  effect = GLaS_Smoke_E;
  delay = 0.5;
  constraint = "target.Bip01 L Foot";
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

$MatchPlayerFootprints = false;

datablock afxZodiacData(GLaS_step_LF_Zode_CE : SHARED_ZodiacBase_CE)
{
  texture = %mySpellDataPath @ "/GLaS/zodiacs/scram_footprint_l";
  radius = 0.4;
  startAngle = 0.0;
  //color = "0.55 0.78 0.25 1.0";
  color = "0 1 0 0.4";
};

datablock afxZodiacData(GLaS_step_RT_Zode_CE : GLaS_step_LF_Zode_CE)
{
  texture = %mySpellDataPath @ "/GLaS/zodiacs/scram_footprint_r";
};

$GLaS_FootPrint_Dur = 0.8;

if ($MatchPlayerFootprints)
{
  //
  // Here the footprints are placed using offsets from the player's
  // origin rather than based on the actual positions of the feet.
  // This approach matches the placement of the built-in footprints
  // by the Player class.
  // 
  datablock afxXM_LocalOffsetData(GLaS_StepOffset_LF_XM)
  {
    localOffset = "-0.25 0.0 0";
  };
  datablock afxEffectWrapperData(GLaS_step_Zode_LF_EW)
  {
    effect = GLaS_step_LF_Zode_CE;
    constraint = "target";
    lifetime = 0.0;
    fadeOutTime = 0.5;
    residueLifetime = $GLaS_FootPrint_Dur;
    xfmModifiers[0] = GLaS_StepOffset_LF_XM;
  };

  //~~~~~~~~~~~~~~~~~~~~//

  datablock afxXM_LocalOffsetData(GLaS_StepOffset_RT_XM)
  {
    localOffset = "0.25 0.0 0";
  };
  datablock afxEffectWrapperData(GLaS_step_Zode_RT_EW : GLaS_step_Zode_LF_EW)
  {
    effect = GLaS_step_RT_Zode_CE;
    residueLifetime = $GLaS_FootPrint_Dur;
    xfmModifiers[0] = GLaS_StepOffset_RT_XM;
  };
}
else
{
  //
  // Here the footprints are placed using constraints to the player's
  // actual foot nodes. This seems like the most natural approach but
  // placement of footprints will not match the built-in footprints
  // left by the Player class.
  // 
  datablock afxXM_LocalOffsetData(GLaS_StepOffset_LF_XM)
  {
    localOffset = "-0.08 0.0 0.0";
  };
  datablock afxEffectWrapperData(GLaS_step_Zode_LF_EW)
  {
    effect = GLaS_step_LF_Zode_CE;
    posConstraint = "target.Bip01 L Foot";
    orientConstraint = "target";
    lifetime = 0.0;
    fadeOutTime = 0.5;
    residueLifetime = $GLaS_FootPrint_Dur;
    xfmModifiers[0] = GLaS_StepOffset_LF_XM;
  };

  //~~~~~~~~~~~~~~~~~~~~//

  datablock afxXM_LocalOffsetData(GLaS_StepOffset_RT_XM)
  {
    localOffset = "0.08 0.0 0.0";
  };
  datablock afxEffectWrapperData(GLaS_step_Zode_RT_EW : GLaS_step_Zode_LF_EW)
  {
    effect = GLaS_step_RT_Zode_CE;
    posConstraint = "caster.Bip01 R Foot";
    xfmModifiers[0] = GLaS_StepOffset_RT_XM;
  };
}

datablock afxFootSwitchData(GLaS_footfall_override_CE)
{
  overrideAll = true;
  //overrideDecals = true;
};
datablock afxEffectWrapperData(GLaS_footfall_override_EW)
{
  effect = GLaS_footfall_override_CE;
  posConstraint = caster;
}; 

datablock afxPlayerMovementData(GLaS_movement_override_CE)
{
  speedBias = 1;
  movement = "0 1.5 0";
  movementOp = "replace";
};

datablock afxEffectWrapperData(GLaS_movement_override_EW)
{
  effect = GLaS_movement_override_CE;
  posConstraint = caster;
}; 

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// AUDIO AND LIGHTING

switch$ (afxGetEngine())
{
  case "TGE":
    exec("./lighting/glas_lighting_tge_sub.cs");
    exec("./audio/glas_audio_sub.cs");
  case "TGEA":
    exec("./lighting/glas_lighting_tgea_sub.cs");
    exec("./audio/glas_audio_sub.cs");
 case "T3D":
    exec("./lighting/glas_lighting_t3d_sub.cs");
    exec("./audio/glas_audio_sub.cs");
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// PHRASE EFFECTS

datablock afxPhraseEffectData(GLaS_phrase_effect_LF_CE)
{
  addEffect = GLaS_step_Zode_LF_EW;
  triggerMask = $AFX::PLAYER_LF_FOOT_C_TRIGGER;
};
datablock afxEffectWrapperData(GLaS_phrase_effect_LF_EW)
{
  effect = GLaS_phrase_effect_LF_CE;
  posConstraint = target;
};

datablock afxPhraseEffectData(GLaS_phrase_effect_RT_CE)
{
  addEffect = GLaS_step_Zode_RT_EW;
  triggerMask = $AFX::PLAYER_RT_FOOT_C_TRIGGER;
};
datablock afxEffectWrapperData(GLaS_phrase_effect_RT_EW)
{
  effect = GLaS_phrase_effect_RT_CE;
  posConstraint = target;
};

// Note: There's a subtle order dependency between the creation of phrase-effects
//    GLaS_phrase_effect_LF_CE and GLaS_phrase_effect_RT_CE and the exec of the audio
//    subscript. The GLaS_add_footstep_Audio_FX() function called below adds effects 
//    defined in glas_audio_sub.cs to the phrase-effects, so it's important that the 
//    subscript exec happens before the phrase-effects are created. Otherwise, on the 
//    client-side, the phrase-effects will be ghosted before the effects they reference
//    which results in "bad datablockId" errors.
// 
GLaS_add_footstep_Audio_FX(GLaS_phrase_effect_LF_CE, GLaS_phrase_effect_RT_CE);

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// spell

datablock afxMagicSpellData(GreenLegsAndScramSpell)
{
  castingDur = 0.3;
  lingerDur = 5.0;
  allowMovementInterrupts = false;
  addLingerEffect = GLaS_phrase_effect_LF_EW;
  addLingerEffect = GLaS_phrase_effect_RT_EW;
  addLingerEffect = GLaS_Smoke_lf_foot_EW;
  addLingerEffect = GLaS_Smoke_rt_foot_EW;
  addLingerEffect = GLaS_footfall_override_EW;
  addLingerEffect = GLaS_movement_override_EW;
};

GLaS_add_Lighting_FX(GreenLegsAndScramSpell);
GLaS_add_Audio_FX(GreenLegsAndScramSpell);

datablock afxRPGMagicSpellData(GreenLegsAndScramSpell_RPG)
{
  spellName = "Green Legs and Scram";
  desc = "To flee, or not flee: that IS the question. " @ 
         "When choosing the former over the latter, this spell gets you out of harm's way, FAST!\n" @ 
         "<font:Arial:6>\n" @
         "<just:center><font:Arial:14>from the makers of <font:Arial Italic:14>Spirit of Fox (In Socks)<just:left>\n" @
         "\n" @
         "<font:Arial Italic:14>spell design and concept: <font:Arial:14>Jeff Faust\n" @
         "<font:Arial Italic:14>sound design: <font:Arial:14>Jeff Faust";
  sourcePack = "Spell Pack 2";
  iconBitmap = %mySpellDataPath @ "/GLaS/icons/glas";
  target = "self";
  canTargetSelf = true;
  manaCost = 10;
  castingDur = GreenLegsAndScramSpell.castingDur;
};

//UAISK+AFX Interop Changes: Start
function GreenLegsAndScramSpell::onActivate(%this, %spell, %caster, %target)
{
    if ($UAISK_Is_Available && %target.isBot)
    {
        //See how long until spell stops
        %time = $Sim::Time + GreenLegsAndScramSpell.lingerDur;
        randAimOffset(%target, %time);
    }
}
//UAISK+AFX Interop Changes: End

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

if ($spell_reload)
{
  // Removes then adds all reloaded datablocks
  touchDataBlocks();
}
else
{
  // save script filename and data path for reloads
  GreenLegsAndScramSpell.scriptFile = $afxAutoloadScriptFile;
  GreenLegsAndScramSpell.spellDataPath = %mySpellDataPath;
  if (isFunction(addDemoSpellbookSpell))
    addDemoSpellbookSpell(GreenLegsAndScramSpell, GreenLegsAndScramSpell_RPG);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

