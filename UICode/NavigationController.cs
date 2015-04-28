using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class NavigationController : MonoBehaviour {

	public FirstController firstController;
	public ViewController root;
	public Button backButton;
	public Text title;

	Stack<ViewController> stack = new Stack<ViewController>();
	
	public float width = 200;
	public float height = 500;
	public float navControls = 100;

	public void popViewController(){

		Destroy(stack.Pop().gameObject);
		if (stack.Count <= 0) {
			gameObject.SetActive (false);
		}
		else {
			stack.Peek ().gameObject.SetActive (true);
			title.text = stack.Peek().gameObject.name;
		}

		if (stack.Peek () == root)
			backButton.gameObject.SetActive (false);

		if (firstController != null) {
			firstController.onSwitchToPage(stack.Peek());
		}
	}
	
	public void pushViewController(ViewController viewController, bool overrideBack = false){
		backButton.gameObject.SetActive (true);

		backButton.onClick.RemoveAllListeners ();
		if (!overrideBack)
			backButton.onClick.AddListener (() => onBack ());
	
		title.text = viewController.gameObject.name;

		if (stack.Count > 0) {
			stack.Peek ().gameObject.SetActive (false);
		}
		stack.Push(viewController);

		if (firstController != null) {
			firstController.onSwitchToPage(stack.Peek());
		}
	}

	
	public void onBack(){
		popViewController();
	}

	public ViewController getCurrentViewController(){
		return stack.Peek ();
	}

	// Use this for initialization
	void Start () {
		root.setFirstController (firstController);
		stack.Push (root);
		backButton.gameObject.SetActive (false);

		if (firstController != null) {
			firstController.onSwitchToPage(stack.Peek());
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
