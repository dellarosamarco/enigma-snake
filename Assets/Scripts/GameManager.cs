using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public LevelDatabase db;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        TextAsset levelsJson = Resources.Load<TextAsset>("levels");
        db = JsonUtility.FromJson<LevelDatabase>(levelsJson.text);
    }

    void Start()
    {
        SceneManager.LoadScene("Game");
    }

    public LevelData getLevel(string id)
    {
        return db.levels.Find(l => l.id == id);
    }
}
