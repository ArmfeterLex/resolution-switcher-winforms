using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace WindowsFormsApp9x
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = Screen.PrimaryScreen.Bounds.Width + "x" + Screen.PrimaryScreen.Bounds.Height;
        }

        [DllImport("user32.dll")]
        public static extern int EnumDisplaySettings(string deviceName, int modeNum, ref DEVMODE devMode);
        [DllImport("user32.dll")]
        public static extern int ChangeDisplaySettings(ref DEVMODE devMode, int flags);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct DEVMODE
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
            public int dmICMMethod;
            public int dmICMIntent;
            public int dmMediaType;
            public int dmDitherType;
            public int dmReserved1;
            public int dmReserved2;
            public int dmPanningWidth;
            public int dmPanningHeight;
        }

        public const int ENUM_CURRENT_SETTINGS = -1;
        public const int CDS_UPDATEREGISTRY = 1;
        public const int CDS_TEST = 2;
        public const int DISP_CHANGE_SUCCESSFUL = 0;
        public const int DISP_CHANGE_RESTART = 1;
        public const int DISP_CHANGE_FAILED = -1;

        private int clickCount = 0;

        private void button2_Click(object sender, EventArgs e)
        {
            int iWidth, iHeight;
            switch (clickCount % 3)
            {
                case 0:
                    iWidth = 640;
                    iHeight = 480;
                    break;
                case 1:
                    iWidth = 800;
                    iHeight = 600;
                    break;
                case 2:
                default:
                    iWidth = 1920;
                    iHeight = 1080;
                    break;
            }

            DEVMODE dm = new DEVMODE();
            dm.dmSize = (short)Marshal.SizeOf(dm);
            if (EnumDisplaySettings(null, ENUM_CURRENT_SETTINGS, ref dm) != 0)
            {
                dm.dmPelsWidth = iWidth;
                dm.dmPelsHeight = iHeight;
                dm.dmFields = 0x00080000 | 0x00100000;

                int iRet = ChangeDisplaySettings(ref dm, CDS_TEST);
                if (iRet == DISP_CHANGE_FAILED)
                {
                    MessageBox.Show("Не получается поменять разрешение");
                }
                else
                {
                    iRet = ChangeDisplaySettings(ref dm, CDS_UPDATEREGISTRY);
                    switch (iRet)
                    {
                        case DISP_CHANGE_SUCCESSFUL:
                            break;
                        case DISP_CHANGE_RESTART:
                            MessageBox.Show("Нужно перезагрузить компьютер");
                            break;
                        default:
                            MessageBox.Show("Ошибка при изменении разрешения");
                            break;
                    }
                }
            }
            clickCount++;
        }
    }
}
