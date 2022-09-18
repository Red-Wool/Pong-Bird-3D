using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            
        }
        else
        {
            Instance = this;
        }
    }

    public void MiniPause(float initSpeed, float pauseTime){ StartCoroutine(DoPause(initSpeed, pauseTime)); }

    public void TwoStepPause(float spdOne, float pTimeOne, float spdTwo, float pTimeTwo) { StartCoroutine(DoPause(spdOne, pTimeOne, spdTwo, pTimeTwo)); }

    public IEnumerator DoPause(float init, float p)
    {
        Time.timeScale = init;
        yield return new WaitForSecondsRealtime(p);
        Time.timeScale = 1f;
    }

    public IEnumerator DoPause(float spdOne, float pOne, float spdTwo, float pTwo)
    {
        Time.timeScale = spdOne;
        yield return new WaitForSecondsRealtime(pOne);
        Time.timeScale = spdTwo;
        yield return new WaitForSecondsRealtime(pTwo);
        Time.timeScale = 1f;
    }
}
