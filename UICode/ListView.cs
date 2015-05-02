using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public interface ListViewListener{
	void elementSelected(int i);
	RectTransform elementAt (int i, GameObject reuseElement);
	int numberOfElements();
	int cellHeight();
}

public class ListView : MonoBehaviour {

	public bool dirty = true;

	public GameObject contentRect;
	private RectTransform rectTransform;
	private ScrollRect scrollRect;

	public Component listener1;
	public ListViewListener listener;

	// Use this for initialization
	void Start () {
		listener = (ListViewListener)listener1;

		contentRect = new GameObject ("Content");
		contentRect.AddComponent<RectTransform> ();
		contentRect.transform.SetParent (this.transform);
		
		rectTransform = (RectTransform)contentRect.transform;
		rectTransform.anchoredPosition = new Vector2 (0, 0);
		
		scrollRect = GetComponent<ScrollRect> ();
		if (scrollRect == null)
			scrollRect = gameObject.AddComponent<ScrollRect> ();
		scrollRect.content = rectTransform;
		scrollRect.vertical = true;
		scrollRect.horizontal = false;

		scrollRect.onValueChanged.AddListener ((arg) => onScroll (arg));

		Mask m = GetComponent<Mask> ();
		if (m == null)
			m = gameObject.AddComponent<Mask> ();
		m.showMaskGraphic = true;
	}

	public void onScroll(Vector2 arg){
		updateVisible ();
	}

	void refreshList(){

		foreach (Transform child in contentRect.transform)
			GameObject.Destroy (child.gameObject);

		float height = 0;
		for (int i = 0; i < listener.numberOfElements(); ++i) {
			int finali = i;

			height += listener.cellHeight();
		}

		rectTransform.sizeDelta = new Vector2 (rectTransform.sizeDelta.x, height);

		if (numberOfItemsPerPage > listener.numberOfElements ())
			numberOfItemsPerPage = listener.numberOfElements();

		scrollRect.verticalNormalizedPosition = 1.0f;

		updateVisible ();
	}

	// Update is called once per frame
	void Update () {
		if (dirty) {
			dirty = false;
			refreshList();
		}

	}


	private int numberOfItemsPerPage = 10;
	//private int cellHeight = 100;
	protected void resized(){
		RectTransform sct = (RectTransform)scrollRect.transform;
		Vector3[] g = new Vector3[4];
		sct.GetWorldCorners(g);
		float topy = g[0].y;
		float bottomy = g[2].y;

		numberOfItemsPerPage = ((int)Mathf.Abs(topy - bottomy) / listener.cellHeight()) + 1;
		if (numberOfItemsPerPage > listener.numberOfElements ())
			numberOfItemsPerPage = listener.numberOfElements();
	}
	
	private Dictionary<GameObject, int> elementIndex = new Dictionary<GameObject, int>();
	private List<GameObject> reusable = new List<GameObject> ();
	protected void updateVisible(){

		RectTransform sct = (RectTransform)scrollRect.transform;
		Vector3[] g = new Vector3[4];
		sct.GetWorldCorners(g);
		float topy = g[0].y;
		float bottomy = g[2].y;

		float position = (1.0f - scrollRect.verticalNormalizedPosition) * (1.0f - Mathf.Abs(topy - bottomy) / (float)(listener.cellHeight() * listener.numberOfElements()));

		int i_start = Mathf.FloorToInt(position * (float)listener.numberOfElements());

		if (i_start < 0)
			i_start = 0;

		int i_end = i_start + numberOfItemsPerPage;

		if (i_end < 0)
			i_end = 0;
		if (i_end >= listener.numberOfElements ())
			i_end = listener.numberOfElements () - 1;

		if (numberOfItemsPerPage >= Mathf.Abs (topy - bottomy)) {
			i_start = 0;
			i_end = listener.numberOfElements() - 1;
		}

		List<GameObject> keys = new List<GameObject> ();
		keys.AddRange (elementIndex.Keys);
		foreach (GameObject go in keys) {
			int ei = elementIndex[go];
			if (ei < i_start || ei > i_end){
				reusable.Add(go);
				decomission(go);
				elementIndex.Remove(go);
			}
		}

		for (int i = i_start; i <= i_end; ++i) {
			int finali = i;

			if (elementIndex.ContainsValue(i))
				continue;

			GameObject button = null;

			if (reusable.Count > 0){
				button = reusable[0];
				reusable.RemoveAt(0);
				button.SetActive(true);
			}
			button = listener.elementAt(i, button).gameObject;

			RectTransform element = (RectTransform)button.transform;
			element.SetParent(contentRect.transform);

			Button buttonB = button.GetComponent<Button>();
			if (buttonB != null){
				buttonB.onClick.RemoveAllListeners();
				buttonB.onClick.AddListener(() => listener.elementSelected(finali));
			}

			element.anchorMax = new Vector2(0.5f, 1.0f);
			element.anchorMin = new Vector2(0.5f, 1.0f);
			element.anchoredPosition = new Vector2(0, - (int)(((float)i + 0.5f) * (float)listener.cellHeight())); //potential bug for different sizes

			elementIndex.Add(button, i);
		}

	}

	protected void decomission(GameObject button){
		if (button.GetComponent<Button> () != null)
			button.GetComponent<Button> ().onClick.RemoveAllListeners ();
		if (button.GetComponent<Image> () != null)
			button.GetComponent<Image> ().sprite = null;
		if (button.GetComponent<Text>() != null)
			button.GetComponentInChildren<Text> ().text = "";
		button.SetActive (false);
	}
}
