
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// RID OF HABEAS CORPUS
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
$spell_reload = isObject(HabeasCorpusSpell);
if ($spell_reload)
{
  // mark datablocks so we can detect which are reloaded this script
  markDataBlocks();
  // reset data path from previously saved value
  %mySpellDataPath = HabeasCorpusSpell.spellDataPath;
}
else
{
  // set data path from default plus containing folder name
  %mySpellDataPath = $afxSpellDataPath @ "/" @ $afxAutoloadScriptFolder;
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

// EXPLOSIONS

if (afxGetEngine() $= "T3D")
  $RoHC_Debris_Model = "art/shapes/weapons/crossbow/debris.dts";
else
  $RoHC_Debris_Model = "~/data/shapes/crossbow/debris.dts";

datablock DebrisData(RoHC_Debris_CE)
{
   shapeFile = $RoHC_Debris_Model;
   emitters = "CrossbowDebrisTrailEmitter";
   explosion = CrossbowDebrisExplosion;
   elasticity = 0.6;
   friction = 0.5;
   numBounces = 1;
   bounceVariance = 1;
   explodeOnMaxBounce = true;
   staticOnMaxBounce = false;
   snapOnMaxBounce = false;
   minSpinSpeed = 0;
   maxSpinSpeed = 700;
   render2D = false;
   lifetime = 4;
   lifetimeVariance = 0.4;
   velocity = 5;
   velocityVariance = 0.5;
   fade = false;
   useRadiusMass = true;
   baseRadius = 0.3;
   gravModifier = 0.5;
   terminalVelocity = 6;
   ignoreWater = true;
};

datablock ExplosionData(RoHC_Explosion_CE)
{
   soundProfile = CrossbowExplosionSound;
   lifeTimeMS = 1200;

   // Volume particles
   particleEmitter = CrossbowExplosionFireEmitter;
   particleDensity = 75;
   particleRadius = 2;

   // Point emission
   emitter[0] = CrossbowExplosionSmokeEmitter;
   emitter[1] = CrossbowExplosionSparkEmitter;

   // Sub explosion objects
   subExplosion[0] = CrossbowSubExplosion1;
   subExplosion[1] = CrossbowSubExplosion2;
   
   // Camera Shaking
   shakeCamera = true;
   camShakeFreq = "10.0 11.0 10.0";
   camShakeAmp = "1.0 1.0 1.0";
   camShakeDuration = 0.5;
   camShakeRadius = 10.0;

   // Exploding debris
   debris = RoHC_Debris_CE;
   debrisThetaMin = 0;
   debrisThetaMax = 60;
   debrisPhiMin = 0;
   debrisPhiMax = 360;
   debrisNum = 6;
   debrisNumVariance = 2;
   debrisVelocity = 1;
   debrisVelocityVariance = 0.5;
   
   // Impulse
   impulseRadius = 10;
   impulseForce = 15;

   // Dynamic light
   lightStartRadius = 6;
   lightEndRadius = 3;
   lightStartColor = "0.5 0.5 0";
   lightEndColor = "0 0 0";
};

datablock afxEffectWrapperData(RoHC_Boom1_EW)
{
  effect = RoHC_Explosion_CE;
  delay = 0.0;
  constraint = "impactedObject.Bip01 L Foot";
};

datablock afxEffectWrapperData(RoHC_Boom2_EW)
{
  effect = RoHC_Explosion_CE;
  delay = 0.3;
  constraint = "impactedObject.Eye";
};

datablock afxEffectWrapperData(RoHC_Boom3_EW)
{
  effect = RoHC_Explosion_CE;
  delay = 1.1;
  constraint = "impactedObject.Bip01 R Hand";
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// ANIMATION

datablock afxAnimClipData(RoHC_SummonClip_CE)
{
  clipName = "summon";
};
datablock afxEffectWrapperData(RoHC_SummonClip_EW)
{
  effect = RoHC_SummonClip_CE;
  constraint = "caster";
};

datablock afxAnimClipData(RoHC_Finish1Clip_CE)
{
  clipName = "throw";
};
datablock afxEffectWrapperData(RoHC_Finish1Clip_EW)
{
  effect = RoHC_Finish1Clip_CE;
  constraint = "caster";
  delay = 0.25;
  lifetime = 1.5;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// STAIN

datablock afxZodiacData(RoHC_Fireblast_CE)
{
  texture = %mySpellDataPath @ "/RoHC/zodiacs/fireblast_decal";
  radius = 2.5;
  startAngle = 0.0;
  rotationRate = 0.0;
  color = "1.0 1.0 1.0 1.0";
  interiorHorizontalOnly = true;
};

datablock afxEffectWrapperData(RoHC_Fireblast_EW)
{
  effect = RoHC_Fireblast_CE;
  constraint = "impactPoint";
  lifetime = 0;
  residueLifetime = 10;
  fadeOutTime = 5;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// RID OF HABEAS CORPUS SPELL
//

datablock afxMagicSpellData(HabeasCorpusSpell)
{
  castingDur = 0.5;
  //
  addCastingEffect = RoHC_SummonClip_EW;
  addCastingEffect = RoHC_Finish1Clip_EW;
  //
  addImpactEffect = RoHC_Boom1_EW;
  addImpactEffect = RoHC_Boom2_EW;
  addImpactEffect = RoHC_Boom3_EW;
  addImpactEffect = RoHC_Fireblast_EW;
};
//
datablock afxRPGMagicSpellData(HabeasCorpusSpell_RPG)
{
  spellName = "Rid of Habeas Corpus";
  desc = "Rid your encampment of that unsightly corpse clutter.\n" @
         "Blow the pesky stiffs to permanent oblivion with this handy sorcery.\n" @
         "\n" @
         "[experimental spell]"; 
  sourcePack = "Core Tech";
  iconBitmap = %mySpellDataPath @ "/RoHC/icons/rohc";
  target = "corpse";
  range = 20;
  manaCost = 10;
  castingDur = HabeasCorpusSpell.castingDur;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// script methods

function HabeasCorpusSpell::onImpact(%this, %spell, %caster, %impObj, %impPos, %impNorm)
{
  afxBurnCorpse(%impObj);
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
  HabeasCorpusSpell.scriptFile = $afxAutoloadScriptFile;
  HabeasCorpusSpell.spellDataPath = %mySpellDataPath;
  if (isFunction(addDemoSpellbookSpell))
    addDemoSpellbookSpell(HabeasCorpusSpell, HabeasCorpusSpell_RPG);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//


