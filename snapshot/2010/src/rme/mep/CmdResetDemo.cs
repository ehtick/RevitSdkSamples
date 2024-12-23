#region Header
// Revit MEP API sample application
//
// Copyright (C) 2007-2009 by Jeremy Tammik, Autodesk, Inc.
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
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  
// AUTODESK, INC. DOES NOT WARRANT THAT THE OPERATION OF THE 
// PROGRAM WILL BE UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject
// to restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
#endregion // Header

#region Namespaces
using System;
using System.Collections.Generic;
using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Symbols;
using CmdResult = Autodesk.Revit.IExternalCommand.Result;
#endregion // Namespaces

namespace mep
{
  class CmdResetDemo : IExternalCommand
  {
    #region Execute Command
    /// <summary>
    /// Reset the Revit model to pre-demo conditions.
    /// </summary>
    public CmdResult Execute(
      ExternalCommandData commandData,
      ref String message,
      ElementSet elements )
    {
      try
      {
        ResetSupplyAirTerminals( commandData );
        SetSpaceCfmPerSfToZero( commandData );
        return CmdResult.Succeeded;
      }
      catch( Exception ex )
      {
        message = ex.Message;
        return CmdResult.Failed;
      }
    }

    private void SetSpaceCfmPerSfToZero( ExternalCommandData commandData )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;
      List<Element> spaces = new List<Element>();
      doc.get_Elements( typeof( Space ), spaces );
      int n = spaces.Count;
      string s = "{0} of " + n.ToString() + " spaces reset...";
      using( ProgressForm pf = new ProgressForm( "Reset parameter", s, n ) )
      {
        foreach( Space space in spaces )
        {
          SetCfmPerSf( space, 0.0 );
          pf.Increment();
        }
      }
    }

    static void SetCfmPerSf( Space space, double value )
    {
      Parameter pCfmPerSf = Util.GetSpaceParameter( space, ParameterName.CfmPerSf );
      pCfmPerSf.Set( value );
    }

    private void ResetSupplyAirTerminals( ExternalCommandData commandData )
    {
      WaitCursor waitCursor = new WaitCursor();
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;
      ElementSet terminals = Util.GetSupplyAirTerminals( app );
      int n = terminals.Size;
      string s = "{0} of " + n.ToString() + " terminals reset...";
      string caption = "Resetting Supply Air Termainal Flows and Sizes";
      using( ProgressForm pf = new ProgressForm( caption, s, n ) )
      {
        foreach( FamilyInstance terminal in terminals )
        {
          // reset flow
          Parameter p = Util.GetTerminalFlowParameter( terminal );
          p.Set( 0 );

          // reset size
          foreach( FamilySymbol sym in terminal.Symbol.Family.Symbols )
          {
            terminal.Symbol = sym; // simply set to first symbol found
            break; // done after getting the first symbol
          }
          pf.Increment();
        }
      }
    }
    #endregion // Execute Command
  }
}
