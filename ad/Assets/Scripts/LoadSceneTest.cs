using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LoadSceneTest : MonoBehaviour {

    // Use this for initialization
    public Text loadText;
    public float  normalI;
    public Image _image;
    AsyncOperation asyO;
    int press = 0;
	void Start () {
        DontDestroyOnLoad(gameObject);


      StartCoroutine(loadScene("Main"));
        
	}
    //IEnumerator LoadSce()
    //{
    //           asyO  =  Application.LoadLevelAsync(1);
    //    yield return asyO;
    //}
	// Update is called once per frame
	void Update () {
        if (asyO != null && !asyO.isDone)
        {
            _image.fillAmount = asyO.progress;

            loadText.text = asyO.progress * 100 + "%";
           // GUILayout.Label("progress:" + (float)asyO.progress * 100 + "%");
        }
    }
  

   // void OnGUI()
   // {
   //     //开始加载场景按钮  
   //     if (GUILayout.Button("Start Load Scene"))
   //     {
   //         StartCoroutine(loadScene("Main"));
   //     }

   //     //判断异步对象并且异步对象没有加载完毕，显示进度  
   ///*     if (asyO != null && !asyO.isDone)
   //     {
   //         _image.fillAmount = asyO.progress;

   //         loadText.text = asyO.progress * 100 + "%";
   //         GUILayout.Label("progress:" + (float)asyO.progress * 100 + "%");
   //     }*/
   // }

    IEnumerator loadScene(string sceneName)
    {
        yield return asyO = Application.LoadLevelAsync(sceneName);
        print("load Complete!");
    }
}
