using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGraphic : MonoBehaviour
{
    Terrain terrain;
    [SerializeField] GraphicSettings _settings;
    // Start is called before the first frame update
    void Start()
    {
        terrain = GetComponent<Terrain>();

        _settings.densityEvent += UpdateTerrainGrassDensity;
        _settings.distanceEvent += UpdateTerrainGrassDistance;

        UpdateTerrainGrassDistance(_settings.grassDistance);
        UpdateTerrainGrassDensity(_settings.grassDensity);
    }

    void UpdateTerrainGrassDensity(float to)
    {
        if (terrain)
        {
            terrain.detailObjectDensity = to;
        }
        else
        {
            TryGetComponent<Terrain>(out terrain);
        }

    }

    void UpdateTerrainGrassDistance(float to)
    {
        if (terrain)
        {
            terrain.detailObjectDistance = to;
        }
        else
        {
            TryGetComponent<Terrain>(out terrain);
        }
        
    }

    private void OnDestroy()
    {
        _settings.densityEvent -= UpdateTerrainGrassDensity;
        _settings.distanceEvent -= UpdateTerrainGrassDistance;
    }
}
