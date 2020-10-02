using UnityEngine;
using UnityEditor;

namespace Assets.Scripts
{
    [CustomPropertyDrawer(typeof(Hex))]
    public class HexCoordinatesDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Hex coordinates = new Hex(
                property.FindPropertyRelative("x").intValue,
                property.FindPropertyRelative("z").intValue
            );
            position = EditorGUI.PrefixLabel(position, label);
            GUI.Label(position, coordinates.ToString());
        }
    }
}
