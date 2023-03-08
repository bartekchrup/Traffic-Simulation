using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class StatusBarManager : MonoBehaviour
{
    private const string IDLE_TEXT = "Select function to perform";
    private const string DRAWING_TEXT = "Drawing...";
    private const string CONNECTING_TEXT = "Connecting Roads...";

    private TMP_Text textElement;

    public void Awake() {
        textElement = GetComponent<TMP_Text>();
        textElement.text = IDLE_TEXT;
    }

    public void SetTextIdle() {
        textElement.text = IDLE_TEXT;
    }

    public void SetTextDrawing() {
        textElement.text = DRAWING_TEXT;
    }

    public void SetTextConnecting() {
        textElement.text = CONNECTING_TEXT;
    }
}
