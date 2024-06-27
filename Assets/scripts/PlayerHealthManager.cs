using TMPro;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{
    private static PlayerHealthManager instance;

    public static PlayerHealthManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerHealthManager>();
                if (instance == null)
                {
                    GameObject managerObject = new GameObject("PlayerHealthManager");
                    instance = managerObject.AddComponent<PlayerHealthManager>();
                    DontDestroyOnLoad(managerObject);
                }
            }
            return instance;
        }
    }

    public int maxHealth = 100; // �ִ� ü��
    public int currentHealth=50; // ���� ü��
    public TextMeshProUGUI HealthText;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        currentHealth = maxHealth;
    }

    void Start()
    {
        // goldText�� �������� �ʾ����� �������� ã��
        if (HealthText == null)
        {
            HealthText = GameObject.FindWithTag("HealthText").GetComponent<TextMeshProUGUI>();
        }

        UpdateHealthText();
    }

    void UpdateHealthText()
    {
        if (HealthText != null)
        {
            HealthText.text = "Health: " + currentHealth.ToString();
        }
        else
        {
            Debug.LogError("Health Text is not assigned.");
        }
    }


    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Took damage: " + amount + ", Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        UpdateHealthText();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        Debug.Log("Healed: " + amount + ", Current health: " + currentHealth);
    }

    void Die()
    {
        Debug.Log("Player died!");
        // �÷��̾� ��� ó�� �ڵ�
    }
}
