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
        if (!eventData.IsEnemy)
        {
            foreach (Transform child in playerHandSpawn.transform)
            {
                Destroy(child.gameObject);
            }
            foreach (CardId cardId in eventData.HandCards)
            {
                var newCard = Instantiate(cardPrefab, playerHandSpawn);
                newCard.cardData = CardScriptable.LoadCardFromDisk(cardId);
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
                newCard.cardData = CardScriptable.LoadCardFromDisk(cardId);
                newCard.SetCardBack(true);
            }
        }
    }
}
