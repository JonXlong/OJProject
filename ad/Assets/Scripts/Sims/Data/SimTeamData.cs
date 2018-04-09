using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

public class SimTeamData {
	public string team;
	public string name;
	public string color;
	public string logo;
	public int score;

	public List<SimUnitData> units;
	public Dictionary<string, SimUnitData> unitsDict;

	public string displayTeamName {
		get {
			string n = name;
			if (IsChinese(n)) {
				if (n.Length > 4) {
					n = name.Substring (0, 4) + "...";
				}
			} else {
				if (n.Length > 9) {
					n = name.Substring (0, 9) + "...";
				}
			}
			return n;
		}
	}

	public static bool IsChinese(string checkedStr) {
		string pattern;
		pattern = @"[\u4e00-\u9fa5]";

		if (Regex.IsMatch (checkedStr, pattern)) {
			return true;
		}

		return false;
	}

	public static SimTeamData CreateFromJSON(JSONObject jsonObj) {
		SimTeamData data = new SimTeamData ();
		data.team = jsonObj.GetField ("team").str;
		data.name = jsonObj.GetField ("name").str;
		data.color = jsonObj.GetField ("color").str;
		data.logo = jsonObj.GetField ("logo").str;
        data.score = (int)jsonObj.GetField("score").f;
        //data.score = (int)float.Parse(jsonObj.GetField("score").str);

        //   Debug.LogError("TeamCOlor"+data.color);
        data.units = new List<SimUnitData> ();
		data.unitsDict = new Dictionary<string, SimUnitData> ();

		List<JSONObject> jsonUnits = jsonObj.GetField ("units").list;
		foreach (JSONObject unitJsonObj in jsonUnits) {
			data.units.Add (SimUnitData.CreateFromJSON (unitJsonObj, data.unitsDict));
		}

		return data;
	}
}

public class SimUnitData {
	public string id;
	public string type;
	public string color;

	public SimVirtualData virtual_world;
	public SimDamagesData damages;
	public List<SimUnitData> units;

	public static SimUnitData CreateFromJSON(JSONObject jsonObj, Dictionary<string, SimUnitData> unitsDict) {
		SimUnitData data = new SimUnitData ();

		data.id = jsonObj.GetField ("id").str;
		data.type = jsonObj.GetField ("type").str;
		data.color = jsonObj.GetField ("color").str;
		data.virtual_world = SimVirtualData.CreateFromJSON (jsonObj.GetField("virtual_world"));
		data.damages = SimDamagesData.CreateFromJSON(jsonObj.GetField("damages"));
		data.units = new List<SimUnitData> ();

		unitsDict[data.id] = data;

		List<JSONObject> jsonUnits = jsonObj.GetField ("units").list;
		foreach (JSONObject unitJsonObj in jsonUnits) {
			data.units.Add (SimUnitData.CreateFromJSON (unitJsonObj, unitsDict));
		}

		return data;
	}
}

public class SimVirtualData {
	public string model;
	public bool show_children;

	public static SimVirtualData CreateFromJSON(JSONObject jsonObj) {
		SimVirtualData data = new SimVirtualData ();
		data.model = jsonObj.GetField ("model").str;
		data.show_children = jsonObj.GetField ("show_children").b;
		return data;
	}
}

public class SimPuzzleData {
	public string id;
	public string name;
//	public int score;
//	public string color;
	public string type;
	public string tag;

	public static SimPuzzleData CreateFromJSON(JSONObject jsonObj) {
		SimPuzzleData data = new SimPuzzleData ();
		data.id = jsonObj.GetField ("id").str;
		data.name = jsonObj.GetField ("name").str;
//		data.score = (int)jsonObj.GetField ("score").f;
//		data.color = jsonObj.GetField ("color").str;
		data.type = jsonObj.GetField ("type").str;
		data.tag = jsonObj.GetField ("tag").str;
		return data;
	}

//	public Color getColor {
//		get {
//			
//			Color c = Color.white;
//			string colStr = color;
//
//			if (colStr.IndexOf("#") != 0) {
//				colStr = "#" + color;
//			}
//
//			if (!ColorUtility.TryParseHtmlString (colStr, out c)) {
//				c = Color.red;
//			}
//
//			return c;
//		}
//	}

}