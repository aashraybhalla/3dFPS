using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{

    public float playerHealth = 100;
    public float playerCurrentHealth;
    // Start is called before the first frame update
    void Start()
    {
        playerCurrentHealth = playerHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetHurt(float hurt)
    {
        playerCurrentHealth -= hurt;

        if (playerCurrentHealth < 0)
        {
            GameObject.Destroy(gameObject);
        }
    }
}
