using UnityEngine;
//using System.Collections;
using System.Collections.Generic;

public class ExampleListener : MonoBehaviour {
    public InterfaceAnimManager IAM;
    void Awake() {
        if (!IAM)
        return;
        AddListener();
    }
    void AddListener() {
        // For the "event" type, + and - operators have been overloaded. "+" adds
        // a method reference to the list of methods to call when the event is invoked.
        // "-" removes the reference from the list.
        IAM.OnStartAppear += HandleOnStartAppear;
        IAM.OnStartDisappear += HandleOnStartDisappear;
        IAM.OnEndAppear += HandleOnEndAppear;
        IAM.OnEndDisappear += HandleOnEndDisappear;
    }
    void RemoveListener() {
        IAM.OnStartAppear -= HandleOnStartAppear;
        IAM.OnStartDisappear -= HandleOnStartDisappear;
        IAM.OnEndAppear -= HandleOnEndAppear;
        IAM.OnEndDisappear -= HandleOnEndDisappear;

    }
    /// <summary>
    /// ToDo   Top3 team data  
    /// </summary>
    TopTeamDate ttd;
    void HandleOnStartAppear(InterfaceAnimManager _IAM) {
        //Debug.Log("EVENT LISTENER : " + _IAM.name + " onStartAppear");
        switch (_IAM.name)
        {
            //case "Top1":
            //    //_IAM.GetComponent<TopTeamDate>().StartCoroutine(LoadTopTeamPic("LoadTopTeamPic",));
            //    ttd = _IAM.GetComponent<TopTeamDate>();
            //    //StartCoroutine(ttd.LoadTopTeamPic("https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1504245749697&di=9ced7fa9b9c73b0de71b4c6a42f99761&imgtype=0&src=http%3A%2F%2Fattachments.gfan.com%2Fforum%2Fattachments2%2Fday_120721%2F1207211752a4e51edb57f39060.jpg", ttd._teamFlg));
            //    //StartCoroutine(ttd.LoadTopTeamPic("https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1504245498708&di=6076d1a2c83d03c0eca428c6627cc50a&imgtype=0&src=http%3A%2F%2Fwww.mediaclub.cc%2FUploads%2FUploads%2F1463021101%2F75cb32f4587cf792e437fec409e9e870ffaab409.png",ttd._teamLogo));
            //    //ttd.SetTopTeamDate("你瞅啥", "瞅你咋地", "25000", "20");
            //    SetTopdata(ttd, 0);
            //    break;
            //case "Top2":
            //    ttd = _IAM.GetComponent<TopTeamDate>();
            //    //StartCoroutine(ttd.LoadTopTeamPic("https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1504244530223&di=94d6a979b5ef529f88bd09fbb9007dc5&imgtype=0&src=http%3A%2F%2Fpic.58pic.com%2F58pic%2F14%2F47%2F79%2F03X58PICpTP_1024.png", ttd._teamFlg));
            //    //StartCoroutine(ttd.LoadTopTeamPic("https://ss2.bdstatic.com/70cFvnSh_Q1YnxGkpoWK1HF6hhy/it/u=1076477859,2143719377&fm=26&gp=0.jpg", ttd._teamLogo));
            //    //ttd.SetTopTeamDate("瞅你咋地", "瞅你咋地", "20000", "10");
            //    SetTopdata(ttd, 1);
            //    //  _IAM.GetComponent<TopTeamDate>().SetTopTeamDate("就瞅你...", "瞅你咋地", "10000", "10");
            //    break;
            //case "Top3":
            //    ttd = _IAM.GetComponent<TopTeamDate>();
            //    //StartCoroutine(ttd.LoadTopTeamPic("https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1504244692295&di=7914052bc235733d3e2d1465abcda47d&imgtype=0&src=http%3A%2F%2Fpic.58pic.com%2F58pic%2F15%2F46%2F84%2F02R58PICEbB_1024.png", ttd._teamFlg));
            //    //StartCoroutine(ttd.LoadTopTeamPic("https://ss1.bdstatic.com/70cFuXSh_Q1YnxGkpoWK1HF6hhy/it/u=1459191346,3422494877&fm=26&gp=0.jpg", ttd._teamLogo));
            //    //ttd.SetTopTeamDate("就不让瞅", "就不让瞅", "15000", "0");
            //    SetTopdata(ttd, 2);
            //    //  _IAM.GetComponent<TopTeamDate>().SetTopTeamDate("就不让瞅", "瞅你咋地", "5000", "5");
            //    break;
            case "top":
                // int index = 0;
                //Debug.LogError("top");
                foreach (Transform item in _IAM.transform)
                {
                  
                    switch (item.name)
                    {
                        
                        case "title":
                            print("title");
                           
                            if (GameManager.Instance.TopScoreData.title!="" )
                            {
                                item.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = GameManager.Instance.TopScoreData.title;
                            }
                            else
                            {
                                item.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "Score Board";
                            }
                            break;
                        case "end":
                            print("end");
                            CreatTopScoreBoard();
                            break;
                        default:
                            break;
                    }
                     //if (ttd = item.GetComponent<TopTeamDate>())
                     //{
                     //    SetTopdata(ttd, index);

                    //    //Test
                    //    index++;
                    // //   StartCoroutine(ttd.LoadTopTeamPic("https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1504675236835&di=5f7ac10506fe0b40a0fd528ef16aded9&imgtype=0&src=http%3A%2F%2Fpic.58pic.com%2F58pic%2F14%2F54%2F54%2F98d58PICUYy_1024.png", ttd._teamFlg));
                    //    //StartCoroutine(ttd.LoadTopTeamPic("https://ss0.bdstatic.com/70cFvHSh_Q1YnxGkpoWK1HF6hhy/it/u=1957009170,1281898542&fm=200&gp=0.jpg", ttd._teamLogo));
                    //    //ttd.SetTopTeamDate("就不让瞅", "就不让瞅", "15000", "0");
                    //}




                    // Debug.LogError(item.name);

                }
                break;
            default:
                break;
        }
    }
   private   List <ScoreBoardData> Toplist;
    //private void SetTopdata(TopTeamDate ttd, int index)
    //{
    //   // StartCoroutine(ttd.LoadTopTeamPic(Toplist[index].teamFlag,ttd._teamFlg));
    //    StartCoroutine(ttd.LoadTopTeamPic(Toplist[index].teamLogo, ttd._teamLogo));
    //    ttd.SetTopTeamDate(Toplist[index].displayTeamName, Toplist[index].teamMvp, Toplist[index].score,Toplist[index].teamFirstBlood);
    //}
    private List<TopTeamDate> topTeamList;
    private void CreatTopScoreBoard()
    {
        Toplist = GameManager.Instance.TopScoreData.list;
        if (Toplist.Count==0)
        {
            return;
        }
        topTeamList = new List<TopTeamDate>();
        //print(topTeamList.Count);
        for (int i = 0; i < Toplist.Count ; i++)
        {
            GameObject obj = Instantiate(UIManager.Instance.gameUI.overScoreBoardPrefab,UIManager.Instance.gameUI.overScoreBoardPrant);
            TopTeamDate ttd = obj.GetComponent<TopTeamDate>();
            StartCoroutine(ttd.LoadTopTeamPic(Toplist[i].teamLogo, ttd._teamLogo));
            ttd.SetTopTeamDate(Toplist[i].teamName, Toplist[i].teamMvp, Toplist[i].score, Toplist[i].teamFirstBlood,Toplist[i].rank,i,Toplist[i].trend);
            topTeamList.Add(ttd);
            obj.gameObject.SetActive(false);
        }
        index = 0;
        InvokeRepeating("ShowScoreBoard",1.0f,0.2f);
    }
    private int index = 0;
    private void ShowScoreBoard()
    {       
            UIManager.Instance.gameUI.overScoreBoardPrant.GetChild(index).gameObject.SetActive(true);
            index++;
            if (index >= Toplist.Count)
            {
                this.CancelInvoke();
            }
        
      
       
    }
    public void DisPlayOverScore()
    {
        //print(topTeamList.Count);
        if (topTeamList!=null)
        {

            print(topTeamList.Count);
            for (int i = 0; i < topTeamList.Count; i++)
            {
                Destroy(topTeamList[i].gameObject);
            }
        }
        //  topTeamList = null; ;
    }
    private void OnDisable()
    {
        RemoveListener();
        //Debug.Log("Remove Top listener");
    }

    void HandleOnStartDisappear(InterfaceAnimManager _IAM) {
     //   Debug.Log("EVENT LISTENER : " + _IAM.name + " onStartDisappear");
        
    }
    void HandleOnEndAppear(InterfaceAnimManager _IAM) {
     //   Debug.Log("EVENT LISTENER : " + _IAM.name + " onEndAppear");
    }
    void HandleOnEndDisappear(InterfaceAnimManager _IAM) {
     //   Debug.Log("EVENT LISTENER : " + _IAM.name + " onEndDisappear");
    }
	
}
