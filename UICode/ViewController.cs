using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ViewController : MonoBehaviour {

	protected FirstController firstController = null;

	public void setFirstController (FirstController firstController){
		this.firstController = firstController;
	}

	public static T createViewController<T>(NavigationController navigationController, string name) where T : ViewController{
		ViewController current = navigationController.getCurrentViewController ();
		RectTransform currentVCTr = current.gameObject.GetComponent<RectTransform> ();
		if (currentVCTr == null)
			Debug.LogError ("current viewcontroller is not connected to GUI element");

		if (current != null) {
			GameObject newViewController = new GameObject (name);
			RectTransform vcTransform = newViewController.AddComponent<RectTransform> ();
			vcTransform.SetParent (navigationController.transform);
			
			vcTransform.anchorMax = currentVCTr.anchorMax;
			vcTransform.anchorMin = currentVCTr.anchorMin;
			vcTransform.anchoredPosition = currentVCTr.anchoredPosition;
			vcTransform.sizeDelta = currentVCTr.sizeDelta;
			
			Image panel = newViewController.AddComponent<Image> ();
			panel.color = current.gameObject.GetComponent<Image> ().color;

			newViewController.AddComponent<ScrollRect> ();
			newViewController.AddComponent<Mask> ();
			T nVC = newViewController.AddComponent<T>();
			nVC.navigationController = navigationController;

			nVC.setFirstController(navigationController.firstController);

			return nVC;
		}

		return null; //TODO should do something else
	}

	public NavigationController navigationController;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
