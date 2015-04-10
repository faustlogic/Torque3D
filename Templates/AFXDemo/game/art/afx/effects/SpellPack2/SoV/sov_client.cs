
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// SHARDS OF VESUVIUS (client script)
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

if (isFunction(SoV_pickRockModel))
  return;

function SoV_clientInit(%spell)
{
}

function SoV_pickRockModel()
{
  %roll = getRandom(1,100);
  if (%roll > 47) return "sov_rockC.dts";
  if (%roll > 20) return "sov_rockB.dts";
  return "sov_rockA.dts";
}

function SoV_pickRockDelay()
{
  %roll = getRandom(1,100);
  if (%roll > 66) return 0.6;
  if (%roll > 33) return 0.8;
  return 1.5; 
}

function SoV_calcCometLifetime(%spell,%idx)
{
  %speed = %spell._cometSpeed[%idx];
  %ang = mDegToRad(%spell._cometAng[%idx]);
  return (2.0*%speed*mSin(%ang))/9.81; 
}

function SoV_calcCometPath(%spell,%idx)
{
  %speed = %spell._cometSpeed[%idx];
  %ang = mDegToRad(%spell._cometAng[%idx]);
  %n_pts = 7;
  %life = SoV_calcCometLifetime(%spell,%idx);
  %yc = %speed*mCos(%ang);
  %zc = %speed*mSin(%ang);
  %t = 0.0;
  %tinc = %life/(%n_pts-1);
  %points = "";
  for (%i = 0; %i < %n_pts; %i++)
  {
    %y = %yc*%t;
    %z = %zc*%t - 0.5*9.81*%t*%t;
    %points = %points SPC 0 SPC %y SPC %z;
    %t += %tinc;
  }
  return %points;
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
