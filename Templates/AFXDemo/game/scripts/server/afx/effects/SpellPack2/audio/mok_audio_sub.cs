
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// MARK OF KORK and SOUL MINER'S SLAUGHTER (audio)
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

datablock SFXProfile(SMS_Flicker_SND)
{
   fileName = %mySpellDataPath @ "/MoK/sounds/sms_flicker_loop.ogg";
   description = SpellAudioLoop_AD;
   preload = true;
};
datablock SFXProfile(SMS_Boil_SND)
{
   fileName = %mySpellDataPath @ "/MoK/sounds/sms_boil_D15.ogg";
   description = SpellAudioCasting_loud_AD;
   preload = true;
};
datablock SFXProfile(SMS_FryBoil_SND : SMS_Boil_SND)
{
   fileName = %mySpellDataPath @ "/MoK/sounds/sms_fry_boil_D10.ogg";
};
datablock SFXProfile(SMS_SteamBoil_SND : SMS_Boil_SND)
{
   fileName = %mySpellDataPath @ "/MoK/sounds/sms_steam_boil_D19.ogg";
};
datablock SFXProfile(SMS_GlowFX_SND : SMS_Boil_SND)
{
   fileName = %mySpellDataPath @ "/MoK/sounds/sms_glow_fx_D4.ogg";
};
datablock SFXProfile(SMS_HandFX_SND : SMS_Boil_SND)
{
   fileName = %mySpellDataPath @ "/MoK/sounds/sms_hand_fx_D5.ogg";
};
datablock SFXProfile(SMS_GroundRumble_SND : SMS_Boil_SND)
{
   fileName = %mySpellDataPath @ "/MoK/sounds/sms_ground_rumble_D5.ogg";
};
datablock SFXProfile(SMS_HandRumble_SND : SMS_Boil_SND)
{
   fileName = %mySpellDataPath @ "/MoK/sounds/sms_hand_rumble_D5.ogg";
};
datablock SFXProfile(SMS_WispLoop_SND)
{
   fileName = %mySpellDataPath @ "/MoK/sounds/sms_wisp_loop.ogg";
   description = SpellAudioLoop_AD;
   preload = true;
};

datablock SFXProfile(MoK_FireOnGround_SND)
{
   fileName = %mySpellDataPath @ "/MoK/sounds/mok_fire_on_ground_D21.ogg";
   description = SpellAudioCasting_loud_AD;
   preload = true;
};
datablock SFXProfile(MoK_FireSizzle_SND : MoK_FireOnGround_SND)
{
   fileName = %mySpellDataPath @ "/MoK/sounds/mok_fire_sizzle_D13.ogg";
};
datablock SFXProfile(MoK_GroundRumble_SND : MoK_FireOnGround_SND)
{
   fileName = %mySpellDataPath @ "/MoK/sounds/mok_ground_rumble_D7.ogg";
};
datablock SFXProfile(MoK_GroundRumbleHiss_SND : MoK_FireOnGround_SND)
{
   fileName = %mySpellDataPath @ "/MoK/sounds/mok_ground_rumble_hiss_D23.ogg";
};
datablock SFXProfile(MoK_SparkExplosion_SND : MoK_FireOnGround_SND)
{
   fileName = %mySpellDataPath @ "/MoK/sounds/mok_spark_explosion_D7.ogg";
};
datablock SFXProfile(MoK_WeldingSparks_L1_SND : MoK_FireOnGround_SND)
{
   fileName = %mySpellDataPath @ "/MoK/sounds/mok_welding_sparks_L1_D7.ogg";
};
datablock SFXProfile(MoK_WeldingSparks_L2_SND : MoK_FireOnGround_SND)
{
   fileName = %mySpellDataPath @ "/MoK/sounds/mok_welding_sparks_L2_D7.ogg";
};

