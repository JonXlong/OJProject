using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimTeamReal : SimTeamBase {
	
	public SimRealWorld.ArrageDIR dir;

	private GameObject teamFloor;

	private Transform linkLineTrans;
	private DrawLineEX drawLineEX;

	[HideInInspector]
	public RealLinkDot linkDot;

    public int teamArrageLine = 0;
	private int teamMaxLines = 1;
    private int maxTeamDepth = 0;

	void Start () {
	}

	// Update is called once per frame
	void Update () {
	
	}

	public override void CreateUnits () {
		GameObject lineLinesGO = new GameObject ("LinkLines++++++++++++++++");
		linkLineTrans = lineLinesGO.transform;

        //print(transform.name);
		linkLineTrans.SetParent (transform);
		drawLineEX = lineLinesGO.AddComponent<DrawLineEX> ();

		unitsDict = new Dictionary<string, SimUnitBase> ();
		simUnits = new List<SimUnitBase> ();
		CreateSubUnits (teamData.units, simUnits, transform, 0);
	}

	private void CreateSubUnits(List<SimUnitData> units, List<SimUnitBase> savedObjects, Transform parent, int level) {
        maxTeamDepth = maxTeamDepth < level ? level : maxTeamDepth;

        foreach (SimUnitData unitData in units) {
            //print(unitData.type);
			GameObject prefab = SimWorldManager.Instance.getSimObjectPrefabByName (unitData.type);
			SimUnitReal simUnit = ((GameObject)Instantiate (prefab, parent, true)).GetComponent<SimUnitReal>();
			simUnit.linkDot = simUnit.gameObject.AddComponent<RealLinkDot> ();

			unitsDict[unitData.id] = simUnit;

			simUnit.gameObject.transform.SetParent (parent);
            //print(unitData.id);
			simUnit.gameObject.name = unitData.id;

			simUnit.unitData = unitData;

			savedObjects.Add (simUnit);

			if (unitData.units != null && unitData.units.Count > 0) {
				if (level < 4) {
					simUnit.children = new List<SimUnitBase> ();
					CreateSubUnits (unitData.units, simUnit.children, parent, level + 1);
				} else {
					CreateSubUnits (unitData.units, savedObjects, parent, level);
				}
			}

		}
	}

	public override void CreateHUD () {
		if (textHud == null) {
			// create team hud
			textHud = gameObject.AddComponent<DUIHUD> ();
			textHud.TextPrefab = GameManager.Instance.realTeamHUD;
			textHud.Init (teamData.team, teamData.displayTeamName, teamData.logo, new Vector3(0f, 0.2f, 0f), true,false );
		}

		if (dir == SimRealWorld.ArrageDIR.left) {
			textHud.UpdateHudRotationZ (new Vector3(0f, 0f, 0f));
		} else if (dir == SimRealWorld.ArrageDIR.right) {
			textHud.UpdateHudRotationZ (new Vector3(0f, 0f, 180f));
		} else if (dir == SimRealWorld.ArrageDIR.top) {
			textHud.UpdateHudRotationZ (new Vector3(0f, 0f, 270f));
		} else if (dir == SimRealWorld.ArrageDIR.bottom) {
			textHud.UpdateHudRotationZ (new Vector3(0f, 0f, 90f));
		}

	}

	public override void RemoveHUD () {
		if (textHud != null) {
			GameObject.Destroy (textHud);
		}
	}

	public override void InitializeTeam () {
		teamFloor = (GameObject)Instantiate (GameManager.Instance.realTeamFloor, transform);

		GameObject frame = teamFloor.transform.FindChild ("frame").gameObject;
		Renderer renderer = frame.GetComponent<Renderer> ();
		if (renderer != null) {
			renderer.material.SetColor ("_TintColor", GameConfigs.Instance.GetRealWorldColorByName (teamData.color));
		}

		float XOffset = -((GameConfigs.Instance.real_max_countX + 1) * GameConfigs.Instance.real_unit_gap * 0.5f) - GameConfigs.Instance.real_unit_gap;
		float ZOffset = GameConfigs.Instance.real_unit_gap;

		//float TeamXSize = (GameConfigs.Instance.real_max_countX + 3) * GameConfigs.Instance.real_unit_gap + 1.5f;

		teamFloor.transform.localPosition += new Vector3 (XOffset, 0, ZOffset);
		teamFloor.transform.localScale = new Vector3 (TeamXSize, teamFloor.transform.localScale.y, -TeamZSize);
	}

	public override void ArrangeUnits() {
		linkDot = gameObject.AddComponent<RealLinkDot> ();
		teamMaxLines = CreateUnitsGroupLevel1 (linkDot, simUnits);
	}

	public float TeamZSize {
		get {
			return (teamMaxLines + 1) * GameConfigs.Instance.real_unit_gap + 1.5f;
		}
	}

    public float TeamXSize {
        get {
            float xSize = 0f;
            if (maxTeamDepth == 1) {
                xSize = (maxTeamDepth + 2) * GameConfigs.Instance.real_unit_gap + 1.5f;
            } else {
                xSize = (maxTeamDepth + 4) * GameConfigs.Instance.real_unit_gap + 1.5f;
            }
            return xSize;
        }
    }

    private int CreateUnitsGroupLevel1(RealLinkDot parentDot, List<SimUnitBase> units) {
		int index = 0;
		int maxLines = 0;
		int lastChildrenLines = 0;

		float posX = parentDot.transform.position.x - ((GameConfigs.Instance.real_max_countX + 1) * GameConfigs.Instance.real_unit_gap * 0.5f);
		float posZ = parentDot.transform.position.z - GameConfigs.Instance.real_unit_gap;

		RealLinkDot prevDot = drawLineEX.CreateRootLineWithDot (parentDot, new Vector3(posX - GameConfigs.Instance.real_unit_gap * 0.5f, 0, posZ), linkLineTrans);

		foreach (SimUnitBase unit in units) {
			SimUnitReal realUnit = unit as SimUnitReal;
			realUnit.team = this;
			realUnit.InitializeUnit ();

			posZ -= GameConfigs.Instance.real_unit_gap * lastChildrenLines;
			realUnit.transform.position = new Vector3 (posX, 0, posZ);

			drawLineEX.DrawLinkLine (prevDot, realUnit.linkDot, false, linkLineTrans);

			if (realUnit.children != null && realUnit.children.Count > 0) {
				lastChildrenLines = CreateUnitsGroupLevel2 (realUnit.linkDot, realUnit.children);
			} else {
				lastChildrenLines = 1;
			}

			if (index == 0) {
				prevDot = realUnit.linkDot.prev.prev;
			} else {
				prevDot = realUnit.linkDot.prev;
			}

			index++;
			maxLines += lastChildrenLines;
		}

		return maxLines;
	}

	private int CreateUnitsGroupLevel2(RealLinkDot parentDot, List<SimUnitBase> units) {
		int maxLines = 0;
		int lastChildrenLines = 0;

		float posX = parentDot.transform.position.x + GameConfigs.Instance.real_unit_gap;
		float posZ = parentDot.transform.position.z;
		RealLinkDot prevDot = parentDot;

		foreach (SimUnitBase unit in units) {
			SimUnitReal realUnit = unit as SimUnitReal;
			realUnit.team = this;
			realUnit.InitializeUnit ();

			posZ -= GameConfigs.Instance.real_unit_gap * lastChildrenLines;
			realUnit.transform.position = new Vector3 (posX, 0, posZ);
		

			drawLineEX.DrawLinkLine (prevDot, realUnit.linkDot, false, linkLineTrans);

			if (realUnit.children != null && realUnit.children.Count > 0) {
				lastChildrenLines = CreateUnitsGroupLevel3 (realUnit.linkDot, realUnit.children);
			} else {
				lastChildrenLines = 1;
			}

			maxLines += lastChildrenLines;
			prevDot = realUnit.linkDot.prev;
		}

		return maxLines;
	}

	private int CreateUnitsGroupLevel3(RealLinkDot parentDot, List<SimUnitBase> units) {
		int maxLines = 0;
		int lastChildrenLines = 0;

		float posX = parentDot.transform.position.x + GameConfigs.Instance.real_unit_gap;
		float posZ = parentDot.transform.position.z;
		RealLinkDot prevDot = parentDot;

		foreach (SimUnitBase unit in units) {
			SimUnitReal realUnit = unit as SimUnitReal;
			realUnit.team = this;
			realUnit.InitializeUnit ();

			posZ -= GameConfigs.Instance.real_unit_gap * lastChildrenLines;
			realUnit.transform.position = new Vector3 (posX, 0, posZ);

			drawLineEX.DrawLinkLine (prevDot, realUnit.linkDot, false, linkLineTrans);

			if (realUnit.children != null && realUnit.children.Count > 0) {
				lastChildrenLines = CreateUnitsGroupLevel4 (realUnit.linkDot, realUnit.children);
			} else {
				lastChildrenLines = 1;
			}

			maxLines += lastChildrenLines;
			prevDot = realUnit.linkDot.prev;
		}

		return maxLines;
	}

	private int CreateUnitsGroupLevel4 (RealLinkDot parentDot, List<SimUnitBase> units) {
		int index = 0;
		float stepX = 0f;

		float posX = parentDot.transform.position.x + GameConfigs.Instance.real_unit_gap;
		float posZ = parentDot.transform.position.z;

		RealLinkDot lastHeadDot = null;
		RealLinkDot prevDot = parentDot;

		foreach (SimUnitBase unit in units) {
			SimUnitReal realUnit = unit as SimUnitReal;
			realUnit.team = this;
			realUnit.InitializeUnit ();

			// move to next row
			if (index == GameConfigs.Instance.real_max_countX) {
				stepX = 0;
				index = 0;
				posZ -= GameConfigs.Instance.real_unit_gap;
			}

			realUnit.transform.position = new Vector3 (posX + stepX, 0, posZ);

			if (index == 0) {
				if (lastHeadDot == null) {
					drawLineEX.DrawLinkLine (prevDot, realUnit.linkDot, false, linkLineTrans);
				} else {
					drawLineEX.DrawLinkLine (lastHeadDot, realUnit.linkDot, false, linkLineTrans);
				}
				lastHeadDot = realUnit.linkDot.prev;
				prevDot = realUnit.linkDot;
			} else {
				drawLineEX.DrawLinkLine (prevDot, realUnit.linkDot, true, linkLineTrans);
				prevDot = realUnit.linkDot;
			}

			stepX += GameConfigs.Instance.real_unit_gap;
			index++;
		}

		return (int)(Mathf.Ceil((float)units.Count / (float)GameConfigs.Instance.real_max_countX));
	}

//	private void CreateLinkDot(SimUnitReal fromUnit, RealLinkDot toDot) {
//		fromUnit.linkDot = fromUnit.gameObject.AddComponent<RealLinkDot> ();
//		fromUnit.linkDot.prev = toDot;
//		drawLineEX.Link_L (fromUnit.linkDot, toDot);
//	}
//
//	private RealLinkDot CreateLinkDotForChildren(SimUnitReal realUnit) {
//		GameObject dotGo = new GameObject ("Line" + realUnit.unitData.id);
//		RealLinkDot dot = dotGo.AddComponent<RealLinkDot> ();
//		dotGo.transform.SetParent (realUnit.transform);
//		dotGo.transform.position = new Vector3(realUnit.transform.position.x + GameConfigs.Instance.real_unit_gap * 0.5f, realUnit.transform.position.y, realUnit.transform.position.z);
//		dot.prev = realUnit.linkDot;
//
//		drawLineEX.Link_T (realUnit.linkDot, dot);
//
//		return dot;
//	}
//
//	private RealLinkDot CreateLinkDotVertialDotForNextLine(RealLinkDot fromDot, int lines) {
//		GameObject dotGo = new GameObject ("RootLine");
//
//		RealLinkDot dot = dotGo.AddComponent<RealLinkDot> ();
//		dotGo.transform.SetParent (fromDot.transform);
//		dotGo.transform.position = new Vector3(fromDot.transform.position.x, fromDot.transform.position.y, fromDot.transform.position.z - GameConfigs.Instance.real_unit_gap * lines);
//		dot.prev = fromDot;
//		drawLineEX.Link_Straight (fromDot, dot);
//
//		return dot;
//	}
}

//public class UnitDepthData {
//	public int depthX;
//	public int depth;
//}
//
//public class SimUnitsCountCompare : IComparer<SimUnitBase> {
//	public int Compare(SimUnitBase x, SimUnitBase y) {
//		if (x == null) {
//			if (y == null) {
//				return 0;
//			} else {
//				return -1;
//			}
//		} else {
//			if (y == null) {
//				return 1;
//			} else {
//				int xCount = x.children.Count;
//				int yCount = y.children.Count;
//				if (xCount == yCount) {
//					return 0;
//				}
//				return xCount > yCount ? 1 : -1;
//			}
//		}
//	}
//}