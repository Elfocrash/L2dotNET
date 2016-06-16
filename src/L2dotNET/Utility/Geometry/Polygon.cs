using System;
using System.Collections.Generic;
using System.Linq;
using L2dotNET.Models;

namespace L2dotNET.Utility.Geometry
{
    public class Polygon : AShape
    {
        private const int TRIANGULATION_MAX_LOOPS = 100;

        protected List<AShape> _shapes;

        protected int _size;

        public Polygon(List<AShape> shapes)
        {
            _shapes = shapes;

            _size = shapes.Sum(shape => shape.GetSize());
        }

        public Polygon(int id, List<int[]> points)
        {
            List<Triangle> triangles;
            int size = 0;
            try
            {
                // not a polygon, throw exception
                if (points.Count < 3)
                    throw new IndexOutOfRangeException("Can not create Polygon (id=" + id + ") from less than 3 coordinates.");

                // get polygon orientation
                bool isCw = GetPolygonOrientation(points);

                // calculate non convex points
                List<int[]> nonConvexPoints = CalculateNonConvexPoints(points, isCw);

                // polygon triangulation of points based on orientation and non-convex points
                triangles = DoTriangulationAlgorithm(points, isCw, nonConvexPoints);

                // calculate polygon size
                size += triangles.Cast<AShape>().Sum(shape => shape.GetSize());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                triangles = new List<Triangle>();
            }

            _shapes = triangles.Cast<AShape>().ToList();
            _size = size;
        }

        public override double GetArea()
        {
            return -1;
        }

        public override Location GetRandomLocation()
        {
            int size = Rnd.Get(_size);

            foreach (AShape shape in _shapes)
            {
                size -= shape.GetSize();
                if (size < 0)
                    return shape.GetRandomLocation();
            }

            // should never happen
            return null;
        }

        public override int GetSize()
        {
            return _size;
        }

        public override double GetVolume()
        {
            return -1;
        }

        public override bool IsInside(int x, int y)
        {
            return _shapes.Any(shape => shape.IsInside(x, y));
        }

        public override bool IsInside(int x, int y, int z)
        {
            return _shapes.Any(shape => shape.IsInside(x, y, z));
        }

        private static bool GetPolygonOrientation(List<int[]> points)
        {
            // first find point with minimum x-coord - if there are several ones take the one with maximal y-coord

            // get point
            int size = points.Count;
            int index = 0;
            int[] point = points[0];
            for (int i = 1; i < size; i++)
            {
                int[] pt = points[i];

                // x lower, or x same and y higher
                if ((pt[0] < point[0]) || ((pt[0] == point[0]) && (pt[1] > point[1])))
                {
                    point = pt;
                    index = i;
                }
            }

            // get previous point
            int[] pointPrev = points[GetPrevIndex(size, index)];

            // get next point
            int[] pointNext = points[GetNextIndex(size, index)];

            // get orientation
            int vx = point[0] - pointPrev[0];
            int vy = point[1] - pointPrev[1];
            int res = pointNext[0] * vy - pointNext[1] * vx + vx * pointPrev[1] - vy * pointPrev[0];

            // return
            return res <= 0;
        }

        private static int GetNextIndex(int size, int index)
        {
            // increase index and check for limit
            return ++index >= size ? 0 : index;
        }

        private static int GetPrevIndex(int size, int index)
        {
            // decrease index and check for limit
            if (--index < 0)
                return size - 1;

            return index;
        }

        private static List<int[]> CalculateNonConvexPoints(List<int[]> points, bool isCw)
        {
            // list of non convex points
            List<int[]> nonConvexPoints = new List<int[]>();

            // result value of test function
            int size = points.Count;
            for (int i = 0; i < size - 1; i++)
            {
                // get 3 points
                int[] point = points[i];
                int[] pointNext = points[i + 1];
                int[] pointNextNext = points[GetNextIndex(size, i + 2)];

                int vx = pointNext[0] - point[0];
                int vy = pointNext[1] - point[1];

                // note: cw means res/newres is <= 0
                bool res = (pointNextNext[0] * vy - pointNextNext[1] * vx + vx * point[1] - vy * point[0]) > 0;
                if (res == isCw)
                    nonConvexPoints.Add(pointNext);
            }

            return nonConvexPoints;
        }

        private static List<Triangle> DoTriangulationAlgorithm(List<int[]> points, bool isCw, List<int[]> nonConvexPoints)
        {
            // create the list
            List<Triangle> triangles = new List<Triangle>();

            int size = points.Count;
            int loops = 0;
            int index = 1;
            while (size > 3)
            {
                // get next and previous indexes
                int indexPrev = GetPrevIndex(size, index);
                int indexNext = GetNextIndex(size, index);

                // get points
                int[] pointPrev = points[indexPrev];
                int[] point = points[index];
                int[] pointNext = points[indexNext];

                // check point to create polygon ear
                if (IsEar(isCw, nonConvexPoints, pointPrev, point, pointNext))
                {
                    // create triangle from polygon ear
                    triangles.Add(new Triangle(pointPrev, point, pointNext));

                    // remove middle point from list, update size
                    points.RemoveAt(index);
                    size--;

                    // move index
                    index = GetPrevIndex(size, index);
                }
                else
                {
                    // move index
                    index = indexNext;
                }

                if (++loops == TRIANGULATION_MAX_LOOPS)
                    throw new Exception("Coordinates are not aligned to form monotone polygon.");
            }

            // add last triangle
            triangles.Add(new Triangle(points[0], points[1], points[2]));

            // return triangles
            return triangles;
        }

        private static bool IsEar(bool isCw, List<int[]> nonConvexPoints, int[] A, int[] B, int[] C)
        {
            // ABC triangle
            if (!(IsConvex(isCw, A, B, C)))
                return false;

            // iterate over all concave points and check if one of them lies inside the given triangle
            return nonConvexPoints.All(i => !IsInside(A, B, C, i));
        }

        private static bool IsConvex(bool isCw, int[] A, int[] B, int[] C)
        {
            // get vector coordinates
            int BAx = B[0] - A[0];
            int BAy = B[1] - A[1];

            // get virtual triangle orientation
            bool cw = (C[0] * BAy - C[1] * BAx + BAx * A[1] - BAy * A[0]) > 0;

            // compare with orientation of polygon
            return cw != isCw;
        }

        private static bool IsInside(int[] A, int[] B, int[] C, int[] P)
        {
            // get vector coordinates
            int BAx = B[0] - A[0];
            int BAy = B[1] - A[1];
            int CAx = C[0] - A[0];
            int CAy = C[1] - A[1];
            int PAx = P[0] - A[0];
            int PAy = P[1] - A[1];

            // get determinant
            double detXYZ = BAx * CAy - CAx * BAy;

            // calculate BA and CA coefficient to each P from A
            double ba = (BAx * PAy - PAx * BAy) / detXYZ;
            double ca = (PAx * CAy - CAx * PAy) / detXYZ;

            // check coefficients
            return ((ba > 0) && (ca > 0) && ((ba + ca) < 1));
        }
    }
}