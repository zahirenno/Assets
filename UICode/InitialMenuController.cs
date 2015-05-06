using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class InitialMenuController : ListViewController<string> {

	private List<string> icons = new List<string> ();

	// Use this for initialization
	protected override void Start ()
	{
		cellName = "LabelListItem";
		
		data.Add ("Store Locator");
		data.Add ("Loved List");
		data.Add ("Cart");
		data.Add ("Create Room");
		data.Add ("Settings");
		data.Add ("Help");
		data.Add ("About");
		
		icons.Add ("icons/ic_explore_white_48dp");
		icons.Add ("icons/ic_favorite_white_48dp");
		icons.Add ("icons/ic_shopping_cart_white_48dp");
		icons.Add ("icons/ic_home_white_48dp");
		icons.Add ("icons/ic_settings_white_48dp");
		icons.Add ("icons/ic_help_white_48dp");
		icons.Add ("icons/ic_info_white_48dp");
		
		navigationController.title.text = "Posh.";
		
		base.Start ();
	}
	
	protected override RectTransform elementAt(int i, GameObject reuseElement){
		RectTransform result = base.elementAt (i, reuseElement);
		
		result.FindChild ("Image").GetComponent<Image> ().sprite = Resources.Load<Sprite> (icons [i]);
		result.FindChild ("Image").GetComponent<Image> ().color = preferences.primaryColor;
		return result;
	}
	
	protected override bool elementSelected(int i){
		if (base.elementSelected (i))
			return true;
		
		switch (i) {
		case 0:
			break;

		}
		
		return false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
