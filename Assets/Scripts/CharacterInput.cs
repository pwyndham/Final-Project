using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class CharacterInput : MonoBehaviour
{
    
    public Dash dashAbility;
    // public Character character;
    // public Transform cameraTransform;
    public List<CharacterAbilities> characterAbilities;

    public GameObject MainMenuUI;
    public GameObject characterStatsUI;

    float xInput;
    float yInput;
    float zInput;

    bool canSprint = true;

    [SerializeField] float gravity = -30f;
    Vector3 gravityVelocity = Vector3.zero;
    public int maxJumps = 1;
    public int jumpsLeft = 1;

    public Transform groundCheckTransform;
    public LayerMask terrainLayers;

    

    Vector3 playerInput;
    Vector3 cameraRelativeMovement;

    public Transform projectilePoint;
    public GameObject projectilePrefab;
    


    public CharacterStats characterStats;
    [SerializeField] CharacterController characterController;
 


    void Awake() 
    {
        dashAbility = GetComponent<Dash>();
        characterStats = GetComponent<CharacterStats>();
        characterController = GetComponent<CharacterController>();
    }
 
    void Update()
    {
        // ShootProjectile();
        if (dashAbility != null && dashAbility.isDashing)
        return;

        ApplyGravity();
        PlayerJump();
        // SwingWeapon();

        xInput = Input.GetAxis("Horizontal");
        zInput = Input.GetAxis("Vertical");

        playerInput.x = xInput;
        playerInput.z = zInput;

        cameraRelativeMovement = ConvertToCameraSpace(playerInput);

        PlayerSprint();
        characterController.Move(cameraRelativeMovement * characterStats.CharacterSpeed * Time.deltaTime);
        
        OpenMenu();
        OpenStats();
    }

    private void OpenStats()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (characterStatsUI.activeInHierarchy)
            {
                characterStatsUI.SetActive(false);
            }
            else
            {
                characterStatsUI.SetActive(true);
            }
        }
    }

    void LateUpdate() 
    {
        //Interact();
        HandleRotation();
    }

    void HandleRotation()
    {
        Vector3 positionToLookAt;

        positionToLookAt.x = cameraRelativeMovement.x;
        positionToLookAt.y = 0;
        positionToLookAt.z = cameraRelativeMovement.z;

        Quaternion currentRotation = transform.rotation;

        if(playerInput != Vector3.zero) 
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, 10 * Time.deltaTime);
        }
    }

    Vector3 ConvertToCameraSpace(Vector3 vectorToRotate) 
    {
        float currentYValue = vectorToRotate.y;

        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward = cameraForward.normalized;
        cameraRight = cameraRight.normalized;

        Vector3 cameraForwardZProduct = vectorToRotate.z * cameraForward;
        Vector3 cameraRightXProduct = vectorToRotate.x * cameraRight;

        Vector3 vectorRotatedToCameraSpace = cameraForwardZProduct + cameraRightXProduct;
        vectorRotatedToCameraSpace.y = currentYValue;
        return vectorRotatedToCameraSpace;
    }

    void PlayerSprint() 
    {
        if(Input.GetKey(KeyCode.LeftShift) && canSprint && characterStats.energyPoints > 0f)
         {
            characterStats.CharacterSpeed = characterStats.SprintSpeed;
            characterStats.UseEnergy(characterStats.EnergyCost * Time.deltaTime); // flat energy usage

            if (characterStats.energyPoints <= 0f)
            {
                canSprint = false;
                characterStats.CharacterSpeed = 5f;
            }
         } 
         else 
         {
            if(Input.GetKeyUp(KeyCode.LeftShift) && canSprint )
            {
            characterStats.CharacterSpeed = characterStats.DefaultSpeed;
            }
            if (!canSprint && characterStats.energyPoints >= characterStats.maxEnergyPoints) // basic timer to disallow sprinting unless refilled completely
            {
                canSprint = true;
            }
         }
    }

    void PlayerJump() 
    {
        if(Input.GetKey(KeyCode.Space) && characterStats.energyPoints > 0f) 
        {

            if (CharacterOnGround()) {
                jumpsLeft = maxJumps;
                gravityVelocity.y = 0;
            } else if (jumpsLeft < 1) {
                return;
            }
             jumpsLeft--;
            if (gravityVelocity.y < 0) {
                gravityVelocity.y = 0;
            }

            gravityVelocity.y = characterStats.JumpSpeed;
            characterStats.UseEnergy(characterStats.EnergyCost * Time.deltaTime); // flat energy usage
            
            Debug.Log("Jump");
        }
        
    }

    void ApplyGravity()
    {
        if (characterController.isGrounded && gravityVelocity.y < 0) {
            gravityVelocity = Vector3.zero;
        }
        gravityVelocity.y += gravity * Time.deltaTime;

        characterController.Move(gravityVelocity * Time.deltaTime);
    }

    public bool CharacterOnGround() {

        return Physics.OverlapSphere(groundCheckTransform.position, .5f, terrainLayers).Length > 0;
    }
    
    // void ShootProjectile()
    // {
    //     if (Input.GetKeyDown(KeyCode.Q) && projectilePrefab != null && characterStats.classTypeCached == "Mage Class")
    //     {
    //         if (characterStats.manaPoints >= characterStats.projectileCost)
    //         {
    //             characterStats.UseMana(characterStats.ProjectileCost); // flat energy usage
    //             Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //             if (Physics.Raycast(ray, out RaycastHit hit, 999f))
    //             {
    //                 Vector3 direction = (hit.point - projectilePoint.position).normalized;
    //                 GameObject projectileInstance = Instantiate(projectilePrefab, projectilePoint.position, Quaternion.LookRotation(direction));
    //                 Projectile projectileComponent = projectileInstance.GetComponent<Projectile>();

    //                 if (projectileComponent != null)
    //                 {
    //                     projectileComponent.Launch(direction, characterStats.magicDamage, characterStats.projectileSpeed);
    //                     projectileComponent.SetProjectileStats(characterStats); // ✅ pass stats to projectile
    //                 }
    //             }
    //         }
    //         else
    //         {
    //             Debug.Log("NOT ENOUGH MANA or not a mage or projectile prefab is null");
    //         }
            
    //     }
    // }

    // void Interact()
    // {
    //     if (Input.GetKeyDown(KeyCode.E))
    //     {
    //         Debug.Log("Interact Button Pressed");
    //         //get object reference in raycast from player interact
    //     }
    // }
    // void SwingWeapon()
    // {
    //     if (Input.GetKeyDown(KeyCode.Mouse0) && characterStats.classTypeCached != "Mage Class")
    //     {
    //         Debug.Log("attacking");
    //     }
    //     else
    //     {
    //         Debug.Log("Is mage or some");
    //     }
    // }

    void OpenMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (MainMenuUI.activeInHierarchy)
            {
                MainMenuUI.SetActive(false);
            }
            else
            {
                MainMenuUI.SetActive(true);
            }
        }
    }
}


