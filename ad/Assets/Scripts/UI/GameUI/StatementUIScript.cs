using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatementUIScript : MonoBehaviour {

    public Sprite UI_statement_virtualworld;
    public Sprite UI_statement_realworld;

    private Image image;

    // Use this for initialization
    void Start () {
        image = GetComponent<Image>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SwitchStatement (bool virtualWorld) {
        if (virtualWorld) {
            image.sprite = UI_statement_virtualworld;
        } else {
            image.sprite = UI_statement_realworld;
        }
    }
}
