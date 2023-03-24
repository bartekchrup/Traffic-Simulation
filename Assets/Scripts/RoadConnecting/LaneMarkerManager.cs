using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LaneMarkerManager : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private static readonly Vector3 LANE_MARKER_SIZE = new Vector3(0.2f, 0.2f, 0.2f);
    private const float HOVER_SCALE_FACTOR = 1.4f;

    public LaneNode LaneNode { get; private set; }

    private bool isMarkerClicked;
    

    // Assign the marker to a lane and move it there
    public void SetLaneNode(LaneNode laneNodeIn) {
        LaneNode = laneNodeIn;

        transform.position = LaneNode.lane.centreLine.GetPoint(LaneNode.laneEndIndex);
    }

    void OnEnable() {
        isMarkerClicked = false;
        // Reset marker size
        transform.localScale = LANE_MARKER_SIZE;
    }

    void OnDisable() {
        gameObject.SetActive(false);
    }

    public Line GetLaneLine() {
        return LaneNode.lane.centreLine;
    }

    public Vector2 GetPosition() {
        return transform.position;
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
