using UnityEngine;
using System.Collections;
using PureMVC.Patterns;
using PureMVC.Interfaces;
using System.Collections.Generic;

public class StatusPanel_Mediator : Mediator
{
    private StatusPanel_View view
    {
        get
        {
            return (StatusPanel_View)ViewComponent;
        }
    }
    public StatusPanel_Mediator(StatusPanel_View statusPanel) : base(NAME, statusPanel)
    {
        statusPanel.Init();
    }

    public override void HandleNotification(INotification notification)
    {
        switch (notification.Name)
        {
            case ViewConst.ShowScore:
                {
                    HandleUpdateScore((int)notification.Body);
                }break;
            case ViewConst.ShowStep:
                {
                    HandleUpdateStep((int)notification.Body);
                }break;
        }
    }
    private void HandleUpdateScore(int score)
    {
        view.UpdateScore(score);
    }
    private void HandleUpdateStep(int step)
    {
        view.UpdateStep(step);
    }
    public override IList<string> ListNotificationInterests()
    {
        List<string> list = new List<string>();
        list.Add(ViewConst.ShowScore);
        list.Add(ViewConst.ShowStep);
        return list;
    }
    public override void OnRegister()
    {
        base.OnRegister();
    }
}
