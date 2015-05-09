using UnityEngine;
using System.Collections;

public class AddObjectState : MoveObjectState {

	public override void Enter(){
		focusedObject.gameObject.GetComponent<Highlightable> ().highlighted = true;

		lifeCycle = STATE.STARTING;
		listener.onStarting (this);
	}

}
