using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class CatalogBrowser : ListViewController<string>, FurnitureListViewController.IFurnitureViewListener {

	public Catalog catalog;

	public delegate void SelectedFurniture(FurnitureListViewController sender, FurnitureEntry selectedEntry);
	public SelectedFurniture SelectedFurnitureHandle;

	public void onSelectedFurniture(FurnitureListViewController sender, FurnitureEntry selectedEntry){

		if (SelectedFurnitureHandle != null)
			SelectedFurnitureHandle (sender, selectedEntry);

	}

	protected override int numberOfElements(){
		return data.Count;
	}
	
	protected override bool elementSelected(int i){
		if (base.elementSelected (i))
			return true;

		FurnitureListViewController flv = ViewController.createViewController<FurnitureListViewController> (navigationController, data[i]);
		GameObject newViewController = flv.gameObject;

		flv.l.AddRange(catalog.entriesByCategory [data [i]]);
		flv.listener = this;
	

		navigationController.pushViewController (flv);

		return false;
	}
	


	// Use this for initialization
	protected override void Start () {
		this.navigationController.title.text = "Catalog";

		catalog = Catalog.getCatalog();
		data.AddRange (catalog.entriesByCategory.Keys);
		//navigationController.pushViewController (this);
		base.Start ();
	}


	// Update is called once per frame
	void Update () {
	
	}
}
