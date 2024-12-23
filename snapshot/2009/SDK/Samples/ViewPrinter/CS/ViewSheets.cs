//
// (C) Copyright 2003-2008 by Autodesk, Inc.
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
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using Autodesk.Revit;
using Autodesk.Revit.Elements;

namespace Revit.SDK.Samples.ViewPrinter.CS
{
    public enum VisibleType
    {
        VT_ViewOnly,
        VT_SheetOnly,
        VT_BothViewAndSheet,
        VT_None
    }

    public interface ISettingNameOperation
    {
        string SettingName
        {
            get;
            set;
        }

        string Prefix
        {
            get;
        }

        int SettingCount
        {
            get;
        }

        bool Rename(string name);
        bool SaveAs(string newName);
    }

    /// <summary>
    /// Exposes the View/Sheet Set interfaces just like 
    /// the View/Sheet Set Dialog (File->Print...; selected views/sheets->Select...) in UI.
    /// </summary>
    public class ViewSheets : ISettingNameOperation
    {
        Document m_doc;

        public ViewSheets(Document doc)
        {
            m_doc = doc;
        }

        public string SettingName
        {
            get
            {
                return m_doc.PrintManager.ViewSheetSetting.CurrentViewSheetSet.Name;
            }
            set
            {
                foreach (ViewSheetSet viewSheetSet in m_doc.ViewSheetSets)
                {
                    if (viewSheetSet.Name.Equals(value as string))
                    {
                        m_doc.PrintManager.ViewSheetSetting.CurrentViewSheetSet = viewSheetSet;
                        return;
                    }
                }
            }
        }

        public string Prefix
        {
            get
            {
                return "Set ";
            }
        }

        public int SettingCount
        {
            get
            {
                return m_doc.ViewSheetSets.Size;
            }
        }

        public bool SaveAs(string newName)
        {
            try
            {
                return m_doc.PrintManager.ViewSheetSetting.SaveAs(newName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Revit Printer");
                return false;
            }
        }

        public bool Rename(string name)
        {            
            try
            {
                return m_doc.PrintManager.ViewSheetSetting.Rename(name);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Revit Printer");
                return false;
            }
        }

        public List<string> ViewSheetSetNames
        {
            get
            {
                List<string> names = new List<string>();
                foreach (ViewSheetSet viewSheetSet in m_doc.ViewSheetSets)
                {
                    names.Add(viewSheetSet.Name);
                }

                return names;
            }
        }

        public bool Save()
        {
            try
            {
                return m_doc.PrintManager.ViewSheetSetting.Save();
            }
            catch (Exception)
            {
                return false;
            }
        }        

        public void Revert()
        {
            try
            {
                m_doc.PrintManager.ViewSheetSetting.Revert();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Revit Printer");
            }
        }
        
        public bool Delete()
        {
            try
            {
                return m_doc.PrintManager.ViewSheetSetting.Delete();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Revit Printer");
                return false;
            }
            
        }

        public List<Autodesk.Revit.Elements.View> AvailableViewSheetSet(VisibleType visibleType)
        {
            if (visibleType == VisibleType.VT_None)
                return null;

            List<Autodesk.Revit.Elements.View> views = new List<Autodesk.Revit.Elements.View>();
            foreach (Autodesk.Revit.Elements.View view in m_doc.PrintManager.ViewSheetSetting.AvailableViews)
            {
                if (view.ViewType == Autodesk.Revit.Enums.ViewType.DrawingSheet
                    && visibleType == VisibleType.VT_ViewOnly)
                {                    
                    continue;   // filter out sheets.
                }
                if (view.ViewType != Autodesk.Revit.Enums.ViewType.DrawingSheet
                    && visibleType == VisibleType.VT_SheetOnly)
                {
                    continue;   // filter out views.
                }

                views.Add(view);
            }

            return views;
        }

        public bool IsSelected(string viewName)
        {
            foreach (Autodesk.Revit.Elements.View view in m_doc.PrintManager.ViewSheetSetting.CurrentViewSheetSet.Views)
            {
                if (viewName.Equals(view.ViewType.ToString() + ": " + view.ViewName))
                {
                    return true;
                }
            }

            return false;
        }

        public void ChangeCurrentViewSheetSet(List<string> names)
        {
            ViewSet selectedViews = new ViewSet();

            if (null != names && 0 < names.Count)
            {
                foreach (Autodesk.Revit.Elements.View view in m_doc.PrintManager.ViewSheetSetting.AvailableViews)
                {
                    if (names.Contains(view.ViewType.ToString() + ": " + view.ViewName))
                    {
                        selectedViews.Insert(view);
                    }
                }
            }

            ViewSheetSet viewSheetSet = m_doc.PrintManager.ViewSheetSetting.CurrentViewSheetSet;
            viewSheetSet.Views = selectedViews;
            m_doc.PrintManager.ViewSheetSetting.CurrentViewSheetSet = viewSheetSet;
        }

    }
}
