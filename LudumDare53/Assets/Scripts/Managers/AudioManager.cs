using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Managers {
	public class AudioManager : MonoBehaviour {

		public bool isMainMenu = false;
		public bool isPunchline = false;
		
		public List<Sound> music = new List<Sound>();
		public List<Sound> damageFx = new List<Sound>();
		public List<Sound> thumpFx = new List<Sound>();
		public List<Sound> punchFx = new List<Sound>();
		public List<Sound> burpFx = new List<Sound>();
		public List<Sound> soundFx = new List<Sound>();

		public static AudioManager instance;

		void Awake() {
			if (instance == null)
				instance = this;
			else {
				Destroy(gameObject);
				return;
			}
			
			DontDestroyOnLoad(gameObject);
			
			foreach (Sound s in music) {
				s.source = gameObject.AddComponent<AudioSource>();
				s.source.clip = s.clip;

				s.source.volume = s.volume;
				s.source.pitch = s.pitch;
				s.source.loop = s.loop;
			}
			
			foreach (Sound s in damageFx) {
				s.source = gameObject.AddComponent<AudioSource>();
				s.source.clip = s.clip;

				s.source.volume = s.volume;
				s.source.pitch = s.pitch;
				s.source.loop = s.loop;
			}
			
			foreach (Sound s in thumpFx) {
				s.source = gameObject.AddComponent<AudioSource>();
				s.source.clip = s.clip;

				s.source.volume = s.volume;
				s.source.pitch = s.pitch;
				s.source.loop = s.loop;
			}
			
			foreach (Sound s in punchFx) {
				s.source = gameObject.AddComponent<AudioSource>();
				s.source.clip = s.clip;

				s.source.volume = s.volume;
				s.source.pitch = s.pitch;
				s.source.loop = s.loop;
			}
			
			foreach (Sound s in burpFx) {
				s.source = gameObject.AddComponent<AudioSource>();
				s.source.clip = s.clip;

				s.source.volume = s.volume;
				s.source.pitch = s.pitch;
				s.source.loop = s.loop;
			}
			
			foreach (Sound s in soundFx) {
				s.source = gameObject.AddComponent<AudioSource>();
				s.source.clip = s.clip;

				s.source.volume = s.volume;
				s.source.pitch = s.pitch;
				s.source.loop = s.loop;
			}
		}

		private void Start() {
			if (isMainMenu)
				PlayMusic("MenuMusic");
			else if (!isPunchline)
				PlayMusic("MainMusic");

		}

		public void TransitionMusic(string name) {
			foreach (Sound s in music) {
				if (s.source.isPlaying)
					FadeOutMusic(s.name, 1);
			}
			FadeInMusic(name,2);
		}

		public void PlayMusic(string name) {
			Sound s = music.Find(sound => sound.soundName == name);
			if (s == null) {
				Debug.LogWarning("Sound " + name + " not found!");
				return;
			}
				
			s.source.Play();
		}
		
		public void PlayFX(string name) {
			Sound s = soundFx.Find(sound => sound.soundName == name);
			if (s == null) {
				Debug.LogWarning("Sound " + name + " not found!");
				return;
			}
				
			s.source.Play();
		}
		
		public void PlayFXPitch(string name, float pitch) {
			Sound s = soundFx.Find(sound => sound.soundName == name);
			if (s == null) {
				Debug.LogWarning("Sound " + name + " not found!");
				return;
			}

			float original = s.pitch;
			s.pitch = pitch;
			s.source.Play();

			s.pitch = original;
		}
		
		public void PlayDamage() {
			Sound s = damageFx[Random.Range(0, damageFx.Count)];
			if (s == null) {
				Debug.LogWarning("No sounds in DamageFX list");
				return;
			}
				
			s.source.Play();
		}
		
		public void PlayPunch() {
			Sound s = punchFx[Random.Range(0, punchFx.Count)];
			if (s == null) {
				Debug.LogWarning("No sounds in PunchFx list");
				return;
			}
				
			s.source.Play();
		}
		
		public void PlayThump() {
			Sound s = thumpFx[Random.Range(0, thumpFx.Count)];
			if (s == null) {
				Debug.LogWarning("No sounds in ThumpFX list");
				return;
			}
				
			s.source.Play();
		}
		
		public void PlayBurp() {
			Sound s = burpFx[Random.Range(0, burpFx.Count)];
			if (s == null) {
				Debug.LogWarning("No sounds in BurpFx list");
				return;
			}
				
			s.source.Play();
		}
		
		public void FadeOutMusic(string name, float duration) {
			Sound s = music.Find(sound => sound.soundName == name);
			if (s == null) {
				Debug.LogWarning("Sound " + name + " not found!");
				return;
			}
			
			s.source.GetComponent<MonoBehaviour>().StartCoroutine(FadeOutCore(s.source, duration));
		}
 
		private static IEnumerator FadeOutCore(AudioSource s, float duration) {
			float startVolume = s.volume;
 
			while (s.volume > 0) {
				s.volume -= startVolume * Time.deltaTime / duration;
				yield return new WaitForEndOfFrame();
			}
 
			s.Stop();
			s.volume = startVolume;
		}
		
		public void FadeInMusic(string name, float duration) {
			Sound s = music.Find(sound => sound.soundName == name);
			if (s == null) {
				Debug.LogWarning("Sound " + name + " not found!");
				return;
			}
			
			s.source.GetComponent<MonoBehaviour>().StartCoroutine(FadeInCore(s.source, duration));
		}
 
		private static IEnumerator FadeInCore(AudioSource s, float duration) {
			float topVolume = s.volume;
			s.volume = 0;
			s.Play();
 
			while (s.volume < topVolume) {
				s.volume += topVolume * Time.deltaTime / duration;
				yield return new WaitForEndOfFrame();
			}
 
			s.volume = topVolume;
		}
		
		public void StopMusic(string name) {
			Sound s = music.Find(sound => sound.soundName == name);
			if (s == null) {
				Debug.LogWarning("Sound " + name + " not found!");
				return;
			}

			s.source.Stop();
		}

		public void StopAll() {
			foreach (Sound s in music) {
				if (s.source.isPlaying)
					FadeOutMusic(s.name, 1);
			}
		}
		
		private IEnumerator PlayDelayed(string name, float delay) {
			Sound s = music.Find(sound => sound.soundName == name);
			if (s == null) {
				Debug.LogWarning("Sound " + name + " not found!");
				yield break;
			}

			yield return new WaitForSeconds(delay);
			s.source.Play();
		}

	}
}