
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// CANTRIP AND FALL SPELL
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
$MIN_REQUIRED_VERSION = 1.12;

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
$spell_reload = isObject(CantripAndFallSpell);
if ($spell_reload)
{
  // mark datablocks so we can detect which are reloaded this script
  markDataBlocks();
  // reset data path from previously saved value
  %mySpellDataPath = CantripAndFallSpell.spellDataPath;
}
else
{
  // set data path from default plus containing folder name
  %mySpellDataPath = $afxSpellDataPath @ "/" @ $afxAutoloadScriptFolder;
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// SPELLCASTER ANIMATION

//
// To initiate this spell, the caster kicks, as if tripping his
// target from afar.
//

datablock afxAnimClipData(CTaF_Casting_Clip_CE)
{
  clipName = "ctaf";
  ignoreCorpse = true;
  rate = 1.0;
};
//
datablock afxEffectWrapperData(CTaF_Casting_Clip_EW)
{
  effect = CTaF_Casting_Clip_CE;
  constraint = "caster";
  lifetime = 35/30;
  delay = 0.0;
};


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// CASTING DUST

//
// Concurrent with the spellcaster's kick animation, dust is kicked-
// up from the caster's right foot.  This is done using an emitter
// constrained to the foot.
//

// dark dust
datablock ParticleData(CTaF_Dust1_P)
{
  textureName          = %mySpellDataPath @ "/CTaF/particles/smoke";
  dragCoeffiecient     = 0.5;
  gravityCoefficient   = 0.2;
  inheritedVelFactor   = 0.00;
  lifetimeMS           = 700;
  lifetimeVarianceMS   = 300;
  useInvAlpha          = true;
  spinRandomMin        = -60.0;
  spinRandomMax        = 60.0;
  colors[0]            = "0.4 0.33 0.2 0.0";  
  colors[1]            = "0.4 0.33 0.2 0.15";
  colors[2]            = "0.4 0.33 0.2 0.05";
  colors[3]            = "0.4 0.33 0.2 0.0";
  sizes[0]             = 0.7; 
  sizes[1]             = 1.6;
  sizes[2]             = 1.0;
  sizes[3]             = 0.3;
  times[0]             = 0.0;
  times[1]             = 0.2;
  times[2]             = 0.7;
  times[3]             = 1.0;   
};
// light dust
datablock ParticleData(CTaF_Dust2_P : CTaF_Dust1_P)
{
  colors[0]            = "0.66 0.55 0.33 0.0";   
  colors[1]            = "0.66 0.55 0.33 0.15";
  colors[2]            = "0.66 0.55 0.33 0.05";
  colors[3]            = "0.66 0.55 0.33 0.0";
};

// foot dust emitter (standard Torque "sprinkler" emitter)
datablock ParticleEmitterData(CTaF_footDust_E)
{
  ejectionPeriodMS      = 30;
  periodVarianceMS      = 7;
  ejectionVelocity      = 0.3;//1.0;
  velocityVariance      = 0.1;//0.3;  
  particles             = "CTaF_Dust1_P CTaF_Dust2_P";

  // TGE emitterType = "sprinkler";
};
// 
datablock afxEffectWrapperData(CTaF_FootDust_EW)
{
  effect = CTaF_footDust_E;
  constraint = "caster.Bip01 R Foot";
  lifetime = 0.3;
  delay = 0.75;
};


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// BANANA PEEL

//
// The target trips on a magic banana peel that is shoots through
// the air.  The orc's fall animation is in a backwards direction,
// so the peel shoots forward.  No attempt is made to line up the
// peel with a specific foot, as this would probably be impossible.
//

// banana peel offset, just a bit in front of the orc
datablock afxXM_LocalOffsetData(CTaF_Banana_offset_XM)
{
  localOffset = "0 1 0";
};

// banana peel
datablock afxModelData(CTaF_Banana_CE)
{
  shapeFile = %mySpellDataPath @ "/CTaF/models/CTaF_banana.dts";
  sequence = "slip";
};
//
datablock afxEffectWrapperData(CTaF_Banana_EW)
{
  effect = CTaF_Banana_CE;
  constraint = "impactedObject";
  delay = 0.2;
  fadeInTime  = 0.1;
  fadeOutTime = 0.1;
  lifetime = (40/30)-0.1;
  xfmModifiers[0] = CTaF_Banana_offset_XM;
};


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// TARGET ANIMATION

//
//
//

datablock afxAnimClipData(CTaF_TripFall_CE)
{
  //clipName = "ctaf_dn";
  clipName = "death11";
  ignoreCorpse = true;
  rate = 1.0;
};
//
datablock afxEffectWrapperData(CTaF_TripFall_EW)
{
  effect = CTaF_TripFall_CE;
  constraint = "impactedObject";
  lifetime = 2.3;
  lifeConstraint = "impactedObject";
  lifeConditions = $AFX::ALIVE;
};

datablock afxAnimClipData(CTaF_GetUp_CE)
{
  //clipName = "ctaf_up";
  clipName = "death2";
  ignoreCorpse = true;
  rate = -1;
  transitionTime = 0.5;
};
//
datablock afxEffectWrapperData(CTaF_GetUp_EW)
{
  effect = CTaF_GetUp_CE;
  constraint = "impactedObject";
  lifetime = 2.2;
  delay = 2.2;
  lifeConstraint = "impactedObject";
  lifeConditions = $AFX::ALIVE;
};

datablock afxAnimLockData(CTaF_AnimLock_CE)
{
  priority = 0;
};
//
datablock afxEffectWrapperData(CTaF_AnimLock_EW)
{
  effect = CTaF_AnimLock_CE;
  constraint = "impactedObject";
  lifetime = 4.5;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// TARGET DUST

//
// When the target falls to the ground, dust is forced-up beneath
// him.  To approximate the area the falling orc strikes a disc
// emitter is used.  The particle types are reused from the casting
// dust.
//

datablock afxParticleEmitterDiscData(CTaF_fallDust_E) // TGEA
{
  ejectionPeriodMS      = 15;
  periodVarianceMS      = 4;
  ejectionVelocity      = 0.3;
  velocityVariance      = 0.1;
  particles             = "CTaF_Dust1_P CTaF_Dust2_P";

  // TGE emitterType = "disc";
  vector = "0 0 1";
  radiusMin = 1.0;
  radiusMax = 2.5;
};
// 
datablock afxEffectWrapperData(CTaF_FallDust_EW)
{
  effect = CTaF_fallDust_E;
  constraint = "impactedObject";
  lifetime = 0.6;
  delay = 0.7;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// AUDIO 

switch$ (afxGetEngine())
{
  case "TGE":
    exec("./audio/ctaf_audio_sub.cs");
  case "TGEA":
    exec("./audio/ctaf_audio_sub.cs");
  case "T3D":
    exec("./audio/ctaf_audio_sub.cs");
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// CANTRIP AND FALL SPELL
//

datablock afxMagicSpellData(CantripAndFallSpell)
{
  castingDur = 0.85;
  directDamage = 1.0;

    // spellcaster animation //
  addCastingEffect = CTaF_Casting_Clip_EW;
    // casting dust //
  addCastingEffect = CTaF_FootDust_EW;

    // banana peel //
  addImpactEffect = CTaF_Banana_EW;
    // target animation //
  addImpactEffect = CTaF_TripFall_EW;
  addImpactEffect = CTaF_GetUp_EW;
  addImpactEffect = CTaF_AnimLock_EW;
    // target dust //
  addImpactEffect = CTaF_FallDust_EW;
};

// sounds added via sub-script functions //
CTaF_add_Audio_FX(CantripAndFallSpell);

datablock afxRPGMagicSpellData(CantripAndFallSpell_RPG)
{
  spellName = "Cantrip and Fall";
  desc = "An old mage school prank. Take a running orc, " @ 
         "add one teleported banana peel, and presto!" @
         "\n" @
         "\nspell design: Jeff Faust, Matthew Durante" @
         "\nsound effects: Dave Schroeder" @ 
         "\nspell concept: Jeff Faust";
  sourcePack = "Spell Pack 1";      
  iconBitmap = %mySpellDataPath @ "/CTaF/icons/ctaf";
  target = "enemy";
  range = 50;
  manaCost = 10;
  directDamage = 1.0;
  castingDur = CantripAndFallSpell.castingDur;
};

//UAISK+AFX Interop Changes: Start
function CantripAndFallSpell::onImpact(%this, %spell, %caster, %impObj, %impPos, %impNorm)
{
  Parent::onImpact(%this, %spell, %caster, %impObj, %impPos, %impNorm);

  if ($UAISK_Is_Available && %impObj.isBot)
  {
    %lockSpell = "CTaF_AnimLock_EW";
    %impObj.schedule(%lockSpell.delay*1000, "doSpecialMove", %lockSpell.lifetime*1000);
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
  CantripAndFallSpell.scriptFile = $afxAutoloadScriptFile;
  CantripAndFallSpell.spellDataPath = %mySpellDataPath;
  if (isFunction(addDemoSpellbookSpell))
    addDemoSpellbookSpell(CantripAndFallSpell, CantripAndFallSpell_RPG);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
