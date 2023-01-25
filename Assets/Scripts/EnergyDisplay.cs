using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnergyDisplay : MonoBehaviour
{
    public TMP_Text energyDisplay;

    // Start is called before the first frame update
    void Start()
    {
        EventDispatcher.Instance.AddEventHandler<EnergyChangeEvent>(8, OnEnergyChange);
    }

    void OnEnergyChange(short type, EnergyChangeEvent eventData)
    {
        energyDisplay.text = "Energy: " + eventData.Energy.ToString();
    }
}
