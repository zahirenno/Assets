using UnityEngine;
using System.Collections;

public class RenderingMenuController : ListViewController<string>
{
	// Use this for initialization
	protected override void Start ()
	{
		data.Add ("Normal");
		data.Add ("Contour");
		data.Add ("Black and White");
		data.Add ("Sketch");

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

	
	// Update is called once per frame
	void Update ()
	{
		
	}
}

