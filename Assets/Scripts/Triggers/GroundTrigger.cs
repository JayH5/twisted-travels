using UnityEngine;
using System.Collections;

namespace Triggers {

	public class GroundTrigger : MonoBehaviour {

		public PlatformerCharacter2D character;

		void OnTriggerEnter2D(Collider2D other)
		{
			character.IsGrounded = true;
		}

		void OnTriggerExit2D(Collider2D other)
		{
			character.IsGrounded = false;
		}

		void OnTriggerStay2D(Collider2D other)
		{
			character.IsGrounded = true;
		}
	}

}
