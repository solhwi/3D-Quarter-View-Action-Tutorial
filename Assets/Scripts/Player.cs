using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    /* 캐릭터 능력치 */
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float jumpPower;

    /* 캐릭터 주위 정보  */
    [SerializeField] protected GameObject nearObject;
    [SerializeField] protected GameObject[] weapons = new GameObject[3];
    [SerializeField] protected bool[] hasWeapons = new bool[3];

    /* 캐릭터의 상태 정보 */
    protected bool isJump = false;
    protected bool isSwap = false;

    [SerializeField] protected WeaponType currWeaponType = WeaponType.None;
    [SerializeField] protected PlayerWeapon currWeapon = null;

    /* 캐릭터 하위 컴포넌트 */
    protected Animator anim;
    protected Rigidbody rigid;
    protected float currFireDelay = 0f;

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
            isJump = false;
            anim.SetBool("isJump", false);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Weapon")
        {
            nearObject = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Weapon")
        {
            nearObject = null;
        }
    }
}
