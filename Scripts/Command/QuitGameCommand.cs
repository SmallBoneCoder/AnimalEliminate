using UnityEngine;
using System.Collections;
using PureMVC.Patterns;
using PureMVC.Interfaces;

public class QuitGameCommand : SimpleCommand
{

    public override void Execute(INotification notification)
    {
        Debug.Log("Quit Game");
        Application.Quit();//退出程序
    }
}
