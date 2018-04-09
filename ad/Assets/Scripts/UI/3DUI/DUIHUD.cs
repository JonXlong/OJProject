using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;

public class DUIHUD : MonoBehaviour {
    /// <summary>
    /// UI Prefab to instatantiate
    /// </summary>
    public GameObject TextPrefab;

    //Privates
	/// <summary>
	/// Hud offset x/y
	/// </summary>
	private Vector3 OffsetV3 = new Vector3();
	private Transform canvas3DParent;
	private DUIText hudText;
   

	public void Init( string teamId, string txtStr, string logoURL, Vector3 offset, bool isfixed,bool isVirBloodTest) {
		if (TextPrefab != null) {
			OffsetV3.Set (offset.x, offset.y, offset.z);

			canvas3DParent = GameObject.Find ("UI/Canvas3D").transform;

			hudText = (Instantiate(TextPrefab) as GameObject).GetComponent<DUIText>();
			hudText.fromObject = gameObject;

			hudText.logoImage = hudText.transform.Find ("LOGOBG/LOGO").GetComponent<Image> ();
			hudText.text = hudText.transform.Find ("TXT").GetComponent<Text> ();

			hudText.rect = hudText.GetComponent<RectTransform>();
			hudText.text.text = txtStr;

			hudText.transform.SetParent(canvas3DParent, false);
			hudText.rect.anchoredPosition = Vector2.zero;

			hudText.OffsetV3 = OffsetV3;
			hudText.fontSize = hudText.text.fontSize;
            // add  test create blood bar
            if(isVirBloodTest == true)
            {
                GameManager.Instance.bloodTeamHudList.Add(teamId, hudText);
                hudText.handPrefab = GameManager.Instance.slideHandPrefab;              
                hudText._slider = hudText.transform.FindChild("BloodSlider").GetComponent<Slider>();
                hudText._scoreText = hudText._slider.transform.FindChild("score").GetComponent<Text>();
                //  Currentscore  maxScore
                hudText.InintSlide(UnityEngine.Random.Range(100,600),100);
                Debug.Log(GameManager.Instance.bloodTeamHudList.Count);
            }

            hudText.transform.position = gameObject.transform.position;
			hudText.isFixed = isfixed;

			if (hudText.isFixed) {
				hudText.transform.position += offset;
//				hudText.logoImage.transform.eulerAngles = new Vector3(90f, 0f, 0f);
				hudText.rect.localScale = new Vector3 (0.03f, 0.03f, 1f);
				hudText.transform.localEulerAngles = new Vector3 (90f, 90f, 0f);
			} else {
				hudText.rect.localScale = new Vector3 (-0.03f, 0.03f, 1f);
			}
            //Debug.Log(logoURL);
			StartCoroutine(LoadLogo(logoURL));
		}
	}

	public void UpdateHudRotationZ (Vector3 hudRotationZ) {
		hudText.transform.localEulerAngles += hudRotationZ;
	}

	IEnumerator LoadLogo(string path) {
		
		while (true) {
			//Texture2D sourceTexture = UIManager.Instance.GetLogoTexture2D(path);

			if (UIManager.Instance.GetLogoTexture2D(path)) {
				try {
                    Texture2D sourceTexture = UIManager.Instance.GetLogoTexture2D(path);
                    Sprite sp = Sprite.Create (sourceTexture, new Rect(0f, 0f, sourceTexture.width, sourceTexture.height), new Vector2 (0.5f, 0.5f));
					hudText.logoImage.sprite = sp;
                    
				} catch (ArgumentException e) {
                    //Debug.Log (e.ToString());
                  //  hudText.logoImage.transform.parent.gameObject.SetActive(false);
                }
				break;
            }
            //else
            //{
            //  //  Debug.Log("team is no  logo");
            //    hudText.logoImage.transform.parent.gameObject.SetActive(false);
            //    break;
            //}
            //change    if  logo is  null  teamLogo is null
           
               
           

			yield return 0;
		}

	}

    /// <summary>
    /// Disable all text when this script gets disabled.
    /// </summary>
    void OnDisable() {
		if (hudText) {
			Destroy(hudText.gameObject);
		}
    }

}