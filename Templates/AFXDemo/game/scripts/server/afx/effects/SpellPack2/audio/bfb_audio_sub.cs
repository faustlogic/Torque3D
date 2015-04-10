
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// BOLT FROM THE BLUE (audio)
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

datablock SFXProfile(BFB_Lightning_SND)
{
   fileName = %mySpellDataPath @ "/BFB/sounds/bfb_lightning.ogg";
   description = SpellAudioImpact_AD;
   preload = true;
};
datablock afxEffectWrapperData(BFB_Lightning_SND_EW)
{
  effect = BFB_Lightning_SND;
  posConstraint = "anchor";
  delay = 0.0;
  lifetime = 3;
};

datablock SFXProfile(BFB_Electricity_SND : BFB_Lightning_SND)
{
   fileName = %mySpellDataPath @ "/BFB/sounds/bfb_electricity.ogg";
};
datablock afxEffectWrapperData(BFB_Electricity_SND_EW : BFB_Lightning_SND_EW)
{
  effect = BFB_Electricity_SND;
  scaleFactor = 0.5;
  posConstraint = "target";
};

datablock SFXProfile(BFB_Roar_SND : BFB_Lightning_SND)
{
   fileName = %mySpellDataPath @ "/BFB/sounds/bfb_roar.ogg";
};
datablock afxEffectWrapperData(BFB_Roar_SND_EW : BFB_Lightning_SND_EW)
{
  effect = BFB_Roar_SND;
  posConstraint = "target";
};

datablock SFXProfile(BFB_Explosion_SND : BFB_Lightning_SND)
{
   fileName = %mySpellDataPath @ "/BFB/sounds/bfb_explosion.ogg";
};
datablock afxEffectWrapperData(BFB_Explosion_SND_EW : BFB_Lightning_SND_EW)
{
  effect = BFB_Explosion_SND;
  posConstraint = "target";
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

function BFB_add_Audio_FX(%spell_data)
{
  %spell_data.addImpactEffect(BFB_Lightning_SND_EW);
  %spell_data.addImpactEffect(BFB_Electricity_SND_EW);
  %spell_data.addImpactEffect(BFB_Roar_SND_EW);
  %spell_data.addImpactEffect(BFB_Explosion_SND_EW);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
