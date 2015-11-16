
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// This script is made specifically for the Full_with_AFX project template and
// is used to load the small set of sample scripts used in the project.
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

// setup the effects data paths
$afxSpellDataPath = expandFilename("art/afx/effects");
$afxSpellScriptsPath = expandFilename("scripts/server/afx/effects");

// reset the selectron styles
resetDemoSelectronStyles();

if (!isObject(afxSampleCoreTechModules))
{
   new ScriptObject(afxSampleCoreTechModules) 
   { 
      moduleName = "CoreTechSamples";
      script[0] = "std_selectron.cs";
      script[1] = "selectrons.cs";
      script[2] = "flying_damage_text.cs";
      script[3] = "great_ball_of_fire.cs";
      script[4] = "end";
   };
 }

// init shared effects datablocks
exec("./afxSharedFX.cs");

function afxExecPrerequisite(%filename, %folder)
{
}

function load_afx_module_pack(%pack)
{
   echo("\c4Loading sample scripts for pack \"" @ %pack.moduleName @ "\"");
   $afxAutoloadScriptFolder = %pack.moduleName;
   for (%pack_mod = 0; %pack.script[%pack_mod] !$= "end"; %pack_mod++)
   {
      if (%pack.script[%pack_mod] !$= "")
      {
         echo("\c4   Loading script \"" @ %pack.script[%pack_mod] @ "\"");
         $afxAutoloadScriptFile = expandFilename("./effects/" @ %pack.moduleName @ "/" @ %pack.script[%pack_mod]);
         exec($afxAutoloadScriptFile);
       }
   }

   $afxAutoloadScriptFile = "";
   $afxAutoloadScriptFolder = "";
}

echo("\c4Load Sample AFX Modules...");

load_afx_module_pack(afxSampleCoreTechModules);

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
