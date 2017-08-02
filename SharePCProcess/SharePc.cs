using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.ServiceProcess;
using Microsoft.Win32;
using System.IO;
using System.Threading;

namespace SharePC
{

    public class SharePC : ServiceBase
    {
        public Process proc=null;
        StreamWriter myStreamWriter;

        string controlString = null;
        public SharePC()
        {

          

        }

        void SystemEvents_SessionSwitch(object sender, Microsoft.Win32.SessionSwitchEventArgs e)
        {
            if (e.Reason == SessionSwitchReason.SessionLock)
            {
                destroy();
                Console.WriteLine("Locked");
                //I left my desk
            }
            else if (e.Reason == SessionSwitchReason.SessionUnlock)
            {

                Console.WriteLine("Unlocked");
                //I returned to my desk
            }
        }

        public String GetControlString()
        {
            lock ( this ) {

                if ( null != proc ) {

                    try {

                        Process existing = Process.GetProcessById( proc.Id );

                        if ( null != existing && null != controlString )
                            return controlString;


                    } catch ( Exception e ) {

                        Console.WriteLine( "the error is :" + e );

                    }

                }

                Microsoft.Win32.SystemEvents.SessionSwitch -= new Microsoft.Win32.SessionSwitchEventHandler( SystemEvents_SessionSwitch );

                Microsoft.Win32.SystemEvents.SessionSwitch += new Microsoft.Win32.SessionSwitchEventHandler( SystemEvents_SessionSwitch );

                //string line = "";

                proc = new Process {

                    StartInfo = new ProcessStartInfo {

                        FileName = "RDPServer\\RDPSessionManager.exe",
                        // Arguments = "command line arguments to your executable",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true,
                        RedirectStandardInput = true

                    }
                };

                try {
                    proc.Start();

                } catch ( Exception e ) {
                    Console.WriteLine( "the error is :" + e );
                    throw e;
                }

                myStreamWriter = proc.StandardInput;
                controlString = proc.StandardOutput.ReadLine();
                //Console.WriteLine(line);
                return controlString;
            }

        }

        public void destroy() {

            lock ( this ) {

                if ( proc != null ) {

                    Microsoft.Win32.SystemEvents.SessionSwitch -= new Microsoft.Win32.SessionSwitchEventHandler( SystemEvents_SessionSwitch );

                    try {

                        proc.StandardOutput.Close();
                        myStreamWriter.Close();
                        controlString = null;
                        proc.Close();

                    } catch ( Exception e ) {

                        Console.WriteLine( "the error is :" + e );
                    }

                    try {

                        proc.Kill();

                    } catch {

                    }

                    proc = null;


                } else {
                    Console.WriteLine( "process null. No process has been created." );
                }

            }
           
        }

        public void disconnect()
        {
            destroy();
        
        }

        public void triggerResolutionSwitch()
        {
            CResolution res1 = new CResolution();
            DEVMODE1 dm1 = res1.getCurrentResolution();
            List<DEVMODE1> dmList = res1.getSupportedResolutionList();
            int len = dmList.Count;

            //int selectedIndex = -1;
            
            DEVMODE1 dm2 = dmList[len-1];

            if (dm1.dmPelsWidth == dm2.dmPelsWidth && dm1.dmPelsHeight == dm2.dmPelsHeight)
            {
                if (len - 2 >= 0)
                {
                    dm2 = dmList[len - 2];
                }
                else 
                {
                    dm2 = dmList[0];
                }
            }

            Console.WriteLine("the switched resolution:" + dm2.dmPelsHeight+"x"+dm2.dmPelsWidth);
          
            res1.setSupportedResolution(dm2);
            Thread.Sleep(2000);
            res1.setSupportedResolution(dm1);

        }
    }
}
