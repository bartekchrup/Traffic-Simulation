using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLightManager : MonoBehaviour
{
    // This UI element is hidden while editing the lights scheme
    [SerializeField] GameObject leftUIPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Called after enabling script, shows all light scheme UI
    public void EnableEditTrafficLights(Intersection intersection) {
        leftUIPanel.SetActive(false);
        foreach (RoadNode node in intersection.GetNodes()) {
            
        }
    }

    void OnDisable() {
        leftUIPanel.SetActive(true);
    }
}
