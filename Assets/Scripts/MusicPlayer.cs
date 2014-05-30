using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour {

	public AudioClip[] clips;

	public bool Muted
	{
		get { return audio.mute; }
		set { audio.mute = value; }
	}

	// Use this for initialization
	void Start () {
		if(!audio.isPlaying)
		{
			audio.clip = clips[Random.Range(0, clips.Length)];
			audio.Play();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(!audio.isPlaying)
		{
			audio.clip = clips[Random.Range(0, clips.Length)];
			audio.Play();
		}
	}
}
