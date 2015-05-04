using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MotherGameController : MonoBehaviour
{
	
	public List<GameController> rooms = new List<GameController>();
	public Button nextButton;
	public Button prevButton;
	public Button deleteButton;
	public Button newButton;
	public Button recolorButton;
	public int current;
	List<GameObject> rt;
	public RoomHelper roomHelper;
	// Use this for initialization
	void Start ()
	{
		roomHelper = new RoomHelper (null, RoomType.LivingRoom);
		rt = new List<GameObject> ();
		rt.Add (create());
		
		current = 0;
		//initializing buttons
		nextButton = GameObject.Find ("NextRoomButton").GetComponent<Button> ();
		nextButton.onClick.AddListener (() => HandleNextRoomClick ());
		
		prevButton = GameObject.Find ("PrevRoomButton").GetComponent<Button> ();
		prevButton.onClick.AddListener (() => HandlePreviousRoomClick ());
		
		newButton = GameObject.Find ("NewRoomButton").GetComponent<Button> ();
		newButton.onClick.AddListener(() => HandleNewRoomClick ());
		
		deleteButton = GameObject.Find ("DeleteRoomButton").GetComponent<Button> ();
		deleteButton.onClick.AddListener (() => HandleDeleteRoomClick ());
		
		recolorButton = GameObject.Find ("RegenButton").GetComponent<Button> ();
		recolorButton.onClick.AddListener (() => HandleRecolorClick ());

		FirstController.getGlobalFirstController ().undoButton.onClick.AddListener (() => HandleUndoClick ());

		/*GameObject secGameController = (GameObject)GameObject.Instantiate (firstGameControllerPrefab, new Vector3(0,0,7), Quaternion.identity);
		secGameController.transform.SetParent (this.transform);
		rooms.Add (secGameController.GetComponent<GameController>());*/
	}
	
	// Update is called once per frame
	
	public void HandleUndoClick(){
		//TODO change 0 to current
		rt[current].GetComponent<GameController>().Undo();
	}
	public void HandleRecolorClick(){
		//TODO change 0 to current
		rt[current].GetComponent<GameController>().cSt.Execute(new RefurnishRoomCommand(rt[current].GetComponent<GameController>().mainRoom.GetComponentInChildren<Room>()));
		roomHelper.Recolor(rt[current].GetComponent<GameController>().mainRoom.GetComponentInChildren<Room>());
	}

	public void HandleNextRoomClick(){
		Debug.Log ("Clicking next");
		if (current < rt.Count - 1) {
			applyAnimationEnterRight(rt[current+1]);
			applyAnimationExitDown(rt[current]);
			current++;
		}
	}
	
	public void HandlePreviousRoomClick(){
		if (current > 0) {
			applyAnimationEnterDown(rt[current-1]);
			applyAnimationExitRight(rt[current]);
			current--;
		}
	}
	
	public void HandleNewRoomClick(){
		rt.Add (create ());
		int k = rt.Count - 1;
		GameObject temp=rt[k];
		current++;
		while (k>current) {
			rt[k]=rt[k-1];
			k--;
		}
		rt [k] = temp;
		temp.AddComponent<CreateRoomAnimation> ();
		if(k>0)
			applyAnimationExitDown (rt [k - 1]);
	}
	public void HandleDeleteRoomClick(){
		if (rt.Count > 0) {
			GameObject temp = rt [current];
			rt.RemoveAt (current);
			//if it is the lase one bring the previous one
			if (current == rt.Count) {
				//if there is more then 1 item left in the list
				if (current > 0) {
					applyAnimationEnterDown(rt[current-1]);
				}
				current--;
			} else {
				applyAnimationEnterRight(rt[current]);
			}
			temp.AddComponent<DeleteRoomAnimation> ();
		}
		
	}
	
	public void applyAnimationExitRight (GameObject obj){
		SwitchRoomAnimation sac= obj.AddComponent<SwitchRoomAnimation>();
		sac.start=new Vector3(0,0,0);
		sac.end=getCameraX()*10;
		sac.keepOn=false;
	}
	public void applyAnimationEnterRight (GameObject obj){
		obj.SetActive (true);
		SwitchRoomAnimation san = obj.AddComponent<SwitchRoomAnimation> ();
		san.start = getCameraX () * 10;
		san.end = new Vector3 (0, 0, 0);
		
	}
	public void applyAnimationExitDown(GameObject obj){
		obj.SetActive (true);
		SwitchRoomAnimation sac = obj.AddComponent<SwitchRoomAnimation> ();
		sac.keepOn = false;
		sac.start = new Vector3 (0, 0, 0);
		sac.end = getCameraX()*(-10);
	}
	public void applyAnimationEnterDown(GameObject obj){
		obj.SetActive(true);
		SwitchRoomAnimation san = obj.AddComponent<SwitchRoomAnimation> ();
		san.start = getCameraX () * (-10);
		san.end=new Vector3(0,0,0);
	}
	
	private GameObject create(){
		/*GameObject roomPrefab = Resources.Load<GameObject> ("Room");
		GameObject x= GameObject.Instantiate (roomPrefab);
		x.transform.parent = this.transform;
		return x;*/

		GameObject firstGameControllerPrefab = null;
		firstGameControllerPrefab = Resources.Load<GameObject> ("RoomGameController");

		GameObject firstGameController = (GameObject)GameObject.Instantiate (firstGameControllerPrefab);
		firstGameController.transform.SetParent (this.transform);
		firstGameController.GetComponent<GameController> ().rh = roomHelper;
		return firstGameController;
		
	}
	
	public Vector3 getCameraX(){
		GameObject cam=GameObject.Find ("Main Camera");
		float ay=cam.transform.eulerAngles.y*3.141592F/180.0F;
		return new Vector3(Mathf.Cos(ay),0,-Mathf.Sin(ay));
	}
	void Update ()
	{
		
	}
}


