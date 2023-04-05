using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class NewRoadPanelManager : MonoBehaviour
{
    // The 2 input fields in the panel
    [SerializeField] TMP_InputField fwdInputField;
    [SerializeField] TMP_InputField revInputField;

    public void drawButtonPressed() {
        bool fwdValid = int.TryParse(fwdInputField.text, out int fwdValue);
        bool revValid = int.TryParse(revInputField.text, out int revValue);
        if (fwdValid && revValid) {
            Debug.Log("fwd: " + fwdValue + " rev: " + revValue);
        } else {
            Debug.Log("The number of lanes must be an integer.");
        }
        
    }
}
