
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// THE LETTER K (lighting)
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

%LK_K_Pulse_Light_intens = 2.0;
%LK_K_Pulse_Light_1_rgb = VectorScale("237 134 73", 1.0*%LK_K_Pulse_Light_intens/255);
%LK_K_Pulse_Light_2_rgb = VectorScale("237 134 73", 0.5*%LK_K_Pulse_Light_intens/255);
%LK_K_Pulse_Light_3_rgb = VectorScale("237 134 73", 0.25*%LK_K_Pulse_Light_intens/255);

datablock afxT3DPointLightData(LK_SymbolGlowPulseLight_1_CE)
{
  radius = 8;
  color = %LK_K_Pulse_Light_1_rgb;
  castShadows = false; // T3D Off (temporarily)
  localRenderViz = false;
};
datablock afxT3DPointLightData(LK_SymbolGlowPulseLight_2_CE : LK_SymbolGlowPulseLight_1_CE)
{
  radius = 6;
  color = %LK_K_Pulse_Light_2_rgb;
};
datablock afxT3DPointLightData(LK_SymbolGlowPulseLight_3_CE : LK_SymbolGlowPulseLight_1_CE)
{
  radius = 5;
  color = %LK_K_Pulse_Light_3_rgb;
};

datablock afxEffectWrapperData(LK_SymbolGlowPulseLight_1_EW : LK_Hot_K_Pulse_1_EW)
{
  effect = LK_SymbolGlowPulseLight_1_CE;
};

datablock afxEffectWrapperData(LK_SymbolGlowPulseLight_2_EW : LK_Hot_K_Pulse_2_EW)
{
  effect = LK_SymbolGlowPulseLight_2_CE;
};

datablock afxEffectWrapperData(LK_SymbolGlowPulseLight_3_EW : LK_Hot_K_Pulse_3_EW)
{
  effect = LK_SymbolGlowPulseLight_3_CE;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

function LK_add_Lighting_FX(%effect_data)
{
  %effect_data.addEffect(LK_SymbolGlowPulseLight_1_EW);
  %effect_data.addEffect(LK_SymbolGlowPulseLight_2_EW);
  %effect_data.addEffect(LK_SymbolGlowPulseLight_3_EW);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

