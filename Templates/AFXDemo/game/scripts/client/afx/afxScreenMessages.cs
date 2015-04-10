
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// This script implements client-side functionality for displaying on screen
// text messages. Typically, these calls are customized to fit the needs of a
// particular project.
//
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

$screenMessageOn = false;
$screenMessageTag = 0;

function clientCmdDisplayScreenMessage(%message, %clear)
{
  if (!$screenMessageOn)
  {
    ScreenMessageBox.visible = true;
    $screenMessageOn = true;
  }

  ScreenMessageBox.visible = !$GUI_HIDDEN;

  // MacOS requires a simpler markup to avoid a crash
  if ($platform $= "macos")
  {
    %markup_open = "<color:FF0000><shadow:1:1><shadowcolor:000000><just:center><font:Arial:25>";
    %markup_close = " ";
  }
  else
  {
    %markup_open = "<color:FF0000><shadow:1:1><shadowcolor:000000><just:center><font:Arial:25> <font:Arial:22>";
    %markup_close = "<font:Arial:25> ";
  }

  ScreenMessageText_C.alpha = ScreenMessageText_B.alpha;
  ScreenMessageText_C.setAlpha(ScreenMessageText_C.alpha);
  ScreenMessageText_C.tag = ScreenMessageText_B.tag;
  ScreenMessageText_C.text_copy = (%clear $= "clear") ? "" : ScreenMessageText_B.text_copy ;
  ScreenMessageText_C.setText(ScreenMessageText_C.text_copy);

  ScreenMessageText_B.alpha = ScreenMessageText_A.alpha * 0.8;
  ScreenMessageText_B.setAlpha(ScreenMessageText_B.alpha);
  ScreenMessageText_B.tag = ScreenMessageText_A.tag;
  ScreenMessageText_B.text_copy = (%clear $= "clear") ? "" : ScreenMessageText_A.text_copy;
  ScreenMessageText_B.setText(ScreenMessageText_B.text_copy);

  ScreenMessageText_A.alpha = 1.0;
  ScreenMessageText_A.setAlpha(1.0);
  ScreenMessageText_A.tag = $screenMessageTag++;
  ScreenMessageText_A.text_copy = %markup_open @ %message @ %markup_close;
  ScreenMessageText_A.setText(ScreenMessageText_A.text_copy);

  schedule(4000, 0, "fadeoutScreenMessage", ScreenMessageText_A.tag);
}

function clientCmdClearScreenMessage()
{
  schedule(0, 0, "fadeoutScreenMessage", ScreenMessageText_A.tag);
  schedule(0, 0, "fadeoutScreenMessage", ScreenMessageText_B.tag);
  schedule(0, 0, "fadeoutScreenMessage", ScreenMessageText_C.tag);
}

function fadeoutScreenMessage(%fade_tag)
{
  %fade_this = 0;
  if (%fade_tag == ScreenMessageText_A.tag)
    %fade_this = ScreenMessageText_A;
  else if (%fade_tag == ScreenMessageText_B.tag)
    %fade_this = ScreenMessageText_B;
  else if (%fade_tag == ScreenMessageText_C.tag)
    %fade_this = ScreenMessageText_C;

  if (!isObject(%fade_this))
    return;

  %alpha = %fade_this.alpha - 0.1;
  if (%alpha < 0)
    %alpha = 0;

  if (%alpha == 0)
  {
    %fade_this.alpha = 0;
    %fade_this.setAlpha(0);
    $screenMessageOn = false;
    %fade_this.clear_msg_cb = "";
    if (ScreenMessageText_A.alpha + ScreenMessageText_B.alpha + ScreenMessageText_C.alpha == 0)
      ScreenMessageBox.visible = false;
  }
  else
  {
    %fade_this.alpha = %alpha;
    %fade_this.setAlpha(%alpha);
    schedule(100, 0, "fadeoutScreenMessage", %fade_this.tag);
  }
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

