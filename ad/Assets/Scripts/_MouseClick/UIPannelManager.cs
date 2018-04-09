using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPannelManager : MonoBehaviour {

    public static UIPannelManager Instance;
    public GameObject titlepannelOBJ;
    public GameObject teampannelOBJ;
    private Animator titleanimator;
    private Animator teamanimator;
    private Text titleNameTxt;
    private Text titleScoreCountTxt;
    private Text scoreTxt;
    private Text firstTxt;
    private Text secondTxt;
    private Text thridTxt;
    private Button titleCloseBtn;

    private Text teamNameTxt;
    private Image teamLogoImage;
    private Text teamInstructionTxt;
    public GameObject teamSolvedPrefab;
    public Transform teamSolvedPrant;
    private Button teamCloseBtn;
   
    private void Awake()
    {
        Instance = this;
        titleanimator = titlepannelOBJ.GetComponent<Animator>();
        teamanimator = teampannelOBJ.GetComponent<Animator>();
        teamNameTxt = teampannelOBJ.transform.FindChild("teamName").GetComponent<Text>();
        teamLogoImage = teampannelOBJ.transform.FindChild("logo").GetComponent<Image>();
        teamInstructionTxt = teampannelOBJ.transform.FindChild("instructions").GetComponent<Text>();

        titleNameTxt = titlepannelOBJ.transform.FindChild("titleName").GetComponent<Text>();
        scoreTxt = titlepannelOBJ.transform.FindChild("score").GetComponent<Text>();
        firstTxt = titlepannelOBJ.transform.FindChild("firstBlood/Text").GetComponent<Text>();
        secondTxt = titlepannelOBJ.transform.FindChild("secondBlood/Text").GetComponent<Text>();
        thridTxt = titlepannelOBJ.transform.FindChild("thridBlood/Text").GetComponent<Text>();
        titleScoreCountTxt  = titlepannelOBJ.transform.FindChild("solveCount").GetComponent<Text>();
        titleCloseBtn = titlepannelOBJ.transform.FindChild("CloseBtn").GetComponent<Button>();
        titleCloseBtn.onClick.AddListener(CloseUIPannel);
        teamCloseBtn = teampannelOBJ.transform.FindChild("CloseBtn").GetComponent<Button>();
        teamCloseBtn.onClick.AddListener(CloseUIPannel);
    }
    public void ShowTitlePannel(string objName, string score, string strsolvedCount, List<JSONObject> bloodsList)
    {
        titleNameTxt.text = objName;
        scoreTxt.text = score;
        titleScoreCountTxt.text = strsolvedCount;
        if (bloodsList!=null )
        {
            firstTxt.text = bloodsList[0].str; secondTxt.text = bloodsList[1].str; thridTxt.text = bloodsList[2].str;
        }
        SetTitleAnimatorState("paly",true ,"back");
    }
    public void ShowTeamPannel()
    {

    }
    public void ShowUIPannel()
    {
        //SetAnimatorState("play", true, "back");
        if (isTitle)
        {
            //titleanimator.SetBool("play", true);
            //titleanimator.SetBool("back", false);
            //isTitle = true;
            SetTitleAnimatorState("play",true ,"back");
        }
        else
        {
            //teamanimator.SetBool("play", true);
            //teamanimator.SetBool("back",false );
            SetTeamAnimatorState ("play",true ,"back");
        }
    }
    public  bool isTitle = false;
    void CloseUIPannel()
    {
        Debug.Log(".........");
        //SetAnimatorState("back", true, "play");
        if (isTitle )
        {
            //titleanimator.SetBool("back", true);
            //titleanimator.SetBool("play",false );
            SetTeamAnimatorState("play", true, "back");
        }
        else
        {
            SetTeamAnimatorState("back",true,"play");
            //teamanimator.SetBool("back",true);
            //teamanimator.SetBool("play", false);
        }
        MouseManager.Instance.mouseState = MouseState.BACK;
        isTitle = false;
    }
    private  void SetTitleAnimatorState(string play, bool isB, string back)
    {
        titleanimator.SetBool(play,isB ); titleanimator.SetBool(back, !isB);
    }
    private void SetTeamAnimatorState(string play, bool isB, string back)
    {
        teamanimator.SetBool(play,isB);
        teamanimator.SetBool(back, !isB);
    }
}
