using UnityEngine;
using System.Collections;

public class playSound : MonoBehaviour {

	private AudioSource[] _audiSource;

	void Start () {
		_audiSource = this.GetComponents<AudioSource>();
	}

	public void PlaySound(string type){
		switch (type) {
			case "jump":
				_audiSource[0].Play();
				break;
			case "power":
				_audiSource[1].Play();
				break;
			case "restart":
				_audiSource[2].Play();
				break;
		}
	}
}
