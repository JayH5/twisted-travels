using UnityEngine;
using System.Collections;

public class SpawnVertScript : MonoBehaviour {

	//public ArrayList platforms = new ArrayList();
	public GameObject[] platforms;
	public float spawnRate = 2; //seconds between each platform spawn

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

		if (ProcGenCounter.platCountLeft == 1 && ProcGenCounter.platCountUp == 1 && ProcGenCounter.platCountRight == 1) //must go up otherwise it creates a closed square
		{
			//get the offset depending on the size of the platform
			if ( platforms[index].name[0] == 'S')
				UpDown = 5.183f;
			else if ( platforms[index].name[0] == 'M')
				UpDown = 10.6f;
			else if ( platforms[index].name[0] == 'L')
				UpDown = 16.05f;

			ProcGenCounter.last2[1] = "up";
			
			//instantiate the platform
			Instantiate (platforms[index], new Vector3(transform.position.x, transform.position.y + UpDown, transform.position.z) , Quaternion.AngleAxis(90,new Vector3(0,0,1)));



			if (ProcGenCounter.last2[0] == "left")
			{
				ProcGenCounter.platCountLeft = 1;
				ProcGenCounter.platCountRight = 0;
			}
			else if (ProcGenCounter.last2[0] == "right")
			{
				ProcGenCounter.platCountLeft = 0;
				ProcGenCounter.platCountRight = 1;
			}

			ProcGenCounter.platCountDown = 0;
			ProcGenCounter.platCountUp = 1;
		}
		else if (ProcGenCounter.platCountLeft == 1 && ProcGenCounter.platCountDown == 1 && ProcGenCounter.platCountRight == 1) //must go down
		{
			//get the offset depending on the size of the platform
			if ( platforms[index].name[0] == 'S')
				UpDown = -5.183f;
			else if ( platforms[index].name[0] == 'M')
				UpDown = -10.6f;
			else if ( platforms[index].name[0] == 'L')
				UpDown = -16.05f;

			ProcGenCounter.last2[1] = "down";
			
			//instantiate the platform
			Instantiate (platforms[index], new Vector3(transform.position.x, transform.position.y + UpDown, transform.position.z) , Quaternion.AngleAxis(270,new Vector3(0,0,1)));



			if (ProcGenCounter.last2[0] == "left")
			{
				ProcGenCounter.platCountLeft = 1;
				ProcGenCounter.platCountRight = 0;
			}
			else if (ProcGenCounter.last2[0] == "right")
			{
				ProcGenCounter.platCountLeft = 0;
				ProcGenCounter.platCountRight = 1;
			}

			ProcGenCounter.platCountDown = 1;
			ProcGenCounter.platCountUp = 0;
		}
		else //random
		{
			//print (index);
			
			int dice = Random.Range (0, 2); //decide weather the platform if going up or down
			if (dice == 0) //up
			{
				ProcGenCounter.platCountUp = 1;
				//get the offset depending on the size of the platform
				if ( platforms[index].name[0] == 'S')
					UpDown = 5.183f;
				else if ( platforms[index].name[0] == 'M')
					UpDown = 10.6f;
				else if ( platforms[index].name[0] == 'L')
					UpDown = 16.05f;

				ProcGenCounter.last2[1] = "up";
								
				//instantiate the platform
				Instantiate (platforms[index], new Vector3(transform.position.x, transform.position.y + UpDown, transform.position.z) , Quaternion.AngleAxis(90,new Vector3(0,0,1)));	


			}
			else //down
			{
				ProcGenCounter.platCountDown = 1;
				//get the offset depending on the size of the platform
				if ( platforms[index].name[0] == 'S')
					UpDown = -5.183f;
				else if ( platforms[index].name[0] == 'M')
					UpDown = -10.6f;
				else if ( platforms[index].name[0] == 'L')
					UpDown = -16.05f;

				ProcGenCounter.last2[1] = "down";
				
				//instantiate the platform
				Instantiate (platforms[index], new Vector3(transform.position.x, transform.position.y + UpDown, transform.position.z) , Quaternion.AngleAxis(270,new Vector3(0,0,1)));


			}
		}
	}
	
}
