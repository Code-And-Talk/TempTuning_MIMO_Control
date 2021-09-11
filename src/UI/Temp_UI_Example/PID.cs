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

        private int ZONE_P;
        private int ZONE_I;
        private int ZONE_D;

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
            // 각 ZONE 별로 P 값 입력
            ZONE_P = ads.CreateVariableHandle("sgbl.slave_fP_Value_[1]");
            ads.WriteAny(ZONE_P, double.Parse(tboxZONE1_P.Text));

            ZONE_P = ads.CreateVariableHandle("sgbl.slave_fP_Value_[2]");
            ads.WriteAny(ZONE_P, double.Parse(tboxZONE2_P.Text));

            ZONE_P = ads.CreateVariableHandle("sgbl.slave_fP_Value_[3]");
            ads.WriteAny(ZONE_P, double.Parse(tboxZONE3_P.Text));

            ZONE_P = ads.CreateVariableHandle("sgbl.slave_fP_Value_[4]");
            ads.WriteAny(ZONE_P, double.Parse(tboxZONE4_P.Text));

            // 각 ZONE 별로 I 값 입력
            ZONE_I = ads.CreateVariableHandle("sgbl.slave_tI_Value_[1]");
            tboxZONE1_I.Text = (float.Parse(tboxZONE1_I.Text) * 0.000000000000000000000000000000000000000000001f).ToString();
            ads.WriteAny(ZONE_I, float.Parse(tboxZONE1_I.Text));

            ZONE_I = ads.CreateVariableHandle("sgbl.slave_tI_Value_[2]");
            tboxZONE2_I.Text = (float.Parse(tboxZONE2_I.Text) * 0.000000000000000000000000000000000000000000001f).ToString();
            ads.WriteAny(ZONE_I, float.Parse(tboxZONE2_I.Text));

            ZONE_I = ads.CreateVariableHandle("sgbl.slave_tI_Value_[3]");
            tboxZONE3_I.Text = (float.Parse(tboxZONE3_I.Text) * 0.000000000000000000000000000000000000000000001f).ToString();
            ads.WriteAny(ZONE_I, float.Parse(tboxZONE3_I.Text));

            ZONE_I = ads.CreateVariableHandle("sgbl.slave_tI_Value_[4]");
            tboxZONE4_I.Text = (float.Parse(tboxZONE4_I.Text) * 0.000000000000000000000000000000000000000000001f).ToString();
            ads.WriteAny(ZONE_I, float.Parse(tboxZONE4_I.Text));

            // 각 ZONE 별로 D 값 입력
            ZONE_D = ads.CreateVariableHandle("sgbl.slave_tD_Value_[1]");
            tboxZONE1_D.Text = (float.Parse(tboxZONE1_D.Text) * 0.000000000000000000000000000000000000000000001f).ToString();
            ads.WriteAny(ZONE_D, float.Parse(tboxZONE1_D.Text));

            ZONE_D = ads.CreateVariableHandle("sgbl.slave_tD_Value_[2]");
            tboxZONE2_D.Text = (float.Parse(tboxZONE2_D.Text) * 0.000000000000000000000000000000000000000000001f).ToString();
            ads.WriteAny(ZONE_D, float.Parse(tboxZONE2_D.Text));

            ZONE_D = ads.CreateVariableHandle("sgbl.slave_tD_Value_[3]");
            tboxZONE3_D.Text = (float.Parse(tboxZONE3_D.Text) * 0.000000000000000000000000000000000000000000001f).ToString();
            ads.WriteAny(ZONE_D, float.Parse(tboxZONE3_D.Text));

            ZONE_D = ads.CreateVariableHandle("sgbl.slave_tD_Value_[4]");
            tboxZONE4_D.Text = (float.Parse(tboxZONE4_D.Text) * 0.000000000000000000000000000000000000000000001f).ToString();
            ads.WriteAny(ZONE_D, float.Parse(tboxZONE4_D.Text));
        }
    }
}
