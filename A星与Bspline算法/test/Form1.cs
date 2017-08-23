using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test
{
    public partial class Form1 : Form
    {
        private Graphics gra;

        private Pen myPen;

        private Point startPnt = new Point();
        private Point endPnt = new Point();
        
        Map _map;

        public Point MousePos = new Point(163, 68);

        public Form1()
        {
            InitializeComponent();
            _map = new Map();

            MousePos = new Point(0,0);
            gra = pictureBox1.CreateGraphics();

            myPen = new Pen(Color.Black, 1);
            //this.pictureBox1.Width = _map.width;
            //this.pictureBox1.Height = _map.height;


        }



        private void Form1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (设置起始点.Text == "设置中")
            {
                startPnt.X = MousePos.X;
                startPnt.Y = MousePos.Y;

                myPen.Color = Color.Purple;

                gra.DrawEllipse(myPen, startPnt.X, startPnt.Y, 3, 3);

                startPoint.Text = "(" + startPnt.X.ToString() + "," + startPnt.Y.ToString() + ")";

                设置起始点.Text = "设置起始点";
            }

            if (设置终止点.Text == "设置中")
            {
                endPnt.X = MousePos.X;
                endPnt.Y = MousePos.Y;

                myPen.Color = Color.Purple;

                gra.DrawEllipse(myPen, endPnt.X, endPnt.Y, 3, 3);

                endPoint.Text = "(" + endPnt.X.ToString() + "," + endPnt.Y.ToString() + ")";
                设置终止点.Text = "设置终止点";

            }
        }

        private void 导入地图_Click(object sender, EventArgs e)
        {

            myPen.Color = Color.Black;
            for (int i = 0; i < _map.height; i++)
            {
                for (int j = 0; j < _map.width; j++)
                {
                    if (_map.costMap[i, j] == 1)
                    {
                        gra.DrawEllipse(myPen, i, j, 1, 1);
                    }
                }
            }
        }



        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            label1.Text = "x:" + e.X.ToString() + "y:" + e.Y.ToString();
            MousePos.X = e.X;
            MousePos.Y = e.Y;
        }

        private void 设置起始点_Click(object sender, EventArgs e)
        {
            设置起始点.Text = "设置中";
        }

        private void 设置终止点_Click(object sender, EventArgs e)
        {
            设置终止点.Text = "设置中";
        }



        
        private void 规划路径_Click(object sender, EventArgs e)
        {
            Maze maze = new Maze(_map.costMap);


            

            //Console.WriteLine(startPnt.X.ToString() + "," + startPnt.Y.ToString());
            //Console.WriteLine(endPnt.X.ToString() + "," + endPnt.Y.ToString());
            
            if (maze.FindPath(startPnt, endPnt, gra, myPen))
            {
                maze.GetPath(startPnt, endPnt, gra, myPen);
            }
            //myPen.Color = Color.Yellow;

            //while (pnt.X != startPnt.X && pnt.Y != startPnt.Y)
            //{
            //    gra.DrawEllipse(myPen, pnt.X, pnt.Y, 1, 1);



            //    pnt = maze.FindPa(pnt);
            //    //parent = parent.ParentPoint;
            //}
        }




        private void button1_Click(object sender, EventArgs e)
        {
            PointF[] a = new PointF[3];
            a[0].X = 0;
            a[0].Y = 0;
            a[1].X = 10;
            a[1].Y = 10;
            a[2].X = 20;
            a[2].Y = 50;
            Bspline.DrawBspline1(3,gra,myPen,a);
            //DrawBspline1();
        }




    }
}
