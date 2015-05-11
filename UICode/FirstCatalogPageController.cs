using UnityEngine;
using System.Collections;

public class FirstCatalogPageController : PagedScrollView
{

	public RectTransform homePage, mainCatalog, categoryPicker;

	// Use this for initialization
	protected override void Start ()
	{
		Pages = 3;
		base.Start ();

	}

	protected override void resized(){
		base.resized ();

		if (homePage == null) {
			homePage = new GameObject ("HomePage").AddComponent<RectTransform> ();
			homePage.gameObject.AddComponent<HomeFurnitureGridViewController> ();
		}
		homePage.SetParent (content);
		homePage.sizeDelta = new Vector2 (getWidth (), getHeight ());
		homePage.localPosition = new Vector2 (-getWidth(), 0);


		if (mainCatalog == null) {
			mainCatalog = new GameObject ("MainCatalog").AddComponent<RectTransform> ();
			mainCatalog.gameObject.AddComponent<FurnitureGridViewController> ();
		}
		mainCatalog.SetParent (content);
		mainCatalog.sizeDelta = new Vector2 (getWidth (), getHeight ());
		mainCatalog.localPosition = new Vector2 (0, 0);

		if (categoryPicker == null) {
			categoryPicker = new GameObject("CategoryPicker").AddComponent<RectTransform>();
			categoryPicker.gameObject.AddComponent<CategoryPickerListView>();
		}
		categoryPicker.SetParent (content);
		categoryPicker.sizeDelta = new Vector2 (getWidth (), getHeight ());
		categoryPicker.localPosition = new Vector2 (getWidth(), 0);

		//scrollRect.horizontalNormalizedPosition = 1.0f;
		goToPage (0, false);
	}

}

