
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// RING OF FIRE (client script)
//
//  This script file should be specified using the clientScriptFile field in
//  afxMagicSpellData, or afxEffectronData. It will be exec'd from the
//  datablock's client-side preload() method.
//
//  Use the clientInitFunction field of afxMagicSpellData, or afxEffectronData to
//  specify a function defined in this script that will be called from the spell,
//  or effectron's onAdd() method. This happens prior to performing substitutions 
//  on the client so you can define additional field parameters here. 
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

if (isFunction(RoF_clientInit))
  return;

function RoF_clientInit(%spell)
{
  if (!isObject(RoF_paths))
  {
    if (afxGetEngine() $= "T3D")
      afxLoadPathsFile("art/afx/effects/SpellPack2/RoF/paths/rof_ring_paths.txt", RoF_paths);
    else
      afxLoadPathsFile("~/data/effects/SpellPack2/RoF/paths/rof_ring_paths.txt", RoF_paths);
  }
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

function RoF_getFirePath(%idx) 
{
  return RoF_paths.paths[%idx];
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
