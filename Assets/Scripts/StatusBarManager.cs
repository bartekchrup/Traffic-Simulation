using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class StatusBarManager : MonoBehaviour
{
    private const string IDLE_TEXT = "Select function to perform";
    private const string DRAWING_TEXT = "Drawing...";
    private const string CONNECTING_ROADS_TEXT = "Connecting Roads...";
    private const string CONNECTING_LANES_ENTRY_TEXT = "Select an entry node to start selection";
    private const string CONNECTING_LANES_EXIT_TEXT = "Select an exit node to make connection, right click to exit";

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

    public void SetTextRoadConnecting() {
        textElement.text = CONNECTING_ROADS_TEXT;
    }

    public void SetTextConnectingEntry() {
        textElement.text = CONNECTING_LANES_ENTRY_TEXT;
    }

    public void SetTextConnectingExit() {
        textElement.text = CONNECTING_LANES_EXIT_TEXT;
    }

    public void SetText(string text) {
        textElement.text = text;
    }
}
