﻿using System;
using System.Drawing;
using System.Runtime.Serialization;

namespace GameTesterClean
{
	public struct Vector
	{

		public double X { get; set; }
		public double Y { get; set; }

		static public Vector FromPoint(Point p)
		{
			return Vector.FromPoint(p.X, p.Y);
		}

		static public Vector FromPoint(int x, int y)
		{
			return new Vector((double)x, (double)y);
		}

		public Vector(double x, double y)
		{
			this.X = x;
			this.Y = y;
		}

		[IgnoreDataMember]
		public float Magnitude
		{
			get { return (float)Math.Sqrt(X * X + Y * Y); }
		}

		public void Normalize()
		{
			float magnitude = Magnitude;
			X = X / magnitude;
			Y = Y / magnitude;
		}

		public Vector GetNormalized()
		{
			float magnitude = Magnitude;

			return new Vector(X / magnitude, Y / magnitude);
		}

		public double DotProduct(Vector vector)
		{
			return this.X * vector.X + this.Y * vector.Y;
		}

		public double DistanceTo(Vector vector)
		{
			return (float)Math.Sqrt(Math.Pow(vector.X - this.X, 2) + Math.Pow(vector.Y - this.Y, 2));
		}

		public static implicit operator Point(Vector p)
		{
			return new Point((int)p.X, (int)p.Y);
		}

		public static implicit operator PointF(Vector p)
		{
			return new PointF((float)p.X, (float)p.Y);
		}

		public static Vector operator +(Vector a, Vector b)
		{
			return new Vector(a.X + b.X, a.Y + b.Y);
		}

		public static Vector operator -(Vector a)
		{
			return new Vector(-a.X, -a.Y);
		}

		public static Vector operator -(Vector a, Vector b)
		{
			return new Vector(a.X - b.X, a.Y - b.Y);
		}

		public static Vector operator *(Vector a, float b)
		{
			return new Vector(a.X * b, a.Y * b);
		}

		public static Vector operator *(Vector a, int b)
		{
			return new Vector(a.X * b, a.Y * b);
		}

		public static Vector operator *(Vector a, double b)
		{
			return new Vector((float)(a.X * b), (float)(a.Y * b));
		}

		public override bool Equals(object obj)
		{
			Vector v = (Vector)obj;

			return X == v.X && Y == v.Y;
		}

		public bool Equals(Vector v)
		{
			return X == v.X && Y == v.Y;
		}

		public override int GetHashCode()
		{
			return X.GetHashCode() ^ Y.GetHashCode();
		}

		public static bool operator ==(Vector a, Vector b)
		{
			return a.X == b.X && a.Y == b.Y;
		}

		public static bool operator !=(Vector a, Vector b)
		{
			return a.X != b.X || a.Y != b.Y;
		}

		public override string ToString()
		{
			return X + ", " + Y;
		}

		public string ToString(bool rounded)
		{
			if (rounded)
			{
				return (int)Math.Round(X) + ", " + (int)Math.Round(Y);
			}
			else
			{
				return ToString();
			}
		}

	}

}
