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
    public partial class Form1 : Form
    {
        // 통신 객체 정의
        private TcAdsClient ads = new TcAdsClient();

        // ADS 정보를 읽기 위한 인터페이스 정의, 값을 읽어오기 위함
        private ITcAdsSymbol pot = null;
        public Form1()
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
        private void button1_Click_1(object sender, EventArgs e)
        {
            // PID 팝업 창 띄우기
            PID pid = new PID();
            pid.Show();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox1.SelectedIndex > 0)
            {
                TempTunetbox1.Enabled = false;
                TempTunetbox2.Enabled = false;
                TempTunetbox3.Enabled = false;
                TempTunetbox4.Enabled = false;
                TempTunetbox5.Enabled = false;
                TempTunetbox6.Enabled = false;
            }
            else
            {
                TempTunetbox1.Enabled = true;
                TempTunetbox2.Enabled = true;
                TempTunetbox3.Enabled = true;
                TempTunetbox4.Enabled = true;
                TempTunetbox5.Enabled = true;
                TempTunetbox6.Enabled = true;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // 화면 크기 최대화
            WindowState = FormWindowState.Maximized;
        }
    }
}
