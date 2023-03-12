using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectButtonCustodian : MonoBehaviour
{
    [SerializeField] ConnectRoadSegments connectionScript;

    // Toggles state of connecting mode script
    public void ConnectButtonPressed() {
        connectionScript.enabled = !connectionScript.enabled;
    }
}
