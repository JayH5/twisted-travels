using UnityEngine;
using System.Collections;

public class BoxFromGroundTrigger : MonoBehaviour {

	//public BoxfromGroundAnimscript anim;

	void OnTriggerEnter2D(Collider2D other) 
	{
		if (other.tag == "Player")
		{
			//print("YES!");
			foreach (Transform child in transform)
			{
				if (child.name == "BoxSpawnAnim")
				{
					//print("hello");
					//anim.StartAnimation();
				}
			}
		}
		
		
	}
}
