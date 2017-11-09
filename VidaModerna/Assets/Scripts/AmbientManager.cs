using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientManager : MonoBehaviour {

  public static AmbientManager instance;
	// Use this for initialization
	void Start () {
		if(instance == null) {
      DontDestroyOnLoad(this);
      instance = this;
    }
    else {
      Destroy(this.gameObject);
    }
	}
	
}
