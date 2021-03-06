﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FurnitureListViewController : ViewController, ListViewListener{

	public interface IFurnitureViewListener{
		void onSelectedFurniture(FurnitureListViewController sender, FurnitureEntry selectedEntry);
		void onDraggedFurniture(FurnitureListViewController sender, FurnitureEntry selectedEntry, Vector2 screenPosition);
	}
	public IFurnitureViewListener listener;

	public List<FurnitureEntry> l = new List<FurnitureEntry> ();

	int ListViewListener.numberOfElements(){
		return l.Count;
	}

	void ListViewListener.elementSelected(int i){
		if (firstController != null) {
			if (firstController.onListItemClicked(this, i, l[i]))
				return;
		}

		if (listener != null)
			listener.onSelectedFurniture (this, l [i]);
	}

	int ListViewListener.cellHeight(){
		return 170;
	}

	RectTransform ListViewListener.elementAt(int i, GameObject reuseElement){

		GameObject element = reuseElement;
		if (element == null)
			element = GameObject.Instantiate (Resources.Load<GameObject> ("StandardListItem"));
		element.name = "Element_" + i;


		RectTransform elementTransform = element.GetComponent<RectTransform> ();

		elementTransform.sizeDelta = new Vector2 (170, 170);

		Sprite sp = Resources.Load<Sprite> ("transparentThumbnails/" + l[i].id);

		Image image = element.GetComponent<Image> ();
		image.sprite = sp;

		Text elementText = element.GetComponentInChildren<Text> ();
		elementText.text = "";
		if (sp == null) {
			elementText.text = l [i].name;
			elementText.font = Font.CreateDynamicFontFromOSFont (Font.GetOSInstalledFontNames () [0], 32);
			elementText.alignment = TextAnchor.MiddleCenter;
			elementText.verticalOverflow = VerticalWrapMode.Truncate;
			elementText.horizontalOverflow = HorizontalWrapMode.Wrap;
			elementText.resizeTextForBestFit = true;
			elementText.fontSize = 25;
		}

		int final_i = i;
		elementTransform.GetComponent<DragOut>().onDraggedOut = (Vector2 position) => listener.onDraggedFurniture(this, l[final_i], position);

		return elementTransform;
	}

	// Use this for initialization
	void Start () {
		ListView lv = this.gameObject.AddComponent<ListView> ();
		lv.listener1 = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
