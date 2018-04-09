using UnityEngine;
using System.Collections;

public class RealGameMode : GameMode {

	public override void OnEnterMod () {
		UIManager.Instance.gameUI.ShowUIContainer ();
        UIManager.Instance.gameUI.UIStatement(false);
        UIManager.Instance.uiCamera.gameObject.SetActive (false);

		RenderSettings.skybox = GameManager.Instance.realSkyBox;
		SimWorldManager.Instance.ActiveRealWorld ();
	}

	public override void OnExitMod () {
		Destroy (this);
	}
}
