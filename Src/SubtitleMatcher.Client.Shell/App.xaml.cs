using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition;
using SubtitlesMatcher.Common;
using SubCenterSubtitlesMatcher;
using System.Diagnostics;
using System.ComponentModel;
using CSUACSelfElevation;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace SubtitleMatcher.Client.Shell
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {



        private static CompositionContainer _container;

        public static CompositionContainer CompositionContainer
        {
            get { return _container; }
        }




        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            SelfElevate();

            InitMefContainer();


            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.Run();
            App.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
        }

        private void SelfElevate()
        {
            //vista or more
            if (Environment.OSVersion.Version.Major >= 6)
            {
                if (!IsProcessElevated() && !IsRunAsAdmin())
                {

                    // Launch itself as administrator
                    ProcessStartInfo proc = new ProcessStartInfo();
                    proc.UseShellExecute = true;
                    proc.WorkingDirectory = Environment.CurrentDirectory;
                    proc.FileName = Process.GetCurrentProcess().MainModule.FileName;
                    proc.Verb = "runas";

                    try
                    {
                        Process.Start(proc);
                    }
                    catch
                    {
                        // The user refused the elevation.
                        // Do nothing and return directly ...
                        return;
                    }

                    Application.Current.Shutdown();  // Quit itself


                }
            }

        }

        internal bool IsRunAsAdmin()
        {
            WindowsIdentity id = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(id);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }



        internal bool IsProcessElevated()
        {
            bool fIsElevated = false;
            SafeTokenHandle hToken = null;
            int cbTokenElevation = 0;
            IntPtr pTokenElevation = IntPtr.Zero;

            try
            {
                // Open the access token of the current process with TOKEN_QUERY.
                if (!NativeMethods.OpenProcessToken(Process.GetCurrentProcess().Handle,
                    NativeMethods.TOKEN_QUERY, out hToken))
                {
                    throw new Win32Exception();
                }

                // Allocate a buffer for the elevation information.
                cbTokenElevation = Marshal.SizeOf(typeof(TOKEN_ELEVATION));
                pTokenElevation = Marshal.AllocHGlobal(cbTokenElevation);
                if (pTokenElevation == IntPtr.Zero)
                {
                    throw new Win32Exception();
                }

                // Retrieve token elevation information.
                if (!NativeMethods.GetTokenInformation(hToken,
                    TOKEN_INFORMATION_CLASS.TokenElevation, pTokenElevation,
                    cbTokenElevation, out cbTokenElevation))
                {
                    // When the process is run on operating systems prior to Windows 
                    // Vista, GetTokenInformation returns false with the error code 
                    // ERROR_INVALID_PARAMETER because TokenElevation is not supported 
                    // on those operating systems.
                    throw new Win32Exception();
                }

                // Marshal the TOKEN_ELEVATION struct from native to .NET object.
                TOKEN_ELEVATION elevation = (TOKEN_ELEVATION)Marshal.PtrToStructure(
                    pTokenElevation, typeof(TOKEN_ELEVATION));

                // TOKEN_ELEVATION.TokenIsElevated is a non-zero value if the token 
                // has elevated privileges; otherwise, a zero value.
                fIsElevated = (elevation.TokenIsElevated != 0);
            }
            finally
            {
                // Centralized cleanup for all allocated resources. 
                if (hToken != null)
                {
                    hToken.Close();
                    hToken = null;
                }
                if (pTokenElevation != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pTokenElevation);
                    pTokenElevation = IntPtr.Zero;
                    cbTokenElevation = 0;
                }
            }

            return fIsElevated;
        }

        void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("Fatal Error: see event viewer for details");
            EventLog.WriteEntry("Application", e.Exception.ToString(), EventLogEntryType.Error);
        }

        private void InitMefContainer()
        {
            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new DirectoryCatalog(Environment.CurrentDirectory));
            _container = new CompositionContainer(catalog);

        }
    }
}
