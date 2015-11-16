
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
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

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
//  afxPerformSpellbookCast()
//
//    Used to cast spells from the player's spellbook.
//    Used by the spellbank gui to cast spells
//
function afxPerformSpellbookCast(%caster, %book_slot, %target, %client)
{
  %spell_game_data = %client.spellbook.getSpellRPGData(%book_slot);
  if (!isObject(%spell_game_data))
  {
    DisplayScreenMessage(%client, "Failed to find spell definition in spellbook. (casting)");
    return;
  }

  %spell_data = %spell_game_data.spellFXData;
  if (!isObject(%spell_data))
  {
    DisplayScreenMessage(%client, "Failed to find spell effects in spellbook. (casting)");
    return;
  }

  afxPerformSpellCast(%caster, %spell_game_data, %spell_data, %target, %client, "");
}

function afxPerformFreeTargetingSpellbookCast(%caster, %book_slot, %free_target, %client)
{
  %spell_game_data = %client.spellbook.getSpellRPGData(%book_slot);
  if (!isObject(%spell_game_data))
  {
    DisplayScreenMessage(%client, "Failed to find spell definition in spellbook. (casting)");
    return;
  }

  %spell_data = %spell_game_data.spellFXData;
  if (!isObject(%spell_data))
  {
    DisplayScreenMessage(%client, "Failed to find spell effects in spellbook. (casting)");
    return;
  }

  if (%free_target $= "")
  {
    DisplayScreenMessage(%client, "Invalid target location.");
    return;
  }

  afxPerformSpellCast(%caster, %spell_game_data, %spell_data, "", %client, %free_target);
}

