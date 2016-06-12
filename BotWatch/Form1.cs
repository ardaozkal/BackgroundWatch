using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BotWatch
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            wc = new WebClient();
        }

        public enum GWL
        {
            ExStyle = -20
        }

        public enum WS_EX
        {
            Transparent = 0x20,
            Layered = 0x80000
        }

        public enum LWA
        {
            ColorKey = 0x1,
            Alpha = 0x2
        }

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        public static extern int GetWindowLong(IntPtr hWnd, GWL nIndex);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        public static extern int SetWindowLong(IntPtr hWnd, GWL nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetLayeredWindowAttributes")]
        public static extern bool SetLayeredWindowAttributes(IntPtr hWnd, int crKey, byte alpha, LWA dwFlags);

        private void axWindowsMediaPlayer1_MouseUpEvent(object sender, AxWMPLib._WMPOCXEvents_MouseUpEvent e)
        {
            var result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                axWindowsMediaPlayer1.URL = openFileDialog1.FileName;
                timer1.Enabled = true;
                timer2.Enabled = true;
                timer1_Tick(null, null);
            }
        }
        WebClient wc;
        private void timer1_Tick(object sender, EventArgs e)
        {
            Cursor.Hide();
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            axWindowsMediaPlayer1.Size = this.Size;

            var wl = GetWindowLong(this.Handle, GWL.ExStyle);
            wl = wl | 0x80000 | 0x20;
            SetWindowLong(this.Handle, GWL.ExStyle, wl);
            SetLayeredWindowAttributes(this.Handle, 0, 255, LWA.Alpha);
            
        }

        string serverlink = "http://example.com";

        private void timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                var status = wc.DownloadString(serverlink+"/playstatus.txt");
                switch (status)
                {
                    case "mute":
                        axWindowsMediaPlayer1.settings.volume = 0;
                        break;
                    case "unmute":
                        axWindowsMediaPlayer1.settings.volume = 50;
                        break;
                    case "play":
                        axWindowsMediaPlayer1.Ctlcontrols.play();
                        break;
                    case "pause":
                        axWindowsMediaPlayer1.Ctlcontrols.pause();
                        break;
                    case "close":
                        Cursor.Show();
                        this.Close();
                        break;
                }
                wc.DownloadString(serverlink+"/setstatus.php?status=none");
            }
            catch
            {

            }
        }
    }
}
