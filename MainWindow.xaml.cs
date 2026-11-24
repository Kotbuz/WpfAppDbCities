using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

namespace WpfAppDbCities
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string ConnectionString = "Host=localhost; Port=5432; Username=postgres; Password=424242; Database=test";
        public NpgsqlConnection connection;
        public MainWindow()
        {
            InitializeComponent();
            connection = new NpgsqlConnection(ConnectionString);
            connection.Open();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var query = tb_query.Text.ToUpper();
            if (query.Split().Contains("DROP") || query.Split().Contains("DELETE") || query.Split().Contains("UPDATE") || query.Split().Contains("INSERT") || query.Split().Contains("ALTER")) 
            {
                MessageBox.Show("Можно использовать только SELECT!");
                query = "SELECT * FROM cities";
            }
            if (query == string.Empty)
                query = "SELECT * FROM cities";
            try
            {
                var cmd = new NpgsqlCommand(query, connection);
                var table = new DataTable();
                table.Load(cmd.ExecuteReader());
                dg_query.ItemsSource = table.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_min_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(new NpgsqlCommand("SELECT MIN(population) FROM cities", connection).ExecuteScalar().ToString(), "min");
        }

        private void btn_max_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(new NpgsqlCommand("SELECT MAX(population) FROM cities", connection).ExecuteScalar().ToString(), "max");
        }

        private void btn_count_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(new NpgsqlCommand("SELECT COUNT(*) FROM cities", connection).ExecuteScalar().ToString(), "count");
        }

        private void btn_sum_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(new NpgsqlCommand("SELECT SUM(population) FROM cities", connection).ExecuteScalar().ToString(), "sum");

        }

        private void btn_avg_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Math.Round((decimal)new NpgsqlCommand("SELECT AVG(population) FROM cities", connection).ExecuteScalar(), 2).ToString(), "avg");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            connection.Close();
        }

        private void btn_all_Click(object sender, RoutedEventArgs e)
        {
            string max = new NpgsqlCommand("SELECT name from cities where population = (select MAX(population) FROM cities)", connection).ExecuteScalar().ToString();
            string min = new NpgsqlCommand("SELECT name from cities where population = (select MIN(population) FROM cities)", connection).ExecuteScalar().ToString();
            string count = new NpgsqlCommand("SELECT COUNT(*) FROM cities", connection).ExecuteScalar().ToString();
            string sum = new NpgsqlCommand("SELECT SUM(population) FROM cities", connection).ExecuteScalar().ToString();
            string avg = Math.Round((decimal)new NpgsqlCommand("SELECT AVG(population) FROM cities", connection).ExecuteScalar(), 2).ToString();
            string text = $"MAX: {max}\nMIN: {min}\nCount: {count}\nSUM: {sum}\nAVG: {avg}";
            MessageBox.Show(text, "All");
        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            string name = tb_name.Text;
            int population = int.Parse(tb_population.Text);
            var cmd = new NpgsqlCommand($"INSERT INTO cities (name, population) VALUES ('{name}', {population})",connection);
            int check = cmd.ExecuteNonQuery();

            var query = "SELECT * FROM cities";
            cmd = new NpgsqlCommand(query, connection);
            var table = new DataTable();
            table.Load(cmd.ExecuteReader());
            dg_query.ItemsSource = table.DefaultView;

        }
    }
}
