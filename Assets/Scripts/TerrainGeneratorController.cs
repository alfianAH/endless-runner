using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TerrainGeneratorController : MonoBehaviour
{
    [Header("Templates")] 
    public List<TerrainTemplateController> terrainTemplates;
    public float terrainTemplateWidth;

    [Header("Generator Area")] 
    public Camera gameCamera;
    public float areaStartOffset,
        areaEndOffset;

    [Header("Force Early Template")] 
    public List<TerrainTemplateController> earlyTerrainTemplates;

    private Dictionary<string, List<GameObject>> pool;
    private List<GameObject> spawnedTerrain;
    private float lastGeneratedPositionX;
    private float lastRemovedPositionX;
    
    private const float DebugLineHeight = 10.0f;

    private float GetHorizontalPositionStart()
    {
        return gameCamera.ViewportToWorldPoint(new Vector3(0f, 0f)).x + areaStartOffset;
    }
    
    private float GetHorizontalPositionEnd()
    {
        return gameCamera.ViewportToWorldPoint(new Vector3(1f, 0f)).x + areaEndOffset;
    }
    
    private void Start()
    {
        pool = new Dictionary<string, List<GameObject>>();
        spawnedTerrain = new List<GameObject>();
        
        lastGeneratedPositionX = GetHorizontalPositionStart();
        lastRemovedPositionX = lastGeneratedPositionX - terrainTemplateWidth;

        foreach (TerrainTemplateController terrain in earlyTerrainTemplates)
        {
            GenerateTerrain(lastGeneratedPositionX, terrain);
            lastGeneratedPositionX += terrainTemplateWidth;
        }
        
        while (lastGeneratedPositionX < GetHorizontalPositionEnd())
        {
            GenerateTerrain(lastGeneratedPositionX);
            lastGeneratedPositionX += terrainTemplateWidth;
        }
    }

    private void Update()
    {
        while (lastGeneratedPositionX < GetHorizontalPositionEnd())
        {
            GenerateTerrain(lastGeneratedPositionX);
            lastGeneratedPositionX += terrainTemplateWidth;
        }

        while (lastRemovedPositionX + terrainTemplateWidth < GetHorizontalPositionStart())
        {
            lastRemovedPositionX += terrainTemplateWidth;
            RemoveTerrain(lastRemovedPositionX);
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 areaStartPosition = transform.position;
        Vector3 areaEndPosition = transform.position;

        areaStartPosition.x = GetHorizontalPositionStart();
        areaEndPosition.x = GetHorizontalPositionEnd();
        
        Debug.DrawLine(areaStartPosition + Vector3.up*DebugLineHeight/2,
            areaStartPosition + Vector3.down*DebugLineHeight/2, 
            Color.red);
        
        Debug.DrawLine(areaEndPosition + Vector3.up*DebugLineHeight/2,
            areaEndPosition + Vector3.down*DebugLineHeight/2, 
            Color.red);
    }

    private void GenerateTerrain(float posX, TerrainTemplateController forceTerrain = null)
    {
        int randomTerrain = Random.Range(0, terrainTemplates.Count);

        GameObject newTerrain = GenerateFromPool(forceTerrain == null 
            ? terrainTemplates[randomTerrain].gameObject 
            : forceTerrain.gameObject, 
            transform);

        newTerrain.transform.position = new Vector3(posX, -4.5f);
        
        spawnedTerrain.Add(newTerrain);
    }

    private void RemoveTerrain(float posX)
    {
        GameObject terrainToRemove = null;
        
        // Find terrain at posX
        foreach (GameObject item in spawnedTerrain)
        {
            if (item.transform.position.x <= posX)
            {
                terrainToRemove = item;
                break;
            }
        }

        if (terrainToRemove)
        {
            spawnedTerrain.Remove(terrainToRemove);
            ReturnToPool(terrainToRemove);
        }
    }
    
    private GameObject GenerateFromPool(GameObject item, Transform parent)
    {
        // If item is available in pool, ...
        if (pool.ContainsKey(item.name))
        {
            if (pool[item.name].Count > 0)
            {
                GameObject newItemFromPool = pool[item.name][0];
                pool[item.name].Remove(newItemFromPool);
                newItemFromPool.SetActive(true);
                return newItemFromPool;
            }
        }
        else // If item list not defined, create new one
        {
            pool.Add(item.name, new List<GameObject>());
        }
        
        // Create new one if no item available in pool
        GameObject newItem = Instantiate(item, parent);
        newItem.name = item.name;

        return newItem;
    }

    private void ReturnToPool(GameObject item)
    {
        if (!pool.ContainsKey(item.name))
        {
            Debug.LogError("Invalid Pool Item!!");
        }
        
        pool[item.name].Add(item);
        item.SetActive(false);
    }
}
