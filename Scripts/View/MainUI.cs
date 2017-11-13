using UnityEngine;
using System.Collections;

public class MainUI : MonoBehaviour
{
    public ItemsPanel_View itemsPanel;
    public LoginPanel_View loginPanel;
    public ControlPanel_View controlPanel;
    public StatusPanel_View statusPanel;
    // Use this for initialization
    void Start()
    {
        AppFacade.GetInstance().Startup(this);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
