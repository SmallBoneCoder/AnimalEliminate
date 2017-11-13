using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class StatusPanel_View : MonoBehaviour
{
    public Text Score_Txt;
    public Text Step_Txt;
    // Use this for initialization
    void Start()
    {

    }
    public void UpdateScore(int score)
    {
        Score_Txt.text = score.ToString();
    }

    public void Init()
    {
        Score_Txt.text = "0";
        Step_Txt.text = "50";
    }

    public void UpdateStep(int step)
    {
        Step_Txt.text = step.ToString();
        Debug.Log("Show Step");
    }
    // Update is called once per frame
    void Update()
    {

    }
}
