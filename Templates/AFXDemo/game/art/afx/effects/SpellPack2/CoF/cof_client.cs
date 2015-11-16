
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// CLUSTERS OF FIRE (client script)
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
   
if (isFunction(CoF_clientInit))
  return;

function CoF_clientInit(%effectron)
{
  %delay_opts[0] = 0.7;
  %delay_opts[1] = 0.9;
  %delay_opts[2] = 1.2;

  %n_fires = %effectron._n;
  for (%i = 0; %i < %n_fires; %i++)
  {
    %effectron.c_fireDelay[%i] = %delay_opts[getRandom(0,2)];
    %effectron.c_fireLifetime[%i] = mFloatLength(getRandomF(6.0, 15.0),1);
  }
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
