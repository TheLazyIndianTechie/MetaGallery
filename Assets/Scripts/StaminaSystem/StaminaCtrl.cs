using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using StarterAssets;

public class StaminaCtrl : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Stamina Main Parameters")]
    public float playerStamina = 100;
    [SerializeField] public float maxStamina = 100.0f;
    [SerializeField] private float jumpCost;
    [HideInInspector] public bool hasRegenrated = true;
    [HideInInspector] public bool isSprinting = false;

    [Header("Stamina Regen Parameters")]
    [SerializeField] private float staminaDrain = 0.5f;
    [SerializeField] private float staminaRegen = 0.5f;

    [Header("Stamina Speed Parameters")]
    [SerializeField] public int slowedRunSpeed = 4;
    [SerializeField] public int normalRunSpeed = 8;

    [Header("Stamina UI Elements")]
    [SerializeField] Image StaminaBar;
    [SerializeField] TextMeshProUGUI StaminaText;
    [SerializeField] GameObject MaxStaminaText;
    [SerializeField] GameObject LowStaminaText;

    //  private PlayerMovementSinglePlayer playerMovement;
    float health, maxHealth = 100;
    float lerpSpeed;

    void Start()
    {
        health = maxHealth;
      //  playerMovement = GetComponent<PlayerMovementSinglePlayer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSprinting)
        {
            if(playerStamina <= maxStamina - 0.01)
            {
                playerStamina += staminaRegen * Time.deltaTime;
                UpdateStamina();
                
                if(playerStamina >= maxStamina)
                {
                    
                    hasRegenrated = true;
                }
            }
        }

        if(playerStamina < 20)
        {
            LowStaminaText.SetActive(true);

        }
        else
        {
            LowStaminaText.SetActive(false);
        }

        /* StaminaText.text = health + "";
        if (health > maxHealth) health = maxHealth;
       
        HealthBarFiller();
      */
        lerpSpeed = 3f * Time.deltaTime;
        

    }

    public void Sprinting()
    {
        if (hasRegenrated)
        {
            isSprinting = true;
            playerStamina -= staminaDrain * Time.deltaTime;
            UpdateStamina();

            if(playerStamina <= 0)
            {
                hasRegenrated = false;
                UpdateStamina();
            }
        }
    }

    public void StaminaJump()
    {
        if(playerStamina >=  jumpCost)
        {
            
            playerStamina = playerStamina - jumpCost;
            Debug.Log(jumpCost);
            UpdateStamina();
        }
    }

    void UpdateStamina()
    {
        StaminaBar.fillAmount = Mathf.Lerp(StaminaBar.fillAmount, (playerStamina / maxStamina), lerpSpeed);
        StaminaText.text = (int)playerStamina + "";
        
    }

    public void Damage(float damagePoints)
    {
        if (playerStamina > 0)
        {
            playerStamina -= damagePoints;
            UpdateStamina();
        }
    }

    public void Heal(float healingPoints)
    {
        if (playerStamina <= 90)
        {
            playerStamina += healingPoints;
        }

        if(playerStamina > 90)
        {
            MaxStaminaText.SetActive(true);
            StartCoroutine(DisableText(MaxStaminaText));
        }
        UpdateStamina();
    }

    IEnumerator DisableText(GameObject text)
    {
        yield return new WaitForSeconds(2f);
        text.SetActive(false);
    }
}
