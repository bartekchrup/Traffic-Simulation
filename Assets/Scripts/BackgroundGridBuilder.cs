using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BackgroundGridBuilder : MonoBehaviour
{
    [SerializeField] private Tile gridSquare;
    [SerializeField] private Tilemap backgroundGrid;
    [SerializeField] private Settings userSettings;

    private int gridWidth;
    private int gridHeight;
    public CameraManager cameraManager;

    // Retrieve size of canvas from user settings
    void Awake()
    {
        gridWidth = userSettings.CanvasWidth;
        gridHeight = userSettings.CanvasHeight;
    }

    // Start is called before the first frame update
    void Start()
    {
        Vector3Int middleCellGrid = new Vector3Int(gridWidth / 2, gridHeight / 2, -10);
        Vector3 middleCellWorld = backgroundGrid.GetCellCenterWorld(middleCellGrid);
        middleCellWorld.z = -10; // Camera not inside of grid
        cameraManager.setCameraLocation(middleCellWorld);

        for (int i = 0; i < gridWidth; i++) {
            for (int j = 0; j < gridHeight; j++) {
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
