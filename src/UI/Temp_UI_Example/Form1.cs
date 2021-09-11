using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
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
        private ITcAdsSymbol[] pot = new ITcAdsSymbol[100];
        double[] Act_Temp = new double[100];
        int cnt = 1;

        //쓰레드
        Thread thread1;
        bool bThreadStart = false;

        // Slave Global Var
        private int Ramp_Set;
        private int Power_Set;
        private int Temp_Set;

        // Master Global Var
        private int Temp_Tune;

        // Array
        TextBox[] tb;
        TextBox[] Temp_s;
        TextBox[] Ramp_s;
        TextBox[] Power_s;

        public Form1()
        {

            InitializeComponent();

            Run();

            thread1 = new Thread(new ThreadStart(ReadData));
            thread1.IsBackground = true;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 화면 크기 최대화
            WindowState = FormWindowState.Maximized;

            // Array
            tb = new TextBox[] { TempTunetbox1, TempTunetbox2, TempTunetbox3, TempTunetbox4, TempTunetbox5, TempTunetbox6 };
            Temp_s = new TextBox[] { TB12, TB13, TB14, TB15 };
            Ramp_s = new TextBox[] { TB16, TB17, TB18, TB19 };
            Power_s = new TextBox[] { TB20, TB21, TB22, TB23 };

            //Tune Set 초기값
            comboBox1.SelectedIndex = 1;

        }

        //TwinCat3 연동
        private void Run()
        {
            ads.Connect(851); //연결 포트
            if (ads.IsConnected == true)
            {
                MessageBox.Show("연결 성공");
            }
            else
            {
                MessageBox.Show("연결 실패");
            }
        }

        //데이터 출력 함수
        private void ReadData()
        {
            while (true)
            {
                //Act Temp 출력
                for(int i = 0; i < 4; i++)
                {

                    pot[i] = ads.ReadSymbolInfo($"slave.s_fPV_Value[{cnt++}]");
                    Act_Temp[i] = Convert.ToDouble(ads.ReadSymbol(pot[i]));
                    this.Controls["tB" + i].Text = Act_Temp[i].ToString();
                    Thread.Sleep(10);
                    
                }
                cnt = 1;

                //Work Set 출력
                for (int i = 4; i < 8; i++)
                {

                    pot[i] = ads.ReadSymbolInfo($"slave.s_fRamp_Out[{cnt++}]");
                    Act_Temp[i] = Convert.ToDouble(ads.ReadSymbol(pot[i]));
                    this.Controls["tB" + i].Text = Act_Temp[i].ToString();
                    Thread.Sleep(10);

                }
                cnt = 1;

                //Act Power 출력
                for (int i = 8; i < 12; i++)
                {

                    pot[i] = ads.ReadSymbolInfo($"slave.s_fMV_Out[{cnt++}]");
                    Act_Temp[i] = Convert.ToDouble(ads.ReadSymbol(pot[i]));
                    this.Controls["tB" + i].Text = Act_Temp[i].ToString();
                    Thread.Sleep(10);
                    
                }
                cnt = 1;

                //Tune Result 출력
                for (int i = 12; i < 16; i++)
                {

                    pot[i] = ads.ReadSymbolInfo($"slave.s_fPV_Value[{cnt++}]");
                    Act_Temp[i] = Convert.ToDouble(ads.ReadSymbol(pot[i]));
                    this.Controls["tB" + (i+12)].Text = Act_Temp[i].ToString();
                    Thread.Sleep(10);

                }
                cnt = 1;

                //Substrate Temp 출력
                for (int i = 16; i < 25; i++)
                {

                    pot[i] = ads.ReadSymbolInfo($"master.fPV_Value_Sub[{cnt++}]");
                    Act_Temp[i] = Convert.ToDouble(ads.ReadSymbol(pot[i]));
                    this.Controls["ST" + (i - 16)].Text = Act_Temp[i].ToString();
                    Thread.Sleep(10);

                }
                cnt = 1;

                //Substrate Temp (MAX,MIN,AVG,MAX-MIN) 출력
                pot[25] = ads.ReadSymbolInfo($"master.glass_max");
                pot[26] = ads.ReadSymbolInfo($"master.glass_min");
                pot[27] = ads.ReadSymbolInfo($"master.master_subTC_AVG");
                Act_Temp[25] = Convert.ToDouble(ads.ReadSymbol(pot[25]));
                Act_Temp[26] = Convert.ToDouble(ads.ReadSymbol(pot[26]));
                Act_Temp[27] = Convert.ToDouble(ads.ReadSymbol(pot[27]));
                Act_Temp[28] = Act_Temp[25] - Act_Temp[26];
                this.Controls["ST" + (25 - 16)].Text = Act_Temp[26].ToString();
                this.Controls["ST" + (26 - 16)].Text = Act_Temp[27].ToString();
                this.Controls["ST" + (27 - 16)].Text = Act_Temp[28].ToString();
                this.Controls["ST" + (28 - 16)].Text = Act_Temp[28].ToString();
                Thread.Sleep(10);

            }
        }

        //폼 종료시 쓰레드 종료
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (thread1 != null)
            {
                if (bThreadStart)
                {
                    thread1.Abort();
                }
                else
                {
                    thread1.Interrupt();
                }
                thread1 = null;
            }
        }

        // PID 팝업 창 띄우기
        private void button1_Click_1(object sender, EventArgs e)
        {     
            PID pid = new PID();
            pid.Show();
        }

        //Temp Tune 활성화/비활성화
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (comboBox1.SelectedIndex > 0)
            {
                for (int i = 0; i <= 5; i++)
                {
                    tb[i].Enabled = false;
                }
            }
            else
            {
                for (int i = 0; i <= 5; i++)
                {
                    tb[i].Enabled = true;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            thread1.Start();

            for (int i = 0; i <= 3; i++)
            {
                if (i == 0)
                {
                    Temp_Set = ads.CreateVariableHandle("sgbl.slave_fTargetTemp[1]");
                    Ramp_Set = ads.CreateVariableHandle("sgbl.slave_fRamp_Value[1]");
                    Power_Set = ads.CreateVariableHandle("sgbl.slave_fPowLimit_[1]");

                    ads.WriteAny(Temp_Set, double.Parse(Temp_s[i].Text));
                    ads.WriteAny(Ramp_Set, double.Parse(Ramp_s[i].Text));
                    ads.WriteAny(Power_Set, double.Parse(Power_s[i].Text));
                }
                else if (i == 1)
                {
                    Temp_Set = ads.CreateVariableHandle("sgbl.slave_fTargetTemp[2]");
                    Ramp_Set = ads.CreateVariableHandle("sgbl.slave_fRamp_Value[2]");
                    Power_Set = ads.CreateVariableHandle("sgbl.slave_fPowLimit_[2]");

                    ads.WriteAny(Temp_Set, double.Parse(Temp_s[i].Text));
                    ads.WriteAny(Ramp_Set, double.Parse(Ramp_s[i].Text));
                    ads.WriteAny(Power_Set, double.Parse(Power_s[i].Text));
                }
                else if (i == 2)
                {
                    Temp_Set = ads.CreateVariableHandle("sgbl.slave_fTargetTemp[3]");
                    Ramp_Set = ads.CreateVariableHandle("sgbl.slave_fRamp_Value[3]");
                    Power_Set = ads.CreateVariableHandle("sgbl.slave_fPowLimit_[3]");

                    ads.WriteAny(Temp_Set, double.Parse(Temp_s[i].Text));
                    ads.WriteAny(Ramp_Set, double.Parse(Ramp_s[i].Text));
                    ads.WriteAny(Power_Set, double.Parse(Power_s[i].Text));
                }
                else if (i == 3)
                {
                    Temp_Set = ads.CreateVariableHandle("sgbl.slave_fTargetTemp[4]");
                    Ramp_Set = ads.CreateVariableHandle("sgbl.slave_fRamp_Value[4]");
                    Power_Set = ads.CreateVariableHandle("sgbl.slave_fPowLimit_[4]");

                    ads.WriteAny(Temp_Set, double.Parse(Temp_s[i].Text));
                    ads.WriteAny(Ramp_Set, double.Parse(Ramp_s[i].Text));
                    ads.WriteAny(Power_Set, double.Parse(Power_s[i].Text));
                }
            }
            if (TempTunetbox1.Enabled == true)
            {
                Temp_Tune = ads.CreateVariableHandle("mgbl.master_TargetTemp");
                ads.WriteAny(Temp_Tune, double.Parse(TempTunetbox1.Text));

                Temp_Tune = ads.CreateVariableHandle("mgbl.master_pi_gain[1]");
                ads.WriteAny(Temp_Tune, double.Parse(TempTunetbox2.Text));
                Temp_Tune = ads.CreateVariableHandle("mgbl.master_pi_gain[2]");
                ads.WriteAny(Temp_Tune, double.Parse(TempTunetbox2.Text));
                Temp_Tune = ads.CreateVariableHandle("mgbl.master_pi_gain[3]");
                ads.WriteAny(Temp_Tune, double.Parse(TempTunetbox2.Text));
                Temp_Tune = ads.CreateVariableHandle("mgbl.master_pi_gain[4]");
                ads.WriteAny(Temp_Tune, double.Parse(TempTunetbox2.Text));

                Temp_Tune = ads.CreateVariableHandle("mgbl.master_integral_constant[1]");
                ads.WriteAny(Temp_Tune, double.Parse(TempTunetbox3.Text));
                Temp_Tune = ads.CreateVariableHandle("mgbl.master_integral_constant[2]");
                ads.WriteAny(Temp_Tune, double.Parse(TempTunetbox3.Text));
                Temp_Tune = ads.CreateVariableHandle("mgbl.master_integral_constant[3]");
                ads.WriteAny(Temp_Tune, double.Parse(TempTunetbox3.Text));
                Temp_Tune = ads.CreateVariableHandle("mgbl.master_integral_constant[4]");
                ads.WriteAny(Temp_Tune, double.Parse(TempTunetbox3.Text));

                Temp_Tune = ads.CreateVariableHandle("mgbl.master_Zone_Temp_Max_per");
                ads.WriteAny(Temp_Tune, double.Parse(TempTunetbox4.Text));

                Temp_Tune = ads.CreateVariableHandle("mgbl.master_Zone_Temp_Min_per");
                ads.WriteAny(Temp_Tune, double.Parse(TempTunetbox5.Text));

                Temp_Tune = ads.CreateVariableHandle("mgbl.master_tuning_time");
                TempTunetbox6.Text = (float.Parse(TempTunetbox6.Text) * 0.000000000000000000000000000000000000000000001f).ToString();
                ads.WriteAny(Temp_Tune, float.Parse(TempTunetbox6.Text));
            }
            else
            {
                MessageBox.Show("Master Value is Null");
            }
        }
    }
}
