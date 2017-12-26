using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO.Ports;
using System.Threading;

namespace WpfSerialPort
{
    //class EventHandler
    //{
    //}

    public partial class MainWindow : Window
    {

        #region Event handler for menu items
        private void TestNoneRadioButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TestRetRadioButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TestNewlineRadioButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TestNewlineRetRadioButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SerialSettingViewMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AutoSendDataSettingViewMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SerialCommunicationSettingViewMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CompactViewMenuItem_Click(object sender, RoutedEventArgs e)
        {


        }

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void HelpMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region EventHandler for serialPort
        // 数据接收缓冲区
        private List<byte> receiveBuffer = new List<byte>();
        // 一个阈值，当接收的字节数大于这么多字节数之后，就将当前的buffer内容交由数据处理的线程
        // 分析。这里存在一个问题，假如最后一次传输之后，缓冲区并没有达到阈值字节数，那么可能就
        // 没法启动数据处理的线程将最后一次传输的数据处理了。这里应当设定某种策略来保证数据能够
        // 在尽可能短的时间内得到处理。
        private const int THRESH_VALUE = 128;
        private bool shouldClear = true;
        /// <summary>
        /// 更新：采用一个缓冲区，当有数据到达时，把字节读取出来暂存到缓冲区中，缓冲区到达定值
        /// 时，在显示区显示数据即可。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = sender as SerialPort;
            if (sp != null)
            {
                // 临时缓冲区将保存串口缓冲区的所有数据
                int bytesToRead = sp.BytesToRead;  //获取接收缓冲区中数据的字节数
                byte[] tempBuffer = new byte[bytesToRead];

                // 将缓冲区所有字节读取出来
                sp.Read(tempBuffer, 0, bytesToRead);

                // 检查是否需要清空全局缓冲区先
                if (shouldClear)
                {
                    receiveBuffer.Clear();
                    shouldClear = false;
                }

                // 暂存缓冲区字节到全局缓冲区中等待处理
                receiveBuffer.AddRange(tempBuffer);  //将指定几何的元素添加到列表末尾

                if (receiveBuffer.Count >= THRESH_VALUE)
                {
                    // 进行数据处理，采用新的线程进行处理。
                    Thread dataHandler = new Thread(new ParameterizedThreadStart(ReceiveDataHandler));
                    dataHandler.Start(receiveBuffer);  //启动线程
                }
                // 启动定时器，防止因为一直没有到达缓冲区字节阈值，而导致接收到的数据一直留存在缓冲区中无法处理。
                StartCheckTimer();
            }

        }

        #endregion
        private void ReceiveDataHandler(object obj)
        {
            List<byte> recvBuffer = new List<byte>();
            recvBuffer.AddRange((List<byte>)obj);   //需要强制转换一下，将参数传给了thread的Start方法

            if (recvBuffer.Count == 0)
            {
                return;
            }
            shouldClear = true;

            this.Dispatcher.Invoke(new Action(() =>
                {
                    if (AutoSendEnableCheckBox.IsChecked == false)
                    {
                        Information("");
                    }
                }));
        }





    }
}
