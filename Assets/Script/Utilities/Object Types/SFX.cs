using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class SFX
{
	public string SFX_name;

	public AudioClip clip;

	[Range(0f, 1f)]
	public float volume = .75f;

	[Range(0f, 100f)]
	public float percentage = 100f;

	[HideInInspector]
	public AudioSource source;
}
