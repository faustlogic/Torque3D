//-----------------------------------------------------------------------------
// Copyright (c) 2012 GarageGames, LLC
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
//-----------------------------------------------------------------------------

// ----------------------------------------------------------------------------
// This is the default save location for any GuiProfiles created in the
// Gui Editor
// ----------------------------------------------------------------------------

// AFX DEMO MOD <<

// A bigger font size is used for flying damage text and overhead player labels.
singleton GuiControlProfile(AFX_FlyingDamageText_Profile : GuiDefaultProfile)
{
  fontSize = 24;
  category = "AFX";
};

singleton GuiControlProfile(AFX_HUDInfoBoxBackdrop_Profile)
{
   opaque = true;
   fillColor = "0 0 0 172";
   border = false;
   category = "AFX";
};

singleton GuiControlProfile(AFX_Text21Firebrick_Profile : GuiTextProfile)
{
   fontSize = 21;
   fontColor = "177 31 36";
   category = "AFX";
};

singleton GuiControlProfile(AFX_MenuButton14_Profile : GuiMenuButtonProfile)
{
   fontSize = 14;
   fontType = "Arial";
   category = "AFX";
};

// AFX DEMO MOD >>