using UnityEngine;
using System.Collections;

namespace Gestures
{
	public class GestureHandler : MonoBehaviour {

		//swipe vars
		public float minMovement = 20.0f;

		public bool enableKeyboardDebug = true;

		private Vector2 StartPos;
		private int SwipeID = -1;

		private IGestureReceiver gestureReceiver;

		void Update ()
		{
			if (gestureReceiver == null)
				return;

			if (enableKeyboardDebug && HandleKeyboardInput())
				return; // Give precedence to keyboard if enabled

			HandleTouchInput();
		}

		bool HandleKeyboardInput()
		{
			bool handled = false;
			if (Input.GetKeyDown (KeyCode.Space))
			{
				gestureReceiver.onTap();
				handled = true;
			}
			else if (Input.GetKeyDown (KeyCode.UpArrow))
			{
				gestureReceiver.onSwipe(SwipeDirection.Up);
				handled = true;
			}
			else if (Input.GetKeyDown (KeyCode.DownArrow))
			{
				gestureReceiver.onSwipe(SwipeDirection.Down);
				handled = true;
			}
			else if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				gestureReceiver.onSwipe(SwipeDirection.Left);
				handled = true;
			}
			else if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				gestureReceiver.onSwipe(SwipeDirection.Right);
				handled = true;
			}
			return handled;
		}

		void HandleTouchInput()
		{
			foreach (var touch in Input.touches) // For each finger... (but only keep track of one)
			{
				var position = touch.position;
				var fingerId = touch.fingerId;
				if (touch.phase == TouchPhase.Began && fingerId != SwipeID) // Begin touch
				{
					SwipeID = fingerId;
					StartPos = position;
				}
				else if (touch.fingerId == SwipeID) // Continue touch detection
				{
					var delta = position - StartPos;
					// Swipe detected if finger moved more than `minMovement`
					if (touch.phase == TouchPhase.Moved && delta.magnitude > minMovement)
					{
						SwipeID = -1; // Swipe detected, this gesture is complete
						SwipeDirection direction;
						if (Mathf.Abs (delta.x) > Mathf.Abs (delta.y)) // Moved more horizontally than vertically
						{
							direction = delta.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;
						}
						else
						{
							direction = delta.y > 0 ? SwipeDirection.Up : SwipeDirection.Down;
						}
						gestureReceiver.onSwipe(direction);
					}
					// Tap detected if finger released without swipe being detected
					else if (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)
					{
						SwipeID = -1;
						//currentRotation = gameObject.transform.eulerAngles;
						gestureReceiver.onTap();
					}
				}
			}
		}

		public void setSwipeReceiver(IGestureReceiver receiver)
		{
			gestureReceiver = receiver;
		}
	}

	public interface IGestureReceiver
	{
		/// <summary>
		/// Called when a tap gesture is detected.
		/// </summary>
		void onTap();
		
		/// <summary>
		/// Called when a swipe gesture is detected.
		/// </summary>
		/// <param name="direction">The direction of the swipe.</param>
		void onSwipe(SwipeDirection direction);
	}
	
	public enum SwipeDirection
	{
		Up, Down, Left, Right
	}
}
