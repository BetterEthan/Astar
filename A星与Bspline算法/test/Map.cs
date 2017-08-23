using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace test
{
    class Map
    {
        public int height;

        public int width;

        private string path = Directory.GetCurrentDirectory() + "\\mymap.pgm";

        public int[,] costMap;

        public  Map()
        {
            if (File.Exists(path))
            {
                FileStream fs = new FileStream(path, FileMode.Open);
                StreamReader sr = new StreamReader(fs, Encoding.UTF7);
                string str = string.Empty;

                str = sr.ReadLine();//p5

                str = sr.ReadLine();//comment context

                str = sr.ReadLine();//width height

                string[] size = str.Split(' ');

                width = Convert.ToInt32(size[0]);
                height = Convert.ToInt32(size[1]);

                costMap = new int[height, width];

                str = sr.ReadLine();//255

                str = sr.ReadLine();



                int iUp = 0;
                int iDown = 0;
                int jUp = 0;
                int jDown = 0;
                //读取数据并二值化
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        if ((byte)str[i * (width) + j] > 0)
                        {
                            costMap[(i)  ,(j)] = 0;
                        }
                        else //障碍物体
                        {
                            costMap[(i) , (j)] = 1;
                            iDown = i - 5 < 0 ? 0 : i - 5;
                            iUp = i + 5 > (height - 1) ? (height - 1) : i + 5;
                            jDown = j - 5 < 0 ? 0 : j - 5;
                            jUp = j + 5 > (width - 1) ? (width - 1) : j + 5;

                            for (int q = iDown; q <= iUp; q++)
                            {
                                for (int w = jDown; w <= jUp; w++)
                                {
                                    costMap[q, w] = 1;
                                }
                            }
                        }
                    }
                }



                ////膨胀障碍物
                //for (int i = 0; i < height; i++)
                //{
                //    for (int j = 0; j < width; j++)
                //    {

                //        if (costMap[(i), (j)] == 1)
                //        {
                //            iDown = i - 5 < 0 ? 0 : i - 5;
                //            iUp = i + 5 > (height - 1) ? (height - 1) : i + 5;
                //            jDown = j - 5 < 0 ? 0 : j - 5;
                //            jUp = j + 5 > (width-1) ? (width-1) : j + 5;

                //            for (int q = iDown; q <= iUp; q++)
                //            {
                //                for (int w = jDown; w <= jUp; w++)
                //                {
                //                    costMap[q, w] = 1;
                //                }
                //            }
                //        }

                //    }
                //}


            }

        }


    }
}
