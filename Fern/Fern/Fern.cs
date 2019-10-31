using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Drawing;

namespace FernNamespace
{
    /*
     * this class draws a fractal fern when the constructor is called.
     * Written as sample C# code for a CS 212 assignment -- October 2011.
     * 
     * Bugs: WPF and shape objects are the wrong tool for the task 
     */
    class Fern
    {
        private static int BERRYMIN = 10;
        private static int TENDRILS = 1;
        private static int TENDRILMIN = 10;
        private static double DELTATHETA = 0.1;
        private static double SEGLENGTH = 200;
        private static int LEVEL_MAX = 2;
        private static int SCALE = 10;
        private static int RESOLUTION = 10;

        private Graphics g;
        /* 
         * Fern constructor erases screen and draws a fern
         * 
         * Size: number of 3-pixel segments of tendrils
         * Redux: how much smaller children clusters are compared to parents
         * Turnbias: how likely to turn right vs. left (0=always left, 0.5 = 50/50, 1.0 = always right)
         * canvas: the canvas that the fern will be drawn on
         */
        public Fern(double size, double redux, double turnbias, Graphics g)
        {
            this.g = g;
            growBranch(1, new System.Drawing.Point(0, 0), 100, 1);
        }

        private void growBranch(int level, System.Drawing.Point point, double length, double direction)
        {       

            System.Drawing.Point[] branchPoints = new System.Drawing.Point[RESOLUTION*200];
          
            for (int i = 0; i < RESOLUTION*200; i++)
            {
                direction += 1;
                point = new System.Drawing.Point(point.X + (int)(SEGLENGTH * Math.Sin(direction)), point.Y + (int)(SEGLENGTH * Math.Cos(direction)));
                branchPoints[i] = point;
            }

            g.DrawCurve(new System.Drawing.Pen(System.Drawing.Color.Red, 20), branchPoints);
        }


        private void createBranch(System.Drawing.Point startPosition, int length, double direction)
        {

        }

        private void createLeaf(System.Drawing.Point startPosition, double direction)
        {

        }



    }
}


