
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX
//
// Overhead camera for Bron-Y-Orc Stomp (SUB-SCRIPT)
//
// Copyright (C) Faust Logic, Inc.
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

// CAMERA EFFECTRON

datablock afxXM_LocalOffsetData(BYOS_Cam1_Offset_XM)
{
  localOffset = "0 -6 12";
};
datablock afxXM_LocalOffsetData(BYOS_Cam1_Offset2_XM)
{
  localOffset = "0 -3 0";
  offsetPos2 = true;
};
datablock afxXM_AimData(BYOS_CamAim_XM)
{
  aimZOnly = false;
};

datablock afxCameraPuppetData(BYOS_CamPuppet_CE)
{
  cameraSpec = "camera";
  networking = $AFX::CLIENT_ONLY;
};

datablock afxEffectWrapperData(BYOS_OverheadCam1_EW)
{
  effect = BYOS_CamPuppet_CE;
  posConstraint2 = "camCOI";
  constraint = "camAnchor";
  xfmModifiers[0] = BYOS_Cam1_Offset_XM;
  xfmModifiers[1] = BYOS_Cam1_Offset2_XM;
  xfmModifiers[2] = BYOS_CamAim_XM;
  delay = 4;
  lifetime = 4.1;
};

datablock afxXM_LocalOffsetData(BYOS_Cam2_Offset_XM)
{
  localOffset = "0 2 -7";
  fadeInTime = 2;
};
datablock afxXM_LocalOffsetData(BYOS_Cam2_Offset2_XM)
{
  localOffset = "0 2 0";
  offsetPos2 = true;
  fadeInTime = 2;
};

datablock afxEffectWrapperData(BYOS_OverheadCam2_EW)
{
  effect = BYOS_CamPuppet_CE;
  posConstraint2 = "camCOI";
  constraint = "camAnchor";
  xfmModifiers[0] = BYOS_Cam1_Offset_XM;
  xfmModifiers[1] = BYOS_Cam1_Offset2_XM;
  xfmModifiers[2] = BYOS_Cam2_Offset_XM;
  xfmModifiers[3] = BYOS_Cam2_Offset2_XM;
  xfmModifiers[4] = BYOS_CamAim_XM;
  delay = 8;
  lifetime = 5;
};

datablock afxEffectronData(BYOS_CineCam_Effe)
{
  addEffect = BYOS_OverheadCam1_EW;
  addEffect = BYOS_OverheadCam2_EW;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
