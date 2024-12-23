
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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;

using Autodesk.Revit.Collections;

using RevitLookup.Snoop.Collectors;

namespace RevitLookup.Snoop.CollectorExts
{
	/// <summary>
	/// Provide Snoop.Data for any classes related to a Document.
	/// </summary>
	
	public class CollectorExtDoc : CollectorExt
	{
		public
		CollectorExtDoc()
		{
		}

        protected override void
        CollectEvent(object sender, CollectorEventArgs e)
        {
                // cast the sender object to the SnoopCollector we are expecting
            Collector snoopCollector = sender as Collector;
            if (snoopCollector == null) {
                Debug.Assert(false);    // why did someone else send us the message?
                return;
            }

                // see if it is a type we are responsible for
			Document doc = e.ObjToSnoop as Document;
			if (doc != null) {
				Stream(snoopCollector.Data(), doc);
				return;
			}

			Selection sel = e.ObjToSnoop as Selection;
			if (sel != null) {
				Stream(snoopCollector.Data(), sel);
				return;
			}

			Settings settings = e.ObjToSnoop as Settings;
			if (settings != null) {
				Stream(snoopCollector.Data(), settings);
				return;
			}

			Category cat = e.ObjToSnoop as Category;
			if (cat != null) {
				Stream(snoopCollector.Data(), cat);
				return;
			}

            PaperSize paperSize = e.ObjToSnoop as PaperSize;
            if (paperSize != null) {
                Stream(snoopCollector.Data(), paperSize);
                return;
            }

            PaperSource paperSource = e.ObjToSnoop as PaperSource;
            if (paperSource != null) {
                Stream(snoopCollector.Data(), paperSource);
                return;
            }

            PrintSetup prnSetup = e.ObjToSnoop as PrintSetup;
            if (prnSetup != null) {
                Stream(snoopCollector.Data(), prnSetup);
                return;
            }

            PrintParameters prnParams = e.ObjToSnoop as PrintParameters;
            if (prnParams != null) {
                Stream(snoopCollector.Data(), prnParams);
                return;
            }

			PlanTopology planTopo = e.ObjToSnoop as PlanTopology;
			if (planTopo != null) {
				Stream(snoopCollector.Data(), planTopo);
				return;
			}

			PlanCircuit planCircuit = e.ObjToSnoop as PlanCircuit;
			if (planCircuit != null) {
				Stream(snoopCollector.Data(), planCircuit);
				return;
			}

            PrintManager printManager = e.ObjToSnoop as PrintManager;
            if (printManager != null) {
                Stream(snoopCollector.Data(), printManager);
                return;
            }





        }
        
