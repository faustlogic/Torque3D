
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// BRON-Y-ORC STOMP (lighting)
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

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// STOMP LIGHT

datablock afxT3DPointLightData(BYOS_ExplosionLight_CE)
{
  radius = "$$ 5.0 * %%._triggerScale[##]";
  color = "1 1 1 1";
  brightness = 3.0;
  castShadows = false;
  localRenderViz = false;
};

datablock afxEffectWrapperData(BYOS_ExplosionLight_LF_EW)
{
  effect = BYOS_ExplosionLight_CE;
  posConstraint = "target.Bip01 L Foot";
  lifetime    = 0.05;
  delay       = 0;
  fadeInTime  = 0.05;
  fadeOutTime = 0.05;
  xfmModifiers[0] = BYOS_StepOffset_LF_XM;
};
datablock afxEffectWrapperData(BYOS_ExplosionLight_RT_EW : BYOS_ExplosionLight_LF_EW)
{
  posConstraint = "target.Bip01 R Foot";
  xfmModifiers[0] = BYOS_StepOffset_RT_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// JUMP LANDING LIGHTING

datablock afxT3DPointLightData(BYOS_ExplosionLight_LAND_CE : BYOS_ExplosionLight_CE)
{
  radius = "$$ 10.0 * %%._triggerScaleLAND";
};

datablock afxEffectWrapperData(BYOS_ExplosionLight_LAND_EW)
{
  effect = BYOS_ExplosionLight_LAND_CE;
  posConstraint = target;
  lifetime    = 0.05;
  delay       = 0;
  fadeInTime  = 0.05;
  fadeOutTime = 0.05;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

function BYOS_add_footstep_Lighting_FX(%phrase_lf, %phrase_rt)
{
  %phrase_rt.addEffect(BYOS_ExplosionLight_RT_EW);
  %phrase_lf.addEffect(BYOS_ExplosionLight_LF_EW);
}

function BYOS_add_landing_Lighting_FX(%phrase_landing)
{
  %phrase_landing.addEffect(BYOS_ExplosionLight_LAND_EW);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

