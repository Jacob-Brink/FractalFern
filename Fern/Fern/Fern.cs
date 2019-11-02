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
        private static int START_LENGTH = 500;
        private static int LEVEL_MAX = 3;
        private static Color limeColor = Color.FromArgb(140, 210, 0, 10);
        private static Color darkGreenColor = Color.FromArgb(88, 130, 0);
        private static Color branchColor = Color.FromArgb(200, 20, 10, 0);

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
        public Fern(double age, double lengthFallOff, double turnBias, Graphics graphics, int width, int height)
        {
            //set instance variables
            this.width = width;
            this.height = height;
            this.graphics = graphics;
            this.random = new Random();
            int branchCount = 3;

            for (int i = 0; i < branchCount; i++)
            {
                generateMain(1, width / 2, height / 2, i * Math.PI * 2 /branchCount, START_LENGTH, age, lengthFallOff, turnBias);
            }
        }

        private void generateMain(int level, double startX, double startY, double direction, double length, double  age, double lengthFallOff, double turnBias)
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

                currentDirectionOffset = getDirectionOffset(turnBias, points);

                direction += currentDirectionOffset;

                x += segmentDistance * Math.Cos(direction);
                y -= segmentDistance * Math.Sin(direction);

                if (level == LEVEL_MAX && pointCount % 5 < 1)
                {
                    createLeaf(x, y, getNewDirection(direction, position, 1), getNewLength(length, position, lengthFallOff, level)/1.2, getLeafColor(age, position));
                    createLeaf(x, y, getNewDirection(direction, position, -1), getNewLength(length, position, lengthFallOff, level)/1.2, getLeafColor(age, position));
                }
                    

                if (pointCount % 3 < 1)
                    generateMain(level + 1, x, y, getNewDirection(direction, position, 1), getNewLength(length, position, lengthFallOff, level), age, lengthFallOff / 4, turnBias);
                else if (pointCount % 3 < 2)
                    generateMain(level + 1, x, y, getNewDirection(direction, position, -1), getNewLength(length, position, lengthFallOff, level), age, lengthFallOff / 4, 1-turnBias);
            }

            graphics.DrawCurve(new Pen(branchColor, 1), pointList);

        }

        private Color getLeafColor(double age, double position)
        {
            byte r = (byte) ((limeColor.R - darkGreenColor.R)*position + darkGreenColor.R);
            byte g =(byte) ((limeColor.G - darkGreenColor.G) * position + darkGreenColor.G);
            return Color.FromArgb(r, g, (byte) 200);
        }

        private double getDirectionOffset(double turnBias, int points)
        {
            double offset = Math.PI / 2 / points;
            return (random.NextDouble() > turnBias) ? -1 * offset : offset;// + .25* lastDirectionOffset;
        }

        private double getNewSegmentDistance(double length, double position, int points)
        {
            //return (length) / (points*position+1);
            return (length - position * length / 2) / points;
        }
        
        private double getNewDirection(double currentDirection, double position, int direction)
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

        private void createLeaf(double x, double y, double direction, double size, Color color) {
            double height = 2 * size;
            double width = 1 * size;
            System.Drawing.Point[] points = new System.Drawing.Point[3];
            
            points[0].X = (int)(x + width / 2 * Math.Cos(direction - Math.PI / 2));
            points[0].Y = (int)(y - width / 2 * Math.Sin(direction - Math.PI / 2));

            points[1].X = (int)(x + width / 2 * Math.Cos(Math.PI / 2 + direction));
            points[1].Y = (int)(y - width / 2 * Math.Sin(Math.PI / 2 + direction));
            
            points[2].X = (int)(x + height * Math.Cos(direction));
            points[2].Y = (int)(y - height * Math.Sin(direction));
            
            graphics.FillPolygon(new SolidBrush(color), points);
        }

    }
}


