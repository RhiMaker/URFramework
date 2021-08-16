using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : SingletonMono<JoyStick>
{
    public GameObject stickCenter;
    public GameObject stickBG;
    Vector3 joyStickPosition;
    Vector3 stickPressPosition;
    public Vector3 joyVec;
    float stickRadius;
    public bool isPlayerMoving = false;

    public const string PLAYER_RUN = "player_run";
    public const string PLAYER_IDLE = "player_idle";
    void Start()
    {
        stickRadius = stickBG.gameObject.GetComponent<RectTransform>().sizeDelta.y / 2;
        joyStickPosition = stickBG.transform.position;
    }
    public void PointDown()
    {
        stickBG.transform.position = Input.mousePosition;
        stickCenter.transform.position = Input.mousePosition;
        stickPressPosition = Input.mousePosition;
        isPlayerMoving = true;
        //PlayerMove.GetInstance().ChangeAnimationState(PLAYER_RUN);
        PlayerMove.GetInstance().animator.SetBool("run",true);
    }
    public void Drag(BaseEventData baseEventData)
    {
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        Vector3 DragPosition = pointerEventData.position;
        joyVec = (DragPosition - stickPressPosition).normalized;
        float stickDistance = Vector3.Distance(DragPosition, stickPressPosition);

        if (stickDistance < stickRadius)
        {
            stickCenter.transform.position = stickPressPosition + joyVec * stickDistance;
        }
        else
        {
            stickCenter.transform.position = stickPressPosition + joyVec * stickRadius;
        }
    }

    public void Drop()
    {
        joyVec = Vector3.zero;
        stickBG.transform.position = joyStickPosition;
        stickCenter.transform.position = joyStickPosition;
        PlayerMove.GetInstance().animator.SetBool("run", false);
        PlayerMove.GetInstance().GetComponent<Rigidbody>().velocity = Vector3.zero;
        //PlayerMove.GetInstance().ChangeAnimationState(PLAYER_IDLE);
        isPlayerMoving = false;
    }
}
