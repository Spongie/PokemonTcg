using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code._2D
{
    [RequireComponent(typeof(GridLayoutGroup))]
    public class GridSizeFixer : MonoBehaviour
    {
        private GridLayoutGroup gridLayout;
        private RectTransform rectTransform;

        public void UpdateFromInspector()
        {
            gridLayout = GetComponent<GridLayoutGroup>();
            rectTransform = GetComponent<RectTransform>();
            Update();
        }

        private void Start()
        {
            gridLayout = GetComponent<GridLayoutGroup>();
            rectTransform = GetComponent<RectTransform>();
        }

        public void Update()
        {
            var cellSize = rectTransform.rect.width / gridLayout.constraintCount;

            gridLayout.cellSize = new Vector2(cellSize, cellSize);
        }
    }
}
