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
        private static double SEGLENGTH = 15;
        private static int LEVEL_MAX = 3;
        private static int BRANCHES = 3;

        private Graphics g;
        private int width, height;
        private Random random;

        /* 
         * Fern constructor erases screen and draws a fern
         * 
         * Size: number of 3-pixel segments of tendrils
         * Redux: how much smaller children clusters are compared to parents
         * Turnbias: how likely to turn right vs. left (0=always left, 0.5 = 50/50, 1.0 = always right)
         * canvas: the canvas that the fern will be drawn on
         */
        public Fern(double size, double density, double turnbias, Graphics g, int width, int height)
        {
            this.random = new Random();
            this.g = g;
            this.width = width;
            this.height = height;

            double length, direction;
            direction = 0;
            for (int j = 0; j < BRANCHES; j++)
            {
                length = randomInRange(2) + 10;
                direction += random.NextDouble() * Math.PI / 200  +  2 * Math.PI / BRANCHES;
                growBranch(1, width/2, height/2, length, direction, turnbias, density);
            }
        }

        private double randomInRange(double range)
        {
            return random.NextDouble() * range - range / 2;
        }

        private void growBranch(int level, double x, double y, double length, double direction, double turnbias, double age)
        {
            if (level >= LEVEL_MAX)
                return;//todo: add leaves

            int points = (int)length + 3;
            System.Drawing.Point[] branchPoints = new System.Drawing.Point[points];
            System.Drawing.Point point = new System.Drawing.Point((int)x, (int)y);
            branchPoints[0] = point;

            double lastDirectionOffset, currentOffset;
            lastDirectionOffset = 0;

            double directionOffsetRange = Math.PI / (8 * level);


            for (int i = 1; i < branchPoints.Length; i++)
            {
                
                
                if (false)
                {
                    direction += i*i*i *.00025 + i * .0025;
                } else
                {
                    currentOffset = randomInRange(Math.PI / 4 + .05 * i * i) % directionOffsetRange;
                    currentOffset = random.NextDouble() < turnbias ? currentOffset : 0;
                    
                }
                direction += currentOffset + lastDirectionOffset * .75;
                lastDirectionOffset = currentOffset;


                x += (SEGLENGTH * length / 5 *  Math.Cos(direction));
                y += (SEGLENGTH * length / 5 * Math.Sin(direction));
                branchPoints[i] = new System.Drawing.Point((int) x, (int) y);

                
                

                //branch off either left or right
                if (true)
                {
                    growBranch(level + 1, x, y, length / (1.9 * Math.Pow(i, .5)), direction + (Math.PI / 3), turnbias, age);
                    growBranch(level + 1, x, y, length / (1.9 * Math.Pow(i, .5)), direction - (Math.PI / 3), turnbias, age);
                }
                //Rectangle rect = new Rectangle((int) x - 5, (int)y - 5, 10, 10);
                //g.DrawRectangle(new System.Drawing.Pen(System.Drawing.Color.Green, 2), rect);

            }
            System.Drawing.Color color = System.Drawing.Color.FromArgb(255 / level, 200, 0);
            g.DrawCurve(new System.Drawing.Pen(color, 5 / level), branchPoints);
        }

        private void createLeaf(System.Drawing.Point startPosition, double direction)
        {

        }



    }
}


