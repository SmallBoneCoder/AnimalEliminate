using UnityEngine;
using System.Collections;
using PureMVC.Patterns;
using PureMVC.Interfaces;
using System.Collections.Generic;

public class LoginPanel_Mediator : Mediator
{
    private LoginPanel_View view
    {
        get
        {
            return (LoginPanel_View)ViewComponent;
        }
    }
    public new const string NAME = "LoginPanel_Mediator";

    public LoginPanel_Mediator(LoginPanel_View loginPanel) : base(NAME, loginPanel)
    {
        loginPanel.Init();
        loginPanel.StartHandle += HandleStart;
    }
    public override IList<string> ListNotificationInterests()
    {
        List<string> list = new List<string>();
        list.Add(ViewConst.HideLoginUI);
        return list;
    }
    private void HandleStart()
    {
        SendNotification(CommandConst.CMD_StartGame);
    }
    public override void OnRegister()
    {
        
    }
    public override void HandleNotification(INotification notification)
    {
        switch (notification.Name)
        {
            case ViewConst.HideLoginUI:
                {
                    view.HideLoginPanel();
                }break;
        }
    }

}
