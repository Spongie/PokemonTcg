using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code._2D
{
    public class BenchController : MonoBehaviour
    {
        [Header("Slots")]
        public GameObject Slot1;
        public GameObject Slot2;
        public GameObject Slot3;
        public GameObject Slot4;
        public GameObject Slot5;


        public GameObject GetSlot(int index)
        {
            switch (index)
            {
                case 0:
                    return Slot1;
                case 1:
                    return Slot2;
                case 2:
                    return Slot3;
                case 3:
                    return Slot4;
                case 4:
                    return Slot5;
                default:
                    return Slot1;
            }
        }

        internal void ClearSlots()
        {
            for (int i = 0; i < 5; i++)
            {
                var slot = GetSlot(i);

                if (slot.transform.childCount > 0)
                {
                    Destroy(slot.transform.GetChild(0).gameObject);
                }
            }
        }
    }
}
