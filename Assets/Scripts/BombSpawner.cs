using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BombSpawner : MonoBehaviour
{
    public Tilemap tilemap;

    public GameObject bombPrefab;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cell = tilemap.WorldToCell(worldPosition);
            Vector3 cellCenterPosition = tilemap.GetCellCenterWorld(cell);

            Instantiate(bombPrefab, cellCenterPosition, Quaternion.identity);

        }
        
    }
}
