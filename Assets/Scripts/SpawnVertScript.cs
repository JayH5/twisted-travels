using UnityEngine;
using System.Collections;

public class SpawnVertScript : MonoBehaviour {

	//public ArrayList platforms = new ArrayList();
	public GameObject[] platforms;
	public int spawnRate = 2; //seconds between each platform spawn

	private float UpDown = 0;
	private int index;

	void Start () {
		Invoke ("Spawn", spawnRate); 
	}

	/*void OnCollisionEnter2D (Collision2D other)
	{
		print ("COLLISION VERT!");
		Destroy (gameObject.transform.parent.gameObject);
		Spawn ();
	}*/
	
	void Spawn()
	{
		index = Random.Range (0, platforms.GetLength (0)); //get a random platform from the platforms array
		//print (index);

		int dice = Random.Range (0, 2); //decide weather the platform if going up or down
		if (dice == 0) //up
		{
			//get the offset depending on the size of the platform
			if ( platforms[index].name == "SmallVertical")
				UpDown = 5.183f;
			else if ( platforms[index].name == "MedVertical")
				UpDown = 10.6f;
			else if ( platforms[index].name == "LargeVertical")
				UpDown = 16.02f;

			//instantiate the platform
			Instantiate (platforms[index], new Vector3(transform.position.x, transform.position.y + UpDown, transform.position.z) , Quaternion.AngleAxis(90,new Vector3(0,0,1)));

		}
		else //down
		{
			//get the offset depending on the size of the platform
			if ( platforms[index].name == "SmallVertical")
				UpDown = -5.183f;
			else if ( platforms[index].name == "MedVertical")
				UpDown = -10.6f;
			else if ( platforms[index].name == "LargeVertical")
				UpDown = -16.02f;

			//instantiate the platform
			Instantiate (platforms[index], new Vector3(transform.position.x, transform.position.y + UpDown, transform.position.z) , Quaternion.AngleAxis(270,new Vector3(0,0,1)));

		}


	}
	
}
