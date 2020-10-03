using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class HexMapEditor : MonoBehaviour
    {
        public Color[] colors;
        public Layout layout;

        private Color activeColor;

        private void Awake()
        {
            SelectColor(0);
        }

        private void Update()
        {
            if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                HandleInput();
            }
        }

        void HandleInput()
        {
            Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(inputRay, out hit))
            {
                layout.ColorCell(hit.point, activeColor);
            }
        }

        public void SelectColor(int index)
        {
            activeColor = colors[index];
        }
    }
}
