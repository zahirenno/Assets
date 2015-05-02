using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CatalogGameController : MonoBehaviour {

	public Preferences preferences;

	// Use this for initialization
	void Start () {
		GameObject.Find ("MainBar").GetComponent<Image> ().color = preferences.primaryColor;
		GameObject.Find ("GridCatalog").GetComponent<Image> ().color = preferences.primaryColor;
		GameObject.Find("Title").GetComponent<Text>().color = preferences.primaryTextColor;
		GameObject.Find("Title").GetComponent<Text>().font = preferences.primaryTextFont;


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
