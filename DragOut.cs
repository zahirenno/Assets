using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class DragOut : MonoBehaviour/*, IDragHandler, IPointerExitHandler, IPointerUpHandler */{

	private enum STATE {standby, dragging, exited, moving};
	private STATE state = STATE.standby;

	public void OnDrag(PointerEventData eventData){
		switch (state) {
		case STATE.standby:
			state = STATE.dragging;
			break;
		case STATE.exited:
			state = STATE.moving;
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
		Debug.Log(baseEvent.selectedObject.name + " triggered an event!");
		//baseEvent.selectedObject is the GameObject that triggered the event,
		// so we can access its components, destroy it, or do whatever.
	}

	// Use this for initialization
	void Start () {
		gameObject.AddComponent<EventTrigger> ();

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
