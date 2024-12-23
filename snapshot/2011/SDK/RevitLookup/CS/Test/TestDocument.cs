
//
// Copyright 2003-2010 by Autodesk, Inc.
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
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC. 
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to 
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
//

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using Revit = Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;


namespace RevitLookup.Test
{

   class TestDocument : RevitLookupTestFuncs
   {

      public
      TestDocument(Autodesk.Revit.UI.UIApplication app)
         : base(app)
      {
         m_testFuncs.Add(new RevitLookupTestFuncInfo("Delete SelSet", "Delete the current SelSet", typeof(Document), new RevitLookupTestFuncInfo.TestFunc(Delete), RevitLookupTestFuncInfo.TestType.Modify));
         m_testFuncs.Add(new RevitLookupTestFuncInfo("Move SelSet --> (10', 10', 0')", "Move the current SelSet", typeof(Document), new RevitLookupTestFuncInfo.TestFunc(Move), RevitLookupTestFuncInfo.TestType.Modify));
         m_testFuncs.Add(new RevitLookupTestFuncInfo("Rotate SelSet by 45 degrees", "Rotate the current SelSet", typeof(Document), new RevitLookupTestFuncInfo.TestFunc(Rotate), RevitLookupTestFuncInfo.TestType.Modify));
         m_testFuncs.Add(new RevitLookupTestFuncInfo("Load Family", "Load a .rfa file", typeof(Document), new RevitLookupTestFuncInfo.TestFunc(LoadFamily), RevitLookupTestFuncInfo.TestType.Modify));
         m_testFuncs.Add(new RevitLookupTestFuncInfo("Linear Array SelSet (Number = 4)", "Linearly array the current SelSet", typeof(Document), new RevitLookupTestFuncInfo.TestFunc(LinearArray), RevitLookupTestFuncInfo.TestType.Modify));
         m_testFuncs.Add(new RevitLookupTestFuncInfo("Radial Array SelSet by 30 degrees (Number = 4)", "Radially array the current SelSet", typeof(Document), new RevitLookupTestFuncInfo.TestFunc(RadialArray), RevitLookupTestFuncInfo.TestType.Modify));
         m_testFuncs.Add(new RevitLookupTestFuncInfo("Linear Array SelSet without associate", "Linearly array the current SelSet without associate", typeof(Document), new RevitLookupTestFuncInfo.TestFunc(ArrayWithoutAssociate), RevitLookupTestFuncInfo.TestType.Modify));
         //m_testFuncs.Add(new RevitLookupTestFuncInfo("Mirror", "Mirror current SelSet", typeof(Revit.Document), new RevitLookupTestFuncInfo.TestFunc(Mirror), RevitLookupTestFuncInfo.TestType.Create));
         m_testFuncs.Add(new RevitLookupTestFuncInfo("Boundary Lines", "Draw lines to sketch the Room Boundary", typeof(Document), new RevitLookupTestFuncInfo.TestFunc(SketchBoundary), RevitLookupTestFuncInfo.TestType.Create));
         m_testFuncs.Add(new RevitLookupTestFuncInfo("Room Area", "Insert Area Value textnotes for all available rooms", typeof(Document), new RevitLookupTestFuncInfo.TestFunc(RoomArea), RevitLookupTestFuncInfo.TestType.Create));
         m_testFuncs.Add(new RevitLookupTestFuncInfo("Filter Element types", "Filter for selected element types", typeof(Document), new RevitLookupTestFuncInfo.TestFunc(FilterElementTypes), RevitLookupTestFuncInfo.TestType.Query));
         m_testFuncs.Add(new RevitLookupTestFuncInfo("Add family types and parameters", "Use family manager for adddition of types/params", typeof(Document), new RevitLookupTestFuncInfo.TestFunc(AddFamilyParameterAndType), RevitLookupTestFuncInfo.TestType.Create));
      }

      public void
      Delete()
      {
         m_revitApp.ActiveUIDocument.Document.Delete(m_revitApp.ActiveUIDocument.Selection.Elements);
      }

      public void
      Move()
      {
         XYZ vec = new XYZ(10.0, 10.0, 0.0);
         m_revitApp.ActiveUIDocument.Document.Move(m_revitApp.ActiveUIDocument.Selection.Elements, vec);
      }

      public void
      Rotate()
      {
         Line zAxis = Line.get_Unbound(GeomUtils.kOrigin, GeomUtils.kZAxis);

         m_revitApp.ActiveUIDocument.Document.Rotate(m_revitApp.ActiveUIDocument.Selection.Elements, zAxis, GeomUtils.kRad45);
      }

      public void
      LoadFamily()
      {
         Utils.UserInput.LoadFamily(null, m_revitApp.ActiveUIDocument.Document);
      }

      public void
      LinearArray()
      {
         if (!m_revitApp.ActiveUIDocument.Selection.Elements.IsEmpty)
         {
            m_revitApp.ActiveUIDocument.Document.Array(m_revitApp.ActiveUIDocument.Selection.Elements, 4, false, GeomUtils.kYAxis);
         }
      }

