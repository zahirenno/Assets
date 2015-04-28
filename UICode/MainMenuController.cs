using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MainMenuController : ListViewController<string>
{

	private List<string> icons = new List<string> ();

	// Use this for initialization
	protected override void Start ()
	{
		cellName = "LabelListItem";

		data.Add ("Catalog");
		data.Add ("Suggestions");
		data.Add ("Rendering");
		data.Add ("View Mode");

		icons.Add ("icons/ic_store_white_48dp");
		icons.Add ("icons/ic_cached_white_48dp");
		icons.Add ("icons/ic_extension_white_48dp");
		icons.Add ("icons/ic_visibility_white_48dp");

		navigationController.title.text = "Posh.";

		base.Start ();
	}

	protected override RectTransform elementAt(int i, GameObject reuseElement){
		RectTransform result = base.elementAt (i, reuseElement);

		result.FindChild ("Image").GetComponent<Image> ().sprite = Resources.Load<Sprite> (icons [i]);
		result.FindChild ("Image").GetComponent<Image> ().color = FirstController.getGlobalFirstController ().prefrences.primaryColor;
		return result;
	}

	protected override bool elementSelected(int i){
		if (base.elementSelected (i))
			return true;

		switch (i) {
		case 0:
			CatalogBrowser cb = ViewController.createViewController<CatalogBrowser>(this.navigationController, "Catalog");
			this.navigationController.pushViewController(cb);
			break;
		case 1:
			SuggestionMenuController sn = ViewController.createViewController<SuggestionMenuController>(this.navigationController, "Suggestions");
			this.navigationController.pushViewController(sn);
			break;
		case 2:
			RenderingMenuController rn = ViewController.createViewController<RenderingMenuController>(this.navigationController, "Shaders");
			this.navigationController.pushViewController(rn);
			break;
		case 3:
			ViewModeMenuController vmn = ViewController.createViewController<ViewModeMenuController>(this.navigationController, "View Mode");
			this.navigationController.pushViewController(vmn);
			break;
		}

		return false;
	}

	protected override int cellHeight(){
		return 50;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}

