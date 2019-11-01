﻿using System;
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
        private static int START_LENGTH = 300;
        private static Color leafColor = Color.FromArgb(200, 10, 100, 10);
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
        public Fern(double directionFallOff, double lengthFallOff, double turnBias, Graphics graphics, int width, int height)
        {
            //set instance variables
            this.width = width;
            this.height = height;
            this.graphics = graphics;

            generateMain(1, width / 2, height / 2, 0, START_LENGTH, directionFallOff, lengthFallOff, turnBias);
        }

        private void generateMain(int level, double startX, double startY, double direction, double length, double directionFallOff, double lengthFallOff, double turnBias)
        {

            if (level > 2)
                return;

            int points = 20;
            Point[] pointList = new Point[points];

            double x = startX;
            double y = startY;
            double segmentDistance;
            double position;
            for (int pointCount = 0; pointCount < points; pointCount++)
            {
                position = (double)pointCount / points;

                pointList[pointCount] = new Point((int)x, (int)y);

                segmentDistance = getNewSegmentDistance(length, position, points);

                x += segmentDistance * Math.Cos(direction);
                y -= segmentDistance * Math.Sin(direction);




                generateMain(level + 1, x, y, getNewDirection(direction, position, directionFallOff), getNewLength(length, position, lengthFallOff), directionFallOff / 2, lengthFallOff / 2, turnBias / 4);
                generateMain(level + 1, x, y, -getNewDirection(direction, position, directionFallOff), getNewLength(length, position, lengthFallOff), directionFallOff / 2, lengthFallOff / 2, turnBias / 4);
            }

            graphics.DrawCurve(new Pen(branchColor, 3), pointList);

        }

        private double getNewSegmentDistance(double length, double position, int points)
        {
            //return (length) / (points*position+1);
            return (length - position * length / 2) / points;
        }
        
        private double getNewDirection(double currentDirection, double position, double fallOff)
        {
            //double directionOffsetRange = Math.PI / 6;
            //double directionOffsetMin = Math.PI / 2;
            //double changePosition = 0.2;
            //return currentDirection - Math.Atan(Math.Pow(position, fallOff*2) - changePosition) * directionOffsetRange + directionOffsetMin;
            return Math.Pow(1 - position, .5) * Math.PI / 2 + currentDirection;
        }

        private double getNewLength(double currentLength, double position, double fallOff)
        {
            double factor = 200;
            int reducingFactor = 4;
            double changePosition = .5;
            return (currentLength - Math.Atan(Math.Pow(position, fallOff*2) - changePosition) * factor) / reducingFactor;
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
            
            graphics.DrawPolygon(new System.Drawing.Pen(leafColor, 4), points);
        }

    }
}


