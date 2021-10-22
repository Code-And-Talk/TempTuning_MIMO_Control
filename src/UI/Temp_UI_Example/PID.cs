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

        // ADS 정보를 읽기 위한 인터페이스 정의
        private ITcAdsSymbol[] pot2 = new ITcAdsSymbol[100];

        // Array
        double[] PT101 = new double[100];
        TextBox[] PID_p;
        TextBox[] PID_i;
        TextBox[] PID_d;

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
            //ads.Connect(851);
            ads.Connect("5.94.115.233.1.1", 851);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= 3; i++)
            {
                // 각 ZONE 별로 P 값 입력
                ZONE_P = ads.CreateVariableHandle($"gbl.slave_fP_Value[{i + 1}]");
                ads.WriteAny(ZONE_P, double.Parse(PID_p[i].Text));

                // 각 ZONE 별로 I 값 입력
                ZONE_I = ads.CreateVariableHandle($"gbl.slave_tI_Value[{i + 1}]");
                ads.WriteAny(ZONE_I, double.Parse(PID_i[i].Text));

                // 각 ZONE 별로 D 값 입력
                ZONE_D = ads.CreateVariableHandle($"gbl.slave_tD_Value[{i + 1}]");
                ads.WriteAny(ZONE_D, double.Parse(PID_d[i].Text));
            }
        }

        private void PID_Load(object sender, EventArgs e)
        {
            // PID 폼이 로드 되면 배열 생성
            PID_p = new TextBox[] { tboxZONE1_P, tboxZONE2_P, tboxZONE3_P, tboxZONE4_P };
            PID_i = new TextBox[] { tboxZONE1_I, tboxZONE2_I, tboxZONE3_I, tboxZONE4_I };
            PID_d = new TextBox[] { tboxZONE1_D, tboxZONE2_D, tboxZONE3_D, tboxZONE4_D };

            // PLC에 있는 값 읽어오기
            for (int i = 0; i <= 3; i++)
            {
                pot2[i] = ads.ReadSymbolInfo($"gbl.slave_fP_Value[{i + 1}]");
                PT101[i] = Convert.ToDouble(ads.ReadSymbol(pot2[i]));
                PID_p[i].Text = PT101[i].ToString();
            }

            for (int i = 0; i <= 3; i++)
            {
                pot2[i] = ads.ReadSymbolInfo($"gbl.slave_tI_Value[{i + 1}]");
                PT101[i] = Convert.ToDouble(ads.ReadSymbol(pot2[i]));
                PID_i[i].Text = PT101[i].ToString();
            }

            for (int i = 0; i <= 3; i++)
            {
                pot2[i] = ads.ReadSymbolInfo($"gbl.slave_tD_Value[{i + 1}]");
                PT101[i] = Convert.ToDouble(ads.ReadSymbol(pot2[i]));
                PID_d[i].Text = PT101[i].ToString();
            }
        }
        // PID 값 한번에 입력
        private void label13_Click(object sender, EventArgs e)
        {
            PID_Pop pid_pop = new PID_Pop();
            pid_pop.Show();
        }
    }
}
