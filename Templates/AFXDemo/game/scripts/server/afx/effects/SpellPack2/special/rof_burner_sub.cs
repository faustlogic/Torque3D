
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX
//
// RING OF FIRE: RING BURNER (SUB-SCRIPT)
//
//   This is a sub-script to the Ring of Fire spell. It uses an effectron to
//   implement a fire effect that can be attached to a player. It also deals
//   damage (direct and DoT).
//
// Copyright (C) Faust Logic, Inc.
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

datablock afxScriptEventData(RoF_CheckRingBarrierScript_CE)
{
  methodName = "CheckRingBarrier";
};
datablock afxEffectWrapperData(RoF_CheckRingBarrierScript_EW)
{
  effect = RoF_CheckRingBarrierScript_CE;
  constraint = "RingAnchor";
  lifetime = 0.1;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// RING MONITOR EFFECTRON
//
datablock afxEffectronData(RoF_RingMonitorEffectron)
{
  duration = 1.0;
  numLoops = "$$ mFloor(%%._ringDur-0.65)";
  addEffect = RoF_CheckRingBarrierScript_EW;
};

//~~~~~~~~~~~~~~~~~~~~//
// Main Function

function RoF_RingMonitorEffectron::CheckRingBarrier(%this, %effectron, %cons, %xfm, %data)
{
  // parse out the ring center
  %pos = getWords(%xfm, 0, 2);

  %ring_radius = %effectron._ringRadius;

  // collect all objects currently in the ring
  %n_inside = 0;
  InitContainerRadiusSearch(%pos, %ring_radius, $TypeMasks::PlayerObjectType | $TypeMasks::CorpseObjectType);
  while ((%in_obj = containerSearchNext()) != 0) 
  {
    %inside[%n_inside] = %in_obj;
    %n_inside++;
  }

  // place new objects in incoming[], remaining in same[]
  %n_incoming = 0;
  %n_same = 0;
  for (%i = 0; %i < %n_inside; %i++)
  {
    %match_idx = -1;
    for (%j = 0; %j < %effectron.n_inside; %j++)
    {
      if (%effectron.inside[%j] == %inside[%i])
      {
        %match_idx = %j;
        break;
      }
    }  
    if (%match_idx != -1)
    {
      %same[%n_same] = %inside[%i];
      %effectron.inside[%match_idx] = "";
      %n_same++;
    }
    else
    {
      %incoming[%n_incoming] = %inside[%i];
      %n_incoming++;
    }
  }

  // place unmatched objects in outgoing[]
  %n_outgoing = 0;
  for (%i = 0; %i < %effectron.n_inside; %i++)
  {
    if (%effectron.inside[%i] !$= "")
    {
      %outgoing[%n_outgoing] = %effectron.inside[%i];
      %n_outgoing++;
    }
  }

  // update effectron's list of ring occupants
  %effectron.n_inside = %n_inside;
  for (%i = 0; %i < %n_inside; %i++)
  {
    %effectron.inside[%i] = %inside[%i];
  }

  // at this point anything in incoming[] and outgoing[]
  // must have crossed the fire ring

  // burn anyone entering the ring
  for (%i = 0; %i < %n_incoming; %i++)
  {
    %burn_obj = %incoming[%i];
    if (%burn_obj.uif_fire $= "")
    {
      %burn_obj.uif_fire = startEffectron(UpInFlamesEffectron, %burn_obj, "victim");
      %burn_obj.uif_fire._dur = getRandomF(9,11);
      %burn_obj.uif_fire.uif_victim = %burn_obj;

      //UAISK+AFX Interop Changes: Start
      if ($UAISK_Is_Available && %burn_obj.isBot)
        confuseBot(%burn_obj, $Sim::Time + %burn_obj.uif_fire._dur + 0.4, %burn_obj.detDis);
      //UAISK+AFX Interop Changes: End
    }
  }

  // burn anyone leaving the ring
  for (%i = 0; %i < %n_outgoing; %i++)
  {
    %burn_obj = %outgoing[%i];
    if (%burn_obj.uif_fire $= "")
    {
      %burn_obj.uif_fire = startEffectron(UpInFlamesEffectron, %burn_obj, "victim");
      %burn_obj.uif_fire._dur = getRandomF(8,10);
      %burn_obj.uif_fire.uif_victim = %burn_obj;

      //UAISK+AFX Interop Changes: Start
      if ($UAISK_Is_Available && %burn_obj.isBot)
        confuseBot(%burn_obj, $Sim::Time + %burn_obj.uif_fire._dur + 0.4, %burn_obj.detDis);
      //UAISK+AFX Interop Changes: End
    }
  }
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
