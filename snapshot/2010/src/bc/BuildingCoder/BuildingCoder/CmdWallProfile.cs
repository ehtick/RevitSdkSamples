#region Header
//
// CmdWallProfile.cs - determine wall
// elevation profile boundary loop polygons
//
// Copyright (C) 2008-2010 by Jeremy Tammik,
// Autodesk Inc. All rights reserved.
//
#endregion // Header

#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Geometry;
using RvtElement = Autodesk.Revit.Element;
using GeoElement = Autodesk.Revit.Geometry.Element;
using CmdResult
  = Autodesk.Revit.IExternalCommand.Result;
#endregion // Namespaces

namespace BuildingCoder
{
  class CmdWallProfile : IExternalCommand
  {
    /// <summary>
    /// Offset the generated boundary polygon loop
    /// model lines outwards to separate them from
    /// the wall edge, measured in feet.
    /// </summary>
    const double _offset = 1.0;

    /// <summary>
    /// Determine the elevation boundary profile
    /// polygons of the exterior vertical planar
    /// face of the given wall solid.
    /// </summary>
    /// <param name="polygons">Return polygonal boundary
    /// loops of exterior vertical planar face, i.e.
    /// profile of wall elevation incl. holes</param>
    /// <param name="solid">Input solid</param>
    /// <param name="w">Vector pointing along
    /// wall centre line</param>
    /// <param name="w">Vector pointing towards
    /// exterior wall face</param>
    /// <returns>False if no exterior vertical
    /// planar face was found, else true</returns>
    static bool GetProfile(
      List<List<XYZ>> polygons,
      Solid solid,
      XYZ v,
      XYZ w )
    {
      double d, dmax = 0;
      PlanarFace outermost = null;
      FaceArray faces = solid.Faces;
      foreach( Face f in faces )
      {
        PlanarFace pf = f as PlanarFace;
        if( null != pf
          && Util.IsVertical( pf )
          && Util.IsZero( v.Dot( pf.Normal ) ) )
        {
          d = pf.Origin.Dot( w );
          if( ( null == outermost )
            || ( dmax < d ) )
          {
            outermost = pf;
            dmax = d;
          }
        }
      }

      if( null != outermost )
      {
        XYZ voffset = _offset * w;
        XYZ p, q = XYZ.Zero;
        bool first;
        int i, n;
        EdgeArrayArray loops = outermost.EdgeLoops;
        foreach( EdgeArray loop in loops )
        {
          List<XYZ> vertices = new List<XYZ>();
          first = true;
          foreach( Edge e in loop )
          {
            XYZArray points = e.Tessellate();
            p = points.get_Item( 0 );
            if( !first )
            {
              Debug.Assert( p.AlmostEqual( q ),
                "expected subsequent start point"
                + " to equal previous end point" );
            }
            n = points.Size;
            q = points.get_Item( n - 1 );
            for( i = 0; i < n - 1; ++i )
            {
              XYZ a = points.get_Item( i );
              a += voffset;
              vertices.Add( a );
            }
          }
          q += voffset;
          Debug.Assert( q.AlmostEqual( vertices[0] ),
            "expected last end point to equal"
            + " first start point" );
          polygons.Add( vertices );
        }
      }
      return null != outermost;
    }

    /// <summary>
    /// Return all wall profile boundary loop polygons
    /// for the given walls, offset out from the outer
    /// face of the wall by a certain amount.
    /// </summary>
    static public List<List<XYZ>> GetWallProfilePolygons(
      Application app,
      List<RvtElement> walls )
    {
      XYZ p, q, v, w;
      Options opt = app.Create.NewGeometryOptions();
      List<List<XYZ>> polygons = new List<List<XYZ>>();

      foreach( Wall wall in walls )
      {
        string desc = Util.ElementDescription( wall );

        LocationCurve curve
          = wall.Location as LocationCurve;

        if( null == curve )
        {
          throw new Exception( desc
            + ": No wall curve found." );
        }
        p = curve.Curve.get_EndPoint( 0 );
        q = curve.Curve.get_EndPoint( 1 );
        v = q - p;
        v = v.Normalized;
        w = XYZ.BasisZ.Cross( v ).Normalized;
        if( wall.Flipped ) { w = -w; }

        GeoElement geo = wall.get_Geometry( opt );
        GeometryObjectArray objects = geo.Objects;
        foreach( GeometryObject obj in objects )
        {
          Solid solid = obj as Solid;
          if( solid != null )
          {
            GetProfile( polygons, solid, v, w );
          }
        }
      }
      return polygons;
    }

    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;

      List<RvtElement> walls = new List<RvtElement>();
      if( !Util.GetSelectedElementsOrAll(
        walls, doc, typeof( Wall ) ) )
      {
        Selection sel = doc.Selection;
        message = ( 0 < sel.Elements.Size )
          ? "Please select some wall elements."
          : "No wall elements found.";
        return CmdResult.Failed;
      }

      List<List<XYZ>> polygons
        = GetWallProfilePolygons( app, walls );

      int n = polygons.Count;

      Debug.Print(
        "{0} boundary loop{1} found.",
        n, Util.PluralSuffix( n ) );

      Creator creator = new Creator( app );
      creator.DrawPolygons( polygons );

      return CmdResult.Succeeded;
    }
  }
}
