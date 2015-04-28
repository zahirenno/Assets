using UnityEngine;
using System.Collections;

public class SuggestionMenuController : ListViewController<string>
{
	// Use this for initialization
	protected override void Start ()
	{
		data.Add ("Layout");
		data.Add ("Colors");
		data.Add ("Generate All");

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

