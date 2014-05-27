using UnityEngine;
using System.Collections;

namespace Triggers {
	
	public class UpcomingCliffTrigger : MonoBehaviour {
		
		public PlatformerCharacter2D character;
		
		private int entries = 0;

		void OnTriggerEnter2D(Collider2D other)
		{
			if (other.name == "Floor")
				entries++;
		}
		
		void OnTriggerExit2D(Collider2D other)
		{
			if (other.name == "Floor")
			{
				entries--;
				if (entries == 0)
					character.OnUpcomingCliffDetected();
			}
		}
		
		void OnTriggerStay2D(Collider2D other)
		{
			// Nothing to do
		}
	}
}
