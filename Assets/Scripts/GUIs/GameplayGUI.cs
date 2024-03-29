﻿using UnityEngine;
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

		public float x = 10;
		public float y = 100;
		public float w = 100;
		public float h = 42;
		public float xOffset = 0;
		public float yOffset = 18;

		GUIStyle blankStyle = new GUIStyle();

		// Use this for initialization
		void Start () {
			scaleX = Screen.width / nativeWidth;
			scaleY = Screen.height / nativeHeight;
			drawRect = new Rect(x * scaleX, Screen.height - y * scaleY, w * scaleX, h * scaleY);
			distanceCounterStyle.fontSize = (int)(20 * scaleX);
			distanceCounterStyle.contentOffset = new Vector2(xOffset * scaleX, yOffset * scaleY);
		}
		
		// Update is called once per frame
		void Update () {
		}

		void OnGUI()
		{
			string distance = formatDistance(character.Distance);
			GUI.Box(drawRect, distance, distanceCounterStyle);
			if (GUI.Button (drawRect, "", blankStyle) && Time.timeScale != 0) // Haxx to check if paused
			{
				GetComponent<PauseMenu>().PauseGame();
			}
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
