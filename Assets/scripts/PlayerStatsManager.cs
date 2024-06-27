using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    private static PlayerStatsManager instance;

    public static PlayerStatsManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerStatsManager>();
                if (instance == null)
                {
                    GameObject managerObject = new GameObject("PlayerStatsManager");
                    instance = managerObject.AddComponent<PlayerStatsManager>();
                    DontDestroyOnLoad(managerObject);
                }
            }
            return instance;
        }
    }

    public int strength = 10; // Èû
    public int agility = 10; // ¹ÎÃ¸¼º
    public int intelligence = 10; // Áö´É
    public int MoveSpeed = 3;
    public int JumpForce = 8;
    
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
    }

    public void IncreaseStrength(int amount)
    {
        strength += amount;
        Debug.Log("Strength increased: " + strength);
    }

    public void IncreaseAgility(int amount)
    {
        agility += amount;
        Debug.Log("Agility increased: " + agility);
    }

    public void IncreaseIntelligence(int amount)
    {
        intelligence += amount;
        Debug.Log("Intelligence increased: " + intelligence);
    }
    public void IncreaseMoveSpeed(int amount)
    {
        MoveSpeed += amount;
        Debug.Log("MoveSpeed increased: " + MoveSpeed);
    }
    public void IncreaseJumpForce(int amount)
    {
        JumpForce += amount;
        Debug.Log("JumpForce increased: " + JumpForce);
    }
}
