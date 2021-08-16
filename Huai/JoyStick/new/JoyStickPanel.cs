using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public enum JoyStickType
{
    Normal,
    Hide,
    Move,
}
public class JoyStickPanel : BasePanel
{
    public JoyStickType type;
    public float maxL=150;
    private Image imgTouchRect;
    private Image imgBK;
    private Image imgControl;
    private void Start()
    {
        imgBK = GetControl<Image>("imgBK");
        imgControl = GetControl<Image>("imgControl");
        //事件监听面板，控制范围
        imgTouchRect = GetControl<Image>("imgTouchRect");
        //通过UI管理器提供的添加自定义时间的方法，把对应的函数和事件关联起来，一一进行处理
        UIManager.AddCustomEvent(imgTouchRect, EventTriggerType.PointerDown,PointerDown);
        UIManager.AddCustomEvent(imgTouchRect, EventTriggerType.PointerUp, PointerUp);
        UIManager.AddCustomEvent(imgTouchRect, EventTriggerType.Drag, Drag);
        if(type != JoyStickType.Normal)
        imgBK.gameObject.SetActive(false);
    }

    private void PointerDown(BaseEventData data)
    {
        imgBK.gameObject.SetActive(true);
        if (type != JoyStickType.Normal)
        {
            Vector2 localPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                imgTouchRect.rectTransform,
                (data as PointerEventData).position,
                 (data as PointerEventData).pressEventCamera,
                 out localPos);
            imgBK.transform.localPosition = localPos;
        }
        //分发触摸状态
        EventCenter.GetInstance().EventTrigger<string>("TouchState", "Down");
    }
    private void PointerUp(BaseEventData data)
    {
        imgControl.transform.localPosition = Vector3.zero;
        if (type != JoyStickType.Normal)
        {
            //分发方向
            EventCenter.GetInstance().EventTrigger<Vector2>("JoyStick", Vector2.zero);
            imgBK.gameObject.SetActive(false);
        }
        //分发触摸状态
        EventCenter.GetInstance().EventTrigger<string>("TouchState", "Up");
    }
    private void Drag(BaseEventData data)
    {
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            imgBK.rectTransform,
            (data as PointerEventData).position,
             (data as PointerEventData).pressEventCamera,
             out localPos);
        //更新位置
        imgControl.transform.localPosition = localPos;

        //范围判断
        if (localPos.magnitude>maxL)
        {
            if (type == JoyStickType.Move)
            {
                imgBK.transform.localPosition += (Vector3)(localPos.normalized * (localPos.magnitude - maxL));
            }
            imgControl.transform.localPosition = localPos.normalized*maxL;
        }
        //分发方向
        EventCenter.GetInstance().EventTrigger<Vector2>("JoyStick",localPos.normalized);
        //分发触摸状态
        EventCenter.GetInstance().EventTrigger<string>("TouchState", "Drag");
    }
}
