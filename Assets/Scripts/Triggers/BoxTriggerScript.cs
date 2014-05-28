using UnityEngine;
using System.Collections;

namespace Triggers {

	public class BoxTriggerScript : MonoBehaviour {

		void OnTriggerEnter2D(Collider2D other) 
		{
			if (other.tag == "Player")
			{
				print(transform.parent.gameObject.transform.rotation.z);
				foreach (Transform child in transform)
				{
					if (transform.parent.gameObject.transform.rotation.z > 0)
						child.rigidbody2D.gravityScale = -10;
					else 
						child.rigidbody2D.gravityScale = 10;
				}
			}

				
		}
	}

}
