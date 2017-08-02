using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;

[StructLayout(LayoutKind.Sequential)]
public struct DEVMODE1
{
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    public string dmDeviceName;
    public short dmSpecVersion;
    public short dmDriverVersion;
    public short dmSize;
    public short dmDriverExtra;
    public int dmFields;

    public short dmOrientation;
    public short dmPaperSize;
    public short dmPaperLength;
    public short dmPaperWidth;

    public short dmScale;
    public short dmCopies;
    public short dmDefaultSource;
    public short dmPrintQuality;
    public short dmColor;
    public short dmDuplex;
    public short dmYResolution;
    public short dmTTOption;
    public short dmCollate;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    public string dmFormName;
    public short dmLogPixels;
    public short dmBitsPerPel;
    public int dmPelsWidth;
    public int dmPelsHeight;

    public int dmDisplayFlags;
    public int dmDisplayFrequency;
    public int dmDisplayOrientation;
    public int dmICMMethod;
    public int dmICMIntent;
    public int dmMediaType;
    public int dmDitherType;
    public int dmReserved1;
    public int dmReserved2;

    public int dmPanningWidth;
    public int dmPanningHeight;
};



class User_32
{
    [DllImport("user32.dll")]
    public static extern int EnumDisplaySettings(string deviceName, int modeNum, ref DEVMODE1 devMode);
    [DllImport("user32.dll")]
    public static extern int ChangeDisplaySettings(ref DEVMODE1 devMode, int flags);


    public const int ENUM_CURRENT_SETTINGS = -1;
    public const int CDS_UPDATEREGISTRY = 0x01;
    public const int CDS_TEST = 0x02;
    public const int DISP_CHANGE_SUCCESSFUL = 0;
    public const int DISP_CHANGE_RESTART = 1;
    public const int DISP_CHANGE_FAILED = -1;
}


namespace SharePC
{
    class CResolution
    {
        public int selectedIndexOfCurrentResolution;
        public CResolution()
        {

        }
        public CResolution(int a, int b)
        {
            Screen screen = Screen.PrimaryScreen;


            int iWidth = a;
            int iHeight = b;


            DEVMODE1 dm = new DEVMODE1();
            dm.dmDeviceName = new String(new char[32]);
            dm.dmFormName = new String(new char[32]);
            dm.dmSize = (short)Marshal.SizeOf(dm);

            getMaximumSupportedResolution();

            if (0 != User_32.EnumDisplaySettings(null, User_32.ENUM_CURRENT_SETTINGS, ref dm))
            {

                dm.dmPelsWidth = iWidth;
                dm.dmPelsHeight = iHeight;

                int iRet = User_32.ChangeDisplaySettings(ref dm, User_32.CDS_TEST);

                if (iRet == User_32.DISP_CHANGE_FAILED)
                {
                    Console.WriteLine("Unable to process your request");
                    Console.WriteLine("Description: Unable To Process Your Request. Sorry For This Inconvenience.");
                }
                else
                {
                    iRet = User_32.ChangeDisplaySettings(ref dm, User_32.CDS_UPDATEREGISTRY);

                    switch (iRet)
                    {
                        case User_32.DISP_CHANGE_SUCCESSFUL:
                            {
                                break;

                                //successfull change
                            }
                        case User_32.DISP_CHANGE_RESTART:
                            {

                                Console.WriteLine("Description: You Need To Reboot For The Change To Happen.\n If You Feel Any Problem After Rebooting Your Machine\nThen Try To Change Resolution In Safe Mode.");
                                break;
                                //windows 9x series you have to restart
                            }
                        default:
                            {

                                Console.WriteLine("Description: Failed To Change The Resolution.");
                                break;
                                //failed to change
                            }
                    }
                }

            }
        }
        public DEVMODE1 getCurrentResolution()
        {
            DEVMODE1 dm = new DEVMODE1();
            dm.dmDeviceName = new String(new char[32]);
            dm.dmFormName = new String(new char[32]);
            dm.dmSize = (short)Marshal.SizeOf(dm);
            if (0 != User_32.EnumDisplaySettings(null, User_32.ENUM_CURRENT_SETTINGS, ref dm))
            {
                /* Console.WriteLine("\t" +
                   "{0} by {1}, " +
                   "{2} bit, " +
                   "{3} degrees, " +
                   "{4} hertz",
                   dm.dmPelsWidth,
                   dm.dmPelsHeight,
                   dm.dmBitsPerPel,
                   dm.dmDisplayOrientation * 90,
                   dm.dmDisplayFrequency);*/
            }
            else
            {
                Console.WriteLine("Unable to process your request");
            }
            return dm;
        }
        public DEVMODE1 getMaximumSupportedResolution()
        {
            DEVMODE1 dm = new DEVMODE1();
            dm.dmDeviceName = new String(new char[32]);
            dm.dmFormName = new String(new char[32]);
            dm.dmSize = (short)Marshal.SizeOf(dm);

            int i = 0;
            while (0 != User_32.EnumDisplaySettings(null, i++, ref dm)) ;

            Console.WriteLine("\t" +
                      "{0} by {1}, " +
                      "{2} bit, " +
                      "{3} degrees, " +
                      "{4} hertz," +
                      "{5} color",
                      dm.dmPelsWidth,
                      dm.dmPelsHeight,
                      dm.dmBitsPerPel,
                      dm.dmDisplayOrientation * 90,
                      dm.dmDisplayFrequency,
                      dm.dmYResolution);

            return dm;
        }
        public void setSupportedResolution(DEVMODE1 dm)
        {
            int iRet = User_32.ChangeDisplaySettings(ref dm, User_32.CDS_TEST);

            if (iRet == User_32.DISP_CHANGE_FAILED)
            {
                Console.WriteLine("Unable to process your request");
                Console.WriteLine("Description: Unable To Process Your Request. Sorry For This Inconvenience.");
            }
            else
            {
                iRet = User_32.ChangeDisplaySettings(ref dm, User_32.CDS_UPDATEREGISTRY);

                switch (iRet)
                {
                    case User_32.DISP_CHANGE_SUCCESSFUL:
                        {
                            break;

                            //successfull change
                        }
                    case User_32.DISP_CHANGE_RESTART:
                        {

                            Console.WriteLine("Description: You Need To Reboot For The Change To Happen.\n If You Feel Any Problem After Rebooting Your Machine\nThen Try To Change Resolution In Safe Mode.");
                            break;
                            //windows 9x series you have to restart
                        }
                    default:
                        {

                            Console.WriteLine("Description: Failed To Change The Resolution.");
                            break;
                            //failed to change
                        }
                }
            }
        }


