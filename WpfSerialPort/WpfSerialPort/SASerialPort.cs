using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Threading;
using System.IO.Ports;


namespace WpfSerialPort
{
    //class SASerialPort
    //{
    //}

    public partial class MainWindow : Window
    {
        /// <summary>
        /// SerialPort对象
        /// </summary>
        private SerialPort SPort = new SerialPort();
        // 需要一个定时器用来，用来保证即使缓冲区没满也能够及时将数据处理掉，防止一直没有到达
        // 阈值，而导致数据在缓冲区中一直得不到合适的处理。
        private DispatcherTimer CheckTimer = new DispatcherTimer(); //在指定时间间隔和指定优先级处理的队列

        private void InitSerialPort()
        {
            SPort.DataReceived += SerialPort_DataReceived;
            //InitCheckTimer();
        }

        #region 定时器
        /// <summary>
        /// 超时时间为50ms
        /// </summary>
        private const int TIMEOUT = 50;
        //private void InitCheckTimer()
        //{
        //    // 如果缓冲区中有数据，并且定时时间达到前依然没有得到处理，将会自动触发处理函数。
        //    CheckTimer.Interval = new TimeSpan(0, 0, 0, 0, TIMEOUT);
        //    CheckTimer.IsEnabled = false;
        //    CheckTimer.Tick += CheckTimer_Tick;
        //}


        private void StartCheckTimer()
        {
            CheckTimer.IsEnabled = true;
            CheckTimer.Start();
        }
        private void StopCheckTimer()
        {
            CheckTimer.IsEnabled = false;
            CheckTimer.Stop();
        }

        #endregion




    }
}
