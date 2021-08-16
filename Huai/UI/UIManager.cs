using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public enum E_UI_Layer
{
    Bot,
    Mid,
    Top,
    System,
}
public class UIManager : BaseManager<UIManager>
{
    public Dictionary<string, BasePanel> PanelDic = new Dictionary<string, BasePanel>();

    private Transform bot;
    private Transform mid;
    private Transform top;
    private Transform system;

    public RectTransform canvas;
    public UIManager()
    {
        //去找到canvas
        GameObject obj = ResMgr.GetInstance().Load<GameObject>("UI/Canvas");
        canvas = obj.transform as RectTransform;

        GameObject.DontDestroyOnLoad(obj);

        //找到各层
        bot = canvas.Find("Bot");
        mid = canvas.Find("Mid");
        top = canvas.Find("Top");
        system = canvas.Find("System");

        //创建EventSystem让其过场景的时候不被删除
        obj = ResMgr.GetInstance().Load<GameObject>("UI/EventSystem");
        GameObject.DontDestroyOnLoad(obj);

    }
    /// <summary>
    /// 通过枚举，返回层级对象给外部
    /// </summary>
    /// <param name="layer"></param>
    /// <returns></returns>
    public Transform GetLayerFather(E_UI_Layer layer)
    {
        switch (layer)
        {
            case E_UI_Layer.Bot:
                return bot;
            case E_UI_Layer.Mid:
                return mid;
            case E_UI_Layer.Top:
                return top;
            case E_UI_Layer.System:
                return system;
        }
        return null;
    }
    /// <summary>
    /// /显示面板
    /// </summary>
    /// <typeparam name="T">面板类型</typeparam>
    /// <param name="panelName">面板名</param>
    /// <param name="layer">显示的层级</param>
    /// <param name="callback">当面板预设体创建成功后，你想做的事情</param>
    public void ShowPanel<T>(string panelName, E_UI_Layer layer, UnityAction<T> callback = null) where T : BasePanel
    {
        if (PanelDic.ContainsKey(panelName))
        { //已经有面板存着了
            //处理面板创建完成后的逻辑
            if (callback != null)
            {
                callback(PanelDic[panelName] as T);
            }
            return; //避免面板重复加载，如果存在该面板，即直接显示，调用回调函数后 直接return ，不再处理
        }

        ResMgr.GetInstance().LoadAsync<GameObject>("UI/" + panelName, (obj) => {
            //把它作为canvas的子对象
            //设置它的相对位置
            //找到父对象，到底显示哪一层
            Transform father = bot;
            switch (layer)
            {
                case E_UI_Layer.Bot:
                    father = bot;
                    break;
                case E_UI_Layer.Mid:
                    father = mid;
                    break;
                case E_UI_Layer.Top:
                    father = top;
                    break;
                case E_UI_Layer.System:
                    father = system;
                    break;
            }
            //设置父对象 设置相对位置和大小
            obj.transform.SetParent(father);

            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;

            (obj.transform as RectTransform).offsetMax = Vector2.zero;
            (obj.transform as RectTransform).offsetMin = Vector2.zero;

            //得到预设体身上的面板脚本
            T panel = obj.GetComponent<T>();
            //处理面板创建完成后的逻辑
            if (callback != null)
            {
                callback(panel);
            }

            panel.ShowMe();//展示面板之后干的事情，基类的虚函数
            //把面板存起来
            PanelDic.Add(panelName, panel);
        });
    }
    /// <summary>
    /// 隐藏面板
    /// </summary>
    /// <param name="panelName"></param>
    public void HidePanel(string panelName)
    {
        if (PanelDic.ContainsKey(panelName))
        {
            PanelDic[panelName].HidenMe();  //隐藏面板时需要做的事情，基类的虚函数
            GameObject.Destroy(PanelDic[panelName].gameObject);
            PanelDic.Remove(panelName);
        }

    }

    /// <summary>
    /// 得到某一个已经显示的面板方便外部使用
    /// </summary>
    public T GetPanel<T>(string name) where T : BasePanel
    {
        if (PanelDic.ContainsKey(name))
        {
            return PanelDic[name] as T;
        }
        return null;
    }
    /// <summary>
    /// 给空间添加自定义事件监听
    /// </summary>
    /// <param name="control">空间对象</param>
    /// <param name="type">事件类型</param>
    /// <param name="callBack">事件的相应函数</param>
    public static void AddCustomEvent(UIBehaviour control, EventTriggerType type, UnityAction<BaseEventData> callBack)
    {

        EventTrigger trigger = control.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = control.gameObject.AddComponent<EventTrigger>();
        }
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener(callBack);
        trigger.triggers.Add(entry);
    }
}
