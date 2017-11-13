using PureMVC.Patterns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PureMVC.Interfaces;
using System;

public class ItemsPanel_Mediator : Mediator {
    private GameDataProxy dataProxy;
    private ItemsPanel_View view
    {
        get
        {
            return (ItemsPanel_View)ViewComponent;
        }
    }
    public new const string NAME = "ItemsPanel_Mediator";
    public ItemsPanel_Mediator(ItemsPanel_View itemsPanel):base(NAME, itemsPanel)
    {
        itemsPanel.Init();
        itemsPanel.Item_EndDrag += HandleItem_EndDrag;
        itemsPanel.ContinueHandle += HandleContinue;
    }

    private void HandleStep(int step)
    {
        dataProxy.gameData.Step = step;
        SendNotification(ViewConst.ShowStep, dataProxy.gameData.Step);
    }

    private void HandleContinue(Item item)
    {
        SendNotification(CommandConst.CMD_MoveTo, new MoveToItem()
        {
            Item = item,
            offset_Row = 0,
            offset_Col = 0
        });
    }
    private void HandleItem_EndDrag(Item item,Direction dir)
    {
        switch (dir)
        {
            case Direction.UP:
                {
                    if (item.Index_Row >=1)
                    {
                        SendNotification(CommandConst.CMD_MoveTo, new MoveToItem()
                        {
                            Item = item,
                            offset_Row = -1,
                            offset_Col=0
                        });
                    }
                }
                break;
            case Direction.DOWN:
                {
                    if (item.Index_Row < AppConst.MAXROWS - 1)
                    {
                        SendNotification(CommandConst.CMD_MoveTo, new MoveToItem()
                        {
                            Item = item,
                            offset_Row = 1,
                            offset_Col = 0
                        });
                    }
                }
                break;
            case Direction.LEFT:
                {
                    if (item.Index_Column >= 1)
                    {
                        SendNotification(CommandConst.CMD_MoveTo, new MoveToItem()
                        {
                            Item = item,
                            offset_Row = 0,
                            offset_Col = -1
                        });
                    }
                }
                break;
            case Direction.RIGHT:
                {
                    if (item.Index_Column < AppConst.MAXCOLUMNS - 1)
                    {
                        SendNotification(CommandConst.CMD_MoveTo, new MoveToItem()
                        {
                            Item = item,
                            offset_Row = 0,
                            offset_Col = 1
                        });
                    }
                }
                break;
        }
    }



    public override void OnRegister()
    {
        base.OnRegister();
        dataProxy = (GameDataProxy)AppFacade.GetInstance().RetrieveProxy(GameDataProxy.NAME);
    }
    public override IList<string> ListNotificationInterests()
    {
        IList<string> list = new List<string>();
        list.Add(ViewConst.ShowAllItems);
        list.Add(ViewConst.ShowExchangeItem);
        list.Add(ViewConst.ShowDropDown);
        list.Add(ViewConst.UpdateStep);
        return list;
    }
    public override void HandleNotification(INotification notification)
    {
        switch (notification.Name)
        {
            case ViewConst.ShowAllItems:
                {
                    CallBack_ShowAllItems((GameData)notification.Body);
                }break;
            case ViewConst.ShowExchangeItem:
                {
                    ExchangeItems exchange = (ExchangeItems)notification.Body;
                    view.ExchangeItemSprite(exchange.from, exchange.to);
                }break;
            case ViewConst.ShowDropDown:
                {
                    view.ShowDropDown((List<Item>)notification.Body);
                }break;
            case ViewConst.UpdateStep:
                {
                    view.UpdateStep((int)notification.Body);
                }break;

        }
    }
    private void CallBack_ShowAllItems(GameData gameData)
    {
        view.ConstructAllItem(gameData.AllItems);
    }
}
