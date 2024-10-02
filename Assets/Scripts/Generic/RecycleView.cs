using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class RecycleView<T> : MonoBehaviour
{
    [SerializeField] GameObject template;
    [SerializeField] Transform parent;
    List<Card<T>> createdCards;

    public void CreateCards(T[] items)
    {
        if (createdCards == null)
        {
            createdCards = new();
        }
        if (items != null)
            foreach (var item in items)
            {
                if (item == null)
                {
                    continue;
                }
                GameObject obj = Instantiate(template, parent);
                Card<T> card = obj.GetComponent<Card<T>>();

                card.ApplyCardValues(item);

                createdCards.Add(card);
            }
    }

    public void DestroyCards()
    {
        if (createdCards != null)
            foreach (var item in createdCards)
            {
                item.Destroy();
            }

        createdCards = new();
    }
}
