using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BackgroundGridBuilder : MonoBehaviour
{
    [SerializeField] public Tile gridSquare;
    [SerializeField] public Tilemap backgroundGrid;
    [SerializeField] public int gridWidth;
    [SerializeField] public int gridHeight;
    public CameraManager cameraManager;

    // Start is called before the first frame update
    void Start()
    {
        Vector3Int middleCellGrid = new Vector3Int(gridWidth / 2, gridHeight / 2, -10);
        Vector3 middleCellWorld = backgroundGrid.GetCellCenterWorld(middleCellGrid);
        middleCellWorld.z = -10; // Camera not inside of grid
        cameraManager.moveCamera(middleCellWorld);

        for (int i = 0; i < gridWidth; i++) {
            for (int j = 0; j < gridHeight; j++) {
                Vector3Int coordinate = new Vector3Int(i, j, 0);
                backgroundGrid.SetTile(coordinate, gridSquare);
                Debug.Log("Added square to" + coordinate);
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
