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


using System;
using System.Collections.Generic;
using System.Text;

using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Structural;
using Autodesk.Revit.Structural.Enums;

namespace Revit.SDK.Samples.DeckProperties.CS
{
    /// <summary>
    /// Implements the Revit add-in interface IExternalCommand
    /// </summary>
    public class Command : IExternalCommand
    {
        private DeckPropertyForm m_displayForm;
        
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
        public IExternalCommand.Result Execute(Autodesk.Revit.ExternalCommandData commandData,
                                               ref string message,
                                               ElementSet elements)
        {
            // Get the application of revit
            Autodesk.Revit.Application revit = commandData.Application;

            try
            {
                using (m_displayForm = new DeckPropertyForm())
                {
                    foreach (Element element in revit.ActiveDocument.Selection.Elements)
                    {
                        Floor floor = element as Floor;
                        if (floor != null)
                        {
                            DumpSlab(floor);
                        }
                    }
                    m_displayForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                // If any error, store error information in message and return failed
                message = ex.Message;
                return IExternalCommand.Result.Failed;
            }
            // If everything goes well, return succeeded.
            return IExternalCommand.Result.Succeeded;
        }

        /// <summary>
        /// Dump the properties of interest for the slab passed as a parameter
        /// </summary>
        /// <param name="slab"></param>
        private void DumpSlab(Floor slab)
        {
            m_displayForm.WriteLine("Dumping Slab" + slab.Id.Value.ToString());

            Autodesk.Revit.Symbols.FloorType slabType = slab.FloorType;
            
            if (slabType != null)
            {
                foreach (CompoundStructureLayer layer in slabType.CompoundStructure.Layers)
                {
                    if (layer.Function == CompoundStructureLayerFunction.StructuralDeck)
                    {
                        DumbDeck(layer);
                    }
                    else
                    {
                        DumpLayer(layer);
                    }
                }
            }

            m_displayForm.WriteLine(" ");
        }

        /// <summary>
        /// Dump properties specific to a decking layer
        /// </summary>
        /// <param name="deck"></param>
        private void DumbDeck(CompoundStructureLayer deck)
        {
            m_displayForm.WriteLine("Dumping Deck");
       
            if (deck.Material != null)
            {
                // get the deck material object. In this sample all we need to display is the
                // name, but other properties are readily available from the material object.
                Autodesk.Revit.Elements.Material deckMaterial = deck.Material;
                m_displayForm.WriteLine("Deck Material = " + deckMaterial.Name);
            }

            if (deck.DeckProfile != null)
            {
                // the deck profile is actually a family symbol from a family of profiles
                Autodesk.Revit.Symbols.FamilySymbol deckProfile = deck.DeckProfile;

                // firstly display the full name as the user would see it in the user interface
                // this is done in the format Family.Name and then Symbol.Name
                m_displayForm.WriteLine("Deck Profile = " 
                    + deckProfile.Family.Name + " : " + deckProfile.Name);

                // the symbol object also contains parameters that describe how the deck is
                // specified. From these parameters an external application can generate
                // identical decking for analysis purposes
                DumpParameters(deckProfile);
            }
        }

        /// <summary>
        /// A generic parameter display method that displays all the parameters of an element
        /// </summary>
        /// <param name="element"></param>
        private void DumpParameters(Element element)
        {
            foreach (Parameter parameter in element.Parameters)
            {
                string value = "";
                switch (parameter.StorageType)
                {
                    case Autodesk.Revit.Parameters.StorageType.Double:
                        value = parameter.AsDouble().ToString();
                        break;
                    case Autodesk.Revit.Parameters.StorageType.ElementId:
                        value = parameter.AsElementId().Value.ToString();
                        break;
                    case Autodesk.Revit.Parameters.StorageType.String:
                        value = parameter.AsString();
                        break;
                    case Autodesk.Revit.Parameters.StorageType.Integer:
                        value = parameter.AsInteger().ToString();
                        break;
                }

                m_displayForm.WriteLine(parameter.Definition.Name + " = " + value);
            }
        }

        /// <summary>
        /// for non deck layers this method is called and it displays minimal information
        /// about the layer
        /// </summary>
        /// <param name="layer"></param>
        private void DumpLayer(CompoundStructureLayer layer)
        {
            // Display the name of the material. More detailed material properties can
            // be found form the material object
            m_displayForm.WriteLine("Dumping Layer");
            if (layer.Material != null)
            {
                m_displayForm.WriteLine("Layer material = " + layer.Material.Name);
            }

            // display the thickness of the layer in inches.
            m_displayForm.WriteLine("Layer Thickness = " + layer.Thickness.ToString());
        }
    }
}
