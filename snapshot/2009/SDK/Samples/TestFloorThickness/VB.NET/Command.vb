'
' (C) Copyright 2003-2008 by Autodesk, Inc.
'
' Permission to use, copy, modify, and distribute this software in
' object code form for any purpose and without fee is hereby granted,
' provided that the above copyright notice appears in all copies and
' that both that copyright notice and the limited warranty and
' restricted rights notice below appear in all supporting
' documentation.
'
' AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
' AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
' MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
' DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
' UNINTERRUPTED OR ERROR FREE.
'
' Use, duplication, or disclosure by the U.S. Government is subject to
' restrictions set forth in FAR 52.227-19 (Commercial Computer
' Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
' (Rights in Technical Data and Computer Software), as applicable.
'

Public Class Command

   Implements Autodesk.Revit.IExternalCommand

   ''' <summary>
   ''' Implement this method as an external command for Revit.
   ''' </summary>
   ''' <param name="commandData">An object that is passed to the external application 
   ''' which contains data related to the command, 
   ''' such as the application object and active view.</param>
   ''' <param name="message">A message that can be set by the external application 
   ''' which will be displayed if a failure or cancellation is returned by 
   ''' the external command.</param>
   ''' <param name="elements">A set of elements to which the external application 
   ''' can add elements that are to be highlighted in case of failure or cancellation.</param>
   ''' <returns>Return the status of the external command. 
   ''' A result of Succeeded means that the API external method functioned as expected. 
   ''' Cancelled can be used to signify that the user cancelled the external operation 
   ''' at some point. Failure should be returned if the application is unable to proceed with 
   ''' the operation.</returns>
   ''' <remarks></remarks>

   Public Function Execute(ByVal commandData As Autodesk.Revit.ExternalCommandData, ByRef message As String, _
       ByVal elements As Autodesk.Revit.ElementSet) As Autodesk.Revit.IExternalCommand.Result _
       Implements Autodesk.Revit.IExternalCommand.Execute

      Dim application As Autodesk.Revit.Application = commandData.Application
      Dim document As Autodesk.Revit.Document = application.ActiveDocument
      Dim element As Autodesk.Revit.Element
      Dim selFloor As Boolean = False

      For Each element In document.Selection.Elements

         If TypeOf element Is Autodesk.Revit.Elements.Floor Then
            Dim floor As Autodesk.Revit.Elements.Floor = element
            Dim layer As Autodesk.Revit.Structural.CompoundStructureLayer
            selFloor = True

            For Each layer In floor.FloorType.CompoundStructure.Layers
               layer.Thickness = layer.Thickness * 10
            Next
         End If

      Next

      If selFloor Then
         Return Autodesk.Revit.IExternalCommand.Result.Succeeded
      Else
         message = "Please select one floor at least."
         Return Autodesk.Revit.IExternalCommand.Result.Cancelled
      End If
   End Function
End Class