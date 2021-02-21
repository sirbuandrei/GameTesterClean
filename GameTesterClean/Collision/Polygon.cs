using System.Collections.Generic;

namespace GameTesterClean
{
	public class Polygon
	{
		private List<Vector> points = new List<Vector>();
		private List<Vector> edges = new List<Vector>();
		public bool drawOrderGuide;

		public void BuildEdges()
		{
			Vector p1;
			Vector p2;

			edges.Clear();

			for (int i = 0; i < points.Count; i++)
			{
				p1 = points[i];

				if (i + 1 >= points.Count)
					p2 = points[0];
				else
					p2 = points[i + 1];

				edges.Add(p2 - p1);
			}
		}

		public List<Vector> Edges
		{
			get { return edges; }
		}

		public List<Vector> Points
		{
			get { return points; }
		}

		public Vector Center
		{
			get
			{
				float totalX = 0;
				float totalY = 0;

				for (int i = 0; i < points.Count; i++)
				{
					totalX += points[i].X;
					totalY += points[i].Y;
				}

				return new Vector(totalX / (float)points.Count, totalY / (float)points.Count);
			}
		}

		public void Offset(Vector v)
		{
			Offset(v.X, v.Y);
		}

		public void Offset(float x, float y)
		{
			for (int i = 0; i < points.Count; i++)
				points[i] = new Vector(points[i].X + x, points[i].Y + y);
		}

		public override string ToString()
		{
			string result = "";

			for (int i = 0; i < points.Count; i++)
			{
				if (result != "") result += " ";
				result += "{" + points[i].ToString(true) + "}";
			}

			return result;
		}

		public Polygon copy()
		{
			Polygon p = new Polygon();

			foreach (Vector point in Points)
				p.Points.Add(point);

			foreach (Vector edge in Edges)
				p.Edges.Add(edge);

			p.drawOrderGuide = drawOrderGuide;

			return p;
		}

		public static Polygon fromString(string points, int omit = 0)
		{
			Polygon p = new Polygon();
			string[] pointPairs = points.Split(' ');

			for (int i = 0; i < pointPairs.Length - omit; i++)
			{
				string[] x_y = pointPairs[i].Split(',');

				float x = float.Parse(x_y[0]);
				float y = float.Parse(x_y[1]);

				p.Points.Add(new Vector(x, y));
			}

			return p;
		}

		public static Polygon fromRect(float x, float y, float w, float h)
		{
			Polygon p = new Polygon();

			p.Points.Add(new Vector(x, y));
			p.Points.Add(new Vector(x + w, y));
			p.Points.Add(new Vector(x + w, y + h));
			p.Points.Add(new Vector(x, y + h));

			return p;
		}
	}
}

