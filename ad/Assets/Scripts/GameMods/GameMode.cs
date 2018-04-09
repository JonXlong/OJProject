using UnityEngine;
using System.Collections;

public abstract class GameMode : MonoBehaviour {
	public GameMode nextGM;

	public const string GM_CountDown = "GM_CountDown";
	public const string GM_Vritual = "GM_Vritual";
	public const string GM_Real = "GM_Real";

	abstract public void OnEnterMod ();
	abstract public void OnExitMod ();
}
