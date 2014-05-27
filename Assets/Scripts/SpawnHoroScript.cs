using UnityEngine;
using System.Collections;

public class SpawnHoroScript : MonoBehaviour {
	
	//public ArrayList platforms = new ArrayList();
	public GameObject[] platforms;
	public int spawnRate = 2; //seconds between each platform spawn
	
	private float leftRight = 0;
	private int index;
	
	void Start () {
		Invoke ("Spawn", spawnRate); 
	}

	/*void OnCollisionEnter2D (Collision2D other)
	{
		print ("COLLISION HORO!");
		Destroy (gameObject.transform.parent.gameObject);
		Spawn ();
	}*/
	
	void Spawn()
	{
		if (ProcGenCounter.platCountLeft == 1 && ProcGenCounter.platCountUp == 1 && ProcGenCounter.platCountDown == 1) //must go left otherwise it creates a closed square
		{
			//get the offset depending on the size of the platform
			if ( platforms[index].name == "SmallHorozontal")
				leftRight = -5.183f;
			else if ( platforms[index].name == "MedHorozontal")
				leftRight = -10.6f;
			else if ( platforms[index].name == "LargeHorozontal")
				leftRight = -16.02f;
			
			//instantiate the platform
			Instantiate (platforms[index], new Vector3(transform.position.x  + leftRight, transform.position.y, transform.position.z) , Quaternion.AngleAxis(180,new Vector3(0,0,1)));

			ProcGenCounter.last2[0] = "left";

			if (ProcGenCounter.last2[1] == "up")
			{
				ProcGenCounter.platCountUp = 1;
				ProcGenCounter.platCountDown = 0;
			}
			else if (ProcGenCounter.last2[1] == "down")
			{
				ProcGenCounter.platCountUp = 0;
				ProcGenCounter.platCountDown = 1;
			}
			ProcGenCounter.platCountRight = 0;
			ProcGenCounter.platCountLeft = 1;
		}
		else if (ProcGenCounter.platCountUp == 1 && ProcGenCounter.platCountDown == 1 && ProcGenCounter.platCountRight == 1) //must go right
		{
			///get the offset depending on the size of the platform
			if ( platforms[index].name == "SmallHorozontal")
				leftRight = 5.183f;
			else if ( platforms[index].name == "MedHorozontal")
				leftRight = 10.6f;
			else if ( platforms[index].name == "LargeHorozontal")
				leftRight = 16.02f;
			
			//instantiate the platform
			Instantiate (platforms[index], new Vector3(transform.position.x  + leftRight, transform.position.y, transform.position.z) , Quaternion.AngleAxis(0,new Vector3(0,0,1)));

			ProcGenCounter.last2[0] = "right";

			if (ProcGenCounter.last2[1] == "up")
			{
				ProcGenCounter.platCountUp = 1;
				ProcGenCounter.platCountDown = 0;
			}
			else if (ProcGenCounter.last2[1] == "down")
			{
				ProcGenCounter.platCountUp = 0;
				ProcGenCounter.platCountDown = 1;
			}
			
			ProcGenCounter.platCountRight = 1;
			ProcGenCounter.platCountLeft = 0;
		}
		else //random
		{
			index = Random.Range (0, platforms.GetLength (0)); //get a random platform from the platforms array
			//print (index);
			
			int dice = Random.Range (0, 2); //decide weather the platform if going left or right
			if (dice == 0) //right
			{
				ProcGenCounter.platCountRight = 1;
				//get the offset depending on the size of the platform
				if ( platforms[index].name == "SmallHorozontal")
					leftRight = 5.183f;
				else if ( platforms[index].name == "MedHorozontal")
					leftRight = 10.6f;
				else if ( platforms[index].name == "LargeHorozontal")
					leftRight = 16.02f;
				
				//instantiate the platform
				Instantiate (platforms[index], new Vector3(transform.position.x  + leftRight, transform.position.y, transform.position.z) , Quaternion.AngleAxis(0,new Vector3(0,0,1)));

				ProcGenCounter.last2[0] = "right";
			}
			else //left
			{
				ProcGenCounter.platCountLeft = 1;
				//get the offset depending on the size of the platform
				//get the offset depending on the size of the platform
				if ( platforms[index].name == "SmallHorozontal")
					leftRight = -5.183f;
				else if ( platforms[index].name == "MedHorozontal")
					leftRight = -10.6f;
				else if ( platforms[index].name == "LargeHorozontal")
					leftRight = -16.02f;
				
				//instantiate the platform
				Instantiate (platforms[index], new Vector3(transform.position.x  + leftRight, transform.position.y, transform.position.z) , Quaternion.AngleAxis(180,new Vector3(0,0,1)));

				ProcGenCounter.last2[0] = "left";
			}
		}
	}

}
