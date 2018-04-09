using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DebugManager : MonoBehaviour {

	public bool debugEnable;
	public Text dispMsg;
	public Button prevButton;
	public Button nextButton;
	public GameObject debugUI;

	[HideInInspector]
	public int curIndex = 0;
	public string[] debugConfigs;

	private List<SimBehaviorData> testBehaviors;
	private int behaviorIndex = 0;
	private string curFile;

	private string ConfigPath;
	private string ConfigSettings;
	private string ConfigTeams;
	private bool isConfigLoaded = false;

	// Use this for initialization
	void Start () {
	}

	public void EnableDebug () {
		if (debugEnable) {
			prevButton.onClick.AddListener (delegate() {
				this.OnClickPrev();
			});
			nextButton.onClick.AddListener (delegate() {
				this.OnClickNext();
			});

			ConfigPath = "file://" + Application.streamingAssetsPath;

			StartCoroutine(LoadGameSettings());
		}
	}

	IEnumerator LoadGameSettings() {
		WWW www = new WWW (ConfigPath + "/GameSettings.json");
		yield return www;

		if (www.isDone) {
			ConfigSettings = www.text;
			JsonUtility.FromJsonOverwrite (ConfigSettings, GameConfigs.Instance);
			//StartCoroutine(LoadTeamSettings());
		} else {
			Debug.LogError ("Load data failed : GameSettings.json");
		}
	}

	IEnumerator LoadTeamSettings() {
		WWW www = new WWW (ConfigPath + "/Teams.json");
		yield return www;

		if (www.isDone) {
			ConfigTeams = www.text;

			SimWorldManager.Instance.CreateWorld (JSONObject.Create (ConfigTeams));
			GameManager.Instance.SetGameMode (GameMode.GM_Vritual);
			GameManager.Instance.initialized = true;

			StartCoroutine(LoadConfig (debugConfigs[curIndex]));

		} else {
			Debug.LogError ("Load data failed : Teams.json");
		}
	}

	public void OnClickPrev () {
		curIndex--;
		if (curIndex <= 0) {
			curIndex = debugConfigs.Length - 1;
		}
		StartCoroutine(LoadConfig (debugConfigs[curIndex]));
	}

	public void OnClickNext () {
		curIndex++;
		if (curIndex >= debugConfigs.Length) {
			curIndex = 0;
		}
		StartCoroutine(LoadConfig (debugConfigs[curIndex]));
	}

	IEnumerator LoadConfig(string fileName) {
		curFile = fileName;

		string ConfigPath = "file://" + Application.streamingAssetsPath;

		WWW www = new WWW (ConfigPath + "/" + fileName);
		yield return www;

		if (www.isDone) {
			isConfigLoaded = true;
			dispMsg.text = fileName + " loaded!";

			JSONObject jsonObj = JSONObject.Create (www.text);
			List<JSONObject> jsonlist = jsonObj.GetField ("list").list;

			testBehaviors = new List<SimBehaviorData> ();
			foreach (JSONObject job in jsonlist) {
				testBehaviors.Add (SimBehaviorData.CreateFromJSON(job));
			}

			behaviorIndex = 0;
		} else {
			Debug.LogError ("Load data failed : Behavior.json");
		}
	}

	// Update is called once per frame
	void Update () {
		if (debugEnable && isConfigLoaded) {
			debugUI.SetActive (true);

			if (GameManager.Instance.initialized) {
				if(Input.GetMouseButtonDown(0)) {
					// Random Attack
					// SimWorldManager.Instance.DebugAttack ();

					// Random apply attack behavior
					ArrayList ary = new ArrayList (testBehaviors);
					SimBehaviorData bd = ary [behaviorIndex] as SimBehaviorData;
					SimWorldManager.Instance.ApplyBehavior (bd);

					dispMsg.text = curFile + ": (" + bd.id + ")";

					behaviorIndex++;
					if (behaviorIndex >= testBehaviors.Count) {
						behaviorIndex = 0;
					}

//					int randIndex = Random.Range(0, testBehaviors.Count);
//					SimWorldManager.Instance.ApplyBehavior (ary[randIndex] as SimBehaviorData);
				}
			}

		} else {
			debugUI.SetActive (false);
		}

	}

}
