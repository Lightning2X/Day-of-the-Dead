using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;

public class WeaponProperties : MonoBehaviour {
	public bool isMelee = false;
	public int ammo = 3;
	public int damage = 1;
	public float range = 5f;
	public float spread = 90;

}

/*[CustomEditor(typeof(WeaponProperties))]
public class WeaponPropertiesEditor : Editor
{
	public override void OnInspectorGUI()
	{
		WeaponProperties weaponProperties = (WeaponProperties)target;

		weaponProperties.isMelee = EditorGUILayout.Toggle("Is Melee", weaponProperties.isMelee);
		if(!weaponProperties.isMelee)
			weaponProperties.ammo = EditorGUILayout.IntSlider("Ammo", weaponProperties.ammo , 0, 100);
		weaponProperties.damage = EditorGUILayout.IntSlider("Damage", weaponProperties.damage , 1, 100);
		weaponProperties.range = EditorGUILayout.FloatField("Range", weaponProperties.range);
		weaponProperties.spread = EditorGUILayout.FloatField("Spread", weaponProperties.spread);
	}
}*/