using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LaneMarkerManager : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private const float HOVER_SCALE_FACTOR = 1.4f;

    private LaneSegment lane;
    // The end of the lane on which the marker is on. 0 is first point
    private int laneEndIndex;
    private bool isMarkerClicked;

    // Assign the marker to a lane and move it there
    public void SetLane(LaneSegment laneIn, int laneEndIndexIn) {
        lane = laneIn;
        laneEndIndex = laneEndIndexIn;
        transform.position = lane.centreLine.GetPoint(laneEndIndex);
    }

    void OnEnable() {
        isMarkerClicked = false;
    }

    void OnDisable() {
        gameObject.SetActive(false);
    }

    public Line GetLaneLine() {
        return lane.centreLine;
    }

    public void SetColor(Color color) {
        GetComponent<SpriteRenderer>().color = color;
    }

    public Color GetColor() {
        return GetComponent<SpriteRenderer>().color;
    }

    public void ResetMarker() {
        isMarkerClicked = false;
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left) {
            isMarkerClicked = true;
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        transform.localScale *= HOVER_SCALE_FACTOR;
    }

    public void OnPointerExit(PointerEventData eventData) {
        transform.localScale /= HOVER_SCALE_FACTOR;
    }

    public bool HasBeenClicked() {
        return isMarkerClicked;
    }

    public void Destroy() {
        Destroy(this.gameObject);
    }

}
