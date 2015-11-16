
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// GREEN LEGS AND SCRAM (lighting)
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

datablock afxXM_SpinData(GLaS_LightSpin_pos_XM)
{
    spinAxis = "1 0 0";
    spinAngle = 0;
    spinRate = 500;
};
datablock afxXM_SpinData(GLaS_LightSpin_neg_XM : GLaS_LightSpin_pos_XM)
{
    spinRate = -500;
};

datablock afxXM_LocalOffsetData(GLaS_LightOffset_Zneg_XM)
{
  localOffset = "0 0 -0.35";
};

datablock afxXM_LocalOffsetData(GLaS_LightOffset_Zpos_XM)
{
  localOffset = "0 0 0.35";
};

datablock afxT3DPointLightData(GLaS_GreenLight_CE)
{
  radius = 1;
  color = "0 1 0";
  brightness = 2;
  castShadows = false;
  localRenderViz = false;
};

datablock afxEffectWrapperData(GLaS_GreenLight_EW)
{
  effect = GLaS_GreenLight_CE;
  constraint = "target";
  delay = 0;
  lifetime = 5.0;
  fadeInTime = 1.0;
  fadeOutTime = 0.5;
};

datablock afxEffectWrapperData(GLaS_GreenLight1_RT_EW)
{
  effect = GLaS_GreenLight_CE;
  constraint = "target.Bip01 R Foot";
  delay = 0;
  lifetime = 5.0;
  fadeInTime = 1.0;
  fadeOutTime = 0.5; 
  xfmModifiers[0] = GLaS_LightSpin_pos_XM;
  xfmModifiers[1] = GLaS_LightOffset_Zpos_XM;
};
datablock afxEffectWrapperData(GLaS_GreenLight2_RT_EW : GLaS_GreenLight1_RT_EW)
{
  xfmModifiers[0] = GLaS_LightSpin_pos_XM;
  xfmModifiers[1] = GLaS_LightOffset_Zneg_XM;
};

datablock afxEffectWrapperData(GLaS_GreenLight1_LF_EW : GLaS_GreenLight2_RT_EW)
{
  constraint = "target.Bip01 L Foot";
  xfmModifiers[0] = GLaS_LightSpin_neg_XM;
};

datablock afxEffectWrapperData(GLaS_GreenLight2_LF_EW : GLaS_GreenLight1_RT_EW)
{
  constraint = "target.Bip01 L Foot";
  xfmModifiers[0] = GLaS_LightSpin_neg_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

function GLaS_add_Lighting_FX(%spell_data)
{
  %spell_data.addCastingEffect(GLaS_GreenLight1_RT_EW);
  %spell_data.addCastingEffect(GLaS_GreenLight1_LF_EW);
  %spell_data.addCastingEffect(GLaS_GreenLight2_RT_EW);
  %spell_data.addCastingEffect(GLaS_GreenLight2_LF_EW);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

