using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField]
    Image HealthBar;

    [SerializeField]
    TextMeshProUGUI HealthText;
    [SerializeField] GameObject MaxhealthText;

    [HideInInspector]public float health;
    float maxHealth = 100;
    float lerpSpeed;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        HealthText.text = health + "";
        if (health > maxHealth) health = maxHealth;
        lerpSpeed = 3f * Time.deltaTime;
        HealthBarFiller();
        ColorChanger();

    }

    private void HealthBarFiller()
    {
        HealthBar.fillAmount = Mathf.Lerp(HealthBar.fillAmount, (health / maxHealth), lerpSpeed);
    }

    void ColorChanger()
    {
        Color healthColor = Color.Lerp(Color.red, Color.green, (health / maxHealth));
        HealthBar.color = healthColor; 
    }
    public void Damage(float damagePoints)
    {
        if (health > 0)
            health -= damagePoints;
    }

    public void Heal(float healingPoints)
    {
        if(health >= 0 && health <= 90)
        {
            health += healingPoints;

        }
        else
        {
            MaxhealthText.SetActive(true);
            StartCoroutine(DisableText());
        }


    }

    IEnumerator DisableText()
    {
        yield return new WaitForSeconds(2f);
        MaxhealthText.SetActive(false);

    }
}
