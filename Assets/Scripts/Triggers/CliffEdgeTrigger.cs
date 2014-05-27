using UnityEngine;
using System.Collections;

namespace Triggers {
	
	public class CliffEdgeTrigger : MonoBehaviour {
		
		public PlatformerCharacter2D character;
		
		void OnTriggerEnter2D(Collider2D other)
		{
			// Nothing to do
		}
		
		void OnTriggerExit2D(Collider2D other)
		{
			character.OnCliffEdgeDetected();
		}
		
		void OnTriggerStay2D(Collider2D other)
		{
			// Nothing to do
		}
	}
}
