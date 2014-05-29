using UnityEngine;
using System.Collections;

namespace Triggers {

	public class CollisionTrigger : MonoBehaviour {

		public PlatformerCharacter2D character;
		
		void OnTriggerEnter2D(Collider2D other)
		{
			character.OnCollisionDetected(other);
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
