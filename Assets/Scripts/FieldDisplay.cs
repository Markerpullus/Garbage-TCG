using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class FieldDisplay : MonoBehaviour, IPointerClickHandler
{
    [Header("Minion Spawn Locations")]
    public RectTransform playerMinionSpawn;
    public RectTransform enemyMinionSpawn;

    void Start()
    {
        EventDispatcher.Instance.AddEventHandler<MinionDeployEvent>(5, OnMinionDeploy);
    }

    public void OnMinionDeploy(short type, MinionDeployEvent eventData)
    {
        var newMinion = eventData.NewMinion;
        newMinion.transform.localScale = new Vector3(15, 15, 15);
        newMinion.GetComponent<CardBehaviour>().SetCardBack(false);
        if (!eventData.IsEnemy)
        {
            newMinion.transform.SetParent(playerMinionSpawn, false);
        }
        else
        {
            newMinion.transform.SetParent(enemyMinionSpawn, false);
        }
        newMinion.transform.SetSiblingIndex(eventData.Location);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        EventDispatcher.Instance.SendEvent(7, new FieldSelectEvent(1)); // TODO: "1" is temporary, replaced with the slot that the card fits in
    }
}