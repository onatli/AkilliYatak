using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace AkilliYatak
{
    public partial class Form1 : Form
    {
        string positionTime;
        int minute = 0;
        int second = 0;
        string lastposition;
        string position;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                cmb_Ports.Items.Add(port);
            }
        }

        private void btn_PortSelect_Click(object sender, EventArgs e)
        {
            serialPort1.PortName = cmb_Ports.SelectedItem.ToString();
            serialPort1.Open();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            serialPort1.Close();
        }

        private void positionChaged(string newposition)
        {
            if (newposition != position)
            {
                lastposition = position;
                position = newposition;
                positionTime = "00:00";

                switch (newposition)
                {
                    case "rlateral":
                        pictureBox1.Image = AkilliYatak.Properties.Resources.SagLateral;
                        break;
                    case "llateral":
                        pictureBox1.Image = AkilliYatak.Properties.Resources.SolLateral;

                        break;
                    case "supine":
                        pictureBox1.Image = AkilliYatak.Properties.Resources.Supine;

                        break;
                    case "fowler":
                        pictureBox1.Image = AkilliYatak.Properties.Resources.Fowler;

                        break;
                    case "empty":
                        pictureBox1.Image = null;
                        break;


                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            second++;
            if (second > 59)
            {
                second = 0;
                minute++;
            }
            positionTime = minute.ToString() + ":" + second.ToString();
            lbl_Time.Text = positionTime;
            positionChaged(decodeData(serialPort1.ReadLine()));
            
        }

        private string decodeData(string data)
        {
            switch (data)
            {
                case "0000000000000000000000001":
                    return "rlateral";
                    
                case "0000000000000000000100000":
                    return "llateral";

                case "0000000000000010000000000":
                    return "spine";

                case "0000100000000000000000000":
                    return "fowler";

                default:
                    return "empty";


            }
        }
    }
}
