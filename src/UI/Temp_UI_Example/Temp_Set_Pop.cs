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
    public partial class Temp_Set_Pop : Form
    {
        // 통신 객체 정의
        private TcAdsClient ads = new TcAdsClient();

        private int ALL_Temp_Set;
        public Temp_Set_Pop()
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
                ALL_Temp_Set = ads.CreateVariableHandle($"gbl.slave_fTargetTemp[{i + 1}]");
                ads.WriteAny(ALL_Temp_Set, double.Parse(All_TB12.Text));
            }
        }
    }
}
