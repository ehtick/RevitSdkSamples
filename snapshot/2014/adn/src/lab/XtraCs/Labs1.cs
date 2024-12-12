#region Header
// Revit API .NET Labs
//
// Copyright (C) 2007-2013 by Autodesk, Inc.
//
// Permission to use, copy, modify, and distribute this software
// for any purpose and without fee is hereby granted, provided
// that the above copyright notice appears in all copies and
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
#endregion // Header

#region Namespaces
using System.Collections.Generic;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
#endregion // Namespaces

namespace XtraCs
{
  #region Lab1_1_HelloWorld
  /// <summary>
  /// Say hello.
  /// <include file='../doc/labs.xml' path='labs/lab[@name="1-1"]/*' />
  /// </summary>
  [Transaction( TransactionMode.ReadOnly )]
  public class Lab1_1_HelloWorld : IExternalCommand
  {
    /// <summary>
    /// The one and only method required by the IExternalCommand interface,
    /// the main entry point for every external command.
    /// </summary>
    /// <param name="commandData">Input argument providing access to the Revit application and its documents and their properties.</param>
    /// <param name="message">Return argument to display a message to the user in case of error if Result is not Succeeded.</param>
    /// <param name="elements">Return argument to highlight elements on the graphics screen if Result is not Succeeded.</param>
    /// <returns>Cancelled, Failed or Succeeded Result code.</returns>
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      #region 1.1 Display a message using the TaskDialog Show method:
      //LabUtils.InfoMsg( "Hello World" );
      TaskDialog.Show( "Revit API Labs", "Hello World" );
      #endregion // 1.1 Display a message using the TaskDialog Show method

      return Result.Failed;
    }
  }
  #endregion // Lab1_1_HelloWorld

  #region Lab1_2_CommandArguments
  /// <summary>
  /// In this lab, we explore the contents and usage of the Execute
  /// method's command data input argument and the meaning of the result
  /// return code and the message and element set return arguments.
  /// The details are discussed in the developer guide,
  /// chapter 3.2, External Commands.
  /// <include file='../doc/labs.xml' path='labs/lab[@name="1-2"]/*' />
  /// </summary>
  [Transaction( TransactionMode.ReadOnly )]
  public class Lab1_2_CommandArguments : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      #region 1.2.a. Examine command data input argument:
      //
      // access application, document, and current view:
      //
      UIApplication uiapp = commandData.Application;
      Application app = uiapp.Application;
      UIDocument uidoc = uiapp.ActiveUIDocument;
      Document doc = uidoc.Document;
      View view = commandData.View;
      LanguageType lt = app.Language;
      ProductType pt = app.Product;
      string s = "Application = " + app.VersionName
        + "\r\nLanguage = " + lt.ToString()
        + "\r\nProduct = " + pt.ToString()
        + "\r\nVersion = " + app.VersionNumber
        + "\r\nDocument path = " + doc.PathName // empty if not yet saved
        + "\r\nDocument title = " + doc.Title
        + "\r\nView name = " + view.Name;
      LabUtils.InfoMsg( s );
      #endregion // 1.2.a. Examine command data input argument

      #region 1.2.b. List selection set content:
      //
      // list the current selection set:
      //
      Selection sel = uidoc.Selection;
      List<string> a = new List<string>();
      foreach( Element e in sel.Elements )
      {
        string name = ( null == e.Category ) ? e.GetType().Name : e.Category.Name;
        a.Add( name + " Id=" + e.Id.IntegerValue.ToString() );
      }
      LabUtils.InfoMsg( "There are {0} element{1} in the selection set{2}", a );
      #endregion // 1.2.b. List selection set content

      #region 1.2.c. Populate return arguments:
      //
      // we pretend that something is wrong with the first
      // element in the selection. pass a message back to
      // the user and indicate the error result:
      //
      if( !sel.Elements.IsEmpty )
      {
        ElementSetIterator iter = sel.Elements.ForwardIterator();
        iter.MoveNext();
        Element errElem = iter.Current as Element;
        elements.Clear();
        elements.Insert( errElem );
        message = "We pretend something is wrong with this element and pass back this message to user";
        return Result.Failed;
      }
      else
      {
        //
        // we return failed here as well, actually.
        // as long as the message string and element set are empty,
        // it makes no difference to the user.
        // it also aborts the automatic transaction, avoiding marking
        // the database as dirty.
        //
        return Result.Failed;
      }
      #endregion // 1.2.c. Populate return arguments
    }
  }
  #endregion // Lab1_2_CommandArguments
}

// "C:\Program Files\Autodesk\Revit Architecture 2014\Samples\rac_basic_sample_project.rvt"