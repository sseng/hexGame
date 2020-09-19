using UnityEngine;

namespace Assets.Scripts
{
    public class HexCell : MonoBehaviour
    {
        public HexCoordinates coordinates;
        public Color color;

        HexMesh mesh;

        private void Awake()
        {
            mesh = GetComponent<HexMesh>();
        }

        private void Start()
        {
            mesh.Triangulate();
        }
    }
}