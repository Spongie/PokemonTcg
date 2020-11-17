using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.UI.Events
{
    public class CoinflipEventHandler : MonoBehaviour
    {
        public Sprite CoinFront;
        public Sprite CoinBack;
        public Image CoinImage;
        public Text HeadsCountText;
        public Text TailsCountText;
        public RectTransform CoinTransform;

        public void TriggerCoinFlips(List<bool> results)
        {
            StartCoroutine(CoinFlipEnumerator(results));
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(CoinFlipEnumerator(new List<bool> { false }));
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                StartCoroutine(CoinFlipEnumerator(new List<bool> { true }));
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                StartCoroutine(CoinFlipEnumerator(new List<bool> { UnityEngine.Random.Range(0, 2) > 0, UnityEngine.Random.Range(0, 2) > 0, UnityEngine.Random.Range(0, 2) > 0 }));
            }
        }

        IEnumerator CoinFlipEnumerator(List<bool> results)
        {
            int tails = 0;
            int heads = 0;

            HeadsCountText.text = heads.ToString();
            TailsCountText.text = tails.ToString();

            foreach (var result in results)
            {
                var flips = result ? 10 : 11;
                bool increasing = false;
                CoinImage.sprite = CoinFront;

                for (int i = 0; i < flips; i++)
                {
                    if (increasing)
                    {
                        while (CoinTransform.localScale.x < 1)
                        {
                            CoinTransform.localScale = new Vector3(CoinTransform.localScale.x + 0.2f, 1, 1);
                            yield return new WaitForSeconds(0.025f);
                        }

                        CoinTransform.localScale = new Vector3(1, 1, 1);
                        increasing = !increasing;
                        CoinImage.sprite = CoinFront;
                    }
                    else
                    {
                        while (CoinTransform.localScale.x > 0.05f)
                        {
                            CoinTransform.localScale = new Vector3(CoinTransform.localScale.x - 0.2f, 1, 1);
                            yield return new WaitForSeconds(0.025f);
                        }

                        increasing = !increasing;
                        CoinImage.sprite = CoinBack;

                        if (i + 1 == flips)
                        {
                            while (CoinTransform.localScale.x < 1)
                            {
                                CoinTransform.localScale = new Vector3(CoinTransform.localScale.x + 0.2f, 1, 1);
                                yield return new WaitForSeconds(0.025f);
                            }

                            CoinTransform.localScale = new Vector3(1, 1, 1);
                            CoinImage.sprite = CoinBack;
                        }
                    }
                }

                heads += result ? 1 : 0;
                tails += result ? 0 : 1;

                HeadsCountText.text = heads.ToString();
                TailsCountText.text = tails.ToString();

                if (heads + tails < results.Count)
                {
                    yield return new WaitForSeconds(0.5f);
                }
            }
        }
    }
}
