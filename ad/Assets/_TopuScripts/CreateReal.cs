using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateReal : MonoBehaviour {

    public    Transform floorTrans;
    public List<SimTeamBase> simTeams = new List<SimTeamBase>();
    void Start () {
		
	}
    public void CreatOBJ(List<SimTeamData> teamsData)
    {
        
        foreach (SimTeamData teamData in teamsData)
        {
            GameObject go = new GameObject(teamData.team);
            go.transform.SetParent(gameObject.transform);

            SimTeamBase simTeam = go.AddComponent<SimTeamReal>();
            simTeam.teamData = teamData;
            simTeam.CreateUnits();
            simTeams.Add(simTeam);
        }
    }
	// Update is called once per frame
	void Update () {
		
	}
}
