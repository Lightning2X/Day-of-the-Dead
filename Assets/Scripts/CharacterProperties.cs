using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterProperties : MonoBehaviour {
	public bool isAlive = true;
	public int _health = 1;
	public int health {
		get {
			return _health;
		}
		set {
			_health = value;
			if (value <= 0)
				isAlive = false;
			else
				isAlive = true;
		}
	}
}