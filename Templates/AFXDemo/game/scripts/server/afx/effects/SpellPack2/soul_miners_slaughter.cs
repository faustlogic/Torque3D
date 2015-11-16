
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// SOUL MINERS SLAUGHTER SPELL
//
//    Convert a handful of Soul Substance into a deadly field of scattered Soul Mines.
//
//    This spell is actually a variation of Mark of Kork. This script is just a front-end
//    which includes an afxRPGMagicSpellData datablock with parameter settings for 
//    Soul Miner's Slaughter. Most of the effect is implemented by the MarkOfKorkSpell.
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
// Here we test if the script is being reloaded or if this is the
// first time the script has executed this mission.
//
$spell_reload = isObject(SoulMinersSlaughterSpell_RPG);
if ($spell_reload)
{
  // mark datablocks so we can detect which are reloaded this script
  markDataBlocks();
}
else
{
  // Soul Miner's Slaughter uses elements from this other script
  afxExecPrerequisite("mark_of_kork.cs", $afxAutoloadScriptFolder);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

datablock afxRPGMagicSpellData(SoulMinersSlaughterSpell_RPG)
{
  spellName = "Soul Miner's Slaughter"; 
  desc = "Convert a handful of Soul Substance into a deadly field of scattered Soul Mines.\n" @
         "\n" @
         "<font:Arial Italic:14>spell design and concept: <font:Arial:14>Matthew Durante\n" @
         "<font:Arial Italic:14>sound effects: <font:Arial:14>Matt Pacyga";
  sourcePack = "Spell Pack 2";
  iconBitmap = $afxSpellDataPath @ "/" @ "SpellPack2/MoK/icons/sms";
  target = "nothing";
  manaCost = 10;
  castingDur = 4.0;

  _castDur = 4.0;
  _sms = 1;
  _flav = "sms";
  _sparks = 0;
  _hand = 0;
};
  
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

if ($spell_reload)
{
  // Removes then adds all reloaded datablocks
  touchDataBlocks();
}
else
{
  if (isFunction(addDemoSpellbookSpell))
  {
    addDemoSpellbookSpell(MarkOfKorkSpell, SoulMinersSlaughterSpell_RPG);
  }
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
