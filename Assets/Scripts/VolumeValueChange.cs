using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeValueChange : MonoBehaviour {

	private AudioSource audioSrc;
	private float volumenBarra = 1f;

	void Start(){
		audioSrc = GetComponent<AudioSource> ();
		audioSrc.volume = volumenBarra;
	}

	public void SetVolume(float vol){
		audioSrc.volume = vol;
	}

}
