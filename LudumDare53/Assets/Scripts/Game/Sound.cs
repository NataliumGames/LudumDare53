using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

namespace Game {
	
	[CreateAssetMenu(menuName = "Sound")]
	public class Sound : ScriptableObject {

		public string soundName = "name";
		public AudioClip clip;

		[Range(0f, 1f)]
		public float volume = 1.0f;
		[Range(.1f,3f)]
		public float pitch = 1.0f;

		public bool loop = false;

		[HideInInspector]
		public AudioSource source;
	}
}