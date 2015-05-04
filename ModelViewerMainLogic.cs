using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using TouchScript.Gestures;
using TouchScript;

public class ModelViewerMainLogic : SimpleGameContoller {

	public Catalog catalog = null;
	public FurnitureEntry furnitureEntry = null;
	public Preferences preferences;

	// Use this for initialization
	protected override void Start () {

		base.Start ();

		catalog = Catalog.getCatalog ();

		furnitureEntry = catalog.entries [PlayerPrefs.GetString("modelViewer_modelid")];
		GameObject furniture = catalog.createFurniture (furnitureEntry.id, new Vector3 (0, 0, 0), 0);
		GameObject floor = (GameObject)GameObject.Instantiate (Resources.Load<GameObject> ("FloorPlane"));
		//floor.transform.SetParent (furniture.transform);
		floor.GetComponent<Floor> ().Height = furnitureEntry.depth * 3.0f;
		floor.GetComponent<Floor> ().Width = furnitureEntry.width * 3.0f;

		GameObject.Find ("boldButton").GetComponent<Image> ().color = preferences.secondaryColor;
		GameObject.Find ("MainBar").GetComponent<Image> ().color = preferences.primaryColor;
		GameObject.Find("FurnitureTitle").GetComponent<Text>().color = preferences.primaryTextColor;
		GameObject.Find("FurnitureTitle").GetComponent<Text>().font = preferences.primaryTextFont;
		GameObject.Find("FurnitureTitle").GetComponent<Text>().text = furnitureEntry.name;
		GameObject.Find ("MainBar").transform.FindChild ("BackButton").GetComponent<Button> ().onClick.AddListener (() => backPressed ());
	}

	void backPressed(){
		Application.LoadLevel ("catalogViewer");
	}

	// Update is called once per frame
	void Update () {
		foreach (MeshCollider mc in FindObjectsOfType<MeshCollider> ()) {
			if (mc.gameObject.GetComponent<TouchScript.Hit.Untouchable> () == null)
				mc.gameObject.AddComponent<TouchScript.Hit.Untouchable> ();
		}
	}
}
