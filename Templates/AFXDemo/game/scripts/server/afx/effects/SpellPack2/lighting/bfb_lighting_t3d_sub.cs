
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// BOLT FROM THE BLUE (lighting)
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

datablock afxT3DPointLightData(BFB_CastingLightningGlowLight_CE)
{
  radius = 50;
  color = ( 42/255) SPC (162/255) SPC (188/255);
  brightness = 1;
  castShadows = false;
  localRenderViz = false;
};

datablock afxXM_LocalOffsetData(BFB_CastingLightningGlowLight_offset_XM)
{  
  localOffset = "$$ \"0 0\" SPC getRandomF(10.0, 40.0)";
};

datablock afxEffectWrapperData(BFB_CastingLightningGlowLight_00_EW : BFB_CastingLightningGlow_00_EW)
{
  effectEnabled = true;
  effect = BFB_CastingLightningGlowLight_CE;
  xfmModifiers[0] = BFB_CastingLightningGlow_00_spin_XM;
  xfmModifiers[1] = BFB_CastingLightningGlowLight_offset_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

function BFB_add_group_Lighting_FX(%group_data)
{
  %group_data.addEffect(BFB_CastingLightningGlowLight_00_EW);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

