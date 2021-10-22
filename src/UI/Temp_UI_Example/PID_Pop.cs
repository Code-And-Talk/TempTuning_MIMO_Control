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
    public partial class PID_Pop : Form
    {
        // 통신 객체 정의
        private TcAdsClient ads = new TcAdsClient();

        // ADS 정보를 읽기 위한 인터페이스 정의
        private ITcAdsSymbol pot;
        double PT101;

        private int ALL_P_Set;
        private int ALL_I_Set;
        private int ALL_D_Set;
        public PID_Pop()
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

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= 3; i++)
            {
                // 각 ZONE 별로 P 값 입력
                ALL_P_Set = ads.CreateVariableHandle($"gbl.slave_fP_Value[{i + 1}]");
                ads.WriteAny(ALL_P_Set, double.Parse(tboxALL_P.Text));

                // 각 ZONE 별로 I 값 입력
                ALL_I_Set = ads.CreateVariableHandle($"gbl.slave_tI_Value[{i + 1}]");
                ads.WriteAny(ALL_I_Set, double.Parse(tboxALL_I.Text));

                // 각 ZONE 별로 D 값 입력
                ALL_D_Set = ads.CreateVariableHandle($"gbl.slave_tD_Value[{i + 1}]");
                ads.WriteAny(ALL_D_Set, double.Parse(tboxALL_D.Text));
            }
        }

        private void PID_Pop_Load(object sender, EventArgs e)
        {
            // PLC에 있는 값 읽어오기
            for (int i = 0; i <= 3; i++)
            {
                pot = ads.ReadSymbolInfo($"gbl.slave_fP_Value[{i + 1}]");
                PT101 = Convert.ToDouble(ads.ReadSymbol(pot));
                tboxALL_P.Text = PT101.ToString();
            }

            for (int i = 0; i <= 3; i++)
            {
                pot = ads.ReadSymbolInfo($"gbl.slave_tI_Value[{i + 1}]");
                PT101 = Convert.ToDouble(ads.ReadSymbol(pot));
                tboxALL_I.Text = PT101.ToString();
            }

            for (int i = 0; i <= 3; i++)
            {
                pot = ads.ReadSymbolInfo($"gbl.slave_tD_Value[{i + 1}]");
                PT101 = Convert.ToDouble(ads.ReadSymbol(pot));
                tboxALL_D.Text = PT101.ToString();
            }
        }
    }
}