datablock SFXProfile(MoK_IgniteSpark_SND)
{
   fileName = %mySpellDataPath @ "/MoK/sounds/mok_ignite_spark.ogg";
   description = SpellAudioCasting_loud_AD;
   preload = true;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// SOUNDS

datablock afxEffectWrapperData(MoK_IgniteSpark_SND_EW)
{
  effect = MoK_IgniteSpark_SND;
  posConstraint = "caster.Bip01 L Hand";
  delay = 40/30;
  lifetime = 0.65;
};

datablock afxEffectWrapperData(SMS_Flicker_SND_EW)
{
  effect = SMS_Flicker_SND;
  delay = 40/30;
  lifetime = 2.0;
  fadeOutTime = 3.0;
  posConstraint = "caster.Bip01 L Hand";
};
datablock afxEffectWrapperData(SMS_Boil_SND_EW)
{
  effect = SMS_Boil_SND;
  delay = 3.2;
  lifetime = 15;
  //posConstraint = "#effect.Caster_Mooring";
  posConstraint = "caster";
};
datablock afxEffectWrapperData(SMS_FryBoil_SND_EW : SMS_Boil_SND_EW)
{
  effect = SMS_FryBoil_SND;
  lifetime = 10;
};
datablock afxEffectWrapperData(SMS_SteamBoil_SND_EW : SMS_Boil_SND_EW)
{
  effect = SMS_SteamBoil_SND;
  lifetime = 19;
  scaleFactor = 0.15;
};
datablock afxEffectWrapperData(SMS_GlowFX_SND_EW : SMS_Boil_SND_EW)
{
  effect = SMS_GlowFX_SND;
  lifetime = 4;
};
datablock afxEffectWrapperData(SMS_HandFX_SND_EW : SMS_Boil_SND_EW)
{
  effect = SMS_HandFX_SND;
  lifetime = 5;
};
datablock afxEffectWrapperData(SMS_GroundRumble_SND_EW : SMS_Boil_SND_EW)
{
  effect = SMS_GroundRumble_SND;
  lifetime = 5;
};
datablock afxEffectWrapperData(SMS_HandRumble_SND_EW : SMS_Boil_SND_EW)
{
  effect = SMS_HandRumble_SND;
  lifetime = 5;
};

datablock afxEffectGroupData(SMS_Sounds_EG)
{
  groupEnabled = "$$ %%._sms == true";

  addEffect = MoK_IgniteSpark_SND_EW;
  addEffect = SMS_Flicker_SND_EW;
  addEffect = SMS_Boil_SND_EW;
  addEffect = SMS_FryBoil_SND_EW;
  addEffect = SMS_SteamBoil_SND_EW;
  addEffect = SMS_GlowFX_SND_EW;
  addEffect = SMS_HandFX_SND_EW;
  addEffect = SMS_GroundRumble_SND_EW;
  addEffect = SMS_HandRumble_SND_EW;
};

//~~~~~~~~~~~~~~~~~~~~//

datablock afxEffectWrapperData(MoK_FireOnGround_SND_EW)
{
  effect = MoK_FireOnGround_SND;
  delay = 1;
  lifetime = 21;
  //posConstraint = "#effect.Caster_Mooring";
  posConstraint = "caster";
};
datablock afxEffectWrapperData(MoK_FireSizzle_SND_EW : MoK_FireOnGround_SND_EW)
{
  effect = MoK_FireSizzle_SND;
  lifetime = 13;
};
datablock afxEffectWrapperData(MoK_GroundRumble_SND_EW : MoK_FireOnGround_SND_EW)
{
  effect = MoK_GroundRumble_SND;
  lifetime = 7;
};
datablock afxEffectWrapperData(MoK_GroundRumbleHiss_SND_EW : MoK_FireOnGround_SND_EW)
{
  effect = MoK_GroundRumbleHiss_SND;
  lifetime = 23;
};
datablock afxEffectWrapperData(MoK_SparkExplosion_SND_EW : MoK_FireOnGround_SND_EW)
{
  effect = MoK_SparkExplosion_SND;
  lifetime = 7;
};
datablock afxEffectWrapperData(MoK_WeldingSparks_L1_SND_EW : MoK_FireOnGround_SND_EW)
{
  effect = MoK_WeldingSparks_L1_SND;
  lifetime = 7;
};
datablock afxEffectWrapperData(MoK_WeldingSparks_L2_SND_EW : MoK_FireOnGround_SND_EW)
{
  effect = MoK_WeldingSparks_L2_SND;
  lifetime = 7;
};

datablock afxEffectGroupData(MoK_Sounds_EG)
{
  groupEnabled = "$$ %%._sms != true";

  addEffect = MoK_IgniteSpark_SND_EW;
  addEffect = MoK_FireOnGround_SND_EW;
  addEffect = MoK_FireSizzle_SND_EW;
  addEffect = MoK_GroundRumble_SND_EW;
  addEffect = MoK_GroundRumbleHiss_SND_EW;
  addEffect = MoK_SparkExplosion_SND_EW;
  addEffect = MoK_WeldingSparks_L1_SND_EW;
  addEffect = MoK_WeldingSparks_L2_SND_EW;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

function MoK_add_Audio_FX(%spell_data)
{
  %spell_data.addCastingEffect(SMS_Sounds_EG);
  %spell_data.addCastingEffect(MoK_Sounds_EG);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
