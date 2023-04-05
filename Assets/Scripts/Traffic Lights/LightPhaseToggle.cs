using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightPhaseToggle : MonoBehaviour
{
    [SerializeField] private Toggle toggle;
    public bool State { get; private set; }

    public void stateChanged() {
        State = toggle.isOn;
    }

    public void SetState(bool state) {
        toggle.isOn = state;
    }
}
