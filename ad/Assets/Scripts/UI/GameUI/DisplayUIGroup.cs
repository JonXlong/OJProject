using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class DisplayUIGroup : MonoBehaviour {

	[HideInInspector]
	public CanvasGroup cgroup;

	public float startAlpha = 1.0f;
	public float endAlpha = 0f;
	public float updateStep = 0.01f;

	private Vector2 startAnchoredPosition;
	public Vector2 endAnchoredPosition;
	public Vector2 endIncludeButtonsAnchoredPosition;

	private float targetAlpha = 1.0f;
	private Vector2 targetAnchoredPosition;
	private Vector2 fromAnchoredPosition;
	private float lerpTime = 0f;
	private float fromAlpha = 1.0f;

	private RectTransform rectTransform;

	void Awake () {
		cgroup = GetComponent<CanvasGroup> ();
		rectTransform = GetComponent<RectTransform> ();
		startAnchoredPosition = new Vector2 (rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y);
	}

	public void Show () {
		cgroup.blocksRaycasts = true;
		targetAlpha = startAlpha;

		targetAnchoredPosition = startAnchoredPosition;

		fromAnchoredPosition.x = rectTransform.anchoredPosition.x;
		fromAnchoredPosition.y = rectTransform.anchoredPosition.y;

		fromAlpha = cgroup.alpha;
		lerpTime = 0;
	}

	public void Hide (bool hideButtons) {
		targetAlpha = endAlpha;

		if (hideButtons) {
			targetAnchoredPosition = endIncludeButtonsAnchoredPosition;
			cgroup.blocksRaycasts = false;
		} else {
			targetAnchoredPosition = endAnchoredPosition;
			cgroup.blocksRaycasts = true;
		}

		fromAnchoredPosition.x = rectTransform.anchoredPosition.x;
		fromAnchoredPosition.y = rectTransform.anchoredPosition.y;

		fromAlpha = cgroup.alpha;
		lerpTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (cgroup.alpha != targetAlpha) {
			cgroup.alpha = Mathf.Lerp (fromAlpha, targetAlpha, lerpTime);
		}

		if (fromAnchoredPosition.x != targetAnchoredPosition.x || fromAnchoredPosition.y != targetAnchoredPosition.y) {
			rectTransform.anchoredPosition = Vector2.Lerp (fromAnchoredPosition, targetAnchoredPosition, lerpTime * 2f);
		}

		lerpTime += updateStep;
	}

}
