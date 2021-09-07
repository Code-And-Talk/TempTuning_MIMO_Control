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
        Master child1 = new Master();
        Slave child2 = new Slave();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            // 사용하면 작업표시줄도 안나옴
            WindowState = FormWindowState.Maximized;

            child1.TopLevel = false;
            child2.TopLevel = false;

            this.Controls.Add(child1);
            this.Controls.Add(child2);

            child1.Parent = this.panel1;
            child2.Parent = this.panel1;

            child1.Text = child2.Text = "";
            child1.ControlBox = child2.ControlBox = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // [1번폼] 메뉴를 클릭했을 때

            child2.Hide();

            child1.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // [2번폼] 메뉴를 클릭했을 때

            child1.Hide();

            child2.Show();
        }
    }
}
