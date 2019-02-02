using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
