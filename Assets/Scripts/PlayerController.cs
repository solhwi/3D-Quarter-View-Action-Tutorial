using System.Collections;
using System.Collections.Generic;
using UnityEngine;
enum DownKey
{
    Move, Attack, Run, Pick, Jump
}

public class PlayerController : Player
{
    [SerializeField] Transform cameraArm;
    [SerializeField] Transform playerTr; 

    [SerializeField] bool[] downKeys = new bool[5];

    Vector2 moveInput;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponentInParent<Rigidbody>();
    }

    private void Update()
    {
        currFireDelay += Time.deltaTime;
        
        GetInput();
        LookAt();
        Move();
        Jump();
        Pick();
    }

    bool IsKeyDown(DownKey dk) => downKeys[(int)dk];

    void GetInput()
    {
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        downKeys[(int)DownKey.Run] = Input.GetButton("Run");
        downKeys[(int)DownKey.Jump] = Input.GetButtonDown("Jump");
        downKeys[(int)DownKey.Pick] = Input.GetButtonDown("Pick");

        if (Input.GetButtonDown("Swap1"))
        {
            Swap(WeaponType.Gun);
        }
        else if (Input.GetButtonDown("Swap2"))
        {
            Swap(WeaponType.SubGun);
        }
        else if (Input.GetButtonDown("Swap3"))
        {
            Swap(WeaponType.Knife);
        }
    }

    void LookAt()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 cameraAngle = cameraArm.rotation.eulerAngles;

        float x = cameraAngle.x - mouseDelta.y;

        x = x < 180f ? Mathf.Clamp(x, -1f, 70f) : Mathf.Clamp(x, 335f, 361f); ;

        cameraArm.rotation = Quaternion.Euler(x, cameraAngle.y + mouseDelta.x, cameraAngle.z);
    }

    void Move()
    {
        Vector3 moveVec;
        bool isMove = moveInput.magnitude != 0;
        
        if (isMove)
        {
            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;

            moveVec = lookForward * moveInput.y + lookRight * moveInput.x;
            playerTr.forward = lookForward;
        }
        else
        {
            moveVec = new Vector3(0f, 0f, 0f);
        } 

        transform.position += downKeys[(int)DownKey.Run] ? moveVec * moveSpeed * 2 * Time.deltaTime : moveVec * moveSpeed * Time.deltaTime;

        anim.SetBool("isWalk", isMove);
        anim.SetBool("isRun", downKeys[(int)DownKey.Run]);

    }

    void Pick()
    {
        if (IsKeyDown(DownKey.Pick) && nearObject != null)
        {
            if (nearObject.tag == "Weapon")
            {
                Weapon w = nearObject?.GetComponent<Weapon>();
                if (w != null)
                {
                    Debug.Log("아이템을 획득하였습니다!");
                    hasWeapons[(int)w.weaponType] = true;
                }
                Destroy(nearObject);
            }
        }
    }

    void Jump()
    {
        if (IsKeyDown(DownKey.Jump) && !isJump && !isSwap)
        {
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            isJump = true;
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
        }
    }

    public void Attack()
    {
        if (currWeapon == null) return;
        Debug.Log("공격 쿨타임입니다.");

        if (hasWeapons[(int)currWeaponType] && currWeapon.attackDelay < currFireDelay && !isSwap)
        {
            currWeapon.Attack();
            anim.SetTrigger("doSwing");
            currFireDelay = 0;
        }
    }

    void Swap(WeaponType weaponType)
    {
        isSwap = true;

        currWeaponType = weaponType;
        int weaponIdx = (int)currWeaponType;

        currWeapon = weapons[weaponIdx].GetComponent<PlayerWeapon>();

        for (int i = 0; i < 3; i++)
        {
            weapons[i].SetActive(false);
        }

        if (hasWeapons[weaponIdx])
        {
            GameObject currWeapon = weapons[weaponIdx];
            currWeapon.SetActive(true);
        }

        anim.SetTrigger("doSwap");
        Invoke("SwapEnd", 0.5f);
    }

    void SwapEnd() => isSwap = false;
}
