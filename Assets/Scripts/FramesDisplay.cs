using UnityEngine;
using TMPro;
using Unity.Mathematics;

public class FramesDisplay : MonoBehaviour
{

    public TMP_Text FramesText;
    float time;
    float updateRate = 1f;
    int frameCount;

    void Update()
    {
        time += Time.deltaTime;

        frameCount++;

        if (time >= updateRate)
        {
            int frameRate = Mathf.RoundToInt(frameCount/time);
            FramesText.text = frameRate.ToString() + " FPS";

            time-=updateRate;
            frameCount=0;
        }   
    }
}