using System;
using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Threading;
using System.IO.Ports;
using System.IO;
//using System.Diagnostics;
//using System.Windows.Forms;
using ShimmerAPI;
using ShimmerLibrary;
using UnityEngine;
using UnityEngine.UI;

namespace connectionTest
{
    public struct biometricCluster
    {
        public DateTime currentTime;
        public ObjectCluster receivedCluster;
    };

    public class ConnectionTest
    {
       // public static readonly string ApplicationName = Application.ProductName.ToString().Replace("_", " ");
        private string ComPort;
        //private Logging WriteToFile;
        //private OpenFileDialog openDialog = new OpenFileDialog();
        //private SaveFileDialog openDialog = new SaveFileDialog();
        //writing to file
//        private String Delimeter = ",";
        private String basePath = "../../../Logs/";
        private String filePath = "";
        //public StreamWriter outputFile;
        public List<biometricCluster> storedClusterData = new List<biometricCluster>();
        public DateTime lastTimeStreamStarted;
        //PPG-HR
        private PPGToHRAlgorithm PPGtoHeartRateCalculation;
        private Boolean EnablePPGtoHRConversion = false;
        private int NumberOfHeartBeatsToAverage = 5;
//        private int NumberOfHeartBeatsToAverageECG = 1;
        private int TrainingPeriodPPG = 10; //5 second buffer
        //filters
        Filter NQF_Exg1Ch1;
        Filter NQF_Exg1Ch2;
        Filter NQF_Exg2Ch1;
        Filter NQF_Exg2Ch2;
        Filter LPF_PPG;
        Filter HPF_PPG;
        Filter HPF_Exg1Ch1;
        Filter HPF_Exg1Ch2;
        Filter HPF_Exg2Ch1;
        Filter HPF_Exg2Ch2;
        Filter BSF_Exg1Ch1;
        Filter BSF_Exg1Ch2;
        Filter BSF_Exg2Ch1;
        Filter BSF_Exg2Ch2;
        public bool DeviceConnected = false;
        public bool EnableHPF_0_05HZ = false;
        public bool EnableNQF = false;
        public bool EnableHPF_0_5HZ = false;
        public bool EnableHPF_5HZ = false;
        public bool EnableBSF_49_51HZ = false;
        public bool EnableBSF_59_61HZ = false;
        public String PPGSignalName = "Internal ADC A13"; //This is used to identify which signal to feed into the PPF to HR algorithm
        //ExG
//        public String ECGSignalName = "ECG LL-RA"; //This is used to identify which signal to feed into the ECG to HR algorithm
//        private int ExGLeadOffCounter = 0;
//        private int ExGLeadOffCounterSize = 0;
//        private int numberOfHeartBeatsToAverage = 0;

        //bool for handling connection for the first time
        private bool firstTime = true;
        //private System.Windows.Forms.Timer firstTimeTimer;

        //use long constructor here
        public ShimmerSDBT ShimmerDevice = new ShimmerSDBT("Shimmer", "");

        public ConnectionTest()
        {
            //InitializeComponent();
            //Application.ThreadException += new ExceptionEventHandler().ApplicationThreadException;
            AppDomain.CurrentDomain.UnhandledException += new ExceptionEventHandler().CurrentDomainUnhandledException;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //ShimmerDevice = new ShimmerSDBT("Shimmer", ComPort, 20.0,0,0,0,4);
            ShimmerDevice = new ShimmerSDBT("Shimmer", ComPort, 20.0, 0, 0, 0, 4);
            //ShimmerDevice = new ShimmerSDBT("Shimmer", ComPort);
            ShimmerDevice.UICallback += this.HandleEvent;
            populateComPorts();
            //sw.StartNew();
        }