      public void
      RadialArray()
      {
         if (!m_revitApp.ActiveUIDocument.Selection.Elements.IsEmpty)
         {
            m_revitApp.ActiveUIDocument.Document.Array(m_revitApp.ActiveUIDocument.Selection.Elements, 4, true, 30.0);
         }
      }

      public void
      ArrayWithoutAssociate()
      {
         if (!m_revitApp.ActiveUIDocument.Selection.Elements.IsEmpty)
         {
            m_revitApp.ActiveUIDocument.Document.ArrayWithoutAssociate(m_revitApp.ActiveUIDocument.Selection.Elements, 4, false, GeomUtils.kYAxis);
         }
      }

      public void
      Mirror()
      {   // TBD: 

         //XYZ startPt1 = new XYZ(0.0, 0.0, 0.0);
         //XYZ endPt1 = new XYZ(56.5, 0.0, 0.0);

         //Line line1 = m_revitApp.Create.NewLine(ref startPt1, ref endPt1, true);

         ////// Note: There seems to be an issue with the Reference returned by curves. It always remains null.
         ////// arj 1/12/07           
         //m_revitApp.ActiveUIDocument.Document.Mirror(m_revitApp.ActiveUIDocument.Selection.Elements, line1.Reference);
      }


      /// <summary>
      /// Draw lines to sketch the boundaries of available rooms in the Active Document.
      /// </summary>
      public void
      SketchBoundary()
      {
         Revit.Creation.Document doc = m_revitApp.ActiveUIDocument.Document.Create;
         SketchPlane sketchPlane = Utils.Geometry.GetWorldPlane(m_revitApp);

         RevitLookup.Test.Forms.Levels lev = new RevitLookup.Test.Forms.Levels(m_revitApp);
         if (lev.ShowDialog() != DialogResult.OK)
            return;

         Level curLevel = lev.LevelSelected;
         if (curLevel == null)
         {
            MessageBox.Show("No Level was selected.");
            return;
         }

         // Get the plan topology of the active doc first
         PlanTopology planTopo = m_revitApp.ActiveUIDocument.Document.get_PlanTopology(curLevel);
         ElementSet roomSet = planTopo.Rooms;

         if (roomSet.Size > 0)
         {
            ElementSetIterator setIter = roomSet.ForwardIterator();
            while (setIter.MoveNext())
            {
               Autodesk.Revit.DB.Architecture.Room room = setIter.Current as Autodesk.Revit.DB.Architecture.Room;

               if (null != room)
               {
                  Revit.DB.Architecture.BoundarySegmentArrayArray boundSegArrayArray = room.Boundary;

                  Revit.DB.Architecture.BoundarySegmentArrayArrayIterator iter = boundSegArrayArray.ForwardIterator();
                  while (iter.MoveNext())
                  {
                     Revit.DB.Architecture.BoundarySegmentArray boundSegArray = iter.Current as Autodesk.Revit.DB.Architecture.BoundarySegmentArray;

                     if (null != boundSegArray)
                     {
                        Revit.DB.Architecture.BoundarySegmentArrayIterator arrayIter = boundSegArray.ForwardIterator();
                        while (arrayIter.MoveNext())
                        {
                           Revit.DB.BoundarySegment boundSeg = arrayIter.Current as Revit.DB.BoundarySegment;

                           if (null != boundSeg)
                           {
                              // once you get to the Boundary Segment which represent one of the sides of the room boundary, draw a Model Curve to 
                              // represent the outline.
                              ModelCurve modCurve = m_revitApp.ActiveUIDocument.Document.Create.NewModelCurve(boundSeg.Curve, sketchPlane);
                           }
                        }
                     }
                  }
               }
            }
         }
         else
         {
            MessageBox.Show("No rooms found in the Active Document", "RevitLookup", MessageBoxButtons.OK, MessageBoxIcon.Information);
         }

      }


