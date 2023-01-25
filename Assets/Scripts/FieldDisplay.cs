using System;
using UnityEngine;

public class FieldDisplay : MonoBehaviour
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
        newMinion.GetComponent<CardDisplay>().SetCardBack(false);
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
}