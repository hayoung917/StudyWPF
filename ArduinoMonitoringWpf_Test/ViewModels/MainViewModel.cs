using ArduinoMonitoringWpf_Test.Helpers;
using ArduinoMonitoringWpf_Test.Models;
using Caliburn.Micro;
using LiveCharts;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Windows;
using System.Windows.Forms;
using static ArduinoMonitoringWpf_Test.Helpers.Commons;
using LiveCharts.Wpf;
using System.Windows.Media;
using LiveCharts.Dtos;
using MessageBox = System.Windows.Forms.MessageBox;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;

namespace ArduinoMonitoringWpf_Test.ViewModels
{
    public class MainViewModel : Conductor<object>, IHaveDisplayName
    {
        #region 속성 영역
        RelayCommand command_Alt_S;
        RelayCommand command_Alt_T;
        RelayCommand command_Ctrl_O;
        RelayCommand command_Ctrl_S;

        public ICommand Command_Alt_S
        {
            get => command_Alt_S;
        }

        public ICommand Command_Alt_T
        {
            get => command_Alt_T;
        }

        public ICommand Command_Ctrl_O
        {
            get => command_Ctrl_O;
        }

        public ICommand Command_Ctrl_S
        {
            get => command_Ctrl_S;
        }

        SerialPort serial;
        //private short xCount = 200;
        private short maxPhotoVal = 1023;
        List<SensorData> photoDatas = new List<SensorData>();

        public bool IsSimulation { get; set; }

        Timer timer = new Timer();
        Random rand = new Random();
        public BindableCollection<string> CboSerialPort { get; set; }
     
        //콤보박스
        string selectedPort;
        public string SelectedPort
        {
            get => selectedPort;
            set
            {
                selectedPort = value;
                SerialSetting();
                NotifyOfPropertyChange(() => SelectedPort);
                NotifyOfPropertyChange(() => CanBtnDisconnect);
                NotifyOfPropertyChange(() => CanBtnConnect);
                NotifyOfPropertyChange(() => IsDisconnect);
                NotifyOfPropertyChange(() => IsConnect);
            }
        }

        //연결시간
        string lblConnectionTime;
        public string LblConnectionTime
        {
            get => lblConnectionTime;
            set
            {
                lblConnectionTime = value;
                NotifyOfPropertyChange(() => LblConnectionTime);
                
            }
        }

        //연결횟수
        string txtSensorCount;
        public string TxtSensorCount
        {
            get => txtSensorCount;
            set
            {
                txtSensorCount = value;
                NotifyOfPropertyChange(() => TxtSensorCount);
                
            }
        }

        //프로그래스바
        string pgbPhotoRegistor;
        public string PgbPhotoRegistor
        {
            get => pgbPhotoRegistor;
            set
            {
                pgbPhotoRegistor = value;
                NotifyOfPropertyChange(() => PgbPhotoRegistor);
                
            }
        }

        //프로그래스바 값
        string lblPhotoRegistor;
        public string LblPhotoRegistor
        {
            get => lblPhotoRegistor;
            set
            {
                lblPhotoRegistor = value;
                NotifyOfPropertyChange(() => LblPhotoRegistor);
            }
        }

        //데이터 로그
        string rtbLog;
        public string RtbLog
        {
            get => rtbLog;
            set
            {
                rtbLog = value;
                NotifyOfPropertyChange(() => RtbLog);
            }
        }

        //차트
        public SeriesCollection ChartLive { get; set; }
        
        //connect/disconnect 버튼 활성화
        bool isConnect = false;
        public bool IsConnect
        { 
            get => isConnect;
            set
            {
                isConnect = value;
                NotifyOfPropertyChange(() => IsConnect);
                NotifyOfPropertyChange(() => CanBtnConnect);
            }
        }

        bool isDisconnect = false;
        public bool IsDisconnect
        {
            get => isDisconnect;
            set
            {
                isDisconnect = value;
                NotifyOfPropertyChange(() => IsDisconnect);
                NotifyOfPropertyChange(() => CanBtnDisconnect);
            }
        }
        public bool CanBtnDisconnect
        {
            get
            {
                if (!string.IsNullOrEmpty(selectedPort) && IsDisconnect)
                    return true;
                else
                    return false;
            }
        }

        //port 버튼
        string btnPortValue = "PORT";
        public string BtnPortValue
        {
            get => btnPortValue;
            set
            {
                btnPortValue = value;
                NotifyOfPropertyChange(() => BtnPortValue);
            }
        }

        public bool CanBtnConnect
        {
            get
            {
                if (!string.IsNullOrEmpty(selectedPort) && IsConnect)
                    return true;
                else
                    return false;
            }
        }

        public string FilePathTextBox { get; private set; }
        #endregion

        #region 생성자 영역
        
        public MainViewModel()
        {
            InitControls();
        }

