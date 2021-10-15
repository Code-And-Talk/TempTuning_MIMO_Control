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
using System.Windows.Forms.DataVisualization.Charting;
using Oracle.ManagedDataAccess.Client;

namespace Temp_UI_Example
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        // 통신 객체 정의
        private TcAdsClient ads = new TcAdsClient();

        // ADS 정보를 읽기 위한 인터페이스 정의
        private ITcAdsSymbol[] pot3 = new ITcAdsSymbol[100];
        private ITcAdsSymbol pot4; // 에러 읽기
        bool bError;
        string[] Error_MSG = new string[100];

        // ADS 정보를 읽기 위한 인터페이스 정의, 값을 읽어오기 위함
        private ITcAdsSymbol[] pot = new ITcAdsSymbol[100];
        double[] Act_Temp = new double[4];
        double[] Act_Power = new double[4];
        double[] Work_Set = new double[4];
        double[] Tune_Result = new double[4];
        double[] Substrate_Temp = new double[9];
        double[] Substrate_Temp2 = new double[4];
        double[] PT102 = new double[100];
        private int cnt = 1;

        //쓰레드1 = 데이터출력, 쓰레드2 = 와치독
        Thread thread1;
        Thread thread2;

        private bool bThreadStart1 = false;
        private bool bThreadStart2 = false;

        //차트
        double k = 0;

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
        private int bStart;

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
        TextBox[] Ctrl;

        //소수점 버림
        String str,str2,str3,str4;
        String str5;

        int count = 0;
        public Form1()
        {

            InitializeComponent();

            Run();

            thread1 = new Thread(new ThreadStart(ReadData));
            thread2 = new Thread(new ThreadStart(WatchDog));

            thread1.IsBackground = true;
            thread2.IsBackground = true;

            // chart 확대 축소
            chart1.ChartAreas[0].CursorX.IsUserEnabled = Enabled;
            chart1.ChartAreas[0].CursorY.IsUserEnabled = Enabled;
            chart1.ChartAreas[0].CursorX.LineWidth = 3;
            chart1.ChartAreas[0].CursorY.LineWidth = 3;
            chart1.ChartAreas[0].CursorX.Interval = 0;
            chart1.ChartAreas[0].CursorY.Interval = 0;
            chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = Enabled;
            chart1.ChartAreas[0].CursorX.AutoScroll = true;
            chart1.ChartAreas[0].CursorY.IsUserSelectionEnabled = Enabled;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 폼 크기 최대화
            WindowState = FormWindowState.Maximized;

            // Array
            tb = new TextBox[] { TempTunetbox1, TempTunetbox2, TempTunetbox3, TempTunetbox4, TempTunetbox5, TempTunetbox6 };
            Temp_s = new TextBox[] { TB12, TB13, TB14, TB15 };
            Ramp_s = new TextBox[] { TB16, TB17, TB18, TB19 };
            Power_s = new TextBox[] { TB20, TB21, TB22, TB23 };
            Ctrl = new TextBox[] { e_Ctrl_modeTB0, e_Ctrl_modeTB1, e_Ctrl_modeTB2, e_Ctrl_modeTB3 };

            // Data Read
            // Temp Set
            for (int i = 0; i <= 3; i++)
            {
                pot3[i] = ads.ReadSymbolInfo($"gbl.slave_fTargetTemp[{i + 1}]");
                PT102[i] = Convert.ToDouble(ads.ReadSymbol(pot3[i]));
                Temp_s[i].Text = "\r\n" + PT102[i].ToString();
            }
            // Ramp Set
            for (int i = 0; i <= 3; i++)
            {
                pot3[i] = ads.ReadSymbolInfo($"gbl.slave_fRamp_Value[{i + 1}]");
                PT102[i] = Convert.ToDouble(ads.ReadSymbol(pot3[i]));
                Ramp_s[i].Text = "\r\n" + PT102[i].ToString();
            }
            // Power Set
            for (int i = 0; i <= 3; i++)
            {
                pot3[i] = ads.ReadSymbolInfo($"gbl.slave_fPowLimit[{i + 1}]");
                PT102[i] = Convert.ToDouble(ads.ReadSymbol(pot3[i]));
                Power_s[i].Text = "\r\n" + PT102[i].ToString();
            }
            // Target Temp
            pot3[0] = ads.ReadSymbolInfo("gbl.master_TargetTemp");
            PT102[0] = Convert.ToDouble(ads.ReadSymbol(pot3[0]));
            TempTunetbox1.Text = "\r\n" + PT102[0].ToString();

            // Gain(P)
            for (int i = 0; i < 3; i++)
            {
                pot3[i] = ads.ReadSymbolInfo($"gbl.master_pi_gain[{i + 1}]");
                PT102[i] = Convert.ToDouble(ads.ReadSymbol(pot3[i]));
                TempTunetbox2.Text = "\r\n" + PT102[i].ToString();
            }

            // i(적분)
            for (int i = 0; i < 3; i++)
            {
                pot3[i] = ads.ReadSymbolInfo($"gbl.master_integral_constant[{i + 1}]");
                PT102[i] = Convert.ToDouble(ads.ReadSymbol(pot3[i]));
                TempTunetbox3.Text = "\r\n" + PT102[i].ToString();
            }

            // Tune MAX
            pot3[0] = ads.ReadSymbolInfo("gbl.master_Zone_Temp_Max");
            PT102[0] = Convert.ToDouble(ads.ReadSymbol(pot3[0]));
            TempTunetbox4.Text = "\r\n" + PT102[0].ToString();

            // Tune MIN
            pot3[0] = ads.ReadSymbolInfo("gbl.master_Zone_Temp_Min");
            PT102[0] = Convert.ToDouble(ads.ReadSymbol(pot3[0]));
            TempTunetbox5.Text = "\r\n" + PT102[0].ToString();

            // Tune Time
            pot3[0] = ads.ReadSymbolInfo("gbl.tuning_time");
            pot3[1] = ads.ReadSymbolInfo("gbl.tune_Remaining_time");
            PT102[0] = Convert.ToDouble(ads.ReadSymbol(pot3[0]));
            PT102[1] = Convert.ToDouble(ads.ReadSymbol(pot3[1]));
            str5 = String.Format("{0:0.00}", PT102[1]);
            TempTunetbox6.Text = PT102[0].ToString() + "\r\n";
            lblRemain_Time.Text = str5;

            // e_Ctrl_Mode
            for (int i = 0; i <= 3; i++)
            {
                pot3[i] = ads.ReadSymbolInfo($"gbl.slave_eCTRL_MODE[{i + 1}]");
                PT102[i] = Convert.ToDouble(ads.ReadSymbol(pot3[i]));

                if (PT102[i].ToString() == 0.ToString())
                {
                    Ctrl[i].Text = "\r\n" + "IDLE";
                }
                else if (PT102[i].ToString() == 2.ToString())
                {
                    Ctrl[i].Text = "\r\n" + "ACTIVE";
                }
            }

            //Tune Set 초기값
            comboBox1.SelectedIndex = 1;

            //차트 설정
            chart1.Series.Clear();
            for (int i = 1; i < 10; i++)
            {
                Series STChart1 = chart1.Series.Add($"SubStrate{i}");
                STChart1.ChartType = SeriesChartType.Line;
            }

            thread1.Start();
            thread2.Start();

            //DB 마지막 에러 조회
            using (OracleConnection conn = new OracleConnection("Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)" +
             "(HOST=localhost)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)" +
             "(SERVICE_NAME=xe)));User Id=hr;Password=hr;"))
            using (OracleCommand cmd = new OracleCommand("select error_id from(select * from log order by error_date desc, num desc) where rownum = 1", conn))
            {
                conn.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    string str = reader["error_id"] as string;
                    txtError.Text = "Last_Error = " + str.ToString();
                    reader.Close();
                    conn.Close();
                }
            }
        }

        // ErrorMessage Enum
        enum eCTRL_ERROR_Message
        {
            eCTRL_ERROR_NOERROR = 0, /* no error */
            eCTRL_ERROR_INVALIDTASKCYCLETIME = 1, /* invalid task cycle time */
            eCTRL_ERROR_INVALIDCTRLCYCLETIME = 2, /* invalid ctrl cycle time */
            eCTRL_ERROR_INVALIDPARAM = 3, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_Tv = 4, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_Td = 5, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_Tn = 6, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_Ti = 7, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_fHystereisisRange = 8, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_fPosOutOn_Off = 9, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_fNegOutOn_Off = 10, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_TableDescription = 11, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_TableData = 12, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_DataTableADR = 13, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_T0 = 14, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_T1 = 15, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_T2 = 16, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_T3 = 17, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_Theta = 18, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_nOrder = 19, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_Tt = 20, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_Tu = 21, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_Tg = 22, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_infinite_slope = 23, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_fMaxIsLessThanfMin = 24, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_fOutMaxLimitIsLessThanfOutMinLimit = 25, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_fOuterWindow = 26, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_fInnerWindow = 27, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_fOuterWindowIsLessThanfInnerWindow = 28, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_fDeadBandInput = 29, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_fDeadBandOutput = 30, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_PWM_Cycletime = 31, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_no_Parameterset = 32, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_fOutOn = 33, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_fOutOff = 34, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_fGain = 35, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_fOffset = 36, /* invalid parameter */
            eCTRL_ERROR_MODE_NOT_SUPPORTED = 37, /* invalid mode: mode not supported*/
            eCTRL_ERROR_INVALIDPARAM_Tv_heating = 38, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_Td_heating = 39, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_Tn_heating = 40, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_Tv_cooling = 41, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_Td_cooling = 42, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_Tn_cooling = 43, /* invalid parameter */
            eCTRL_ERROR_RANGE_NOT_SUPPORTED = 44, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_nParameterChangeCycleTicks = 45, /* invalid parameter */
            eCTRL_ERROR_ParameterEstimationFailed = 46, /* invalid parameter */
            eCTRL_ERROR_NoiseLevelToHigh = 47, /* invalid parameter */
            eCTRL_ERROR_INTERNAL_ERROR_0 = 48, /* internal error*/
            eCTRL_ERROR_INTERNAL_ERROR_1 = 49, /* internal error*/
            eCTRL_ERROR_INTERNAL_ERROR_2 = 50, /* internal error*/
            eCTRL_ERROR_INTERNAL_ERROR_3 = 51, /* internal error*/
            eCTRL_ERROR_INTERNAL_ERROR_4 = 52, /* internal error*/
            eCTRL_ERROR_INTERNAL_ERROR_5 = 53, /* internal error*/
            eCTRL_ERROR_INTERNAL_ERROR_6 = 54, /* internal error*/
            eCTRL_ERROR_INTERNAL_ERROR_7 = 55, /* internal error*/
            eCTRL_ERROR_INTERNAL_ERROR_8 = 56, /* internal error*/
            eCTRL_ERROR_INTERNAL_ERROR_9 = 57, /* internal error*/
            eCTRL_ERROR_INVALIDPARAM_WorkArrayADR = 58, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_tOnTiime = 59, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_tOffTiime = 60, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_nMaxMovingPulses = 61, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_nAdditionalPulsesAtLimits = 62, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_fCtrlOutMax_Min = 63, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_fDeltaMax = 64, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_tMovingTime = 65, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_tDeadTime = 66, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_tAdditionalMoveTimeAtLimits = 67, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_fThreshold = 68, /* invalid parameter */
            eCTRL_ERROR_MEMCPY = 69, /* MEMCPY failed */
            eCTRL_ERROR_MEMSET = 70, /* MEMSET failed */
            eCTRL_ERROR_INVALIDPARAM_nNumberOfColumns = 71, /* invalid parameter */
            eCTRL_ERROR_FileClose = 72, /* File Close failed*/
            eCTRL_ERROR_FileOpen = 73, /* File Open failed*/
            eCTRL_ERROR_FileWrite = 74, /* File Write failed*/
            eCTRL_ERROR_INVALIDPARAM_fVeloNeg = 75, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_fVeloPos = 76, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_DeadBandInput = 77, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_DeadBandOutput = 78, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_CycleDuration = 79, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_tStart = 80, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_StepHeigthTuningToLow = 81, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_fMinLimitIsLessThanZero = 82, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_fMaxLimitIsGreaterThan100 = 83, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_fStepSize = 84, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_fOkRangeIsLessOrEqualZero = 85, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_fForceRangeIsLessOrEqualfOkRange = 86, /* invalid parameter */
            eCTRL_ERROR_INVALIDPWMPeriod = 87, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_tMinimumPulseTime = 88, /* invalid parameter */
            eCTRL_ERROR_FileDelete = 89, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_nNumberOfPwmOutputs = 90, /* File Delete failed*/
            eCTRL_ERROR_INVALIDPARAM_nPwmInputArray_SIZEOF = 91, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_nPwmOutputArray_SIZEOF = 92, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_nPwmWaitTimesConfig_SIZEOF = 93, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_nPwmInternalData_SIZEOF = 94, /* invalid parameter */
            eCTRL_ERROR_SIZEOF = 95, /* SIZEOF failed */
            eCTRL_ERROR_INVALIDPARAM_nOrderOfTheTransferfunction = 96, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_nNumeratorArray_SIZEOF = 97, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_nDenominatorArray_SIZEOF = 98, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_a_n_IsZero = 99, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_WorkArraySIZEOF = 100, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_MOVINGRANGE_MIN_MAX = 101, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_MOVINGTIME = 102, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_DEADTIME = 103, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_fMinLimitIsGreaterThanfMaxLimit = 104, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_DataTableSIZEOF = 105, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_OutputVectorDescription = 106, /* invalid parameter */
            eCTRL_ERROR_TaskCycleTimeChanged = 107, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_nMinMovingPulses = 108, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_fAcceleration = 109, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_fDeceleration = 110, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_StartAndTargetPos = 111, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_fVelocity = 112, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_fTargetPos = 113, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_fStartPos = 114, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_fMovingLength = 115, /* invalid parameter */
            eCTRL_ERROR_NT_GetTime = 116, /* internal error NT_GetTime */
            eCTRL_ERROR_INVALIDPARAM_No3PhaseSolutionPossible = 117, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_fStartVelo = 118, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_fTargetVelo = 119, /* invalid parameter */
            eCTRL_ERROR_INVALID_NEW_PARAMETER_TYPE = 120,  /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_fBaseTime = 121, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_nOrderOfTheTransferfunction_SIZEOF = 122, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_nFilterOrder = 124, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_nCoefficientsArray_a_SIZEOF = 125, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_nCoefficientsArray_b_SIZEOF = 126, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_nDigitalFiterData_SIZEOF = 127, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_nLogBuffer_SIZEOF = 128, /* invalid parameter */
            eCTRL_ERROR_LogBufferOverflow = 129, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_nLogBuffer_ADR = 130, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_nCoefficientsArray_a_ADR = 131, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_nCoefficientsArray_b_ADR = 132, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_nPwmInputArray_ADR = 133, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_nPwmOutputArray_ADR = 134, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_nPwmWaitTimesConfig_ADR = 135, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_nPwmInternalData_ADR = 136, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_nDigitalFiterData_ADR = 137, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_nNumeratorArray_ADR = 138, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_nDenominatorArray_ADR = 139, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_nTransferfunction1Data_ADR = 140, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_nTransferfunction2Data_ADR = 141, /* invalid parameter */
            eCTRL_ERROR_FileSeek = 142, /* internal error FB_FileSeek */
            eCTRL_ERROR_INVALIDPARAM_AmbientTempMaxIsLessThanAmbientTempMin = 143, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_ForerunTempMaxIsLessThanForerunTempMin = 144, /* invalid parameter */
            eCTRL_ERROR_INVALIDLOGCYCLETIME = 145, /* invalid parameter */
            eCTRL_ERROR_INVALIDVERSION_TcControllerToolbox = 146,
            eCTRL_ERROR_INVALIDPARAM_Bandwidth = 147, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_NotchFrequency = 148, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_DampingCoefficient = 149, /* invalid parameter */
            eCTRL_ERROR_INVALIDPARAM_fKpIsLessThanZero = 150  /* invalid parameter */
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
                    str = String.Format("{0:0.00}", Act_Temp[i]);
                    this.Controls["ActTempTB" + i].Text = "\r\n" + str;
                    //Thread.Sleep(10);
                }
                cnt = 1;

                //Work Set 출력
                for (int i = 0; i < 4; i++)
                {

                    pot[i] = ads.ReadSymbolInfo($"gbl.slave_fRamp_Out[{cnt++}]");
                    Work_Set[i] = Convert.ToDouble(ads.ReadSymbol(pot[i]));
                    str = String.Format("{0:0.00}", Work_Set[i]);
                    this.Controls["WorkSetTB" + i].Text = "\r\n" + str;
                    //Thread.Sleep(10);
                }
                cnt = 1;

                //Act Power 출력
                for (int i = 0; i < 4; i++)
                {

                    pot[i] = ads.ReadSymbolInfo($"gbl.slave_fMV_Out[{cnt++}]");
                    Act_Power[i] = Convert.ToDouble(ads.ReadSymbol(pot[i]));
                    str = String.Format("{0:0.00}", Act_Power[i]);
                    this.Controls["ActPowerTB" + i].Text = "\r\n" + str;
                    //Thread.Sleep(10);
                }
                cnt = 1;

                //Tune Result 출력
                for (int i = 0; i < 4; i++)
                {

                    pot[i] = ads.ReadSymbolInfo($"gbl.tune_result[{cnt++}]");
                    Tune_Result[i] = Convert.ToDouble(ads.ReadSymbol(pot[i]));
                    str = String.Format("{0:0.00}", Tune_Result[i]);
                    this.Controls["TuneResultTB" + i].Text = "\r\n" + str;
                    //Thread.Sleep(10);
                }
                cnt = 1;

                //Substrate Temp 출력
                for (int i = 0; i < 9; i++)
                {

                    pot[i] = ads.ReadSymbolInfo($"gbl.fPV_Value_SubTC[{cnt++}]");
                    Substrate_Temp[i] = Convert.ToDouble(ads.ReadSymbol(pot[i]));
                    str = String.Format("{0:0.00}", Substrate_Temp[i]);
                    this.Controls["SubstrateTemp" + i].Text = "\r\n" + str;
                    //Thread.Sleep(10);
                }
                cnt = 1;

                pot3[0] = ads.ReadSymbolInfo("gbl.tuning_time");
                pot3[1] = ads.ReadSymbolInfo("gbl.tune_Remaining_time");
                PT102[0] = Convert.ToDouble(ads.ReadSymbol(pot3[0]));
                PT102[1] = (Convert.ToDouble(ads.ReadSymbol(pot3[1])) / 60) * 0.001;
                str5 = String.Format("{0:0.00}", PT102[1]);
                lblRemain_Time.Text = "Reamin Time = " + str5;

                pot[25] = ads.ReadSymbolInfo("gbl.subTC_max");
                pot[26] = ads.ReadSymbolInfo("gbl.subTC_min");
                pot[27] = ads.ReadSymbolInfo("gbl.subTC_AVG");
                pot[28] = ads.ReadSymbolInfo("gbl.subTC_dev");
                pot[29] = ads.ReadSymbolInfo("gbl.tune_Remaining_time");
                Substrate_Temp2[0] = Convert.ToDouble(ads.ReadSymbol(pot[25]));
                Substrate_Temp2[1] = Convert.ToDouble(ads.ReadSymbol(pot[26]));
                Substrate_Temp2[2] = Convert.ToDouble(ads.ReadSymbol(pot[27]));
                Substrate_Temp2[3] = Convert.ToDouble(ads.ReadSymbol(pot[28]));
                str = String.Format("{0:0.00}", Substrate_Temp2[0]);
                str2 = String.Format("{0:0.00}", Substrate_Temp2[1]);
                str3 = String.Format("{0:0.00}", Substrate_Temp2[2]);
                str4 = String.Format("{0:0.00}", Substrate_Temp2[3]);
                this.Controls["ST" + 0].Text = str;
                this.Controls["ST" + 1].Text = str2;
                this.Controls["ST" + 2].Text = str3;
                this.Controls["ST" + 3].Text = str4;

                chart1.Series[0].Points.AddXY(k, Convert.ToDouble(SubstrateTemp0.Text));
                chart1.Series[1].Points.AddXY(k, Convert.ToDouble(SubstrateTemp1.Text));
                chart1.Series[2].Points.AddXY(k, Convert.ToDouble(SubstrateTemp2.Text));
                chart1.Series[3].Points.AddXY(k, Convert.ToDouble(SubstrateTemp3.Text));
                chart1.Series[4].Points.AddXY(k, Convert.ToDouble(SubstrateTemp4.Text));
                chart1.Series[5].Points.AddXY(k, Convert.ToDouble(SubstrateTemp5.Text));
                chart1.Series[6].Points.AddXY(k, Convert.ToDouble(SubstrateTemp6.Text));
                chart1.Series[7].Points.AddXY(k, Convert.ToDouble(SubstrateTemp7.Text));
                chart1.Series[8].Points.AddXY(k, Convert.ToDouble(SubstrateTemp8.Text));

                k++;
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
                pot[28] = ads.ReadSymbolInfo("gbl.bWatch");
                Watchdog1 = Convert.ToBoolean(ads.ReadSymbol(pot[28]));
                this.Controls["label" + 36].Text = Watchdog1.ToString();
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
            pot4 = ads.ReadSymbolInfo($"gbl.slave_bError");
            bError = Convert.ToBoolean(ads.ReadSymbol(pot4));

            //DB
            string strConn = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=xe)));User Id=hr;Password=hr";
            // 1. 연결 객체 만들기 - Client
            OracleConnection conn = new OracleConnection(strConn);
            // 2. 데이터베이스 접속을 위한 연결
            conn.Open();
            // 3. 서버와 함께 신나게 놀기 
            // ~~~~~~~~~~~
            // 3.1 Query 명령객체 만들기
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            if (bError == true)
            {
                Error_True();

                pot4 = ads.ReadSymbolInfo($"gbl.slave_nErrorID");
                Error_MSG[0] = Convert.ToString(ads.ReadSymbol(pot4));
                txtError.Text = string.Format("Error = " + ((eCTRL_ERROR_Message)Enum.ToObject(typeof(eCTRL_ERROR_Message), Convert.ToInt32(Error_MSG[0]))).ToString().Substring(12) + "\r\n");

                // 에러가 발생하면 DB에 에러내용 입력
                cmd.CommandText = $"INSERT INTO LOG VALUES ({count},{Error_MSG[0]},to_char(sysdate, 'yyyy-mm-dd hh24:mi:ss'),'{((eCTRL_ERROR_Message)Enum.ToObject(typeof(eCTRL_ERROR_Message), Convert.ToInt32(Error_MSG[0]))).ToString().Substring(12)}')";
                cmd.ExecuteNonQuery();

                count++;

                conn.Close();
            }
            else if (bError == false)
            {
                pictureBox1.Load(@"C:\image\녹색등.png");
            }

            bStart = ads.CreateVariableHandle($"gbl.slave_bstart");
            ads.WriteAny(bStart, true);

            // e_Ctrl_Mode
            for (int i = 0; i <= 3; i++)
            {
                pot3[i] = ads.ReadSymbolInfo($"gbl.slave_eCTRL_MODE[{i + 1}]");
                PT102[i] = Convert.ToDouble(ads.ReadSymbol(pot3[i]));

                if (PT102[i].ToString() == 0.ToString())
                {
                    Ctrl[i].Text = "\r\n" + "IDLE";
                }
                else if (PT102[i].ToString() == 2.ToString())
                {
                    Ctrl[i].Text = "\r\n" + "ACTIVE";
                }
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
                pictureBox1.Load(@"C:\image\켜진등.png");
                Delay(150);
                pictureBox1.Load(@"C:\image\꺼진등.png");
                Delay(150);
            }
            pictureBox1.Load(@"C:\image\켜진등.png");
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

            pot4 = ads.ReadSymbolInfo($"gbl.slave_nErrorID");
            Error_MSG[0] = Convert.ToString(ads.ReadSymbol(pot4));

            Error_True();

            txtError.Text = string.Format("Error = EmergencyStop\r\n");

            //DB
            string strConn = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=xe)));User Id=hr;Password=hr";
            // 1. 연결 객체 만들기 - Client
            OracleConnection conn = new OracleConnection(strConn);
            // 2. 데이터베이스 접속을 위한 연결
            conn.Open();
            // 3. 서버와 함께 신나게 놀기 
            // ~~~~~~~~~~~
            // 3.1 Query 명령객체 만들기
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            // 에러가 발생하면 DB에 에러내용 입력
            cmd.CommandText = $"INSERT INTO LOG VALUES ({count},{Error_MSG[0]},to_char(sysdate, 'yyyy-mm-dd hh24:mi:ss'),'EmergencyStop')";
            cmd.ExecuteNonQuery();

            count++;
            conn.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Error_Log log = new Error_Log();
            log.Show();
        }

        // 값 한번에 넣기
        // Temp_Set
        private void label18_Click(object sender, EventArgs e)
        {
            Temp_Set_Pop temp_set_pop = new Temp_Set_Pop();
            temp_set_pop.StartPosition = FormStartPosition.Manual;
            temp_set_pop.Location = new Point(420, 75);
            temp_set_pop.Show();
        }
        // Ramp_Set
        private void label6_Click(object sender, EventArgs e)
        {
            Ramp_Set_Pop ramp_set_pop = new Ramp_Set_Pop();
            ramp_set_pop.StartPosition = FormStartPosition.Manual;
            ramp_set_pop.Location = new Point(547, 75);
            ramp_set_pop.Show();
        }
        // Power Set
        private void label5_Click(object sender, EventArgs e)
        {
            Power_Set_Pop power_set_pop = new Power_Set_Pop();
            power_set_pop.StartPosition = FormStartPosition.Manual;
            power_set_pop.Location = new Point(674, 75);
            power_set_pop.Show();
        }
    }
}
