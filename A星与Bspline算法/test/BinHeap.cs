using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
namespace test
{
    public class BinHeap
    {
        private int size;
        public int Size
        {
            get
            {
                return size;
            }
        }


        private int curPos;
        public int CurPos
        {
            get
            {
                return curPos;
            }
        }

        public Pnt[] items;

        public BinHeap(int size)
        {
            this.size = size;
            items = new Pnt[this.size];           
            this.curPos = 0;
        }


        public void InsertItem(Pnt item)
        {
            int i = 0;
            if (this.curPos == this.size - 1)
            {
                throw new Exception("The heap is fulfilled, can't add item anymore.");  
            }
            for (i = ++(this.curPos); this.items[i / 2].F > item.F; i = i / 2)
            {
                this.items[i] = this.items[i / 2];
            }
            this.items[i] = item;
        }


        public Pnt PopMinItem()
        {
            int minIndex = 1;
            int childIndex = 0;
            int holeIndex = 0;
            Pnt lastNode;
            Pnt popNode;


            lastNode = this.items[this.curPos];
            popNode = this.items[minIndex];

            holeIndex = minIndex;
            childIndex = minIndex * 2;


            while (childIndex <= this.curPos)
            {
                if (lastNode.F < this.items[childIndex].F && lastNode.F < this.items[childIndex + 1].F)
                {
                    break;
                }
                else
                {
                    if ((childIndex < this.curPos) && (this.items[childIndex].F > this.items[childIndex + 1].F))
                        childIndex += 1;
                    this.items[holeIndex] = this.items[childIndex];
                    holeIndex = childIndex;
                    childIndex = holeIndex * 2;
                }
            }
            this.items[holeIndex] = lastNode;
            //free(h->ele[h->curPos]);
            this.curPos--;

            return popNode;
        }

        public int IsExist(Pnt item)
        {
            for (int i = 0; i < this.curPos; i++)
            {
                if (item.X == this.items[i].X && item.Y == this.items[i].Y)
                {
                    return i;
                }
            }
            return -1;
        }




        //public void ReInitHeap()
        //{
        //    Point initPoint = new Point(0, 0);
        //    Pnt initNode;
        //    initNode.F = 0;
        //    initNode.G = 0;
        //    initNode.H = 0;
        //    initNode.ParentPoint = initPoint;
        //    for (int i = 1; i < this.curPos; i++)
        //    {
        //        this.items[i] = initNode;
        //    }
        //}
    }
    //public struct NodeStruct
    //{
    //    public Point point;
    //    public int gValue;
    //    public int hValue;
    //    public int fValue;
    //};
    public struct Pnt
    {
        public int F;
        public int G;
        public int H;
        public int X;
        public int Y;

    }
    

}
