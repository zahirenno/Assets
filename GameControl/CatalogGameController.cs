using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CatalogGameController : MonoBehaviour {

	public List<FurnitureEntry> wishList = new List<FurnitureEntry> ();

	public Preferences preferences;
	public PagedScrollView catalogView;

	private MenuSlide menuBar;

	public List<Button> tabs = new List<Button> ();

	// Use this for initialization
	void Start () {
		GameObject.Find ("MainBar").GetComponent<Image> ().color = preferences.primaryColor;
		GameObject.Find ("TheGreaterContainer").GetComponent<Image> ().color = preferences.darkBackColor;
		//GameObject.Find ("MainBar").transform.FindChild("Title").GetComponent<Text>().color = preferences.primaryTextColor;
		//GameObject.Find ("MainBar").transform.FindChild("Title").GetComponent<Text>().font = preferences.primaryTextFont;
		GameObject.Find ("newRoomButton").GetComponent<Image> ().color = preferences.secondaryColor;
		GameObject.Find ("newRoomButton").GetComponent<Button> ().onClick.AddListener (() => newRoomButtonClicked ());
		GameObject.Find ("ShowMenuButton").GetComponent<Button> ().onClick.AddListener (() => onHideMenuButtonClicked ());

		menuBar = GameObject.Find ("MenuSlider").GetComponent<MenuSlide> ();

		foreach (Button tab in tabs) {
			Button t = tab;

			tab.GetComponent<Text>().font = preferences.primaryTextFont;
			Color c = preferences.primaryTextColor;
			c.a = 0.7f;
			tab.GetComponent<Text>().color = c;
			tab.onClick.AddListener (() => onTabClicked (t));
		}
	}

	void onTabClicked(Button sender){
		Debug.Log ("dsg");

		foreach (Button tab in tabs) {
			Color c = preferences.primaryTextColor;
			c.a = 0.3f;
			tab.GetComponent<Text>().color = c;

		}
		sender.GetComponent<Text> ().color = preferences.primaryTextColor;

		catalogView.goToPage (tabs.IndexOf (sender));
	}

	void onHideMenuButtonClicked(){
		menuBar.visible = !menuBar.visible;
	}

	void newRoomButtonClicked(){
		Application.LoadLevel ("EmptyRoomCreation");
	}

	// Update is called once per frame
	void Update () {
	
	}
}
