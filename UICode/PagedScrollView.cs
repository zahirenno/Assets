using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PagedScrollView : MonoBehaviour
{
	private int _pages = 3;
	public int Pages{
		get { return _pages;}
		set { _pages = value;}
	}

	public ScrollRect scrollRect;
	public RectTransform content;
	protected RectTransform rectTransform;
	// Use this for initialization
	protected virtual void Start ()
	{
		rectTransform = (RectTransform)transform;

		if (scrollRect == null)
			scrollRect = transform.GetComponent<ScrollRect> ();

		if (scrollRect.content == null) {
			GameObject g = new GameObject(transform.name + "Content");
			RectTransform rt = g.AddComponent<RectTransform>();
			rt.SetParent(transform);
			rt.localPosition = new Vector3(0,0,0);
			scrollRect.content = rt;

		}
		content = (RectTransform)scrollRect.content.transform;

		resized ();

	
	}


	private float targetPosition = 0;
	public float normalizedSpeed = 1.0f;
	public void goToPage(int p, bool animated = true){
		targetPosition = (float)p / (float)(_pages - 1);
		if (!animated)
			scrollRect.horizontalNormalizedPosition = targetPosition;
	}

	protected float getWidth(){
		Vector3[] corners = new Vector3[4];
		rectTransform.GetLocalCorners (corners);

		//return rectTransform.sizeDelta.x;

		return Mathf.Abs(corners[0].x - corners[2].x);
	}

	protected float getHeight(){
		Vector3[] corners = new Vector3[4];
		rectTransform.GetWorldCorners (corners);

		//return rectTransform.sizeDelta.y;

		return Mathf.Abs(corners[0].y - corners[2].y);
	}

	protected virtual void resized(){

		content.sizeDelta = new Vector2 (getWidth() * _pages, getHeight());
	}


	public float lastScreenWidth = 0f;
	public float lastScreenHeight = 0f;	
	// Update is called once per frame
	protected virtual void Update ()
	{
		if (targetPosition < scrollRect.horizontalNormalizedPosition) {
			scrollRect.horizontalNormalizedPosition -= Time.deltaTime * normalizedSpeed;
			if (scrollRect.horizontalNormalizedPosition < targetPosition)
				scrollRect.horizontalNormalizedPosition = targetPosition;
		} else if (targetPosition > scrollRect.horizontalNormalizedPosition) {
			scrollRect.horizontalNormalizedPosition += Time.deltaTime * normalizedSpeed;
			if (scrollRect.horizontalNormalizedPosition > targetPosition)
				scrollRect.horizontalNormalizedPosition = targetPosition;
		}
		

		if (lastScreenWidth != Screen.width || lastScreenHeight != Screen.height) {
			lastScreenWidth = Screen.width;
			lastScreenHeight = Screen.height;
			resized();
		}
	}
}

