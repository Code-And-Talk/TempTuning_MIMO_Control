using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TwinCAT.Ads;

namespace Temp_UI_Example
{
    public partial class PID : Form
    {
        // 통신 객체 정의
        private TcAdsClient ads = new TcAdsClient();

        private int hvar;
        public PID()
        {
            InitializeComponent();
            Run();
        }
        private void Run()
        {
            // TwinCAT 연동
            ads.Connect(851);

            if (ads.IsConnected == true)
            {
                MessageBox.Show("연결 성공");
            }
            else
            {
                MessageBox.Show("연결 실패");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            hvar = ads.CreateVariableHandle("MAIN.P");

            ads.WriteAny(hvar, Int16.Parse(textBox4.Text));
        }
    }
}
