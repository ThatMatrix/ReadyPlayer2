using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTalkingBubble : MonoBehaviour
{
    private float timeBeforeDespawn;
    public GameObject text;
    public float cooldown;
    
    // Start is called before the first frame update
    void Start()
    {
        timeBeforeDespawn = Time.time + cooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > timeBeforeDespawn)
        {
            text.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
