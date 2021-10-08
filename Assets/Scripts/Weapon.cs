using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Gun, SubGun, Knife, None
}

public class Weapon : MonoBehaviour
{
    [SerializeField] public WeaponType weaponType;
    [SerializeField] public int damage;
    [SerializeField] public float attackDelay;

}
