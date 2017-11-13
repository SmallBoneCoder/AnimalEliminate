using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class LoginPanel_View : MonoBehaviour
{
    public Button BtnStart;
    public System.Action StartHandle;
    // Use this for initialization
    void Start()
    {
        
    }
    private void Click_BtnStart()
    {
        if (StartHandle != null)
        {
            BtnStart.interactable = false;
            StartHandle.Invoke();
        }
    }
    public void HideLoginPanel()
    {
        gameObject.SetActive(false);
    }
    internal void Init()
    {
        BtnStart.onClick.AddListener(Click_BtnStart);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
