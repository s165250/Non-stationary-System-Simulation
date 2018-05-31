using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using static System.Math;

namespace WpfApp1
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const double integration_step = 0.001;
        double A = 0;
        double B = 0;
        double C = 0;
        double alfa = 0;
        double beta = 0;
        private int function = 0;

        public MainWindow()
        {
            InitializeComponent();
            Exponential_C.IsChecked = true;
        }

        public static double Integrate(double start_x, double stop_x)
        {
            double integral = 0;
            double prev_y = 0;
            double current_y = 0;
            long steps_N = (int)((stop_x * 1 / integration_step - start_x * 1 / integration_step));

            prev_y = start_x;
            current_y = start_x + integration_step;

            for (int i = 0; i < steps_N; i++)
            {
                integral += (prev_y + current_y) * integration_step / 2;
                prev_y = current_y;
                current_y += integration_step;

            }
            return integral;
        }

        private void Exponential_C_Checked(object sender, RoutedEventArgs e)
        {
            function = 1;
        }

        private void SINUS_C_Checked(object sender, RoutedEventArgs e)
        {
            function = 2;
        }

        private void Save_Data_Click(object sender, RoutedEventArgs e)
        {
            if (!(Double.TryParse(COEFF_a.Text, out A) &&
            Double.TryParse(COEFF_b.Text, out B) &&
            Double.TryParse(COEFF_c.Text, out C) &&
            Double.TryParse(COEFF_alfa.Text, out alfa) &&
            Double.TryParse(COEFF_beta.Text, out beta)))
            {
                MessageBox.Show("Podaj prawidłowe współczynniki!");
            }
        }

        private void BTN_CT_Click(object sender, RoutedEventArgs e)
        {
            double t = 0;
            List<double> ct = new List<double>();
            double currentval = 0;
            for (int i = 0; i < 50000; i++)
            {
                t = i / 1000;
                currentval = valueOfCt(t);
                ct.Add(currentval);
                System.Console.WriteLine(currentval);
            }
        }

        private double valueOfCt(double t)
        {
            double result = 0;
            // Exponential c(t) function
            if (function == 1)
            {
                result = alfa + alfa * Exp(-beta * t);
                return result;
            }
            // Sine c(t) function
            else if (function == 2)
            {
                result = 2 * alfa + alfa * Sin(beta * t);
                return result;
            }
            else
            {
                return -356;
            }
        }
      
    }

}