        public void populateComPorts()
        {
            ComPort = comboBoxComPorts.Text;

            String[] names = SerialPort.GetPortNames();
            foreach (String s in names)
            {
                comboBoxComPorts.Items.Add(s);
            }
            comboBoxComPorts.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBoxComPorts.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        public void Connect()
        {
            //for Shimmer and ShimmerSDBT
            ShimmerDevice.SetComPort(comboBoxComPorts.Text);

            //bool connect = true; // check to connect one at a time

            if (ShimmerDevice.GetState() != Shimmer.SHIMMER_STATE_CONNECTED)
            {
                if (!DeviceConnected)
                {
                    DeviceConnected = true;
                    ShimmerDevice.StartConnectThread();
                }
            }

        }

        /**
        public void firstTimeCycleConnect(Object sender, EventArgs e)
        {
            //firstTimeTimer.Stop();
            Console.WriteLine("Prepping to stop streaming");
            ShimmerDevice.StopStreaming();
            Console.WriteLine("prepping to disconnect");
            Disconnect();
            firstTime = false;
            Connect();
        }
        */

        private void connectButton_Click(object sender, EventArgs e)
        {
            Connect();
        }

        public void ChangeStatusLabel(string text)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(ChangeStatusLabel), new object[] { text });
                return;
            }
            labelConnectionState.Text = text; //windows form label; convert to a unity text overlay
        }

        public void HandleEvent(object sender, EventArgs args)
        {
            CustomEventArgs eventArgs = (CustomEventArgs)args;
            int indicator = eventArgs.getIndicator();

            switch (indicator)
            {
                case (int)Shimmer.ShimmerIdentifier.MSG_IDENTIFIER_STATE_CHANGE:
                    //System.Diagnostics.Debug.Write(((Shimmer)sender).GetDeviceName() + " State = " + ((Shimmer)sender).GetStateString() + System.Environment.NewLine);
                    int state = (int)eventArgs.getObject();
                    if (state == (int)Shimmer.SHIMMER_STATE_CONNECTED)
                    {
                        //labelConnectionState.Text = "Connected";
                        ChangeStatusLabel("Connected to " + ShimmerDevice.GetComPort() + ". Firmware Version: " + ShimmerDevice.GetFirmwareVersionFullName());
                        //ChangeStatusLabel("Connected");
                        //firstTimeTimer = new System.Windows.Forms.Timer();
                        //firstTimeTimer.Tick += new EventHandler(firstTimeCycleConnect);
                        //firstTimeTimer.Interval = (int)(5000);
                        //firstTimeTimer.Start();
                    }
                    else if (state == (int)Shimmer.SHIMMER_STATE_CONNECTING)
                    {
                        //labelConnectionState.Text = "Connecting";
                        ChangeStatusLabel("Connecting");
                    }
                    else if (state == (int)Shimmer.SHIMMER_STATE_NONE)
                    {
                        //labelConnectionState.Text = "Disconnected";
                        ChangeStatusLabel("Disconnected");
                    }
                    else if (state == (int)Shimmer.SHIMMER_STATE_STREAMING)
                    {
                        //labelConnectionState.Text = "Streaming";
                        ChangeStatusLabel("Streaming");
                        if (firstTime)
                        {

                        }
                    }
                    break;
                case (int)Shimmer.ShimmerIdentifier.MSG_IDENTIFIER_DATA_PACKET:
                    // this is essential to ensure the object is not a reference
                    DateTime timestamp = DateTime.Now;
                    ObjectCluster objectCluster = new ObjectCluster((ObjectCluster)eventArgs.getObject());
                    List<String> names = objectCluster.GetNames();
                    List<String> formats = objectCluster.GetFormats();
                    List<String> units = objectCluster.GetUnits();
                    List<Double> data = objectCluster.GetData();

                    //add element to list of biometricClusters
                    biometricCluster bc = new biometricCluster();
                    bc.currentTime = timestamp;
                    bc.receivedCluster = objectCluster;
                    storedClusterData.Add(bc);

                    //print log to file
                    writeLogToFile(bc);

                    Console.WriteLine("Current Timestamp: " + timestamp);

                    int gsrRawIndex = objectCluster.GetIndex("GSR", "RAW");
                    int gsrCalIndex = objectCluster.GetIndex("GSR", "CAL");
                    int adcRawIndex = objectCluster.GetIndex("Internal ADC A13", "RAW");
                    int adcCalIndex = objectCluster.GetIndex("Internal ADC A13", "CAL");

                    for (int i = 0; i < names.Count; i++)
                    {
                        //Console.WriteLine(names[i]);
                    }

                    //Console.WriteLine("Current Raw GSR Readings: " + data[gsrRawIndex]);
                    //Console.WriteLine("Current Calculated GSR Readings: " + data[gsrCalIndex]);
                    //Console.WriteLine("Current Raw PPG Readings: " + data[adcRawIndex]);
                    //Console.WriteLine("Current Calculated PPG Readings: " + data[adcCalIndex]);
                    //Console.WriteLine();
                    break;
                case (int)Shimmer.ShimmerIdentifier.MSG_IDENTIFIER_NOTIFICATION_MESSAGE:
                    string message = (string)eventArgs.getObject();
                    System.Diagnostics.Debug.Write(((Shimmer)sender).GetDeviceName() + message + System.Environment.NewLine);
                    //Message BOX
                    int minorIdentifier = eventArgs.getMinorIndication();
                    if (minorIdentifier == (int)ShimmerSDBT.ShimmerSDBTMinorIdentifier.MSG_WARNING)
                    {
                        MessageBox.Show(message, ConnectionTest.ApplicationName,
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (minorIdentifier == (int)ShimmerSDBT.ShimmerSDBTMinorIdentifier.MSG_EXTRA_REMOVABLE_DEVICES_DETECTED)
                    {
                        MessageBox.Show(message, "Message");
                        FolderBrowserDialog fbd = new FolderBrowserDialog();
                        DialogResult result = fbd.ShowDialog();
                        ShimmerDevice.SetDrivePath(fbd.SelectedPath);
                    }
                    else if (minorIdentifier == (int)ShimmerSDBT.ShimmerSDBTMinorIdentifier.MSG_ERROR)
                    {
                        MessageBox.Show(message, ConnectionTest.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (message.Equals("Connection lost"))
                    {
                        MessageBox.Show("Connection with device lost while streaming", ConnectionTest.ApplicationName,
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        ChangeStatusLabel(message);
                    }

                    break;
                default:
                    break;
            }
        }



        public void ChangeGSRLabel(string text)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(ChangeGSRLabel), new object[] { text });
                return;
            }
            currentGSRLabel.Text = text;
        }

        public void Disconnect()
        {
            if (ShimmerDevice != null)
            {
                if (ShimmerDevice.GetState() == (int)Shimmer.SHIMMER_STATE_STREAMING)
                {
                    if (ShimmerDevice.GetFirmwareIdentifier() == 3)
                    {

                    }
                    else
                    {
                        ShimmerDevice.StopStreaming();
                    }
                }
            }
            DeviceConnected = false;
            ShimmerDevice.Disconnect();
            //if (streamingActuallyOccurred())
            {
            //    handleLogging();
            }

        }

        private void disconnectButton_Click(object sender, EventArgs e)
        {
            Disconnect();
        }

        private void streamButton_Click(object sender, EventArgs e)
        {
            //if (openDialog.ShowDialog() == DialogResult.OK)
            //{
            // WriteToFile = new Logging(openDialog.FileName, Delimeter);
            //}
            //filePath = basePath;
            //outputFile = startLogFile();
            Console.WriteLine("Stream button pressed");
            lastTimeStreamStarted = DateTime.Now;
            Console.WriteLine("Attempting to stream");
            ShimmerDevice.StartStreamingandLog();
            createLogFile(lastTimeStreamStarted);
            //ShimmerDevice.StartStreaming();
        }

        private void stopStream()
        {
            ShimmerDevice.StopStreaming();
            //if (streamingActuallyOccurred())
            {
            //    handleLogging();
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            stopStream();
        }

        public void SetNumberOfBeatsToAve(int number)
        {
            NumberOfHeartBeatsToAverage = number;
        }

        private void SetupFilters()
        {
            //Create NQ Filters
            if ((((ShimmerDevice.GetEnabledSensors() & (int)Shimmer.SensorBitmapShimmer3.SENSOR_EXG1_24BIT) > 0) || ((ShimmerDevice.GetEnabledSensors() & (int)Shimmer.SensorBitmapShimmer3.SENSOR_EXG1_16BIT) > 0)) && EnableNQF)
            {
                double cutoff = ShimmerDevice.GetSamplingRate() / 2;
                NQF_Exg1Ch1 = new Filter(Filter.LOW_PASS, ShimmerDevice.GetSamplingRate(), new double[] { cutoff });
                NQF_Exg1Ch2 = new Filter(Filter.LOW_PASS, ShimmerDevice.GetSamplingRate(), new double[] { cutoff });

            }

            if ((((ShimmerDevice.GetEnabledSensors() & (int)Shimmer.SensorBitmapShimmer3.SENSOR_EXG2_24BIT) > 0) || ((ShimmerDevice.GetEnabledSensors() & (int)Shimmer.SensorBitmapShimmer3.SENSOR_EXG2_16BIT) > 0)) && EnableNQF)
            {
                double cutoff = ShimmerDevice.GetSamplingRate() / 2;
                NQF_Exg2Ch1 = new Filter(Filter.LOW_PASS, ShimmerDevice.GetSamplingRate(), new double[] { cutoff });
                NQF_Exg2Ch2 = new Filter(Filter.LOW_PASS, ShimmerDevice.GetSamplingRate(), new double[] { cutoff });

            }

            //Create High Pass Filters for EXG

            if ((((ShimmerDevice.GetEnabledSensors() & (int)Shimmer.SensorBitmapShimmer3.SENSOR_EXG1_24BIT) > 0) || ((ShimmerDevice.GetEnabledSensors() & (int)Shimmer.SensorBitmapShimmer3.SENSOR_EXG1_16BIT) > 0)) && EnableHPF_0_05HZ)
            {
                HPF_Exg1Ch1 = new Filter(Filter.HIGH_PASS, ShimmerDevice.GetSamplingRate(), new double[] { 0.05 });
                HPF_Exg1Ch2 = new Filter(Filter.HIGH_PASS, ShimmerDevice.GetSamplingRate(), new double[] { 0.05 });
            }

            if ((((ShimmerDevice.GetEnabledSensors() & (int)Shimmer.SensorBitmapShimmer3.SENSOR_EXG2_24BIT) > 0) || ((ShimmerDevice.GetEnabledSensors() & (int)Shimmer.SensorBitmapShimmer3.SENSOR_EXG2_16BIT) > 0)) && EnableHPF_0_05HZ)
            {
                HPF_Exg2Ch1 = new Filter(Filter.HIGH_PASS, ShimmerDevice.GetSamplingRate(), new double[] { 0.05 });
                HPF_Exg2Ch2 = new Filter(Filter.HIGH_PASS, ShimmerDevice.GetSamplingRate(), new double[] { 0.05 });
            }
            if ((((ShimmerDevice.GetEnabledSensors() & (int)Shimmer.SensorBitmapShimmer3.SENSOR_EXG1_24BIT) > 0) || ((ShimmerDevice.GetEnabledSensors() & (int)Shimmer.SensorBitmapShimmer3.SENSOR_EXG1_16BIT) > 0)) && EnableHPF_0_5HZ)
            {
                HPF_Exg1Ch1 = new Filter(Filter.HIGH_PASS, ShimmerDevice.GetSamplingRate(), new double[] { 0.5 });
                HPF_Exg1Ch2 = new Filter(Filter.HIGH_PASS, ShimmerDevice.GetSamplingRate(), new double[] { 0.5 });
            }
            if ((((ShimmerDevice.GetEnabledSensors() & (int)Shimmer.SensorBitmapShimmer3.SENSOR_EXG2_24BIT) > 0) || ((ShimmerDevice.GetEnabledSensors() & (int)Shimmer.SensorBitmapShimmer3.SENSOR_EXG2_16BIT) > 0)) && EnableHPF_0_5HZ)
            {
                HPF_Exg2Ch1 = new Filter(Filter.HIGH_PASS, ShimmerDevice.GetSamplingRate(), new double[] { 0.5 });
                HPF_Exg2Ch2 = new Filter(Filter.HIGH_PASS, ShimmerDevice.GetSamplingRate(), new double[] { 0.5 });
            }
            if ((((ShimmerDevice.GetEnabledSensors() & (int)Shimmer.SensorBitmapShimmer3.SENSOR_EXG1_24BIT) > 0) || ((ShimmerDevice.GetEnabledSensors() & (int)Shimmer.SensorBitmapShimmer3.SENSOR_EXG1_16BIT) > 0)) && EnableHPF_5HZ)
            {
                HPF_Exg1Ch1 = new Filter(Filter.HIGH_PASS, ShimmerDevice.GetSamplingRate(), new double[] { 5 });
                HPF_Exg1Ch2 = new Filter(Filter.HIGH_PASS, ShimmerDevice.GetSamplingRate(), new double[] { 5 });
            }
            if ((((ShimmerDevice.GetEnabledSensors() & (int)Shimmer.SensorBitmapShimmer3.SENSOR_EXG2_24BIT) > 0) || ((ShimmerDevice.GetEnabledSensors() & (int)Shimmer.SensorBitmapShimmer3.SENSOR_EXG2_16BIT) > 0)) && EnableHPF_5HZ)
            {
                HPF_Exg2Ch1 = new Filter(Filter.HIGH_PASS, ShimmerDevice.GetSamplingRate(), new double[] { 5 });
                HPF_Exg2Ch2 = new Filter(Filter.HIGH_PASS, ShimmerDevice.GetSamplingRate(), new double[] { 5 });
            }

            //Create Band Stop Filters for EXG
            if ((((ShimmerDevice.GetEnabledSensors() & (int)Shimmer.SensorBitmapShimmer3.SENSOR_EXG1_24BIT) > 0) || ((ShimmerDevice.GetEnabledSensors() & (int)Shimmer.SensorBitmapShimmer3.SENSOR_EXG1_16BIT) > 0)) && EnableBSF_49_51HZ)
            {
                BSF_Exg1Ch1 = new Filter(Filter.BAND_STOP, ShimmerDevice.GetSamplingRate(), new double[] { 49, 51 });
                BSF_Exg1Ch2 = new Filter(Filter.BAND_STOP, ShimmerDevice.GetSamplingRate(), new double[] { 49, 51 });
            }
            if ((((ShimmerDevice.GetEnabledSensors() & (int)Shimmer.SensorBitmapShimmer3.SENSOR_EXG2_24BIT) > 0) || ((ShimmerDevice.GetEnabledSensors() & (int)Shimmer.SensorBitmapShimmer3.SENSOR_EXG2_16BIT) > 0)) && EnableBSF_49_51HZ)
            {
                BSF_Exg2Ch1 = new Filter(Filter.BAND_STOP, ShimmerDevice.GetSamplingRate(), new double[] { 49, 51 });
                BSF_Exg2Ch2 = new Filter(Filter.BAND_STOP, ShimmerDevice.GetSamplingRate(), new double[] { 49, 51 });
            }
            if ((((ShimmerDevice.GetEnabledSensors() & (int)Shimmer.SensorBitmapShimmer3.SENSOR_EXG1_24BIT) > 0) || ((ShimmerDevice.GetEnabledSensors() & (int)Shimmer.SensorBitmapShimmer3.SENSOR_EXG1_16BIT) > 0)) && EnableBSF_59_61HZ)
            {
                BSF_Exg1Ch1 = new Filter(Filter.BAND_STOP, ShimmerDevice.GetSamplingRate(), new double[] { 59, 61 });
                BSF_Exg1Ch2 = new Filter(Filter.BAND_STOP, ShimmerDevice.GetSamplingRate(), new double[] { 59, 61 });
            }
            if ((((ShimmerDevice.GetEnabledSensors() & (int)Shimmer.SensorBitmapShimmer3.SENSOR_EXG2_24BIT) > 0) || ((ShimmerDevice.GetEnabledSensors() & (int)Shimmer.SensorBitmapShimmer3.SENSOR_EXG2_16BIT) > 0)) && EnableBSF_59_61HZ)
            {
                BSF_Exg2Ch1 = new Filter(Filter.BAND_STOP, ShimmerDevice.GetSamplingRate(), new double[] { 59, 61 });
                BSF_Exg2Ch2 = new Filter(Filter.BAND_STOP, ShimmerDevice.GetSamplingRate(), new double[] { 59, 61 });
            }

            //PPG-HR Conversion
            if (EnablePPGtoHRConversion)
            {
                PPGtoHeartRateCalculation = new PPGToHRAlgorithm(ShimmerDevice.GetSamplingRate(), NumberOfHeartBeatsToAverage, TrainingPeriodPPG);
                LPF_PPG = new Filter(Filter.LOW_PASS, ShimmerDevice.GetSamplingRate(), new double[] { 5 });
                HPF_PPG = new Filter(Filter.HIGH_PASS, ShimmerDevice.GetSamplingRate(), new double[] { 0.5 });
            }
        }

        private bool streamingActuallyOccurred()
        {
            return (storedClusterData.Count > 0);
        }

        private void handleLogging()
        {
            Console.WriteLine("Writing stored data to file");
            writeLogFile(lastTimeStreamStarted);
            Console.WriteLine("Finished writing logs to file; deleting old logs");
            storedClusterData.Clear();
        }

        public void writeLogToFile(biometricCluster bc)
        {
            String lineToWrite = "";
            //Console.WriteLine("write to file");
            using (StreamWriter output = new StreamWriter((filePath), true))
            {
                Int32 unixTimestamp = (Int32)(bc.currentTime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                int rawGSRIndex = bc.receivedCluster.GetIndex("GSR", "RAW");
                int calGSRIndex = bc.receivedCluster.GetIndex("GSR", "CAL");
                int adcRawIndex = bc.receivedCluster.GetIndex("Internal ADC A13", "RAW");
                int adcCalIndex = bc.receivedCluster.GetIndex("Internal ADC A13", "CAL");
                //Console.WriteLine("making string");
                lineToWrite = String.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", unixTimestamp, bc.currentTime.Hour, bc.currentTime.Minute,
                    bc.currentTime.Second, bc.currentTime.Millisecond, bc.receivedCluster.GetData()[adcRawIndex],
                    bc.receivedCluster.GetData()[adcCalIndex], bc.receivedCluster.GetData()[rawGSRIndex],
                    bc.receivedCluster.GetData()[calGSRIndex]);
                //Console.WriteLine("actually writing to file");
                output.WriteLine(lineToWrite);
                //Console.WriteLine("done");
            }
        }

        public void createLogFile(DateTime rightNow)
        {
            filePath = basePath + "Biometrics log-" + rightNow.Month + "-" + rightNow.Day + "-"
                + rightNow.Year + "-" + rightNow.Hour + "-"
                    + rightNow.Minute + "-" + rightNow.Second + ".csv";
            Console.WriteLine(filePath);

            System.IO.FileInfo file = new System.IO.FileInfo(filePath);
            file.Directory.Create(); // If the directory already exists, this method does nothing.
            //System.IO.File.WriteAllText(file.FullName, content);

            StreamWriter output = new StreamWriter(filePath);
            using (output)
            {
                //Console.WriteLine("actually writing to file");
                output.WriteLine("Timestamp_Unix, Timestamp_Hour, Timestamp_Minute, Timestamp_Second, Timestamp_Millisecond, Current_Internal_ADC_A13_Raw, Current_Internal_ADC_A13_Cal, Current_GSR_Raw, Current_GSR_Calculated");
                //Console.WriteLine("we wrote");
            }
        }

        public void writeLogFile(DateTime rightNow)
        {
            filePath = basePath + "Biometrics log-" + rightNow.Month + "-" + rightNow.Day + "-"
                + rightNow.Year + "-" + rightNow.Hour + "-"
                    + rightNow.Minute + "-" + rightNow.Second + ".csv";
            String lineToWrite = "";
            StreamWriter output = new StreamWriter(@filePath);

            using (output)
            {
                output.WriteLine("Timestamp_Unix, Timestamp_Hour, Timestamp_Minute, Timestamp_Second, Timestamp_Millisecond, Current_Internal_ADC_A13_Raw, Current_Internal_ADC_A13_Cal, Current_GSR_Raw, Current_GSR_Calculated");
                //writeLogToFile(DateTime.Now, 0,-255);
                for (int i = 0; i < storedClusterData.Count; i++)
                {
                    Int32 unixTimestamp = (Int32)(storedClusterData[i].currentTime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    int rawGSRIndex = storedClusterData[i].receivedCluster.GetIndex("GSR", "RAW");
                    int calGSRIndex = storedClusterData[i].receivedCluster.GetIndex("GSR", "CAL");
                    int adcRawIndex = storedClusterData[i].receivedCluster.GetIndex("Internal ADC A13", "RAW");
                    int adcCalIndex = storedClusterData[i].receivedCluster.GetIndex("Internal ADC A13", "CAL");
                    lineToWrite = String.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", unixTimestamp, storedClusterData[i].currentTime.Hour, storedClusterData[i].currentTime.Minute,
                        storedClusterData[i].currentTime.Second, storedClusterData[i].currentTime.Millisecond, storedClusterData[i].receivedCluster.GetData()[adcRawIndex],
                        storedClusterData[i].receivedCluster.GetData()[adcCalIndex], storedClusterData[i].receivedCluster.GetData()[rawGSRIndex], 
                        storedClusterData[i].receivedCluster.GetData()[calGSRIndex]);
                    //Console.WriteLine(lineToWrite2);
                    //lineToWrite = "" + storedClusterData[i].currentTime.Hour + ", " + storedClusterData[i].currentTime.Minute + ", " + storedClusterData[i].currentTime.Second + ", " + storedClusterData[i].currentTime.Millisecond + ", ";
                    //lineToWrite += storedClusterData[i].receivedCluster.GetData()[rawGSRIndex] + ", " + storedClusterData[i].receivedCluster.GetData()[calGSRIndex];
                    output.WriteLine(lineToWrite);
                }
            }
            output.Close();
        }
    }

    // ExceptionReporter Class
    internal class ExceptionEventHandler
    {
        private static readonly string ApplicationName = Application.ProductName.ToString().Replace("_", " ");
        //    private string versionNumber = Application.ProductVersion.ToString().Substring(0, Application.ProductVersion.ToString().LastIndexOf(".")).ToLower();
        private string versionNumber = Application.ProductVersion.ToString();

        public void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            ReportCrash(e.ExceptionObject as Exception);
            Environment.Exit(0);
        }

        public void ApplicationThreadException(object sender, ThreadExceptionEventArgs e)
        {
            ReportCrash(e.Exception);
            Environment.Exit(0);
        }

        private static void ReportCrash(Exception exception)
        {

        }
    }
}
