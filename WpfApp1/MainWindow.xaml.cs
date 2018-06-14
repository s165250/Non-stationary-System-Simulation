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
        const double integration_step = 0.001; // Krok całkowania
        double A = 0; // Współczynnik A
        double B = 0; // Współczynnik B
        double alfa = 0; // Współczynnik Alfa
        double beta = 0; // Współczynnik Beta
        private int function = 0; // Zmienna określająca funkcje c(t)
        private int trianglepart = 0; // Zmienna określająca część fali trójkątnej obecnie rysowanej
        SolidColorBrush Chart_Color = new SolidColorBrush(Colors.Black); // Domyślny kolor dla wykresu
        SolidColorBrush OutputChart_Color = new SolidColorBrush(Colors.Magenta); // Kolor dla wykresu funkcji wyjścia
        SolidColorBrush InputChart_Color = new SolidColorBrush(Colors.Green); // Kolor dla wykresu funkcji wejścia
        double Chart_Thickness = 1.5; // Grubość linii, którą kreślony jest wykres
        double OutputChart_Thickness = 1.5; // Grubość linii, którą kreślony jest wykres wyjścia
        static double period = 80; // Okres fal pobudzających
        double slope = 1 / (period / 4); // Wspoczynnik nachylenia fali trójkątnej
        int tmax = 80; // Wartość czasu (w sekundach) do której rysowany bedzie wykres


        public MainWindow()
        {
            InitializeComponent();
            Exponential_C.IsChecked = true;
            Draw_Axes(chart_1);
            Draw_Axes(chart_2);
        }

        private void Exponential_C_Checked(object sender, RoutedEventArgs e)
        {
            function = 1;
        }

        private void SINUS_C_Checked(object sender, RoutedEventArgs e)
        {
            function = 2;
        } 

        private void CALCULATE_OUTPUT_Click(object sender, RoutedEventArgs e)
        {
            //Parsowanie wprowadzonych wspolczynnikow i zapis do odpowiednich zmiennych
            if (CheckData())
            {
                MessageBox.Show("Podaj prawidłowe współczynniki!");
            }
            else
            {
                InOut inOutData = new InOut();
                inOutData = CalculateOutput();
                DrawChart(inOutData.getInput(), chart_2, InputChart_Color);
                DrawChart(inOutData.getOutput(), chart_1, OutputChart_Color);
            }
            
        }

        private double ValueOfCt(double t)
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

        private void DrawChart(List<double> chartData, Canvas chart_pole, SolidColorBrush color)
        {            
            // Skalowanie i nanoszenie skali na wykres pierwszy
            double max = FindMax(chartData);
            double min = FindMin(chartData);
            double maxscale = (Abs(max) >= Abs(min)) ? Abs(Ceiling(max)) : Abs(Floor(min));
             
            chart_pole.Children.Clear();
            Draw_Axes(chart_pole);

            for (int index = 0; index < 7; index++)
            {
                    TextBlock scaleEff = new TextBlock();
                    scaleEff.Width = 30;
                    scaleEff.Height = 16;
                    scaleEff.Margin = new Thickness(5, 140 + (index - 3) * 40, 0, 0);
                    scaleEff.FontSize = 12;
                    scaleEff.TextAlignment = TextAlignment.Center;
                    scaleEff.FontWeight = System.Windows.FontWeights.Bold;

                    // Skala na dodatniej półosi

                    if (index == 0) scaleEff.Text = maxscale.ToString();
                    else if (index == 1) scaleEff.Text = Round((maxscale * 2 / 3), 2).ToString();
                    else if (index == 2) scaleEff.Text = Round((maxscale / 3), 2).ToString();

                    else if (index == 3) scaleEff.Text = "0";

                    // Skala na ujemnej półosi
                    else if (index == 4) scaleEff.Text = ((-1) * Round((maxscale / 3), 2)).ToString();
                    else if (index == 5) scaleEff.Text = ((-1) * Round((maxscale * 2 / 3), 2)).ToString();
                    else if (index == 6) scaleEff.Text = ((-1) * maxscale).ToString();
                    chart_pole.Children.Add(scaleEff);
            }

            double startX = 40;
            double endX = 0;
            double startY = 150;
            double endY = 0;
            foreach (double y in chartData)
            {
                endX = startX + 1;
                endY = 150 + (-1) * (int) 120 * y / maxscale;
                Line chartline = new Line();
                chartline.Stroke = color;
                chartline.StrokeThickness = OutputChart_Thickness;
                chartline.X1 = startX;
                chartline.X2 = endX;
                chartline.Y1 = startY;
                chartline.Y2 = endY;
                chart_pole.Children.Add(chartline);
                startX = endX;
                startY = endY;
            }
        }

        private void Draw_Axes(Canvas chart_pole)
        {
            // OŚ Y
            Line axisY = new Line();
            axisY.Stroke = Chart_Color;
            axisY.StrokeThickness = Chart_Thickness;

            axisY.X1 = 40;
            axisY.X2 = 40;
            axisY.Y1 = 10;
            axisY.Y2 = 280;
            chart_pole.Children.Add(axisY);

            // OŚ X
            Line axisX = new Line();
            axisX.Stroke = Chart_Color;
            axisX.StrokeThickness = Chart_Thickness;

            axisX.X1 = 40;
            axisX.X2 = 1070;
            axisX.Y1 = 150;
            axisX.Y2 = 150;

            chart_pole.Children.Add(axisX);


            // STRZAŁKA OŚ X
            Line arrowX1 = new Line();
            arrowX1.Stroke = Chart_Color;
            arrowX1.StrokeThickness = Chart_Thickness;

            arrowX1.X1 = 1060;
            arrowX1.X2 = 1070;
            arrowX1.Y1 = 144;
            arrowX1.Y2 = 150;

            chart_pole.Children.Add(arrowX1);

            Line arrowX2 = new Line();
            arrowX2.Stroke = Chart_Color;
            arrowX2.StrokeThickness = Chart_Thickness;

            arrowX2.X1 = 1060;
            arrowX2.X2 = 1070;
            arrowX2.Y1 = 156;
            arrowX2.Y2 = 150;

            chart_pole.Children.Add(arrowX2);

            // STZAŁKA OŚ Y
            Line arrowY1 = new Line();
            arrowY1.Stroke = Chart_Color;
            arrowY1.StrokeThickness = Chart_Thickness;

            arrowY1.X1 = 34;
            arrowY1.X2 = 40;
            arrowY1.Y1 = 20;
            arrowY1.Y2 = 10;

            chart_pole.Children.Add(arrowY1);

            Line arrowY2 = new Line();
            arrowY2.Stroke = Chart_Color;
            arrowY2.StrokeThickness = Chart_Thickness;

            arrowY2.X1 = 46;
            arrowY2.X2 = 40;
            arrowY2.Y1 = 20;
            arrowY2.Y2 = 10;

            chart_pole.Children.Add(arrowY2);

            // SKALA OŚ X

            for (int index = 0; index < 21; index++)
            {
                TextBlock scaleEff = new TextBlock();
                chart_pole.Children.Add(scaleEff);
                scaleEff.Height = 16;
                scaleEff.Width = 30;
                scaleEff.FontSize = 14;
                scaleEff.Margin = new Thickness((index + 1) * 50 + 36, 155, 0, 0);
                if (index != 20)
                {
                    scaleEff.Text = ((index + 1) * Round((tmax / 20.0), 1)).ToString();
                    Line scale = new Line();
                    scale.Stroke = Chart_Color;
                    scale.StrokeThickness = Chart_Thickness;
                    scale.X1 = (index + 1) * 50 + 40;
                    scale.X2 = scale.X1;
                    scale.Y1 = 145;
                    scale.Y2 = 155;
                    chart_pole.Children.Add(scale);
                }
                else
                {
                    scaleEff.Margin = new Thickness((index + 1) * 50 + 20, 155, 0, 0);
                    scaleEff.FontWeight = FontWeights.Bold;
                    scaleEff.Text = "t";
                }
            }

            // SKALA OŚ Y

            for (int index = -4; index < 3; index++)
            {
                Line scale = new Line();
                scale.Stroke = Chart_Color;
                scale.StrokeThickness = Chart_Thickness;
                scale.X1 = 35;
                scale.X2 = 45;
                scale.Y1 = 150 + (index + 1) * 40;
                scale.Y2 = scale.Y1;
                chart_pole.Children.Add(scale);
            }

        }

        private double FindMax(List<double> list)
        {
            double maxvalue = -1000;
            if (list.Count() == 0)
            {
                MessageBox.Show("Nie wyznaczone wartości funkcji!");
                return maxvalue;
            }
            else
            {
                foreach (double value in list)
                {
                    if (value > maxvalue)
                    {
                        maxvalue = value;
                    }
                }
                return maxvalue;
            }
        }

        private double FindMin(List<double> list)
        {
            double minvalue = +6000;
            if (list.Count() == 0)
            {
                MessageBox.Show("Nie wyznaczone wartości funkcji!");
                return minvalue;
            }
            else
            {
                foreach (double value in list)
                {
                    if (value < minvalue)
                    {
                        minvalue = value;
                    }
                }
                return minvalue;
            }
        }

        private double InputValue(double t)
        {
            double currentvalue = 0;
            switch (InputBox.Text)
            {
                case "Fala prostokątna":
                    int wavepart = (int) (t / (period / 2));
                    switch(wavepart % 2)
                    {
                        case 0: currentvalue = 1; break;
                        case 1: currentvalue = -1; break;
                    }
                    break;
                case "Fala trójkątna":
                    trianglepart = (int) (t / (period / 4));
                    currentvalue = TriangleWaveValue(t, trianglepart);
                    break;
                case "Sinusoida":
                    currentvalue = Sin(2 * PI * (1 / period) * t);
                    break;

            }
            return currentvalue;
        }

        private double TriangleWaveValue(double t, int trianglepart)
        {
            int x = trianglepart % 4;
            double value = 0;
            switch (x)
            {
                case 0:
                    {
                        value = slope * (t - (period/4)*trianglepart);
                        break;
                    }
                case 1:
                    {
                        value = 1 -  slope * (t - (period / 4) * trianglepart);
                        break;
                    }
                case 2:
                    {
                        value = -slope * (t - (period / 4) * trianglepart);
                        break;
                    }
                case 3:
                    {
                        value = -1 + slope * (t - (period / 4) * trianglepart);
                        break;
                    }
            }
            return value;
        }

        private double Integral(double y1, double y2)
        {
            return ((y1 + y2) / 2) * integration_step;
        }

        private InOut CalculateOutput()
        {
            // Inicjalizacja zmiennych
            InOut inOutData = new InOut();
            // Zerowe warunki początkowe
            double y3_prev = 0;
            double y2_prev = 0;
            double y1_prev = 0;
            double y_prev = 0;

            double y3_current = 0;
            double y2_current = 0;
            double y1_current = 0;
            double y_current = 0;
            double t = 0;
            for (int i = 0; i < (tmax*1030); i++)
            {
                t = i * integration_step;
                y3_current = InputValue(t) - A * y2_prev - B * y1_prev -  ValueOfCt(t)*y_prev;
                y2_current += Integral(y3_prev, y3_current);
                y1_current += Integral(y2_prev, y2_current);
                y_current += Integral(y1_prev, y1_current);
                if(i % tmax == 0)
                {
                    inOutData.AddInput(InputValue(t));
                    inOutData.AddOutput(y_current);
                }
                y3_prev = y3_current;
                y2_prev = y2_current;
                y1_prev = y1_current;
                y_prev = y_current;
                
            }


            return inOutData;
        }
        private bool CheckData()
        {
            if (!(Double.TryParse(COEFF_a.Text, out A) &&
                Double.TryParse(COEFF_b.Text, out B) &&
                Double.TryParse(COEFF_alfa.Text, out alfa) &&
                Double.TryParse(COEFF_beta.Text, out beta) &&
                InputBox.Text == "" &&
                (SINUS_C.IsChecked == true || Exponential_C.IsChecked == true)))
            {
                return false;
            }
            else return true;
        }
    }
}
