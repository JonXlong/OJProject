using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimArbiterReal : SimUnitReal {

    public List<GameObject> puzzleDBList = new List<GameObject>();
    public GameObject puzzleItems;

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
    // Change  realworld puzzle count 
    public GameObject GetPuzzleDBGameObject(int index) {
        GameObject dbTank = null;
      
        if (index >= 0 && index < puzzleDBList.Count) {
            dbTank = puzzleDBList[index];
        }
        return dbTank;
    }

    public void HidePuzzleItems() {
        puzzleItems.SetActive (false);
    }

}
