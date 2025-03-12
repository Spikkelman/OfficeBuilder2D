using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class TileManager : MonoBehaviour
{
    // Reference to the Tilemap in the scene
    public Tilemap tilemap;

    // The currently selected tile (set via the UI)
    public TileBase selectedTile;

    // Called by TileButton scripts to update the selection
    public void SetSelectedTile(TileBase tile)
    {
        selectedTile = tile;
        Debug.Log("Selected tile: " + tile.name);
    }

    void Update()
    {
        // Ignore clicks when the pointer is over a UI element
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        // Place tile on left mouse click if a tile is selected
        if (Input.GetMouseButtonDown(0) && selectedTile != null)
        {
            // Convert mouse position to world coordinates
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Convert world position to cell position on the Tilemap grid
            Vector3Int cellPosition = tilemap.WorldToCell(mouseWorldPos);
            // Place the tile on the Tilemap
            tilemap.SetTile(cellPosition, selectedTile);
        }
    }
}