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
    public partial class Master : Form
    {
        public Master()
        {
            InitializeComponent();
        }

        private void Master_Load(object sender, EventArgs e)
        {
            // FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            // 사용하면 작업표시줄도 안나옴
            WindowState = FormWindowState.Maximized;
        }
    }
}
