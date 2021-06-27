using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogBox : MonoBehaviour
{
    private float despawnTimer;
    public float coolDown;
    
    // Start is called before the first frame update
    void Start()
    {
        despawnTimer = Time.time + coolDown;
    }

    // Update is called once per frame
    void Update()
    {
        if (despawnTimer < Time.time)
        {
            gameObject.SetActive(false);
        }
    }
}
