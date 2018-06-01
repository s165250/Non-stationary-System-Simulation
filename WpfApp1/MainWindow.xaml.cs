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
        SolidColorBrush Chart_Color = new SolidColorBrush(Colors.Black);
        double Chart_Thickness = 1.5;

        public MainWindow()
        {
            InitializeComponent();
            Exponential_C.IsChecked = true;
            draw_Axes(chart_1);
            draw_Axes(chart_2);
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
        private void draw_Axes(Canvas chart_pole)
        {
            // OŚ Y
            Line axisY = new Line();
            axisY.Stroke = Chart_Color;
            axisY.StrokeThickness = Chart_Thickness;

            axisY.X1 = 20;
            axisY.X2 = 20;
            axisY.Y1 = 10;
            axisY.Y2 = 280;
            chart_pole.Children.Add(axisY);

            // OŚ X
            Line axisX = new Line();
            axisX.Stroke = Chart_Color;
            axisX.StrokeThickness = Chart_Thickness;

            axisX.X1 = 20;
            axisX.X2 = 830;
            axisX.Y1 = 150;
            axisX.Y2 = 150;

            chart_pole.Children.Add(axisX);
            

            // STRZAŁKA OŚ X
            Line arrowX1 = new Line();
            arrowX1.Stroke = Chart_Color;
            arrowX1.StrokeThickness = Chart_Thickness;

            arrowX1.X1 = 820;
            arrowX1.X2 = 830;
            arrowX1.Y1 = 144;
            arrowX1.Y2 = 150;

            chart_pole.Children.Add(arrowX1);

            Line arrowX2 = new Line();
            arrowX2.Stroke = Chart_Color;
            arrowX2.StrokeThickness = Chart_Thickness;

            arrowX2.X1 = 820;
            arrowX2.X2 = 830;
            arrowX2.Y1 = 156;
            arrowX2.Y2 = 150;

            chart_pole.Children.Add(arrowX2);

            // STZAŁKA OŚ Y
            Line arrowY1 = new Line();
            arrowY1.Stroke = Chart_Color;
            arrowY1.StrokeThickness = Chart_Thickness;

            arrowY1.X1 = 14;
            arrowY1.X2 = 20;
            arrowY1.Y1 = 20;
            arrowY1.Y2 = 10;

            chart_pole.Children.Add(arrowY1);

            Line arrowY2 = new Line();
            arrowY2.Stroke = Chart_Color;
            arrowY2.StrokeThickness = Chart_Thickness;

            arrowY2.X1 = 26;
            arrowY2.X2 = 20;
            arrowY2.Y1 = 20;
            arrowY2.Y2 = 10;

            chart_pole.Children.Add(arrowY2);

            // SKALA OŚ X

            for(int index = 0; index <9; index++)
            {
                Line scale = new Line();
                scale.Stroke = Chart_Color;
                scale.StrokeThickness = Chart_Thickness;
                scale.X1 = (index + 1) * 81+20;
                scale.X2 = scale.X1;
                scale.Y1 = 145;
                scale.Y2 = 155;
                chart_pole.Children.Add(scale);
            }

            // SKALA OŚ Y

            for(int index = -4; index <3; index++)
            {
                Line scale = new Line();
                scale.Stroke = Chart_Color;
                scale.StrokeThickness = Chart_Thickness;
                scale.X1 = 15;
                scale.X2 = 25;
                scale.Y1 = 150 + (index + 1) * 40 ;
                scale.Y2 = scale.Y1;
                chart_pole.Children.Add(scale);
            }

        }
    }

}
