using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code._2D
{
    public class SelectIndicator : MonoBehaviour
    {
        public Image image;

        public void SetSelected(bool selected)
        {
            image.enabled = selected;
        }
    }
}
