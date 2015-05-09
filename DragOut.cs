using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class DragOut : MonoBehaviour, IDragHandler, IPointerExitHandler, IPointerUpHandler {

	private enum STATE {standby, dragging, exited, moving};
	private STATE state = STATE.standby;

	public delegate void OnDraggedOut(Vector2 pos);

	public OnDraggedOut onDraggedOut = null;

	public void OnDrag(PointerEventData eventData){

		switch (state) {
		case STATE.standby:
			state = STATE.dragging;
			break;
		case STATE.exited:
			state = STATE.moving;
			if (onDraggedOut != null)
				onDraggedOut.Invoke(eventData.position);
			break;
		case STATE.moving:
			break;
		}
	}

	public void OnPointerExit(PointerEventData eventData){

		switch (state) {
		case STATE.dragging:
			state = STATE.exited;
			break;
		}


	}

	public void OnPointerUp(PointerEventData eventData){
		state = STATE.standby;
	}


	public delegate void EventDelegate(UnityEngine.EventSystems.BaseEventData baseEvent);

	public void DropEventMethod(UnityEngine.EventSystems.BaseEventData baseEvent) {
		//Debug.Log(baseEvent.selectedObject.name + " triggered an event!");
		//baseEvent.selectedObject is the GameObject that triggered the event,
		// so we can access its components, destroy it, or do whatever.
	}

	// Use this for initialization
	void Start () {

		EventTrigger trigger = GetComponent<EventTrigger> ();

		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.Drag;
		entry.callback = new EventTrigger.TriggerEvent ();

		UnityEngine.Events.UnityAction<BaseEventData> callback = new UnityEngine.Events.UnityAction<BaseEventData>(DropEventMethod);
		entry.callback.AddListener(callback);
		trigger.delegates.Add(entry);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
