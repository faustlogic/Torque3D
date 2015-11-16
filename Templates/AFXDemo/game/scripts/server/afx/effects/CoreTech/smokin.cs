
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// SMOKIN SPELL
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
$spell_reload = isObject(SmokinSpell);
if ($spell_reload)
{
  // mark datablocks so we can detect which are reloaded this script
  markDataBlocks();
  // reset data path from previously saved value
  %mySpellDataPath = SmokinSpell.spellDataPath;
}
else
{
  // set data path from default plus containing folder name
  %mySpellDataPath = $afxSpellDataPath @ "/" @ $afxAutoloadScriptFolder;
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock ParticleData(SMKN_Smoke_P)
{
   textureName = %mySpellDataPath @ "/SMKN/particles/smoke";
   dragCoefficient = 0.0;
   gravityCoefficient = -0.05;
   inheritedVelFactor = 1.00;
   lifetimeMS = 1500;
   lifetimeVarianceMS = 250;
   useInvAlpha = false;
   spinRandomMin = -30.0;
   spinRandomMax = 30.0;
   colors[0] = "0.4 0.4 0.4 0.06";
   colors[1] = "0.5 0.5 0.5 0.1";
   colors[2] = "0.6 0.6 0.6 0.0";
   sizes[0] = 0.4;
   sizes[1] = 0.6;
   sizes[2] = 0.8;
   times[0] = 0.0;
   times[1] = 0.5;
   times[2] = 1.0;
};

datablock ParticleEmitterData(SMKN_Smoke_E)
{
   ejectionPeriodMS = 10;
   periodVarianceMS = 5;
   ejectionVelocity = 0.25;
   velocityVariance = 0.10;
   thetaMin = 0.0;
   thetaMax = 90.0;  
   particles = SMKN_Smoke_P;
};

datablock afxEffectWrapperData(SMKN_Smoke_rt_foot_EW)
{
  effect = SMKN_Smoke_E;
  constraint = "target.Ski0";
};

datablock afxEffectWrapperData(SMKN_Smoke_lf_foot_EW)
{
  effect = SMKN_Smoke_E;
  constraint = "target.Ski1";
};
 
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// spell

datablock afxMagicSpellData(SmokinSpell)
{
  lingerDur = $AFX::INFINITE_TIME;

  addLingerEffect = SMKN_Smoke_rt_foot_EW;
  addLingerEffect = SMKN_Smoke_lf_foot_EW;
  execOnNewClients = true;
};
//
datablock afxRPGMagicSpellData(SmokinSpell_RPG)
{
  spellName = "Smokin'";
  desc = "Permanently attaches smoke emitters to the target's feet. " @ 
         "Cast again at the same target to stop the smoke.\n\n" @
         "[experimental effect]"; 
  sourcePack = "Core Tech";
  iconBitmap = %mySpellDataPath @ "/SMKN/icons/smkn";
  target = "friend";
  canTargetSelf = true;
  manaCost = 10;
  castingDur = SmokinSpell.castingDur;
};

function SmokinSpell::onImpact(%this, %spell, %caster, %impObj, %impPos, %impNorm)
{
  if (isObject(%impObj))
  {
    if (isObject(%impObj.lingering_spell))
    {
      // note - this interrupts both the new instance and the old
      %spell.interrupt();
      %impObj.lingering_spell.interrupt();
      %impObj.lingering_spell = "";
    }
    else
    {
      %impObj.lingering_spell = %spell;
    }
  }
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
  SmokinSpell.scriptFile = $afxAutoloadScriptFile;
  SmokinSpell.spellDataPath = %mySpellDataPath;
  if (isFunction(addDemoSpellbookSpell))
    addDemoSpellbookSpell(SmokinSpell, SmokinSpell_RPG);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