		private void
		Stream(ArrayList data, Document doc)
		{
		    data.Add(new Snoop.Data.ClassSeparator(typeof(Document)));

            data.Add(new Snoop.Data.Object("Application", doc.Application));
            data.Add(new Snoop.Data.String("Title", doc.Title));
            data.Add(new Snoop.Data.String("Pathname", doc.PathName));
            data.Add(new Snoop.Data.Object("Settings", doc.Settings));
            data.Add(new Snoop.Data.BindingMap("Parameter bindings", doc.ParameterBindings));
            data.Add(new Snoop.Data.Enumerable("Phases", doc.Phases));
            data.Add(new Snoop.Data.Bool("Reactions are up to date", doc.ReactionsAreUpToDate));
            data.Add(new Snoop.Data.Object("Active View", doc.ActiveView));
            data.Add(new Snoop.Data.String("Display unit system", doc.DisplayUnitSystem.ToString()));
            data.Add(new Snoop.Data.Object("Active project location", doc.ActiveProjectLocation));
            data.Add(new Snoop.Data.Object("Project information", doc.ProjectInformation));
            data.Add(new Snoop.Data.Enumerable("Project locations", doc.ProjectLocations));
            data.Add(new Snoop.Data.Object("Site location", doc.SiteLocation));
            data.Add(new Snoop.Data.Object("Project unit", doc.ProjectUnit));
            try
            {
               Transaction t = new Transaction(doc);
               t.SetName("PlanTopologies");
               t.Start();
               data.Add(new Snoop.Data.Enumerable("Plan topologies", doc.PlanTopologies));
               t.RollBack();
            }
            catch (Autodesk.Revit.Exceptions.ArgumentException)
            {
               //catch exception caused because of inability to create transaction for linked document
            }
            ElementSet elemSet1 = new ElementSet();
            FilteredElementCollector fec = new FilteredElementCollector(doc);
            ElementClassFilter wallFilter = new ElementClassFilter(typeof(Wall));
            ElementClassFilter notWallFilter = new ElementClassFilter(typeof(Wall),true);
            LogicalOrFilter orFilter = new LogicalOrFilter(wallFilter, notWallFilter);
            fec.WherePasses(orFilter);
            List<Element> elements = fec.ToElements() as List<Element>;

            foreach (Element element in elements)
            {
               elemSet1.Insert(element);
            }

            data.Add(new Snoop.Data.ElementSet("Elements", elemSet1));

            data.Add(new Snoop.Data.Bool("Is modified", doc.IsModified));
            data.Add(new Snoop.Data.Bool("Is workshared", doc.IsWorkshared));

            data.Add(new Snoop.Data.Bool("Is A Family Document", doc.IsFamilyDocument));
            if (doc.IsFamilyDocument)
            {
                data.Add(new Snoop.Data.Object("Family Manager", doc.FamilyManager));
                data.Add(new Snoop.Data.Object("Owner Family", doc.OwnerFamily));
            }

            data.Add(new Snoop.Data.String("Worksharing central filename", doc.WorksharingCentralFilename));
            data.Add(new Snoop.Data.Object("Print manager", doc.PrintManager));

           //data.Add(new Snoop.Data.Enumerable("Print settings", doc.PrintSettings));  //TBD: Behaves badly, need to investigate.                

            data.Add(new Snoop.Data.Enumerable("Beam system types", doc.BeamSystemTypes));
            data.Add(new Snoop.Data.Enumerable("Continuous footing types", doc.ContFootingTypes));
            data.Add(new Snoop.Data.Enumerable("Curtain system types", doc.CurtainSystemTypes));
            data.Add(new Snoop.Data.Enumerable("Deck profiles", doc.DeckProfiles));
            data.Add(new Snoop.Data.Enumerable("Dimension types", doc.DimensionTypes));
            data.Add(new Snoop.Data.Enumerable("Electrical equipment types", doc.ElectricalEquipmentTypes));
            data.Add(new Snoop.Data.Enumerable("Fascia types", doc.FasciaTypes));
            data.Add(new Snoop.Data.Enumerable("Floor types", doc.FloorTypes));
            data.Add(new Snoop.Data.Enumerable("Grid types", doc.GridTypes));
            data.Add(new Snoop.Data.Enumerable("Gutter types", doc.GutterTypes));
            data.Add(new Snoop.Data.Enumerable("Level types", doc.LevelTypes));
            data.Add(new Snoop.Data.Enumerable("Lighting device types", doc.LightingDeviceTypes));
            data.Add(new Snoop.Data.Enumerable("Lighting fixture types", doc.LightingFixtureTypes));
            data.Add(new Snoop.Data.Enumerable("Mechanical equipment types", doc.MechanicalEquipmentTypes));
            data.Add(new Snoop.Data.Enumerable("Mullion types", doc.MullionTypes));
            data.Add(new Snoop.Data.Enumerable("Panel types", doc.PanelTypes));
            data.Add(new Snoop.Data.Enumerable("Annotation symbol types", doc.AnnotationSymbolTypes));
            data.Add(new Snoop.Data.Enumerable("Text note types", doc.TextNoteTypes));
            data.Add(new Snoop.Data.Enumerable("Rebar bar types", doc.RebarBarTypes));
            data.Add(new Snoop.Data.Enumerable("Rebar cover types", doc.RebarCoverTypes));
            data.Add(new Snoop.Data.Enumerable("Rebar hook types", doc.RebarHookTypes));
            data.Add(new Snoop.Data.Enumerable("Rebar shapes", doc.RebarShapes));
            data.Add(new Snoop.Data.Enumerable("Roof types", doc.RoofTypes));
            data.Add(new Snoop.Data.Enumerable("Room tag types", doc.RoomTagTypes));
            data.Add(new Snoop.Data.Enumerable("Slabe edge types", doc.SlabEdgeTypes));
            data.Add(new Snoop.Data.Enumerable("Space tag types", doc.SpaceTagTypes));
            data.Add(new Snoop.Data.Enumerable("Spot dimension types", doc.SpotDimensionTypes));
            data.Add(new Snoop.Data.Enumerable("Text note types", doc.TextNoteTypes));
            data.Add(new Snoop.Data.Enumerable("Title blocks", doc.TitleBlocks));
            data.Add(new Snoop.Data.Enumerable("Truss types", doc.TrussTypes));
            data.Add(new Snoop.Data.Enumerable("View sheet sets", doc.ViewSheetSets));
            data.Add(new Snoop.Data.Enumerable("Wall types", doc.WallTypes));            
        }

		private void
		Stream(ArrayList data, Selection sel)
		{
		    data.Add(new Snoop.Data.ClassSeparator(typeof(Selection)));

            data.Add(new Snoop.Data.ElementSet("Elements", sel.Elements));
        }

