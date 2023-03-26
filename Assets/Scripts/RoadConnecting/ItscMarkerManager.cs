using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItscMarkerManager : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Intersection intersection;

    void Start() {
        transform.localScale = Settings.INTERSECTION_MARKER_SIZE;
    }

    public void SetIntersection(Intersection intersectionIn) {
        intersection = intersectionIn;
        transform.position = intersection.GetMiddlePosition();
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left) {
            // EDIT INTERSECTION LANE CONNECTIONS

        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        transform.localScale *= Settings.HOVER_SCALE_FACTOR;
    }

    public void OnPointerExit(PointerEventData eventData) {
        transform.localScale /= Settings.HOVER_SCALE_FACTOR;
    }
}
