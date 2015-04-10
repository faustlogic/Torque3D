//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
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

// Load dts shapes and merge animations
exec("art/shapes/actors/OrcWarrior/OrcWarrior.cs");

datablock PlayerData(OrcWarriorData : DefaultPlayerData)
{
   className = PlayerDataAFX;
   shapeFile = "art/shapes/actors/OrcWarrior/OrcWarrior.dts";
   mass = 90;
   repairRate = 0.05;
   maxEnergy =  0;  
   rechargeRate = 0;
   boundingBox = "1.2 1.2 2.3";
   runSurfaceAngle  = 70;
   recoverDelay = 9;
   recoverRunForceScale = 1.2;
   landSequenceTime = 0;
   mass = 90;
   drag = 0.3;
   maxForwardSpeed = 14;
   maxBackwardSpeed = 13;
   maxSideSpeed = 13;
   minLookAngle = "-1.4";
   maxLookAngle = "1.4";
   allowImageStateAnimation = false;
};

datablock PlayerData(NonPlayerData : OrcWarriorData)
{
   shootingDelay = 2000;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
