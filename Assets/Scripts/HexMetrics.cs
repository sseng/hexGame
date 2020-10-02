using UnityEngine;

namespace Assets.Scripts
{
    //metrics for Pointy top hexagon
    public static class HexMetrics
    {
        public const float size = 10f;

        // outer radius = distance from center to each corner.
	    public const float outerRadius = 1 * size;
        //inner radius = distance from center of hex to each side.
        //inner radius is equal to sqrt(3)/2 * outer radius
	    public const float innerRadius = 0.866025404f * outerRadius;
        public const float height = 2* outerRadius;
        public const float rowHeight = 1.5f * outerRadius;
        public static float halfWidth = Mathf.Sqrt((outerRadius * outerRadius) - ((outerRadius / 2) * (outerRadius / 2)));
        public static float width = 2 * halfWidth;
        public static float extraHeight = height - rowHeight;


        public static Vector3[] corners = {
            new Vector3(0f, 0f, outerRadius),
            new Vector3(innerRadius, 0f, 0.5f * outerRadius),
            new Vector3(innerRadius, 0f, -0.5f * outerRadius),
            new Vector3(0f, 0f, -outerRadius),
            new Vector3(-innerRadius, 0f, -0.5f * outerRadius),
            new Vector3(-innerRadius, 0f, 0.5f * outerRadius),
            new Vector3(0f, 0f, outerRadius)
        };
    }
}