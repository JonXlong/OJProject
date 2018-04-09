using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class ScoreBoardItem : MonoBehaviour {

	public Sprite spriteUp;
	public Sprite spriteDown;
	public Sprite spriteKeep;

	public Image logoImage;
	public Text teamTxt;
	public Text scoreTxt;
	public Image statusImg;
    //Change
    public Text rankTxt;
    private string logoUrl = "";

	// Use this for initialization
	void Start () {
	
	}

	public void InitItem(SimTeamData teamData) {
        if (teamData != null) {
            if (logoUrl != teamData.logo) {
               // Debug.LogError(teamData.logo);
                StartCoroutine(LoadLogo(teamData.logo));
            }
            else
            {
                logoImage.enabled = false;
            }
            teamTxt.text = teamData.displayTeamName;
        }
		
		// scoreTxt.text = teamData.score.ToString();
	}

	public void UpdateScore (string score, string trend) {
		scoreTxt.text = score;

		if (trend == "up") {
			statusImg.sprite = spriteUp;
		} else if (trend == "down") {
			statusImg.sprite = spriteDown;
		} else {
			statusImg.sprite = spriteKeep;
		}
	}

	IEnumerator LoadLogo(string path) {

		while (true) {
			Texture2D sourceTexture = UIManager.Instance.GetLogoTexture2D(path);

			if (sourceTexture != null) {
				try {
					Sprite sp = Sprite.Create (sourceTexture, new Rect(0, 0, sourceTexture.width, sourceTexture.height), new Vector2 (0.5f, 0.5f));
					logoImage.sprite = sp;
				} catch (ArgumentException e) {
					Debug.Log (e.ToString());
				}
				break;
            }
            // change   
            //else
            //{
            //    logoImage.enabled = false;
            //    Debug.Log("---++++");
            //    break;
            //}

            yield return 0;
		}

	}

	// Update is called once per frame
	void Update () {
	
	}
}
