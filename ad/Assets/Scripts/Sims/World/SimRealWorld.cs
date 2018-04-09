using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SimRealWorld : SimWorldBase {

    public SimCamera cameraCircle1;
    public SimCamera cameraCircle2;
    public SimCamera cameraCircle3;

    public SimCamera cameraCircle;
    public Transform worldlinkLinesTrans;
    public Transform floorTrans;

    private RealLinkDot arbiterDot;
    private DrawLineEX drawLineEX;
    private Dictionary<string, int> puzzlesDict = new Dictionary<string, int>();

    void Awake () {
        cameraCircle1.gameObject.SetActive(false);
        cameraCircle2.gameObject.SetActive(false);
        cameraCircle3.gameObject.SetActive(false);
    }

	public override void CreateWorld (List<SimTeamData> teamsData) {
		
		drawLineEX = gameObject.AddComponent<DrawLineEX> ();

		// Initialize teams data
		foreach (SimTeamData teamData in teamsData) {
			GameObject go = new GameObject (teamData.team);
            //print(teamData.team);
            //print(gameObject.transform.name);
			go.transform.SetParent (gameObject.transform);

			SimTeamBase simTeam = go.AddComponent<SimTeamReal>();
			simTeam.teamData = teamData;
			simTeam.CreateUnits ();
			simTeams.Add (simTeam);
		}

		CreateArbiter ();

		if (simTeams.Count > 0) {
            ArrangeTeams();
        }
     //   Debug.LogError(GameConfigs.Instance.real_floor_scale);
        floorTrans.localScale = new Vector3(GameConfigs.Instance.real_floor_scale, 1, GameConfigs.Instance.real_floor_scale);

        activeCamera = cameraCircle;
	}

    public void CreatePuzzles(List<JSONObject> puzzleObjs) {
     //   Debug.Log(puzzleObjs);
        for (int i = 0; i < puzzleObjs.Count; i++) {
            SimPuzzleData puzzleData = SimPuzzleData.CreateFromJSON(puzzleObjs[i]);
            puzzlesDict[puzzleData.name] = i % 4;
          //  Debug.Log("--puzzleDicCount:" + puzzlesDict.Count + "....TitleName  :" + puzzleData.name + "  ——————  :" + puzzlesDict[puzzleData.name]);
        }

        if (puzzleObjs.Count == 0) {
            SimArbiterReal realArbiter = arbiter as SimArbiterReal;
            realArbiter.HidePuzzleItems();
        }
    }

    public enum ArrageDIR:int
	{
		left = 0,
		right = 1,
		top = 2,
		bottom = 3
	}

	// Arrange Teams
	protected override void ArrangeTeams() {
		if (simTeams.Count <= 4) {
			ArrangeTeamsLE4 ();
		} else {
			ArrangeTeamsGT4 ();
		}
	}
    //给队伍排列 和画线
	private void ArrangeTeamsLE4() {
		List<SimTeamBase> l = new List<SimTeamBase> ();
		List<SimTeamBase> r = new List<SimTeamBase> ();
		List<SimTeamBase> t = new List<SimTeamBase> ();
		List<SimTeamBase> b = new List<SimTeamBase> ();

		int i = 0;
		foreach (SimTeamBase team in simTeams) {
			if (i > 3) {
				i = 0;
			}

			switch (i) {
			case 0:
				l.Add (team);
				break;
			case 1:
				r.Add (team);
				break;
			case 2:
				t.Add (team);
				break;
			case 3:
				b.Add (team);
				break;
			}

			i++;
		}

//		int maxSideLen = (int)Mathf.Ceil(simTeams.Count / 4f);

		ArrageTeamSide (l, ArrageDIR.left, 180f, 10f, 4, false);
		ArrageTeamSide (r, ArrageDIR.right, 0f, 10f, 4, false);
		ArrageTeamSide (t, ArrageDIR.top, 90f, 10f, 4, false);
		ArrageTeamSide (b, ArrageDIR.bottom, 270f, 10f, 4, false);
	}

	private void ArrangeTeamsGT4() {
		List<SimTeamBase> l = new List<SimTeamBase> ();
		List<SimTeamBase> r = new List<SimTeamBase> ();
		List<SimTeamBase> t = new List<SimTeamBase> ();
		List<SimTeamBase> b = new List<SimTeamBase> ();

		float lengthPerTeam = (GameConfigs.Instance.real_max_countX + 2) * GameConfigs.Instance.real_unit_gap;

		int i = 0;
		foreach (SimTeamBase team in simTeams) {
			switch (i % 6) {
			case 0:
				t.Add (team);
				break;
			case 1:
				b.Add (team);
				break;
			case 2:
				l.Add (team);
				break;
			case 3:
				r.Add (team);
				break;
			case 4:
				t.Add (team);
				break;
			case 5:
				b.Add (team);
				break;
			}

			i++;
		}

		ArrageTeamSide (l, ArrageDIR.left, 180f, 10f, 1, true);
		ArrageTeamSide (r, ArrageDIR.right, 0f, 10f, 1, true);

		int maxRow = 4;
//		int teamsCount = simTeams.Count;

		ArrageTeamSide (t, ArrageDIR.top, 90f, lengthPerTeam + 2f, maxRow, true);
		ArrageTeamSide (b, ArrageDIR.bottom, 270f, lengthPerTeam + 2f, maxRow, true);
	}

	private void ArrageTeamSide (List<SimTeamBase> sideTeams, ArrageDIR dir, float degree, float radius, int wrapCount, bool isSerialLink) {
		int teamsCount = sideTeams.Count;

		if (teamsCount == 0) {
			return;
		}

		string dirStr = "";
		switch (dir) {
		case ArrageDIR.left:
			dirStr = "S_left";
			break;
		case ArrageDIR.right:
			dirStr = "S_right";
			break;
		case ArrageDIR.top:
			dirStr = "S_top";
			break;
		case ArrageDIR.bottom:
			dirStr = "S_bottom";
			break;
		}

		GameObject sideContainer = new GameObject (dirStr);
        GameObject sideLinkLines = new GameObject ("LinkLines");
        sideContainer.transform.SetParent (transform);
        sideLinkLines.transform.SetParent(sideContainer.transform, true);

        float lengthPerTeam = (GameConfigs.Instance.real_max_countX + 5) * GameConfigs.Instance.real_unit_gap;
        //float lengthPerTeam = 0f;
        //foreach (SimTeamBase team in sideTeams) {
        //    SimTeamReal realTeam = team as SimTeamReal;
        //    // (GameConfigs.Instance.real_max_countX + 5) * GameConfigs.Instance.real_unit_gap;
        //    lengthPerTeam += realTeam.TeamXSize + 1 * GameConfigs.Instance.real_unit_gap;
        //}

        int rowCount = sideTeams.Count <= wrapCount ? sideTeams.Count : wrapCount;
		float startX = -((rowCount - 1) * lengthPerTeam) * 0.5f;
		float startZ = 0f;

		float maxTeamDepth = 0f;
		int index = 0;
		foreach (SimTeamBase team in sideTeams) {
			SimTeamReal realTeam = team as SimTeamReal;
			realTeam.dir = dir;

			realTeam.ArrangeUnits ();     //
			realTeam.InitializeTeam ();

			maxTeamDepth = maxTeamDepth < realTeam.TeamZSize ? realTeam.TeamZSize : maxTeamDepth;

			realTeam.transform.SetParent (sideContainer.transform);
			realTeam.transform.position = new Vector3 (startX, 0, startZ);
//			realTeam.transform.position = new Vector3 (startX, 2, startZ);

			startX += lengthPerTeam;
            realTeam.teamArrageLine = index / wrapCount;

            index++;

			if (index % wrapCount == 0) {
				rowCount = (sideTeams.Count - index) <= wrapCount ? (sideTeams.Count - index) : wrapCount;
				startX = -((rowCount - 1) * lengthPerTeam) * 0.5f;
				startZ += -maxTeamDepth - GameConfigs.Instance.real_unit_gap;
				maxTeamDepth = 0f;
            }
        }

		float angle = degree * Mathf.Deg2Rad;
		float xpos = radius * Mathf.Cos (angle);
		float zpos = radius * Mathf.Sin (angle);
		
        if (isSerialLink) {
            SimTeamBase[] teamsArry = sideTeams.ToArray();
            SimTeamReal firstTeam = teamsArry[0] as SimTeamReal;

            float lineHeadOffsetX = 8f;
            float lineHeadOffsetZ = 3f;

            GameObject topLeftLinkGO = new GameObject("TopLeftConner");
            RealLinkDot topLeftLinkDot = topLeftLinkGO.AddComponent<RealLinkDot>();
            topLeftLinkGO.transform.SetParent(sideContainer.transform);

            topLeftLinkGO.transform.localPosition = new Vector3(firstTeam.transform.localPosition.x - lineHeadOffsetX, 0, firstTeam.transform.localPosition.z);

            // link to topleft dot
            if (dir == ArrageDIR.left || dir == ArrageDIR.right) { // left right
                foreach (SimTeamBase team in sideTeams) {
                    SimTeamReal realTeam = team as SimTeamReal;

                    float lineOffsetZ = lineHeadOffsetZ;
                    if (realTeam.teamArrageLine == 0) {
                        lineOffsetZ = 0f;
                    }

                    drawLineEX.DrawLinkLineToLeftConner(topLeftLinkDot, realTeam.linkDot, lineOffsetZ, sideLinkLines.transform);
                }
            } else { // top bottom
                
                foreach (SimTeamBase team in sideTeams) {
                    SimTeamReal realTeam = team as SimTeamReal;
                    
                    if (realTeam.teamArrageLine > 0) {
                        drawLineEX.DrawLinkLineToLeftConner(topLeftLinkDot, realTeam.linkDot, lineHeadOffsetZ, sideLinkLines.transform);
                    }
                    
                }
            }
            
            sideContainer.transform.position = new Vector3(xpos, 0, zpos);
            sideContainer.transform.LookAt(Vector3.zero);
            
            // top and bottom link first row
            if (dir == ArrageDIR.top || dir == ArrageDIR.bottom) {

                // top left link to arbiter
                if (sideTeams.Count > wrapCount) {
                    if (dir == ArrageDIR.top) {
                        drawLineEX.DrawLinkLineToLeftConner(arbiterDot, topLeftLinkDot, -lineHeadOffsetZ, worldlinkLinesTrans);
                    } else {
                        drawLineEX.DrawLinkLineToLeftConner(arbiterDot, topLeftLinkDot, lineHeadOffsetZ, worldlinkLinesTrans);
                    }
                }
                
                // first row link to arbiter
                foreach (SimTeamBase team in sideTeams) {
                    SimTeamReal realTeam = team as SimTeamReal;

                    if (realTeam.teamArrageLine == 0) {
                        if (dir == ArrageDIR.top) {
                            drawLineEX.DrawLinkLineToLeftConner(arbiterDot, realTeam.linkDot, -(lineHeadOffsetZ), worldlinkLinesTrans);
                        } else {
                            drawLineEX.DrawLinkLineToLeftConner(arbiterDot, realTeam.linkDot, lineHeadOffsetZ, worldlinkLinesTrans);
                        }
                        
                    }
                }
            } else { // left right
                drawLineEX.DrawLinkLineZ(arbiterDot, topLeftLinkDot, dir, worldlinkLinesTrans);
            }

        } else {

            sideContainer.transform.position = new Vector3(xpos, 0, zpos);
            sideContainer.transform.LookAt(Vector3.zero);

            // driectly link to arbiter
            foreach (SimTeamBase team in sideTeams) {
                SimTeamReal realTeam = team as SimTeamReal;
                realTeam.linkDot.prev = arbiterDot;

                if (realTeam.teamArrageLine == 0) {
                    drawLineEX.DrawLinkLineZ(arbiterDot, realTeam.linkDot, dir, worldlinkLinesTrans);
                }
            }
        }
		
	}

	public override SimUnitBase FindPuzzle (string uid) {
        SimUnitBase puzzle = null;

        SimArbiterReal arbiterReal = arbiter as SimArbiterReal;
        //Debug.Log("Find Title Name  :"+uid );
        int _index = 0;
        try
        {
            _index = puzzlesDict[uid];
        }
        catch (Exception)
        {

            _index =UnityEngine.Random.Range(0,3);
        }
        GameObject puzzleDBGO = arbiterReal.GetPuzzleDBGameObject(_index);
        //  GameObject puzzleDBGO = arbiterReal.GetPuzzleDBGameObject(UnityEngine.Random.Range(0,3));

        if (puzzleDBGO != null) {
            puzzle = puzzleDBGO.GetComponent<SimUnitBase>();
        }

        if (puzzle == null) {
            puzzle = arbiter;
        }

		return puzzle;
	}

	protected override void CreateArbiter () {
		GameObject prefab = SimWorldManager.Instance.getSimObjectPrefabByName (GameConfigs.real_arbiter);
		arbiter = ((GameObject)Instantiate (prefab, gameObject.transform)).GetComponent<SimArbiterReal>();
		arbiterDot = arbiter.gameObject.AddComponent<RealLinkDot> ();

		if (!GameConfigs.Instance.show_npc) {
			if (arbiter.model != null) {
                arbiter.model.gameObject.SetActive (false);
                //Debug.Log("Model isfalse");
			}
		}
	}

    public override void CreatDazzleLight(string strLeft, string strRight)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 2017.05.08 To Change Add dazzle light on Arbiter to Show  RoundNumber
    /// </summary>

}