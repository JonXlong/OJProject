using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SimVirtualWorld : SimWorldBase {

	public SimCamera cameraCircle1;
	public SimCamera cameraCircle2;
	public SimCamera cameraCircle3;

	public Transform puzzlesTrans;

	public GameObject earth;

	private Dictionary<string, SimUnitBase> puzzlesDict = new Dictionary<string, SimUnitBase> ();

	private int currentCircle = 1;

	private string[] randomPuzzlesList = new string[8]{"Puzzle_1","Puzzle_2","Puzzle_3","Puzzle_4","Puzzle_5","Puzzle_6","Puzzle_7","Puzzle_8"};

	void Awake () {
        cameraCircle1.gameObject.SetActive (false  ); // change true
		cameraCircle2.gameObject.SetActive (false);
		cameraCircle3.gameObject.SetActive (false);

		earth.gameObject.SetActive (false);
	}

	public override void CreateWorld (List<SimTeamData> teamsData) {
		
		// Initialize teams data
		foreach (SimTeamData teamData in teamsData) {
			GameObject go = new GameObject (teamData.team);
			go.transform.SetParent (gameObject.transform);

			SimTeamBase simTeam = go.AddComponent<SimTeamVirtual>();
			simTeam.teamData = teamData;
			simTeam.CreateUnits ();
			simTeams.Add (simTeam);
		}

		CreateArbiter ();

		if (simTeams.Count > 0) {
			ArrangeTeams ();
		}
	}

	public void CreatePuzzles (List<JSONObject> puzzleObjs) {
        List<Transform> puzzlesList = new List<Transform>();

        foreach (JSONObject puzzleJson in puzzleObjs) {
			SimPuzzleData puzzleData = SimPuzzleData.CreateFromJSON (puzzleJson);

			string prefabName = "Puzzles/" + puzzleData.type;
			GameObject prefab = Resources.Load<GameObject> (prefabName);

			if (prefab == null) {
				prefab = Resources.Load<GameObject> ("Puzzles/" + randomPuzzlesList[UnityEngine.Random.Range(0, randomPuzzlesList.Length)]);
			}

			GameObject simUnitGo = ((GameObject)Instantiate (prefab, puzzlesTrans));
			SimUnitVirtual simUnit = simUnitGo.AddComponent<SimUnitVirtual>();
            
//			Color puzzleColor = puzzleData.getColor;
//			Renderer renderer = simUnitGo.GetComponentInChildren<Renderer> ();
//			if (renderer) {
//				renderer.material.SetColor ("_CristalColor", puzzleColor);
//			}
			simUnit.puzzleData = puzzleData;

			puzzlesDict[puzzleData.name] = simUnit;
            
            puzzlesList.Add(simUnitGo.transform);
            SimWorldManager.Instance.puzzlesDicTrans.Add(puzzleData.name, simUnitGo.transform);

        }

        if (puzzlesList.Count <= 10) {
            //simUnits.GetRange(1, simUnits.Count - 1).ToArray();   
            //change  puzzle position  Y  10,5,0
            CalualteCircleRing(puzzlesList, 10f, 5f);
        } else {
            // CalualteSphere(puzzlesList);
            CalualteCircleRing(puzzlesList.GetRange(0, puzzlesList.Count / 2), 5f, 5f);
            CalualteCircleRing(puzzlesList.GetRange(puzzlesList.Count / 2, puzzlesList.Count - puzzlesList.Count / 2), 0f, 5f);
        }
        
    }

    //private float circleSize = 5f;
    private void CalualteSphere(List<Transform> puzzles, float circleSize) {
        int num = puzzles.Count;

        float inc = Mathf.PI * (3.0f - Mathf.Sqrt(5.0f));
        float off = 2.0f / (float)num;
        float y, r, phi;

        for (int i = 0; i < num; i++) {
            Transform puzzleTrans = puzzles[i];
            y = (float)i * off - 1.0f + (off / 2.0f);
            r = Mathf.Sqrt(1.0f - y * y);
            phi = (float)i * inc;
            puzzleTrans.localPosition = new Vector3(Mathf.Cos(phi) * r * circleSize, y * circleSize, Mathf.Sin(phi) * r * circleSize);
        }
    }
    
    private void CalualteCircleRing(List<Transform> puzzlesAry, float yOffset, float circleSize) {
        int num = puzzlesAry.Count;
        int steps = puzzlesAry.Count;
        float stepRadius = 360f / steps;
        
        for (int i = 0; i < num; i++) {
            Transform puzzleTrans = puzzlesAry[i];
            float angle = (i * stepRadius) * Mathf.Deg2Rad;
            puzzleTrans.localPosition = new Vector3(circleSize * Mathf.Cos(angle), yOffset, circleSize * Mathf.Sin(angle));
        }
    }

    // Arrange Teams
    protected override void ArrangeTeams() {
		int teamsCount = simTeams.Count;

		if (teamsCount <= GameConfigs.Instance.circle_1_max) {
			currentCircle = 1;

			CircleArrange (simTeams, GameConfigs.Instance.circle_1_radii, GameConfigs.Instance.circle_1_angle);
		} else if (teamsCount <= GameConfigs.Instance.circle_1_max + GameConfigs.Instance.circle_2_max) {
			currentCircle = 2;

			int diffNum = (teamsCount - GameConfigs.Instance.circle_1_min - GameConfigs.Instance.circle_2_max);
			int cir1Num = GameConfigs.Instance.circle_1_min + (diffNum > 0 ? diffNum : 0);
			int cir2Num = teamsCount - cir1Num;

			CircleArrange (simTeams.GetRange(0, cir1Num), GameConfigs.Instance.circle_1_radii, GameConfigs.Instance.circle_1_angle);
			CircleArrange (simTeams.GetRange(cir1Num, cir2Num), GameConfigs.Instance.circle_2_radii, GameConfigs.Instance.circle_2_angle);
		} else if (teamsCount <= GameConfigs.Instance.circle_1_max + GameConfigs.Instance.circle_2_max + GameConfigs.Instance.circle_3_max) {
			currentCircle = 3;

			int cir1Num = GameConfigs.Instance.circle_1_max;
			int diffNum = (teamsCount - GameConfigs.Instance.circle_1_max - GameConfigs.Instance.circle_2_min - GameConfigs.Instance.circle_3_max);
			int cir2Num = GameConfigs.Instance.circle_2_min + (diffNum > 0 ? diffNum : 0);
			int cir3Num = teamsCount - cir2Num - cir1Num;

			CircleArrange (simTeams.GetRange(0, cir1Num), GameConfigs.Instance.circle_1_radii, GameConfigs.Instance.circle_1_angle);
			CircleArrange (simTeams.GetRange(cir1Num, cir2Num), GameConfigs.Instance.circle_2_radii, GameConfigs.Instance.circle_2_angle);
			CircleArrange (simTeams.GetRange(cir1Num + cir2Num, cir3Num), GameConfigs.Instance.circle_3_radii, GameConfigs.Instance.circle_3_angle);
		} else {
			Debug.LogError ("Team number out of capacity : " + GameConfigs.Instance.circle_1_max + GameConfigs.Instance.circle_2_max + GameConfigs.Instance.circle_3_max);
		}

		switch (currentCircle) {
		case 1:
			activeCamera = cameraCircle1;
			arbiter.gameObject.transform.position += new Vector3 (0, GameConfigs.Instance.circle_1_arbitY, 0);
			break;
		case 2:
			activeCamera = cameraCircle2;
			arbiter.gameObject.transform.position += new Vector3 (0, GameConfigs.Instance.circle_2_arbitY, 0);
			break;
		case 3:
			activeCamera = cameraCircle3;
			arbiter.gameObject.transform.position += new Vector3 (0, GameConfigs.Instance.circle_3_arbitY, 0);
			break;
		default:
			Debug.LogError ("Teams cricles more than 3 rounds.");
			break;
		}
	}

	private void CircleArrange(List<SimTeamBase> teams, float radius, float startAngle) {
		float xpos, zpos, angle;
		int steps = teams.Count;
		float stepRadius = 360f / steps;

		int i = 0;

		foreach (SimTeamBase team in teams) {
			angle = (i * stepRadius + startAngle) * Mathf.Deg2Rad;
			xpos = radius * Mathf.Cos (angle);
			zpos = radius * Mathf.Sin (angle);

			team.gameObject.transform.position = new Vector3 (xpos, 0, zpos);
			team.gameObject.transform.LookAt (Vector3.zero);

			i++;

			team.ArrangeUnits ();

			team.InitializeTeam ();
		}
	}

	public override SimUnitBase FindPuzzle (string uid) {
		SimUnitBase unit = null;
		if (puzzlesDict.ContainsKey (uid)) {
			unit = puzzlesDict [uid];
		}
		return unit;
	}

	protected override void OnShowWorld () {
		earth.SetActive (false);
	}

	protected  override void CreateArbiter () {
		GameObject prefab = SimWorldManager.Instance.getSimObjectPrefabByName (GameConfigs.virtual_arbiter);
		arbiter = ((GameObject)Instantiate (prefab, this.transform)).GetComponent<SimArbiterVirtual>();

		if (!GameConfigs.Instance.show_npc) {
            if (arbiter.model != null) {
                arbiter.model.gameObject.SetActive(false);
            }
        }
	}

	public void DebugAttack() {
		DebugRandomTeamAttack (1);
	}

	private void DebugRandomTeamAttack(int times) {
//		List<GameObject> missileObjects = new List<GameObject> ();
//		missileObjects.Add (GameManager.Instance.missileVirtualWeak);
//		missileObjects.Add (GameManager.Instance.missileVirtualMid);
//		missileObjects.Add (GameManager.Instance.missileVirtualHeavy);
//		missileObjects.Add (GameManager.Instance.missileVirtualCharge);
//
//		for (int i = 0; i < times; i++) {
//			ArrayList teamList = new ArrayList (simTeams);
//
//			int team1Index = Random.Range (0, teamList.Count);
//			SimTeamBase teams1 = teamList[team1Index] as SimTeamBase;
//			SimUnitBase unit1 = teams1.simUnits.ToArray()[Random.Range (0, teams1.simUnits.Count)];
//
//			teamList.RemoveAt (team1Index);
//			SimTeamBase teams2 = teamList[Random.Range (0, teamList.Count)] as SimTeamBase;
//			SimUnitBase unit2 = teams2.simUnits.ToArray()[0];
//
//			GameObject randMissile = missileObjects.ToArray()[Random.Range(0, missileObjects.Count)];
//			unit1.ApplyBehavior (unit2, randMissile);
//		}
	}

    /// <summary>
    /// 2017.05.08
    /// To Change  ：Add dazzle light  on Arbiter to show RoundNumber.
    /// </summary>
    public  override void CreatDazzleLight(string strLeft,string strRight)
    {
        //if (DazzleObject)
        //{
        //    Destroy(DazzleObject.GetComponent<DazzleManager>().DazzleRoundTxt.gameObject);
        //    Destroy(DazzleObject);

        //}
        //GameObject prefab = SimWorldManager.Instance.getSimObjectPrefabByName(GameConfigs.virtual_dazzle);
        //DazzleObject = Instantiate(prefab,this.transform);
        //DazzleObject.GetComponent<DazzleManager>().AddRoudString(strLeft,strRight);
        //DazzleObject.transform.position = new Vector3(arbiter.transform.position.x,arbiter.transform.position.y-5,arbiter.transform.position.z);
        //DazzleObject.transform.name = GameConfigs.virtual_dazzle;
        //Debug.Log(DazzleObject.transform.name+"  :this  Postion :"+DazzleObject.transform.position);


    }
}