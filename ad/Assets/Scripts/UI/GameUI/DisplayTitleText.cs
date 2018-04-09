using UnityEngine;
//using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class DisplayTitleText : MonoBehaviour {

	[HideInInspector]
	public CanvasGroup cgroup;

	public float startAlpha = 1.0f;
	public float endAlpha = 0f;
	public float updateStep = 0.01f;

	private Vector2 startAnchoredPosition;
	public Vector2 endAnchoredPosition;

	private float targetAlpha = 1.0f;
	private Vector2 targetAnchoredPosition;
	private Vector2 fromAnchoredPosition;
	private float lerpTime = 0f;
	private float fromAlpha = 1.0f;

	private RectTransform rectTransform;
    private float hideTime;
    private bool isShowing;

	void Awake () {
		cgroup = GetComponent<CanvasGroup> ();
		rectTransform = GetComponent<RectTransform> ();
		startAnchoredPosition = new Vector2 (rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y);
	}

	public void Hide () {
        isShowing = false;

        cgroup.blocksRaycasts = true;
		targetAlpha = startAlpha;

		targetAnchoredPosition = startAnchoredPosition;

		fromAnchoredPosition.x = rectTransform.anchoredPosition.x;
		fromAnchoredPosition.y = rectTransform.anchoredPosition.y;

		fromAlpha = cgroup.alpha;
		lerpTime = 0;
	}

	public void Show (string txt, float _hideTime) {
		Text titleText = transform.GetComponentInChildren<Text> ();
		titleText.text = txt;

		targetAlpha = endAlpha;

		targetAnchoredPosition = endAnchoredPosition;
		cgroup.blocksRaycasts = true;

		fromAnchoredPosition.x = rectTransform.anchoredPosition.x;
		fromAnchoredPosition.y = rectTransform.anchoredPosition.y;

		fromAlpha = cgroup.alpha;
		lerpTime = 0;

        isShowing = true;
        if (_hideTime > 0) {
            hideTime = _hideTime;
        }
    }  //add
    public void ShowTopScoreBoard(float _hideTime)
    {
        targetAlpha = endAlpha;

        targetAnchoredPosition = endAnchoredPosition;
        cgroup.blocksRaycasts = true;

        fromAnchoredPosition.x = rectTransform.anchoredPosition.x;
        fromAnchoredPosition.y = rectTransform.anchoredPosition.y;

        fromAlpha = cgroup.alpha;
        lerpTime = 0;

        isShowing = true;
        if (_hideTime > 0)
        {
            hideTime = _hideTime;
        }
    }

    // Update is called once per frame
    void Update () {
        if (isShowing && hideTime > 0) {
            if (lerpTime >= hideTime) {
                Hide();
            }
        }

		if (cgroup.alpha != targetAlpha) {
			cgroup.alpha = Mathf.Lerp (fromAlpha, targetAlpha, lerpTime);
		}

		if (fromAnchoredPosition.x != targetAnchoredPosition.x || fromAnchoredPosition.y != targetAnchoredPosition.y) {
			rectTransform.anchoredPosition = Vector2.Lerp (fromAnchoredPosition, targetAnchoredPosition, lerpTime * 2f);
		}

		lerpTime += updateStep;
	}

}
