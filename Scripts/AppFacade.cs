using PureMVC.Patterns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppFacade : Facade {
    private static AppFacade _instance;
    protected AppFacade()
    {

    }
    public static AppFacade GetInstance()
    {
        if (_instance == null)
        {
            _instance = new AppFacade();
        }
        return _instance;
    }
    protected override void InitializeController()
    {
        base.InitializeController();
        RegisterCommand(CommandConst.StartUp, typeof(StartUpCommand));
        RegisterCommand(CommandConst.CMD_MoveTo, typeof(MoveToCommand));
        RegisterCommand(CommandConst.CMD_QuitGame, typeof(QuitGameCommand));
        RegisterCommand(CommandConst.CMD_ResetGame, typeof(ResetGameCommand));
        RegisterCommand(CommandConst.CMD_StartGame, typeof(StartGameCommand));
    }
    public void Startup(MainUI mainUI)
    {
        SendNotification(CommandConst.StartUp, mainUI);//启动框架
    }
}
