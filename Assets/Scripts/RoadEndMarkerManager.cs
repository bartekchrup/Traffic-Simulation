using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RoadEndMarkerManager : MonoBehaviour, IPointerClickHandler
{
    private RoadSegment road;
    // for identifying which end of the road, 0 is the start of the road, 1 is the end of the road
    private int roadEndIndex;
    private bool isSelected = false;

    public void SetRoad(RoadSegment roadIn, int roadEndIndexIn) {
        road = roadIn;
        roadEndIndex = roadEndIndexIn;
    }

    public RoadSegment GetRoad() {
        return road;
    }

    public int GetRoadEndIndex() {
        return roadEndIndex;
    }

    public bool IsSelected() {
        return isSelected;
    }

    // Toggles the selected state of the road end marker
    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left) {
            isSelected = !isSelected;
            Debug.Log("This marker is now " + (isSelected? "selected" : "unselected"));
            if (isSelected) {
                this.GetComponentInChildren<SpriteRenderer>().color = Color.green;
            } else {
                this.GetComponentInChildren<SpriteRenderer>().color = Color.red;
            }
        }
    }

    public void Destroy() {
        Destroy(this.gameObject);
    }

}
