using System;
using System.Collections.Generic;
using System.Text;

namespace vtkPointCloud
{
    public class Point2D
    {
        public double x;
        public double y;
    };


    public class Point3D
    {
        public double x;
        public double y;
        public double z;
    };


    public class Line2D
    {
        public Point2D startPoint;
        public Point2D endPoint;
    };

    public class Line3D
    {
        public Point3D startPoint;
        public Point3D endPoint;
    };

    public class Pixel
    {
        public int x;
        public int y;
        public int value;
    }
}
