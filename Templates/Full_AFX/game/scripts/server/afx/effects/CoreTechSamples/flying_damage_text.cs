
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// FLYING DAMAGE TEXT EFFECT
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
$spell_reload = isObject(FlyingDamageTextEffectron);
if ($spell_reload)
{
  // mark datablocks so we can detect which are reloaded this script
  markDataBlocks();
  // reset data path from previously saved value
  %mySpellDataPath = FlyingDamageTextEffectron.spellDataPath;
}
else
{
  // set data path from default plus containing folder name
  %mySpellDataPath = $afxSpellDataPath @ "/" @ $afxAutoloadScriptFolder;
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock afxXM_WorldOffsetData(FDT_offset_XM)
{
  worldOffset = "0.0 0.0 5";
  lifetime = 2.8;
  fadeInTime = 2.8;
  fadeInEase = "0.0 0.2";
};

datablock afxGuiTextData(FDT_Label_CE)
{
  text = "$$ %%._text";
  color = "1 0 0 1";
};
datablock afxEffectWrapperData(FDT_Label_EW)
{
  effect = FDT_Label_CE;
  posConstraint = "damaged.#center";
  xfmModifiers[0] = FDT_offset_XM;
  fadeInTime = 0.5;
  lifetime = 2;
  fadeOutTime = 0.8;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// effectron

datablock afxEffectronData(FlyingDamageTextEffectron)
{
  addEffect = FDT_Label_EW;
};

function displayFlyingDamageText(%damaged, %amount)
{
  if (!isObject(FlyingDamageTextEffectron))
    return;

  if (%damaged.dmg_accum > 0)
  {
    %damaged.dmg_accum += %amount;
  }
  else
  {
    %damaged.dmg_accum = %amount;
    schedule(0, 0, spewFlyingDamageText, %damaged);
  }
}

function spewFlyingDamageText(%damaged)
{
  if (%damaged.dmg_accum >= 1)
  {
    %effectron = new afxEffectron() 
    {
      datablock = FlyingDamageTextEffectron;
      _text = %damaged.dmg_accum;
    };
    %effectron.addConstraint(%damaged, "damaged");
  }
  %damaged.dmg_accum = 0;
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
  FlyingDamageTextEffectron.scriptFile = $afxAutoloadScriptFile;
  FlyingDamageTextEffectron.spellDataPath = %mySpellDataPath;
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

