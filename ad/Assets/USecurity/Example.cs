using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.USecurity
{
    /// <summary>
    /// Usage example.
    /// </summary>
    public class Example : MonoBehaviour
    {
        public Text Text;
        public Text _text;

        /// <summary>
        /// Execute on start.
        /// </summary>
        public void Start()
        {
            const string sample = @"{""action"" : ""camera"",""type"" : ""command"",""param"":10}";

        Text.text = string.Format("Plain text: {0}\nMD5 hash: {1}\nBase64 encoding: {2}\nB64R encoding: {3}\nB64X encryption: {4}\nAES encryption: {5}",
                sample,
                Md5.ComputeHash(sample),
                Base64.Encode(sample),
                B64R.Encode(sample),
                B64X.Encrypt(sample, "password Hello, world!"),
                AES.Encrypt(sample, "13215555"));
            //const string test = '{ "action": "waitScene", "type": "command", "param": 10}';
            string mmstr = B64X.Encrypt(sample, "password Hello, world!");
            Debug.Log(mmstr);
             string test = "{'name':'jifeng','company':'taobao','value':++value}";

            _text.text = string.Format("Test :{0}\n :{1}\n",
            Base64.Encode(test),
            Base64.Decode(Base64.Encode(test)));

            string str = B64X.Decrypt(mmstr, "password Hello, world!");
            //SimCommandData dd = JsonUtility.FromJson<SimCommandData>(str);
            JSONObject jo = null;
            Debug.LogError(str);
            jo = JSONObject.Create(str,-2,false ,false  );
            SimCommandData scd = JsonUtility.FromJson<SimCommandData>(str);

          //  CreateMyJsonData cmd = CreateMyJsonData.GetMyJson(jo);
            Debug.LogError( scd.param);
        }
        string initData = "";
        string key = "";
        string encryption = "";
        string decrypt = "";
        
        private void OnGUI()
        {

            
            //未加密数据
              GUI.Label(new Rect(0, 0, 400, 40), "No  Encryption Data:");
            GUI.skin.label.fontSize = 30;
            GUI.skin.textField.fontSize = 26;
            initData = GUI.TextField(new Rect(0, 40, 1000, 40), initData);
            //加密的密匙
               GUI.Label(new Rect(0, 100, 200, 40), "Key:");
            key = GUI.TextField(new Rect(0, 140, 1000, 40), key);
            
           
            //加密后的数据
            GUI.Label(new Rect(0, 240, 240, 40), "Encryption Data");

            encryption = GUI.TextField(new Rect(0, 280, 1000, 40), encryption);

            //解密后的数据
            GUI.Label(new Rect(0, 330, 200, 40), "Decrypt Data:");
            decrypt = GUI.TextField(new Rect(0, 380, 1000, 40), decrypt);
            GUI.skin.button.fontSize = 22;
            if (GUI.Button(new Rect(1000,0, 200, 60), "Begin Encryption"))
            {
                //开始加密
                encryption = B64X.Encrypt(initData, key);
                print(encryption);
               
                //encryption = SimpleEncryption.EncryptString(initData);
            }
            if (GUI.Button(new Rect(1000,140,200,60),"Decrypt Data"))
            {
                decrypt = AES.Decrypt(encryption,key);
                //decrypt = B64X.Decrypt(encryption, key);
                //JSONObject js = JSONObject.Create(decrypt);
                //print(js);
                //CreateMyJsonData cmd = CreateMyJsonData.GetMyJson(js);
                //print(cmd.param);
                //decrypt = SimpleEncryption.DecryptString(encryption);
            }

        }
    }
  
    public class CreateMyJsonData
        {
        public string action;
        public string type;
        public string  param;
        public static CreateMyJsonData GetMyJson(JSONObject js)
        {
            CreateMyJsonData cmd = new CreateMyJsonData();
            cmd.action = js.GetField("action").str;
            cmd.type = js.GetField("type").str;
            cmd.param = js.GetField("param").str;
            return cmd;
        }

}
    public class SimCommandData
    {
        public const string ACTION_scene = "scene";
        public const string ACTION_mute = "mute";
        public const string ACTION_ui_all = "ui_all";
        public const string ACTION_ui_scoreboard = "ui_socreboard";
        public const string ACTION_ui_messagebox = "ui_messagebox";
        public const string ACTION_camera = "camera";
        public const string ACTION_timer = "timer";
        public const string ACTION_reset_units = "reset_units";
        public const string ACTION_set_round = "set_round";
        public const string ACTION_ui_bottom = "ui_bottom";

        public string type;
        public string action;
        public string param;
    }
}
