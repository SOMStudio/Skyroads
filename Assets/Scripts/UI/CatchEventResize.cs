using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public delegate void OnEventCatch();

public class CatchEventResize : UIBehaviour {
	[System.NonSerialized]
	public static CatchEventResize instance = null;

	public OnEventCatch WindowResizeEvent;
	public OnEventCatch WindowCloseEvent;

	protected override void Awake () {
		instance = this;
	}

	protected override void OnRectTransformDimensionsChange() {
		base.OnRectTransformDimensionsChange ();

		if (WindowResizeEvent != null)
			WindowResizeEvent ();
	}

	protected override void OnDestroy() {
		base.OnDestroy ();

		if (WindowCloseEvent != null)
			WindowCloseEvent ();
	}

	// in program we must create public method ResizeWindow_Ev, CloseWindow_Ev
	// and in initialize write code
	//
	//	OnEventCatch resizeWindow = ResizeWindow_Ev;
	//	OnEventCatch closeWindow = CloseWindow_Ev;
	//
	//	CatchEventResize chatchEv = CatchEventResize.Instance;
	//	chatchEv.WindowResizeEvent = resizeWindow;
	//	chatchEv.WindowCloseEvent = closeWindow;
}
