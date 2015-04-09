using UnityEngine;
using System.Collections;

public class RotateTerrain : MonoBehaviour {
	
	void Start () {
		
		Terrain terrain = GetComponent<Terrain>();
		
		// Get a reference to the terrain
		TerrainData terrainData = terrain.terrainData;
		
		// Populate an array with current height data
		float[,] orgHeightData = terrainData.GetHeights(0,0,terrainData.heightmapWidth, terrainData.heightmapHeight);
		
		// Initialise a new array of same size to hold rotated data
		float[,] mirroredHeightData = new float[terrainData.heightmapWidth, terrainData.heightmapHeight];
		
		for (int y = 0; y < terrainData.heightmapHeight; y++)
		{
			for (int x = 0; x < terrainData.heightmapWidth; x++)
			{
				
				// Rotate each element clockwise
				//mirroredHeightData[y,x] = orgHeightData[terrainData.heightmapHeight - y - 1, x];
				mirroredHeightData[y,x] = orgHeightData[y, terrainData.heightmapWidth - x - 1];
			}
		}
		
		// Finally assign the new heightmap to the terrainData:
		terrainData.SetHeights(0, 0, mirroredHeightData);
	}
}