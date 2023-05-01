using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITimer : MonoBehaviour
{
    public Text time;
    public Text day;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager.packageTransforms != null)
        {
            float timeRemaining = gameManager.timeLimit - gameManager.GetTime();
            if (timeRemaining < 0.0)
            {
                timeRemaining = 0.0f;
            }
            int mins = Mathf.FloorToInt(timeRemaining / 60.0f);
            int secs = (int)(timeRemaining % 60.0f);

            if (secs / 10.0f < 1.0f)
            {
                time.text = mins + ":0" + secs;
            }
            else
            {
                time.text = mins + ":" + secs;
            }

            day.text = "Day " + gameManager.GetDay();
        }
    }
}
