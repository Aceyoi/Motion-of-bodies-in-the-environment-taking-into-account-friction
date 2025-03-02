using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Graphics Graphicsbriz = pictureBox1.CreateGraphics();
            int r = int.Parse(textBox1.Text); // Расстояние до цели (метры)
            int a = int.Parse(textBox2.Text); // Угол выстрела (градусы)
            double q = 997; // Плотность воды (кг/м^3)
            double m = 0.57; // Масса гарпуна (кг)
            double u = 100; // Начальная скорость гарпуна (м/с)
            double Cd = 0.47; // Коэффициент сопротивления (для сферы)
            double A = 0.0001; // Площадь поперечного сечения гарпуна (м^2)
            double g = 9.81; // Ускорение свободного падения (м/с^2)

            double dt = 0.01; // Шаг по времени (секунды)
            double vx = u * Math.Cos(a * Math.PI / 180); // Начальная скорость по X
            double vy = u * Math.Sin(a * Math.PI / 180); // Начальная скорость по Y
            double x = 0, y = 0; // Начальные координаты гарпуна

            List<PointF> points = new List<PointF>();
            points.Add(new PointF(100, 100)); // Начальная точка (в пикселях)

            bool hit = false; // Флаг попадания в цель
            double targetRadius = 0.5; // Радиус цели (метры), в пределах которого считается попадание

            if (a > 80 || a < -80 || r < 40 || r > 0)
            {
                textBox3.Text = "Некоректное значение!";
            }
            else
            {

                // Рассчет траектории
                while (x < r * 1.5) // Увеличиваем диапазон, чтобы увидеть траекторию после цели
                {
                    double v = Math.Sqrt(vx * vx + vy * vy); // Текущая скорость
                    double Fd = 0.5 * q * Cd * A * v * v; // Сила сопротивления
                    double ax = -Fd * vx / (m * v); // Ускорение по X
                    double ay = -g - Fd * vy / (m * v); // Ускорение по Y

                    vx += ax * dt;
                    vy += ay * dt;
                    x += vx * dt;
                    y += vy * dt;

                    // Проверка на попадание в цель
                    if (Math.Abs(x - r) < targetRadius && Math.Abs(y) < targetRadius)
                    {
                        hit = true;
                    }

                    // Добавляем точку в список для отрисовки
                    points.Add(new PointF(100 + (int)(x * 10), 100 - (int)(y * 10))); // Масштабируем для визуализации
                }

                // Отрисовка траектории
                for (int i = 1; i < points.Count; i++)
                {
                    Graphicsbriz.DrawLine(Pens.Black, points[i - 1], points[i]);
                }

                // Отрисовка цели
                int targetSize = 10; // Размер цели в пикселях
                Graphicsbriz.DrawEllipse(Pens.Blue, 100 + (int)(r * 10) - targetSize / 2, 100 - targetSize / 2, targetSize, targetSize);

                // Вывод результата
                if (hit)
                {
                    textBox3.Text = "Попадание!";
                }
                else
                {
                    textBox3.Text = "Промах!";
                }
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            textBox3.Text = null;
        }
    }
}
