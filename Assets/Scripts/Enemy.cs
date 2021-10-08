using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public int maxHp;
    [SerializeField] public int currHp;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Weapon")
        {
            Debug.Log("들어온 거 무기 맞음");

            Weapon hitWeapon = other.gameObject.GetComponent<Weapon>();

            currHp -= currHp > hitWeapon.damage ? hitWeapon.damage : currHp;
            if (currHp < 1) Destroy(gameObject);
        }
       
    }
}
