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
    public partial class Ramp_Set_Pop : Form
    {
        // 통신 객체 정의
        private TcAdsClient ads = new TcAdsClient();

        // ADS 정보를 읽기 위한 인터페이스 정의
        private ITcAdsSymbol pot;
        double PT101;

        private int ALL_Remp_Set;
        public Ramp_Set_Pop()
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
                ALL_Remp_Set = ads.CreateVariableHandle($"gbl.slave_fRamp_Value[{i + 1}]");
                ads.WriteAny(ALL_Remp_Set, double.Parse(ALL_TB16.Text));
            }
        }

        private void Ramp_Set_Pop_Load(object sender, EventArgs e)
        {
            // PLC에 있는 값 읽어오기
            for (int i = 0; i <= 3; i++)
            {
                pot = ads.ReadSymbolInfo($"gbl.slave_fRamp_Value[{i + 1}]");
                PT101 = Convert.ToDouble(ads.ReadSymbol(pot));
                ALL_TB16.Text = "\r\n" + PT101.ToString();
            }
        }
    }
}
