using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CatalogGameController : MonoBehaviour {

	public Preferences preferences;

	// Use this for initialization
	void Start () {
		GameObject.Find ("MainBar").GetComponent<Image> ().color = preferences.primaryColor;
		GameObject.Find ("GridCatalog").GetComponent<Image> ().color = preferences.darkBackColor;
		GameObject.Find("Title").GetComponent<Text>().color = preferences.primaryTextColor;
		GameObject.Find("Title").GetComponent<Text>().font = preferences.primaryTextFont;
		GameObject.Find ("newRoomButton").GetComponent<Image> ().color = preferences.secondaryColor;
		GameObject.Find ("newRoomButton").GetComponent<Button> ().onClick.AddListener (() => newRoomButtonClicked ());
	}

	void newRoomButtonClicked(){
		Application.LoadLevel ("j");
	}

	// Update is called once per frame
	void Update () {
	
	}
}
