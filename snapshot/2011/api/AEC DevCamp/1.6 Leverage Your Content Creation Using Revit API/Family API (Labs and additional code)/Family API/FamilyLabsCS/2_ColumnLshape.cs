#region Copyright
//
// (C) Copyright 2009-2010 by Autodesk, Inc.
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
#endregion // Copyright

#region Namespaces

using System;
using System.Collections.Generic;
using System.Linq; // in System.Core 
using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices; 

#endregion // Namespaces

#region Description
/// <summary>
/// Revit Family Creation API Lab - 2
///
/// This command defines a column family, and creates a column family with a L-shape profile.
/// In order to define a L-shape profile, we will need additional reference plans, dimensions and parameters.
///
/// Objective:
/// ----------
///
/// In the previous lab, we have learned the following:
///
///   0. set up family environment
///   1. create a solid
///   2. set alignment
///   3. add types
///
/// In this lab, we will learn the following:
///
///   4. add reference planes
///   5. add parameters
///   6. add dimensions
///
/// To test this lab, open a family template "Metric Column.rft", and run a command.
///
/// Context:
/// --------
///
/// In the previous rfa lab (lab1), we have defined a column family, using a rectangle profile.
/// In this lab, we will modify the profile to L-shape like the follow sketch shows:
///
///      5 Tw 4
///        +-+
///        | | 3          h = height
/// Depth  | +---+ 2
///        +-----+ Td
///       0        1
///       6  Width
///
/// in addition to what we have learned in the first lab, we will do the following:
///
///   1. add reference planes along (1) 2-3 and (2)3-4.
///   2. add parameters, Tw and Td
///   3. add dimensions and label with parameters Tw and Td
///
/// Desclaimer: code in these labs is written for the purpose of learning the Revit family API.
/// In practice, there will be much room for performance and usability improvement.
/// For code readability, minimum error checking.
/// </summary>
#endregion // Description

