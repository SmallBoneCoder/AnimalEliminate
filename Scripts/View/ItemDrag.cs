using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 originPos;
    public System.Action<Transform, Direction> DragEndHandle;
    private Direction CurrentDir;
    void Start()
    {
        //transform.GetComponent<RectTransform>().pivot = new Vector2(1, 1);
        
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        originPos = transform.localPosition;
        transform.SetSiblingIndex(transform.parent.childCount - 1);
        Debug.Log("OnBeginDrag");
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        transform.position = eventData.position;
        //transform.localPosition = Input.mousePosition+new Vector3(
        //    (MainUI_Width-transform.parent.GetComponent<Image>().flexibleWidth)/2,
        //   (MainUI_Height- transform.parent.GetComponent<Image>().flexibleHeight)/2) ;
        Debug.Log(eventData.position);
        //求出拖拽后和正上方的夹角
        float angle = Vector3.Angle(Vector3.up, (transform.localPosition - originPos).normalized);
        if (angle < 45f)//往上拖
        {
            Debug.Log("UP");
            CurrentDir = Direction.UP;
            return;
        }
        angle = Vector3.Angle(Vector3.down, (transform.localPosition - originPos).normalized);
        if (angle < 45f)//往下拖
        {
            Debug.Log("DOWN");
            CurrentDir = Direction.DOWN;
            return;
        }
        angle = Vector3.Angle(Vector3.left, (transform.localPosition - originPos).normalized);
        if (angle < 45f)//往左拖
        {
            Debug.Log("LEFT");
            CurrentDir = Direction.LEFT;
            return;
        }
        angle = Vector3.Angle(Vector3.right, (transform.localPosition - originPos).normalized);
        if (angle < 45f)//往右拖
        {
            Debug.Log("RIGHT");
            CurrentDir = Direction.RIGHT;
            return;
        }
        eventData.Reset();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        transform.localPosition = originPos;
        if (DragEndHandle != null)
        {
            DragEndHandle.Invoke(transform, CurrentDir);//处理拖拽结束
        }
    }
}
public enum Direction
{
    UP,
    DOWN,
    LEFT,
    RIGHT
}
