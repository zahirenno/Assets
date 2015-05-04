using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FurnitureDetailView : MonoBehaviour {

	private RectTransform r_transform;
	private RectTransform r_parent_transform;
	public RectTransform r_button_transform = null;
	public RectTransform r_content_transform = null;
	public Preferences preferences = null;
	public bool visible = false;
	public float animationTime = 0.3f;

	private Vector2 speed;
	private Vector2 expandedPosition, collapsedPosition;

	Text createText(string v){
		GameObject t = new GameObject ("tt");
		RectTransform r_text_transform = t.AddComponent<RectTransform> ();

		Text te = t.AddComponent<Text> ();
		te.fontSize = 40;
		te.text = v;
		te.font = preferences.primaryTextFont;
		te.transform.parent = r_content_transform;

		return te;
	}

	Text createPrimaryText(string v){
		Text te = createText (v);
		te.color = preferences.primaryListTextColor;
		te.fontSize = 40;
		return te;
	}

	Text createSecondaryText(string v){
		Text te = createText (v);
		te.color = preferences.secondaryListTextColor;
		te.fontSize = 40;
		return te;
	}

	void displayInfos(){
		r_content_transform.GetComponent<VerticalLayoutGroup> ().spacing = 15;

		Catalog c = Catalog.getCatalog ();
		FurnitureEntry fe = c.entries [PlayerPrefs.GetString("modelViewer_modelid")];

		createPrimaryText ("Name:");
		createSecondaryText (fe.name);
		createPrimaryText ("Brand:");
		createSecondaryText ("IKEA");
		createPrimaryText ("Category:");
		createSecondaryText (fe.category);
		createPrimaryText ("Dimensions:");
		createSecondaryText (fe.depth + "m x " + fe.height + "m x " + fe.width + "m");



	}

	// Use this for initialization
	void Start () {
		r_transform = (RectTransform)transform;
		r_parent_transform = (RectTransform)transform.parent;
		r_button_transform.GetComponent<Button> ().onClick.AddListener (() => handleVisibleHideToggle ());

		r_transform.sizeDelta = new Vector2 (r_parent_transform.sizeDelta.x * 0.5f, 
		                                     r_parent_transform.sizeDelta.y - r_button_transform.sizeDelta.y / 2.0f);
		
		expandedPosition = new Vector2 (0,
		                                -r_button_transform.sizeDelta.y / 2.0f);

		collapsedPosition = new Vector2 (0, 
		                                -r_transform.sizeDelta.y + r_button_transform.sizeDelta.y * (1.0f - 0.5f));

		r_transform.localPosition = collapsedPosition;

		r_content_transform.sizeDelta = new Vector2 (r_content_transform.sizeDelta.x, 8 * 100);


		r_button_transform.sizeDelta = new Vector2 (r_transform.sizeDelta.x / 5.0f, r_transform.sizeDelta.x / 5.0f);
		r_button_transform.position = r_transform.position + new Vector3(r_transform.sizeDelta.x / 2.0f - r_button_transform.sizeDelta.x * (1.0f / 2.0f + 1.0f / 10.0f),
		                                                                 r_transform.sizeDelta.y / 2.0f,
		                                                                 0);

		displayInfos ();
	}

	void handleVisibleHideToggle(){
		visible = !visible;
		animating = true;

		if (visible) {
			speed = (expandedPosition - collapsedPosition) / animationTime;
		} else {
			speed = -(expandedPosition - collapsedPosition) / animationTime;
		}
	}

	private bool animating = false;

	// Update is called once per frame
	void Update () {


		Vector2 v = speed * Time.deltaTime;
		if (animating) {
			r_transform.localPosition = r_transform.localPosition + new Vector3 (v.x, v.y, 0);
			if (visible) {
				if (r_transform.localPosition.y > expandedPosition.y) {
					r_transform.localPosition = expandedPosition;
					animating = false;
				}
			} else {
				if (r_transform.localPosition.y < collapsedPosition.y) {
					r_transform.localPosition = collapsedPosition;
					animating = false;
				}
			}

			r_button_transform.position = r_transform.position + new Vector3(r_transform.sizeDelta.x / 2.0f - r_button_transform.sizeDelta.x * (1.0f / 2.0f + 1.0f / 10.0f),
			                                                                 r_transform.sizeDelta.y / 2.0f,
			                                                                 0);
		}

		//r_transform.GetComponent<ScrollRect> ().normalizedPosition = new Vector2(0,1.0f);

		//Debug.Log (r_transform.GetComponent<ScrollRect> ().normalizedPosition);
	}
}
