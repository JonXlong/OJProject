using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SimArbiterVirtual : SimUnitVirtual {

	public Transform gun1;
	public Transform gun2;
	public Transform gun3;

	private ArrayList guns;
	private int curGun = 0;

	void Awake () {
		guns = new ArrayList ();
		guns.Add (gun1);
		guns.Add (gun2);
		guns.Add (gun3);
		curGun = 0;
	}

	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.down * 0.5f, Space.World);
	}

	override public Vector3 gunPos {
		get {
			if (curGun >= guns.Count) {
				curGun = 0;
			}
			Transform gunTrans = (Transform)guns [curGun];
			curGun++;
			return gunTrans.position;
		}
	}

	override public void InitializeUnit() {
	}

	override public void UpdateDamages(SimBehaviorDest dest) {
	}

    public override string FriendlyTeamName
    {
        get
        {
            return "仲裁者";
        }
    }

}
