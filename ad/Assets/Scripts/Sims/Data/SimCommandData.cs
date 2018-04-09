//using UnityEngine;
//using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class SimCommandData {
	public const string ACTION_scene = "scene";
	public const string ACTION_mute = "mute";
	public const string ACTION_ui_all = "ui_all";
	public const string ACTION_ui_scoreboard = "ui_scoreboard";
	public const string ACTION_ui_messagebox = "ui_messagebox";
	public const string ACTION_camera = "camera";
	public const string ACTION_timer = "timer";
	public const string ACTION_reset_units = "reset_units";
    //add finals name
    public const string ACTION_set_finalsname = "set_contestname";
    public const string ACTION_set_round = "set_round";
    public const string ACTION_ui_bottom = "ui_bottom";
    //add gameover scene
    public const string ACITON_overscene = "GameOverScene";
    public const string ACTION_Wait = "timer_start";
    public const string ACTION_set_progress = "set_progress";
    public const string ACTION_off_topscoreboard ="topscoreboard";
    public string type;
	public string action;
	public string param;
   
}

public class SimUpdateScoreData {
	public const string ACTION_score_board = "update_scoreboard";
	public List<ScoreBoardData> list;
    //add titleName
    public string title;
	public static SimUpdateScoreData CreateFromJSON(JSONObject jsonObj) {
		SimUpdateScoreData data = new SimUpdateScoreData ();

		data.list = new List<ScoreBoardData> ();
		List<JSONObject> l = jsonObj.GetField ("list").list;
        if (jsonObj.GetField("title"))
        {
            data.title = jsonObj.GetField("title").str;
        }
		foreach (JSONObject obj in l) {
			data.list.Add (ScoreBoardData.CreateFromJSON(obj));
		}

		return data;
	}
}

public class SlideEventData {
    public float pos;
    public string events;
    public string color;
    public List<SlideEventData> list;
    public static SlideEventData CreateFromJson(JSONObject jsonObj)
    {
        SlideEventData sed = new SlideEventData();
        if (jsonObj.GetField("pos"))
        {
            sed.pos = jsonObj.GetField("pos").f;
        }
        if (jsonObj.GetField("color"))
        {
            sed.color = jsonObj.GetField("color").str;
        }
        if (jsonObj.GetField("event"))
        {
            sed.events = jsonObj.GetField("event").str;
        }
        return sed;
    }
}
public class TeamBloodData {
    public string teamid;
    public int  init_score;
    public int   current_score;
  
    public static TeamBloodData CreateFromJson(JSONObject jsonObj)
    {
        TeamBloodData tbd = new TeamBloodData();
        if (jsonObj.GetField("teamid"))
        {
            tbd.teamid = jsonObj.GetField("teamid").str;
        }
        if (jsonObj.GetField("init_score"))
        {
            tbd.init_score =int.Parse( jsonObj.GetField("init_score").str);
        }
        if (jsonObj.GetField("current_score"))
        {
            tbd.current_score =int.Parse( jsonObj.GetField("current_score").str);
        }
        return tbd;
    }

}

public class ScoreBoardData {
	public string id;
	public string score;
	public string trend;
    //Change 
    public string rank;
    //add  09.04
    public string teamName;
    public string teamMvp;
    public string teamLogo;
    public string teamFlag;
    public string teamFirstBlood;
    public string displayTeamName
    {
        get
        {
            string n = teamName;
            if (IsChinese(n))
            {
                if (n.Length > 9)
                {
                    n = teamName.Substring(0, 6) + "...";
                }
            }
            else
            {
                if (n.Length > 10)
                {
                    n = teamName.Substring(0, 6) + "...";
                }
            }
            return n;
        }
    }
    public static bool IsChinese(string checkedStr)
    {
        string pattern;
        pattern = @"[\u4e00-\u9fa5]";

        if (Regex.IsMatch(checkedStr, pattern))
        {
            return true;
        }

        return false;
    }
    public static ScoreBoardData CreateFromJSON(JSONObject jsonObj) {
		ScoreBoardData data = new ScoreBoardData ();
        if (jsonObj.GetField("id")) {
            data.id = jsonObj.GetField("id").str;
        }
        if (jsonObj.GetField("score")) {
            data.score = jsonObj.GetField("score").f.ToString();
        }
        if (jsonObj.GetField("trend")) {
            data.trend = jsonObj.GetField("trend").str;
        }
        if (jsonObj .GetField ("rank"))
        {
            data.rank = jsonObj.GetField("rank").ToString();
        }
        if (jsonObj.GetField("name"))
        {
            data.teamName = jsonObj.GetField("name").str;
        }
        if (jsonObj.GetField("mvp"))
        {
            data.teamMvp = jsonObj.GetField("mvp").str;
        }
        if (jsonObj.GetField("firstblood"))
        {
            data.teamFirstBlood = jsonObj.GetField("firstblood").str;
        }
        if (jsonObj.GetField("logo"))
        {
            data.teamLogo = jsonObj.GetField("logo").str;
        }
        if (jsonObj.GetField("countryflag"))
        {
            data.teamFlag= jsonObj.GetField("countryflag").str;
        }
		return data;
	}

}