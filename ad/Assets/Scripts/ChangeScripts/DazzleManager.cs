using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DazzleManager : MonoBehaviour {

    // Use this for initialization
    public static DazzleManager Instance;
    public GameObject DazzleObj;
    public float DalteTime = 5.0f;
    public DazzleRoundTxt DazzleRoundTxt;
    private Transform Canvas3DP;
    private string StrLeft, StrRight;
    private void Awake()
    {
        Instance = this;
    }
    void Start () {
        StartCoroutine(ShowDazzleObj(DalteTime));
	}
  public   void AddRoudString(string strLeft,string strRight) {
        StrLeft = strLeft;StrRight = strRight;
    }

  IEnumerator ShowDazzleObj(float _time)
    {
        yield return new WaitForSeconds(_time);
        DazzleObj.SetActive(true);
        Canvas3DP= GameObject.Find("UI/Canvas3D").transform;
        foreach (Transform item in DazzleObj.transform)
        {
            Debug.Log(item.transform.name);
            yield return new WaitForSeconds(0.2f);
            item.gameObject.SetActive(true);
        }
        yield return new WaitForSeconds(0.1f);
        DazzleRoundTxt = (Instantiate(GameManager.Instance.virtualDazzleRound) as GameObject).GetComponent<DazzleRoundTxt>();
        DazzleRoundTxt.DazzleObj = DazzleObj;
        DazzleRoundTxt.DazzleRect = DazzleRoundTxt.GetComponent<RectTransform>();
        DazzleRoundTxt.transform.SetParent(Canvas3DP);
        DazzleRoundTxt.DazzleRect.anchoredPosition = Vector2.zero;
        Vector3 Vect3Dazzle = new Vector3(0,1.0f,0);
        DazzleRoundTxt.OffsetV3 = Vect3Dazzle;
        DazzleRoundTxt.RoundTxtLeft = DazzleRoundTxt.transform.FindChild("RoundText").GetComponent<Text>();
        DazzleRoundTxt.RoundTxtRight = DazzleRoundTxt.transform.FindChild("RoundTypeText").GetComponent<Text>();
        DazzleRoundTxt.RoundTxtLeft.text = StrLeft;
        DazzleRoundTxt.RoundTxtRight.text = StrRight;
       
        DazzleRoundTxt.transform.position = gameObject.transform.position;
        DazzleRoundTxt.IsRound = false;
        if (!DazzleRoundTxt.IsRound)
        {
            DazzleRoundTxt.transform.position += Vect3Dazzle;
            //				hudText.logoImage.transform.eulerAngles = new Vector3(90f, 0f, 0f);
            DazzleRoundTxt.DazzleRect.localScale = new Vector3(-0.03f, 0.03f, 1f);
          //  DazzleRoundTxt.transform.localEulerAngles = new Vector3(90f, 90f, 0f);
        }
        //else
        //{
        //    DazzleRoundTxt.transform.localScale = new Vector3(-0.03f, 0.03f, 1f);
        //}
    }
    // Update is called once per frame
    //void Update () {
    //       DalteTime -= Time.deltaTime;
    //       if (DalteTime<0)
    //       {
    //           DazzleObj.SetActive(true);
    //       }
    //}
}
