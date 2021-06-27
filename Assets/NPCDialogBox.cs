using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class NPCDialogBox : MonoBehaviour
{
    private float despawnTimer;
    public float coolDown;
    public bool go;
    private bool waiting = false;

    public GameObject teleportTo;
    // Start is called before the first frame update
    void OnEnable()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (go && !waiting)
        {
            despawnTimer = Time.time + coolDown;
            waiting = true;
            gameObject.transform.position = teleportTo.transform.position;
        }
        
        if (waiting && despawnTimer < Time.time)
        {
            gameObject.SetActive(false);
        }
    }
}
