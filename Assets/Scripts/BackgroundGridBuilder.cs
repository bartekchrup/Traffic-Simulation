using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BackgroundGridBuilder : MonoBehaviour
{
    [SerializeField] private Tile gridSquare;
    [SerializeField] private Tilemap backgroundGrid;

    public CameraManager cameraManager;

    // Start is called before the first frame update
    void Start()
    {
        Vector3Int middleCellGrid = new Vector3Int(Settings.CanvasWidth / 2, Settings.CanvasHeight / 2, -10);
        Vector3 middleCellWorld = backgroundGrid.GetCellCenterWorld(middleCellGrid);
        middleCellWorld.z = -10; // Camera not inside of grid
        cameraManager.setCameraLocation(middleCellWorld);

        for (int i = 0; i < Settings.CanvasWidth; i++) {
            for (int j = 0; j < Settings.CanvasHeight; j++) {
                Vector3Int coordinate = new Vector3Int(i, j, 0);
                backgroundGrid.SetTile(coordinate, gridSquare);
            }
        }
        
    }

    public void DarkenBackground() {
        backgroundGrid.color = Color.black;
    }

    public void ResetBackground() {
        backgroundGrid.color = Color.white;
    }

}
