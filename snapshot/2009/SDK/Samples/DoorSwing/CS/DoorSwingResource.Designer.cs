﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.832
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Revit.SDK.Samples.DoorSwing.CS {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class DoorSwingResource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal DoorSwingResource() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Revit.SDK.Samples.DoorSwing.CS.DoorSwingResource", typeof(DoorSwingResource).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to L.
        /// </summary>
        internal static string LeftDoor {
            get {
                return ResourceManager.GetString("LeftDoor", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to R.
        /// </summary>
        internal static string RightDoor {
            get {
                return ResourceManager.GetString("RightDoor", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Double.
        /// </summary>
        internal static string TwoLeaf {
            get {
                return ResourceManager.GetString("TwoLeaf", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Double-R.
        /// </summary>
        internal static string TwoLeafActiveLeafLeft {
            get {
                return ResourceManager.GetString("TwoLeafActiveLeafLeft", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Double-L.
        /// </summary>
        internal static string TwoLeafActiveLeafRight {
            get {
                return ResourceManager.GetString("TwoLeafActiveLeafRight", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to N/A.
        /// </summary>
        internal static string Undefined {
            get {
                return ResourceManager.GetString("Undefined", resourceCulture);
            }
        }
    }
}