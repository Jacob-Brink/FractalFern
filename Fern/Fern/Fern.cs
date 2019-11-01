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
        private static double SEGLENGTH = 50;
        private static int LEVEL_MAX = 3;
        private static int BRANCHES = 1;
        private static double STEM_CLEARANCE = .3;
        private static System.Drawing.Color leafColor = System.Drawing.Color.FromArgb(200, 10, 100, 10);
        private static System.Drawing.Color branchColor = System.Drawing.Color.FromArgb(100, 20, 10, 0);

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
            //set instance variables
            this.random = new Random();
            this.g = g;
            this.width = width;
            this.height = height;

            double length, direction;
            direction = 0;// random.NextDouble() * Math.PI / 2;
            for (int j = 0; j < BRANCHES; j++)
            {
                length = randomInRange(2) + 800;
                //direction += random.NextDouble() * Math.PI / 200  +  2 * Math.PI / BRANCHES;
                generateMain(1, length, density, new System.Drawing.Point(width / 2, height / 2), direction, turnbias, size, 0);
            }
        }

        private double randomInRange(double range)
        {
            return random.NextDouble() * range - range / 2;
        }

        

        private void generateMain(int level, double length, double density, System.Drawing.Point startPoint, double direction, double turnbias, double size, double turnDirection)
        {
            if (level > LEVEL_MAX)
                return;

            //creates number of points relative to length
            int points = 80;

            //get segmentLength
            double segmentLength = length / points;

            //initialize and set first point of curve array
            System.Drawing.Point[] branchPoints = new System.Drawing.Point[points];
            branchPoints[0] = startPoint;

            //directional variables
            double lastDirectionOffset, currentOffset;
            lastDirectionOffset = 0;

            double position;
            double x = startPoint.X;
            double y = startPoint.Y;
            int shifted_i;

            for (int i = 1; i < branchPoints.Length; ++i)
            {
                position = (double) i / branchPoints.Length;
                shifted_i = (int) (i -  STEM_CLEARANCE * branchPoints.Length);

                currentOffset = getDirectionOffset(level, size, i, points, direction, lastDirectionOffset);
                //currentOffset = turnDirection != 0 ? currentOffset * turnDirection : currentOffset * ((random.NextDouble() * 2) < 1 ? -1 : 1); 
                direction += currentOffset;
                lastDirectionOffset = currentOffset;

                x += segmentLength * Math.Cos(direction);
                y += segmentLength * Math.Sin(direction);

                branchPoints[i] = new System.Drawing.Point((int)x, (int)y);

                double newDirectionOffset = getNewBranchDirectionOffset(shifted_i, points, level);
                double newLength = getLength(level, length, (int) shifted_i, points);
                
                if (position < STEM_CLEARANCE)
                    continue;

                //last branch is in same direction
                if (i == points - 1)
                    newDirectionOffset = 0;

                //grow branches staggered
                if ((shifted_i % 3) < 1)
                {
                    generateMain(level+1, newLength, density, new System.Drawing.Point((int)x, (int)y), direction + newDirectionOffset, turnbias, size, 1);
                }
                else if ((shifted_i % 3) < 2)
                {
                    generateMain(level+1, newLength, density, new System.Drawing.Point((int)x, (int)y), direction - newDirectionOffset, turnbias, size, -1);
                }

            }

            //draw curve by points
            g.DrawCurve(new System.Drawing.Pen(branchColor, 10 / level), branchPoints);
        }

        private double getNewBranchDirectionOffset(int i, int points, int level)
        {
            return 3 * Math.PI / 8 + random.NextDouble() * Math.PI / 16;
        }

        private double getLength(int level, double currentLength, int positionFromTrunk, int points)
        {

            double newLength;
            if (level == 1)
            {
                double smoothnessFactor = .25;
                double changeLocation = 1 / 5; //where fern changes direction by a significant amount ___/--------
                newLength = (currentLength - Math.Atan(smoothnessFactor * positionFromTrunk - points * changeLocation) * 30 - Math.Pow(positionFromTrunk, .15) * 50) / 4;
            } else
            {
                newLength = (currentLength - positionFromTrunk * 2) / 3;
            }
            
            return newLength > 0 ? newLength : 2;
        }

        private double getDirectionOffset(int level, double age, int positionFromTrunk, int points, double currentDirection, double lastDirectionOffset)
        {
            //spiral for young unfurled ferns
            double directionOffsetRange = Math.PI / (4 * level * points);
            double nextOffset = (randomInRange(Math.PI / 4 + .05 * Math.Pow(positionFromTrunk, 2))) % directionOffsetRange;

            nextOffset += lastDirectionOffset * .75;
            return 0;
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
            
            g.DrawPolygon(new System.Drawing.Pen(leafColor, 4), points);
        }

    }
}


