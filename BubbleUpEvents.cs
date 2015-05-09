using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class BubbleUpEvents : MonoBehaviour, IBeginDragHandler, ICancelHandler, IDeselectHandler, IDragHandler, IDropHandler, IEndDragHandler, IInitializePotentialDragHandler, IMoveHandler, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IScrollHandler, ISelectHandler, ISubmitHandler, IUpdateSelectedHandler {

	private void DoForParents<T>(Action<T> action) where T:IEventSystemHandler
	{
		Transform parent = transform.parent;
		while(parent != null) {
			foreach(var component in parent.GetComponents<Component>()) {
				if(component is T)
					action((T)(IEventSystemHandler)component);
			}
			parent = parent.parent;
		}
	}

	public void OnBeginDrag(PointerEventData eventData) { DoForParents<IBeginDragHandler> ((parent) => {parent.OnBeginDrag(eventData); });} 
	public void OnCancel(BaseEventData eventData) { DoForParents<ICancelHandler> ((parent) => {parent.OnCancel(eventData); });} 
	public void OnDeselect(BaseEventData eventData) { DoForParents<IDeselectHandler> ((parent) => {parent.OnDeselect(eventData); });} 
	public void OnDrag(PointerEventData eventData) { DoForParents<IDragHandler> ((parent) => {parent.OnDrag(eventData); });} 
	public void OnDrop(PointerEventData eventData) { DoForParents<IDropHandler> ((parent) => {parent.OnDrop(eventData); });} 
	public void OnEndDrag(PointerEventData eventData) { DoForParents<IEndDragHandler> ((parent) => {parent.OnEndDrag(eventData); });} 
	public void OnInitializePotentialDrag(PointerEventData eventData) { DoForParents<IInitializePotentialDragHandler> ((parent) => {parent.OnInitializePotentialDrag(eventData); });} 
	public void OnMove(AxisEventData eventData) { DoForParents<IMoveHandler> ((parent) => {parent.OnMove(eventData); });} 
	public void OnPointerClick(PointerEventData eventData) { DoForParents<IPointerClickHandler> ((parent) => {parent.OnPointerClick(eventData); });} 
	public void OnPointerDown(PointerEventData eventData) { DoForParents<IPointerDownHandler> ((parent) => {parent.OnPointerDown(eventData); });} 
	public void OnPointerEnter(PointerEventData eventData) { DoForParents<IPointerEnterHandler> ((parent) => {parent.OnPointerEnter(eventData); });} 
	public void OnPointerExit(PointerEventData eventData) { DoForParents<IPointerExitHandler> ((parent) => {parent.OnPointerExit(eventData); });} 
	public void OnPointerUp(PointerEventData eventData) { DoForParents<IPointerUpHandler> ((parent) => {parent.OnPointerUp(eventData); });} 
	public void OnScroll(PointerEventData eventData) { DoForParents<IScrollHandler> ((parent) => {parent.OnScroll(eventData); });} 
	public void OnSelect(BaseEventData eventData) { DoForParents<ISelectHandler> ((parent) => {parent.OnSelect(eventData); });} 
	public void OnSubmit(BaseEventData eventData) { DoForParents<ISubmitHandler> ((parent) => {parent.OnSubmit(eventData); });} 
	public void OnUpdateSelected(BaseEventData eventData) { DoForParents<IUpdateSelectedHandler> ((parent) => {parent.OnUpdateSelected(eventData); });} 

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
