using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    public const int Red = 0;
    public const int RedToGreen = 1;
    public const int Green = 2;
    public const int GreenToRed = 3;

    // Lane node associated with traffic light
    public LaneNode node { get; private set; }
    
    // Displays the lane number
    [SerializeField] private TMP_Text laneTextField;
    // To change the sprite when that state changes
    [SerializeField] private SpriteRenderer lightSprite;

    // State sprites
    [SerializeField] private Sprite redSprite;
    [SerializeField] private Sprite redToGreenSprite;
    [SerializeField] private Sprite greenSprite;
    [SerializeField] private Sprite greenToRedSprite;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNode(LaneNode nodeIn) {
        node = nodeIn;
    }

    public void SetLane(int laneNumber) {
        laneTextField.SetText("Lane " + (laneNumber + 1));
        // +1 because index starts at zero
    }

    public void SetState(int state) {
        if (state == TrafficLight.Red) {
            lightSprite.sprite = redSprite;
        } else if (state == TrafficLight.RedToGreen) {
            lightSprite.sprite = redToGreenSprite;
        } else if (state == TrafficLight.Green) {
            lightSprite.sprite = greenSprite;
        } else {
            lightSprite.sprite = greenToRedSprite;
        }
    }

}
