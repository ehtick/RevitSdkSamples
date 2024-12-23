//
// (C) Copyright 2003-2007 by Autodesk, Inc.
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


namespace Revit.SDK.Samples.ObjectViewer.CS
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
    using System.ComponentModel;

	using Autodesk.Revit;
	using Autodesk.Revit.Elements;
	using Autodesk.Revit.Geometry;
	using Autodesk.Revit.Structural;

	using Element = Autodesk.Revit.Element;
	using GeometryElement = Autodesk.Revit.Geometry.Element;

	/// <summary>
	/// main command class of the ObjectViewer application
	/// </summary>
	public class ObjectViewer
	{
		/// <summary>
		/// choose the display kinds of preview
		/// </summary>
		public enum DisplayKinds
		{
            /// <summary>
            /// Geometry Model
            /// </summary>
			GeometryModel,

            /// <summary>
            /// AnalyticalModel
            /// </summary>
			AnalyticalModel
		}

		private Element m_selected;		// selected element to display
		private View m_currentView;		// current View for preview
		// current detail level for preview
		private Options.DetailLevels m_detailLevel = Options.DetailLevels.Fine;
		private List<View> m_allViews = new List<View>();	// all Views in current project
		// current display kind for preview
		private DisplayKinds m_displayKind = DisplayKinds.GeometryModel;
		// selected element's parameters' data
        private BindingList<Para> m_paras;
		// current sketch instance to draw the element
		private Sketch3D m_currentSketch3D = null;
        // indicate whether select view or detail
        private bool m_isSelectView = true;

		/// <summary>
		/// Current display kind for preview
		/// </summary>
		public DisplayKinds DisplayKind
		{
			get 
			{
				return m_displayKind; 
			}
			set
			{
				m_displayKind = value;
				UpdateSketch3D();
			}
		}

		/// <summary>
		/// Current View for preview
		/// </summary>
		public View CurrentView
		{
			get 
			{
				return m_currentView; 
			}
			set
			{
				m_currentView = value;
                m_isSelectView = true;
				UpdateSketch3D();
			}
		}

		/// <summary>
		/// All Views in current project
		/// </summary>
		public ReadOnlyCollection<View> AllViews
		{
			get 
			{
				return new ReadOnlyCollection<View>(m_allViews); 
			}
		}

		/// <summary>
		/// Current detail level for preview
		/// </summary>
		public Options.DetailLevels DetailLevel
		{
			get 
			{
				return m_detailLevel; 
			}
			set
			{
				m_detailLevel = value;
                m_isSelectView = false;
				UpdateSketch3D();
			}
		}

		/// <summary>
		/// Current sketch instance to draw the element
		/// </summary>
		public Sketch3D CurrentSketch3D
		{
			get 
			{
				return m_currentSketch3D; 
			}
		}

		/// <summary>
		/// Selected element's parameters' data
		/// </summary>
        public BindingList<Para> Params
		{
			get
			{ 
				return m_paras; 
			}
		}

		/// <summary>
		/// constructor
		/// </summary>
		public ObjectViewer()
		{
			Document doc = Command.CommandData.Application.ActiveDocument;
			ElementSet selection = doc.Selection.Elements;
			// only one element should be selected
			if (1 != selection.Size)
			{
			    throw new ErrorMessageException("Please select only one element.");
			}
			// get selected element
			foreach (Element e in selection)
			{
				m_selected = e;
			}
			// get current view and all views
			m_currentView = doc.ActiveView;
			ElementIterator itor = doc.Elements;
			itor.Reset();
			while (itor.MoveNext())
			{
				View view = itor.Current as View;
				if (null != view)
				{
					m_allViews.Add(view);
				}
			}

			// create a instance of Sketch3D            
			GeometryData geomFactory = new GeometryData(m_selected, m_currentView);
			m_currentSketch3D = new Sketch3D(geomFactory.Data3D, Graphics2DData.Empty);

            //get a instance of ParametersFactory and then use it to create Parameters
            ParasFactory parasFactory = new ParasFactory(m_selected);
			m_paras = parasFactory.CreateParas();
		}

		/// <summary>
		/// update current Sketch3D using current UCS and settings
		/// </summary>
		private void UpdateSketch3D()
		{
			if (m_displayKind == DisplayKinds.GeometryModel)
			{
                GeometryData geomFactory;
                if(m_isSelectView)
                {
                    geomFactory = new GeometryData(m_selected, m_currentView);
                    m_detailLevel = Options.DetailLevels.Undefined;

                }
                else
                {
                    geomFactory = new GeometryData(m_selected, m_detailLevel, m_currentView);
                }
                
				Graphics3DData geom3DData = geomFactory.Data3D;
				Graphics3DData old3DData = m_currentSketch3D.Data3D;
				geom3DData.CurrentUCS = old3DData.CurrentUCS;
				m_currentSketch3D.Data3D = geom3DData;
				m_currentSketch3D.Data2D = Graphics2DData.Empty;
			}
			else if (m_displayKind == DisplayKinds.AnalyticalModel)
			{
				ModelData modelFactory = new ModelData(m_selected);
				Graphics3DData model3DData = modelFactory.Data3D;
				Graphics2DData model2DData = modelFactory.Data2D;
				Graphics3DData old3DData = m_currentSketch3D.Data3D;
				model3DData.CurrentUCS = old3DData.CurrentUCS;
				m_currentSketch3D.Data3D = model3DData;
				m_currentSketch3D.Data2D = model2DData;
			}
		}
	}
}