		private void
		Stream(ArrayList data, Settings settings)
		{
		    data.Add(new Snoop.Data.ClassSeparator(typeof(Settings)));
		    
            data.Add(new Snoop.Data.Enumerable("Categories", settings.Categories));
            //To get FillPatterns, first filter FillPatternElement out and use FillPatternElement.GetFillPattern()
            //Same for LinePatterns
            //data.Add(new Snoop.Data.Enumerable("Fill patterns", settings.FillPatterns));
            //data.Add(new Snoop.Data.Enumerable("Line patterns", settings.LinePatterns));
            data.Add(new Snoop.Data.Enumerable("Materials", settings.Materials));

            try {
                data.Add(new Snoop.Data.Object("Electrical setting", settings.ElectricalSetting));
            }
            catch (System.Exception ex) {
                data.Add(new Snoop.Data.Exception("Electrical setting", ex));
            }

            try {
                data.Add(new Snoop.Data.Object("Volume calculation setting", settings.VolumeCalculationSetting));
            }
            catch (System.Exception ex) {
                data.Add(new Snoop.Data.Exception("Volume calculation setting", ex));
            }
        }

		private void
		Stream(ArrayList data, Category cat)
		{
		    data.Add(new Snoop.Data.ClassSeparator(typeof(Category)));

            data.Add(new Snoop.Data.Bool("Allow bound parameters", cat.AllowsBoundParameters));
            data.Add(new Snoop.Data.Bool("Can add sub-category", cat.CanAddSubcategory));
            data.Add(new Snoop.Data.Bool("Has material quantities", cat.HasMaterialQuantities));
            data.Add(new Snoop.Data.Bool("Is cuttable", cat.IsCuttable));
            data.Add(new Snoop.Data.Object("Line color", cat.LineColor));
            data.Add(new Snoop.Data.String("Name", cat.Name));
            data.Add(new Snoop.Data.Int("Element Id", cat.Id.IntegerValue));
            data.Add(new Snoop.Data.String("Built-in category", ((BuiltInCategory)cat.Id.IntegerValue).ToString()));
            data.Add(new Snoop.Data.Object("Material", cat.Material));
            data.Add(new Snoop.Data.Object("Parent", cat.Parent));
            data.Add(new Snoop.Data.CategoryNameMap("Sub categories", cat.SubCategories));
        }

