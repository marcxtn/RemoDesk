using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace remoDesk
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            InitializeComponent();
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {

            try
            {

                //try to open the window "ClientGUI"
                ClientGUI CLNgui = new ClientGUI();
                CLNgui.Show();
                this.Close();

            }
            catch
            {

                //if it catch, give an error message
                MessageBox.Show("Opening the Client GUI failed!", "Error");

            }

        }

        private void getConnected_Click(object sender, RoutedEventArgs e)
        {

            try
            {

                //try to open the window "ServerGUI"
                ServerGUI SRVgui = new ServerGUI();
                SRVgui.Show();
                this.Close();

            }
            catch
            {
                //if it catch, give an error message
                MessageBox.Show("Opening the Client GUI failed!", "Error");

            }

        }
    }
}
