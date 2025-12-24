using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class LevelData
{
    public string id;
    public int[][] grid;

    public int[,] To2DArray()
    {
        int h = this.grid.Length;
        int w = this.grid[0].Length;

        var result = new int[h, w];

        for (int y = 0; y < h; y++)
            for (int x = 0; x < w; x++)
                result[y, x] = this.grid[y][x];

        return result;
    }
}

[System.Serializable]
public class LevelDatabase
{
    public List<LevelData> levels;
}
