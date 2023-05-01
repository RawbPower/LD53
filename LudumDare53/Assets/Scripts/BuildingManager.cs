using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BuildingManager
{
    static public void GoToBuilding(string buildingName)
    {
        GlobalAudioManager audioManager = GlobalAudioManager.instance;
        if (audioManager.IsPlaying("Drive"))
        {
            audioManager.Stop("Drive");
        }
        SceneManager.LoadScene(GameManager.instance.GetCurrentVanScene());
    }
}
