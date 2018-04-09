using UnityEngine;
using System.Collections;

public static class ObjectUtils {
	
	public static void DestoryChildrenObjects (GameObject rootObj, float delay) {
		int childrenNum = rootObj.transform.childCount;
		for (int i = 0; i < childrenNum; i++) {
			GameObject.Destroy (rootObj.transform.GetChild (i), delay);
		}
	}

}
