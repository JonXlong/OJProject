using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadProgressImages : MonoBehaviour {
	
	public Color inactiveColor = Color.gray;
	public Color activeColor = Color.white;
	public Image[] images = new Image[0];

	protected void Start() {
	}

	public void OnProgress(int num) {
		if (num <= images.Length) {
			int len = images.Length - num;
			for (int i = 0; i < len; i++)
			{
				images[i].color = activeColor;
			}
		}

	}
}