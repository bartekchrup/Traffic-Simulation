using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewRoadDrawManager : MonoBehaviour
{
    [SerializeField] private Settings userSettings;
    
    [SerializeField] TMP_InputField fwdInputField;
    [SerializeField] TMP_InputField revInputField;

    [SerializeField] private float laneWidth;
    bool leftHandDrive;

    private List<RoadSegment> roadSegments = new List<RoadSegment>();
    private int fwdLanesNumber;
    private int revLanesNumber;

    void Awake()
    {
        leftHandDrive = userSettings.LeftHandDrive;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddNewRoad(Vector2[] endPoints) {
        // If the number of lanes on the road selected is invalid, return.
        if (! updateLaneNumberVars()) {
            return;
        }
        roadSegments.Add(new RoadSegment(endPoints, fwdLanesNumber, revLanesNumber, leftHandDrive));
    }

    private bool updateLaneNumberVars() {
        bool fwdValid = int.TryParse(fwdInputField.text, out int fwdVal);
        bool revValid = int.TryParse(revInputField.text, out int revVal);
        fwdLanesNumber = fwdVal;
        revLanesNumber = revVal;
        // True if both numbers are valid
        return (fwdValid && revValid);
    }
}
