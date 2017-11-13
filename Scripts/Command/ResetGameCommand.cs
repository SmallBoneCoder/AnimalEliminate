using UnityEngine;
using System.Collections;
using PureMVC.Patterns;
using PureMVC.Interfaces;

public class ResetGameCommand : SimpleCommand
{
    public override void Execute(INotification notification)
    {
        Debug.Log("Reset Game");
        GameDataProxy proxy = (GameDataProxy)AppFacade.GetInstance().RetrieveProxy(GameDataProxy.NAME);
        proxy.InitGameData();
        SendNotification(ViewConst.ShowAllItems, proxy.gameData);
        SendNotification(ViewConst.UpdateStep,proxy.gameData.Step);
        SendNotification(ViewConst.ShowScore, proxy.gameData.Score);
        SendNotification(ViewConst.ShowStep, proxy.gameData.Step);
    }

}
