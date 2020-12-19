using System.Collections.Generic;
using TCGCards;
using UnityEngine;

namespace Assets.Code._2D.GameCard
{
    public class AttachedCardPanel : MonoBehaviour
    {
        public GameObject AttachmentPrefab;

        [Header("Slots")]
        public GameObject Slot1;
        public GameObject Slot2;
        public GameObject Slot3;
        public GameObject Slot4;
        public GameObject Slot5;

        public void SetAttachments(List<TrainerCard> attachments)
        {
            for (int i = 0; i < attachments.Count; i++)
            {
                var slot = GetSlot(i);
                var currentAttachment = slot.GetComponent<Attachment>();

                if (currentAttachment == null)
                {
                    var spawnedCard = Instantiate(AttachmentPrefab, slot.transform);
                    spawnedCard.GetComponent<Attachment>().SetCard(attachments[i]);
                }
                else
                {
                    currentAttachment.SetCard(attachments[i]);
                }
            }

            for (int i = attachments.Count; i < 5; i++)
            {
                var slotTransform = GetSlot(i).transform;

                if (slotTransform.childCount > 0)
                {
                    Destroy(slotTransform.GetChild(0).gameObject);
                }

            }
        }

        private GameObject GetSlot(int index)
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
    }
}
