//For disabling unnecessary SerializeField private field warnings
//https://forum.unity.com/threads/serializefield-warnings.560878/
#pragma warning disable CS0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDisable : MonoBehaviour {
    [SerializeField]
	private float _disabletime;
	// Use this for initialization
	private void OnEnable () {
		Invoke("DisableSelf", _disabletime);
	}
	
	// Update is called once per frame
	void DisableSelf () {
		gameObject.SetActive(false);
	}
}
