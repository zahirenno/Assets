using TouchScript.Gestures;

public interface ITouchListener {
	
	void GestureChanged (PanGesture gesture, GestureStateChangeEventArgs e);
	void GestureChanged (LongPressGesture gesture, GestureStateChangeEventArgs e);
	void GestureChanged (PressGesture gesture, GestureStateChangeEventArgs e);
	void GestureChanged (TapGesture gesture, GestureStateChangeEventArgs e);
	void GestureChanged (ScaleGesture gesture, GestureStateChangeEventArgs e);
	void GestureChanged (ReleaseGesture gesture, GestureStateChangeEventArgs e);
	
}
