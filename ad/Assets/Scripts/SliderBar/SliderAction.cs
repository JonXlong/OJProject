using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

public class SliderAction : MonoBehaviour {

    // Use this for initialization
    public Text explainText;
    public string sliderMessage;
    public RectTransform rtf;
    //add  eventTrigger   
    private void OnEnable()
    {
        //to do   add mouseEnter Event
        explainText.gameObject.SetActive(false);
        EventTrigger etg = gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback = new EventTrigger.TriggerEvent();
        UnityAction<BaseEventData> callback = new UnityAction<BaseEventData>(delegate { MouseEnter(sliderMessage); });
        entry.callback.AddListener(callback);
        etg.triggers.Add(entry);

        //mouse exit event
        EventTrigger.Entry entryExit = new EventTrigger.Entry();
        entryExit.eventID = EventTriggerType.PointerExit;
        entryExit.callback = new EventTrigger.TriggerEvent();
        UnityAction<BaseEventData> callExit = new UnityAction<BaseEventData>(MouseExit);
        entryExit.callback.AddListener(callExit);
        etg.triggers.Add(entryExit);

        //add mouse up event
        EventTrigger.Entry pointUpE = new EventTrigger.Entry();
        pointUpE.eventID = EventTriggerType.PointerUp;
        pointUpE.callback = new EventTrigger.TriggerEvent();
        UnityAction<BaseEventData> callUp = new UnityAction<BaseEventData>(SlideScaleMouseUp);
        pointUpE.callback.AddListener(callUp);
        etg.triggers.Add(pointUpE);
    }
    //Slide Scale Mouse up Function
    private void SlideScaleMouseUp(BaseEventData bed)
    {
        UIManager.Instance.gameUI.slider.value = slideV;
       
        UIManager.Instance.gameUI.slideText.text = Math.Round(slideV*100,4).ToString()+"%";

        GameManager.Instance.slideValue = Math.Round(slideV,4); ;
        //print(GameManager.Instance.slideValue+"   :slideV"+slideV);
        GameManager.Instance.requestSlideData = true;
    }
    //set slidescale postion 
    private float slideV;
    public void SetSlideScalePostion(float flt)
    {
        
        rtf.anchorMax = new Vector2(flt,1);
        rtf.anchorMin = new Vector2(flt ,0);
        explainText.text = sliderMessage;
        slideV = flt;
       
    }
    //set slider scale color 
    public void SetSlideScaleColor(string str)
    {
        GetComponent<Image>().color = GameConfigs.Instance.GetRealWorldColorByName(str);
    }
    // mouse exit hide slide scale event message
    public void MouseExit(BaseEventData bed)
    {
        if (explainText.gameObject.activeSelf)
        {
            explainText.gameObject.SetActive(false);
        }
       
    }
    // mouse enter active slide scale event message
    public void MouseEnter(string str)
    {
      //  explainText.text = str;
        explainText.gameObject.SetActive(true);
    }
}
