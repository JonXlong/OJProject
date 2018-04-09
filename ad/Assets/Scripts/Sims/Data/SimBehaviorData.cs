using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SimBehaviorData {
	public string type;
	public string id;
	public string scr_team;
	public string scr_unit;
	public string time;
	public List<string> messages;
	public List<SimBehaviorDest> dests;

	public static SimBehaviorData CreateFromJSON(JSONObject jsonObj) {
		SimBehaviorData simBehaviorData = new SimBehaviorData ();
		simBehaviorData.type = jsonObj.GetField ("type").str;
		simBehaviorData.id = jsonObj.GetField ("id").str;
		simBehaviorData.scr_team = jsonObj.GetField ("src_team").str;
		simBehaviorData.scr_unit = jsonObj.GetField ("src_unit").str;
		simBehaviorData.time = jsonObj.GetField ("time").str;

		simBehaviorData.messages = new List<string> ();
		List<JSONObject> jsonMsgs = jsonObj.GetField ("message").list;
		foreach (JSONObject msgJo in jsonMsgs) {
			simBehaviorData.messages.Add (msgJo.str);
		}
			
		simBehaviorData.dests = new List<SimBehaviorDest> ();
		List<JSONObject> jsonDests = jsonObj.GetField ("dests").list;

		foreach (JSONObject destJsonObj in jsonDests) {
			simBehaviorData.dests.Add (SimBehaviorDest.CreateFromJSON (destJsonObj));
		}

		return simBehaviorData;
	}
}

public class SimBehaviorDest {
	public string team;
	public string unit;
	public string behavior;
	public string intercepted;
	public bool gather_result;
	public bool firstblood;
	public string firstblood_team;
    public string firstblood_message;
    public SimDamagesData damages;
	public SimScoreData score;

	public static SimBehaviorDest CreateFromJSON(JSONObject jsonObj) {
		SimBehaviorDest beahviorDest = new SimBehaviorDest ();
		beahviorDest.team = jsonObj.GetField ("team").str;
		beahviorDest.unit = jsonObj.GetField ("unit").str;
		beahviorDest.behavior = jsonObj.GetField ("behavior").str;
		beahviorDest.intercepted = jsonObj.GetField ("intercepted").str;
		beahviorDest.gather_result = jsonObj.GetField ("gather_result").b;
		beahviorDest.firstblood = jsonObj.GetField ("firstblood").b;
		beahviorDest.firstblood_team = jsonObj.GetField ("firstblood_team").str;
        beahviorDest.firstblood_message = jsonObj.HasField("firstblood_message") ? jsonObj.GetField("firstblood_message").str : "First Blood";
        beahviorDest.damages = SimDamagesData.CreateFromJSON (jsonObj.GetField("damages"));
		beahviorDest.score = SimScoreData.CreateFromJSON (jsonObj.GetField("score"));
		return beahviorDest;
	}

	public bool isIntercepted {
		get {
			return intercepted != null && intercepted.Length > 0;
		}
	}

}

public class SimDamagesData {
	public bool occupied = false;
	public bool pwn = false;
	public bool down = false;

	public static SimDamagesData CreateFromJSON(JSONObject jsonObj) {
		SimDamagesData damagesData = new SimDamagesData ();
		damagesData.occupied = jsonObj.GetField ("occupied").b;
		damagesData.pwn = jsonObj.GetField ("pwn").b;
		damagesData.down = jsonObj.GetField ("down").b;

		return damagesData;
	}
}

public class SimScoreData {
	public int src_addscore;
	public int dest_addscore;

	public static SimScoreData CreateFromJSON(JSONObject jsonObj) {
		SimScoreData scoreData = new SimScoreData ();
		scoreData.src_addscore = (int)jsonObj.GetField ("src_addscore").f;
		scoreData.dest_addscore = (int)jsonObj.GetField ("dest_addscore").f;
		return scoreData;
	}
}