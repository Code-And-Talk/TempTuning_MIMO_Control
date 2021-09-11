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
        private int cnt = 1;

        //쓰레드1 = 데이터출력, 쓰레드2 = 와치독
        Thread thread1;
        Thread thread2;
        private bool bThreadStart1 = false;
        private bool bThreadStart2 = false;

        //와치독 변수
        private bool Watchdog1 = true;
        private bool Watchdog2 = true;
        private int Watchdog3 = 0;
        private int Watchdog4 = 0;


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
            thread2 = new Thread(new ThreadStart(WatchDog));
            thread1.IsBackground = true;
            thread2.IsBackground = true;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Array
            tb = new TextBox[] { TempTunetbox1, TempTunetbox2, TempTunetbox3, TempTunetbox4, TempTunetbox5, TempTunetbox6 };
            Temp_s = new TextBox[] { TB12, TB13, TB14, TB15 };
            Ramp_s = new TextBox[] { TB16, TB17, TB18, TB19 };
            Power_s = new TextBox[] { TB20, TB21, TB22, TB23 };

            //Tune Set 초기값
            comboBox1.SelectedIndex = 1;

            thread1.Start();
            thread2.Start();

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
                pot[25] = ads.ReadSymbolInfo("master.glass_max");
                pot[26] = ads.ReadSymbolInfo("master.glass_min");
                pot[27] = ads.ReadSymbolInfo("master.master_subTC_AVG");
                Act_Temp[25] = Convert.ToDouble(ads.ReadSymbol(pot[25]));
                Act_Temp[26] = Convert.ToDouble(ads.ReadSymbol(pot[26]));
                Act_Temp[27] = Convert.ToDouble(ads.ReadSymbol(pot[27]));
                Act_Temp[28] = Act_Temp[25] - Act_Temp[26];
                this.Controls["ST" + (25 - 16)].Text = Act_Temp[25].ToString();
                this.Controls["ST" + (26 - 16)].Text = Act_Temp[26].ToString();
                this.Controls["ST" + (27 - 16)].Text = Act_Temp[27].ToString();
                this.Controls["ST" + (28 - 16)].Text = Act_Temp[28].ToString();
                Thread.Sleep(10);

            }
        }

        //폼 종료시 쓰레드 종료
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (thread1 != null)
            {
                if (bThreadStart1)
                {
                    thread1.Abort();
                }
                else
                {
                    thread1.Interrupt();
                }
                thread1 = null;
            }

            if (thread2 != null)
            {
                if (bThreadStart2)
                {
                    thread2.Abort();
                }
                else
                {
                    thread2.Interrupt();
                }
                thread2 = null;
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

        //와치독 함수
        private void WatchDog()
        {
            while (true)
            {
                pot[28] = ads.ReadSymbolInfo("slave.Watch_Dog_Bool");
                Watchdog1 = Convert.ToBoolean(ads.ReadSymbol(pot[28]));
                //this.Controls["ST" + (28 - 16)].Text = Watchdog1.ToString();
                Thread.Sleep(1000);
                Watchdog4++;
                if(Watchdog4 >= 3)
                {
                    Watchdog4 = 0;
                    Watchdog3 = 0;
                }

                pot[29] = ads.ReadSymbolInfo("slave.Watch_Dog_Bool");
                Watchdog2 = Convert.ToBoolean(ads.ReadSymbol(pot[28]));

                if (Watchdog1 == Watchdog2)
                {
                    Watchdog3++;
                    if (Watchdog3 > 2)
                    {
                        MessageBox.Show("PLC가 정지되었습니다.");
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            for (int i = 0; i <= 3; i++) // Slave
            {
                // Temp_Set
                Temp_Set = ads.CreateVariableHandle($"sgbl.slave_fTargetTemp[{i + 1}]");
                ads.WriteAny(Temp_Set, double.Parse(Temp_s[i].Text));

                // Ramp_Set
                Ramp_Set = ads.CreateVariableHandle($"sgbl.slave_fRamp_Value[{i + 1}]");
                ads.WriteAny(Ramp_Set, double.Parse(Ramp_s[i].Text));

                // Power_Set
                Power_Set = ads.CreateVariableHandle($"sgbl.slave_fPowLimit_[{i + 1}]");
                ads.WriteAny(Power_Set, double.Parse(Power_s[i].Text));
            }
            if (TempTunetbox1.Enabled == true) // Master
            {
                // Target_Temp
                Temp_Tune = ads.CreateVariableHandle("mgbl.master_TargetTemp");
                ads.WriteAny(Temp_Tune, double.Parse(TempTunetbox1.Text));

                // gain(P)
                Temp_Tune = ads.CreateVariableHandle("mgbl.master_pi_gain[1]");
                ads.WriteAny(Temp_Tune, double.Parse(TempTunetbox2.Text));
                Temp_Tune = ads.CreateVariableHandle("mgbl.master_pi_gain[2]");
                ads.WriteAny(Temp_Tune, double.Parse(TempTunetbox2.Text));
                Temp_Tune = ads.CreateVariableHandle("mgbl.master_pi_gain[3]");
                ads.WriteAny(Temp_Tune, double.Parse(TempTunetbox2.Text));
                Temp_Tune = ads.CreateVariableHandle("mgbl.master_pi_gain[4]");
                ads.WriteAny(Temp_Tune, double.Parse(TempTunetbox2.Text));

                // i값(적분 상수)
                Temp_Tune = ads.CreateVariableHandle("mgbl.master_integral_constant[1]");
                ads.WriteAny(Temp_Tune, double.Parse(TempTunetbox3.Text));
                Temp_Tune = ads.CreateVariableHandle("mgbl.master_integral_constant[2]");
                ads.WriteAny(Temp_Tune, double.Parse(TempTunetbox3.Text));
                Temp_Tune = ads.CreateVariableHandle("mgbl.master_integral_constant[3]");
                ads.WriteAny(Temp_Tune, double.Parse(TempTunetbox3.Text));
                Temp_Tune = ads.CreateVariableHandle("mgbl.master_integral_constant[4]");
                ads.WriteAny(Temp_Tune, double.Parse(TempTunetbox3.Text));

                // Tune_Max
                Temp_Tune = ads.CreateVariableHandle("mgbl.master_Zone_Temp_Max_per");
                ads.WriteAny(Temp_Tune, double.Parse(TempTunetbox4.Text));

                // Tune_Min
                Temp_Tune = ads.CreateVariableHandle("mgbl.master_Zone_Temp_Min_per");
                ads.WriteAny(Temp_Tune, double.Parse(TempTunetbox5.Text));

                // Tune_Time
                Temp_Tune = ads.CreateVariableHandle("mgbl.master_tuning_time");
                ads.WriteAny(Temp_Tune, double.Parse(TempTunetbox6.Text));
            }
            else
            {
                MessageBox.Show("Master Value is Null");
            }
        }

        //Run 모드
        private void button3_Click(object sender, EventArgs e)
        {
            ads.WriteControl(new StateInfo(AdsState.Run, ads.ReadState().DeviceState));
        }

        //Stop 모드
        private void button4_Click(object sender, EventArgs e)
        {
            ads.WriteControl(new StateInfo(AdsState.Stop, ads.ReadState().DeviceState));
        }
    }
}
