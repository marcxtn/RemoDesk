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
using System.IO;

namespace remoDesk
{
    /// <summary>
    /// Interaktionslogik für ServerGUI.xaml
    /// </summary>
    public partial class ServerGUI : Window
    {
        public ServerGUI()
        {
            InitializeComponent();
        }

        private void ShowScreen_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                //convert the Textbox conntent to a int
                int port = int.Parse(PortInputTxt.Text);
                stream strm = new stream(port);
                strm.Show();
            }
            catch
            {

                System.Windows.MessageBox.Show("You have to insert a port!");

            }

        }

        private void MyIpBTN_Click(object sender, RoutedEventArgs e)
        {

            if (MyIpBTN.Content != "Show my IP")
            {

                //change the content of the button with your Public IP
                MyIpBTN.Content = "Your IP Adress is: " +GetMyIPAdressShow();

            }
            else
            {



            }

        }

        private string GetMyIPAdressShow()
        {

            //go on checkip.dyndns.org
            string url = "http://checkip.dyndns.org";
            //get an Web Request nad get the Website Conntent
            System.Net.WebRequest req = System.Net.WebRequest.Create(url);
            System.Net.WebResponse resp = req.GetResponse();
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            //save the Website Conntent in a variable and trim it
            string response = sr.ReadToEnd().Trim();
            string[] a = response.Split(':');
            string a2 = a[1].Substring(1);
            string[] a3 = a2.Split('<');
            string a4 = a3[0];
            //return your Public IP
            return a4;

        }
    }
}
