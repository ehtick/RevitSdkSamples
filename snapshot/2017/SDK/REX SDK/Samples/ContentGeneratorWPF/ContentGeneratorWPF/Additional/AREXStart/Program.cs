using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Autodesk.REX.Framework;

namespace REX.Start
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.AssemblyResolve += new ResolveEventHandler(currentDomain_AssemblyResolve);


            RunExtension(@"c:\my documents\visual studio 2010\Projects\ContentGeneratorWPF\ContentGeneratorWPF\bin\Debug\ContentGeneratorWPF.dll", "2017");
        }

        static System.Reflection.Assembly currentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            return Autodesk.REX.Framework.REXAssemblies.Resolve(sender, args, System.Reflection.Assembly.GetExecutingAssembly());
        }

        static void RunExtension(string FullPath, string VersionName)
        {
            REXApplicationInstance applicationInstance = new REXApplicationInstance();
            if (applicationInstance.LoadExtension(FullPath, "REX.ContentGeneratorWPF.Application"))
            {
                REXContext context = new REXContext();

                context.Control.Mode = REXMode.Dialog;
                context.Control.ControlMode = REXControlMode.ModalDialog;
                context.Product.Type = REXInterfaceType.Common;
                context.Control.Parent = null;

                REXEnvironment environment = new REXEnvironment(VersionName);
                if (environment.LoadEngine(ref context))
                {
                    if (applicationInstance.Extension.Create(ref context))
                        applicationInstance.Extension.Show();
                }
            }
        }
    }
}