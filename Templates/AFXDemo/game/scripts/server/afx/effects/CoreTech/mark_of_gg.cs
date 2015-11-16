
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// MARK OF GG SPELL
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
  error("AFX script " @ fileName($Con::File) @ " is not compatible with AFX versions older than " @ $MIN_REQUIRED_VERSION @ ".");
  return;
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

//
// Here we test if the script is being reloaded or if this is the
// first time the script has executed this mission.
//
$spell_reload = isObject(MarkOfGGSpell);
if ($spell_reload)
{
  // mark datablocks so we can detect which are reloaded within this script
  markDataBlocks();
}

%mySpellDataPath = afxStandardAssetsPath($Con::File);

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock afxAnimClipData(MGG_SummonClip_CE)
{
  clipName = "summon";
  rate = 3;
  rate = "$$ getRandomF(0.1, 0.9)";
};
//
datablock afxEffectWrapperData(MGG_SummonClip_EW)
{
  effect = MGG_SummonClip_CE;
  constraint = "caster";
  lifetime = 1.0;
};

datablock afxZodiacData(MGG_LogoZodiac_CE : SHARED_ZodiacBase_CE)
{
  texture = %mySpellDataPath @ "/MGG/zodiacs/mgg_zode";
  radius = 2.5;
  startAngle = 0;
  rotationRate = 0.0;
  color = "1.0 1.0 1.0 0.6";
};

datablock afxEffectWrapperData(MGG_LogoZodiac_EW)
{
  effect = MGG_LogoZodiac_CE;
  constraint = caster;
  lifetime = 0;
  residueLifetime = 10;
  fadeOutTime = 5;
  xfmModifiers[0] = SHARED_AltitudeConform_XM;
};


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// spell

datablock afxMagicSpellData(MarkOfGGSpell)
{
  castingDur = 0.25;
  allowMovementInterrupts = false;
  //
  addCastingEffect = MGG_LogoZodiac_EW;
  addCastingEffect = MGG_SummonClip_EW;
};
//
datablock afxRPGMagicSpellData(MarkOfGGSpell_RPG)
{
  spellName = "Mark of GG";
  desc = "Tag the ground with the GarageGames logo." @
          "\n\n[novelty spell]";
  sourcePack = "Core Tech";
  iconBitmap = %mySpellDataPath @ "/MGG/icons/mgg";
  target = "nothing";
  manaCost = 0;
  castingDur = MarkOfGGSpell.castingDur;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

if ($spell_reload)
{
  // Removes then adds back all datablocks reloaded by this script 
  touchDataBlocks();
}
else
{
  if (isFunction(addDemoSpellbookSpell))
    addDemoSpellbookSpell(MarkOfGGSpell, MarkOfGGSpell_RPG);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

