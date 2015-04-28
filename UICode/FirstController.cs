using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FirstController : MonoBehaviour {

	public Preferences prefrences;

	public static FirstController getGlobalFirstController(){
		GameObject ui = GameObject.Find ("UI");
		if (ui == null)
			Debug.LogError("You need an object named UI that has a FirstController component. This object is usually a Canvas");
		return ui.GetComponent<FirstController> ();
	}

	protected ViewController currentViewController = null;


	public bool onButtonClick (Button sender){
		if (sender.Equals(deleteButton)) {
			return this.onDeleteButtonClicked(sender);
		}
		return false;
	}

	public void onSwitchToPage (ViewController to){
		if (to is FurnitureListViewController) {
			onSwitchToFurnitureList((FurnitureListViewController)to);
		} else if (to is CatalogBrowser) {
			onSwitchToCatalogCategories((CatalogBrowser)to);
		}

		currentViewController = to;
	}

	public bool onListItemClicked (object sender, int index, object obj){
		if (sender is FurnitureListViewController) {
			return this.onFurniturePicked (currentViewController, (FurnitureEntry)obj);
		} else if (sender is CatalogBrowser) {
			return this.onFurnitureCategoryPicked(currentViewController, (string)obj);
		}
		return false;
	}

	public delegate bool DFurniturePicked(ViewController sender, FurnitureEntry entry);
	public List<DFurniturePicked> dFurniturePicked = new List<DFurniturePicked>();
	public delegate bool DFurnitureCategoryPicked(ViewController sender, string categoryId);
	public List<DFurnitureCategoryPicked> dFurnitureCategoryPicked = new List<DFurnitureCategoryPicked>();
	public delegate bool DButtonClicked(Button sender);

	public List<DButtonClicked> dDeleteButtonClicked = new List<DButtonClicked>();
	public List<DButtonClicked> dUnknownButtonClicked = new List<DButtonClicked>();
	public List<DButtonClicked> dRegenButtonClicked = new List<DButtonClicked> ();
	public List<DButtonClicked> dUndoButtonClicked = new List<DButtonClicked> ();

	void onSwitchToCatalogCategories(CatalogBrowser browser){
		Debug.Log ("Presented Furniture Categories");
	}

	void onSwitchToFurnitureList(FurnitureListViewController browser){
		Debug.Log ("Presented Furnitures");
	}

	bool onFurniturePicked(ViewController sender, FurnitureEntry entry){
		Debug.Log ("Picked Furniture: " + entry.model);

		bool res = false;
		foreach (DFurniturePicked df in dFurniturePicked)
			if (df.Invoke (sender, entry))
				res = true;
		return res;
	}

	bool onFurnitureCategoryPicked(ViewController sender, string categoryId){
		Debug.Log ("Picked Furniture Category: " + categoryId);

		bool res = false;
		List<DFurnitureCategoryPicked> lsCopy = new List<DFurnitureCategoryPicked> ();
		lsCopy.AddRange (dFurnitureCategoryPicked);
		foreach (DFurnitureCategoryPicked df in lsCopy)
			if (df.Invoke (sender, categoryId))
				res = true;
		return res;
	}

	bool onDeleteButtonClicked(Button sender){
		Debug.Log ("Delete Pressed");

		bool res = false;
		List<DButtonClicked> lsCopy = new List<DButtonClicked> ();
		lsCopy.AddRange (dDeleteButtonClicked);
		foreach (DButtonClicked df in lsCopy)
			if (df.Invoke (sender))
				res = true;
		return res;
	}

	bool onNextRoomButtonClicked(Button sender){
		Debug.Log ("Next Room Pressed");
		return false;
	}
	bool onPrevRoomButtonClicked(Button sender){
		Debug.Log ("Prev Room Pressed");
		return false;
	}
	bool onNewRoomButtonClicked(Button sender){
		Debug.Log ("New Room Pressed");
		return false;
	}
	bool onDeleteRoomButtonClicked(Button sender){
		Debug.Log ("Delete Room Pressed");
		return false;
	}

	bool onShowMenuButtonClicked(Button sender){
		menuBar.GetComponent<MenuSlide> ().visible = !menuBar.GetComponent<MenuSlide> ().visible;
		return false;
	}

	bool onRegenButtonClicked(Button sender){
		bool res = false;
		List<DButtonClicked> lsCopy = new List<DButtonClicked> ();
		lsCopy.AddRange (dRegenButtonClicked);
		foreach (DButtonClicked df in lsCopy)
			if (df.Invoke (sender))
				res = true;
		return res;
	}

	bool onUndoButtonClicked(Button sender){
		Debug.Log ("Undo Button Pressed");

		bool res = false;
		List<DButtonClicked> lsCopy = new List<DButtonClicked> ();
		lsCopy.AddRange (dUndoButtonClicked);
		foreach (DButtonClicked df in lsCopy)
			if (df.Invoke (sender))
				res = true;
		return res;
	}

	public void deleteButtonIsVisible(bool v){
		deleteButton.gameObject.SetActive (v);
	}

	public void navButtonIsVisible(bool v){
		navBar.gameObject.SetActive (v);
	}

	public void displayMessage(string msg){
	}

	public Button deleteButton;

	public Image mainBar;
	public Text roomTitle;
	public Button nextRoomButton, prevRoomButton, deleteRoomButton, newRoomButton;
	public Button showMenuButton;
	public GameObject menuBar;
	public GameObject navBar;
	public Button regenButton;
	public Button undoButton, undoButtonFS;

	// Use this for initialization
	void Start () {
		prefrences = this.gameObject.GetComponent<Preferences> ();

		deleteButton = GameObject.Find ("DeleteButton").GetComponent<Button> ();
		deleteButton.onClick.AddListener (() => onButtonClick (deleteButton));
		deleteButton.gameObject.SetActive (false);

		mainBar = GameObject.Find ("MainBar").GetComponent<Image> ();
		mainBar.color = prefrences.primaryColor;

		navBar = mainBar.transform.FindChild ("NavButtonsGeneral").gameObject;

		roomTitle = mainBar.transform.FindChild ("RoomTitle").GetComponent<Text>();
		roomTitle.color = prefrences.primaryTextColor;
		roomTitle.font = prefrences.primaryTextFont;

		nextRoomButton = GameObject.Find ("NextRoomButton").GetComponent<Button>();
		nextRoomButton.onClick.AddListener (() => onNextRoomButtonClicked (nextRoomButton));

		prevRoomButton = GameObject.Find ("PrevRoomButton").GetComponent<Button>();
		prevRoomButton.onClick.AddListener (() => onPrevRoomButtonClicked (prevRoomButton));

		deleteRoomButton = GameObject.Find ("DeleteRoomButton").GetComponent<Button>();
		deleteRoomButton.onClick.AddListener (() => onDeleteRoomButtonClicked (deleteRoomButton));

		newRoomButton = GameObject.Find ("NewRoomButton").GetComponent<Button>();
		newRoomButton.onClick.AddListener (() => onNewRoomButtonClicked (newRoomButton));

		showMenuButton = GameObject.Find ("ShowMenuButton").GetComponent<Button>();
		showMenuButton.onClick.AddListener (() => onShowMenuButtonClicked (showMenuButton));

		menuBar = GameObject.Find ("Window");

		regenButton = GameObject.Find ("RegenButton").GetComponent<Button> ();
		//regenButton.onClick.AddListener (() => onRegenButtonClicked (regenButton));
		regenButton.gameObject.GetComponent<Image> ().color = prefrences.secondaryColor;

		undoButton = GameObject.Find ("UndoButton").GetComponent<Button> ();
		undoButton.gameObject.SetActive (true);

		undoButtonFS = GameObject.Find ("UndoButtonFS").GetComponent<Button> ();
		undoButtonFS.gameObject.SetActive (false);


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
