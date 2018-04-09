using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class SimTeamBase : MonoBehaviour {
	protected DUIHUD textHud = null;

	public SimTeamData teamData;

	public List<SimUnitBase> simUnits;

	public Forcefield teamShield;

	private int _score;

	protected Dictionary<string, SimUnitBase> unitsDict;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public abstract void CreateUnits ();

	public SimUnitBase findUnitById (string unitId) {
		SimUnitBase u = null;

		if (unitsDict.ContainsKey (unitId)) {
			u = unitsDict [unitId];
		}

		return u;
	}

	public int score {
		set {
			_score = value;
		}

		get {
			return _score;
		}
	}

	public void ResetAllUnits() {
		foreach (SimUnitBase unit in unitsDict.Values) {
			unit.ResetStatus ();
		}
	}

	public void RefreshUnitsDamageStatus() {
		foreach (SimUnitBase unit in unitsDict.Values) {
			unit.RefreshDamageStatus ();
		}
	}

	public void RemoveAllUnitsEffects() {
		foreach (SimUnitBase unit in unitsDict.Values) {
			unit.RemoveAllTempEffects ();
		}
	}

	public abstract void CreateHUD ();
	public abstract void RemoveHUD ();

	public abstract void ArrangeUnits ();

	/** Already has team data **/
	public abstract void InitializeTeam ();
}