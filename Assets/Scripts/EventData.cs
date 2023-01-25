using System;
using System.Collections.Generic;
using UnityEngine;

// #4: Hand change event
public struct HandChangeEvent
{
    public bool IsEnemy { get; private set; }
    public List<CardBehaviour> HandCards { get; private set; }

    public HandChangeEvent(bool isEnemy, List<CardBehaviour> handCards)
    {
        IsEnemy = isEnemy;
        HandCards = handCards;
    }
}

// #5: Minion deploy event
public struct MinionDeployEvent
{
    public bool IsEnemy { get; private set; }
    public CardBehaviour NewMinion;
    public int Location;

    public MinionDeployEvent(bool isEnemy, CardBehaviour newMinion, int location)
    {
        IsEnemy = isEnemy;
        NewMinion = newMinion;
        Location = location;
    }
}

// #6: Card select event
public struct CardClickEvent
{
    public CardBehaviour Card;

    public CardClickEvent(CardBehaviour card)
    {
        Card = card;
    }
}

// #7: Field Select Event
public struct FieldSelectEvent
{
    public int Location;

    public FieldSelectEvent(int location) { Location = location; }
}