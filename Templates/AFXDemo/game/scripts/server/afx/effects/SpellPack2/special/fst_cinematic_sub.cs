
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX
//
// Cinematic for Flaming Stick Trick (SUB-SCRIPT)
//
// Copyright (C) Faust Logic, Inc.
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

// CAMERA EFFECTRON

// These two offsets create a camera center-of-interest that transititions
// from the coi on the caster to a good coi for most of the spell.
//
datablock afxXM_WorldOffsetData(FST_Cam1_coi1_offset_XM)
{
  worldOffset = "0 0 2";
  fadeInTime = 4.0;
  fadeInEase = "0.2 0.8";
  offsetPos2 = true;
};
datablock afxXM_WorldOffsetData(FST_Cam1_coi2_offset_XM)
{
  worldOffset = "$$ %%._camCoiOffset";
  lifetime = 0;
  fadeOutTime = 4.0;
  fadeOutEase = "0.2 0.8";
  offsetPos2 = true;
};

// These two offsets create a camera position that transititions
// from the starting location of the camera to an orbit offset.
//
datablock afxXM_LocalOffsetData(FST_Cam1_offset1_XM)
{
  localOffset = "0 -3 3";
  localOffset = "$$ MatrixInverseMulVector(  %%._camAnchor,  VectorSub(ServerConnection.getCameraObject().getPosition(),%%._camAnchor)  )";
  lifetime = 0;
  fadeOutTime = 4.0;
  fadeOutEase = "0.2 0.8";
};
datablock afxXM_LocalOffsetData(FST_Cam1_offset2_XM)
{
  localOffset = "0 -6 5";
  fadeInTime = 4.0;
  fadeInEase = "0.2 0.8";
};

datablock afxXM_SpinData(FST_Cam1_spin1_XM)
{
  spinAxis = "0 0 1";
  spinAngle = 0;
  fadeInTime = 5.0;
  fadeInEase = "0.2 0.8";
  lifetime = 10.5;
  fadeOutTime = 4.5;
  fadeInEase = "0.2 0.8";
  spinRate = 48;
};
datablock afxXM_AimData(FST_Cam1_aim_XM)
{
  aimZOnly = false;
};

datablock afxCameraPuppetData(FST_Cam1_CE)
{
  cameraSpec = "camera";
  networking = $AFX::CLIENT_ONLY;
};
//
datablock afxEffectWrapperData(FST_Cam1_EW)
{
  effect = FST_Cam1_CE;
  posConstraint2 = "camAnchor";
  constraint = "camAnchor";
  xfmModifiers[0] = FST_Cam1_coi1_offset_XM;
  xfmModifiers[1] = FST_Cam1_coi2_offset_XM;

  xfmModifiers[2] = FST_Cam1_spin1_XM;
  xfmModifiers[3] = FST_Cam1_offset1_XM;
  xfmModifiers[4] = FST_Cam1_offset2_XM;
  xfmModifiers[5] = FST_Cam1_aim_XM;
  lifetime = 15;
};

datablock afxEffectronData(FST_CamShot_Effe)
{
  addEffect = FST_Cam1_EW;
  lifetime = 15;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
