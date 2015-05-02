using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FurnitureGridViewController : ListViewController<List<FurnitureEntry>>
{
	private Catalog catalog;

	public int width = 3;
	private GameObject cellPrefab;

	private RectTransform r_transform;

	// Use this for initialization
	protected override void Start ()
	{
		base.Start ();

		r_transform = (RectTransform)(transform.parent);

		cellName = "FurniturePreviewCellPrefab";

		catalog = Catalog.getCatalog();

		data.Add(new List<FurnitureEntry>());
		int i = 0;
		foreach (FurnitureEntry fe in catalog.entries.Values) {
			++i;
			if (i > width){
				data.Add(new List<FurnitureEntry>());
				i = 1;
			}

			data[data.Count - 1].Add(fe);
		}

		cellPrefab = Resources.Load<GameObject> (cellName);
	}

	protected virtual void cellSelected(GameObject sender, int i){
		Debug.Log (sender.GetComponent<FurniturePreviewCellView> ().furnitureEntry.name);
	}

	protected override int cellHeight(){
		return (int)(Mathf.Abs(r_transform.sizeDelta.x) * 286.0f / 230.0f / (float)width);
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
		                                                              Mathf.Abs(r_transform.sizeDelta.x) * 286.0f / 230.0f / (float)width);
		//TODO set holder dimensions


		int j = -1;
		foreach (FurnitureEntry fe in data[i]) {
			GameObject fecv = (GameObject)GameObject.Instantiate(cellPrefab);
			FurniturePreviewCellView fpcv = fecv.GetComponent<FurniturePreviewCellView>();
			fpcv.init();
			fpcv.furnitureEntry = fe;
			fpcv.cellButton.onClick.AddListener(()=>cellSelected(fpcv.gameObject, i));

			((RectTransform)(fpcv.transform)).sizeDelta = new Vector2(((RectTransform)(holder.transform)).sizeDelta.x / (float)width,
			                                                          ((RectTransform)(holder.transform)).sizeDelta.y);


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

