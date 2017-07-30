﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.ServiceProcess;
using Microsoft.Win32;
using System.IO;


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

                    Process temp = proc;

                    proc = null;

                    temp.Kill();

                } else {
                    Console.WriteLine( "process null. No process has been created." );
                }

            }
           
        }

        public void disconnect()
        {
            destroy();
        
        }
    }
}
