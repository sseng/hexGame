using System;
using UnityEngine;

namespace Assets.Scripts
{
    public struct Hex
    {
        public int Q { get; }
        public int R { get; }
	    public int S { get; }

        public Hex(int q, int r) : this()
        {
            Q = q;
            R = r;
        }

        public Hex(int q, int r, int s) : this(q, r)
        {
            S = s;
        }

        #region square board calculations
        public static Hex FromOffsetCoordinates(Vector3 position)
        {
            var x = Convert.ToInt32(position.x);
            var z = Convert.ToInt32(position.z);
            return new Hex(x - z / 2, z);
        }

        public static Hex FromPositionSquareBoard(Vector3 position)
        {
		    float x = position.x / (HexMetrics.innerRadius * 2f);
		    float y = -x;

		    float offset = position.z / (HexMetrics.outerRadius * 3f);
		    x -= offset;
		    y -= offset;

		    int iX = Mathf.RoundToInt(x);
		    int iY = Mathf.RoundToInt(y);
		    int iZ = Mathf.RoundToInt(-x -y);

		    if (iX + iY + iZ != 0)
            {
			    float dX = Mathf.Abs(x - iX);
			    float dY = Mathf.Abs(y - iY);
			    float dZ = Mathf.Abs(-x -y - iZ);

			    if (dX > dY && dX > dZ)
                {
				    iX = -iY - iZ;
			    }
			    else if (dZ > dY)
                {
				    iZ = -iX - iY;
			    }
		    }

		    return new Hex(iX, iZ);
	    }
        #endregion

        public override string ToString()
        {
            return $"({Q.ToString()}, {R.ToString()}, {S.ToString()})";
        }

        public string ToStringWithIndex(int index)
        {
            return $"({index}){Environment.NewLine}{Q.ToString()}   {R.ToString()}   {S.ToString()}";
        }

        public string ToStringQR(int index)
        {
            return $"({index}){Environment.NewLine}{Q.ToString()},{R.ToString()}";
        }

        public string ToStringOnSeparateLines()
        {
		    return $"{Q.ToString()}{Environment.NewLine}{S.ToString()}{Environment.NewLine}{R.ToString()}";
	    }
    }
}