using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapDestroyer : MonoBehaviour
{

    public Tilemap tilemap;

    public Tile parede;
    public Tile pedra;
    public Tile grey;

    public GameObject explosion;

    public void Explode(Vector2 worldPos) {
        Vector3Int originCell = tilemap.WorldToCell(worldPos);

        ExplodeCell(originCell);
        if(ExplodeCell(originCell+ new Vector3Int(1, 0, 0))) {
            ExplodeCell(originCell + new Vector3Int(2, 0, 0));
        }
        if (ExplodeCell(originCell + new Vector3Int(-1, 0, 0))) {
            ExplodeCell(originCell + new Vector3Int(-2, 0, 0));
        }
        if (ExplodeCell(originCell + new Vector3Int(0, 1, 0))) {
            ExplodeCell(originCell + new Vector3Int(0, 2, 0));
        }
        if (ExplodeCell(originCell + new Vector3Int(0, -1, 0))) {
            ExplodeCell(originCell + new Vector3Int(0, -2, 0));
        }

    }

    bool ExplodeCell(Vector3Int cell) {
        Tile tile = tilemap.GetTile<Tile>(cell);

        if(tile == parede || tile == grey) {
            return false; 
        }

        if(tile == pedra) {
            tilemap.SetTile(cell, null);
            Vector3 pos2 = tilemap.GetCellCenterWorld(cell);
            Instantiate(explosion, pos2, Quaternion.identity);
            return false;    
        }
        Vector3 pos = tilemap.GetCellCenterWorld(cell);
        Instantiate(explosion, pos, Quaternion.identity);
        return true;
        
    }
}
