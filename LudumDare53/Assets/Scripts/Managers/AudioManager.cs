using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Managers {
	public class AudioManager : MonoBehaviour {

		public List<Sound> music = new List<Sound>();
		public List<Sound> hitFx = new List<Sound>();
		public List<Sound> thumpFx = new List<Sound>();
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
		}

		private void Start() {
			FadeInMusic("MenuMusic", 1);
		}

		public void PlayMusic(string name) {
			Sound s = music.Find(sound => sound.name == name);
			if (s == null) {
				Debug.LogWarning("Sound " + name + " not found!");
				return;
			}
				
			s.source.Play();
		}
		
		public void FadeOutMusic(string name, float duration) {
			Sound s = music.Find(sound => sound.name == name);
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
			Sound s = music.Find(sound => sound.name == name);
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
			Sound s = music.Find(sound => sound.name == name);
			if (s == null) {
				Debug.LogWarning("Sound " + name + " not found!");
				return;
			}

			s.source.Stop();
		}
		
		private IEnumerator PlayDelayed(string name, float delay) {
			Sound s = music.Find(sound => sound.name == name);
			if (s == null) {
				Debug.LogWarning("Sound " + name + " not found!");
				yield break;
			}

			yield return new WaitForSeconds(delay);
			s.source.Play();
		}

	}
}