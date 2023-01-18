using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandDisplay : MonoBehaviour
{
    public CardDisplay cardPrefab;

    [Header("Card Spawn Locations")]
    public RectTransform playerHandSpawn;
    public RectTransform enemyHandSpawn;

    // Start is called before the first frame update
    void OnEnable()
    {
        EventDispatcher.Instance.AddEventHandler<HandChangeEvent>(4, OnHandChange);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnHandChange(short type, HandChangeEvent eventData)
    {
        if (!eventData.IsEnemy)
        {
            foreach (GameObject card in playerHandSpawn.transform)
            {
                Destroy(card);
            }
            foreach (CardId cardId in eventData.HandCards)
            {
                var newCard = Instantiate(cardPrefab, playerHandSpawn);
                newCard.cardData = HandCard.CreateCard(cardId);
            }
        }
        else
        {
            foreach (GameObject card in enemyHandSpawn.transform)
            {
                Destroy(card);
            }
            foreach (CardId cardId in eventData.HandCards)
            {
                var newCard = Instantiate(cardPrefab, enemyHandSpawn);
                newCard.cardData = HandCard.CreateCard(cardId);
            }
        }
    }
}