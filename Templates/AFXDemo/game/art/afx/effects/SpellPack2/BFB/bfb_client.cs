//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// BOLT FROM THE BLUE (client script)
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

if (!isObject(BFB_skyBoltGlow_Data))
{
  new ScriptObject(BFB_skyBoltGlow_Data)
  {
    start[0]  = 8/30;
    start[1]  = 1/30;
    start[2]  = 7/30;
    start[3]  = 5/30;
    start[4]  = 3/30;
    start[5]  = 8/30;
    start[6]  = 10/30;
    start[7]  = 6/30;
    start[8]  = 23/30;
    start[9]  = 13/30;
    start[10] = 27/30;
    start[11] = 18/30;
    start[12] = 24/30;
    start[13] = 20/30;
    start[14] = 26/30;
    start[15] = 19/30;

    end[0]  = 14/30;
    end[1]  = 4/30;
    end[2]  = 12/30;
    end[3]  = 9/30;
    end[4]  = 7/30;
    end[5]  = 13/30;
    end[6]  = 16/30;
    end[7]  = 11/30;
    end[8]  = 26/30;
    end[9]  = 18/30;
    end[10] = 30/30;
    end[11] = 24/30;
    end[12] = 28/30;
    end[13] = 25/30;
    end[14] = 30/30;
    end[15] = 23/30;

    angle[0]  = 0;
    angle[1]  = 0;
    angle[2]  = 96.0;
    angle[3]  = 56.124;
    angle[4]  = 169.92;
    angle[5]  = -71.64;
    angle[6]  = 0;
    angle[7]  = 0;
    angle[8]  = 136.0;
    angle[9]  = 299.0;
    angle[10] = -36.0;
    angle[11] = -138.239;
    angle[12] = 0;
    angle[13] = 66;
    angle[14] = 96;
    angle[15] = -130;
  };
}             
                
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

if (isFunction(BFB_clientInit))
  return;

function BFB_clientInit(%spell)
{
  %spell._skyboltsGlow = BFB_skyBoltGlow_Data;

  if (!isObject(BFB_glowPaths))
  {
    if (afxGetEngine() $= "T3D")
      afxLoadPathsFile("art/afx/effects/SpellPack2/BFB/paths/bfb_paths.txt", BFB_glowPaths);
    else
      afxLoadPathsFile("~/data/effects/SpellPack2/BFB/paths/bfb_paths.txt", BFB_glowPaths);
  }
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

function BFB_getGlowPath(%idx) 
{
  return BFB_glowPaths.paths[%idx];
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
