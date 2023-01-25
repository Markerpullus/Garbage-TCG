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
        /*
        if (!eventData.IsEnemy)
        {
            foreach (Transform child in playerHandSpawn.transform)
            {
                Destroy(child.gameObject);
            }
            foreach (CardId cardId in eventData.HandCards)
            {
                var newCard = Instantiate(cardPrefab, playerHandSpawn);
                newCard.cardData = CardScriptable.LoadCardDataFromDisk(cardId);
                newCard.SetCardBack(false);
            }
        }
        else
        {
            foreach (Transform child in enemyHandSpawn.transform)
            {
                Destroy(child.gameObject);
            }
            foreach (CardId cardId in eventData.HandCards)
            {
                var newCard = Instantiate(cardPrefab, enemyHandSpawn);
                newCard.cardData = CardScriptable.LoadCardDataFromDisk(cardId);
                newCard.SetCardBack(true);
            }
        }*/
    }
}
