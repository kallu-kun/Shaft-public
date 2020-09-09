using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
	[SerializeField]
	private AudioClip bGMusic;
	[SerializeField]
	private AudioClip bGMusicIntro;

	private AudioSource source;

	private void Start()
	{
		source = GetComponent<AudioSource>();

		source.clip = bGMusicIntro;
		source.Play();
	}

	private void Update()
	{
		if (!source.isPlaying)
		{
			source.clip = bGMusic;
			source.loop = true;
			source.Play();
		}
	}
}