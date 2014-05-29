using UnityEngine;
using System.Collections;

public class BoxSmash : MonoBehaviour {

	SpriteRenderer renderer;
	float textureFadeDuration = 1.0f;
	bool isSmashed = false;

	public float smashForce = 50000.0f;

	// Use this for initialization
	void Start () {
		renderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void smash(Vector2 from)
	{
		if (!isSmashed)
		{
			Debug.Log ("SMASH!");
			isSmashed = true;
			rigidbody2D.AddForceAtPosition(from.normalized * smashForce, Random.insideUnitSphere); // Send it flying
			collider2D.isTrigger = true; // Hack to make other objects go through box
			StartCoroutine(textureFadeAnimation());
		}
	}

	/// <summary>
	/// Fades the texture and destroys the object.
	/// </summary>
	/// <returns>The fade animation.</returns>
	IEnumerator textureFadeAnimation()
	{
		Color originalColor = renderer.color;
		Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0.0f); // Transparent
		for (float i = 0.0f; i < 1.0f; i += Time.deltaTime / textureFadeDuration)
		{
			renderer.color = Color.Lerp(originalColor, targetColor, i);
			yield return new WaitForSeconds(0);
		}
		Destroy(this);
	}
}
