using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandDisplay : MonoBehaviour
{
    public CardBehaviour cardPrefab;

    [Header("Card Spawn Locations")]
    public RectTransform playerHandSpawn;
    public RectTransform enemyHandSpawn;

    // Start is called before the first frame update
    void Start()
    {
        EventDispatcher.Instance.AddEventHandler<HandChangeEvent>(4, OnHandChange);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnHandChange(short type, HandChangeEvent eventData)
    {
        var handCards = eventData.HandCards;
        if (!eventData.IsEnemy)
        {
            foreach (var handCard in handCards)
            {
                handCard.SetDisplay(true);
                handCard.SetCardBack(false);
                handCard.transform.SetParent(playerHandSpawn, false);
            }
        }
        else
        {
            foreach (var handCard in handCards)
            {
                handCard.SetDisplay(true);
                handCard.SetCardBack(true);
                handCard.transform.SetParent(enemyHandSpawn, false);
            }
        }
    }
}
