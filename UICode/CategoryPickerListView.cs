using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CategoryPickerListView : ListViewController<string>
{
	private Catalog catalog;

	protected override bool elementSelected(int i){
		PlayerPrefs.SetString ("categoryViewer_category", data[i]);
		Application.LoadLevel ("catalogCategoryViewer");

		return false;
	}

	protected override int cellHeight ()
	{
		return 150;
	}

	protected override RectTransform elementAt (int i, GameObject reuseElement)
	{
		RectTransform e = base.elementAt (i, reuseElement);
		e.GetComponentInChildren<Text> ().resizeTextMaxSize = 50;
		return e;
	}

	// Use this for initialization
	protected override void Start ()
	{
		base.Start ();
		this.preferences = GameObject.Find ("GameObject").GetComponent<Preferences> ();
		cellName = "EvenMoreStandardListItem";
		catalog = Catalog.getCatalog ();
		this.data.AddRange (catalog.entriesByCategory.Keys);
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}

