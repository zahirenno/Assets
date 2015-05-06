using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HomeFurnitureGridViewController : ListViewController<List<FurnitureEntry>>
{
	private Catalog catalog;
	
	public int width = 3;
	private GameObject cellPrefab;
	
	private RectTransform r_transform;

	private List<string> titles = new List<string>();

	// Use this for initialization
	protected override void Start ()
	{
		base.Start ();
		
		r_transform = (RectTransform)(transform);
		
		cellName = "FurniturePreviewCellPrefab";
		
		catalog = Catalog.getCatalog();

		titles.Add ("Summer Colors");
		data.Add(new List<FurnitureEntry>());
		data [data.Count - 1].Add (catalog.entries ["sofa1#9"]);
		data [data.Count - 1].Add (catalog.entries ["sofa2#13"]);
		data [data.Count - 1].Add (catalog.entries ["sofa2#17"]);


		titles.Add ("Hot Items");
		data.Add(new List<FurnitureEntry>());
		data [data.Count - 1].Add (catalog.entries ["GdB#armchair2"]);
		data [data.Count - 1].Add (catalog.entries ["GdB#computerWorkstation"]);
		data [data.Count - 1].Add (catalog.entries ["Renouzate#armchair"]);

		titles.Add ("Avantgarde");
		data.Add(new List<FurnitureEntry>());
		data [data.Count - 1].Add (catalog.entries ["Renouzate#Table3x3"]);
		data [data.Count - 1].Add (catalog.entries ["Sheep#aquarium"]);
		data [data.Count - 1].Add (catalog.entries ["Geantick#barStool"]);

		titles.Add ("Chique");
		data.Add(new List<FurnitureEntry>());
		data [data.Count - 1].Add (catalog.entries ["sofa1#1"]);
		data [data.Count - 1].Add (catalog.entries ["sofa1#4"]);
		data [data.Count - 1].Add (catalog.entries ["sofa1#5"]);

		titles.Add ("Italian Leather");
		data.Add(new List<FurnitureEntry>());
		data [data.Count - 1].Add (catalog.entries ["Renouzate#sofa2"]);
		data [data.Count - 1].Add (catalog.entries ["sofa1#22"]);
		data [data.Count - 1].Add (catalog.entries ["sofa2#44"]);
		
		cellPrefab = Resources.Load<GameObject> (cellName);
	}
	
	protected virtual void cellSelected(GameObject sender, int i){
		
		PlayerPrefs.SetString ("modelViewer_modelid", sender.GetComponent<FurniturePreviewCellView> ().furnitureEntry.id);
		Application.LoadLevel ("modelViewer");
		
	}
	
	protected override int cellHeight(){
		return (int)((Mathf.Abs(r_transform.sizeDelta.x) * 286.0f / 230.0f / (float)width) * 1.3f);
	}
	
	protected override RectTransform elementAt(int i, GameObject reuseElement){
		
		GameObject holder = reuseElement;
		
		//TODO make it work without this... when you feel better that is.
		GameObject.Destroy (holder);
		holder = null;
		
		if (holder == null)
			holder = new GameObject ();
		holder.name = "Line_" + i;
		
		holder.AddComponent<RectTransform> ().sizeDelta = new Vector2(Mathf.Abs(r_transform.sizeDelta.x), 
		                                                              cellHeight());
		//TODO set holder dimensions
		
		GameObject title = new GameObject("LineLabel");
		RectTransform labelRT = title.AddComponent<RectTransform>();
		labelRT.SetParent(holder.transform);
		labelRT.sizeDelta = new Vector2(((RectTransform)(holder.transform)).sizeDelta.x * 0.9f,
		                                ((RectTransform)(holder.transform)).sizeDelta.y * 0.12f);
		labelRT.localPosition = new Vector2(0, ((RectTransform)holder.transform).sizeDelta.y / 2.0f - labelRT.sizeDelta.y / 2.0f);
		Text t = title.AddComponent<Text>();
		t.text = titles [i];
		t.resizeTextForBestFit = true;
		t.resizeTextMinSize = 10;
		t.resizeTextMaxSize = 50;
		t.font = GameObject.Find("GameObject").GetComponent<Preferences>().primaryTextFont;
		
		int j = -1;
		foreach (FurnitureEntry fe in data[i]) {
			
			GameObject fecv = (GameObject)GameObject.Instantiate(cellPrefab);
			FurniturePreviewCellView fpcv = fecv.GetComponent<FurniturePreviewCellView>();
			fpcv.init();
			fpcv.furnitureEntry = fe;
			fpcv.cellButton.onClick.AddListener(()=>cellSelected(fpcv.gameObject, i));
			
			((RectTransform)(fpcv.transform)).sizeDelta = new Vector2(((RectTransform)(holder.transform)).sizeDelta.x / (float)width,
			                                                          ((RectTransform)(holder.transform)).sizeDelta.y / 1.3f);
			
			
			((RectTransform)(fpcv.transform)).localPosition = new Vector2((float)j * ((RectTransform)(holder.transform)).sizeDelta.x / (float)width, 0);
			++j;
			
			((RectTransform)(fpcv.transform)).localScale = new Vector3(.98f, .98f, .98f);
			
			fpcv.transform.SetParent(holder.transform);
		}
		
		
		return ((RectTransform)(holder.transform));
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}

