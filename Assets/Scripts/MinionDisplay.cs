using System;
using UnityEngine;

public class MinionDisplay : MonoBehaviour
{
    [Header("Minion Spawn Locations")]
    public RectTransform playerMinionSpawn;
    public RectTransform enemyMinionSpawn;

    void Start()
    {
        if (!EventDispatcher.Instance) {Debug.Log(1);}
        EventDispatcher.Instance.AddEventHandler<MinionDeployEvent>(5, OnMinionDeploy);
    }

    public void OnMinionDeploy(short type, MinionDeployEvent eventData)
    {
        var newMinion = eventData.NewMinion;
        if (!eventData.IsEnemy)
        {
            newMinion.transform.SetParent(playerMinionSpawn);
        }
        else
        {
            newMinion.transform.SetParent(enemyMinionSpawn);
        }
        newMinion.transform.localScale = new Vector3(15, 15, 15);
        newMinion.transform.SetSiblingIndex(eventData.Location);
    }
}