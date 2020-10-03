using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Assets.Scripts;

namespace Tests
{
    public class TestSuite
    {
        [TestCaseSource("HexData")]
        public void PointToHexReturnsCorrectValues(Hex hex)
        {
            // arrange
            var layout = Object.FindObjectOfType<Layout>();
            layout.Orientation = layout.LayoutOrientation(OrientationType.pointy);
            var hexGrid = HexData();

            // act
            var point = layout.HexToPoint(hex);
            var hexFromPoint = layout.PointToHex(point);

            // assert
            Assert.AreEqual(hex, hexFromPoint);
        }

        private static List<Hex> HexData()
        {
            var hexes = new List<Hex>();
            var mapRadius = 3;

            for (int q = -mapRadius; q <= mapRadius; q++)
            {
                int r1 = Mathf.Max(-mapRadius, -q - mapRadius);
                int r2 = Mathf.Min(mapRadius, -q + mapRadius);
                for (int r = r1; r <= r2; r++)
                {
                    hexes.Add(new Hex(q, r, -q - r));
                }
            }

            return hexes;
        }
    }
}