        public List<DEVMODE1> getSupportedResolutionList()
        {
            List<DEVMODE1> ResolutionList = new List<DEVMODE1>();

            DEVMODE1 CurrentResolution = getCurrentResolution();

            DEVMODE1 dm = new DEVMODE1();
            dm.dmDeviceName = new String(new char[32]);
            dm.dmFormName = new String(new char[32]);
            dm.dmSize = (short)Marshal.SizeOf(dm);

            int previousResolutionX = 0;
            int previousResolutionY = 0;

            int i = 0, k = 0;
            while (0 != User_32.EnumDisplaySettings(null, i++, ref dm))
            {
                if ((dm.dmPelsWidth > previousResolutionX || dm.dmPelsHeight > previousResolutionY) && dm.dmBitsPerPel >= 32)
                {
                    /*  Console.WriteLine("\t" +
                       "{0} by {1}, " +
                       "{2} bit, " +
                       "{3} degrees, " +
                       "{4} hertz," +
                       "{5} color",
                       dm.dmPelsWidth,
                       dm.dmPelsHeight,
                       dm.dmBitsPerPel,
                       dm.dmDisplayOrientation * 90,
                       dm.dmDisplayFrequency,
                       dm.dmYResolution);*/
                    k++;
                    if (dm.dmPelsWidth == CurrentResolution.dmPelsWidth && dm.dmPelsHeight == CurrentResolution.dmPelsHeight && dm.dmBitsPerPel == CurrentResolution.dmBitsPerPel) selectedIndexOfCurrentResolution = k;
                    ResolutionList.Add(dm);

                    previousResolutionX = dm.dmPelsWidth;
                    previousResolutionY = dm.dmPelsHeight;

                    dm = new DEVMODE1();
                    dm.dmDeviceName = new String(new char[32]);
                    dm.dmFormName = new String(new char[32]);
                    dm.dmSize = (short)Marshal.SizeOf(dm);



                }


            }
            return ResolutionList;
        }
    }
}
