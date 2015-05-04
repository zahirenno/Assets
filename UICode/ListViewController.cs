using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ListViewController<T> : ViewController, ListViewListener
{

	public List<T> data = new List<T>();
	public string cellName = "StandardListItem";

	int ListViewListener.numberOfElements(){
		return this.numberOfElements ();
	}
	
	void ListViewListener.elementSelected(int i){
		this.elementSelected (i);
	}

	int ListViewListener.cellHeight(){
		return this.cellHeight ();
	}

	RectTransform ListViewListener.elementAt(int i, GameObject reuseElement){
		return this.elementAt (i, reuseElement);
	}

	protected virtual int numberOfElements(){
		return data.Count;
	}

	protected virtual bool elementSelected(int i){
		if (firstController != null) {
			if (firstController.onListItemClicked(this, i, data[i]))
				return true;
		}
		return false;
	}

	protected virtual int cellHeight(){
		return 100;
	}

	// Use this for initialization
	protected virtual void Start ()
	{
		ListView l = gameObject.GetComponent<ListView> ();
		if (l == null)
			l = gameObject.AddComponent<ListView> ();
		l.listener1 = this;
	}

	protected virtual RectTransform elementAt(int i, GameObject reuseElement){	
		GameObject element = reuseElement;
		if (element == null)
			element = GameObject.Instantiate (Resources.Load<GameObject> (cellName));
		element.name = "Element_" + i;
		
		
		RectTransform elementTransform = element.GetComponent<RectTransform> ();

		RectTransform tr = (RectTransform)this.transform;
		Vector3[] g = new Vector3[4];
		tr.GetWorldCorners(g);
		float leftx = g[0].x;
		float rightx = g[2].x;

		elementTransform.sizeDelta = new Vector2 (Mathf.Abs(leftx - rightx), this.cellHeight());
		
		Sprite sp = null;
		
		Image image = element.GetComponent<Image> ();
		//image.sprite = sp;
		
		if (sp == null) {
			Text elementText = element.GetComponentInChildren<Text> ();
			
			elementText.text = data [i].ToString();
			elementText.font = FirstController.getGlobalFirstController().prefrences.secondaryTextFont;
			elementText.alignment = TextAnchor.MiddleCenter;
			elementText.verticalOverflow = VerticalWrapMode.Truncate;
			elementText.horizontalOverflow = HorizontalWrapMode.Wrap;
			elementText.resizeTextForBestFit = true;
			elementText.resizeTextMinSize = 10;
			elementText.resizeTextMaxSize = 25;
			//elementText.fontSize = 25;
		}
		
		
		return elementTransform;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}

