using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FurnitureCategoryGridViewController : FurnitureGridViewController
{
	protected override ICollection<FurnitureEntry> furnituresToAdd(){
		return catalog.entriesByCategory [PlayerPrefs.GetString("categoryViewer_category")];
	}
}

