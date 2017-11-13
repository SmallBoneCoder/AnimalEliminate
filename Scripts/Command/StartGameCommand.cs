using UnityEngine;
using System.Collections;
using PureMVC.Patterns;
using PureMVC.Interfaces;

public class StartGameCommand : SimpleCommand
{

    public override void Execute(INotification notification)
    {
        Debug.Log("Start Game");
        GameDataProxy proxy = (GameDataProxy)AppFacade.GetInstance().RetrieveProxy(GameDataProxy.NAME);
        SendNotification(ViewConst.ShowAllItems, proxy.gameData);
        SendNotification(ViewConst.ShowScore, proxy.gameData.Score);
        SendNotification(ViewConst.ShowStep, proxy.gameData.Step);
        SendNotification(ViewConst.UpdateStep, proxy.gameData.Step);
        SendNotification(ViewConst.HideLoginUI);
    }
}
