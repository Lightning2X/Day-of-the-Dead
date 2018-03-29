using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemController : MonoBehaviour {
    private int timer = 0;
    ParticleSystem partSystem;
	// Use this for initialization
	void Start () {
        partSystem = this.GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
        if (gameObject.activeSelf == true && timer <= 0)
        {
            timer = 600;
            partSystem.Play();
        }
        timer = timer - 1;
        if (timer <= 0)
            gameObject.SetActive(false);
    }
}
