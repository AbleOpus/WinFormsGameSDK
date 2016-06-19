using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace WinFormsGameSDK
{
    /// <summary>
    /// Represents a geometric shape for hit testing and drawing to screen as a simple model.
    /// </summary>
    public class Geometry : IDisposable, ICloneable<Geometry>
    {
        /// <summary>
        /// Gets the bounds that just contains all of the points of this geometry.
        /// </summary>
        public RectangleF Bounds
        {
            get
            {
                float minX = Path.PathPoints.Min(p => p.X);
                float minY = Path.PathPoints.Min(p => p.Y);
                float maxX = Path.PathPoints.Max(p => p.X);
                float maxY = Path.PathPoints.Max(p => p.Y);
                return new RectangleF(minX, minY, maxX - minX, maxY - minY);
            }
        }

        /// <summary>
        /// Gets the vertexes of the geometry.
        /// </summary>
        public PointF[] Points => Path.PathPoints;

        /// <summary>
        /// Gets the path of the geometry.
        /// </summary>
        public GraphicsPath Path { get; } = new GraphicsPath();

        /// <summary>
        /// Gets or sets the point in which to apply transforms around.
        /// </summary>
        public PointF TransformPoint { get; set; }

        private float facingDegree;
        /// <summary>
        /// Gets or sets the facing degree of the geometry.
        /// </summary>
        public float FacingDegree
        {
            get { return facingDegree; }
            set
            {
                float diff = facingDegree - value;
                facingDegree = value;
                RotateTransform(diff);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Geometry"/> class
        /// with the specified argument.
        /// </summary>
        /// <param name="path">The path to build the geometry from.</param>
        public Geometry(PointF[] path)
        {
            Path.AddPolygon(path);
            CenterTransformPoint();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Geometry"/> class
        /// with the specified argument.
        /// </summary>
        /// <param name="path">The <see cref="GraphicsPath"/> which will directly represent
        /// the geometry.</param>
        public Geometry(GraphicsPath path)
        {
            Path = path;
            CenterTransformPoint();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Geometry"/> class
        /// with the specified arguments.
        /// </summary>
        /// <param name="path">The <see cref="GraphicsPath"/> which will directly represent
        /// the geometry.</param>
        /// <param name="transformPoint">The starting transform point of this geometry.</param>
        public Geometry(PointF[] path, PointF transformPoint)
        {
            Path.AddPolygon(path);
            TransformPoint = transformPoint;
        }

        /// <summary>
        /// Creates rectangular geometry from the specified rectangle.
        /// </summary>
        /// <param name="bounds">The rectangle to build the geometry from.</param>
        /// <returns>The <see cref="Geometry"/> representation of the specified rectangle.</returns>
        public static Geometry FromRectangle(RectangleF bounds)
        {
            GraphicsPath GP = new GraphicsPath();
            GP.AddRectangle(bounds);
            return new Geometry(GP);
        }

        /// <summary>
        /// Creates rectangular geometry from the specified rectangle.
        /// </summary>
        /// <param name="x">The x position of the rectangle.</param>
        /// <param name="y">The y position of the rectangle.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        /// <returns>The <see cref="Geometry"/> representation of the specified rectangle.</returns>
        public static Geometry FromRectangle(float x, float y, float width, float height)
        {
            return FromRectangle(new RectangleF(x, y, width, height));
        }

        /// <summary>
        /// Creates circular geometry with the specified radius and fidelity.
        /// </summary>
        /// <param name="points">How many points the circle will be constructed from.</param>
        /// <param name="radius">The radius of the circle.</param>
        public static Geometry CreateCircle(int points, float radius)
        {
            if (points < 3)
                throw new ArgumentException("Value must be greater than or equal to 3.", nameof(points));

            PointF[] vertices = new PointF[points];
            Vector2D vector = new Vector2D(radius, radius);

            for (int i = 0; i < points; i++)
            {
                Vector2D temp = vector.Clone();
                temp.FacingDegree = i * 360f / points;
                temp.Project(radius);
                vertices[i] = temp.Position;
            }

            return new Geometry(vertices);
        }

        /// <summary>
        /// Gets whether this geometry collides with the specified geometry.
        /// </summary>
        /// <param name="geo">The geometry to test against.</param>
        /// <returns>The point in the target geometry that was hit. If no point found, 
        /// then Point.Empty will be returned.</returns>
        public PointF CollidesWith(Geometry geo)
        {
            return geo.Points.FirstOrDefault(Path.IsVisible);
        }

        /// <summary>
        /// Gets whether this geometry contains the specified point.
        /// </summary>
        /// <param name="point">The point to test.</param>
        /// <returns>True, if contains, otherwise false.</returns>
        public bool ContainsPoint(Point point)
        {
            return Path.IsVisible(point);
        }

        /// <summary>
        /// Gets whether this geometry contains the specified point.
        /// </summary>
        /// <param name="point">The point to test.</param>
        /// <returns>True, if contains, otherwise false.</returns>
        public bool ContainsPoint(PointF point)
        {
            return Path.IsVisible(point);
        }

        /// <summary>
        /// Gets whether this geometry contains the specified point.
        /// </summary>
        /// <param name="x">The x position of the point to test.</param>
        /// <param name="y">The y position of the point to test.</param>
        /// <returns>True, if contains, otherwise false.</returns>
        public bool ContainsPoint(float x, float y)
        {
            return ContainsPoint(new PointF(x, y));
        }

        /// <summary>
        /// Rotate transforms the geometry by the specified degrees.
        /// </summary>
        /// <param name="degrees">The degrees to rotate.</param>
        private void RotateTransform(float degrees)
        {
            using (Matrix matrix = new Matrix())
            {

                matrix.RotateAt(degrees, TransformPoint);
                Path.Transform(matrix);
            }
        }

        /// <summary>
        /// Offsets both the geometries path and transform point by the specified amount.
        /// </summary>
        /// <param name="point">The offset amount.</param>
        public void MoveTransform(PointF point)
        {
            float xDiff = point.X - TransformPoint.X;
            float yDiff = point.Y - TransformPoint.Y;
            TransformPoint = new PointF(TransformPoint.X + xDiff, TransformPoint.Y + yDiff);
            Offset(xDiff, yDiff);
        }

        /// <summary>
        /// Offsets both the geometries path and transform point by the specified amount.
        /// </summary>
        /// <param name="x">The offset amount along the x-axis.</param>
        /// <param name="y">The offset amount along the y-axis.</param>
        public void MoveTransform(float x, float y)
        {
            MoveTransform(new PointF(x, y));
        }

        /// <summary>
        /// Offsets this geometry by the specified amount.
        /// This does not move the transform point.
        /// </summary>
        /// <param name="x">The amount to offset along the x-axis.</param>
        /// <param name="y">The amount to offset along the y-axis.</param>
        public void Offset(float x, float y)
        {
            Offset(new PointF(x, y));
        }

        /// <summary>
        /// Offsets this geometry by the specified amount.
        /// This does not move the transform point.
        /// </summary>
        /// <param name="offset">The amount to offset.</param>
        private void Offset(PointF offset)
        {
            PointF[] points = new PointF[Path.PointCount];
            for (int i = 0; i < Path.PathPoints.Length; i++)
            {
                points[i] = new PointF(
                    Path.PathPoints[i].X + offset.X,
                    Path.PathPoints[i].Y + offset.Y);
            }

            Path.Reset();
            Path.AddPolygon(points);
        }

        /// <summary>
        /// Centers the transform point inside the geometry's path.
        /// </summary>
        private void CenterTransformPoint()
        {
            float avgX = Path.PathPoints.Average(p => p.X);
            float avgY = Path.PathPoints.Average(p => p.Y);
            TransformPoint = new PointF(avgX, avgY);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Path.Dispose();
        }

        /// <summary>
        /// Creates a clone of this <see cref="Geometry"/> instance.
        /// </summary>
        /// <returns>The cloned instance.</returns>
        public Geometry Clone()
        {
            return new Geometry((GraphicsPath)Path.Clone())
            { TransformPoint = TransformPoint };
        }
    }
}
