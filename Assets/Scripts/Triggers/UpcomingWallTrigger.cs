using UnityEngine;
using System.Collections;

namespace Triggers {
	
	public class UpcomingWallTrigger : MonoBehaviour {
		
		public PlatformerCharacter2D character;
		
		void OnTriggerEnter2D(Collider2D other)
		{
			if (other.name == "Floor")
				character.OnUpcomingWallDetected();
		}
		
		void OnTriggerExit2D(Collider2D other)
		{
			// Nothing to do
		}
		
		void OnTriggerStay2D(Collider2D other)
		{
			// Nothing to do
		}
	}
}
