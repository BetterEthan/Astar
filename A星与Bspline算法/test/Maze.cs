using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace test
{
    class Maze
    {
        public const int OBLIQUE = 14;
        public const int STEP = 10;
        public int[,] MazeArray { get; private set; }
        public Point[,] parents;
        public BinHeap OpenList;
        public int[,] CloseList;
        private TWD[] twds = new TWD[8];

        public int width, height;

        public Maze(int[,] maze)
        {
            this.MazeArray = maze;

            OpenList = new BinHeap(MazeArray.Length);

            width = MazeArray.GetLength(1);
            height = MazeArray.GetLength(0);

            parents = new Point[height, width];
            CloseList = new int[height, width];

            twds[0].x = 0; twds[0].y = -1; twds[0].cost = 10;  //上
            twds[1].x = 0; twds[1].y = 1; twds[1].cost = 10;  //下
            twds[2].x = -1; twds[2].y = 0; twds[2].cost = 10;  //左
            twds[3].x = 1; twds[3].y = 0; twds[3].cost = 10;  //右
            twds[4].x = -1; twds[4].y = -1; twds[4].cost = 14;  //左上
            twds[5].x = 1; twds[5].y = -1; twds[5].cost = 14; //右上
            twds[6].x = -1; twds[6].y = 1; twds[6].cost = 14; //左下
            twds[7].x = 1; twds[7].y = 1; twds[7].cost = 14; //右下
        }




        public bool FindPath(Point start, Point end, Graphics gra, Pen myPen)
        {
            Pnt pnt_start = new Pnt();
            Pnt pnt_end = new Pnt();

            pnt_start.X = start.X;
            pnt_start.Y = start.Y;

            pnt_end.X = end.X;
            pnt_end.Y = end.Y;

            OpenList.InsertItem(pnt_start);

            while (OpenList.CurPos != 0)
            {
                //找出F值最小的点
                var tempStart = OpenList.PopMinItem();


                //myPen.Color = Color.Red;

                //gra.DrawEllipse(myPen, tempStart.X, tempStart.Y, 1, 1);



                //将F值最小点添加到闭集合
                CloseList[tempStart.X, tempStart.Y] = 1;

                int row, col;
                for (int i = 0; i < 8; i++)
                {
                    //列 为横坐标  对应width
                    row = tempStart.X + twds[i].x;
                    //行 为纵坐标 对应height
                    col = tempStart.Y + twds[i].y;

                    //在地图内部
                    if (row >= 0 && row < width && col >= 0 && col < height)
                    {
                        //不是障碍物且不在closed列表里
                        if (CloseList[row, col] != 1 && MazeArray[row, col] != 1)
                        {
                            if (row != end.X || col != end.Y)
                            {
                                Pnt tmpNode = new Pnt();

                                tmpNode.X = row;
                                tmpNode.Y = col;
                                tmpNode.H = CalcDist(tmpNode, pnt_end);
                                tmpNode.G = tempStart.G + twds[i].cost;// +costMap[col * map.width + row];
                                tmpNode.F = tmpNode.H + tmpNode.G;

                                //如果节点已经在open列表中，比较之前的fvalue和新的fvalue
                                int index = OpenList.IsExist(tmpNode);

                                //如果已经存在
                                if (index != -1)
                                {
                                    if (OpenList.items[index].F > tmpNode.F)
                                    {
                                        OpenList.items[index].F = tmpNode.F;
                                        OpenList.items[index].G = tmpNode.G;
                                        //改变tmpNode的父节点索引
                                        parents[row, col].X = tempStart.X;
                                        parents[row, col].Y = tempStart.Y;
                                    }
                                }
                                else
                                {
                                    //添加tmpNode的父节点索引
                                    parents[row, col].X = tempStart.X;
                                    parents[row, col].Y = tempStart.Y;
                                    OpenList.InsertItem(tmpNode);
                                }
                            }
                            else
                            {
                                //添加tmpNode的父节点索引
                                parents[row, col].X = tempStart.X;
                                parents[row, col].Y = tempStart.Y;
                                //SmoothPath();
                                return true;
                            }
                        }
                    }
                }


            }

            return false;

        }


       
        //获取最终路径
        public bool GetPath(Point start, Point end, Graphics gra, Pen myPen)
        {
            List<PointF> keyPoints = new List<PointF>();
            Point tmpPnt = new Point();
            int count = 0;
            myPen.Color = Color.Yellow;

            tmpPnt = parents[end.X,end.Y];

            keyPoints.Add(end);

            while (tmpPnt != start)
            {
                count++;
                //gra.DrawEllipse(myPen, tmpPnt.X, tmpPnt.Y, 1, 1);
                tmpPnt = parents[tmpPnt.X, tmpPnt.Y];
                if (count % 10 == 0)
                {
                    keyPoints.Add(tmpPnt);
                }
            }

            keyPoints.Add(start);

            PointF[] pnts = keyPoints.ToArray();

            Bspline.DrawBspline1(pnts.Count(), gra, myPen, pnts);
            return true;
        }


        //计算H值
        public int CalcDist(Pnt start, Pnt goal)
        {

            int distX = (int)(Math.Abs(start.X - goal.X) * 10);
            int distY = (int)(Math.Abs(start.Y - goal.Y) * 10);
            return (distX + distY);
            //return (10*(distX+distY)+(14-20)*Math.Min(distX,distY));
        }









        public struct TWD
        {
            public int x, y;
            public int cost;
        }











        //Point temP = new Point();

        ////根据规则更新子节点的GF值
        //private void FoundPoint(Pnt tempStart, Pnt point)
        //{
        //    var G = CalcG(tempStart, point);
        //    if (G < point.G)
        //    {
        //        Point tempP = new Point(tempStart.X, tempStart.Y);
        //        point.ParentPoint = tempP;
        //        point.G = G;
        //        point.F = point.G + point.H;
        //    }
        //}


        ////开辟新的节点，计算G H F值
        //private void NotFoundPoint(Pnt tempStart, Pnt end, Pnt point)
        //{
        //    //if (point.X == end.X && point.Y == end.Y)
        //    //{
        //    //    point.F = point.G + point.H;
        //    //    if (CloseList.IsExist(tempStart))
        //    //    {
        //    //        point.F = point.G + point.H;
 
        //    //    }
        //    //}

        //    Point tempP = new Point(tempStart.X, tempStart.Y);
        //    point.ParentPoint = tempP;
        //    point.G = CalcG(tempStart, point);
        //    point.H = CalcH(end, point);
        //    point.F = point.G + point.H;


        //    Pnt a = FindPa(tempStart);

        //    OpenList.InsertItem(point);
        //}


        ////计算G值
        //private int CalcG(Pnt start, Pnt point)
        //{
        //    int G = (Math.Abs(point.X - start.X) + Math.Abs(point.Y - start.Y)) == 2 ? STEP : OBLIQUE;

        //    Pnt parent = new Pnt();
        //    Pnt temP = new Pnt();
        //    temP.X = point.ParentPoint.X;
        //    temP.Y = point.ParentPoint.Y;

        //    parent = CloseList.FindParent(temP);


        //    return G + parent.G;
        //}

        ////计算H值
        //private int CalcH(Pnt end, Pnt point)
        //{
        //    int step = Math.Abs(point.X - end.X) + Math.Abs(point.Y - end.Y);
        //    return step * 10;
        //}

        ////获取某个点周围可以到达的点
        //public List<Pnt> SurrroundPoints(Pnt point)
        //{
        //    var surroundPoints = new List<Pnt>(9);

        //    for (int x = point.X - 1; x <= point.X + 1; x++)
        //        for (int y = point.Y - 1; y <= point.Y + 1; y++)
        //        {
        //            if (CanReach(x, y))
        //            {
        //                Pnt temPnt = new Pnt();
        //                temPnt.X = x;
        //                temPnt.Y = y;
        //                surroundPoints.Add(temPnt);
        //            }
        //        }
        //    return surroundPoints;
        //}

        ////在二维数组对应的位置不为障碍物
        //private bool CanReach(int x, int y)
        //{
        //    Pnt temP = new Pnt();
        //    temP.X = x;
        //    temP.Y = y;


        //    if (MazeArray[x, y] == 0 && !CloseList.IsExist(temP))
        //     return true;


        //    return false;
        //}


        //判断是否为可以添加到开集合
        //public bool CanReach(Point start, int x, int y, bool IsIgnoreCorner)
        //{

        //    if (!CanReach(x, y) || CloseList.IsExist(x, y))
        //        return false;
        //    else
        //    {
        //        if (Math.Abs(x - start.X) + Math.Abs(y - start.Y) == 1)
        //            return true;
        //        //如果是斜方向移动, 判断是否 "拌脚"
        //        else
        //        {
        //            if (CanReach(Math.Abs(x - 1), y) && CanReach(x, Math.Abs(y - 1)))
        //                return true;
        //            else
        //                return IsIgnoreCorner;
        //        }
        //    }
        //}
    //}

    //Point 类型
//    public struct Point
//    {
//        public Point ParentPoint { get; set; }
//        public int F { get; set; }  //F=G+H
//        public int G { get; set; }
//        public int H { get; set; }
//        public int X { get; set; }
//        public int Y { get; set; }

//        public Point(int x, int y)
//        {
//            this.X = x;
//            this.Y = y;
//        }

        
//        public void CalcF()
//        {
//            this.F = this.G + this.H;
//        }
//    }

//    //对 List<Point> 的一些扩展方法
//    public static class ListHelper
//    {
//        //判断是否在开集合里
//        public static bool Exists(this List<Point> points, Point point)
//        {
//            foreach (Point p in points)
//                if ((p.X == point.X) && (p.Y == point.Y))
//                    return true;
//            return false;
//        }

//        //判断是否在开集合里
//        public static bool Exists(this List<Point> points, int x, int y)
//        {
//            foreach (Point p in points)
//                if ((p.X == x) && (p.Y == y))
//                    return true;
//            return false;
//        }

//        //找数组中的F最小值
//        public static Point MinPoint(this List<Point> points)
//        {
//            points = points.OrderBy(p => p.F).ToList();
//            return points[0];
//        }

//        //添加新点在集合里
//        public static void Add(this List<Point> points, int x, int y)
//        {
//            Point point = new Point(x, y);
//            points.Add(point);
//        }

//        //判断某点是不是在点集合里
//        public static Point Get(this List<Point> points, Point point)
//        {
//            foreach (Point p in points)
//                if ((p.X == point.X) && (p.Y == point.Y))
//                    return p;
//            Point a = new Point(-1,-1);
//            return a;
//        }

//        //将某点在集合中去除
//        public static void Remove(this List<Point> points, int x, int y)
//        {
//            foreach (Point point in points)
//            {
//                if (point.X == x && point.Y == y)
//                    points.Remove(point);
//            }
//        }
    }
}
