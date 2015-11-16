
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// RING OF FIRE (lighting)
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

datablock afxT3DPointLightData(RoF_CastingZodeLight_CE)
{
  radius = 5;
  color = "1.0 0.8 0.3";
  brightness = 2.5;
  castShadows = false;
  localRenderViz = false;
};

datablock afxT3DPointLightData(RoF_FireballRingLight_CE)
{
  radius = 7;
  color = "1.0 0.1 0.0";
  brightness = 2.5;
  castShadows = false;
  localRenderViz = false;
};

datablock afxT3DPointLightData(RoF_FireBurstLight_CE)
{
  radius = 7;
  color = "1.0 0.1 0.0";
  brightness = 5;
  castShadows = false; // T3D Off (temporarily)
  localRenderViz = false;
};

// this offset defines the lights distance beneath the terrain
datablock afxXM_LocalOffsetData(RoF_CastingZodeLight_offset_XM)
{
  localOffset = "0 0 -4";
};
datablock afxEffectWrapperData(RoF_CastingLight_EW) // SWIRL
{
  effect = RoF_CastingZodeLight_CE;
  constraint = "RingAnchor";
  delay = 0;
  fadeInTime = 0.75;
  fadeOutTime = 1.0;
  lifetime = 0.75;
  xfmModifiers[0] = RoF_CastingZodeLight_offset_XM;
};

datablock afxEffectWrapperData(RoF_FireballRingLight_EW) // SWIRL
{
  effect = RoF_FireballRingLight_CE;
  constraint = "RingAnchor";

  delay = 0.5;
  lifetime = %RoF_FireRing_spread_delay;
  fadeInTime = %RoF_FireRing_spread_delay;
  fadeOutTime = 0.25; //0.5;
  xfmModifiers[0] = RoF_CastingZodeLight_offset_XM;
};  

datablock afxEffectWrapperData(RoF_FireBurstLight_EW) // SWIRL
{
  effect = RoF_FireBurstLight_CE;
  constraint = "RingAnchor";
  delay = 2.5-0.05;
  lifetime = 0.10;
  fadeInTime = 0.10;
  fadeOutTime = 0.75;
};  

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock afxT3DPointLightData(RoF_FireballRingBLight_CE)
{
  radius = 8;
  color = "1.0 0.1 0.0";
  brightness = 0.3;
  castShadows = false;
  localRenderViz = false;
};

datablock afxXM_OscillateData(RoF_FireballRingB_osc0_XM)
{    
  mask = $afxXfmMod::POS;
  min   = "0 0 1.5"; 
  max   = "0 0 -1.5"; 
  speed = "$$ getRandomF(4.0,20.0)";
};
datablock afxEffectWrapperData(RoF_RingFire_Light_Swirl_00_EW : RoF_RingFire_Flames_Swirl_00_EW)
{
  // effectEnabled = "$$ ## % 2"; // this sub will enable half the lights
  effect = RoF_FireballRingBLight_CE;
  xfmModifiers[0] = RoF_FireballRingB_osc0_XM;
};

$RoF_CastingLight = RoF_CastingLight_EW;
$RoF_FireballRingLight = RoF_FireballRingLight_EW;
$RoF_FireBurstLight = RoF_FireBurstLight_EW;
$RoF_RingFire_Light_Swirl_00 = RoF_RingFire_Light_Swirl_00_EW;

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