        private void InitControls()
        {
            command_Alt_S = new RelayCommand(BtnStart);
            command_Alt_T = new RelayCommand(BtnStop);
            command_Ctrl_S = new RelayCommand(BtnSave);
            command_Ctrl_O = new RelayCommand(BtnOpen);

            CboSerialPort = new BindableCollection<string>();
            foreach (var item in SerialPort.GetPortNames())
            {
                CboSerialPort.Add(item);
            }
            //콤보박스 확인    
            //CboSerialPort.Add("1");
            //CboSerialPort.Add("2");
            //CboSerialPort.Add("3");
            //CboSerialPort.Add("4");

            ChartLive = new SeriesCollection()
            {
                
                new LineSeries
                {
                    Title = "PotoValue",
                    Values = new ChartValues<int>(),
                    Fill = Brushes.Transparent,
                    Stroke = Brushes.Teal,
                    PointGeometrySize = 6
                }
            };
        }

        #endregion
        //DB연결
        public void InsertDataToDB(SensorData data)
        {
            using (MySqlConnection conn = new MySqlConnection(Commons.CONNSTRING))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(Sensordatatbl.STRQUERY, conn);
                MySqlParameter paramDate = new MySqlParameter("@Date", MySqlDbType.DateTime)
                {
                    Value = data.Date
                };
                cmd.Parameters.Add(paramDate);
                MySqlParameter paramValue = new MySqlParameter("@Value", MySqlDbType.Int32)
                {
                    Value = data.Value
                };
                cmd.Parameters.Add(paramValue);
                cmd.ExecuteNonQuery();
            }
        }

        public void DisplayValue(string sVal)
        {
            try
            {
                ushort v = ushort.Parse(sVal);
                if (v < 0 || v > maxPhotoVal)
                    return;

                SensorData data = new SensorData(DateTime.Now, v);
                photoDatas.Add(data);
                InsertDataToDB(data);

                TxtSensorCount = photoDatas.Count.ToString();
                PgbPhotoRegistor = v.ToString();
                LblPhotoRegistor = v.ToString();
                BtnPortValue = v.ToString();

                string item = $"{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}\t{v}";

                RtbLog += item.ToString() + "\n";
                ChartLive[0].Values.Add((int)v);
                //ChtSensorValues.Series[0].Points.Add(v);

                if (IsSimulation == false)
                    BtnPortValue = $"{serial.PortName}\n{sVal}";
                else
                    BtnPortValue = $"{sVal}";
            }
            catch (Exception ex)
            {
                RtbLog = ($"Error : {ex.Message}\n");
                //RtbLog.ScrollToCaret();
            }
        }
        public void BtnInfo()
        {
            IWindowManager btninfo = new WindowManager();
            btninfo.ShowDialog(new InfoViewModel(), null, null);
        }

        public void BtnStart()
        {
            IsSimulation = true;
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            timer.Start();

            LblConnectionTime = $"연결시간 : {DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}";
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            ushort value = (ushort)rand.Next(1, 1024);
            DisplayValue(value.ToString());
        }

        public void BtnStop()
        {
            timer.Stop();
            IsSimulation = false;

            // serial 통신 재시작
            //BtnConnect_Click(sender, e);
        }

        public void BtnMenuExit()
        {
            (GetView() as Window).Close();
            //Environment.Exit(0);
        }

        public void BtnConnect()
        {
            serial.Open();
            LblConnectionTime = $"연결시간 : {DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}";

            IsConnect = false;
            IsDisconnect = true;
            //BtnConnect.Enabled = false;
            //BtnDisconnect.Enabled = true;
        }

        public void BtnDisconnect()
        {
            serial.Close();

            IsConnect = true;
            IsDisconnect = false;
            //BtnConnect.Enabled = true;
            //BtnDisconnect.Enabled = false;
        }

        public void SerialSetting()
        {
            //var portName = CboSerialPort.selectedPort.ToString();
            serial = new SerialPort(selectedPort);
            serial.DataReceived += Serial_DataReceived;

            IsConnect = true;
            //BtnConnect.Enabled = true;
        }

        public void Serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string sVal = serial.ReadLine();
            DisplayValue(sVal.ToString());
            //this.BeginInvoke(new Action(delegate { DisplayValue(sVal); }));
        }

        public void BtnViewAll()
        {
            ChartLive.Chart.ClearZoom();
        }
        
        public void BtnZoom()
        {
            //ChtSensorValues.ChartAreas[0].AxisX.Minimum = 0;
            //ChtSensorValues.ChartAreas[0].AxisX.Maximum =
            //    (photoDatas.Count >= xCount) ? photoDatas.Count : xCount;

            //if (photoDatas.Count > xCount)
            //    ChtSensorValues.ChartAreas[0].AxisX.ScaleView.Zoom(
            //        photoDatas.Count - xCount, photoDatas.Count);
            //else
            //    ChtSensorValues.ChartAreas[0].AxisX.ScaleView.Zoom(0, xCount);
            
            ChartLive.Chart.ZoomIn(new CorePoint(1000, 0));
        }
        
        public void BtnOpen()
        {
            OpenFileDialog dlgOpenFile = new OpenFileDialog();
            dlgOpenFile.Filter = "Image Files (*.jpg, *.png) | *.jpg; *.png; | All files (*.*) | *.*";

            if (dlgOpenFile.ShowDialog() == DialogResult.OK)
            {
                // 텍스트 박스에 파일 경로 쓰기
                FilePathTextBox = dlgOpenFile.FileName;
                MessageBox.Show(FilePathTextBox);
                //RtbLog += dlgOpenFile.FileName;
            }
        }

        public void BtnSave()
        {
             MessageBox.Show("Complete");
        }
    }
}
