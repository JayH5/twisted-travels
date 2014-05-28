using UnityEngine;
using System.Collections;

public class MusicPlayerC : MonoBehaviour {


	public AudioClip[] clips;

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
