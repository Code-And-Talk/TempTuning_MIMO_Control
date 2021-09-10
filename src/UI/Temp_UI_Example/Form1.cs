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

        Thread thread1;
        bool bThreadStart = false;



        public Form1()
        {

            InitializeComponent();

            thread1 = new Thread(new ThreadStart(ReadData));
            thread1.IsBackground = true;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 화면 크기 최대화
            WindowState = FormWindowState.Maximized;

            Run();

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
                    Thread.Sleep(100);
                    
                }
                cnt = 1;

                //Work Set 출력
                for (int i = 4; i < 8; i++)
                {

                    pot[i] = ads.ReadSymbolInfo($"slave.s_fRamp_Out[{cnt++}]");
                    Act_Temp[i] = Convert.ToDouble(ads.ReadSymbol(pot[i]));
                    this.Controls["tB" + i].Text = Act_Temp[i].ToString();
                    Thread.Sleep(100);

                }
                cnt = 1;

                //Act Power 출력
                for (int i = 8; i < 12; i++)
                {

                    pot[i] = ads.ReadSymbolInfo($"slave.s_fMV_Out[{cnt++}]");
                    Act_Temp[i] = Convert.ToDouble(ads.ReadSymbol(pot[i]));
                    this.Controls["tB" + i].Text = Act_Temp[i].ToString();
                    Thread.Sleep(100);
                    
                }
                cnt = 1;

                //Tune Result 출력
                for (int i = 12; i < 16; i++)
                {

                    pot[i] = ads.ReadSymbolInfo($"slave.s_fPV_Value[{cnt++}]");
                    Act_Temp[i] = Convert.ToDouble(ads.ReadSymbol(pot[i]));
                    this.Controls["tB" + (i+12)].Text = Act_Temp[i].ToString();
                    Thread.Sleep(100);

                }
                cnt = 1;

                //Substrate Temp 출력
                for (int i = 16; i < 25; i++)
                {

                    pot[i] = ads.ReadSymbolInfo($"master.fPV_Value_Sub[{cnt++}]");
                    Act_Temp[i] = Convert.ToDouble(ads.ReadSymbol(pot[i]));
                    this.Controls["ST" + (i - 16)].Text = Act_Temp[i].ToString();
                    Thread.Sleep(100);

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
                Thread.Sleep(100);

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
            if(comboBox1.SelectedIndex > 0)
            {
                TempTunetbox1.Enabled = false;
                TempTunetbox2.Enabled = false;
                TempTunetbox3.Enabled = false;
                TempTunetbox4.Enabled = false;
                TempTunetbox5.Enabled = false;
                TempTunetbox6.Enabled = false;
            }
            else
            {
                TempTunetbox1.Enabled = true;
                TempTunetbox2.Enabled = true;
                TempTunetbox3.Enabled = true;
                TempTunetbox4.Enabled = true;
                TempTunetbox5.Enabled = true;
                TempTunetbox6.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            thread1.Start();
        }
    }
}
