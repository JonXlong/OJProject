using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using Assets.USecurity;

public class GameWebSocket : MonoBehaviour {
	
	public   WebSocket ws;
	private SendRequestData requestConfigAndMap;
	private SendRequestData requestMap;
	private SendRequestData requestConfig;
    private SendRequestSlideData requestSlideData;
    private SendRequestExplainData requestExplainData=new SendRequestExplainData();
    public string companyLogoUrl;
	private bool firstRequest = true;
	private string socketURL;

	void Start () {
	}
    private string channel_id;
	public void WebsocketLaunch(string url) {
		socketURL = url;
        channel_id = GameManager.Instance.channelId.Substring(GameManager.Instance.channelId.LastIndexOf("/") + 1);
        requestConfigAndMap = new SendRequestData ();
		requestConfigAndMap.type = "request";
		requestConfigAndMap.data = "setting_and_map";

		requestMap = new SendRequestData ();
		requestMap.type = "request";
		requestMap.data = "map";

		requestConfig = new SendRequestData ();
		requestConfig.type = "request";
		requestConfig.data = "setting";

		firstRequest = true;

		StartCoroutine (StartGetMessage());
	}

	// Use this for initialization
	IEnumerator StartGetMessage () {
		ws = new WebSocket(new Uri(socketURL));
		yield return ws.Connect();

		if (firstRequest) {
			ws.SendString(requestConfigAndMap.toJsonStr());
			firstRequest = false;
		}

		while (true) {
            // send slide data message
            if (GameManager.Instance.requestSlideData)
            {
                requestSlideData = new SendRequestSlideData();
                requestSlideData.type = "progress_request";
                requestSlideData.channel_id = channel_id;  // "AD_1_REPLAY";
                //Debug.Log(requestSlideData.channel_id);
                requestSlideData.value = GameManager.Instance.slideValue;
                ws.SendString(requestSlideData.toJsonStr());
                //print(requestSlideData.toJsonStr());
                GameManager.Instance.requestSlideData = false;

            }
            //send  request team explain data
            if (GameManager.Instance.requestTeamData)
            {
                requestExplainData = new SendRequestExplainData();
                requestExplainData.type = "get_data";
                requestExplainData.data = "teamdata_request";
                requestExplainData.id = GameManager.Instance.requestId;
                requestExplainData.channel_id = channel_id;
                ws.SendString(requestExplainData.toJsonStr());
                //Debug.Log(requestExplainData.toJsonStr());
                GameManager.Instance.requestTeamData = false;
            }
            //send request title explain data
            if (GameManager.Instance.requestTitleData)
            {
                requestExplainData = new SendRequestExplainData();
                requestExplainData.type = "get_data";
                requestExplainData.data = "titledata_request";
                requestExplainData.id = GameManager.Instance.requestId;
                requestExplainData.channel_id = channel_id;
                ws.SendString(requestExplainData.toJsonStr());
                GameManager.Instance.requestTitleData = false;
            }
            string received = ws.RecvString();
            JSONObject j = null;
           // Debug.Log(received);
            if (received != null && received != "") {
                //Debug.Log("Received: " + received);
                string strDecrpt = AES.Decrypt(received, "J#1AD^5k@kdu6uUch3$n^RO!$Ciy2g4z");
                j = JSONObject.Create(strDecrpt);

                print(strDecrpt);
                if (j == null || j.type == JSONObject.Type.NULL) {
                    Debug.LogError("JSON data error: " + strDecrpt);
                } else
                {
                    if (j.GetField("logo"))
                    {
						Debug.Log ("..............");
                        companyLogoUrl = j.GetField("logo").str;
                        if (isFirstLoadLogo)
                        {
                            StartCoroutine(LoadCompanyLogo(companyLogoUrl, UIManager.Instance.gameUI.companyImage)); 
                            isFirstLoadLogo = false;
                          
                        }
                       
                    }
                    GameManager.Instance.SocketMessageHandler(j, strDecrpt);
                    
                 
                }
			}

			if (ws.error != null && (j == null || j.type == JSONObject.Type.NULL)) {
				//Debug.LogError ("Error: " + ws.error + ", " + received);
				break;
			} else {
				UIManager.Instance.HideLoadingBar ();
			}
          
//			Debug.Log (received);
			yield return 0;
		}

		ws.Close ();
		UIManager.Instance.ShowLoadingBar ();
		StartCoroutine (StartGetMessage());
	}
    bool isFirstLoadLogo = true;
    IEnumerator LoadCompanyLogo(string url, Image image)
    {

        WWW w = new WWW(url);
        yield return w;
        try
        {
            if (string.IsNullOrEmpty(w.error))
            {
                //Debug.Log("---------");
                Texture2D texture = w.texture;
                Sprite sp = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                image.sprite = sp;

                image.gameObject.SetActive(true);
            }
            else
            {
                //Debug.Log("Load texture failed : " + url);
            }
        }
        catch (Exception)
        {


        }
    }
    void Update () {
		if (Application.internetReachability == NetworkReachability.NotReachable) {
			UIManager.Instance.ShowLoadingBar ();
		} else {
			UIManager.Instance.HideLoadingBar ();
		}
	}

//	public void CloseWebsocket() {
//		Debug.Log ("Websocket closed!!!");
//		ws.Close();
//	}

//	void OnApplicationQuit() {
//		if (ws != null) {
//			Debug.Log ("Websocket closed!!!");
//			ws.Close();
//		}
//	}

}