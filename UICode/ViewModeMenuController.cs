using UnityEngine;
using System.Collections;

public class ViewModeMenuController : ListViewController<string>
{
	// Use this for initialization
	protected override void Start ()
	{
		data.Add ("Orbit");
		data.Add ("FPV");
		
		base.Start ();
	}
	
	protected override bool elementSelected(int i){
		if (base.elementSelected (i))
			return true;
		
		switch (i) {
		case 0:
			
			break;
		}
		
		return false;
	}
	
	protected override int cellHeight(){
		return 50;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}

