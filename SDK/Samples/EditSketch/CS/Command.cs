﻿//
// (C) Copyright 2003-2023 by Autodesk, Inc.
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
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;


namespace Revit.SDK.Samples.EditSketch.CS
{
   /// <summary>
   /// Implements the Revit add-in interface IExternalCommand
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   public class Command : IExternalCommand
   {
      /// <summary>
      /// Implement this method as an external command for Revit.
      /// </summary>
      /// <param name="commandData">An object that is passed to the external application 
      /// which contains data related to the command, 
      /// such as the application object and active view.</param>
      /// <param name="message">A message that can be set by the external application 
      /// which will be displayed if a failure or cancellation is returned by 
      /// the external command.</param>
      /// <param name="elements">A set of elements to which the external application 
      /// can add elements that are to be highlighted in case of failure or cancellation.</param>
      /// <returns>Return the status of the external command. 
      /// A result of Succeeded means that the API external method functioned as expected. 
      /// Cancelled can be used to signify that the user cancelled the external operation 
      /// at some point. Failure should be returned if the application is unable to proceed with 
      /// the operation.</returns>
      public virtual Result Execute(ExternalCommandData commandData
         , ref string message, ElementSet elements)
      {

         //  Shape of example wall created below:
         //
         //    ───────
         //   /   ___  \
         //  /  /    \  \
         // │  /      \  │
         // │  \      /  │
         // │   \___ /   │
         // │            │
         // │            │
         // │            │
         // │            │
         // │            │
         // │            │
         // │            │
         // │            │
         // └────────────┘


         try
         {
            Document document = commandData.Application.ActiveUIDocument.Document;

            // Parameters of the example wall
            double height = 20;
            double width = 10;
            XYZ start = new XYZ(0, 0, 0);
            XYZ end = new XYZ(width, 0, 0);

            Wall wall = AddWall.execute(document, start, end, height, "Level 1");
            
            WallSketchEditor wallSketchEditor = new WallSketchEditor(document, wall);

            wallSketchEditor.ReplaceTopOfWallWithArc();

            XYZ circleCenter = new XYZ(width / 2, 0, height);
            double circleRadius = 3;
            wallSketchEditor.AddHoleToSketch(circleCenter, circleRadius);

            wallSketchEditor.AddLinearDimensionBetweenArcsInSketch();
            wallSketchEditor.AddAngularDimensionToWallBase();
            return Result.Succeeded;

         }
         catch (Exception ex)
         {
            message = ex.Message;
            return Result.Failed;
         }
      }
   }
}

