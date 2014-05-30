using UnityEngine;
using System.Collections;

/// <summary>
/// Plays sound effects.
/// </summary>
public class EffectsPlayer : MonoBehaviour {

	public AudioClip jumpSound;
	public AudioClip dashSound;
	public AudioClip rotateSound;

	public bool muted = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void dash()
	{
		if (!muted)
			AudioSource.PlayClipAtPoint(dashSound, Vector3.zero);
	}

	public void jump()
	{
		if (!muted)
			AudioSource.PlayClipAtPoint(jumpSound, Vector3.zero);
	}

	public void rotate()
	{
		if (!muted)
			AudioSource.PlayClipAtPoint(rotateSound, Vector3.zero);
	}
}
