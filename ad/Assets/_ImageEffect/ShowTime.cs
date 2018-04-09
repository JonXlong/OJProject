//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShowTime : MonoBehaviour {
    [Header("Now Time")]
    public Text _text;
    [Header("Target")]
    public Transform _targetTrans;
    [Header("Camera")]
    public Transform _cameraTrans;
    [Header("MainCamera")]
    public GameObject mainCamera;
    public static ShowTime Instance;
    [Space(20)]
    [Tooltip("Show this scene when it's receive wait data")]
    [Header("WAIT SCENE ")]
    public GameObject waitScene;
    [Space(20)]
    [Tooltip("Game over")]
    [Header("Game over Scene")]
    public GameObject gameOverScene;
    [Tooltip("Hide virtualcamera when it's end of game")]
    [Header("VirtualCamer3")]
    public GameObject simVirtualCamera;
    [Tooltip("Change this camera target when it's end of game")]
    [Header("ExampleCam")]
    public MouseOrbitImproved exampleCam;
    [Space(6)]
    public ExampleUI exampleUI;
    public Text finalsName;
    private void Awake()
    {
        Instance = this;
       // mainCamera = SimWorldManager.Instance.currentWorld.activeCamera.gameObject;
        InitScene();
    }
   
    private bool isMove = false;
    private Vector3 refVect;
    [Header("Move Speed")]
    public  float moveSpeed = 3;
    [Header("Distance")]
    public float distance = 5;
    private void InitScene()
    {
        //exampleCam.transform.localPosition = new Vector3(0, 0, -5);
        //exampleCam.transform.localRotation = Quaternion.Euler(Vector3.zero);
        waitScene.SetActive(true);
        //Debug.Log("show time  init");
        gameOverScene.SetActive(false);
       
    }
  
    public  void GameOverScene()
    {
        GameManager.Instance.gameObject.GetComponent<AudioListener>().enabled = false ;
        waitScene.SetActive(false);
        gameOverScene.SetActive(true);
        SimWorldManager.Instance.currentWorld.activeCamera.gameObject.SetActive(false);
        //  simVirtualCamera.SetActive(false);
        exampleCam.transform.position = new Vector3(0, 0, -5);
        exampleCam.transform.rotation = Quaternion.Euler(Vector3.zero);
        exampleCam.enabled = true;
        exampleCam.target = gameOverScene.transform.GetChild(0).transform;

      
        //init UI
        InvokeRepeating("NextUI", 3.0f, 3.0f);
    }
    private int index = 0;
    private void NextUI()
    {
      
        print("+++++++++++++++++");
        exampleUI.DoNext();
        index++;
        switch (index)
        {
            case 2:
                //TODO  Fire  work 
                //Debug.LogError("TO DO FIRE ");
                GameObject fire1=  Instantiate(GameManager.Instance.fireWork,exampleCam.target);
                fire1.transform.localPosition = new Vector3(-2, -18, 0);
                GameObject fire2 = Instantiate(GameManager.Instance.fireWorkLogo,exampleCam.target);
                fire2.transform.localPosition = new Vector3(0, -20, 0);
                break;
            case 3:
                this.CancelInvoke();
                exampleUI.enabled = false;
                break;
            default:
                break;
        }
      
    }
    private void Start()
    {
        // finalsName.text = "比赛即将开始";
        string strName = GameManager.Instance.finalsName;
        if (strName != "" && strName != null)
        {
            finalsName.text = strName;
        }
        else { finalsName.text = "比赛即将开始"; }

        TimeBegin(); 
    }
    //private void OnGUI()
    //{
    //    if (GUI.Button(new Rect(0, 100, 100, 60), "Show Door"))
    //    {
    //        _targetTrans.gameObject.SetActive(true);
    //    }
    //    if (GUI.Button (new Rect(0,200,100,60),"Begin move to dack"))
    //    {
    //        isMove = true;
    //    }
    //    if (GUI.Button(new Rect(180, 200, 100, 60), "Begin time"))
    //    {
    //        _index = 10;
    //    }
    //}
    //public void BeginToScene()
    //{
    //    _targetTrans.gameObject.SetActive(true);
    //    isMove = true;
    //}
    void SetOBJ()
    {
       
        gameObject.SetActive(false);
        _targetTrans.gameObject.SetActive(false);
       // mainCamera.GetComponent<SimCamera>().enabled = false;
        if (GameManager.Instance.gameObject.GetComponent<AudioListener>())
        {
            GameManager.Instance.gameObject.GetComponent<AudioListener>().enabled = true;
        }
        GameManager.Instance.SetGameMode(GameMode.GM_Vritual);
        UIManager.Instance.gameUI.worldSwitchButton.SetState(true);
        //SetGameMode(GameMode.GM_Vritual);
        //SimWorldManager.Instance.currentWorld.gameObject.SetActive(true);
        //mainCamera.SetActive(true);
        // GameManager.Instance.GoStart();
    }
   public  int _index=10;
   
    public void TimeBegin()
    {
        if (_index > 0)
        {
            _text.text = GetTime(_index);
            _index--;
            //Debug.Log(GetTime(_index));
           
        }
        else if (_index==0)
        {
            _text.text = "00:00:00";
            _targetTrans.gameObject.SetActive(true);
            UIManager.Instance.gameUI.worldSwitchButton.GetComponent<Button>().enabled = true;
            UIManager.Instance.gameUI.CameraSwitchButton.GetComponent<Button>().enabled = true;


            GameManager.Instance.isWait = false;
            isMove = true;
            this.CancelInvoke();
        }
        Invoke("TimeBegin", 1.0f);
        if (isMove)
        {
            this.CancelInvoke();
        }
        //if (IsInvoking("TimeBegin"))
        //{
        //    Debug.LogError("............");
        //}
        
    }
    private string GetTime(int index)
    {
        string str = "";
        int  hour = index / 3600;int min = index % 3600 / 60;int sec = index % 60;
        if (hour < 10)
        {
            str += "0" + hour.ToString();
        }
        else {
            str += hour.ToString();
        }
        str += ":";
        if (min<10)
        {
            str +="0"+ min.ToString();
        }
        else
        {
            str += min.ToString();
        }str += ":";
        if (sec<10)
        {
            str += sec.ToString();
        }
        else
        {
            str += sec.ToString();
        }
        return str;
    }
    // Update is called once per frame
    bool isShow = false;
    void Update () {
        if (isMove )
        {
            Vector3  pos = _targetTrans.position - _cameraTrans.position;
           
            Quaternion lookat = Quaternion.LookRotation(pos, Vector3.up);
            _cameraTrans.rotation = Quaternion.Lerp(_cameraTrans.rotation, lookat, 2 * Time.deltaTime);
            _cameraTrans.position = new Vector3(Mathf.SmoothDamp(_cameraTrans.position.x, _targetTrans.position.x, ref refVect.x, moveSpeed), Mathf.SmoothDamp(_cameraTrans.position.y, _targetTrans.position.y, ref refVect.y, moveSpeed), Mathf.SmoothDamp(_cameraTrans.position.z, _targetTrans.position.z, ref refVect.z, moveSpeed));
            float distancePos = Vector3.Distance(_cameraTrans.position, _targetTrans.position);
           isMove=  distancePos <= distance ? false : isMove;
            if (isMove==false )
            {
                GameManager.Instance._light.SetActive(true);
                isShow = true;
            }
        }

        if (isShow )
        {

            SetOBJ();
            isShow = false;
        }
      //  _text.text = System.DateTime.Now.ToString();
    }
}
