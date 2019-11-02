using System;
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
        private static int START_LENGTH = 400;
        private static int LEVEL_MAX = 3;
        private static Color leafColor = Color.FromArgb(100, 10, 100, 10);
        private static Color branchColor = Color.FromArgb(200, 20, 10, 0);
        private static Pen leafPen = new Pen(leafColor, 1);

        private Graphics graphics;
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
        public Fern(double directionFallOff, double lengthFallOff, double turnBias, Graphics graphics, int width, int height)
        {
            //set instance variables
            this.width = width;
            this.height = height;
            this.graphics = graphics;
            this.random = new Random();
            generateMain(1, width / 2, height / 2, 0, START_LENGTH, directionFallOff, lengthFallOff, turnBias);
        }

        private void generateMain(int level, double startX, double startY, double direction, double length, double directionFallOff, double lengthFallOff, double turnBias)
        {

            if (level > LEVEL_MAX)
                return;

            int points = (int) (20 * level * 1.2);
            Point[] pointList = new Point[points];

            double x = startX;
            double y = startY;
            double segmentDistance;
            double position;
            double currentDirectionOffset = 0;
            double lastDirectionOffset = currentDirectionOffset;
            for (int pointCount = 0; pointCount < points; pointCount++)
            {
                position = (double)pointCount / points;

                pointList[pointCount] = new Point((int)x, (int)y);

                segmentDistance = getNewSegmentDistance(length, position, points);

                currentDirectionOffset = getDirectionOffset(direction, position, turnBias, lastDirectionOffset);
                lastDirectionOffset = currentDirectionOffset;

                direction += currentDirectionOffset;

                x += segmentDistance * Math.Cos(direction);
                y -= segmentDistance * Math.Sin(direction);

                if (level == LEVEL_MAX && pointCount % 5 < 1)
                {
                    createLeaf(x, y, getNewDirection(direction, position, directionFallOff, 1), getNewLength(length, position, lengthFallOff, level), 200, 100, 0);
                    createLeaf(x, y, getNewDirection(direction, position, directionFallOff, -1), getNewLength(length, position, lengthFallOff, level), 200, 100, 0);
                }
                    

                if (pointCount % 3 < 1)
                    generateMain(level + 1, x, y, getNewDirection(direction, position, directionFallOff, 1), getNewLength(length, position, lengthFallOff, level), directionFallOff / 2, lengthFallOff / 4, turnBias / 4);
                else if (pointCount % 3 < 2)
                    generateMain(level + 1, x, y, getNewDirection(direction, position, directionFallOff, -1), getNewLength(length, position, lengthFallOff, level), directionFallOff / 2, lengthFallOff / 4, turnBias / 4);
            }

            graphics.DrawCurve(new Pen(branchColor, 1), pointList);

        }


        private double getDirectionOffset(double currentDirection, double position, double turnBias, double lastDirectionOffset)
        {
            double offsetRange = Math.PI / 30;
            return offsetRange * (random.NextDouble()-.5);// + .25* lastDirectionOffset;
        }

        private double getNewSegmentDistance(double length, double position, int points)
        {
            //return (length) / (points*position+1);
            return (length - position * length / 2) / points;
        }
        
        private double getNewDirection(double currentDirection, double position, double fallOff, int direction)
        {
            return Math.Pow(1 - position, .5) * Math.PI / 2 * direction + currentDirection;
        }

        private double getNewLength(double currentLength, double position, double fallOff, int level)
        {
            double factor = 5;
            int reducingFactor = 5;
            double changePosition = .5;
            double minPosition = .05;

            return (currentLength - (.75 * Math.Atan(Math.Pow(position*(1-minPosition)+minPosition, fallOff * factor + .25 * Math.Pow(level / LEVEL_MAX, 2)) - changePosition)) * currentLength ) / reducingFactor;
        }

        private void createLeaf(double x, double y, double direction, double size, byte r, byte g, byte b) {
            double height = 2 * size;
            double width = 1 * size;
            System.Drawing.Point[] points = new System.Drawing.Point[3];
            
            points[0].X = (int)(x + width / 2 * Math.Cos(direction - Math.PI / 2));
            points[0].Y = (int)(y + width / 2 * Math.Sin(direction - Math.PI / 2));

            points[1].X = (int)(x + width / 2 * Math.Cos(Math.PI / 2 + direction));
            points[1].Y = (int)(y + width / 2 * Math.Sin(Math.PI / 2 + direction));
            
            points[2].X = (int)(x + height * 1.2 * Math.Cos(direction));
            points[2].Y = (int)(y + height * 1.2 * Math.Sin(direction));
            
            graphics.DrawPolygon(leafPen, points);
        }

    }
}


