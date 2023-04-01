using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    // STATIC
    // Used to assign each light a unique number of identify them
    private static int usedLightIndexTracker = 0;
    private static int GetNextLightIndex() {
        int indexToReturn = usedLightIndexTracker;
        usedLightIndexTracker = (usedLightIndexTracker + 1) % 26;
        return indexToReturn++;
    }
    // Converts an int index to a displayable char
    public static char IndexToChar(int indexIn) {
        return (char)(indexIn+65);
    }

    public enum LightState {
        Red = 0,
        RedToGreen = 1,
        Green = 2,
        GreenToRed = 3}

    // State sprites
    [SerializeField] private Sprite redSprite;
    [SerializeField] private Sprite redToGreenSprite;
    [SerializeField] private Sprite greenSprite;
    [SerializeField] private Sprite greenToRedSprite;


    // INSTANCE VARIABLES
    // Displays the lane number
    [SerializeField] private TMP_Text laneTextField;
    // To change the sprite when that state changes
    [SerializeField] private SpriteRenderer lightSprite;
    // Lane node associated with traffic light
    public LaneNode node { get; private set; }
    // Index used to display letter above light and identify it in scheme edit panel
    public int Index { get; private set; }


    public void InstantiateLight(LaneNode nodeIn) {
        node = nodeIn;
        node.SetTrafficLight(this);
        setIndex();
    }

    public void SetState(LightState state) {
        if (state == LightState.Red) {
            lightSprite.sprite = redSprite;
        } else if (state == LightState.RedToGreen) {
            lightSprite.sprite = redToGreenSprite;
        } else if (state == LightState.Green) {
            lightSprite.sprite = greenSprite;
        } else if (state == LightState.GreenToRed) {
            lightSprite.sprite = greenToRedSprite;
        } else {
            Debug.LogError("Invalid light state");
        }
    }

    // Gets a new index and sets the display to the associated char, lights are A, B, C...
    private void setIndex() {
        Index = GetNextLightIndex();
        laneTextField.SetText("" + IndexToChar(Index));
    }

}
