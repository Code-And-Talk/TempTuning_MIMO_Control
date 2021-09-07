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

namespace TwinCAT_ADS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        // ADS(통신) 객체 정의
        private TcAdsClient ads = new TcAdsClient();

        // 제어 변수 ? ?
        private int hvar;

        private void Form1_Load(object sender, EventArgs e)
        {
            // 폼이 로딩 될 때

            ads.Connect(851); // TwinCAT3 디폴트 포트번호 851 TwinCAT2 는 801
            // 연결 성공, 실패 여부 체크
            if(ads.IsConnected == true)
            {
                MessageBox.Show("연결 성공");
            }
            else
            {
                MessageBox.Show("연결 실패");
            }
            // Run bit를 제어하기 위한 제어 변수
            hvar = ads.CreateVariableHandle("MAIN.Run");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ads.WriteAny(hvar, true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ads.WriteAny(hvar, false);
        }
    }
}
