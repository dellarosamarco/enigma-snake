using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject player;
    public GameObject food;
    public GameObject floor;
    public GameObject portal;

    void Start()
    {
        string levelId = "level_" + GameManager.instance.currentLevel.ToString();
        int[,] level = GameManager.instance.getLevel(levelId).To2DArray();
        generateLevel(level);
    }

    void generateLevel(int[,] matrix)
    {
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(
            new Vector3(0, 0, Camera.main.nearClipPlane)
        );
        bottomLeft.z = 0;

        float cellSize = 1f;
        int height = matrix.GetLength(0);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < matrix.GetLength(1); x++)
            {
                Vector3 position = new Vector3(
                    bottomLeft.x + x * cellSize + (cellSize / 2),
                    bottomLeft.y + (height - 1 - y) * cellSize + (cellSize / 2),
                    0
                );

                position.x = Mathf.Floor(position.x);
                position.y = Mathf.Floor(position.y);

                switch (matrix[y, x])
                {
                    case 1: Instantiate(floor, position, Quaternion.identity); break;
                    case 2: Instantiate(food, position, Quaternion.identity); break;
                    case 3: Instantiate(player, position, Quaternion.identity); break;
                    case 4: Instantiate(portal, position, Quaternion.identity); break;
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
