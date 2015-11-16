
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// SPACE HELMET SCI-FI EFFECT
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
$effect_reload = isObject(Equip_SciFi_Gear);
if ($effect_reload)
{
  // mark datablocks so we can detect which are reloaded this script
  markDataBlocks();
  // reset data path from previously saved value
  %mySpellDataPath = Equip_SciFi_Gear.spellDataPath;
}
else
{
  // set data path from default plus containing folder name
  %mySpellDataPath = $afxSpellDataPath @ "/" @ $afxAutoloadScriptFolder;
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock afxModelData(SCIFI_Helmet_CE)
{
  shapeFile = %mySpellDataPath @ "/SF_GEAR/models/helmet.dts";
  sequence = "animate";
};

datablock afxXM_LocalOffsetData(SCIFI_Helmet_Offset_XM)
{
  localOffset = "0.055 -0.11 0";  // x=up, y=forward/back
};

datablock afxEffectWrapperData(SCIFI_Helmet_EW)
{
  effect = SCIFI_Helmet_CE;

  constraint = "astronaut.Bip01 Head";

  delay       = 0;
  fadeInTime  = 0.5;
  fadeOutTime = 0.5;
  //lifetime    = 1000;

  xfmModifiers[0] = SCIFI_Helmet_Offset_XM;
};

datablock afxModelData(SCIFI_HelmetLight_CE)
{
  shapeFile = %mySpellDataPath @ "/SF_GEAR/models/helmet_light.dts";
  sequence = "animate";
  forceOnMaterialFlags = $MaterialFlags::Additive | $MaterialFlags::SelfIlluminating; // TGE (ignored by TGEA)
  remapTextureTags = "helmet.png:helmet_light"; // TGEA (ignored by TGE)
};
//
datablock afxEffectWrapperData(SCIFI_HelmetLight_EW : SCIFI_Helmet_EW)
{
  effect = SCIFI_HelmetLight_CE;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock afxEffectronData(Equip_SciFi_Gear)
{
  duration = $AFX::INFINITE_TIME;
  execOnNewClients = true;
  addEffect = SCIFI_Helmet_EW;
  addEffect = SCIFI_HelmetLight_EW;
};

function equipSciFiOrc(%orc)
{
  if (%orc.dataBlock !$= SpaceOrcMageData && %orc.dataBlock !$= SpaceOrcMage_Night_Data)
    %orc.scifi_gear = startEffectron(Equip_SciFi_Gear, %orc, "astronaut");
}

function unequipSciFiOrc(%orc)
{
  if (isObject(%orc.scifi_gear))
  {
    %orc.scifi_gear.interrupt();
    %orc.scifi_gear = "";
  }
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

if ($effect_reload)
{
  // Removes then adds all reloaded datablocks
  touchDataBlocks();
}
else
{
  // save script filename and data path for reloads
  Equip_SciFi_Gear.scriptFile = $afxAutoloadScriptFile;
  Equip_SciFi_Gear.spellDataPath = %mySpellDataPath;
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
