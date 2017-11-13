using UnityEngine;
using System.Collections;
using PureMVC.Patterns;
using PureMVC.Interfaces;
using System.Collections.Generic;

public class ControlPanel_Mediator : Mediator
{
    private ControlPanel_View view
    {
        get
        {
            return (ControlPanel_View)ViewComponent;
        }
    }
    public new const string NAME = "ControlPanel_Mediator";

    public ControlPanel_Mediator(ControlPanel_View controlPanel) : base(NAME, controlPanel)
    {
        controlPanel.Init();
        controlPanel.QuitHandle += HandleQuit;
        controlPanel.ResetHandle += HandleReset;
    }
    private void HandleQuit()
    {
        SendNotification(CommandConst.CMD_QuitGame);
    }
    private void HandleReset()
    {
        SendNotification(CommandConst.CMD_ResetGame);
    }
    public override void HandleNotification(INotification notification)
    {
        
    }
    public override IList<string> ListNotificationInterests()
    {
        return base.ListNotificationInterests();
    }
    public override void OnRegister()
    {
        base.OnRegister();
    }
}
