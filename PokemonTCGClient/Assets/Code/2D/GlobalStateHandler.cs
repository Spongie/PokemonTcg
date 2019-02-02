using UnityEngine;

public class GlobalStateHandler : MonoBehaviour
{
    public static GlobalStateHandler Instance;

    public bool isDragging = false;

    private void Awake()
    {
        Instance = this;
    }
}
