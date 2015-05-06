using UnityEngine;
using System.Collections;

public class FirstCatalogPageController : PagedScrollView
{

	public RectTransform homePage, mainCatalog;

	// Use this for initialization
	protected override void Start ()
	{
		Pages = 2;
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
		homePage.localPosition = new Vector2 (-getWidth()/2.0f, 0);


		if (mainCatalog == null) {
			mainCatalog = new GameObject ("MainCatalog").AddComponent<RectTransform> ();
			mainCatalog.gameObject.AddComponent<FurnitureGridViewController> ();
		}
		mainCatalog.SetParent (content);
		mainCatalog.sizeDelta = new Vector2 (getWidth (), getHeight ());
		mainCatalog.localPosition = new Vector2 (getWidth() / 2.0f, 0);

		
		scrollRect.horizontalNormalizedPosition = 1.0f;
	}

}

