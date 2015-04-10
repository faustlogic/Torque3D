
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// CANTRIP AND FALL (audio)
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

datablock SFXProfile(CTaF_KickSnd_CE)
{
   fileName = %mySpellDataPath @ "/CTaF/sounds/CTAF_cast.wav";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(CTaF_KickSnd_EW)
{
  effect = CTaF_KickSnd_CE;
  constraint = "caster.Bip01 R Foot";
  delay = 0.75;
  lifetime = 0.4;
  scaleFactor = 0.2;
};

datablock SFXProfile(CTaF_GetupSnd_CE)
{
   fileName = %mySpellDataPath @ "/CTaF/sounds/CTAF_getup.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(CTaF_GetupSnd_EW)
{
  effect = CTaF_GetupSnd_CE;
  constraint = "impactedObject";
  delay = 3.6;
  lifetime = 1.154;
};

datablock SFXProfile(CTaF_RolloverSnd_CE)
{
   fileName = %mySpellDataPath @ "/CTaF/sounds/CTAF_rollover.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(CTaF_RolloverSnd_EW)
{
  effect = CTaF_RolloverSnd_CE;
  constraint = "impactedObject";
  delay = 2.25;
  lifetime = 0.841;
};

datablock SFXProfile(CTaF_TripSnd_CE)
{
   fileName = %mySpellDataPath @ "/CTaF/sounds/CTAF_trip.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(CTaF_TripSnd_EW)
{
  effect = CTaF_TripSnd_CE;
  constraint = "impactedObject";
  delay = 0.1;
  lifetime = 1.366;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

function CTaF_add_Audio_FX(%spell_data)
{
  %spell_data.addCastingEffect(CTaF_KickSnd_EW);
  %spell_data.addImpactEffect(CTaF_TripSnd_EW);
  %spell_data.addImpactEffect(CTaF_RolloverSnd_EW);
  %spell_data.addImpactEffect(CTaF_GetupSnd_EW);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
