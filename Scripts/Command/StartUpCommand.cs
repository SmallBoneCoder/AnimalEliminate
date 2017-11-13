using PureMVC.Patterns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PureMVC.Interfaces;

public class StartUpCommand : SimpleCommand {

    public override void Execute(INotification notification)
    {
        Debug.Log("StartUp Command is Executing...");
        AppFacade.GetInstance().RegisterProxy(new GameDataProxy());

        MainUI mainUI = (MainUI)notification.Body;
        AppFacade.GetInstance().RegisterMediator(new ItemsPanel_Mediator(mainUI.itemsPanel));
        AppFacade.GetInstance().RegisterMediator(new ControlPanel_Mediator(mainUI.controlPanel));
        AppFacade.GetInstance().RegisterMediator(new StatusPanel_Mediator(mainUI.statusPanel));
        AppFacade.GetInstance().RegisterMediator(new LoginPanel_Mediator(mainUI.loginPanel));
    }
}