      /// <summary>
      /// Calulate the area of all the available rooms and specify them using TextNotes
      /// </summary>
      public void
      RoomArea()
      {
         Revit.Creation.Document doc = m_revitApp.ActiveUIDocument.Document.Create;
         SketchPlane sketchPlane = Utils.Geometry.GetWorldPlane(m_revitApp);

         RevitLookup.Test.Forms.Levels lev = new RevitLookup.Test.Forms.Levels(m_revitApp);
         if (lev.ShowDialog() != DialogResult.OK)
            return;

         Level curLevel = lev.LevelSelected;
         if (curLevel == null)
         {
            MessageBox.Show("No Level was selected.");
            return;
         }

         // Get the plan topology of the active doc first
         PlanTopology planTopo = m_revitApp.ActiveUIDocument.Document.get_PlanTopology(curLevel);
         ElementSet roomSet = planTopo.Rooms;

         if (roomSet.Size > 0)
         {
            ElementSetIterator setIter = roomSet.ForwardIterator();
            while (setIter.MoveNext())
            {
               Room room = setIter.Current as Room;

               if (null != room)
               {
                  Autodesk.Revit.DB.View view = m_revitApp.ActiveUIDocument.Document.ActiveView;
                  LocationPoint locationPoint = room.Location as LocationPoint;

                  Double area = room.get_Parameter(BuiltInParameter.ROOM_AREA).AsDouble();

                  Double roundedArea = Math.Round(area, 2);

                  // providing an offset so that the Room Tag and the Area Tag dont overlap. Overlapping leads to an 
                  // alignment related assert.

                  XYZ offset = new XYZ(5.0, 0, 0);

                  /// align text middle and center
                  TextAlignFlags align = TextAlignFlags.TEF_ALIGN_MIDDLE ^ TextAlignFlags.TEF_ALIGN_CENTER;
                  TextNote txtNote = m_revitApp.ActiveUIDocument.Document.Create.NewTextNote(view, offset + locationPoint.Point, GeomUtils.kXAxis,
                                          view.ViewDirection, .25,
                                          align, roundedArea.ToString());
               }
            }
         }
         else
         {
            MessageBox.Show("No rooms found in the Active Document", "RevitLookup", MessageBoxButtons.OK, MessageBoxIcon.Information);
         }


         // TBD: Tried to play around with PlanCircuits and there seems to be a problem with the IsRoomLocated property. 
         // arj 1/23/07

         //Revit.PlanCircuitSet circSet = planTopo.Circuits;
         //Revit.PlanCircuitSetIterator setIters = circSet.ForwardIterator();

         //while (setIters.MoveNext())
         //{
         //    Revit.PlanCircuit planCircuit = setIters.Current as Revit.PlanCircuit;

         //    if (null != planCircuit)
         //    {
         //
         //        if (planCircuit.IsRoomLocated) // throws an exception always "Attempted to read or write protected memory.
         // This is often an indication that other memory is corrupt."
         //        {
         //        }
         //    }
         //}
      }

      /// <summary>
      /// Have the user select a type of Element and then filter the document for all instances of that type.
      /// </summary>
      public void FilterElementTypes()
      {
         Test.Forms.Elements elems = new Test.Forms.Elements(m_revitApp);
         if (elems.ShowDialog() != DialogResult.OK)
            return;

         ElementSet elemSet = new ElementSet();

         FilteredElementCollector fec = new FilteredElementCollector(m_revitApp.ActiveUIDocument.Document);
         ElementClassFilter whatAreWanted = new ElementClassFilter(elems.ElemTypeSelected);
         fec.WherePasses(whatAreWanted);
         List<Element> elements = fec.ToElements() as List<Element>;

         foreach (Element element in elements)
         {
            elemSet.Insert(element);
         }

         Snoop.Forms.Objects objs = new Snoop.Forms.Objects(elemSet);
         objs.ShowDialog();
      }

      /// <summary>
      /// Ask the user to open a revit family template and then add FamilyTypes and FamilyParameters
      /// to it. Say the user opens a Door template, he can then save the family as a new door family and load
      /// it into a new project for use.
      /// </summary>
      public void AddFamilyParameterAndType()
      {
         Document doc;

         OpenFileDialog openFileDialog = new OpenFileDialog();
         openFileDialog.Title = "Select family document";
         openFileDialog.Filter = "RFT Files (*.rft)|*.rft";

         if (openFileDialog.ShowDialog() == DialogResult.OK)
         {
            doc = m_revitApp.Application.NewFamilyDocument(openFileDialog.FileName);
         }
         else
            return;

         Transaction transaction = new Transaction(m_revitApp.ActiveUIDocument.Document, "AddFamilyParameterAndType");
         transaction.Start();

         if (doc.IsFamilyDocument)
         { // has to be a family document to be able to use the Family Manager.

            FamilyManager famMgr = doc.FamilyManager;

            //Add a family param. 
            FamilyParameter famParam = famMgr.AddParameter("RevitLookup_Param", BuiltInParameterGroup.PG_TITLE, ParameterType.Text, false);
            famMgr.Set(famParam, "Default text.");

            //Create a couple of new family types. Note that we can set different values for the param
            //in different family types.                

            FamilyType newFamType = famMgr.NewType("RevitLookup_Type1");
            famMgr.CurrentType = newFamType;

            if (newFamType.HasValue(famParam))
            {
               famMgr.Set(famParam, "Text1.");
            }

            FamilyType newFamType1 = famMgr.NewType("RevitLookup_Type2");
            famMgr.CurrentType = newFamType;

            if (newFamType.HasValue(famParam))
            {
               famMgr.Set(famParam, "Text2.");
            }

            famMgr.MakeType(famParam);

            if ((famParam != null) && (newFamType != null))
            {
               MessageBox.Show("New family types/params added successfully.", "RevitLookup", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
               MessageBox.Show("Family types/params addition failed.", "RevitLookup", MessageBoxButtons.OK, MessageBoxIcon.Error);
         }

         transaction.Commit();
      }
   }
}