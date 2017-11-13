using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData{

    public int Max_Columns;
    public int Max_Rows;
    public List<List<Item>> AllItems;
    public int Score;
    public int Step=50;
	public GameData()
    {
        Max_Columns = AppConst.MAXCOLUMNS;
        Max_Rows = AppConst.MAXROWS;
        AllItems = new List<List<Item>>();
        Step = 50;
    }
    public List<List<Item>> CloneAllItems()
    {
        List<List<Item>> list = new List<List<Item>>();
        for(int i = 0; i < AllItems.Count; i++)
        {
            list.Add(new List<Item>());
            for(int j = 0; j < AllItems[i].Count; j++)
            {
                list[i].Add(AllItems[i][j].Clone());
            }
        }
        return list;
    }
}