namespace FamilyLabsCS
{
  [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)]
  [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Automatic)]
  class RvtCmd_FamilyCreateColumnLShape : IExternalCommand
  {
    // member variables for top level access to the Revit database
    //
    Application _rvtApp;
    Document _rvtDoc;

    // command main
    //
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      // objects for the top level access
      //
      _rvtApp = commandData.Application.Application;
      _rvtDoc = commandData.Application.ActiveUIDocument.Document;

      // (0) This command works in the context of family editor only.
      //     We also check if the template is for an appropriate category if needed.
      //     Here we use a Column(i.e., Metric Column.rft) template.
      //     Although there is no specific checking about metric or imperial, our lab only works in metric for now.
      //
      if (!isRightTemplate(BuiltInCategory.OST_Columns))
      {
        Util.ErrorMsg( "Please open Metric Column.rft" );
        return Result.Failed;
      }

      // (1.1) add reference planes
      addReferencePlanes();

      // (1.2) create a simple extrusion. This time we create a L-shape.
      Extrusion pSolid = createSolid();

      // (2) add alignment
      addAlignments( pSolid );

      // (3.1) add parameters
      addParameters();

      // (3.2) add dimensions
      addDimensions();

      // (3.3) add types
      addTypes();

      // test family parameter value modification:
      //modifyFamilyParamValue();

      // finally, return
      return Result.Succeeded;
    }

    // ============================================
    //   (0) check if we have a correct template
    // ============================================
    bool isRightTemplate(BuiltInCategory targetCategory)
    {
      // This command works in the context of family editor only.
      //
      if( !_rvtDoc.IsFamilyDocument )
      {
        Util.ErrorMsg( "This command works only in the family editor." );
        return false;
      }

      // Check the template for an appropriate category here if needed.
      //
      Category cat = _rvtDoc.Settings.Categories.get_Item( targetCategory );
      if( _rvtDoc.OwnerFamily == null )
      {
        Util.ErrorMsg( "This command only works in the family context." );
        return false;
      }
      if( !cat.Id.Equals( _rvtDoc.OwnerFamily.FamilyCategory.Id ) )
      {
        Util.ErrorMsg( "Category of this family document does not match the context required by this command." );
        return false;
      }

      // if we come here, we should have a right one.
      return true;
    }

    // ==============================
    //   (1.1) add reference planes
    // ==============================
    void addReferencePlanes()
    {
      //
      // we are defining a simple L-shape profile like the following:
      //
      //  5 tw 4
      //   +-+
      //   | | 3          h = height
      // d | +---+ 2
      //   +-----+ td
      //  0        1
      //  6  w
      //
      //
      // we want to add ref planes along (1) 2-3 and (2)3-4.
      // Name them "OffsetH" and "OffsetV" respectively. (H for horizontal, V for vertical).
      //
      double tw = mmToFeet( 150.0 );  // thickness added for Lab2.  Hard-coding for simplicity.
      double td = mmToFeet( 150.0 );

      //
      // (1) add a horizonal ref plane 2-3.
      //
      // get a plan view
      View pViewPlan = findElement( typeof( ViewPlan ), "Lower Ref. Level" ) as View;

      // we have predefined ref plane: Left/Right/Front/Back
      // get the ref plane at Front, which is aligned to line 2-3
      ReferencePlane refFront = findElement( typeof( ReferencePlane ), "Front" ) as ReferencePlane;

      // get the bubble and free ends from front ref plane and offset by td.
      //
      XYZ p1 = refFront.BubbleEnd;
      XYZ p2 = refFront.FreeEnd;
      XYZ pBubbleEnd = new XYZ(p1.X, p1.Y + td, p1.Z);
      XYZ pFreeEnd = new XYZ(p2.X, p2.Y + td, p2.Z);

      // create a new one reference plane and name it "OffsetH"
      //
      ReferencePlane refPlane = _rvtDoc.FamilyCreate.NewReferencePlane( pBubbleEnd, pFreeEnd, XYZ.BasisZ, pViewPlan );
      refPlane.Name = "OffsetH";

      //
      // (2) do the same to add a vertical reference plane.
      //

      // find the ref plane at left, which is aligned to line 3-4
      ReferencePlane refLeft = findElement( typeof( ReferencePlane ), "Left" ) as ReferencePlane;

      // get the bubble and free ends from front ref plane and offset by td.
      //
      p1 = refLeft.BubbleEnd;
      p2 = refLeft.FreeEnd;
      pBubbleEnd = new XYZ(p1.X + tw, p1.Y, p1.Z);
      pFreeEnd = new XYZ(p2.X + tw, p2.Y, p2.Z);

      // create a new reference plane and name it "OffsetV"
      //
      refPlane = _rvtDoc.FamilyCreate.NewReferencePlane( pBubbleEnd, pFreeEnd, XYZ.BasisZ, pViewPlan );
      refPlane.Name = "OffsetV";
    }

    // =================================================================
    //   (1.2) create a simple solid by extrusion with L-shape profile
    // =================================================================
    Extrusion createSolid()
    {
      //
      // (1) define a simple L-shape profile
      //
      //CurveArrArray  pProflie = createBox();
      CurveArrArray pProfile = createProfileLShape();  // Lab2

      //
      // (2) create a sketch plane
      //
      // we need to know the template. If you look at the template (Metric Column.rft) and "Front" view,
      // you will see "Reference Plane" at "Lower Ref. Level". We are going to create an extrusion there.
      // findElement() is a helper function that find an element of the given type and name.  see below.
      //
      ReferencePlane pRefPlane = findElement( typeof( ReferencePlane ), "Reference Plane" ) as ReferencePlane;  // need to know from the template
      SketchPlane pSketchPlane = _rvtDoc.FamilyCreate.NewSketchPlane( pRefPlane.Plane );

      // (3) height of the extrusion
      //
      // same as profile, you will need to know your template. unlike UI, the alightment will not adjust the geometry.
      // You will need to have the exact location in order to set alignment.
      // Here we hard code for simplicity. 4000 is the distance between Lower and Upper Ref. Level.
      // as an exercise, try changing those values and see how it behaves.
      //
      double dHeight = mmToFeet( 4000.0 );

      // (4) create an extrusion here. at this point. just an box, nothing else.
      //
      bool bIsSolid = true;  // as oppose to void.
      return _rvtDoc.FamilyCreate.NewExtrusion( bIsSolid, pProfile, pSketchPlane, dHeight );
    }

    // ===========================================
    //   (1.2a) create a simple L-shaped profile
    // ===========================================
    CurveArrArray createProfileLShape()
    {
      //
      // define a simple L-shape profile
      //
      //  5 tw 4
      //   +-+
      //   | | 3          h = height
      // d | +---+ 2
      //   +-----+ td
      //  0        1
      //  6  w
      //

      // sizes (hard coded for simplicity)
      // note: these need to match reference plane. otherwise, alignment won't work.
      // as an exercise, try changing those values and see how it behaves.
      //
      double w = mmToFeet( 600.0 ); // those are hard coded for simplicity here. in practice, you may want to find out from the references)
      double d = mmToFeet( 600.0 );
      double tw = mmToFeet( 150.0 ); // thickness added for Lab2
      double td = mmToFeet( 150.0 );

      // define vertices
      //
      const int nVerts = 6; // the number of vertices

      XYZ[] pts = new XYZ[] {
        new XYZ(-w / 2.0, -d / 2.0, 0.0),
        new XYZ(w / 2.0, -d / 2.0, 0.0),
        new XYZ(w / 2.0, (-d / 2.0) + td, 0.0),
        new XYZ((-w / 2.0) + tw, (-d / 2.0) + td, 0.0),
        new XYZ((-w / 2.0) + tw, d / 2.0, 0.0),
        new XYZ(-w / 2.0, d / 2.0, 0.0),
        new XYZ(-w / 2.0, -d / 2.0, 0.0) };  // the last one is to make the loop simple

      // define a loop. define individual edges and put them in a curveArray
      //
      CurveArray pLoop = _rvtApp.Create.NewCurveArray();
      for( int i = 0; i < nVerts; ++i )
      {
        Line line = _rvtApp.Create.NewLineBound( pts[i], pts[i + 1] );
        pLoop.Append( line );
      }

      // then, put the loop in the curveArrArray as a profile
      //
      CurveArrArray pProfile = _rvtApp.Create.NewCurveArrArray();
      pProfile.Append( pLoop );
      // if we come here, we have a profile now.

      return pProfile;
    }

    // ==============================================
    //   (1.2b) create a simple rectangular profile
    // ==============================================
    CurveArrArray createProfileRectangle()
    {
      //
      // define a simple rectangular profile
      //
      //  3     2
      //   +---+
      //   |   | d    h = height
      //   +---+
      //  0     1
      //  4  w
      //

      // sizes (hard coded for simplicity)
      // note: these need to match reference plane. otherwise, alignment won't work.
      // as an exercise, try changing those values and see how it behaves.
      //
      double w = mmToFeet( 600.0 );
      double d = mmToFeet( 600.0 );

      // define vertices
      //
      const int nVerts = 4; // the number of vertices

      XYZ[] pts = new XYZ[] {
        new XYZ(-w / 2.0, -d / 2.0, 0.0),
        new XYZ(w / 2.0, -d / 2.0, 0.0),
        new XYZ(w / 2.0, d / 2.0, 0.0),
        new XYZ(-w / 2.0, d / 2.0, 0.0),
        new XYZ(-w / 2.0, -d / 2.0, 0.0) };

      // define a loop. define individual edges and put them in a curveArray
      //
      CurveArray pLoop = _rvtApp.Create.NewCurveArray();
      for( int i = 0; i < nVerts; ++i )
      {
        Line line = _rvtApp.Create.NewLineBound( pts[i], pts[i + 1] );
        pLoop.Append( line );
      }

      // then, put the loop in the curveArrArray as a profile
      //
      CurveArrArray pProfile = _rvtApp.Create.NewCurveArrArray();
      pProfile.Append( pLoop );
      // if we come here, we have a profile now.

      return pProfile;
    }

    // ======================================
    //   (2.1) add alignments
    // ======================================
    void addAlignments( Extrusion pBox )
    {
      //
      // (1) we want to constrain the upper face of the column to the "Upper Ref Level"
      //

      // which direction are we looking at?
      //
      View pView = findElement( typeof( View ), "Front" ) as View;

      // find the upper ref level
      // findElement() is a helper function. see below.
      //
      Level upperLevel = findElement( typeof( Level ), "Upper Ref Level" ) as Level;
      Reference ref1 = upperLevel.PlaneReference;

      // find the face of the box
      // findFace() is a helper function. see below.
      //
      PlanarFace upperFace = findFace( pBox, new XYZ( 0.0, 0.0, 1.0 ) ); // find a face whose normal is z-up.
      Reference ref2 = upperFace.Reference;

      // create alignments
      //
      _rvtDoc.FamilyCreate.NewAlignment( pView, ref1, ref2 );

      //
      // (2) do the same for the lower level
      //

      // find the lower ref level
      // findElement() is a helper function. see below.
      //
      Level lowerLevel = findElement( typeof( Level ), "Lower Ref. Level" ) as Level;
      Reference ref3 = lowerLevel.PlaneReference;

      // find the face of the box
      // findFace() is a helper function. see below.
      PlanarFace lowerFace = findFace( pBox, new XYZ( 0.0, 0.0, -1.0 ) ); // find a face whose normal is z-down.
      Reference ref4 = lowerFace.Reference;

      // create alignments
      //
      _rvtDoc.FamilyCreate.NewAlignment( pView, ref3, ref4 );

      //
      // (3)  same idea for the Right/Left/Front/Back
      //
      // get the plan view
      // note: same name maybe used for different view types. either one should work.
      View pViewPlan = findElement( typeof( ViewPlan ), "Lower Ref. Level" ) as View;

      // find reference planes
      ReferencePlane refRight = findElement( typeof( ReferencePlane ), "Right" ) as ReferencePlane;
      ReferencePlane refLeft = findElement( typeof( ReferencePlane ), "Left" ) as ReferencePlane;
      ReferencePlane refFront = findElement( typeof( ReferencePlane ), "Front" ) as ReferencePlane;
      ReferencePlane refBack = findElement( typeof( ReferencePlane ), "Back" ) as ReferencePlane;
      ReferencePlane refOffsetV = findElement( typeof( ReferencePlane ), "OffsetV" ) as ReferencePlane; // added for L-shape
      ReferencePlane refOffsetH = findElement( typeof( ReferencePlane ), "OffsetH" ) as ReferencePlane; // added for L-shape

      // find the face of the box
      // Note: findFace need to be enhanced for this as face normal is not enough to determine the face.
      //
      PlanarFace faceRight = findFace( pBox, new XYZ( 1.0, 0.0, 0.0 ), refRight ); // modified for L-shape
      PlanarFace faceLeft = findFace( pBox, new XYZ( -1.0, 0.0, 0.0 ) );
      PlanarFace faceFront = findFace( pBox, new XYZ( 0.0, -1.0, 0.0 ) );
      PlanarFace faceBack = findFace( pBox, new XYZ( 0.0, 1.0, 0.0 ), refBack ); // modified for L-shape
      PlanarFace faceOffsetV = findFace( pBox, new XYZ( 1.0, 0.0, 0.0 ), refOffsetV ); // added for L-shape
      PlanarFace faceOffsetH = findFace( pBox, new XYZ( 0.0, 1.0, 0.0 ), refOffsetH ); // added for L-shape

      // create alignments
      //
      _rvtDoc.FamilyCreate.NewAlignment( pViewPlan, refRight.Reference, faceRight.Reference );
      _rvtDoc.FamilyCreate.NewAlignment( pViewPlan, refLeft.Reference, faceLeft.Reference );
      _rvtDoc.FamilyCreate.NewAlignment( pViewPlan, refFront.Reference, faceFront.Reference );
      _rvtDoc.FamilyCreate.NewAlignment( pViewPlan, refBack.Reference, faceBack.Reference );
      _rvtDoc.FamilyCreate.NewAlignment( pViewPlan, refOffsetV.Reference, faceOffsetV.Reference ); // added for L-shape
      _rvtDoc.FamilyCreate.NewAlignment( pViewPlan, refOffsetH.Reference, faceOffsetH.Reference ); // added for L-shape
    }

    // ======================================
    //   (3.1) add parameters
    // ======================================
    void addParameters()
    {
      FamilyManager mgr = _rvtDoc.FamilyManager;

      // API parameter group for Dimension is PG_GEOMETRY:
      //
      FamilyParameter paramTw = mgr.AddParameter( 
        "Tw", BuiltInParameterGroup.PG_GEOMETRY, 
        ParameterType.Length, false );

      FamilyParameter paramTd = mgr.AddParameter( 
        "Td", BuiltInParameterGroup.PG_GEOMETRY, 
        ParameterType.Length, false );

      // set initial values:
      //
      double tw = mmToFeet( 150.0 );
      double td = mmToFeet( 150.0 );
      mgr.Set( paramTw, tw );
      mgr.Set( paramTd, td );
    }

    // ======================================
    //   (3.2) add dimensions
    // ======================================
    void addDimensions()
    {
      // find the plan view
      //
      View pViewPlan = findElement( typeof( ViewPlan ), "Lower Ref. Level" ) as View;

      // find reference planes
      //
      ReferencePlane refLeft = findElement( typeof( ReferencePlane ), "Left" ) as ReferencePlane;
      ReferencePlane refFront = findElement( typeof( ReferencePlane ), "Front" ) as ReferencePlane;
      ReferencePlane refOffsetV = findElement( typeof( ReferencePlane ), "OffsetV" ) as ReferencePlane; // OffsetV is added for L-shape
      ReferencePlane refOffsetH = findElement( typeof( ReferencePlane ), "OffsetH" ) as ReferencePlane; // OffsetH is added for L-shape

      //
      // (1)  add dimension between the reference planes 'Left' and 'OffsetV', and label it as 'Tw'
      //

      // define a dimension line
      //
      XYZ p0 = refLeft.FreeEnd;
      XYZ p1 = refOffsetV.FreeEnd;
      Line pLine = _rvtApp.Create.NewLineBound( p0, p1 );

      // define references
      //
      ReferenceArray pRefArray = new ReferenceArray();
      pRefArray.Append( refLeft.Reference );
      pRefArray.Append( refOffsetV.Reference );

      // create a dimension
      //
      Dimension pDimTw = _rvtDoc.FamilyCreate.NewDimension( pViewPlan, pLine, pRefArray );

      // add label to the dimension
      //
      FamilyParameter paramTw = _rvtDoc.FamilyManager.get_Parameter( "Tw" );
      pDimTw.Label = paramTw;

      //
      // (2)  do the same for dimension between 'Front' and 'OffsetH', and lable it as 'Td'
      //

      // define a dimension line
      //
      p0 = refFront.FreeEnd;
      p1 = refOffsetH.FreeEnd;
      pLine = _rvtApp.Create.NewLineBound( p0, p1 );

      // define references
      //
      pRefArray = new ReferenceArray();
      pRefArray.Append( refFront.Reference );
      pRefArray.Append( refOffsetH.Reference );

      // create a dimension
      //
      Dimension pDimTd = _rvtDoc.FamilyCreate.NewDimension( pViewPlan, pLine, pRefArray );

      // add label to the dimension
      //
      FamilyParameter paramTd = _rvtDoc.FamilyManager.get_Parameter( "Td" );
      pDimTd.Label = paramTd;
    }

    // ======================================
    //  (3.3) add types
    // ======================================
    void addTypes()
    {
      // addType(name, Width, Depth)
      //
      //addType("600x900", 600.0, 900.0)
      //addType("1000x300", 1000.0, 300.0)
      //addType("600x600", 600.0, 600.0)

      // addType(name, Width, Depth, Tw, Td)
      //
      addType( "600x900", 600.0, 900.0, 150.0, 225.0 );
      addType( "1000x300", 1000.0, 300.0, 250.0, 75.0 );
      addType( "600x600", 600.0, 600.0, 150.0, 150.0 );
    }

    // add one type (version 2)
    //
    void addType( string name, double w, double d, double tw, double td )
    {
      // get the family manager from the current doc
      FamilyManager pFamilyMgr = _rvtDoc.FamilyManager;

      // add new types with the given name
      //
      FamilyType type1 = pFamilyMgr.NewType( name );

      // look for 'Width' and 'Depth' parameters and set them to the given value
      //

      // first 'Width'
      //
      FamilyParameter paramW = pFamilyMgr.get_Parameter( "Width" );
      double valW = mmToFeet( w );
      if( paramW != null )
      {
        pFamilyMgr.Set( paramW, valW );
      }

      // same idea for 'Depth'
      //
      FamilyParameter paramD = pFamilyMgr.get_Parameter( "Depth" );
      double valD = mmToFeet( d );
      if( paramD != null )
      {
        pFamilyMgr.Set( paramD, valD );
      }

      // let's set "Tw' and 'Td'
      //
      FamilyParameter paramTw = pFamilyMgr.get_Parameter( "Tw" );
      double valTw = mmToFeet( tw );
      if( paramTw != null )
      {
        pFamilyMgr.Set( paramTw, valTw );
      }
      FamilyParameter paramTd = pFamilyMgr.get_Parameter( "Td" );
      double valTd = mmToFeet( td );
      if( paramTd != null )
      {
        pFamilyMgr.Set( paramTd, valTd );
      }
    }


    // add one type (version 1)
    //
    void addType( string name, double w, double d )
    {
      // get the family manager from the current doc
      FamilyManager pFamilyMgr = _rvtDoc.FamilyManager;

      // add new types with the given name
      //
      FamilyType type1 = pFamilyMgr.NewType( name );

      // look for 'Width' and 'Depth' parameters and set them to the given value
      //
      // first 'Width'
      //
      FamilyParameter paramW = pFamilyMgr.get_Parameter( "Width" );
      double valW = mmToFeet( w );
      if( paramW != null )
      {
        pFamilyMgr.Set( paramW, valW );
      }

      // same idea for 'Depth'
      //
      FamilyParameter paramD = pFamilyMgr.get_Parameter( "Depth" );
      double valD = mmToFeet( d );
      if( paramD != null )
      {
        pFamilyMgr.Set( paramD, valD );
      }
    }

    void modifyFamilyParamValue()
    {
      FamilyManager mgr = _rvtDoc.FamilyManager;

      FamilyParameter [] a = new FamilyParameter[] {
        mgr.get_Parameter( "Width" ),
        mgr.get_Parameter( "Depth" )
      };

      foreach( FamilyType t in mgr.Types )
      {
        mgr.CurrentType = t;
        foreach( FamilyParameter fp in a )
        {
          if( t.HasValue( fp ) )
          {
            double x = ( double ) t.AsDouble( fp );
            mgr.Set( fp, 2.0 * x );
          }
        }
      }
    }

    #region Helper Functions

    // ===============================================================
    // helper function: given a solid, find a planar 
    // face with the given normal (version 2)
    // this is a slightly enhaced version of the previous 
    // version and checks if the face is on the given reference plane.
    // ===============================================================
    PlanarFace findFace( Extrusion pBox, XYZ normal, ReferencePlane refPlane )
    {
      // get the geometry object of the given element
      //
      Options op = new Options();
      op.ComputeReferences = true;
      GeometryObjectArray geomObjs = pBox.get_Geometry( op ).Objects;

      // loop through the array and find a face with the given normal
      //
      foreach( GeometryObject geomObj in geomObjs )
      {
        if( geomObj is Solid )  // solid is what we are interested in.
        {
          Solid pSolid = geomObj as Solid;
          FaceArray faces = pSolid.Faces;
          foreach( Face pFace in faces )
          {
            PlanarFace pPlanarFace = (PlanarFace) pFace;
            // check to see if they have same normal
            if( ( pPlanarFace != null ) && pPlanarFace.Normal.IsAlmostEqualTo( normal ) )
            {
              // additionally, we want to check if the face is on the reference plane
              //
              XYZ p0 = refPlane.BubbleEnd;
              XYZ p1 = refPlane.FreeEnd;
              Line pCurve = _rvtApp.Create.NewLineBound( p0, p1 );
              if( pPlanarFace.Intersect( pCurve ) == SetComparisonResult.Subset )
              {
                return pPlanarFace; // we found the face
              }
            }
          }
        }

        // will come back later as needed.
        //
        //else if (geomObj is Instance)
        //{
        //}
        //else if (geomObj is Curve)
        //{
        //}
        //else if (geomObj is Mesh)
        //{
        //}
      }

      // if we come here, we did not find any.
      return null;
    }

    // =============================================================
    //   helper function: find a planar face with the given normal
    // =============================================================
    PlanarFace findFace( Extrusion pBox, XYZ normal )
    {
      // get the geometry object of the given element
      //
      Options op = new Options();
      op.ComputeReferences = true;
      GeometryObjectArray geomObjs = pBox.get_Geometry( op ).Objects;

      // loop through the array and find a face with the given normal
      //
      foreach( GeometryObject geomObj in geomObjs )
      {
        if( geomObj is Solid )  // solid is what we are interested in.
        {
          Solid pSolid = geomObj as Solid;
          FaceArray faces = pSolid.Faces;
          foreach( Face pFace in faces )
          {
            PlanarFace pPlanarFace = (PlanarFace) pFace;
            if( ( pPlanarFace != null ) && pPlanarFace.Normal.IsAlmostEqualTo( normal ) ) // we found the face
            {
              return pPlanarFace;
            }
          }
        }
        // will come back later as needed.
        //
        //else if (geomObj is Instance)
        //{
        //}
        //else if (geomObj is Curve)
        //{
        //}
        //else if (geomObj is Mesh)
        //{
        //}
      }

      // if we come here, we did not find any.
      return null;
    }

    // ==================================================================================
    //   helper function: find an element of the given type and the name.
    //   You can use this, for example, to find Reference or Level with the given name.
    // ==================================================================================
    Element findElement( Type targetType, string targetName )
    {
      // get the elements of the given type
      //
      FilteredElementCollector collector = new FilteredElementCollector(_rvtDoc);
      collector.WherePasses(new ElementClassFilter(targetType));

      // parse the collection for the given name
      // using LINQ query here. 
      // 
      var targetElems = from element in collector where element.Name.Equals(targetName) select element;
      List<Element> elems = targetElems.ToList<Element>();

      if (elems.Count > 0)
      {  // we should have only one with the given name. 
          return elems[0];
      }

      // cannot find it.
      return null;
    }

    // ===============================================
    //   helper function: convert millimeter to feet
    // ===============================================
    double mmToFeet( double mmVal )
    {
      return mmVal / 304.8;
    }

    #endregion // Helper Functions
  }
}
