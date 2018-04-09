using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DUIText : MonoBehaviour {
	[HideInInspector]
	public Text text;
	[HideInInspector]
	public Image logoImage;
	[HideInInspector]
	public RectTransform rect;
	[HideInInspector]
	public Vector3 OffsetV3;
	[HideInInspector]
	public int fontSize;
	[HideInInspector]
	public GameObject fromObject;
	[HideInInspector]
	public bool isFixed = false;
    public  Slider _slider;
    public Text _scoreText;
	void Start() {
        //if (_slider==transform.FindChild("BloodSlider").GetComponent<Slider>())
        //{
        //    Debug.LogError("find slider");
        //}
	}

	void Update() {
		if (!isFixed) {
			SimCamera cam = SimWorldManager.Instance.currentWorld.activeCamera;

			if (cam) {
				Vector3 position = fromObject.transform.position + OffsetV3;
				this.transform.position = position;
				rect.LookAt (cam.transform, Vector3.up);
			}
		}
//		Camera cam = (Camera.main != null) ? Camera.main : Camera.current;


//		// Convert Word Position in screen position for UI
//		int mov = ScreenPosition(cam, position);
//
//		// Get center up of target
//		// Vector3 front = position - cam.transform.position;
//
//		// Convert position to view port
//		Vector2 v = cam.WorldToViewportPoint(position);
//
//		text.fontSize = ((int)(mov / 2) + fontSize) / 2;
//
//		//Calculate the movement 
//		Vector2 v2 = new Vector2(v.x, -v.y);
//		//Apply to Text
//		rect.anchorMax = v;
//		rect.anchorMin = v;
//
//		rect.anchoredPosition = v2;
	}

    //	private int ScreenPosition(Camera cam, Vector3 pos) {
    //		int p = (int)(cam.WorldToScreenPoint(pos + (((Vector3.up * 10f) * 0.5f))).y - 
    //			cam.WorldToScreenPoint(pos - (((Vector3.up * 10f) * 0.5f))).y);
    //		return p;
    //	}
    int oldIndex;
    int oldCount;
    int oldListCount;
    public List<RectTransform> _recList=new List<RectTransform>();
    public GameObject handPrefab;
    public void InintSlide(int currentValue, int maxValue)
    {
        Debug.LogError("init");
        _slider.maxValue = maxValue;
        //_recList = new List<RectTransform>();
        oldIndex= currentValue / maxValue;
        if (oldIndex>_recList.Count)
        {
            CreateHandPrefab(oldIndex);
            int rest = currentValue % maxValue;
            _slider.fillRect = _recList[oldIndex - 1];
            _slider.value = rest;
            _scoreText.color = _recList[oldIndex - 1].GetComponent<Image>().color;
            _scoreText.text = currentValue.ToString();
        }
        oldCount = currentValue;
        oldListCount = _recList.Count;
        isInit = true;
    }
    public void MinScore(int score,int maxScore)
    {
        oldCount = oldCount - score;
        _slider.maxValue = maxScore;
        int index = oldCount / maxScore;
        int a = index - 1;
        DesRreList(index);
        _slider.fillRect = _recList[_recList.Count - 1];

      //  _slider.fillRect = GetRect(a);

        _scoreText.color = _recList[_recList.Count-1].GetComponent<Image>().color;
        _scoreText.text = oldCount.ToString();
        _slider.value = oldCount % maxScore;
        
    }
    bool isInit = false;
    public void AddScore(int score,int maxScore)
    {
        //int countIndex = oldCount / maxScore;
        if (!isInit )
        {
            InintSlide(score, maxScore);
           
        }
        _slider.maxValue = maxScore;
        oldCount = oldCount + score;
        Debug.LogError("............."+score +"   ..."+oldCount);
        //destory 
        foreach (RectTransform  item in _recList)
        {
            Destroy(item.gameObject);
        }
        _recList.Clear();
        int index = oldCount / maxScore;
        if (index>=1)
        {
            CreateHandPrefab(index);
        }
       

        //int index = oldCount / maxScore;
        //if (index>countIndex)
        //{
        //    int a = index - countIndex;
        //    _recList[_recList.Count - 1].anchorMax = new Vector2(1, 1);
        //    for (int i = 0; i <a; i++)
        //    {
        //        handPrefab.GetComponent<Image>().color = SwitchColor(_recList.Count);

        //        RectTransform rect = Instantiate(handPrefab, _slider.transform).GetComponent<RectTransform>();
        //        rect.anchorMax = new Vector2(1, 1);
        //        _recList.Add(rect);

        //    }
        //}
        int rest = oldCount % maxScore;
        if (_recList.Count-1>=0)
        {
            _slider.fillRect = _recList[_recList.Count - 1];
            _scoreText.color = _recList[_recList.Count - 1].GetComponent<Image>().color;
            _scoreText.text = oldCount.ToString();
            _slider.value = rest;
        }
        if (_recList.Count==0)
        {
            _slider.fillRect = transform.Find("BloodSlider/BackgroundG").GetComponent<RectTransform>();
            _scoreText.color = Color.green;
            _scoreText.text = oldCount.ToString();
            _slider.value = rest;
        }
        

    }
    void CreateHandPrefab(int index)
    {

        for (int i = 0; i < index; i++)
        {
            handPrefab.GetComponent<Image>().color = SwitchColor(i);
            RectTransform rect = Instantiate(handPrefab, _slider.transform).GetComponent<RectTransform>();
           // rect.anchorMax.Set(1, 1);
            
            _recList.Add(rect);
           
        }
       
    }
    //old min 
    RectTransform GetRect(int index)
    {



        Debug.LogError(index);
            for (int i = 0; i < _recList.Count; i++)
            {
                _recList[i].anchorMax = new Vector2(1, 1);
                if (i >=index)
                {
                Debug.LogError(i);
                Destroy(_recList[i].gameObject);
                //_recList[i].gameObject.SetActive(false);
                _recList.RemoveAt(i);
                    //_recList[i].gameObject.SetActive(false);
                }

            }
      
        return _recList[index];
    }

    //new min
    void DesRreList(int cout)
    {
        foreach (RectTransform item in _recList)
        {
            Destroy(item.gameObject);
        }
        _recList.Clear();
      
            CreateHandPrefab(cout);
       
    }
    Color SwitchColor(int  index)
    {
        Color toColor = Color.white;
        switch (index)
        {
            case 1:
                toColor = Color.red;
                break;
            case 2:
                toColor = Color.yellow;
                break;
            case 3:
                toColor = new Color(0.3f, 0.3f, 1f);
                break;
            case 4:
                toColor = new Color(0.64f, 0.33f, 0.218f);
                break;
            case 5:
                toColor = new Color(0.5f, 0f, 0.5f);
                break;
            //case 6:
            //    toColor = Color.white;
            //    break;
            case 6:
                toColor = Color.cyan;
                break;
            case 7:
                toColor = Color.magenta;
                break;
            case 8:
                toColor = new Color(0xCC / 255f, 0x33 / 255f, 0x33 / 255f, 0xCC / 255f);
                break;
            case 9:
                toColor = new Color(0xCC / 255f, 0xC8 / 255f, 0x31 / 255f, 0xCC / 255f);
                break;
            case 10:
                toColor = new Color(0x33 / 255f, 0x60 / 255f, 0xCC / 255f, 0xCC / 255f);
                break;
            case 11:
                toColor = new Color(0xCC / 255f, 0x61 / 255f, 0x31 / 255f, 0xCC / 255f);
                break;
            case 12:
                toColor = new Color(0x36 / 255f, 0x31 / 255f, 0xCC / 255f, 0xCC / 255f);
                break;
            case 13:
                toColor = new Color(0x33 / 255f, 0xCC / 255f, 0x4D / 255f, 0xCC / 255f);
                break;
        }
        return toColor;
    }
    Color RandomColor()
    {
        //随机颜色的RGB值。即刻得到一个随机的颜色  
        float r = Random.Range(0f, 1f);
        float g = Random.Range(0f, 1f);
        float b = Random.Range(0f, 1f);
        Color color = new Color(r, g, b);
        return color;
    }
}
