using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject bubblePrefab; // Bubble prefab
    public int rows = 5; // Number of rows
    public int columns = 5; // Number of columns
    public float spacing = 1.2f; // Space between bubbles

    private void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Vector3 position = new Vector3(col * spacing, row * -spacing, 0); // Position of each bubble
                GameObject bubble = Instantiate(bubblePrefab, position, Quaternion.identity, transform);
                bubble.name = $"Bubble_{row}_{col}";
            }
        }
    }
}

