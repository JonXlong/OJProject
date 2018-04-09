using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TopTeamDate : MonoBehaviour {
    //public Image _teamFlg;
    public Image _teamLogo;
    public Text _teamName;
    //public Text _teamMvp;
    public Text _teamScore;
    public Text _teamRank;
    public Image statusImg;
    public Sprite spriteUp;
    public Sprite spriteDown;
    public Sprite spriteKeep;
    //public Text _teamFirstBlood;
    private void Awake()
    {
        _teamRank = transform.FindChild("teamRank").GetComponent<Text>();
      //  _teamFlg = transform.FindChild("teamFlag").GetComponent<Image>();
        _teamLogo = transform.FindChild("teamLogo").GetComponent<Image>();
        _teamName = transform.FindChild("teamName").GetComponent<Text>();
        //_teamMvp = transform.FindChild("teamMvp").GetComponent<Text>();
        _teamScore = transform.FindChild("teamScore").GetComponent<Text>();

        //_teamFirstBlood = transform.FindChild("teamFirstblood").GetComponent<Text>();
    }
    public void SetTopTeamDate(string teamNameStr, string teamMvpStr, string teamScoreStr, string teamFirstBloodStr,string teamRankStr,int i,string trend)
    {
        _teamName.text = "";
        _teamName.text = teamNameStr;
        //_teamMvp.text = "";
       // _teamMvp.text = teamMvpStr;_teamScore.text = "";
        _teamScore.text = teamScoreStr;
        _teamRank.text = "";
        _teamRank.text = teamRankStr;
      
          

            if (trend == "up")
            {
                statusImg.sprite = spriteUp;
            }
            else if (trend == "down")
            {
                statusImg.sprite = spriteDown;
            }
            else
            {
                statusImg.sprite = spriteKeep;
            }
      
        if (i<3)
        {
            //todo getcolor
            _teamRank.color = UIManager.Instance.gameUI.GetScoreColor(i);
            _teamName.color = UIManager.Instance.gameUI.GetScoreColor(i);
            _teamScore.color = UIManager.Instance.gameUI.GetScoreColor(i);
        }
        //_teamFirstBlood.text = ""; 
       // _teamFirstBlood.text = teamFirstBloodStr;
    }
    //void OnEnable()
    //{
    //    SetTopTeamDate(GameManager.Instance._teamName,GameManager.Instance._teamMvp,"5000","100");
    //}
    /// <summary>
    /// if(picUrl.Contains("png、jpg、jpeg"))
    /// </summary>
    /// <param name="picUrl"></param>
    /// <param name="image"></param>
    /// <returns></returns>
    //todo find of picUrl if load picture
  public   IEnumerator LoadTopTeamPic(string picUrl,Image image)
    {
       // print(picUrl);
        if (string .IsNullOrEmpty(picUrl ))
        {
            yield break;
        }
        WWW w = new WWW(picUrl);
        yield return w;
        if (string.IsNullOrEmpty(w.error))
        {
            image.sprite = GetSP(w.texture);
        }
        else
        {
            yield  break;
        }
    }
    Sprite GetSP(Texture2D text2d)
    {
        Sprite sp;
        sp = Sprite.Create(text2d, new Rect(0,0,text2d.width,text2d.height),Vector2.zero,100.0f,0,SpriteMeshType.FullRect);
        return sp;
    }
   
	
	// Update is called once per frame
	void Update () {
		
	}
}
