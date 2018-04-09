using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SendRequestData {
	public string type;
	public string data;

	public string toJsonStr() {
		return JsonUtility.ToJson (this);
	}
}
public class SendRequestExplainData
{
    public string type;
    public string data;
    public string id;
    public string channel_id;
    public string toJsonStr()
    {
        return JsonUtility.ToJson(this);
    }
}
    public class SendRequestSlideData {
    public string type;
    public double value;
    public string channel_id;
    public string toJsonStr()
    {
        return JsonUtility.ToJson(this);
    }
}