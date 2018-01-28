using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour {

    public GameObject _GameManager;

	// Use this for initialization
	private void Awake () {
        if (GameManager.instance == null)
            Instantiate(_GameManager);
	}

}
