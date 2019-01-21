using Assets.Code;
using System.Collections;
using System.Linq;
using UnityEngine;

public class HandStacker : MonoBehaviour {

    public float spacing;

	void Start ()
    {
        ReDrawHand();
    }

    public void ReDrawHand()
    {
        StartCoroutine(ReDrawDelayed());
    }

    IEnumerator ReDrawDelayed()
    {
        yield return new WaitForEndOfFrame();

        var children = GetComponentsInChildren<CardController>();

        if (children.Any())
        {
            var cardSize = Vector3.Distance(children.First().leftPoint.position, children.First().rightPoint.position);
            var childCount = children.Count();
            var totalSize = (cardSize + spacing) * childCount;
            var leftPoint = (totalSize / 2) * -1;
            var currentPoint = new Vector3(leftPoint + cardSize / 2, transform.position.y, transform.position.z);

            foreach (CardController child in children)
            {
                child.transform.position = currentPoint;
                currentPoint = new Vector3(currentPoint.x + spacing + cardSize, currentPoint.y, currentPoint.z);
            }
        }
    }
}
