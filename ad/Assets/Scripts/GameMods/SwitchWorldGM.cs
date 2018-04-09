using UnityEngine;
using System.Collections;

public class SwitchWorldGameMode : GameMode {

	public bool isVritualToRealWorld;

	public override void OnEnterMod () {
		
	}

	public override void OnExitMod () {
		if (isVritualToRealWorld) {
			GameManager.Instance.SetGameMode (GameMode.GM_Real);
		} else {
			GameManager.Instance.SetGameMode (GameMode.GM_Vritual);
		}
		Destroy (this);
	}

}
