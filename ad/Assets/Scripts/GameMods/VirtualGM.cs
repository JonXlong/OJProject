using UnityEngine;
using System.Collections;

public class VritualGameMode : GameMode {
	
	public override void OnEnterMod () {
		UIManager.Instance.gameUI.ShowUIContainer ();
        UIManager.Instance.gameUI.UIStatement(true);
        UIManager.Instance.uiCamera.gameObject.SetActive (false);

		RenderSettings.skybox = GameManager.Instance.virutalSkyBox;
		SimWorldManager.Instance.ActiveVirtualWorld ();
	}

	public override void OnExitMod () {
		Destroy (this);
	}

}
