using UnityEngine;
using System.Collections;

public class BackgroundScript : MonoBehaviour {
	
	int currentRot = 0; //1 = 90, 2 = 180, 3 = 270, 4 = 360/0
	bool vertical = false; //the camera rotation is verticle (90 or 270 degrees)
	bool offset1, offset2, offset3, offset4 = false; //limits the update to only get the offset value once per rotation
	float offsetX, offsetY; //offset from the centre of the image to the player/camera position 
	
	//public SwipeHandler camera;
	public PlatformerCharacter2D player;
	public float moveSpeed = 0.9f;

	/*void Start () {
		offsetX = transform.position.x - player.transform.position.x;
		offsetY = transform.position.y - player.transform.position.y;	
	}*/
	
	void Update () {

		//temp - need to fix this properly later
		Vector3 camera = player.transform.position;
		transform.position = new Vector3(camera.x, camera.y, transform.position.z); 
		
		/*if camera.transform.position.x (camera.currentRotation.z > 89 && camera.currentRotation.z < 91) //1
		{
			vertical = true;
			offset2 = false;
			offset3 = false;
			offset4 = false;

			//get x offset once
			if (offset1 == false)
			{
				offsetX = transform.position.x - player.transform.position.x;
				offsetY = transform.position.y - player.transform.position.y;
				offset1 = true;

				print("OffsetX: " + offsetX + "      offsetY: " + offsetY);
			}
		}
		else if (camera.currentRotation.z > 179 && camera.currentRotation.z < 181) //2
		{
			vertical = false;
			offset1 = false;
			offset3 = false;
			offset4 = false;

			//get y offset once
			if (offset2 == false)
			{
				offsetY = transform.position.y - player.transform.position.y;
				offsetX = transform.position.x - player.transform.position.x;
				offset2 = true;

				print("OffsetX: " + offsetX + "      offsetY: " + offsetY);
			}
		}
		else if (camera.currentRotation.z > 269 && camera.currentRotation.z < 271) //3
		{
			vertical = true;
			offset1 = false;
			offset2 = false;
			offset4 = false;

			//get x offset once
			if (offset3 == false)
			{
				offsetX = transform.position.x - player.transform.position.x;
				offsetY = transform.position.y - player.transform.position.y;
				offset3 = true;

				print("OffsetX: " + offsetX + "      offsetY: " + offsetY);
			}
		}
		else if (camera.currentRotation.z > -1 && camera.currentRotation.z < 1) //4
		{
			vertical = false;
			offset1 = false;
			offset2 = false;
			offset3 = false;

			//get y offset once
			if (offset4 == false)
			{
				offsetY = transform.position.y - player.transform.position.y;
				offsetX = transform.position.x - player.transform.position.x;
				offset4 = true;

				print("OffsetX: " + offsetX + "      offsetY: " + offsetY);
			}
		}*/

		//move the image at a certain amount slower than the player, to give the effect that it is far away
		/*if (vertical == false) 
		{
			transform.position = new Vector3((camera.transform.position.x * moveSpeed) + offsetX, camera.transform.position.y + offsetY, transform.position.z);
		}
		else
		{
			transform.position = new Vector3(camera.transform.position.x + offsetX, (camera.transform.position.y * moveSpeed) /*+ offsetY, transform.position.z);
		}*/
		
	}
}