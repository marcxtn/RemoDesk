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
using System.Windows.Shapes;
using remoDesk.Properties;

namespace remoDesk
{
    /// <summary>
    /// Interaktionslogik für remoSettings.xaml
    /// </summary>
    public partial class remoSettings : Window
    {
        public remoSettings()
        {
            InitializeComponent();
        }

        //create the required variables
        string framesSet = "1";
        string LangSupWin = "Yes";

        private void ComboBoxItem_Selected(object sender, RoutedEventArgs e)
        {

            //overwrite the current state of framesSet
            framesSet = "1";

        }

        private void ComboBoxItem_Selected_1(object sender, RoutedEventArgs e)
        {

            framesSet = "2";

        }

        private void ComboBoxItem_Selected_2(object sender, RoutedEventArgs e)
        {

            framesSet = "6";

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            //overwrite the informations in the DB and save them
            Settings.Default["STRMframes"] = framesSet;
            Settings.Default["SpeechSup"] = LangSupWin;
            Settings.Default.Save();
            System.Windows.MessageBoxResult result = MessageBox.Show("To accept all settings you have to restart the program!");

            this.Close();

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            //close this Window
            this.Close();

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            //overwrite the current state of langSupWin
            LangSupWin = "Yes";

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

            LangSupWin = "No";

        }
    }
}
