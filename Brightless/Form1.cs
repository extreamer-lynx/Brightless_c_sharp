using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Collections;
using System.Diagnostics;



namespace Brightless
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        PowerStatus status = SystemInformation.PowerStatus;

        private void ShowPowerStatus()
        {
            label6.Text = status.BatteryChargeStatus.ToString();
            if (status.BatteryFullLifetime == -1)
                label8.Text = "Unknown";
            else
                label8.Text =
                    status.BatteryFullLifetime.ToString();
            label7.Text = status.BatteryLifePercent.ToString("P0");
            label4.Text = status.PowerLineStatus.ToString();
        }

        double GetBatteryPercent()
        {
            return status.BatteryLifePercent;
        }

        short val;

        private void SwitchImg()
        {
            Image img = Image.FromFile(@"img\8.png");
            if (status.BatteryChargeStatus.ToString() == "High" && status.PowerLineStatus.ToString() == "Online")
            {
                if (GetBatteryPercent() >= 0.95)
                    img = Image.FromFile(@"img\1.png");
                if (GetBatteryPercent() < 0.95 && GetBatteryPercent() >= 0.2)
                    img = Image.FromFile(@"img\2.png");
                if (GetBatteryPercent() < 0.2)
                    img = Image.FromFile(@"img\7.png");
                pictureBox1.Image = img;
            }
            if (status.PowerLineStatus.ToString() == "Offline")
            {
                if (GetBatteryPercent() > 0.9)
                    img = Image.FromFile(@"img\3.png");
                if (0.9 > GetBatteryPercent() && GetBatteryPercent() > 50)
                    img = Image.FromFile(@"img\4.png");
                if (GetBatteryPercent() < 0.5 && GetBatteryPercent() > 0.1)
                    img = Image.FromFile(@"img\5.png");
                if (GetBatteryPercent() <= 0.1)
                    img = Image.FromFile(@"img\6.png");
                pictureBox1.Image = img;
            }
            if (status.BatteryChargeStatus.ToString() == "NoSystemBattery")
            {
                img = Image.FromFile(@"img\8.png");
                pictureBox1.Image = img;
            }

            if (status.BatteryChargeStatus.ToString() == "Unknown")
            {
                img = Image.FromFile(@"img\8.png");
                pictureBox1.Image = img;
            }

        }

        Hashtable pow = new Hashtable();
        string Active;

        private void GetPowers()
        {
            RegistryKey Key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Power\User\PowerSchemes");
            string[] powers = Key.GetSubKeyNames();
            Active = (string)Key.GetValue("ActivePowerScheme");
            string Shema;
            foreach (string str in powers)
            {
                Key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Power\User\PowerSchemes\" + str);
                Shema = (string)Key.GetValue("FriendlyName");
                try
                {
                    Shema = Shema.Remove(0, Shema.LastIndexOf(",") + 1);
                }
                catch { }
                pow.Add(Shema, str);
            }
        }
        
        private void Create_radio()
        {
            RadioButton[] rb = { radioButton1, radioButton2, radioButton3, radioButton4, radioButton5, radioButton6, radioButton7, radioButton8, radioButton9, radioButton10 };
            int i = 0;
            foreach (var dd in rb)
                dd.Visible = false;

            foreach (DictionaryEntry de in pow)
            {
                rb[i].Visible = true;
                rb[i].Text = (string)de.Key;
                if (Active == de.Value.ToString())
                {
                    rb[i].Checked = true;
                }
                i++;
            }
        }
        

        private void Form1_Load(object sender, EventArgs e)
        {
            ShowPowerStatus();
            
            SwitchImg();
            GetPowers();
            if (pow.Count != 0)
            {
                int h = this.Height + 53;
                this.Size = new Size(this.Width, h);
            }
            Create_radio();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            Brightness.Brightness.SetBrightness((short)trackBarBrightness.Value);
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            ShowPowerStatus();
            SwitchImg();
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            ChangePow(radioButton6);
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            ChangePow(radioButton5);
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            ChangePow(radioButton4);
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            ChangePow(radioButton3);
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            ChangePow(radioButton2);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            ChangePow(radioButton1);
        }



        private void ChangePow(RadioButton rb)
        {
            Active = (string)pow[rb.Text];
            ProcessStartInfo prc = new ProcessStartInfo("Powercfg", "-setactive " + Active);
            prc.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(prc);
        }

        private void Form1Closed(object sender, FormClosedEventArgs e)
        {
           
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}