using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGraphic : MonoBehaviour
{
    [SerializeField] GraphicSettings _settings;
    // Start is called before the first frame update
    void Start()
    {
        _settings.densityEvent += UpdateTerrainGrassDensity;
        _settings.distanceEvent += UpdateTerrainGrassDistance;

        UpdateTerrainGrassDistance(_settings.grassDistance);
        UpdateTerrainGrassDensity(_settings.grassDensity);
    }

    void UpdateTerrainGrassDensity(float to)
    {
        Terrain.activeTerrain.detailObjectDensity = to;
    }

    void UpdateTerrainGrassDistance(float to)
    {
        Terrain.activeTerrain.detailObjectDistance = to;
    }
}
