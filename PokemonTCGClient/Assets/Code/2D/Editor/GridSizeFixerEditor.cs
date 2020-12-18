using UnityEditor;
using UnityEngine;

namespace Assets.Code._2D
{
    [CustomEditor(typeof(GridSizeFixer))]
    public class GridSizeFixerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var gridSizeFixer = (GridSizeFixer)target;

            if (GUILayout.Button("Update component"))
            {
                gridSizeFixer.UpdateFromInspector();
            }
        }
    }
}
