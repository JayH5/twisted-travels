using UnityEngine;
using System.Collections;

namespace Triggers {
	
	public class UpcomingCliffTrigger : MonoBehaviour {
		
		public PlatformerCharacter2D character;
		
		void OnTriggerEnter2D(Collider2D other)
		{
			// Nothing to do
		}
		
		void OnTriggerExit2D(Collider2D other)
		{
			character.OnUpcomingCliffDetected();
		}
		
		void OnTriggerStay2D(Collider2D other)
		{
			// Nothing to do
		}
	}
}
