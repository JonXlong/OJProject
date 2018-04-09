using UnityEngine;
//using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
//using System.Text;
using System.Text.RegularExpressions;
using System;

public class UIManager : MonoBehaviour {
    //add  StopcameraButtonPannel
	public enum UI_TYPE:int {
		All = 0,
		scoreboard = 1,
		messagebox = 2,
        stopCameraButton=3
	}

	public ScifiClockUI scifiClockUI;
	public Camera uiCamera;
	public CanvasGroup loadingBar;

	public GameUIScript gameUI;
	public static UIManager Instance;

	private List<string> logoUrls;
	private Dictionary<string, Texture2D> logoDict;

	// Use this for initialization
	void Awake () {
		Instance = this;
	}

	//void Start () {
	//}

	public void ShowLoadingBar() {
		loadingBar.gameObject.SetActive (true);
	}

	public void HideLoadingBar() {
		loadingBar.gameObject.SetActive (false);
	}

	public void PreloadLogos (List<string> _logoUrls) {
		logoUrls = _logoUrls;
		logoDict = new Dictionary<string, Texture2D> ();

		foreach (string url in logoUrls) {
			if (url.Length > 0) {
				StartCoroutine (LoadLogo (url, logoDict));
			}
		}
	}

	IEnumerator LoadLogo(string path, Dictionary<string, Texture2D> logoDict) {
		//WWW www = new WWW (path);
		//yield return www;
        //print(path);
        //		try {
        //			if (www.isDone && www.texture != null) {
        //                //www.texture.filterMode = FilterMode.Bilinear;  
        //                //www.texture.Apply();
        //				logoDict[path] = www.texture;
        //                //Debug.LogError("===========");
        ////				CheckAllLogoLoaded ();
        //			} else {
        //				Debug.Log ("Load texture failed : " + path);
        //			}
        //		} catch (Exception e) {
        //			Debug.Log ("Load texture failed : " + path + "\n(error):" + e.ToString());
        //		}
        WWW www = new WWW(path);
        yield return www;
        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.Log(www.error);
        }
        else
        {
            if (www.isDone)
            {
                Texture2D text = new Texture2D(64, 64);
                //Texture2D text = new Texture2D(4, 4, TextureFormat.DXT1, false);
                www.LoadImageIntoTexture(text);
                //Sprite sprite = Sprite.Create(tex, new Rect(0, 0,64,64), Vector2.zero);
                logoDict[path] =text;
               
            }
        }

    }

	private bool isURL(string url) {
		string regEx = "^(http|https|ftp)//://([a-zA-Z0-9//.//-]+(//:[a-zA-"
			+ "Z0-9//.&%//$//-]+)*@)?((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{"
			+ "2}|[1-9]{1}[0-9]{1}|[1-9])//.(25[0-5]|2[0-4][0-9]|[0-1]{1}"
			+ "[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)//.(25[0-5]|2[0-4][0-9]|"
			+ "[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)//.(25[0-5]|2[0-"
			+ "4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|([a-zA-Z0"
			+ "-9//-]+//.)*[a-zA-Z0-9//-]+//.[a-zA-Z]{2,4})(//:[0-9]+)?(/"
			+ "[^/][a-zA-Z0-9//.//,//?//'///////+&%//$//=~_//-@]*)*$";
		Regex reg = new Regex (regEx);
		return reg.IsMatch (url);
	}

//	private void CheckAllLogoLoaded() {
//		if (logoDict.Count >= logoUrls.Count) {
//			Debug.Log ("All logo images loaded :" + logoUrls.Count);
//			foreach (KeyValuePair<string, Texture2D> kv in logoDict) {
//				Debug.Log (kv.Key);
//			}
//		}
//	}

	public Texture2D GetLogoTexture2D(string logoURL) {
		Texture2D t = null;
      
		if (logoDict.ContainsKey(logoURL)) {
			t = logoDict [logoURL];
           
		}
		return t;
	}

	//// Update is called once per frame
	//void Update () {
		
	//}

}
