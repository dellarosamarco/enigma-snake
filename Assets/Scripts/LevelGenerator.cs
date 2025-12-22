using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject player;
    public GameObject food;
    public GameObject floor;

    int[,] level =
    {
        { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
        { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
        { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
        { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
        { 1, 0, 0, 0, 0, 0, 2, 0, 2, 0, 1 },
        { 1, 0, 0, 0, 0, 0, 0, 2, 0, 0, 1 },
        { 1, 0, 0, 0, 0, 0, 0, 0, 0, 2, 1 },
        { 1, 0, 0, 3, 0, 0, 0, 2, 0, 0, 1 },
        { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
    };

    void Start()
    {
        int[,] _level = GameManager.instance.getLevel("level_01").To2DArray();
        generateLevel(_level);
    }

    void generateLevel(int[,] matrix)
    {
        Vector3 position = Vector3.zero;
        int xOffset = matrix.GetLength(0) / 2 + 1;
        int yOffset = matrix.GetLength(1) / 2 - 1;

        for (int y = 0; y < matrix.GetLength(0); y++)
        {
            for(int x = 0; x < matrix.GetLength(1); x++)
            {
                position.x = x - xOffset;
                position.y = 0 - y + yOffset;

                if (matrix[y, x] == 1)
                {
                    Instantiate(floor, position, Quaternion.identity);
                }
                else if(matrix[y, x] == 2)
                {
                    Instantiate(food, position, Quaternion.identity);
                }
                else if (matrix[y, x] == 3)
                {
                    Instantiate(player, position, Quaternion.identity);
                }
            }
        }
    }

    int[] GetRow(int[,] array, int rowIndex)
    {
        int cols = array.GetLength(1);
        int[] row = new int[cols];

        for (int c = 0; c < cols; c++)
            row[c] = array[rowIndex, c];

        return row;
    }
}
