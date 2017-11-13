using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ControlPanel_View : MonoBehaviour
{
    public Button BtnReset;
    public Button BtnQuit;
    public System.Action ResetHandle;
    public System.Action QuitHandle;
    // Use this for initialization
    void Start()
    {
        
    }

    public void Init()
    {
        BtnQuit.onClick.AddListener(Click_BtnQuit);
        BtnReset.onClick.AddListener(Click_BtnReset);
    }

    private void Click_BtnReset()
    {
        if (ResetHandle != null)
        {
            ResetHandle.Invoke();
        }
    }
    private void Click_BtnQuit()
    {
        if (QuitHandle != null)
        {
            QuitHandle.Invoke();
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
