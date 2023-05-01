using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct PackageInfo
{
    public Vector3 position;
    public Quaternion rotation;
    public bool isCargo;
    public bool isDelivered;

    public PackageInfo(Vector3 pos, Quaternion rot, bool cargo)
    {
        position = pos;
        rotation = rot;
        isCargo = cargo;
        isDelivered = false;
    }
}

public struct BuildingInfo
{
    public string currentLocation;
    public Vector2 cachedMapLocation;
    public Sprite buildingSprite;
    public string colourCode;
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public float timeLimit;
    private float timer = 0.0f;
    private int musicVolumeIndex = -1;
    private int sfxVolumeIndex = -1;
    private int screenSizeIndex = -1;
    private int fullScreenIndex = -1;
    private int currentVanSceneIndex = 0;
    private int[] vanScenes = { 4, 5, 6, 7, 8 };

    public Dictionary<string, PackageInfo> packageTransforms;
    public BuildingInfo buildingInfo;

    void Awake()
    {
        // Singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);

        currentVanSceneIndex = 0;
        packageTransforms = new Dictionary<string, PackageInfo>();
        buildingInfo = new BuildingInfo();
        buildingInfo.currentLocation = "";
        buildingInfo.cachedMapLocation = new Vector2(134.0f, 85.0f);
        buildingInfo.buildingSprite = null;
    }

    public void Update()
    {
        if (timeLimit > 0.0)
        {
            timer += Time.deltaTime;
            if (timer > timeLimit)
            {
                LoseLevel();
            }
        }
    }

    public void StartPause(float pauseTime)
    {
        StartCoroutine(PauseGame(pauseTime));
    }

    public IEnumerator PauseGame(float pauseTime)
    {
        Time.timeScale = 0.0f;
        float pauseEndTime = Time.realtimeSinceStartup + pauseTime;
        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            yield return 0;
        }
        Time.timeScale = 1.0f;
    }

    public int GetMusicVolumeIndex()
    {
        return musicVolumeIndex;
    }

    public void SetMusicVolumeIndex(int value)
    {
        musicVolumeIndex = value;
    }

    public int GetSFXVolumeIndex()
    {
        return sfxVolumeIndex;
    }

    public void SetSFXVolumeIndex(int value)
    {
        sfxVolumeIndex = value;
    }

    public int GetScreenSizeIndex()
    {
        return screenSizeIndex;
    }

    public void SetScreenSizeIndex(int value)
    {
        screenSizeIndex = value;
    }

    public int GetFullScreenIndex()
    {
        return fullScreenIndex;
    }

    public void SetFullScreenIndex(int value)
    {
        fullScreenIndex = value;
    }

    public void ResetGame()
    {
        timer = 0.0f;
        packageTransforms = new Dictionary<string, PackageInfo>();
        buildingInfo = new BuildingInfo();
        buildingInfo.currentLocation = "";
        buildingInfo.cachedMapLocation = new Vector2(134.0f, 85.0f);
        buildingInfo.buildingSprite = null;
        buildingInfo.colourCode = "";

        SceneManager.LoadScene(vanScenes[currentVanSceneIndex]);
    }

    public void LoseLevel()
    {
        ResetGame();
        SceneManager.LoadScene(2);
    }

    public void WinLevel()
    {
        currentVanSceneIndex += 1;
        if (currentVanSceneIndex >= vanScenes.Length)
        {
            currentVanSceneIndex = 0;
            ResetGame();
            SceneManager.LoadScene(9);
        }
        else
        {
            ResetGame();
            SceneManager.LoadScene(3);
        }
    }

    public void RestProgress()
    {
        currentVanSceneIndex = 0;
        SceneManager.LoadScene(0);
    }

    public int GetCurrentVanScene()
    {
        return vanScenes[currentVanSceneIndex];
    }

    public int GetDelivered()
    {
        int deliveredCount = 0;

        foreach (KeyValuePair<string, PackageInfo> pair in packageTransforms)
        {
            if (pair.Value.isDelivered)
            {
                deliveredCount += 1;
            }
        }

        return deliveredCount;
    }

    public float GetTime()
    {
        return timer;
    }

    public int GetDay()
    {
        return currentVanSceneIndex+1;
    }

    public bool IsValid()
    {
        return SceneManager.GetActiveScene().buildIndex > 0;
    }
}

