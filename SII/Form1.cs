﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SII
{
    
    public partial class Form1 : Form
    {
        int clicksMD = 0;
        Point[] points = new Point[11];
        int[] xs = new int[11];
        int[] ys = new int[11];
        bool MD = true;
        Graphics g;
        int count;
        public Pen brush = new Pen(Color.Black, 3);
        double t, a, shortPath;
        public static double Way(float x1, float y1, float x2, float y2)
        {   
            return Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
        }
        private void Point(float x, float y)
        {
            g.DrawEllipse(brush, x, y, brush.Width, brush.Width);
            
        }
        public static int[] changePath(int[] path)
        {
            Random rnd = new Random();
            int[] newPath = new int[path.Length];
            path.CopyTo(newPath, 0);
            int first = 0, second = 0;
            while (first == second)
            {
                first = rnd.Next(1, path.Length - 1);
                second = rnd.Next(1, path.Length - 1);
            }
            int cont = newPath[first];
            newPath[first] = newPath[second];
            newPath[second] = cont;
            return newPath;
        }
        public static double fullPath(int[] path, int[] xs, int[] ys)
        {
            double fullPath = 0;
            for (int i = 1; i < path.Length; i++)
            {
                fullPath += Way(xs[path[i]], ys[path[i]], xs[path[i - 1]], ys[path[i - 1]]);
            }
            return fullPath;
        }
        public static bool checkTemper(double s, double t)
        {
            Random rnd = new Random();
            double prob = 100 * Math.Pow(Math.E, -(s / t));
            if (prob > rnd.Next(1, 100)) return true;
            else return false;
        }
        public static bool checkList(List<int[]> arList, int[] path)
        {
            bool check = true;
            for (int i = 0; i < arList.Count; i++)
            {
                if (path.SequenceEqual(arList[i]))
                {
                    check = false;
                    break;
                }
            }
            return check;
        }

        public Form1()
        {
            InitializeComponent();
            g = pictureBox1.CreateGraphics();
            count = Convert.ToInt32(numericUpDown1.Value);
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
            MD = true;
            clicksMD = 0;
            textBox1.Text = "";
            button3.Enabled = true;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            
            if (MD == true)
            {
                Point(e.X, e.Y);
                xs[clicksMD] = e.X;
                ys[clicksMD] = e.Y;
                clicksMD++;
                if (clicksMD >= count) MD = false;
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            count = Convert.ToInt32(numericUpDown1.Value);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            count = Convert.ToInt32(numericUpDown1.Value);
            t = Convert.ToDouble(textBox2.Text);
            a = Convert.ToDouble(textBox3.Text);
            List<int[]> arList = new List<int[]>();
            int[] path = new int[count+1];
            path[0] = 0;
            xs[count] = xs[0];
            ys[count] = ys[0];
            path[count] = path[0];
            for (int i = 0; i < count; i++)
            {
                path[i] = i;
            }
            arList.Add(path);
            while (t > 1)
            {
                shortPath = fullPath(path, xs, ys);
                int[] newpath = new int[path.Length];
                path.CopyTo(newpath, 0);
                while (!checkList(arList, newpath))
                    newpath = changePath(newpath);
                arList.Add(newpath);
                if (fullPath(newpath, xs, ys) <= shortPath)
                {
                    shortPath = fullPath(newpath, xs, ys);
                    path = newpath;
                }
                else
                {
                    if (checkTemper(fullPath(newpath, xs, ys) - shortPath, t))
                    {
                        shortPath = fullPath(newpath, xs, ys);
                        path = newpath;
                    }

                }
                t = a * t;
                //для вывода промежуточного результата. Можно удалить вместе с textBox
                textBox1.Text += (fullPath(path, xs, ys) + " > " + t + " ") + Environment.NewLine;
                
            }
            textBox4.Text = (fullPath(path, xs, ys) + " > " + t + " ") + Environment.NewLine;
            for (int i = 1; i < path.Length; i++)
            {
                g.DrawLine(brush, xs[path[i - 1]], ys[path[i - 1]], xs[path[i]], ys[path[i]]);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MD = false;
            Random rnd = new Random();
            for (int i = 0; i < count; i++)
            {
                int x = rnd.Next(10, pictureBox1.Width-10);
                int y = rnd.Next(10, pictureBox1.Height-10);
                Point(x, y);
                xs[i] = x;
                ys[i] = y;
            }
            button3.Enabled = false;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            t = Convert.ToDouble(textBox2.Text);
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            a = Convert.ToDouble(textBox3.Text);
        }
    }
}