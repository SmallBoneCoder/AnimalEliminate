using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item {

    public int SpriteNameId;//图片名称
    public int Index_Column;//列坐标
    public int Index_Row;//行坐标
    public Item(int id,int col,int row)
    {
        SpriteNameId = id;
        Index_Column = col;
        Index_Row = row;
    }
    public Item Clone()
    {
        return new Item(SpriteNameId, Index_Column, Index_Row);
    }
}
