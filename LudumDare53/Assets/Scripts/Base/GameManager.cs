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
    private int musicVolumeIndex = -1;
    private int sfxVolumeIndex = -1;
    private int screenSizeIndex = -1;
    private int fullScreenIndex = -1;

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

        packageTransforms = new Dictionary<string, PackageInfo>();
        buildingInfo = new BuildingInfo();
        buildingInfo.currentLocation = "";
        buildingInfo.cachedMapLocation = new Vector2(134.0f, 85.0f);
        buildingInfo.buildingSprite = null;
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
        packageTransforms = new Dictionary<string, PackageInfo>();
        buildingInfo = new BuildingInfo();
        buildingInfo.currentLocation = "";
        buildingInfo.cachedMapLocation = new Vector2(134.0f, 85.0f);
        buildingInfo.buildingSprite = null;
        buildingInfo.colourCode = "";

        SceneManager.LoadScene(0);
    }
}

