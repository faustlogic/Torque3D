
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

if (!isObject(afxCoreTechModules))
{
  new ScriptObject(afxCoreTechModules) 
  { 
    moduleName = "CoreTech";
    script[0] = "std_selectron.cs";
    script[1] = "selectrons.cs";
    script[2] = "astral_passport.cs";
    script[3] = "flying_damage_text.cs";
    script[4] = "great_ball_of_fire.cs";
    script[5] = "light_my_fire.cs";
    script[6] = "mark_of_gg.cs";
    script[7] = "phrase_tester.cs";
    script[8] = "presto_change_orc.cs";
    script[9] = "reaper_madness.cs";
    script[10] = "rid_of_habeas_corpus.cs";
    script[11] = "sf_gear.cs";
    script[12] = "sf_occams_laser.cs";
    script[13] = "smokin.cs";
    script[14] = "try_alphafade.cs";
    script[15] = "try_flying.cs";
    script[16] = "try_gui_text.cs";
    script[17] = "try_machinegun.cs";
    script[18] = "try_phys_zone.cs";
    script[19] = "try_puppet_cam.cs";
    script[20] = "wave_tester.cs";
    script[21] = "light_tester.cs";
    script[22] = "end";
  };

  new ScriptObject(afxSpellPack1Modules) 
  { 
    moduleName = "SpellPack1";
    script[0] = "selectrons.cs";
    script[1] = "spirit_of_roach.cs";
    script[2] = "flame_broil.cs";
    script[3] = "mapleleaf_frag.cs";
    script[4] = "cantrip_and_fall.cs";
    script[5] = "summon_moth.cs";
    script[6] = "arcane_membrane.cs";
    script[7] = "thors_hammer.cs";
    script[8] = "insectoplasm.cs";
    script[9] = "end";
  };
  new ScriptObject(afxSpellPack2Modules) 
  { 
    moduleName = "SpellPack2";
    script[0] = "sp2_shared_fx.cs";
    script[1] = "up_in_flames.cs";
    script[2] = "selectrons.cs";
    script[3] = "the_letter_k.cs";
    script[4] = "soul_nuke.cs";
    script[5] = "soul_mine_solo.cs";
    script[6] = "ring_of_dust.cs";
    script[7] = "clusters_of_fire.cs";
    script[8] = "bolt_from_the_blue.cs";
    script[9] = "bron_y_orc_stomp.cs";
    script[10] = "chill_kill.cs"; // model/material troubles
    script[11] = "wandering_wisps.cs";
    script[12] = "ring_of_fire.cs";
    script[13] = "soul_sucking_jerk.cs";
    script[14] = "pillar_of_fire.cs";
    script[15] = "dread_bull_portent.cs";
    script[16] = "flaming_stick_trick.cs";
    script[17] = "mark_of_kork.cs";
    script[18] = "try_rocket_orc.cs";
    script[19] = "soul_miners_slaughter.cs";
    script[20] = "green_legs_and_scram.cs";
    script[21] = "shards_of_vesuvius.cs"; // particle pools
    script[22] = "fire_in_the_sky.cs"; // misc.
    script[23] = "end";
  };
}

function load_afx_module_pack(%pack)
{
  error("Loading scripts for pack \"" @ %pack.moduleName @ "\"");
  $afxAutoloadScriptFolder = %pack.moduleName;
  for (%pack_mod = 0; %pack.script[%pack_mod] !$= "end"; %pack_mod++)
  {
    if (%pack.script[%pack_mod] !$= "")
    {
      error("   Loading script \"" @ %pack.script[%pack_mod] @ "\"");
      $afxAutoloadScriptFile = expandFilename("./effects/" @ %pack.moduleName @ "/" @ %pack.script[%pack_mod]);
      exec($afxAutoloadScriptFile);
    }
  }

  $afxAutoloadScriptFile = "";
  $afxAutoloadScriptFolder = "";
}

function loadAFXModulesFIXED()
{
  load_afx_module_pack(afxCoreTechModules);
  load_afx_module_pack(afxSpellPack1Modules);
  load_afx_module_pack(afxSpellPack2Modules);
}

function afxExecPrerequisite(%filename, %folder)
{
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
