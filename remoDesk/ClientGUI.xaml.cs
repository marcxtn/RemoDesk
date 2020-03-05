using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;
using System.Drawing.Imaging;
using remoDesk.Properties;
using System.Speech.Recognition;
using System.Speech.Synthesis;

namespace remoDesk
{
    /// <summary>
    /// Interaktionslogik für ClientGUI.xaml
    /// </summary>
    public partial class ClientGUI : Window
    {

        //create the required variables
        bool sendDesktop = false;
        private readonly TcpClient client = new TcpClient();
        private NetworkStream mainSteam;

        private int PortNumber;
        private Boolean sz = false;

        //Get data from the database
        string framesSet = Settings.Default["STRMframes"].ToString();
        string LangSupWin = Settings.Default["SpeechSup"].ToString();

        private static Image GrabDesktop()
        {

            //set settings for the screenshots and make screenshots
            Rectangle bounds = Screen.PrimaryScreen.Bounds;
            Bitmap screenshot = new Bitmap(bounds.Width, bounds.Height);
            Graphics graphic = Graphics.FromImage(screenshot);
            graphic.CopyFromScreen(bounds.X, bounds.Y, 0, 0, bounds.Size, CopyPixelOperation.SourceCopy);
            return screenshot;

        }

        //create a new speech recognition and speech synthesition
        SpeechRecognitionEngine AS = new SpeechRecognitionEngine();
        SpeechSynthesizer ss = new SpeechSynthesizer();

        private void startSreco()
        {

            //create a list of all needet commands
            Choices commands = new Choices();
            commands.Add(new string[] { "Options", "close all" });

            //create a new Grammar Builder and add the needet commands from the list
            GrammarBuilder gBuilder = new GrammarBuilder();
            gBuilder.Append(commands);

            Grammar grammar = new Grammar(gBuilder);

            //load the commands
            AS.LoadGrammar(grammar);
            AS.SetInputToDefaultAudioDevice();
            AS.SpeechRecognized += recEngine_SpeechRecognized;

            //Set configurations
            AS.RecognizeAsync(RecognizeMode.Multiple);
            ss.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Adult);
            //Say hello
            ss.SpeakAsync("Hello");

        }

        void recEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {

            //get the result and process and compare them with the existing Comands
            switch (e.Result.Text)
            {
                #region options
                case "Options":
                    ss.SpeakAsync("Options get opend");
                    remoSettings SRM = new remoSettings();
                    SRM.Show();
                    break;
                #endregion
                #region close all
                case "close all":
                    ss.SpeakAsync("Okay");
                    this.Close();
                    break;
                    #endregion

            }

        }

        private void SendDesktopImage()
        {

            try
            {

                //create a new Binary Formatter
                BinaryFormatter binFormatter = new BinaryFormatter();
                //save the stream in mainStream
                mainSteam = client.GetStream();
                //and Serialize it and get the screenshots
                binFormatter.Serialize(mainSteam, GrabDesktop());

            }
            catch
            {

                System.Windows.MessageBox.Show("The Server has left convergence!");

            }

        }

        public ClientGUI()
        {

            InitializeComponent();
            //if the Speech R. and S. is activated
            if (LangSupWin == "Yes")
            {
                //load the Speech R. and S. 
                startSreco();
            }
            else
            {



            }
        }

        private void ConnectBTN_Click(object sender, RoutedEventArgs e)
        {

            //check if the textbox is not empty
            if (PortInputTxt.Text != "")
            {
                //if not save the textbox conntent in "PortNumber"
                PortNumber = int.Parse(PortInputTxt.Text);
            }
            else
            {

                //Output a mistake (Messagebox)
                System.Windows.MessageBox.Show("You need to set a port!");

            }

            try
            {

                //Connect the client to the server with the given data
                client.Connect(IpInputTxt.Text, PortNumber);
                //and set the Status to "connectet"
                statusTXT.Content = "Status: connectet";
                ss.SpeakAsync("connectet");


            }
            catch
            {

                //Output a mistake (Messagebox)
                System.Windows.MessageBox.Show("Failed to connectet!");
                statusTXT.Content = "Status: failed to connectet";
                ss.SpeakAsync("failed to connect");

            }

        }

        private void ShareBTN_Click(object sender, RoutedEventArgs e)
        {

            if (Convert.ToString(statusTXT.Content) == "Status: connectet")
            {

                //create a new var
                string BTNinput;
                BTNinput = ShareBTN.Content.ToString();

                //create a new Dispatchertimer
                System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();

                if (BTNinput.StartsWith("share"))
                {

                    //convert the input of StreamFPS to an int
                    int StreamFPS = Int32.Parse(framesSet);

                    //start Timer, add 1 o 2 o 6 sec. and start it
                    dispatcherTimer.Tick += new EventHandler(Timer1_Tick);
                    dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, StreamFPS);
                    dispatcherTimer.Start();

                    ShareBTN.Content = "stop sharing";

                }
                else
                {

                    //stop the Timer
                    dispatcherTimer.Stop();
                    ShareBTN.Content = "share Screen";

                }

            }
            else
            {

                System.Windows.MessageBox.Show("You must connect with an Server!");

            }

        }

        private void SafeServerBTN_Click(object sender, RoutedEventArgs e)
        {

            //compare the input with the datebase data
            if (IpInputTxt.Text != "" || PortInputTxt.Text != "" || 
                IpInputTxt.Text == Settings.Default["ServerIP"].ToString() && PortInputTxt.Text == Settings.Default["ServerPort"].ToString())
            {

                //safe in the DB the new informations about the safed server
                Settings.Default["ServerIP"] = IpInputTxt.Text;
                Settings.Default["ServerPort"] = PortInputTxt.Text;
                Settings.Default.Save();

            }
            else
            {

                System.Windows.MessageBox.Show("check if you have filled in all empty fields!\n If this is the case you cannot save the same server again!");

            }

        }

        private void Timer1_Tick(object sender, EventArgs e)
        {

            //get Screens and start the stream
            SendDesktopImage();
            statusTXT.Content = "stream";

        }

        private void LoadServerBTN_Click(object sender, RoutedEventArgs e)
        {

            //Load the required data from the database
            string IPtoLoade = Settings.Default["ServerIP"].ToString();
            string PorttoLoade = Settings.Default["ServerPort"].ToString();

            //convert them to a string
            IpInputTxt.Text = IPtoLoade.ToString();
            PortInputTxt.Text = PorttoLoade.ToString();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            //open remoSettings
            remoSettings RMS = new remoSettings();
            RMS.Show();

        }
    }
}