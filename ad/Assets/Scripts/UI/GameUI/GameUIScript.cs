using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameUIScript : MonoBehaviour {

    public ImageButton worldSwitchButton;
    public ImageButton SoundSwitchButton;
    public ImageButton CameraSwitchButton;
    public ImageButton UISwitchButton;

    public GameObject scoreboardItemPrefab;
    public GameObject messageItemPrefab;
    public Text gameTimeText;
    public StatementUIScript statementImage;

    public DisplayUIGroup panelContainer;
    public DisplayUIGroup TimeAndButtonsUIDispGroup;
    public DisplayUIGroup messageboxUIDispGroup;
    public DisplayUIGroup scoreboardUIDispGroup;
    public DisplayUIGroup roundUIDispGroup;

    //Add camera Button pannel
    public DisplayUIGroup stopCameraButton;
    public DisplayTitleText titleTextDisp;
    public DisplayTitleText topScoreDisp;
    public InterfaceAnimManager topIAM;
    public float fadeTime = 1.0f;
    private int gameTimeSec = 0;

    public VerticalLayoutGroup behaviorContentsTransform;
    public Transform scoreContentsTransform;
    public Image companyImage;
    public int maxMsgNum = 4;

    private List<SimTeamData> teamsInfoData = null;
    private List<ScoreBoardItem> scoreBoardItemList;
    private ArrayList msgList = new ArrayList();
    [Space(30)]
    public Text finalsNameTxt;
    public Slider slider;
    public Text slideText;
    [Space(20)]
    public Transform overScoreBoardPrant;
    public GameObject overScoreBoardPrefab;
    //add slide scale prefab
    public GameObject slidescalePrefab;
    public Transform slidescaleParnt;
    public Transform slideHandle;
    [Header("Test FPS")]
    [Tooltip("Test Fps")]
    public Text FpsText;
    private float m_LastUpdateShowTime = 0f;  //上一次更新帧率的时间;  

    public float m_UpdateShowDeltaTime = 0.2f;//更新帧率的时间间隔;  

    private int m_FrameUpdate = 0;//帧数;  

    private float m_FPS = 0;
    [Header("QualitySetting")]
    [Tooltip("M open")]
    public GameObject QualityObj;
    private Button CloseQualityPannel;
    private Dropdown dpd;
    private Toggle fantasticToggle;
    private Toggle goodToggle;
    private Toggle simpleToggle;
    // Use this for initialization
    void Start() {
        TimeCountDown();
        //开始隐藏 比赛名称面板当接收到后台数据时显示比赛名称
        finalsNameTxt.transform.parent.gameObject.SetActive(false);
        //当没有进行比赛数据流回放时，进度条不显示
        slider.gameObject.SetActive(false);
        //add  隐藏比赛结束的排行榜面板
        topIAM.gameObject.SetActive(false);
        topScoreDisp.transform.GetChild(0).gameObject.SetActive(false);
        topScoreDisp.transform.GetChild(1).gameObject.SetActive(false);
        slider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        //add  M 键的设置面板
        dpd = QualityObj.transform.Find("Dropdown").GetComponent<Dropdown>();
        CloseQualityPannel = QualityObj.transform.Find("ImageClose").GetComponent<Button>();
        CloseQualityPannel.onClick.AddListener(delegate { CloseQualityObj(); });
        fantasticToggle = QualityObj.transform.Find("Fantastic").GetComponent<Toggle>();
        goodToggle = QualityObj.transform.Find("Good").GetComponent<Toggle>();
        simpleToggle = QualityObj.transform.Find("Simple").GetComponent<Toggle>();
        fantasticToggle.onValueChanged.AddListener(delegate{ QualitySettingFantastic(); });
        goodToggle.onValueChanged.AddListener(delegate { QualitySettingGood(); });
        simpleToggle.onValueChanged.AddListener(delegate { QualitySettingSimple(); });
    }
    private void QualitySettingFantastic()
    {
        if (fantasticToggle.isOn)
        {
            //Debug.Log("fantastic");
            QualitySettings.SetQualityLevel(5);
        }
    }
    private void QualitySettingGood()
    {
        if (goodToggle.isOn)
        {
            //Debug.Log("Good");
            QualitySettings.SetQualityLevel(3);
        }     
    }
    private void QualitySettingSimple()
    {
        if (simpleToggle.isOn)
        {
            //Debug.Log("simple");
            QualitySettings.SetQualityLevel(2);
        }  
    }
    private void CloseQualityObj()
    {
        QualityObj.SetActive(false);
        dpd.onValueChanged.RemoveAllListeners();
    }
    public void InitTimeCountDown(int time) {
        gameTimeSec = time;
    }
    private bool isShowScoreboard = false;
    void TimeCountDown() {

        if (gameTimeSec > 0) {
            gameTimeText.text = TransTimeSecondIntToString(gameTimeSec);
            gameTimeSec--;
            isShowScoreboard = true;
        } else if (gameTimeSec == 0) {
            gameTimeText.text = "00:00:00";

            //Debug.Log("Time is ending");
            if (isShowScoreboard)
            {
                UIManager.Instance.gameUI.HideUI(UIManager.UI_TYPE.scoreboard, false);
                isShowScoreboard = false;
                Debug.Log("Game is ending");
                //To Do  begin  end scene
            }
        }

        Invoke("TimeCountDown", 1.0f);
    }

    private string TransTimeSecondIntToString(int second) {
        string str = "";
        int hour = second / 3600;
        int min = second % 3600 / 60;
        int sec = second % 60;
        if (hour < 10)
        {
            str += "0" + hour.ToString();
        }
        else
        {
            str += hour.ToString();
        }
        str += ":";
        if (min < 10)
        {
            str += "0" + min.ToString();
        }
        else
        {
            str += min.ToString();
        }
        str += ":";
        if (sec < 10)
        {
            str += "0" + sec.ToString();
        }
        else
        {
            str += sec.ToString();
        }
        return str;
    }

    public void DisplayBehaviorMessage(SimBehaviorData behavior) {
        foreach (string msg in behavior.messages) {
            DispalyMsgItem(msg);
        }

        //		string fromName = SimWorldManager.Instance.FindNameByID (behavior.scr_team);
        //
        //		foreach (SimBehaviorDest dest in behavior.dests) {
        //			string targetName = SimWorldManager.Instance.FindNameByID (dest.team);
        //
        //			string headName = "<color=#00ff00ff>[攻击]</color> ";
        //			string linkName = " 攻击了 ";
        //			switch (dest.behavior) {
        //			case GameConfigs.behavior_attack_weak:
        //				headName = "<color=#ffff00ff>[暴力破解]</color> ";
        //				break;
        //			case GameConfigs.behavior_attack_moderate:
        //				headName = "<color=#ffa500ff>[SQL注入]</color> ";
        //				break;
        //			case GameConfigs.behavior_attack_heavy:
        //				headName = "<color=#ff0000ff>[远程命令执行]</color> ";
        //				break;
        //			case GameConfigs.behavior_enhance:
        //				headName = "<color=#ffffffff>[补丁]</color> ";
        //				linkName = " 加固了 ";
        //				break;
        //			}
        //
        //			string timeStr = behavior.time;
        //			if (timeStr == null || timeStr.Length == 0) {
        //				System.DateTime t = System.DateTime.Now;
        //				timeStr = t.Year + "-" + t.Month + "-" + t.Day + " " + t.Hour + ":" + t.Minute + ":" + t.Second + " ";
        //			}
        //
        //			DispalyMsgItem(timeStr + " " + headName + fromName + linkName + targetName);
        //		}
    }

    public void DispalyMsgItem(string msg) {
        GameObject messageItemGo = (GameObject)Instantiate(messageItemPrefab, behaviorContentsTransform.transform);
        Text messageTxt = messageItemGo.GetComponentInChildren<Text>();
        messageTxt.text = msg;

        msgList.Insert(0, messageItemGo);
        //messageItemGo.transform.SetSiblingIndex (0);

        if (msgList.Count > maxMsgNum) {
            GameObject lastItemGo = msgList[msgList.Count - 1] as GameObject;
            msgList.Remove(lastItemGo);
            Destroy(lastItemGo);
        }
    }

    public void InitGameUI(List<SimTeamData> _teamsInfoData) {
        teamsInfoData = _teamsInfoData;
        scoreBoardItemList = new List<ScoreBoardItem>();

        if (teamsInfoData != null) {
            // initialize socreboard
            foreach (SimTeamData teamData in teamsInfoData) {
                GameObject scoreItemGo = Instantiate(scoreboardItemPrefab, scoreContentsTransform);
                ScoreBoardItem scoreItem = scoreItemGo.GetComponent<ScoreBoardItem>();
                scoreBoardItemList.Add(scoreItem);
                scoreItem.gameObject.SetActive(false);
            }
        }
    }

    public void UpdateTeamScore(List<ScoreBoardData> list) {
        if (teamsInfoData != null) {
            int i = 0;
            foreach (ScoreBoardItem scoreItem in scoreBoardItemList) {
                if (i < list.Count) {
                    ScoreBoardData data = list[i];
                    scoreItem.gameObject.SetActive(true);
                    scoreItem.rankTxt.text = data.rank;
                    scoreItem.InitItem(FindTeamInfoDataByID(data.id));
                    scoreItem.UpdateScore(data.score, data.trend);
                } else {
                    scoreItem.gameObject.SetActive(false);
                }
                i++;
            }
        }
    }

    private SimTeamData FindTeamInfoDataByID(string teamId) {
        SimTeamData d = null;
        foreach (SimTeamData tData in teamsInfoData) {
            if (tData.team == teamId) {
                d = tData;
                break;
            }
        }
        return d;
    }
    //Add Show  StopCameraButton 
    public void ShowCameraButton(UIManager.UI_TYPE type)
    {

        if (type == UIManager.UI_TYPE.stopCameraButton)
        {
            stopCameraButton.transform.FindChild("Button").GetComponent<ImageButton>().StopCamerButtonInit();
            stopCameraButton.Show();
        }
    }
    public void HideCamerButton(UIManager.UI_TYPE type, bool isTrue)
    {
        if (type == UIManager.UI_TYPE.stopCameraButton)
        {
            stopCameraButton.Hide(isTrue);
        }
    }
    public void ShowUI(UIManager.UI_TYPE type) {
        if (type == UIManager.UI_TYPE.All) {
            TimeAndButtonsUIDispGroup.Show();
            messageboxUIDispGroup.Show();
            scoreboardUIDispGroup.Show();
            roundUIDispGroup.Show();
        } else if (type == UIManager.UI_TYPE.messagebox) {
            messageboxUIDispGroup.Show();
        } else if (type == UIManager.UI_TYPE.scoreboard) {
            scoreboardUIDispGroup.Show();
        }
    }

    public void HideUI(UIManager.UI_TYPE type, bool hideButtons) {
        if (type == UIManager.UI_TYPE.All) {
            TimeAndButtonsUIDispGroup.Hide(hideButtons);
            messageboxUIDispGroup.Hide(hideButtons);
            scoreboardUIDispGroup.Hide(hideButtons);
            roundUIDispGroup.Hide(hideButtons);
        } else if (type == UIManager.UI_TYPE.messagebox) {
            messageboxUIDispGroup.Hide(hideButtons);
        } else if (type == UIManager.UI_TYPE.scoreboard) {
            scoreboardUIDispGroup.Hide(hideButtons);
        }
    }

    public void ShowUIContainer() {
        panelContainer.Show();
    }

    public void HideUIContainer() {
        panelContainer.Hide(true);
    }

    public void ShowTitleTxt(string txt, bool isShowRound) {
        // show round title
        if (isShowRound) {
            titleTextDisp.Show(txt, 3.0f);
        } else { // show first blood title
            titleTextDisp.Show(txt, -1);
        }
    }
    //add 显示回合排行榜
    public void ShowTopScore()
    {  //防止UI 按钮击穿
       //  panelContainer.cgroup.blocksRaycasts = false;
       //  SimWorldManager.Instance.currentWorld.activeCamera.GetComponent<MouseCamerManager>().enabled = false;
       //SimWorldManager.Instance.currentWorld.activeCamera.GetComponent<FreeLookCamera>().enabled = false;
        topScoreDisp.transform.GetChild(0).gameObject.SetActive(true);
        topScoreDisp.transform.GetChild(1).gameObject.SetActive(true);
        topScoreDisp.ShowTopScoreBoard(-1.0f);
        Invoke("BeginMyFramAnim", 1.4f);
    }
    // add  隐藏回合排行榜
    public void DisPlayTopScore()
    {
        //防止UI 按钮击穿
        //  panelContainer.cgroup.blocksRaycasts = true;
        // SimWorldManager.Instance.currentWorld.activeCamera.GetComponent<MouseCamerManager>().enabled = true;
        //SimWorldManager.Instance.currentWorld.activeCamera.GetComponent<FreeLookCamera>().enabled = true;
        topIAM.startDisappear();
        topScoreDisp.Hide();

        topIAM.GetComponent<ExampleListener>().DisPlayOverScore();



    }
    private void BeginMyFramAnim()
    {
        topIAM.startAppear();
    }
    public void HideFirstBloodTitle() {
        titleTextDisp.Hide();
    }

    public void UIStatement(bool virtualWorld) {
        statementImage.SwitchStatement(virtualWorld);
    }

    public void ShowRoundTitleTxt(string roundNoTxt, string roundTypeTxt) {
        Text roundNoUI = roundUIDispGroup.transform.Find("RoundText").GetComponent<Text>();
        roundNoUI.text = roundNoTxt;

        Text roundTypeUI = roundUIDispGroup.transform.Find("RoundTypeText").GetComponent<Text>();
        roundTypeUI.text = roundTypeTxt;
    }
    //private void OnGUI()
    //{
    //    if (GUI.Button (new Rect (0,0,100,60),"武汉赛"))
    //    {
    //        ShowFinalsName("武汉赛");
    //    }
    //    if (GUI.Button(new Rect(0, 80, 100, 60), "NUll"))
    //    {
    //        ShowFinalsName(" ");
    //    }
    //    if (GUI.Button(new Rect(0, 200, 100, 60), "南京总决赛"))
    //    {
    //        ShowFinalsName("南京总决赛");
    //    }
    //}
    //add finalsName
    public void ShowFinalsName(string str)
    {
        if (str != null && str != "")
        {
            finalsNameTxt.text = "";
            finalsNameTxt.text = str;
            finalsNameTxt.transform.parent.gameObject.SetActive(true);
        }


    }
    // change hide progress bar
    public void HideProgress()
    {
        slider.gameObject.SetActive(false);
    }
    // change progress bar
    public void ChangeProgressBar(string str)
    {
        if (restSlidePos)
        {

            //if (mouseUp > 0 && mouseUp * 100- double.Parse(str)>0)
            //{
            //    print(mouseUp * 100 + " 后台位置数据为  ：" + float.Parse(str) + "  相减得：" + (mouseUp * 100 - double.Parse(str)));
            //    slider.value = float.Parse(mouseUp.ToString());

            //    return;

            //}
            //else if (mouseUp > 0 && mouseUp * 100 -double.Parse(str)<0) {
            //    slider.value = float.Parse(mouseUp.ToString());

            //    return;
            //}

          //else 
 if (str != null && str != "")
        {
                if (!slider.gameObject.activeSelf)
                {
                    slider.gameObject.SetActive(true);

                }

                float fl1 = float.Parse(str) / 100;
           
            if (fl1>=1)
            {
                    slider.value = 1; slideText.text = "100%";
                }
                if (fl1<=0)
                {
                    slider.value = 0;
                    slideText.text = "0.00%";
                }
                else
                {
                    slider.value = fl1;
                    //slideText.text = str+"%";
                }
        }
        }
    }
    private double mouseUp = 0;
    // mouse click slide and mousedrag slide sendmessage
    public void MouseUpSlider()
    {
        restSlidePos = false;
        mouseUp = Math.Round(slider.value, 4);
        GameManager.Instance.slideValue =mouseUp;
        //slideText.text =( mouseUp*100).ToString("f2")+"%";
        GameManager.Instance.requestSlideData = true;
        //print("Begin sendmessage slider value:  " + Math.Round(slider.value, 4));
        //Invoke("RestRectSlidePos",3.0f);
        RestRectSlidePos();
    }
    private void RestRectSlidePos()
    {
        restSlidePos = true;
    }
    private bool restSlidePos = true;
    // receive slide data create slide scale
    public void CreateSliderScale(List<SlideEventData> slideList)//List<GameObject> slidList)
    {
        for (int i = 0; i < slideList.Count; i++)
        {
            SliderAction obj = Instantiate(slidescalePrefab, slidescaleParnt).GetComponent<SliderAction>();
            obj.sliderMessage = slideList[i].events;  //slidList[i].message
            float index = i * 0.1f;
            float pos = slideList[i].pos;
            obj.SetSlideScalePostion(pos);  //slidList[i].pos;
            //obj.SetSlideScaleColor(testColor[UnityEngine.Random.Range(0,testColor.Length)]);
            obj.SetSlideScaleColor(slideList[i].color);
            if (i==slideList.Count-1)
            {
                slideHandle.transform.SetSiblingIndex(slidescaleParnt.childCount );
                //Debug.LogError(slideHandle.transform.GetSiblingIndex());
            }
        }
      
    }
    string strSlideValue;
    public void ValueChangeCheck()
    {
        //Debug.Log(slider.value);
      
        if (slider.value>0.982)
        {
            slideText.transform.localPosition = new Vector3(50, 17.5f, 0);
            if (slider.value==1)
            {
                strSlideValue = (slider.value*100).ToString("f0")+"%";
            }
            else
            {
                strSlideValue = (slider.value*100).ToString("f2")+"%";
            }
        }
        if (slider.value<0.982)
        {
            //to change pos
            slideText.transform.localPosition = new Vector3(68, 17.5f, 0);
            strSlideValue = (slider.value*100).ToString("f2")+"%";
        }
       
            slideText.text = strSlideValue;
        
    }
    string[] testColor = new string[4] { "red","blue", "yellow","green" };
    //public void OnGUI()
    //{
    //    if (GUI.Button(new Rect(0,0,160,60),"Test slide"))
    //    {

    //        CreateSliderScale(4);
    //    }
    //}
 
    bool isFps = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
          
            isFps = !isFps;
            FpsText.transform.parent.gameObject.SetActive(isFps);
        }
        m_FrameUpdate++;
        if (Time.realtimeSinceStartup - m_LastUpdateShowTime >= m_UpdateShowDeltaTime)
        {
            m_FPS = m_FrameUpdate / (Time.realtimeSinceStartup - m_LastUpdateShowTime);
            m_FrameUpdate = 0;
            m_LastUpdateShowTime = Time.realtimeSinceStartup;
            FpsText.text = m_FPS.ToString("0.00");
            //print(Time.realtimeSinceStartup);
        }
        if (Input.GetKeyUp(KeyCode.M))
        {
            //todo 
            QualityObj.SetActive(true);
            dpd.onValueChanged.AddListener(delegate{DropDownChangeValue(); });
        }
        if (Input.GetKeyUp(KeyCode.P))
        {
            //5.6版本unity 截屏方法 图片位置为Windows版本  CPSOJ_Windows/oj_Data/
            Application.CaptureScreenshot(string.Format("{0}X{1}_{2}.jpg", Screen.width, Screen.height,
            System.DateTime.Now.ToString("yyyyMMdd HHmmssff")));
            /* 2017.2 版本截屏方法
                 UnityEngine.ScreenCapture.CaptureScreenshot(string.Format("{0}X{1}_{2}.jpg",Screen.width,Screen.height,
                    System.DateTime.Now.ToString("yyyyMMdd HHmmssff")));   
             */
        }
    }

    void DropDownChangeValue()
    {
        switch (dpd.value)
        {
            case 0: Screen.SetResolution(900, 600, false);Debug.Log(900);  break;
            case 1: Screen.SetResolution(1024, 768, false);Debug.Log(1024);  break;
            case 2: Screen.SetResolution(1920, 1080, true); Debug.Log(1920); break;

            default:
                break;
        }
    }
    
    //add score color
    Color color;
    public Color GetScoreColor(int i)
    {
        switch (i)
        {
            case 0:
                color = new Color(232f / 255f, 61f / 255f, 24f / 255f, 1);


                break;
            case 1:
                color = new Color(236f / 255f, 104f / 255f, 25f / 255f, 1);

                break;
            case 2:
                color = new Color(1.0f, 182f / 255f, 0.0f, 1);

                break;
        }
        return color;

    }
    //private void OnGUI()
    //{
    //    if (GUI.Button(new Rect(0, 0, 100, 60), "slider"))
    //    {
    //        ChangeProgressBar("0.6");
    //    }
    //}
}