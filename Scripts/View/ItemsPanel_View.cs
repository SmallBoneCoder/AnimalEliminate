using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemsPanel_View : MonoBehaviour {
    private GameObject ItemPrefab;
    private string[] SpriteNames;
    private Dictionary<string, Sprite> NameToSprite;
    private Dictionary<Transform, Item> TransformToItem;
    private Transform[,] ItemToTransform;
    private Transform StartPos;
    public System.Action<Item,Direction> Item_EndDrag;
    public System.Action<Item> ContinueHandle;
    private EventSystem eventSystem;
    private int Step;
    // Use this for initialization
    void Start () {
    }
    public void Init()
    {
        SpriteNames = new string[AppConst.MAXSPRITENUM] {
            ViewConst.SpriteName_Chick,
            ViewConst.SpriteName_Bear,ViewConst.SpriteName_Fox,
            ViewConst.SpriteName_Hippo,ViewConst.SpriteName_Owl};
        NameToSprite = new Dictionary<string, Sprite>();
        TransformToItem = new Dictionary<Transform, Item>();
        ItemToTransform = new Transform[AppConst.MAXROWS,AppConst.MAXCOLUMNS];
        StartPos = transform.parent.Find("StartPos");
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        LoadPrefabs();
    }
	private void LoadPrefabs()
    {
        ItemPrefab = Resources.Load<GameObject>("Prefabs/Item");
        Debug.Log(ItemPrefab);
        for(int i = 0; i < SpriteNames.Length; i++)
        {
            Sprite spr = Resources.Load<Sprite>("Image/" + SpriteNames[i]);
            NameToSprite.Add(SpriteNames[i], spr);//存储字符串对应的图片
        }
    }
    private void DestroyAllItems()
    {
        TransformToItem.Clear();
        Debug.Log("Destroy Num:"+ transform.childCount);
        int count = transform.childCount;
        for (int i = 0; i < count; i++)
        {
            
            DestroyImmediate(transform.GetChild(0).gameObject);
            
        }
        ItemToTransform = new Transform[AppConst.MAXROWS, AppConst.MAXCOLUMNS];
    }
    public void ConstructAllItem(List<List<Item>> allItems)
    {
        DestroyAllItems();
        for (int i = 0; i < allItems.Count; i++)
        {
            for(int j = 0; j < allItems[i].Count; j++)
            {
                GameObject item = Instantiate(ItemPrefab, transform);
                item.transform.localPosition = StartPos.localPosition +
                    new Vector3(j * AppConst.ITEMSIZE, -i * AppConst.ITEMSIZE, 0);
                
                item.GetComponent<Image>().sprite = 
                    NameToSprite[SpriteNames[allItems[i][j].SpriteNameId]];
                item.AddComponent<ItemDrag>();
                item.GetComponent<ItemDrag>().DragEndHandle = HandleDragEnd;//添加事件
                TransformToItem.Add(item.transform, allItems[i][j]);//保存索引
                ItemToTransform[i, j] = item.transform;
            }
        }
        ContinueHandle(TransformToItem[ItemToTransform[0, 0]]);
    }
    private void HandleDragEnd(Transform item,Direction dir)
    {
        Debug.Log(item.GetComponent<Image>().sprite.name + " " + dir);
        Debug.Log(TransformToItem[item].Index_Row + " " +
            TransformToItem[item].Index_Column);
        if (Item_EndDrag != null)
        {
            if (Step > 0)
            {
                Item_EndDrag.Invoke(TransformToItem[item], dir);
            }
        }
    }
    public void UpdateStep(int step)
    {
        Step = step;
    }
    public void ExchangeItemSprite(Item item1,Item item2)
    {
        Transform trans1 = null;
        Transform trans2 = null;
        //查找到对应的transform
        foreach (var item in TransformToItem)
        {
            if (item.Value.Equals(item1))
            {
                trans1 = item.Key;
            }
            if (item.Value.Equals(item2))
            {
                trans2 = item.Key;
            }
        }
        //交换图片
        Sprite temp = trans1.GetComponent<Image>().sprite;
        trans1.GetComponent<Image>().sprite = trans2.GetComponent<Image>().sprite;
        trans2.GetComponent<Image>().sprite = temp;
        //
    }

    private IEnumerator ShowEffects(List<Item> list, int count,float rate)
    {
        for(int i = 0; i < count; i++)
        {
            for(int j = 0; j < list.Count; j++)
            {
                ItemToTransform[list[j].Index_Row, list[j].Index_Column].
                    GetComponent<Image>().color = Color.red;
            }
            yield return new WaitForSeconds(rate);
            for (int j = 0; j < list.Count; j++)
            {
                ItemToTransform[list[j].Index_Row, list[j].Index_Column].
                    GetComponent<Image>().color = Color.white;
            }
            yield return new WaitForSeconds(rate);
        }
        StartDropDown(list);
    }
    public void ShowDropDown(List<Item> list)
    {
        eventSystem.enabled = false;
        StartCoroutine(ShowEffects(list, 3, 0.1f));
    }
    public void StartDropDown(List<Item> list)
    {
        UpdateSprites();
        Dictionary<int, int> colCount = new Dictionary<int, int>();
        //统计所有列有几个要消除
        for(int i = 0; i < list.Count; i++)
        {
            if (colCount.ContainsKey(list[i].Index_Column))
            {
                colCount[list[i].Index_Column] += 1;
            }
            else
            {
                colCount.Add(list[i].Index_Column, 1);
            }
        }
        Debug.Log(colCount.Count);
        //把所有的在消除的元素上方的和元素本身向上移动该列消除的元素的单位量
        List<Transform> isMoved = new List<Transform>();
        for(int i = 0; i < list.Count; i++)
        {
            for(int j=list[i].Index_Row;j>=0; j--)
            {
                if (!isMoved.Contains(ItemToTransform[j, list[i].Index_Column]))
                {
                    ItemToTransform[j, list[i].Index_Column].localPosition +=
                        new Vector3(0, colCount[list[i].Index_Column] * AppConst.ITEMSIZE);
                    isMoved.Add(ItemToTransform[j, list[i].Index_Column]);//已经移过就不移动了
                    StartCoroutine(StartDrop(ItemToTransform[j, list[i].Index_Column],
                        colCount[list[i].Index_Column] * AppConst.ITEMSIZE));//开启下落协程
                }
                else
                {
                    Debug.Log("已经移动过了");
                }
            }
        }
        int max = -1;
        foreach (var item in colCount)
        {
            if (item.Value > max)
            {
                max = item.Value;
            }
        }
        StartCoroutine(NotifiMoveEnd(AppConst.ITEMSIZE * max));
    }
    private IEnumerator NotifiMoveEnd(float dis)
    {
        float curDis = 0;
        while (curDis < dis)
        {
            curDis += AppConst.FIXEDTIME * AppConst.DROPSPEED;
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("Start Continue");
        ContinueHandle(TransformToItem[ItemToTransform[0, 0]]);
        eventSystem.enabled = true;
    }
    private IEnumerator StartDrop(Transform item,float dis)
    {
        Debug.Log("Move start " + Time.time +" "+dis);
        float curDis = 0;
        while (curDis < dis)
        {
            curDis += AppConst.FIXEDTIME * AppConst.DROPSPEED;
            Vector3 temp = item.localPosition;
            temp.y-= AppConst.FIXEDTIME * AppConst.DROPSPEED;
            item.localPosition = temp;
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("Move End"+Time.time);
    }
    public void UpdateSprites()
    {
        foreach (var item in TransformToItem)
        {
            item.Key.GetComponent<Image>().sprite = 
                NameToSprite[SpriteNames[item.Value.SpriteNameId]];//替换成对应的图片
        }
    }
    // Update is called once per frame
    void Update () {
		
	}
}
