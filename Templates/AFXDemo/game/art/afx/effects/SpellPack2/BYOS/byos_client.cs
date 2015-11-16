
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// BRON-Y-ORC STOMP (client script)
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

if (isFunction(BYOS_clientInit))
  return;

function BYOS_clientInit(%spell)
{
  BYOS_pickStepParams(%spell, 0);
  BYOS_pickStepParams(%spell, 1);
  BYOS_pickLandParams(%spell);
}

function BYOS_pickStepParams(%spell, %side)
{
  // Pick Angle
  %spell._triggerAngle[%side] = getRandomF(0.0, 360.0);

  // Pick Scale
  %scale_roll = getRandomF(0.3, 1.0);
  if (%scale_roll > 0.8)
    %scale_mag = 2;
  else if (%scale_roll <= 0.5)
    %scale_mag = 0;
  else
    %scale_mag = 1;
  %spell._triggerScale[%side] = %scale_roll;
  %spell._triggerScaleMag[%side] = %scale_mag;

  // Pick Sound
  %sound_roll = getRandom(0, 100);
  if (%sound_roll < 18)
    %spell._triggerRandomSnd[%side] = %sound_roll;
  else if (%sound_roll > 18)
    %spell._triggerRandomSnd[%side] = -1;
  else // if (%sound_roll == 18)
    %spell._triggerRandomSnd[%side] = (getRandom(0, 1) == 1) ? 18 : -1;
}

function BYOS_pickLandParams(%spell)
{
  // Pick Angle
  %spell._triggerAngleLAND = getRandomF(0.0, 360.0);

  // Pick Scale
  %spell._triggerScaleLAND = getRandomF(0.5, 1.0);

  // Pick Sound
  %sound_roll = getRandom(0, 9);
  %spell._triggerRandomSndLAND = (%sound_roll < 6) ? %sound_roll : -1;
}


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
