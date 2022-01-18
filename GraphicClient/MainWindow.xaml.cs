using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using FTP;

namespace GraphicClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var loginViewModel = new LoginViewModel(Consts.ConfigPath);
            this.DataContext = loginViewModel;
            //this.listBox.DataContext = loginViewModel.FileSystem;

        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.LoginPanel.DataContext != null)
            {
                ((dynamic)this.LoginPanel.DataContext).Config.Password = ((PasswordBox)sender).SecurePassword;
            }
        }
    }
}
