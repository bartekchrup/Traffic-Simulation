using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LaneMarkerManager : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private const float HOVER_SCALE_FACTOR = 1.4f;
    [SerializeField] private ConnectLanes connectLanesScript;

    private LaneSegment lane;
    // The end of the lane on which the marker is on. 0 is first point
    private int laneEndIndex;

    // Assign the marker to a lane and move it there
    public void SetLane(LaneSegment laneIn, int laneEndIndexIn) {
        lane = laneIn;
        laneEndIndex = laneEndIndexIn;
        transform.position = lane.centreLine.GetPoint(laneEndIndex);
    }

    public void SetColor(Color color) {
        this.gameObject.GetComponent<SpriteRenderer>().color = color;
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left) {
            connectLanesScript.MarkerClicked(this);
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        transform.localScale *= HOVER_SCALE_FACTOR;
    }

    public void OnPointerExit(PointerEventData eventData) {
        transform.localScale /= HOVER_SCALE_FACTOR;
    }

    public void Destroy() {
        Destroy(this.gameObject);
    }

}
