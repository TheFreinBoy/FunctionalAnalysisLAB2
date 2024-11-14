using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    public struct Point
    {
        public double X;
        public double Y;

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }
    }

    static void Main(string[] args)
    {
        List<Point> points = new List<Point>
        {
            new Point(3, 8),
            new Point(4, 4),
            new Point(4, 6),
            new Point(5, 6),
            new Point(5, 9),
            new Point(6, 5),
            new Point(7, 7),
            new Point(7, 10),
            new Point(9, 9),
            new Point(12, 3),
            new Point(13, 2),
            new Point(14, 2),
            new Point(14, 3),
            new Point(15, 4),
            new Point(13, 12),
            new Point(14, 10),
            new Point(15, 12),
            new Point(15, 14),
            new Point(16, 11),
            new Point(17, 13)
        };

        int countClasters = 2; 
        List<Point> centroids = InitializeCentroids(points, countClasters);
        bool centroidsChanged = true;
        List<List<Point>> clusters = new List<List<Point>>();
        while (centroidsChanged)
        {
            // Розподіл точок по кластерам
            clusters = AssignClusters(points, centroids);

            // Обчислення нових центроїдів
            centroidsChanged = UpdateCentroids(clusters, ref centroids);
        }

        for (int i = 0; i < centroids.Count; i++)
        {
            Console.WriteLine($"Centroid {i + 1}: ({centroids[i].X}, {centroids[i].Y})");
            Console.WriteLine("Points in cluster:");
            foreach (var point in clusters[i])
            {
                Console.WriteLine($"({point.X}, {point.Y})");
            }
            Console.WriteLine();
        }
    }

    private static List<Point> InitializeCentroids(List<Point> points, int k)
    {
        Random rand = new Random();
        return points.OrderBy(x => rand.Next()).Take(k).ToList();
    }

    private static List<List<Point>> AssignClusters(List<Point> points, List<Point> centroids)
    {
        List<List<Point>> clusters = new List<List<Point>>(centroids.Count);
        for (int i = 0; i < centroids.Count; i++)
        {
            clusters.Add(new List<Point>());
        }

        foreach (var point in points)
        {
            int closestCentroidIndex = GetClosestCentroidIndex(point, centroids);
            clusters[closestCentroidIndex].Add(point);
        }

        return clusters;
    }

    private static int GetClosestCentroidIndex(Point point, List<Point> centroids)
    {
        double minDistance = double.MaxValue;
        int closestIndex = -1;

        for (int i = 0; i < centroids.Count; i++)
        {
            double distance = GetEuclideanDistance(point, centroids[i]);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestIndex = i;
            }
        }

        return closestIndex;
    }

    // евклідова відстань
    private static double GetEuclideanDistance(Point p1, Point p2)
    {
        return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
    }

    private static bool UpdateCentroids(List<List<Point>> clusters, ref List<Point> centroids)
    {
        bool changed = false;

        for (int i = 0; i < clusters.Count; i++)
        {
            if (clusters[i].Count > 0)
            {
                double newX = clusters[i].Average(p => p.X);
                double newY = clusters[i].Average(p => p.Y);
                Point newCentroid = new Point(newX, newY);

                if (newCentroid.X != centroids[i].X || newCentroid.Y != centroids[i].Y)
                {
                    centroids[i] = newCentroid;
                    changed = true;
                }
            }
        }

        return changed;
    }
}
