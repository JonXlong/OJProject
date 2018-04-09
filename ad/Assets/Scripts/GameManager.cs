using UnityEngine;
//using System;
using System.Collections;
//using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	public static GameManager Instance;

	public GameWebSocket websocket;
	// public DebugManager debugMgr;

	public Material virutalSkyBox;
    public Material realSkyBox;

    // GameObject Prefabs
    public GameObject missileVirtualWeak;
	public GameObject missileVirtualMid;
	public GameObject missileVirtualHeavy;
	public GameObject missileVirtualCharge_ready;
	public GameObject missileVirtualCharge_attack;
	public GameObject virtualShield;
	public GameObject virtualEnhanceEffect;
	public GameObject virtualGatherEffect;
	public Material virtualDamagePWN;

	public GameObject realTeamFloor;
	public GameObject realMissile;
	public GameObject realDamagePWN;
	public GameObject realDamageDown;
	public GameObject realDamageOccupied;
	public GameObject realGatherEffect;
	public GameObject realAttackImpackMidEffect;
	public GameObject realAttackHeavyImpackEffect;
	public GameObject realShieldEffect;
	public GameObject realUnitRing;

	public GameObject real_line;
	public GameObject real_corner;
    //add wait scene name 
    public string finalsName;
	// UI Prefabs (blood bar)
	public GameObject virtualBloodTeamHUD;
    public GameObject slideHandPrefab;
    public Dictionary<string ,DUIText> bloodTeamHudList;
    public GameObject realTeamHUD;
    //Change Add  Dazzle light Round
    public GameObject virtualDazzleRound;
	public GameObject scoreHUD;
	public AudioListener audioListener;
    [Space(10)]
    public GameObject fireWork;
    public GameObject fireWorkLogo;
    public Transform fireParnt;
    [Space(20)]
    public GameObject _light;
    [Space(5)]
    public ShowTime gameOverScene;
    [Space(20)]
    public EarthTransportScript _anim;
    [HideInInspector]
	public bool initialized = false;
    [Space(20)]
    public SimUpdateScoreData TopScoreData;
    [HideInInspector]
	public GameMode currentGameMode;
    //add request  title team explain data  slide data
    public bool requestSlideData = false;
    public bool requestTeamData = false;
    public bool requestTitleData = false;
    public string channelId;
    public double slideValue;
    public string requestId;
    public string explainData;

    //add 
    public string QuestId;
    public string sendChannelId;
    [DllImport("__Internal")]
	private static extern void RequestConnection();

    //private const string websocketURL = "ws://58.213.63.28:8050/ctfdata";
    //private const string websocketURL = "ws://202.112.51.152:8050/situation_stream/AD_1_REPLAY"; //"ws://192.168.1.250:8050/situation_stream/AD_1_REPLAY";
	private const string websocketURL = "ws://10.10.49.215:8050/situation_stream/AD3_10_REPLAY";

    void Awake () {
		Instance = this;

       
      
        gameOverScene.gameObject.SetActive(false);
        bloodTeamHudList = new Dictionary<string, DUIText>();
    }
    public bool isWait = false;
    public void GoStart(int time)
    {
        //change wait scene
        //if (!SimWorldManager.Instance.currentWorld)
        //{
          //  SetGameMode(GameMode.GM_Vritual);
        //    SimWorldManager.Instance.currentWorld.activeCamera = SimWorldManager.Instance.simVirtualWorld.cameraCircle1;
        //}
       
        UIManager.Instance.gameUI.worldSwitchButton.GetComponent<Button>(). enabled = false;
        UIManager.Instance.gameUI.CameraSwitchButton.GetComponent<Button>().enabled = false;
        
        isWait = true;
        //  SimWorldManager.Instance.currentWorld.activeCamera.GetComponent<SimCamera>().OnZoomInEarthEnd();
        //  SimWorldManager.Instance.currentWorld.activeCamera.GetComponent<SimCamera>().TransportToEarth();
        _anim.Play(false, CreateFrieWork);
        gameOverScene._index = time;
        _light.SetActive(false);
    //    Debug.Log(SimWorldManager.Instance.currentWorld.activeCamera.name);
        //  SimWorldManager.Instance.currentWorld.activeCamera.OnZoomInEarthEnd();
    }
  
    //private void OnGUI()
    //{
    //    GUI.skin.button.fontSize = 20;
    //    if (GUI.Button(new Rect(100, 200, 200, 70), "LoadTestData"))
    //    {
    //        StartCoroutine(LoadTestData());

    //    }
    //    if (GUI.Button(new Rect(100, 400, 200, 70), "GameOverScene"))
    //    {
    //        // SetToGameOverScene();
    //        UIManager.Instance.gameUI.ShowTopScore(); //New topscoreboard
    //    // UIManager.Instance.gameUI.ChangeProgressBar(Testprogress);
    //    }
    //    if (GUI.Button(new Rect(0, 800, 100, 60), "GameStarScene"))
    //    {
    //        GoStart(50);
    //    }
    //}
    private string testStr;
     IEnumerator LoadTestData()
    {
        WWW w = new WWW("file://"+Application.streamingAssetsPath+ "/testdata.txt");
        yield return w;
        testStr = w.text;
        print(testStr);
        JSONObject js = JSONObject.Create(testStr);
        TopScoreData = SimUpdateScoreData.CreateFromJSON(js);
        //print(TopScoreData.list[1].teamName);
    }
    private void SetToGameOverScene()
    {
        gameOverScene.gameObject.SetActive(true);
        // gameOverScene.gameOverScene.SetActive(true);

        gameOverScene.GameOverScene();

    }
    void Start() {
		// UIManager.Instance.gameUI.InitTimeCountDown (69 * 1000 / 1000);

//		if (debugMgr.debugEnable) {
//			debugMgr.EnableDebug ();
//		} else {
			
			if (Application.platform == RuntimePlatform.WindowsEditor) {
                StartSocket (websocketURL);
            } else if (Application.platform == RuntimePlatform.WebGLPlayer) {
				RequestConnection ();
			} else if (Application.platform == RuntimePlatform.WindowsPlayer) {
                StartCoroutine(LoadWebSocketURL());
			} else {
				StartSocket (websocketURL);
			}
        StopMoveCamera(false);
        //hide UI pannel
        CommandSwitchUI(false);
        UIManager.Instance.gameUI.UISwitchButton.SetState(false);
        ////			StartSocket (websocketURL);
        //		}
    }
    private GameObject _fireWork, _fireWorkLogo;
    public void CreateFrieWork()
    {
        //_fireWork = Instantiate(fireWork, fireParnt); _fireWork.transform.localPosition = new Vector3(2, 0, 0);
        //_fireWorkLogo = Instantiate(fireWorkLogo, fireParnt); _fireWorkLogo.transform.localPosition = new Vector3(0, 0, 0);
        //Invoke("DestroyFrieWork", 20.0f);
    }
    public void DestroyFrieWork()
    {
        Destroy(_fireWork); Destroy(_fireWorkLogo);
    }
    IEnumerator LoadWebSocketURL() {
        WWW www = new WWW("file://" + Application.streamingAssetsPath + "/WebSocketSetting.txt");
        yield return www;

        if (www.isDone && www.text.Length > 0) {
          
            StartSocket(www.text);

        } else {
            StartSocket(websocketURL);
            Debug.LogError("Load data failed : WebSocketSetting.txt");
        }
    }

    public void StartSocket(string url) {
        channelId = url;
        websocket.WebsocketLaunch (url);
	}
		
	public void SetGameMode (string gameMode) {
		if (currentGameMode != null) {
			currentGameMode.OnExitMod ();
		}

		switch (gameMode) {
		case GameMode.GM_Vritual:
			currentGameMode = gameObject.AddComponent<VritualGameMode> ();
			break;
		case GameMode.GM_Real:
			currentGameMode = gameObject.AddComponent<RealGameMode> ();
			break;
		case GameMode.GM_CountDown:
			currentGameMode = gameObject.AddComponent<CountDownGameMode> ();
			break;
		}

		currentGameMode.OnEnterMod ();
	}

	public void SetSwtichWorldGameMode (bool virutaltoRealWorld) {
		SwitchWorldGameMode switchGM = gameObject.AddComponent<SwitchWorldGameMode> ();
		switchGM.isVritualToRealWorld = virutaltoRealWorld;
		switchGM.OnEnterMod ();
	}

	/** 
	 * Move to real world
	 **/
	private void SwitchToRealWorld() {
		if (SimWorldManager.Instance.currentWorld.type == SimWorldBase.WORLDATYPE.virtual_world && SimWorldManager.Instance.IsReadyToTransport) {
			SimCamera activeCamera = SimWorldManager.Instance.currentWorld.activeCamera;
			activeCamera.gameObject.SetActive (true);

			UIManager.Instance.uiCamera.gameObject.SetActive (false);
			UIManager.Instance.gameUI.HideUIContainer ();
			SimWorldManager.Instance.currentWorld.StopRotation ();

			SimWorldManager.Instance.TransportToEarth ();
            //Debug.LogError("current world");
        }
	}

	/** 
	 * Move to virtual world
	 **/
	private void SwitchToVritualWorld() {
		if (SimWorldManager.Instance.currentWorld.type == SimWorldBase.WORLDATYPE.real_world && SimWorldManager.Instance.IsReadyToTransport) {
			SimCamera activeCamera = SimWorldManager.Instance.currentWorld.activeCamera;
			activeCamera.gameObject.SetActive (false);

			UIManager.Instance.gameUI.HideUIContainer ();
			SimWorldManager.Instance.currentWorld.StopRotation ();

			SimWorldManager.Instance.TransportToSpace ();
		}
	}
  
	public void SocketMessageHandler(JSONObject msgObj, string jsonMsg) {
        //JSONObject msgObj = null;

        //try {
        //	msgObj = new JSONObject (jsonMsg);
        //} finally {
        //	// nothing to do;
        //}
        //print(jsonMsg);
      
		if (msgObj != null && msgObj.GetField ("type") != null) {
          
            string dataType = msgObj.GetField ("type").str;
			switch (dataType) {
			case GameConfigs.messageType_config:
                    Debug.Log(jsonMsg);
                    Debug.Log(GameConfigs.Instance.scene_default_virtual);
                    JsonUtility.FromJsonOverwrite (jsonMsg, GameConfigs.Instance);
                   
                    Debug.Log(GameConfigs.Instance.real_floor_scale);
                    break;
			case GameConfigs.messageType_command:
                    //Debug.LogError(jsonMsg);
                    SimCommandData commandData = JsonUtility.FromJson<SimCommandData> (jsonMsg);
				CommandHandler (commandData, msgObj);
				break;
			case SimUpdateScoreData.ACTION_score_board:
				SimUpdateScoreData scoresData = SimUpdateScoreData.CreateFromJSON (msgObj);
                    //Debug.Log(scoresData);
				UIManager.Instance.gameUI.UpdateTeamScore (scoresData.list);
				break;
			case GameConfigs.messageType_teams:
				if (!initialized) {
                        //Debug.Log(msgObj);
                        SimWorldManager.Instance.CreateWorld (msgObj);
                    if (GameConfigs.Instance.scene_default_virtual) {
                        UIManager.Instance.gameUI.worldSwitchButton.SetState(true);
                        SetGameMode(GameMode.GM_Vritual);
                    } else {
                        UIManager.Instance.gameUI.worldSwitchButton.SetState(false);
                        SetGameMode(GameMode.GM_Real);
                    }
					initialized = true;
				}
				break;
			case GameConfigs.messageType_behavior:
				if (initialized) {
                     //   Debug.Log(msgObj);
                        SimWorldManager.Instance.ApplyBehavior(SimBehaviorData.CreateFromJSON(msgObj));
                    }
				break;
                //add game over scene
                case GameConfigs.messageType_topScore:
                    TopScoreData = SimUpdateScoreData.CreateFromJSON(msgObj);

                    //SetToGameOverScene(); //old over scene
                    Debug.LogError(TopScoreData.list.Count);
                    //Debug.LogError(msgObj);
                    UIManager.Instance.gameUI.ShowTopScore(); //new  
                    isTopScoreboard = true;
                    break;
                case "titledata":
                    GetExplain(msgObj);
                    break;
                case "teamdata":
                    GetExplain(msgObj);
                    break;
                    // create slide scale  
               
                default:
				//Debug.Log ("GameManager : unknow data type :" + dataType);
				break;
			}
		}
	}
    private void GetExplain(JSONObject job) {
        if (job.GetField("explain"))
        {
            explainData = job.GetField("explain").str;
        }
        else {
            explainData = "Loading data .....";
        }
    }

    private void CommandHandler(SimCommandData commandData,JSONObject  jsobj) {
		switch (commandData.action) {
		case SimCommandData.ACTION_camera:
			if (commandData.param == "normal") {
				CommandSwitchCamera (true);
				UIManager.Instance.gameUI.CameraSwitchButton.SetState (true);
			} else if (commandData.param == "roaming") {
				CommandSwitchCamera (false);
				UIManager.Instance.gameUI.CameraSwitchButton.SetState (false);
			}
			break;
		case SimCommandData.ACTION_mute:
			if (commandData.param == "on") {
				CommandSwitchSound (true);
				UIManager.Instance.gameUI.SoundSwitchButton.SetState (true);
			} else if (commandData.param == "off") {
				CommandSwitchSound (false);
				UIManager.Instance.gameUI.SoundSwitchButton.SetState (false);
			}
			break;
		case SimCommandData.ACTION_reset_units:
			SimWorldManager.Instance.currentWorld.ResetAllUnits ();
			break;
        case SimCommandData.ACTION_set_round:
            UIManager.Instance.gameUI.ShowTitleTxt("Round " + commandData.param, true);
            break;
            //添加比赛名称面板
            case SimCommandData.ACTION_set_finalsname:
                finalsName = commandData.param;
                UIManager.Instance.gameUI.ShowFinalsName(finalsName);
                break;

        case SimCommandData.ACTION_ui_bottom:
			string rmsg = commandData.param;
                if (rmsg.Length > 0) {
                    string[] sArray = rmsg.Split('|');
                    string leftString = sArray.Length > 0 ? sArray[0] : rmsg;
                    string rightString = sArray.Length > 1 ? sArray[1] : "";
                    UIManager.Instance.gameUI.ShowRoundTitleTxt(leftString, rightString);

                    //TODO change Round Number
                    SimWorldManager.Instance.ActiveVirtualDazzle(leftString,rightString);
                }
			
			break;
		case SimCommandData.ACTION_scene:
			if (commandData.param == "virtual") {
				CommandSwitchWorld (true);
				UIManager.Instance.gameUI.worldSwitchButton.SetState (true);
			} else if (commandData.param == "real") {
                // add waitScene hide realscene
				//CommandSwitchWorld (false);
			//	UIManager.Instance.gameUI.worldSwitchButton.SetState (false);
			}
			break;
		case SimCommandData.ACTION_timer:
            int timeNumber = 0;
               
            if (int.TryParse(commandData.param, out timeNumber)) {
                UIManager.Instance.gameUI.InitTimeCountDown(timeNumber);
            }
			break;
		case SimCommandData.ACTION_ui_all:
			if (commandData.param == "show") {
				CommandSwitchUI (true);
				UIManager.Instance.gameUI.UISwitchButton.SetState (true);
			} else if (commandData.param == "hide") {
				CommandSwitchUI (false);
				UIManager.Instance.gameUI.UISwitchButton.SetState (false);
			}
			break;
		case SimCommandData.ACTION_ui_scoreboard:
			if (commandData.param == "show") {
				UIManager.Instance.gameUI.ShowUI (UIManager.UI_TYPE.scoreboard);
			} else if (commandData.param == "hide") {
				UIManager.Instance.gameUI.HideUI (UIManager.UI_TYPE.scoreboard, false);
			}
			break;
		case SimCommandData.ACTION_ui_messagebox:
			if (commandData.param == "show") {
				UIManager.Instance.gameUI.ShowUI (UIManager.UI_TYPE.messagebox);
			} else if (commandData.param == "hide") {
				UIManager.Instance.gameUI.HideUI (UIManager.UI_TYPE.messagebox, false);
			}
			break;
            //add game over scene 
          //  case SimCommandData.ACITON_overscene:
            //    SetToGameOverScene(); //old over scene
           //     UIManager.Instance.gameUI.ShowTopScore(); //new over scene
            //    break;
            //add wait scene
            case SimCommandData.ACTION_Wait:
              //  Debug.LogError("Move to wait scene");
                GoStart(int.Parse(commandData.param));
                break;
            //add data progressbar
            case SimCommandData.ACTION_set_progress:
                // Time.timeScale = 5;
                //if (commandData.param=="hide")
                //{
                //    //Todo hide slide
                //    UIManager.Instance.gameUI.HideProgress();
                //}
                //else
                //{
                    UIManager.Instance.gameUI.ChangeProgressBar(commandData.param);
                    //print(commandData.param);
                //}
                break;
            //add  remove topscoreboard
            case SimCommandData.ACTION_off_topscoreboard:
                if (isTopScoreboard)
                {
                    UIManager.Instance.gameUI.DisPlayTopScore(); isTopScoreboard = false;
                }              
                break;
                // add slide event 
            case "progress_events":
                if (isFirstCreateSlider)
                {
                    List<JSONObject> slidList = jsobj.GetField("param").list;
                    List<SlideEventData> SlideList = new List<SlideEventData>();
                    SlideEventData sed = new SlideEventData();
                    for (int i = 0; i < slidList.Count; i++)
                    {
                        sed = SlideEventData.CreateFromJson(slidList[i]);
                        SlideList.Add(sed);
                    }
                    UIManager.Instance.gameUI.CreateSliderScale(SlideList);
                    isFirstCreateSlider = false;
                }
                break;
                //add team blood bar
            case "scorebucket":
                List<JSONObject> teamjsonList = jsobj.GetField("param").list ;
                //List<TeamBloodData> teamDataList = new List<TeamBloodData>();
                TeamBloodData tbd = new TeamBloodData();
                for (int i = 0; i < teamjsonList.Count; i++)
                {
                    tbd = TeamBloodData.CreateFromJson(teamjsonList[i]);
                    //teamDataList.Add(tbd);
                    DUIText dh = bloodTeamHudList[tbd.teamid];
                    //血条变化
                    dh.AddScore(tbd.current_score,tbd.init_score);
                }
                break;
            default:
			//Debug.Log ("Unkown command type :" + commandData.type);
			break;
		}
	}
    private bool isFirstCreateSlider = true;
    private bool isTopScoreboard = false;
    /**
	 * @true 	normal
	 * @false	roaming
	 **/
    public void CommandSwitchCamera(bool isOn) {
		SimWorldBase world = SimWorldManager.Instance.currentWorld;
      
		if (world) {
			if (isOn) {
				world.SetCameraType (SimWorldBase.CAMERATYPE.normal);
			} else {
				world.SetCameraType (SimWorldBase.CAMERATYPE.roaming);
			}
		}
       
	}

	/**
	 * @true 	on
	 * @false	off
	 **/
	public void CommandSwitchSound(bool isMute) {
		if (isMute) {
			AudioListener.pause = true;
		} else {
			AudioListener.pause = false;
		}
	}

	/**
	 * @true 	virtual
	 * @false	real
	 **/
	public void CommandSwitchWorld(bool isOn) {
		if (isOn) {
			SwitchToVritualWorld ();
		} else {
			SwitchToRealWorld ();
		}
	}

	/**
	 * @true 	on
	 * @false	off
	 **/
	public void CommandSwitchUI(bool isOn) {
		if (isOn) {
			UIManager.Instance.gameUI.ShowUI (UIManager.UI_TYPE.All);
		} else {
			UIManager.Instance.gameUI.HideUI (UIManager.UI_TYPE.All, false);
		}
	}
    /// <summary>
    /// Add Close Camera Move

    /// </summary>
    public void StopMoveCamera(bool isMove)
    {
        switch (isMove)
        {
            //Todo  if click mouse  stopCameraButton show
           case  true: UIManager.Instance.gameUI.ShowCameraButton(UIManager.UI_TYPE.stopCameraButton); break;
            case false:UIManager.Instance.gameUI.HideCamerButton(UIManager.UI_TYPE.stopCameraButton,
                false);  break;
            default:
                break;
        }
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            Time.timeScale += 1;
        }
        if (Input.GetKeyUp(KeyCode.V))
        {
            Time.timeScale =1;
        }
        if (Input.GetKeyUp(KeyCode.M))
        {
            //todo 
        }
    }
    private bool isInitBlood = false;

    // Test  blood bar
    //private void OnGUI()
    //{
    //    if (GUI.Button(new Rect(0, 0, 160, 60), "TestAdd"))
    //    {

    //        DUIText dh = bloodTeamHudList["team-1"];

    //        dh.AddScore(-50, 100);
    //    }
    //    if (GUI.Button(new Rect(0, 100, 160, 60), "Test min"))
    //    {
    //        DUIText dh = bloodTeamHudList["team-1"];
    //        dh.MinScore(Random.Range(50, 200), 100);


    //    }
    //}
}