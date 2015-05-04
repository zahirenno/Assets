using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class RoomTypePickerBehaviour : MonoBehaviour
{
	private bool pickerOpen = false;
	public Button livingRoomButton;
	public Button kitchenButton;
	public Button diningRoomButton;
	public Button bedroomButton;
	public GameObject roomTypePickerPanel;
	public Button mainButton;

	// Use this for initialization
	void Start ()
	{
		mainButton = gameObject.GetComponent<Button> ();
		mainButton.onClick.AddListener (() => HandlePickRoomTypeClick ()); 

		livingRoomButton.onClick.AddListener (() => HandleLivingRoomClick ());
		kitchenButton.onClick.AddListener (() => HandleKitchenClick ());
		diningRoomButton.onClick.AddListener (() => HandleDiningRoomClick ());
		bedroomButton.onClick.AddListener (() => HandleBedroomClick ());
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void HandlePickRoomTypeClick(){
		pickerOpen = !pickerOpen;
		roomTypePickerPanel.SetActive (pickerOpen);
	}

	void HandleLivingRoomClick(){
		CloseAndUpdateButton (RoomType.LivingRoom);
	}
	void HandleKitchenClick(){
		CloseAndUpdateButton (RoomType.Kitchen);
	}
	void HandleBedroomClick(){
		CloseAndUpdateButton (RoomType.Bedroom);
	}
	void HandleDiningRoomClick(){
		CloseAndUpdateButton (RoomType.DiningRoom);
	}

	void CloseAndUpdateButton(RoomType type){
		pickerOpen = false;
		roomTypePickerPanel.SetActive (pickerOpen);
		string text = "";
		switch (type) {
		case RoomType.Bedroom:
			text = "Bedroom";
			break;
		case RoomType.DiningRoom:
			text = "Dining Room";
			break;
		case RoomType.Kitchen:
			text = "Kitchen";
			break;
		case RoomType.LivingRoom:
			text = "Living Room";
			break;
		default: break;
		}
		mainButton.transform.FindChild ("MainText").GetComponent<Text> ().text = text;
	}
}

