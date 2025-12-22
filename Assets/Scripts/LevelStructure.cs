using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class LevelData
{
    public string id;
    public int width;
    public int height;
    public int[] cells;

    public int[,] To2DArray()
    {
        var grid = new int[this.height, this.width];

        for (int y = 0; y < this.height; y++)
        {
            for (int x = 0; x < this.width; x++)
            {
                grid[y, x] = cells[y * this.width + x];
            }
        }

        return grid;
    }
}

[System.Serializable]
public class LevelDatabase
{
    public List<LevelData> levels;
}
