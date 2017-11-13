using UnityEngine;
using System.Collections;
using PureMVC.Patterns;
using PureMVC.Interfaces;
using System.Collections.Generic;

public class MoveToCommand : SimpleCommand
{

    public override void Execute(INotification notification)
    {
        Debug.Log("Move To Command is Executing...");
        MoveToItem moveToItem = notification.Body as MoveToItem;
        GameDataProxy proxy = (GameDataProxy)AppFacade.GetInstance().RetrieveProxy(GameDataProxy.NAME);
        Item item_From = moveToItem.Item;
        Item item_To = proxy.gameData.AllItems
            [moveToItem.offset_Row + item_From.Index_Row]
            [moveToItem.offset_Col + item_From.Index_Column];
        Debug.Log("from "+item_From.SpriteNameId+" to "+item_To.SpriteNameId);
        //交换图片编号
        int temp_SpriteId = item_From.SpriteNameId;
        item_From.SpriteNameId = item_To.SpriteNameId;
        item_To.SpriteNameId = temp_SpriteId;
        List<List<Item>> list = proxy.gameData.AllItems;
        List<Item> result= proxy.DoEliminate(list);
        if (result.Count > 0)//结果大于0表示有可以消除的对象
        {
            if (item_From != item_To)
            {
                proxy.gameData.Step--;
                SendNotification(ViewConst.UpdateStep, proxy.gameData.Step);
                SendNotification(ViewConst.ShowStep, proxy.gameData.Step);
            }
            Debug.Log("开始消除");
            SendNotification(ViewConst.ShowExchangeItem, new ExchangeItems()
            {
                from=item_From,
                to=item_To
            });//显示交换的两个元素图片
            proxy.DropDownAllItemsId(list,result);//变为消除之后的值
            SendNotification(ViewConst.ShowDropDown, result);
            proxy.gameData.Score += result.Count * 10;
            SendNotification(ViewConst.ShowScore, proxy.gameData.Score);
        }
        else
        {
            Debug.Log("没有可以消除的");
            //交换回图片编号
            temp_SpriteId = item_From.SpriteNameId;
            item_From.SpriteNameId = item_To.SpriteNameId;
            item_To.SpriteNameId = temp_SpriteId;
        }
    }
}
