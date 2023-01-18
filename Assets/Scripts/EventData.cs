using System;
using System.Collections.Generic;
using UnityEngine;

// #4: Hand change event
public struct HandChangeEvent
{
    public bool IsEnemy { get; private set; }
    public List<CardId> HandCards { get; private set; }

    public HandChangeEvent(bool isEnemy, List<CardId> handCards)
    {
        IsEnemy = isEnemy;
        HandCards = handCards;
    }
}