        private void Stream(ArrayList data, FamilyManager mgr)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(FamilyManager)));

            try
            {
                data.Add(new Snoop.Data.Object("Current Type", mgr.CurrentType));
            }
            catch (System.Exception ex)
            {
                data.Add(new Snoop.Data.Exception("Current Type", ex));
            }

            data.Add(new Snoop.Data.Enumerable("Parameters", mgr.Parameters));

            data.Add(new Snoop.Data.Enumerable("Types", mgr.Types));


        }






        private void
        Stream(ArrayList data, PaperSize paperSize)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(PaperSize)));

            data.Add(new Snoop.Data.String("Name", paperSize.Name));
        }

        private void
        Stream(ArrayList data, PaperSource paperSource)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(PaperSource)));

            data.Add(new Snoop.Data.String("Name", paperSource.Name));
        }

        private void
        Stream(ArrayList data, PrintSetup prnSetup)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(PrintSetup)));

            data.Add(new Snoop.Data.Object("Current print setting", prnSetup.CurrentPrintSetting));
            //data.Add(new Snoop.Data.Enumerable("Paper sizes", prnSetup.PaperSizes));
            //data.Add(new Snoop.Data.Enumerable("Paper sources", prnSetup.PaperSources));
        }

        private void
        Stream(ArrayList data, PrintParameters prnParams)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(PrintParameters)));

            data.Add(new Snoop.Data.String("Color depth", prnParams.ColorDepth.ToString()));
            data.Add(new Snoop.Data.String("Hidden line views", prnParams.HiddenLineViews.ToString()));
            data.Add(new Snoop.Data.Bool("Hide crop boundaries", prnParams.HideCropBoundaries));
            data.Add(new Snoop.Data.Bool("Hide refor work planes", prnParams.HideReforWorkPlanes)); // TBD - Check property name (seems to be spelt wrong)
            data.Add(new Snoop.Data.Bool("Hide scope boxes", prnParams.HideScopeBoxes));
            data.Add(new Snoop.Data.Bool("Hide unreferenced view tags", prnParams.HideUnreferencedViewTages)); // TBD - Check property name (seems to be spelt wrong)
            data.Add(new Snoop.Data.String("Margin type", prnParams.MarginType.ToString()));
            data.Add(new Snoop.Data.String("Page orientation", prnParams.PageOrientation.ToString()));

            try {
                data.Add(new Snoop.Data.String("Paper placement", prnParams.PaperPlacement.ToString()));
            }
            catch (System.Exception ex) {
                data.Add(new Snoop.Data.Exception("Paper placement", ex));
            }

            data.Add(new Snoop.Data.Object("Paper size", prnParams.PaperSize));
            data.Add(new Snoop.Data.Object("Paper source", prnParams.PaperSource));

            try {
                data.Add(new Snoop.Data.String("Raster quality", prnParams.RasterQuality.ToString()));
            }
            catch (System.Exception ex) {
                data.Add(new Snoop.Data.Exception("Raster quality", ex));
            }

            try {
                data.Add(new Snoop.Data.Double("User defined margin X", prnParams.UserDefinedMarginX));
            }
            catch (System.Exception ex) {
                data.Add(new Snoop.Data.Exception("User defined margin X", ex));
            }

            try {
                data.Add(new Snoop.Data.Double("User defined margin Y", prnParams.UserDefinedMarginY));
            }
            catch (System.Exception ex) {
                data.Add(new Snoop.Data.Exception("User defined margin Y", ex));
            }

            data.Add(new Snoop.Data.Bool("View links in blue", prnParams.ViewLinksinBlue));

            try {
                data.Add(new Snoop.Data.Int("Zoom", prnParams.Zoom));
            }
            catch (System.Exception ex) {
                data.Add(new Snoop.Data.Exception("Zoom", ex));
            }

            try {
                data.Add(new Snoop.Data.String("Zoom type", prnParams.ZoomType.ToString()));
            }
            catch (System.Exception ex) {
                data.Add(new Snoop.Data.Exception("Zoom type", ex));
            }
        }

		private void
		Stream(ArrayList data, PlanTopology planTopo)
		{
		    data.Add(new Snoop.Data.ClassSeparator(typeof(PlanTopology)));

            data.Add(new Snoop.Data.Enumerable("Circuits", planTopo.Circuits));
            data.Add(new Snoop.Data.Object("Level", planTopo.Level));
            data.Add(new Snoop.Data.Object("Phase", planTopo.Phase));
            try
            {
                data.Add(new Snoop.Data.ElementSet("Rooms", planTopo.Rooms));
            }
            catch (System.Exception)
            {
            }
        }

		private void
		Stream(ArrayList data, PlanCircuit planCircuit)
		{
		    data.Add(new Snoop.Data.ClassSeparator(typeof(PlanCircuit)));

            data.Add(new Snoop.Data.Double("Area", planCircuit.Area));
            /// TBD: This always throws an exception
            /// 01/24/07
            try {
                data.Add(new Snoop.Data.Bool("Is room located", planCircuit.IsRoomLocated));
            }
            catch (System.Exception ex) {
                data.Add(new Snoop.Data.Exception("Is room located", ex));
            }
            data.Add(new Snoop.Data.Int("Side num", planCircuit.SideNum));
        }

        private void
        Stream(ArrayList data, PrintManager printManager)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(PrintManager)));

            try {
                data.Add(new Snoop.Data.Bool("Collate", printManager.Collate));
            }
            catch (System.Exception ex) {
                data.Add(new Snoop.Data.Exception("Collate", ex));
            }

            try {
                data.Add(new Snoop.Data.Bool("Combined file", printManager.CombinedFile));
            }
            catch (System.Exception ex) {
                data.Add(new Snoop.Data.Exception("Combined file", ex));
            }

            data.Add(new Snoop.Data.Int("Copy number", printManager.CopyNumber));
            data.Add(new Snoop.Data.String("Is virtual", printManager.IsVirtual.ToString()));
            data.Add(new Snoop.Data.String("Printer name", printManager.PrinterName));
            data.Add(new Snoop.Data.Bool("Printer order reverse", printManager.PrintOrderReverse));
            data.Add(new Snoop.Data.String("Print range", printManager.PrintRange.ToString()));
            data.Add(new Snoop.Data.Object("Print setup", printManager.PrintSetup));

            try {
                data.Add(new Snoop.Data.Bool("Print to file", printManager.PrintToFile));
            }
            catch (System.Exception ex) {
                data.Add(new Snoop.Data.Exception("Print to file", ex));
            }

            try {
                data.Add(new Snoop.Data.String("Print to file name", printManager.PrintToFileName));
            }
            catch (System.Exception ex) {
                data.Add(new Snoop.Data.Exception("Print to file name", ex));               
            }

            try  {
                data.Add(new Snoop.Data.Object("View sheet setting", printManager.ViewSheetSetting));
            }
            catch (System.Exception ex) {
                data.Add(new Snoop.Data.Exception("View sheet setting", ex));
            }
        }


	}
}
