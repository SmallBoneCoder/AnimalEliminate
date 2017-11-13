using PureMVC.Patterns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataProxy : Proxy {

    public GameData gameData {
        get
        {
            return (GameData)base.Data;
        }
    }
    public new const string NAME = "GameDataProxy";
    public GameDataProxy() : base(NAME,new GameData())
    {
        Debug.Log("GameDataProxy is Created");
        InitGameData();
    }

    public void InitGameData()
    {
        gameData.Score = 0;
        gameData.Step = 50;
        Item item = null;
        gameData.AllItems.Clear();

        for(int i = 0; i < gameData.Max_Rows; i++)
        {
            gameData.AllItems.Add(new List<Item>());
            for(int j = 0; j < gameData.Max_Columns; j++)
            {
                int index = Random.Range(0, 5);//随机范围0~4
                item = new Item(index,j,i);
                gameData.AllItems[i].Add(item);
            }
        }
    }
    public List<Item> DoEliminate(List<List<Item>> list)
    {
        List<Item> result = new List<Item>();
        for(int i = 0; i < list.Count; i++)
        {
            for(int j = 0; j < list[i].Count; j++)
            {
                //以所有元素为中心，查找所有连续重复类型的元素
                List<Item> temp=GetEliminableItems(list, list[i][j]);
                for(int k = 0; k < temp.Count; k++)
                {
                    if (!result.Contains(temp[k]))
                    {
                        result.Add(temp[k]);//结果元素引用不重复就添加
                    }
                }
            }
        }
        
        return result;
    }
    public List<Item> GetEliminableItems(List<List<Item>> list,Item center)
    {
        List<Item> result = new List<Item>();
        //横向扫描左方,从目标列-1到第0列
        List<Item> temp = new List<Item>();
        for (int i = center.Index_Column-1; i>=0 ; i--)
        {
            if (list[center.Index_Row][i].SpriteNameId == center.SpriteNameId)
            {
                temp.Add(list[center.Index_Row][i]);//是同一种类就添加
            }
            else
            {
                break;//不是就退出扫描
            }
        }
        //横向扫描右方,从目标列+1扫描最后一列
        for (int i = center.Index_Column + 1; i < AppConst.MAXCOLUMNS; i++)
        {
            if (list[center.Index_Row][i].SpriteNameId == center.SpriteNameId)
            {
                temp.Add(list[center.Index_Row][i]);//是同一种类就添加
            }
            else
            {
                break;//不是就退出扫描
            }
        }
        if (temp.Count == 2)//连成3个
        {
            result.AddRange(temp);//把所有横向相等的都添加进结果
        }
        if (temp.Count == 3)
        {
            result.AddRange(list[center.Index_Row]);//连续大于4个消除一整行
        }
        if(temp.Count >= 4)
        {
            result.AddRange(list[center.Index_Row]);//连续大于5个消除一整行和列
            for (int i = 0; i < list.Count; i++)
            {
                result.Add(list[i][center.Index_Column]);//添加一整列
            }
        }
        temp = new List<Item>();//开辟新空间

        //纵向扫描上方，从目标行-1扫描最后一行
        for (int i = center.Index_Row - 1; i >= 0; i--)
        {
            if (list[i][center.Index_Column].SpriteNameId == center.SpriteNameId)
            {
                temp.Add(list[i][center.Index_Column]);//是同一种类就添加
            }
            else
            {
                break;//不是就退出扫描
            }
        }
        //纵向扫描下方,从目标行+1扫描最后一行
        for (int i = center.Index_Row + 1; i <AppConst.MAXROWS; i++)
        {
            if (list[i][center.Index_Column].SpriteNameId == center.SpriteNameId)
            {
                temp.Add(list[i][center.Index_Column]);//是同一种类就添加
            }
            else
            {
                break;//不是就退出扫描
            }
        }
        if (temp.Count == 2)//连成3个
        {
            result.AddRange(temp);//把所有横向相等的都添加进结果
        }
        if (temp.Count >= 3)
        {
            for(int i = 0; i < list.Count; i++)
            {
                result.Add(list[i][center.Index_Column]);//添加一整列
            }
        }
        if (temp.Count >= 4)
        {
            result.AddRange(list[center.Index_Row]);//连续大于5个消除一整行和列
            for (int i = 0; i < list.Count; i++)
            {
                result.Add(list[i][center.Index_Column]);//添加一整列
            }
            
        }
        if (result.Count > 0)
        {
            result.Add(center);//有连续三个以上就把自己也加入到结果
        }
        return result;
    }
    /// <summary>
    /// 把要消除的元素移动到最上面并随机新的值
    /// </summary>
    /// <param name="eliminate"></param>
    public void DropDownAllItemsId(List<List<Item>> list,List<Item> eliminate)
    {
        for(int i = 0; i < eliminate.Count; i++)
        {
            eliminate[i].SpriteNameId = Random.Range(0, AppConst.MAXSPRITENUM);//重新随机一个值
            for(int j = eliminate[i].Index_Row; j > 0; j--)
            {
                //向上（行数减少）交换ID
                int tempId = list[j][eliminate[i].Index_Column].SpriteNameId;
                list[j][eliminate[i].Index_Column].SpriteNameId = 
                    list[j-1][eliminate[i].Index_Column].SpriteNameId;
                list[j-1][eliminate[i].Index_Column].SpriteNameId = tempId;
            }
        }
    }
}
