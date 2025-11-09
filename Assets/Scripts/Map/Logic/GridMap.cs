using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class GridMap : MonoBehaviour
{
    public MapData_SO mapData;
    public Tilemap currentTilemap;
    private GridType gridType;

    private void OnEnable()
    {
        if (!Application.IsPlaying(this))
        {
            currentTilemap = GetComponent<Tilemap>();
#if UNITY_EDITOR
            if (mapData != null)
                mapData.tileProperties.Clear();
#endif
        }
    }

    private void OnDisable()
    {
        if (!Application.IsPlaying(this))
        {
            currentTilemap = GetComponent<Tilemap>();

            UpdateTileProperties();

            if(mapData != null)
                EditorUtility.SetDirty(mapData);
        }
    }
    
    private void UpdateTileProperties()
    {
        currentTilemap.CompressBounds();
        
        if(!Application.IsPlaying(this))
        {
            if(mapData != null)
            {
                //已经绘制范围的左下角
                Vector3Int startPos = currentTilemap.cellBounds.min;
                //已经绘制范围的右上角
                Vector3Int endPos = currentTilemap.cellBounds.max;

                for (int x = startPos.x; x < endPos.x; x++)
                {
                    for (int y = startPos.y; y< endPos.y; y++)
                    {
                        TileBase tile = currentTilemap.GetTile(new Vector3Int(x, y, 0));
                     
                        if(tile != null)
                        {
                            TileProperty newTile = new TileProperty
                            {
                                tileCoordinate = new Vector2Int(x, y),
                                gridType = gridType,
                                boolTypeValue = true
                            };
                            mapData.tileProperties.Add(newTile);
                        }
                    }
                }
            }
        }
    }
}
