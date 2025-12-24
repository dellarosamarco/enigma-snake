using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public LevelDatabase db;
    public int currentLevel = 1;

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
        db = JsonConvert.DeserializeObject<LevelDatabase>(levelsJson.text);
    }

    void Start()
    {
        startLevel();
    }

    public LevelData getLevel(string id)
    {
        return db.levels.Find(l => l.id == id);
    }

    public void startLevel()
    {
        SceneManager.LoadScene("Game");
    }

    public void nextLevel()
    {
        currentLevel++;
        SceneManager.LoadScene("Game");
    }
}
