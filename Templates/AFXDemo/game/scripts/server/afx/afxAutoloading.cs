
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// Functions for AFX Autoloading 
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

// autoloading of effects

function autoloadAFXModules()
{
  afxLoadedModules.clear();

  $afxEngine = (isFunction(afxGetEngine)) ? afxGetEngine() : "TGE";

  %pattern = $afxEffectsScriptsPath @ "/*/*.cs";

  %n_found = 0;

  // loop thru all scripts that match the pattern
  for (%file = findFirstFile(%pattern); %file !$= ""; %file = findNextFile(%pattern)) 
  {
    %filebase = fileBase(%file);
    %filebase_len = strlen(%filebase);

    // skip over scripts with names ending with "_sub"
    if (%filebase_len > 4)
    {
      if (strcmp(getSubStr(%filebase, %filebase_len-4, 4), "_sub") == 0)
        continue;
    }

    // skip over scripts that are in the disabled-list
    if ($pref::AFX::enableFXLoadFiltering && afxDisabledModules.isInList(%file))
      continue;

    // skip over scripts that have already been autoloaded
    if (afxLoadedModules.isInList(%file))
      continue;

    // extract parent folder name from script's path
    %path = filePath(%file);
    %tokens = %path;
    %folder = "";
    while (%tokens !$= "")
    {
      %tokens = nextToken(%tokens, "tokfolder", "/");
      if (%tokfolder !$= "")
        %folder = %tokfolder;
    }

    $afxAutoloadScriptFile = %file;
    $afxAutoloadScriptFolder = %folder;

    // add to list before the exec to prevent cycles
    afxLoadedModules.add($afxAutoloadScriptFile);

    // exec the script here
    exec($afxAutoloadScriptFile);

    $afxAutoloadScriptFile = "";
    $afxAutoloadScriptFolder = "";

    %n_found++;
  }

  if (%n_found == 0)
    error("No AFX Modules Found.");
}

function afxStandardAssetsPath(%scriptFile)
{
  // extract parent folder name from script's path
  %tokens = filePath(%scriptFile);
  %folder = "";
  while (%tokens !$= "")
  {
    %tokens = nextToken(%tokens, "tokfolder", "/");
    if (%tokfolder !$= "")
      %folder = %tokfolder;
  }

  return $afxEffectsDataPath @ "/" @ %folder;
}

function afxExecPrerequisite(%filename, %folder)
{
  // assemble a fullname for the script 
  %fullname = $afxEffectsScriptsPath @ "/" @ %folder @ "/" @ %filename;

  // check if script exists
  if (!isFile(%fullname))
  {
    error("Error: afxExecPrerequisite() failed to locate a script matching," @ %filename @ ".");
    return;
  }

  // check if script has already been auto-loaded
  if (afxLoadedModules.isInList(%fullname))
   return;

  // has not auto-loaded so do it now...

  // save the auto globals
  %afxAutoloadScriptFile_save = $afxAutoloadScriptFile;
  %afxAutoloadScriptFolder_save = $afxAutoloadScriptFolder;

  $afxAutoloadScriptName = %filename;
  $afxAutoloadScriptFile = %fullname;
  $afxAutoloadScriptFolder = %folder;

  // add to list before the exec to prevent cycles
  afxLoadedModules.add($afxAutoloadScriptFile);

  // exec the script here
  exec($afxAutoloadScriptFile);

  // restore the auto globals
  $afxAutoloadScriptFile = %afxAutoloadScriptFile_save;
  $afxAutoloadScriptFolder = %afxAutoloadScriptFolder_save;
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// afxNameList -- a simple name-list 

function afxNameList::isInList(%this, %name)
{
  %name = makeRelativePath(%name, getMainDotCSDir());
  for (%i = 0; %i < %this.count; %i++)
  {
    if (%name $= %this.names[%i])
      return true;
  }

  return false;
}

function afxNameList::add(%this, %name)
{
  %this.names[%this.count] = makeRelativePath(%name, getMainDotCSDir());
  %this.count++;
}

function afxNameList::clear(%this)
{
  for (%i = 0; %i < %this.count; %i++)
    %this.names[%i] = "";
  %this.count = 0;
}

//~~~~~~~~~~~~~~~~~~~~//

new ScriptObject(afxLoadedModules) 
{ 
  superclass = "afxNameList";
  count = 0; 
};

new ScriptObject(afxDisabledModules) 
{ 
  superclass = "afxNameList";
  count = 0; 
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

