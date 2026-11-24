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
using Npgsql;
namespace WpfAppDbCities
{
    /// <summary>
    /// Логика взаимодействия для Auth.xaml
    /// </summary>
    public partial class Auth : Window
    {
        public string strConn;
        public Auth()
        {
            InitializeComponent();
             strConn = "Host=localhost;Port = 5432; username= postgres; password = 424242; db=test";
        }

        private void tb_login_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        
        private void tb_pass_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void tb_login_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tb_login.Text == "")
            {
                tb_pass.Foreground = new SolidColorBrush(Colors.LightGray);
                tb_login.Text = "Login";
            }

        }

        private void tb_login_GotFocus(object sender, RoutedEventArgs e)
        {

            if (tb_login.Text == "Login")
            {
                tb_pass.Foreground = new SolidColorBrush(Colors.Black);
                tb_login.Text = "";
            }
        }

        private void tb_pass_GotFocus(object sender, RoutedEventArgs e)
        {

            if (tb_pass.Text == "Password")
            {

                tb_pass.Foreground = new SolidColorBrush(Colors.Black);
                tb_pass.Text = "";
            }

        }

        private void tb_pass_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tb_pass.Text == "")
            {
                tb_pass.Foreground = new SolidColorBrush(Colors.LightGray);
                tb_pass.Text = "Password";
            }

        }

        private void btn_auth_Click(object sender, RoutedEventArgs e)
        {

            NpgsqlConnection conn = new NpgsqlConnection(strConn);
            conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand($"SELECT * FROM USERS WHERE login = '{tb_login.Text}' and pass='{tb_pass.Text}'",conn);
            var respense = cmd.ExecuteScalar();
            if (respense != null)
            {

                MainWindow main = new MainWindow();
                main.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Неправильный логин или пароль!", "Ошибка");
            }
        }
    }
}
