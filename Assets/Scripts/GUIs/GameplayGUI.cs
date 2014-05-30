using UnityEngine;
using System.Collections;

namespace GUIs {

	public class GameplayGUI : MonoBehaviour {

		public PlatformerCharacter2D character;
		Rect drawRect;

		public GUIStyle distanceCounterStyle;

		int nativeWidth = 480;
		int nativeHeight = 270;
		float scaleX;
		float scaleY;

		// Use this for initialization
		void Start () {
			scaleX = Screen.width / nativeWidth;
			scaleY = Screen.height / nativeHeight;
			drawRect = new Rect(10 * scaleX, Screen.height - 20 * scaleY, 50 * scaleX, 10 * scaleY);
			distanceCounterStyle.fontSize = (int)(16 * scaleX);
		}
		
		// Update is called once per frame
		void Update () {
			if (character.Dead) 
			{

			}
		}

		void OnGUI()
		{
			string distance = formatDistance(character.Distance);
			GUI.Box(drawRect, distance, distanceCounterStyle);
		}

		/// <summary>
		/// Formats the distance float to a nice string. Everything should be 10 chars long.
		/// </summary>
		/// <returns>The distance string.</returns>
		/// <param name="distance">Distance.</param>
		static string formatDistance(float distance)
		{
			if (distance < 1.0f)
			{
				return string.Format("{0,9:f1}m", distance);
			}
			else if (distance < 1000.0f)
			{
				return string.Format("{0,9:d}m", (int) distance);
			}
			else
			{
				float kilometers = distance / 1000.0f;
				return string.Format("{0,8:f2}km", kilometers);
			}
		}
	}
}
