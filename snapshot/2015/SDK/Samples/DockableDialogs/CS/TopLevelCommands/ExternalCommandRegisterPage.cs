﻿//
// (C) Copyright 2003-2014 by Autodesk, Inc.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
//

using System;
using System.Collections;
using System.Collections.Generic;
using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.DockableDialogs.CS
{

   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   public class ExternalCommandRegisterPage : IExternalCommand, IExternalCommandAvailability
   {
      public virtual Result Execute(ExternalCommandData commandData
          , ref string message, ElementSet elements)
      {
         ThisApplication.thisApp.GetDockableAPIUtility().Initialize(commandData.Application);
        ThisApplication.thisApp.CreateWindow();

         DockingSetupDialog dlg = new DockingSetupDialog();
         Nullable<bool> dlgResult = dlg.ShowDialog();
         if (dlgResult == false)
            return Result.Succeeded;
       
         ThisApplication.thisApp.GetMainWindow().SetInitialDockingParameters(dlg.FloatLeft, dlg.FloatRight, dlg.FloatTop, dlg.FloatBottom, dlg.DockPosition, dlg.TargetGuid);
         try
         {
           ThisApplication.thisApp.RegisterDockableWindow(commandData.Application, dlg.MainPageGuid);
         }
         catch (Exception ex)
         {
            TaskDialog.Show(Globals.ApplicationName, ex.Message);
         }
         return Result.Succeeded;
      }

      /// <summary>
      /// Onlys show the dialog when a document is open, as Dockable dialogs are only available
      /// when a document is open.
      /// </summary>
      public bool IsCommandAvailable(UIApplication applicationData, CategorySet selectedCategories)
      {
         if (applicationData.ActiveUIDocument == null)
            return true;
         else
            return false;
      }
   }
}
