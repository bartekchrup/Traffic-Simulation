using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItscMarkerManager : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private UIFlowManager flowManager;
    public Intersection Intersection { get; private set; }
    private bool isMarkerClicked;

    void Start() {
        transform.localScale = Settings.INTERSECTION_MARKER_SIZE;
        isMarkerClicked = false;
    }

    public bool HasBeenClicked() {
        return isMarkerClicked;
    }

    public void ResetMarker() {
        isMarkerClicked = false;
    }

    public void SetIntersection(Intersection intersectionIn) {
        Intersection = intersectionIn;
        transform.position = Intersection.GetCentrePosition();
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left) {
            // Toggle marker state to clicked
            isMarkerClicked = true;
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        transform.localScale *= Settings.HOVER_SCALE_FACTOR;
    }

    public void OnPointerExit(PointerEventData eventData) {
        transform.localScale /= Settings.HOVER_SCALE_FACTOR;
    }
}
