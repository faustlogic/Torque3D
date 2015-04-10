
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// ARCANE IN THE MEMBRANE (audio)
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
// SOUNDS

datablock SFXProfile(AitM_Conjure1_Snd_CE)
{
   fileName = %mySpellDataPath @ "/AitM/sounds/AITM_conjure_1.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(AitM_Conjure1_Snd_EW)
{
  effect = AitM_Conjure1_Snd_CE;
  constraint = "caster";
  delay = 0.0;
  lifetime = 6.017;
  scaleFactor = 0.9;
};

datablock SFXProfile(AitM_Conjure2_Snd_CE)
{
   fileName = %mySpellDataPath @ "/AitM/sounds/AITM_conjure_2.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(AitM_Conjure2_Snd_EW)
{
  effect = AitM_Conjure2_Snd_CE;
  constraint = "caster";
  delay = 3.0;
  lifetime = 3.545;
};

datablock SFXProfile(AitM_Face1_Snd_CE)
{
   fileName = %mySpellDataPath @ "/AitM/sounds/AITM_face_1.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(AitM_FaceA1_Snd_EW)
{
  effect = AitM_Face1_Snd_CE;
  constraint = "impactedObject";
  delay = 8.0;
  lifetime = 3.092;
  xfmModifiers[0] = AitM_TargetCrazyMaskA_spin_XM;
  xfmModifiers[1] = AitM_TargetCrazyMask_offset_XM;
  xfmModifiers[2] = AitM_TargetCrazyMask_spin_XM;
  xfmModifiers[3] = AitM_TargetCrazyMaskA_path_XM;
};
datablock afxEffectWrapperData(AitM_FaceB1_Snd_EW : AitM_FaceA1_Snd_EW)
{
  delay = 17.0 + 0.85;
  xfmModifiers[0] = AitM_TargetCrazyMaskB_spin_XM;
  xfmModifiers[3] = AitM_TargetCrazyMaskB_path_XM;
};

datablock SFXProfile(AitM_Face2_Snd_CE)
{
   fileName = %mySpellDataPath @ "/AitM/sounds/AITM_face_2.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(AitM_FaceA2_Snd_EW)
{
  effect = AitM_Face2_Snd_CE;
  constraint = "impactedObject";
  delay = 13.0;
  lifetime = 3.138;
  xfmModifiers[0] = AitM_TargetCrazyMaskA_spin_XM;
  xfmModifiers[1] = AitM_TargetCrazyMask_offset_XM;
  xfmModifiers[2] = AitM_TargetCrazyMask_spin_XM;
  xfmModifiers[3] = AitM_TargetCrazyMaskA_path_XM;
};
datablock afxEffectWrapperData(AitM_FaceB2_Snd_EW : AitM_FaceA2_Snd_EW)
{
  delay = 8.0 + 0.85;
  xfmModifiers[0] = AitM_TargetCrazyMaskB_spin_XM;
  xfmModifiers[3] = AitM_TargetCrazyMaskB_path_XM;
};


datablock SFXProfile(AitM_Face3_Snd_CE)
{
   fileName = %mySpellDataPath @ "/AitM/sounds/AITM_face_3.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(AitM_FaceA3_Snd_EW)
{
  effect = AitM_Face3_Snd_CE;
  constraint = "impactedObject";
  delay = 17.0;
  lifetime = 3.777;
  xfmModifiers[0] = AitM_TargetCrazyMaskA_spin_XM;
  xfmModifiers[1] = AitM_TargetCrazyMask_offset_XM;
  xfmModifiers[2] = AitM_TargetCrazyMask_spin_XM;
  xfmModifiers[3] = AitM_TargetCrazyMaskA_path_XM;
};
datablock afxEffectWrapperData(AitM_FaceB3_Snd_EW : AitM_FaceA3_Snd_EW)
{
  delay = 13.0 + 0.85;
  xfmModifiers[0] = AitM_TargetCrazyMaskB_spin_XM;
  xfmModifiers[3] = AitM_TargetCrazyMaskB_path_XM;
};

datablock SFXProfile(AitM_Impact1_Snd_CE)
{
   fileName = %mySpellDataPath @ "/AitM/sounds/AITM_impact_1.ogg";
   description = SpellAudioImpact_AD;
   preload = true;
};
datablock afxEffectWrapperData(AitM_Impact1_Snd_EW)
{
  effect = AitM_Impact1_Snd_CE;
  constraint = "impactedObject";
  delay = 0.5;
  lifetime = 5.188;
};

datablock SFXProfile(AitM_Pound_Snd_CE)
{
   fileName = %mySpellDataPath @ "/AitM/sounds/AITM_pound.ogg";
   description = SpellAudioImpact_AD;
   preload = true;
};
datablock afxEffectWrapperData(AitM_Pound_Snd_EW)
{
  effect = AitM_Pound_Snd_CE;
  constraint = "impactedObject";
  delay = 4.3;
  lifetime = 3.217;
};

datablock SFXProfile(AitM_Cyclone_Snd_CE)
{
   fileName = %mySpellDataPath @ "/AitM/sounds/AITM_cyclone.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(AitM_Cyclone_Snd_EW)
{
  effect = AitM_Cyclone_Snd_CE;
  constraint = "impactedObject";
  delay = 4.314;
  lifetime = 16.707;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

function AitM_add_Audio_FX(%spell_data)
{
  %spell_data.addCastingEffect(AitM_Conjure1_Snd_EW);
  %spell_data.addCastingEffect(AitM_Conjure2_Snd_EW);
  %spell_data.addImpactEffect(AitM_Impact1_Snd_EW);
  %spell_data.addImpactEffect(AitM_Pound_Snd_EW);
  %spell_data.addImpactEffect(AitM_Cyclone_Snd_EW);
  %spell_data.addImpactEffect(AitM_FaceA1_Snd_EW);
  %spell_data.addImpactEffect(AitM_FaceA2_Snd_EW);
  %spell_data.addImpactEffect(AitM_FaceA3_Snd_EW);
  %spell_data.addImpactEffect(AitM_FaceB1_Snd_EW);
  %spell_data.addImpactEffect(AitM_FaceB2_Snd_EW);
  %spell_data.addImpactEffect(AitM_FaceB3_Snd_EW);  
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
