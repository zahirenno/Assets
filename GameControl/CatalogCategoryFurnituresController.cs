using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CatalogCategoryFurnituresController : MonoBehaviour
{

	public Preferences preferences;

	// Use this for initialization
	void Start ()
	{
		GameObject mainPanel = GameObject.Find ("FurnitureBrowser").gameObject;
		mainPanel.GetComponent<Image> ().color = preferences.darkBackColor;

		GameObject mainBar = GameObject.Find ("MainBar").gameObject;
		mainBar.GetComponent<Image> ().color = preferences.primaryColor;

		Button backButton = mainBar.transform.FindChild ("BackButton").GetComponent<Button> ();
		backButton.onClick.AddListener (() => onBackButtonPressed ());

		Text title = mainBar.transform.FindChild ("RoomTitle").GetComponent<Text> ();
		title.font = preferences.primaryTextFont;
		title.color = preferences.primaryTextColor;
		title.text = PlayerPrefs.GetString ("categoryViewer_category");
	}

	void onBackButtonPressed(){
		Application.LoadLevel ("catalogViewer");
	}

	// Update is called once per frame
	void Update ()
	{
	
	}
}

