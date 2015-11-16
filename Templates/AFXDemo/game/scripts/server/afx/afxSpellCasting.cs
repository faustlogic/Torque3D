 
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

// Note - Players and NPCs can cast spells using afxPerformSpellCast(). 
// When used to cast an NPC spell, set the %client argument to "".
// This prevents message display.
function afxPerformSpellCast(%caster, %rpg_data, %spell_data, %target, %client, %free_target)
{
  if (!isObject(%rpg_data) || !isObject(%spell_data) || %rpg_data.getClassName() !$= "afxRPGMagicSpellData" || %spell_data.getClassName() !$= "afxMagicSpellData")
  {
    DisplayScreenMessage(%client, "Invalid spell definition.");
    return;
  }

  // test if caster exists
  if (!isObject(%caster))
  {
    DisplayScreenMessage(%client, "This client has no spellcaster.");
    return;
  }
 
  // test if caster is alive (enabled)
  if (!%caster.isEnabled())
  {
    DisplayScreenMessage(%client, "You're dead.");
    return;
  }

  // test if caster is already casting a spell
  if (isObject(%caster.spellBeingCast))
  {
    DisplayScreenMessage(%client, "Already casting another spell.");
    return;
  }

  // test if caster is anim-locked
  if (%caster.isAnimationLocked())
  {
    DisplayScreenMessage(%client, "You're unable to concentrate.");
    return;
  }

  %mana_cost = %rpg_data.manaCost;
  %mana_pool = %caster.getEnergyLevel();

  // test if caster has enough mana
  if (%mana_pool < %mana_cost)
  {
    DisplayScreenMessage(%client, "Not enough mana.");
    return;
  }

  %tgt = %rpg_data.target;

  // clear superfluous target 
  if ((%tgt $= "nothing" || %tgt $= "free") && !%rpg_data.targetOptional)
    %target = 0;

  // test if free target is required
  if (%free_target $= "" && %tgt $= "free")
  {
    DisplayScreenMessage(%client, "Spell requires a freely positioned target.");
    return;
  }

  // test if target is required
  if (!isObject(%target) && !%rpg_data.targetOptional)
  {
    if (%tgt $= "enemy" || %tgt $= "corpse" || %tgt $= "friend")
    {
      DisplayScreenMessage(%client, "Spell requires a target.");
      return;
    }
  }

  // validate target
  if (isObject(%target))
  {
    // make sure corpse targets are really dead 
    if (%tgt $= "corpse" && %target.isEnabled())
    {
      DisplayScreenMessage(%client, "Try targeting something that's dead.");
      return;
    }

    // make sure targeting self is allowed 
    if (%target $= %caster && %tgt !$= "self" && !%rpg_data.canTargetSelf)
    {
      DisplayScreenMessage(%client, "Casting this spell on yourself is not good idea.");
      return;
    }

    // check range
    if (%rpg_data.range > 0)
    {
      %target_dist = VectorDist(%caster.getWorldBoxCenter(), %target.getWorldBoxCenter());
      if (%target_dist > %rpg_data.range)
      {
        DisplayScreenMessage(%client, "Target is out of range.");
        return;
      }
    }
  }

  // self-targeting
  if (%tgt $= "self")
    %target = %caster;

  // spell datablock gets last chance to find reasons to fizzle
  if (!%spell_data.readyToCast(%caster, %target))
    return;

  if (%caster.replaceSpellTarget !$= "")
  {
     %target = %caster.replaceSpellTarget;
     %caster.replaceSpellTarget = "";
  }

  if (isObject(%client)) // for cooldown display 
    %caster.activeSpellbook = %client.spellbook;
   
  %spell = castSpell(%spell_data, %caster, %target, %rpg_data);
  if (%free_target !$= "")
  {
    %spell.addConstraint(%free_target, "freeTarget");
    %spell.freeTarget = %free_target;
  }
}

function afxTestSpellcastingDamageInterruption(%caster, %damage, %damageType)
{
  if (!isObject(%caster.spellBeingCast))
    return;

  %spell = %caster.spellBeingCast;
  %rpg_data = %spell.extra;
  if (%rpg_data.allowDamageInterrupts && %rpg_data.minDamageToInterrupt <= %damage)
  {
    %spell.interrupt();
    if (isObject(%caster.client))
      DisplayScreenMessage(%caster.client, "Spellcasting interrupted by damage.");
  }
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
//  performSpellCastingInterrupt()
//
//    Used to interrupt a spell being cast by the player
//    associated with the given client.
//
function performSpellCastingInterrupt(%client)
{
  %caster = %client.player;
  if (isObject(%client.player.spellBeingCast))
  {
    %client.player.spellBeingCast.interrupt();
    %client.player.spellBeingCast = "";
    DisplayScreenMessage(%client, "Spellcasting interrupted.");
  }
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
