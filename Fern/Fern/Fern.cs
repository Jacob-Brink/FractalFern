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
        private static double SEGLENGTH = 55;
        private static int LEVEL_MAX = 5;
        private static int BRANCHES = 1;

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
            direction = random.NextDouble() * Math.PI / 2;
            for (int j = 0; j < BRANCHES; j++)
            {
                length = randomInRange(2) + 800;
                direction += random.NextDouble() * Math.PI / 200  +  2 * Math.PI / BRANCHES;
                growBranch(1, width/2, height/2, age * length, direction, turnbias, age, density);
            }
        }

        private double randomInRange(double range)
        {
            return random.NextDouble() * range - range / 2;
        }

        private void growBranch(int level, double x, double y, double length, double direction, double turnbias, double age, double density)
        {
            if (level >= LEVEL_MAX)
                return;//todo: add leaves

            double segmentLength = SEGLENGTH / density;

            //creates number of points relative to length
            int points = (int) (length / segmentLength) + 2;

            //ensures that true length is brought out due to points a truncated number of length / SEGLENGTH
            double offset = ((length % segmentLength) / points);
            double segmentDistance = segmentLength * level  + offset;

            //initialize and set first point of curve array
            System.Drawing.Point[] branchPoints = new System.Drawing.Point[points];
            System.Drawing.Point point = new System.Drawing.Point((int)x, (int)y);
            branchPoints[0] = point;

            //directional variables
            double lastDirectionOffset, currentOffset;
            lastDirectionOffset = 0;
            double directionOffsetRange = Math.PI / (32 * level);


            for (int i = 1; i < branchPoints.Length; i++)
            {
                currentOffset = randomInRange(Math.PI / 4 + .05 * i * i) % directionOffsetRange;
                currentOffset = random.NextDouble() < turnbias ? currentOffset : 0;
                currentOffset += lastDirectionOffset * .75;
                
                direction += currentOffset;
                lastDirectionOffset = currentOffset;

                x += (segmentDistance  * Math.Cos(direction));
                y += (segmentDistance  * Math.Sin(direction));
                branchPoints[i] = new System.Drawing.Point((int) x, (int) y);

                //Rectangle rect = new Rectangle((int)x , (int)y -2, 5, 5);
                //g.DrawRectangle(new System.Drawing.Pen(System.Drawing.Color.Green, 2), rect);
                

                //branch off either left or right
                if (level == LEVEL_MAX - 1)
                {
                    for (int b = -1; b < 2; b += 2)
                    {
                        //createLeaf(x, y, direction + Math.PI / 2 * b, 10 / i * .25 + 4);
                    }
                    continue;
                }

                double smoothnessFactor = .5;
                double newLength = (length - Math.Atan(smoothnessFactor * i - branchPoints.Length * smoothnessFactor / 2) * 40 - Math.Pow(i, .25) * 50) / 5;
                newLength = newLength > 0 ? newLength : 2;
                double newDirectionOffset = -1 * Math.Atan(smoothnessFactor * i - branchPoints.Length * smoothnessFactor / 20) * Math.PI / 4 + Math.PI / 2;

                //grow branches staggered
                if ((i % 2) < 1)
                {
                    growBranch(level + 1, x, y, newLength, direction + newDirectionOffset * 1, turnbias, age, density * 4);                   
                } else
                {
                    growBranch(level + 1, x, y, newLength, direction + newDirectionOffset * -1, turnbias, age, density * 4);
                }

            }

            //draw curve by points
            System.Drawing.Color color = System.Drawing.Color.FromArgb(100 / level, (200 * level) % 255, 0);
            g.DrawCurve(new System.Drawing.Pen(color, 5 / level), branchPoints);
        }

        private void createLeaf(double x, double y, double direction, double size) { 

            double height = 2 * size;
            double width = 1 * size;
            System.Drawing.Point[] points = new System.Drawing.Point[3];
            
            points[0].X = (int)(x + width / 2 * Math.Cos(direction - Math.PI / 2));
            points[0].Y = (int)(y + width / 2 * Math.Sin(direction - Math.PI / 2));

            points[1].X = (int)(x + width / 2 * Math.Cos(Math.PI / 2 + direction));
            points[1].Y = (int)(y + width / 2 * Math.Sin(Math.PI / 2 + direction));
            
            points[2].X = (int)(x + height * 1.2 * Math.Cos(direction));
            points[2].Y = (int)(y + height * 1.2 * Math.Sin(direction));

            g.DrawPolygon(new System.Drawing.Pen(System.Drawing.Color.Green, 4), points);
        }



    }
}


