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
using System.Windows.Shapes;

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
        private static double SEGLENGTH = 3.0;
        private static int LEVEL_MAX = 2;
        private static int SCALE = 10;

        /* 
         * Fern constructor erases screen and draws a fern
         * 
         * Size: number of 3-pixel segments of tendrils
         * Redux: how much smaller children clusters are compared to parents
         * Turnbias: how likely to turn right vs. left (0=always left, 0.5 = 50/50, 1.0 = always right)
         * canvas: the canvas that the fern will be drawn on
         */
        public Fern(double size, double redux, double turnbias, Canvas canvas)
        {
            canvas.Children.Clear();                                // delete old canvas contents
            // draw a new fern at the center of the canvas with given parameters
            cluster((int)(canvas.Width / 2), (int)(canvas.Height / 2), size, redux, turnbias, canvas);
        }

        /*
         * cluster draws a cluster at the given location and then draws a bunch of tendrils out in 
         * regularly-spaced directions out of the cluster.
         */
        private void cluster(int x, int y, double size, double redux, double turnbias, Canvas canvas)
        {
            for (int i = 0; i < TENDRILS; i++)
            {
                // compute the angle of the outgoing tendril
                double theta = i * 2 * Math.PI / TENDRILS;
                tendril(x, y, size, redux, theta, 0, canvas);
            }
        }

        /*
         * tendril draws a tendril (a randomly-wavy line) in the given direction, for the given length, 
         * and draws a cluster at the other end if the line is big enough.
         */
        private void tendril(int x1, int y1, double length, double density, double direction, int level, Canvas canvas)
        {
            int x2 = x1, y2 = y1;
            Random random = new Random();

            //draw current branch
            x2 = x1 + (int)(SEGLENGTH * Math.Sin(direction));
            y2 = y1 + (int)(SEGLENGTH * Math.Cos(direction));
            stem(x1, y1, x2 * SCALE, y2 * SCALE, 1 + length / 5, canvas);

            //base case, leaves
            if (level >= LEVEL_MAX)
            {
                //leaf(x1, y1, direction, canvas);  
                return;
            }
                    
            //recursive step
            for (int num = 0; num < 1; num++)
            {
                //create direction offset that is around perpendicular to branch
                double directionOffset = random.NextDouble() % 30 +45;
                direction = direction + directionOffset;

                //draw pair of branches
                tendril(x1, y1, length / 2, density / 2, direction, ++level, canvas);
                tendril(x1, y1, length / 2, density / 2, direction, ++level, canvas);
            }

        }

        /*
         * draw a red circle centered at (x,y), radius radius, with a black edge, onto canvas
         */
        private void leaf(int x, int y, double direction, Canvas canvas)
        {
            Ellipse myEllipse = new Ellipse();
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();
            mySolidColorBrush.Color = Color.FromArgb(0, 255, 0, 0);
            myEllipse.Fill = mySolidColorBrush;
            myEllipse.StrokeThickness = 1;
            myEllipse.Stroke = Brushes.ForestGreen;
            myEllipse.HorizontalAlignment = HorizontalAlignment.Center;
            myEllipse.VerticalAlignment = VerticalAlignment.Center;
            myEllipse.Width = 2;
            myEllipse.Height = 2;
            myEllipse.SetCenter(x, y);
            canvas.Children.Add(myEllipse);
        }

        /*
         * draw a line segment (x1,y1) to (x2,y2) with given color, thickness on canvas
         */
        private void stem(int x1, int y1, int x2, int y2, double thickness, Canvas canvas)
        {
            Line myLine = new Line();
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();
            mySolidColorBrush.Color = Color.FromArgb(100, 0, 0, 100);
            myLine.X1 = x1;
            myLine.Y1 = y1;
            myLine.X2 = x2;
            myLine.Y2 = y2;
            myLine.Stroke = mySolidColorBrush;
            myLine.VerticalAlignment = VerticalAlignment.Center;
            myLine.HorizontalAlignment = HorizontalAlignment.Left;
            myLine.StrokeThickness = thickness;
            canvas.Children.Add(myLine);
        }
    }
}

/*
 * this class is needed to enable us to set the center for an ellipse (not built in?!)
 */
public static class EllipseX
{
    public static void SetCenter(this Ellipse ellipse, double X, double Y)
    {
        Canvas.SetTop(ellipse, Y - ellipse.Height / 2);
        Canvas.SetLeft(ellipse, X - ellipse.Width / 2);
    }
}

