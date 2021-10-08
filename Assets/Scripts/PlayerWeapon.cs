using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : Weapon
{
    BoxCollider hitBox;
    TrailRenderer trailEffect;

    private void Awake()
    {
        hitBox = GetComponent<BoxCollider>();
        trailEffect = GetComponentInChildren<TrailRenderer>();
    }

    public void Attack()
    {
        switch(weaponType)
        { 
            case WeaponType.Gun:
                break;
            case WeaponType.SubGun:
                break;
            case WeaponType.Knife:
                if(gameObject.activeSelf)
                    StartCoroutine(Swing());
                break;
            default:
                break;
        }
    }

    IEnumerator Swing()
    {
        yield return new WaitForSeconds(0.1f);
        hitBox.enabled = true;
        trailEffect.enabled = true;
        yield return new WaitForSeconds(0.8f);
        hitBox.enabled = false;
        yield return new WaitForSeconds(0.3f);
        trailEffect.enabled = false;
    }

}
