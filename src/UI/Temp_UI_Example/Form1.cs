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

        // ADS 정보를 읽기 위한 인터페이스 정의
        private ITcAdsSymbol[] pot3 = new ITcAdsSymbol[100];
        private ITcAdsSymbol[] pot4 = new ITcAdsSymbol[100];
        bool[] bError = new bool[100];
        string[] Error_MSG = new string[100];

        // ADS 정보를 읽기 위한 인터페이스 정의, 값을 읽어오기 위함
        private ITcAdsSymbol[] pot = new ITcAdsSymbol[100];
        double[] Act_Temp = new double[4];
        double[] Act_Power = new double[4];
        double[] Work_Set = new double[4];
        double[] Tune_Result = new double[4];
        double[] Substrate_Temp = new double[9];
        double[] Substrate_Temp2 = new double[4];
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
        private int Temp_Set;
        private int Ramp_Set;
        private int Power_Set;
        private int Ramp_En;
        private int e_Ctrl_Mode;

        // Master Global Var
        private int Temp_Tune;

        // Global Var
        private int Tune_Set;
        private int Emergency;

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
            WindowState = FormWindowState.Maximized;
            // Array
            tb = new TextBox[] { TempTunetbox1, TempTunetbox2, TempTunetbox3, TempTunetbox4, TempTunetbox5, TempTunetbox6 };
            double[] PT102 = new double[100];
            Temp_s = new TextBox[] { TB12, TB13, TB14, TB15 };
            Ramp_s = new TextBox[] { TB16, TB17, TB18, TB19 };
            Power_s = new TextBox[] { TB20, TB21, TB22, TB23 };

            // Data Read
            // Temp Set
            for (int i = 0; i <= 3; i++)
            {
                pot3[i] = ads.ReadSymbolInfo($"gbl.slave_fTargetTemp[{i + 1}]");
                PT102[i] = Convert.ToDouble(ads.ReadSymbol(pot3[i]));
                Temp_s[i].Text = PT102[i].ToString();
            }
            // Ramp Set
            for (int i = 0; i <= 3; i++)
            {
                pot3[i] = ads.ReadSymbolInfo($"gbl.slave_fRamp_Value[{i + 1}]");
                PT102[i] = Convert.ToDouble(ads.ReadSymbol(pot3[i]));
                Ramp_s[i].Text = PT102[i].ToString();
            }
            // Power Set
            for (int i = 0; i <= 3; i++)
            {
                pot3[i] = ads.ReadSymbolInfo($"gbl.slave_fPowLimit[{i + 1}]");
                PT102[i] = Convert.ToDouble(ads.ReadSymbol(pot3[i]));
                Power_s[i].Text = PT102[i].ToString();
            }
            // Target Temp
            pot3[0] = ads.ReadSymbolInfo("gbl.master_TargetTemp");
            PT102[0] = Convert.ToDouble(ads.ReadSymbol(pot3[0]));
            TempTunetbox1.Text = PT102[0].ToString();

            // Gain(P)
            for (int i = 0; i < 3; i++)
            {
                pot3[i] = ads.ReadSymbolInfo($"gbl.master_pi_gain[{i + 1}]");
                PT102[i] = Convert.ToDouble(ads.ReadSymbol(pot3[i]));
                TempTunetbox2.Text = PT102[i].ToString();
            }

            // i(적분)
            for (int i = 0; i < 3; i++)
            {
                pot3[i] = ads.ReadSymbolInfo($"gbl.master_integral_constant[{i + 1}]");
                PT102[i] = Convert.ToDouble(ads.ReadSymbol(pot3[i]));
                TempTunetbox3.Text = PT102[i].ToString();
            }

            // Tune MAX
            pot3[0] = ads.ReadSymbolInfo("gbl.master_Zone_Temp_Max");
            PT102[0] = Convert.ToDouble(ads.ReadSymbol(pot3[0]));
            TempTunetbox4.Text = PT102[0].ToString();

            // Tune MIN
            pot3[0] = ads.ReadSymbolInfo("gbl.master_Zone_Temp_Min");
            PT102[0] = Convert.ToDouble(ads.ReadSymbol(pot3[0]));
            TempTunetbox5.Text = PT102[0].ToString();

            // Tune Time
            pot3[0] = ads.ReadSymbolInfo("gbl.tuning_time");
            PT102[0] = Convert.ToDouble(ads.ReadSymbol(pot3[0]));
            TempTunetbox6.Text = PT102[0].ToString();

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
                // Act Temp 출력
                for (int i = 0; i < 4; i++)
                {

                    pot[i] = ads.ReadSymbolInfo($"gbl.slave_fPV_Value[{cnt++}]");
                    Act_Temp[i] = Convert.ToDouble(ads.ReadSymbol(pot[i]));
                    this.Controls["ActTempTB" + i].Text = Act_Temp[i].ToString();
                    Thread.Sleep(10);

                }
                cnt = 1;

                //Work Set 출력
                for (int i = 0; i < 4; i++)
                {

                    pot[i] = ads.ReadSymbolInfo($"gbl.slave_fRamp_Out[{cnt++}]");
                    Work_Set[i] = Convert.ToDouble(ads.ReadSymbol(pot[i]));
                    this.Controls["WorkSetTB" + i].Text = Work_Set[i].ToString();
                    Thread.Sleep(10);

                }
                cnt = 1;

                //Act Power 출력
                for (int i = 0; i < 4; i++)
                {

                    pot[i] = ads.ReadSymbolInfo($"gbl.slave_fMV_Out[{cnt++}]");
                    Act_Power[i] = Convert.ToDouble(ads.ReadSymbol(pot[i]));
                    this.Controls["ActPowerTB" + i].Text = Act_Power[i].ToString();
                    Thread.Sleep(10);
                    
                }
                cnt = 1;

                //Tune Result 출력
                for (int i = 0; i < 4; i++)
                {

                    pot[i] = ads.ReadSymbolInfo($"gbl.slave_fPV_Value[{cnt++}]");
                    Tune_Result[i] = Convert.ToDouble(ads.ReadSymbol(pot[i]));
                    this.Controls["TuneResultTB" + i].Text = Tune_Result[i].ToString();
                    Thread.Sleep(10);

                }
                cnt = 1;

                //Substrate Temp 출력
                for (int i = 0; i < 9; i++)
                {

                    pot[i] = ads.ReadSymbolInfo($"gbl.fPV_Value_SubTC[{cnt++}]");
                    Substrate_Temp[i] = Convert.ToDouble(ads.ReadSymbol(pot[i]));
                    this.Controls["SubstrateTemp" + i].Text = Substrate_Temp[i].ToString();
                    Thread.Sleep(10);

                }
                cnt = 1;

                //Substrate Temp (MAX,MIN,AVG,MAX-MIN) 출력
                //pot[25] = ads.ReadSymbolInfo("gbl.subTC_max");
                //pot[26] = ads.ReadSymbolInfo("gbl.subTC_min");
                //pot[27] = ads.ReadSymbolInfo("gbl.subTC_AVG");
                //Act_Temp[25] = Convert.ToDouble(ads.ReadSymbol(pot[25]));
                //Act_Temp[26] = Convert.ToDouble(ads.ReadSymbol(pot[26]));
                //Act_Temp[27] = Convert.ToDouble(ads.ReadSymbol(pot[27]));
                //Act_Temp[28] = Act_Temp[25] - Act_Temp[26];
                //this.Controls["ST" + (25 - 16)].Text = Act_Temp[25].ToString();
                //this.Controls["ST" + (26 - 16)].Text = Act_Temp[26].ToString();
                //this.Controls["ST" + (27 - 16)].Text = Act_Temp[27].ToString();
                ////this.Controls["ST" + (28 - 16)].Text = Act_Temp[28].ToString();
                //Thread.Sleep(10);

                pot[25] = ads.ReadSymbolInfo("gbl.subTC_max");
                pot[26] = ads.ReadSymbolInfo("gbl.subTC_min");
                pot[27] = ads.ReadSymbolInfo("gbl.subTC_AVG");
                Substrate_Temp2[0] = Convert.ToDouble(ads.ReadSymbol(pot[25]));
                Substrate_Temp2[1] = Convert.ToDouble(ads.ReadSymbol(pot[26]));
                Substrate_Temp2[2] = Convert.ToDouble(ads.ReadSymbol(pot[27]));
                Substrate_Temp2[3] = Substrate_Temp2[0] - Substrate_Temp2[1];
                this.Controls["ST" + 0].Text = Substrate_Temp2[0].ToString();
                this.Controls["ST" + 1].Text = Substrate_Temp2[1].ToString();
                this.Controls["ST" + 2].Text = Substrate_Temp2[2].ToString();
                //밑에 와치독 테스트중
                //this.Controls["ST" + 3].Text = Substrate_Temp2[3].ToString();
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
        //private void WatchDog()
        //{
        //    while (true)
        //    {
        //        pot[28] = ads.ReadSymbolInfo("gbl.bWatch");
        //        Watchdog1 = Convert.ToBoolean(ads.ReadSymbol(pot[28]));
        //        this.Controls["ST" + (28 - 16)].Text = Watchdog1.ToString();
        //        Thread.Sleep(10000);
        //        Watchdog4++;
        //        if(Watchdog4 >= 3)
        //        {
        //            Watchdog4 = 0;
        //            Watchdog3 = 0;
        //        }

        //        pot[29] = ads.ReadSymbolInfo("gbl.bWatch");
        //        Watchdog2 = Convert.ToBoolean(ads.ReadSymbol(pot[28]));

        //        if (Watchdog1 == Watchdog2)
        //        {
        //            Watchdog3++;
        //            if (Watchdog3 > 2)
        //            {
        //                MessageBox.Show("PLC가 정지되었습니다.");
        //            }
        //        }
        //    }
        //}

        //와치독 함수
        private void WatchDog()
        {
            while (true)
            {
                pot[28] = ads.ReadSymbolInfo("gbl.bWatch");
                Watchdog1 = Convert.ToBoolean(ads.ReadSymbol(pot[28]));
                this.Controls["ST" + 3].Text = Watchdog1.ToString();
                Thread.Sleep(10000);
                Watchdog4++;
                if (Watchdog4 >= 3)
                {
                    Watchdog4 = 0;
                    Watchdog3 = 0;
                }

                pot[29] = ads.ReadSymbolInfo("gbl.bWatch");
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

        // Data Write
        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= 3; i++) // Slave
            {
                // Temp_Set
                Temp_Set = ads.CreateVariableHandle($"gbl.slave_fTargetTemp[{i + 1}]");
                ads.WriteAny(Temp_Set, double.Parse(Temp_s[i].Text));

                // Ramp_Set
                Ramp_Set = ads.CreateVariableHandle($"gbl.slave_fRamp_Value[{i + 1}]");
                ads.WriteAny(Ramp_Set, double.Parse(Ramp_s[i].Text));

                // Power_Set
                Power_Set = ads.CreateVariableHandle($"gbl.slave_fPowLimit[{i + 1}]");
                ads.WriteAny(Power_Set, double.Parse(Power_s[i].Text));
            }

            if (comboBox1.Text == true.ToString()) // Master
            {
                // Target_Temp
                Temp_Tune = ads.CreateVariableHandle("gbl.master_TargetTemp");
                ads.WriteAny(Temp_Tune, double.Parse(TempTunetbox1.Text));

                // gain(P)
                for (int i = 0; i <= 3; i++)
                {
                    Temp_Tune = ads.CreateVariableHandle($"gbl.master_pi_gain[{i + 1}]");
                    ads.WriteAny(Temp_Tune, double.Parse(TempTunetbox2.Text));
                }

                // i값(적분 상수)
                for (int i = 0; i <= 3; i++)
                {
                    Temp_Tune = ads.CreateVariableHandle($"gbl.master_integral_constant[{i + 1}]");
                    ads.WriteAny(Temp_Tune, double.Parse(TempTunetbox3.Text));
                }

                // Tune_Max
                Temp_Tune = ads.CreateVariableHandle("gbl.master_Zone_Temp_Max");
                ads.WriteAny(Temp_Tune, double.Parse(TempTunetbox4.Text));

                // Tune_Min
                Temp_Tune = ads.CreateVariableHandle("gbl.master_Zone_Temp_Min");
                ads.WriteAny(Temp_Tune, double.Parse(TempTunetbox5.Text));

                // Tune_Time
                Temp_Tune = ads.CreateVariableHandle("gbl.tuning_time");
                ads.WriteAny(Temp_Tune, double.Parse(TempTunetbox6.Text));

                // Tune_Set - tunning mode true / false 결정
                Tune_Set = ads.CreateVariableHandle("gbl.tuning_mode");
                ads.WriteAny(Tune_Set, bool.Parse(comboBox1.Text));
            }
            else if (comboBox1.Text == false.ToString())
            {
                // Tune_Set - tunning mode true / false 결정
                Tune_Set = ads.CreateVariableHandle("gbl.tuning_mode");
                ads.WriteAny(Tune_Set, bool.Parse(comboBox1.Text));
            }

            // Pamp_Set 값이 0보다 크면 Ramping_Enable => true / false
            for (int i = 0; i <= 3; i++)
            {
                if (int.Parse(Ramp_s[i].Text) > 0)
                {
                    Ramp_En = ads.CreateVariableHandle($"gbl.slave_bRamp_Enable[{i + 1}]");
                    ads.WriteAny(Ramp_En, true);
                }
                else if (int.Parse(Ramp_s[i].Text) <= 0)
                {
                    Ramp_En = ads.CreateVariableHandle($"gbl.slave_bRamp_Enable[{i + 1}]");
                    ads.WriteAny(Ramp_En, false);
                }
            }

            // eCtrl_Mode 설정
            for (int i = 0; i <= 3; i++)
            {
                if (int.Parse(Temp_s[i].Text) > 0 && int.Parse(Power_s[i].Text) > 0)
                {
                    e_Ctrl_Mode = ads.CreateVariableHandle($"gbl.slave_eCTRL_MODE[{i + 1}]");
                    ads.WriteAny(e_Ctrl_Mode, Int16.Parse("2"));
                }
                else if (int.Parse(Temp_s[i].Text) <= 0 || int.Parse(Power_s[i].Text) <= 0)
                {
                    e_Ctrl_Mode = ads.CreateVariableHandle($"gbl.slave_eCTRL_MODE[{i + 1}]");
                    ads.WriteAny(e_Ctrl_Mode, Int16.Parse("0"));
                }
            }

            // Error 
            pot4[0] = ads.ReadSymbolInfo($"gbl.slave_bError");
            bError[0] = Convert.ToBoolean(ads.ReadSymbol(pot4[0]));
            if (bError[0] == true)
            {
                Error_True();

                pot4[0] = ads.ReadSymbolInfo($"gbl.slave_nErrorID");
                Error_MSG[0] = Convert.ToString(ads.ReadSymbol(pot4[0]));

                txtError.Text = string.Format("Error = " + Error_MSG[0].ToString() + "\r\n");
            }
            else if (bError[0] == false)
            {
                pictureBox1.Load(@"C:\SmartFactory\Last_Project\Temp_UI_Example\Resources\꺼진등.png");
            }
        }

        // 깜빡깜빡
        //딜레이 메소드
        private static DateTime Delay(int MS)

        {

            DateTime ThisMoment = DateTime.Now;

            TimeSpan duration = new TimeSpan(0, 0, 0, 0, MS);

            DateTime AfterWards = ThisMoment.Add(duration);

            while (AfterWards >= ThisMoment)

            {

                System.Windows.Forms.Application.DoEvents();

                ThisMoment = DateTime.Now;

            }

            return DateTime.Now;

        }

        // 에러 발생 시 애니메이션
        public void Error_Check()
        {
            // 경로 설정 검토
            for (int i = 0; i < 10; i++)
            {
                pictureBox1.Load(@"C:\SmartFactory\Last_Project\Temp_UI_Example\Resources\켜진등.png");
                Delay(150);
                pictureBox1.Load(@"C:\SmartFactory\Last_Project\Temp_UI_Example\Resources\꺼진등.png");
                Delay(150);
            }
            pictureBox1.Load(@"C:\SmartFactory\Last_Project\Temp_UI_Example\Resources\켜진등.png");
        }

        // 쓰레드3 : 에러발생
        public void Error_True()
        {
            Thread thread3 = new Thread(Error_Check);
            thread3.Start();
        }

        // 비상정지(UI에서 bError = True 값을 주면 PLC에서 Emergency_Stop이 True // eCTRL_MODE가 IDLE로 변경)
        private void button3_Click(object sender, EventArgs e)
        {
            Emergency = ads.CreateVariableHandle($"gbl.slave_bError");
            ads.WriteAny(Emergency, true);
        }
    }
}
