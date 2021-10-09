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

        private int ALL_Remp_Set;
        public Ramp_Set_Pop()
        {
            InitializeComponent();
            Run();
        }
        private void Run()
        {
            // TwinCAT 연동
            ads.Connect(851);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= 3; i++)
            {
                ALL_Remp_Set = ads.CreateVariableHandle($"gbl.slave_fRamp_Value[{i + 1}]");
                ads.WriteAny(ALL_Remp_Set, double.Parse(ALL_TB16.Text));
            }
        }
    }
}
