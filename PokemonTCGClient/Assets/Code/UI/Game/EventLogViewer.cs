using BaseSet.TrainerCards;
using System.Collections;
using System.Collections.Generic;
using TCGCards;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.UI.Game
{
    public class EventLogViewer : MonoBehaviour
    {
        public Image image;
        public Queue<Card> events;
        public bool isDisplayingEvent;

        private void Start()
        {
            events = new Queue<Card>();
            StartCoroutine(HandleEventQueue());
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //QueueEvent(new Bill());
            }
        }

        public void QueueEvent(Card card)
        {
            events.Enqueue(card);
        }

        private IEnumerator HandleEventQueue()
        {
            while (true)
            {
                if (events.Count == 0)
                {
                    yield return new WaitForSeconds(0.05f);
                    continue;
                }

                var card = events.Dequeue();

                yield return CardImageLoader.Instance.LoadSpriteRoutine(card, image);

                while (image.color.a < 1)
                {
                    image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + 0.05f);
                    yield return new WaitForSeconds(0.025f);
                }

                yield return new WaitForSeconds(2);

                while (image.color.a > 0)
                {
                    image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - 0.05f);
                    yield return new WaitForSeconds(0.05f);
                }
            }
        }
    }
}
