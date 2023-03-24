using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RoadEndMarkerManager : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private const float HOVER_SCALE_FACTOR = 1.4f;
    public RoadNode roadNode { get; private set; }
    private bool isSelected = false;

    public void SetRoad(RoadSegment road, int roadEndIndex) {
        roadNode = road.GetRoadNode(roadEndIndex);
    }

    public bool IsSelected() {
        return isSelected;
    }

    // Toggles the selected state of the road end marker
    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left) {
            isSelected = !isSelected;
            if (isSelected) {
                this.GetComponentInChildren<SpriteRenderer>().color = Color.green;
            } else {
                this.GetComponentInChildren<SpriteRenderer>().color = Color.red;
            }
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
