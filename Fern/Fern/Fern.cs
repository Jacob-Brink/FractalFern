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
        private static double SEGLENGTH = 30;
        private static int LEVEL_MAX = 4;
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
        public Fern(double age, double density, double turnbias, Graphics g, int width, int height)
        {
            this.random = new Random();
            this.g = g;
            this.width = width;
            this.height = height;

            double length, direction;
            direction = 0;
            for (int j = 0; j < BRANCHES; j++)
            {
                length = randomInRange(2) + 400;
                direction += random.NextDouble() * Math.PI / 200  +  2 * Math.PI / BRANCHES;
                growBranch(1, width/2, height/2, length, direction, turnbias, age);
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
            
            //creates number of points relative to length
            int points = (int)(length * level / SEGLENGTH) + 2;
            //ensures that true length is brought out due to points a truncated number of length / SEGLENGTH
            double offset = ((length * level / SEGLENGTH) % 1) / points;
            double segmentDistance = (SEGLENGTH * offset + SEGLENGTH) / (level);

            //initialize and set first point of curve array
            System.Drawing.Point[] branchPoints = new System.Drawing.Point[points];
            System.Drawing.Point point = new System.Drawing.Point((int)x, (int)y);
            branchPoints[0] = point;

            double lastDirectionOffset, currentOffset;
            lastDirectionOffset = 0;

            double directionOffsetRange = Math.PI / (32 * level);


            for (int i = 1; i < branchPoints.Length; i++)
            {

                double relativePosition = .5;
                if (false)
                {
                    
                    currentOffset = i * 5 *i + 2;
                } else
                {
                    currentOffset = randomInRange(Math.PI / 4 + .05 * i * i) % directionOffsetRange;
                    currentOffset = random.NextDouble() < turnbias ? currentOffset : 0;
                    currentOffset += lastDirectionOffset * .75;
                }
                direction += currentOffset;
                lastDirectionOffset = currentOffset;


                x += (segmentDistance *  Math.Cos(direction));
                y += (segmentDistance * Math.Sin(direction));
                branchPoints[i] = new System.Drawing.Point((int) x, (int) y);


                Rectangle rect = new Rectangle((int)x , (int)y -2, 5, 5);
                //g.DrawRectangle(new System.Drawing.Pen(System.Drawing.Color.Green, 2), rect);

                //branch off either left or right
                if (level == LEVEL_MAX - 1)
                {
                    for (int b = -1; b < 2; b += 2)
                    {
                        createLeaf(x, y, direction + Math.PI / 2 * b);
                    }
                    continue;
                }
                    

                if ((i % 2) >= 1)
                    continue;

                for (int b = -1; b < 2; b+=2)
                {
                    growBranch(level + 1, x, y, length / (.25 * i + 2 + .05 * i * i), direction + (Math.PI / 3) * b, turnbias, age);
                }
                
                
                

            }
            System.Drawing.Color color = System.Drawing.Color.FromArgb(100, 200, 0);
            g.DrawCurve(new System.Drawing.Pen(color, 5 / level), branchPoints);
        }

        private void createLeaf(double x, double y, double direction) { 

            int height = 14;
            int width = 5;
            System.Drawing.Point[] points = new System.Drawing.Point[3];
            
            points[0].X = (int)(x + width / 2 * Math.Cos(direction - Math.PI / 2));
            points[0].Y = (int)(y + width / 2 * Math.Sin(direction - Math.PI / 2));

            points[1].X = (int)(x + width / 2 * Math.Cos(Math.PI / 2 + direction));
            points[1].Y = (int)(y + width / 2 * Math.Sin(Math.PI / 2 + direction));
            
            points[2].X = (int)(x + width * 1.2 * Math.Cos(direction));
            points[2].Y = (int)(y + height * 1.2 * Math.Sin(direction));

            g.DrawPolygon(new System.Drawing.Pen(System.Drawing.Color.Green, 4), points);
        }



    }
}


