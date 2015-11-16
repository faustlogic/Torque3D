
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// OCCAM'S LASER (audio)
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
// SOUNDS (MAIN - afxModel Drone)

datablock SFXProfile(SF_DroneWail_Snd_CE)
{
  fileName = %mySpellDataPath @ "/SF_OL/sounds/SF_drone_wail6.ogg";
  description = SpellAudioCasting_AD;
  preload = true;
};
//
datablock afxEffectWrapperData(SF_DroneWail_Snd_EW : SF_Drone_Body_EW)
{
  effect = SF_DroneWail_Snd_CE;
  scaleFactor = 0.5;
};

datablock SFXProfile(SF_Tele_IN_Snd_CE)
{
  fileName = %mySpellDataPath @ "/SF_OL/sounds/SF_teleport_in_3b.ogg";
  description = SpellAudioCasting_AD;
  preload = true;
};
//
datablock afxEffectWrapperData(SF_Tele_IN_Snd_EW : SF_Tele_IN_Beam_EW)
{
  effect = SF_Tele_IN_Snd_CE;
  scaleFactor = 0.9;
};

datablock SFXProfile(SF_Tele_OUT_Snd_CE)
{
  fileName = %mySpellDataPath @ "/SF_OL/sounds/SF_teleport_out_3b.ogg";
  description = SpellAudioCasting_AD;
  preload = true;
};
//
datablock afxEffectWrapperData(SF_Tele_OUT_Snd_EW : SF_Tele_OUT_Beam_EW)
{
  effect = SF_Tele_OUT_Snd_CE;
  scaleFactor = 0.9;
};

datablock SFXProfile(SF_LaserBeam_Snd_CE)
{
  fileName = %mySpellDataPath @ "/SF_OL/sounds/SF_laserbeam_4.ogg";
  description = SpellAudioCasting_AD;
  preload = true;
};
//
datablock afxEffectWrapperData(SF_LaserBeam_Snd_EW)
{
  effect = SF_LaserBeam_Snd_CE;
  constraint = "#effect.DroneMooring";
  delay = %SCIFI_Satellite_delay+6.0;
  lifetime = 1.3507;
  scaleFactor = 1.0;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// SOUNDS (RELOADED - afxMachineGun)

datablock SFXProfile(SF_LaserBlastFastSnd_CE)
{
  fileName = %mySpellDataPath @ "/SF_OL/sounds/SF_laserBlast_fast_loud.ogg";
  description = SpellAudioCasting_AD;
  preload = true;
};
datablock afxEffectWrapperData(SF_LaserBlastSnd_1_EW)
{
  effect = SF_LaserBlastFastSnd_CE;
  constraint = "#ghost.DroneMooring_MG";

  delay       = %SF_LaserPulseFlash_1_delay;
  lifetime = 0.5018;
  fadeInTime = 0;
  fadeOutTime = 0;

  scaleFactor = 1.0;
};
datablock afxEffectWrapperData(SF_LaserBlastSnd_2_EW : SF_LaserBlastSnd_1_EW)
{
  delay       = %SF_LaserPulseFlash_2_delay;
};
datablock afxEffectWrapperData(SF_LaserBlastSnd_3_EW : SF_LaserBlastSnd_1_EW)
{
  delay       = %SF_LaserPulseFlash_3_delay;
};

datablock afxEffectWrapperData(SF_DroneWail_Snd_MG_EW : SF_Drone_Body_MG_EW)
{
  effect = SF_DroneWail_Snd_CE;
  scaleFactor = 0.5;
};
datablock afxEffectWrapperData(SF_Tele_IN_Snd_MG_EW : SF_Tele_IN_Beam_MG_EW)
{
  effect = SF_Tele_IN_Snd_CE;
  scaleFactor = 0.9;
};
datablock afxEffectWrapperData(SF_Tele_OUT_Snd_MG_EW : SF_Tele_OUT_Beam_MG_EW)
{
  effect = SF_Tele_OUT_Snd_CE;
  scaleFactor = 0.9;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// SOUNDS (REDUX - StaticShape Drone)

datablock afxEffectWrapperData(SF_DroneWail_Snd_S_EW : SF_Drone_Body_S_EW)
{
  effect = SF_DroneWail_Snd_CE;
  scaleFactor = 0.5;
  // sound effects won't play on the server...
  constraint = "#ghost.DroneMooring_S";
};
datablock afxEffectWrapperData(SF_Tele_IN_Snd_S_EW : SF_Tele_IN_Beam_S_EW)
{
  effect = SF_Tele_IN_Snd_CE;
  scaleFactor = 0.9;
};
datablock afxEffectWrapperData(SF_Tele_OUT_Snd_S_EW : SF_Tele_OUT_Beam_S_EW)
{
  effect = SF_Tele_OUT_Snd_CE;
  scaleFactor = 0.9;
};
datablock afxEffectWrapperData(SF_LaserBeam_Snd_S_EW : SF_LaserBeam_Snd_EW)
{
  constraint = "#ghost.DroneMooring_S";
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

function OL_main_add_Audio_FX(%spell_data)
{
  %spell_data.addCastingEffect(SF_DroneWail_Snd_EW);
  %spell_data.addCastingEffect(SF_Tele_IN_Snd_EW);
  %spell_data.addCastingEffect(SF_Tele_OUT_Snd_EW);
  %spell_data.addDeliveryEffect(SF_LaserBeam_Snd_EW);
}

function OL_reloaded_add_Audio_FX(%spell_data)
{
  %spell_data.addCastingEffect(SF_DroneWail_Snd_MG_EW);
  %spell_data.addCastingEffect(SF_Tele_IN_Snd_MG_EW);
  %spell_data.addCastingEffect(SF_Tele_OUT_Snd_MG_EW);
  %spell_data.addDeliveryEffect(SF_LaserBlastSnd_1_EW);
  %spell_data.addDeliveryEffect(SF_LaserBlastSnd_2_EW);
  %spell_data.addDeliveryEffect(SF_LaserBlastSnd_3_EW);
}

function OL_redux_add_Audio_FX(%spell_data)
{
  %spell_data.addCastingEffect(SF_DroneWail_Snd_S_EW);
  %spell_data.addCastingEffect(SF_Tele_IN_Snd_S_EW);
  %spell_data.addCastingEffect(SF_Tele_OUT_Snd_S_EW);
  %spell_data.addDeliveryEffect(SF_LaserBeam_Snd_S_EW);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
