using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class NPCDialogBox : MonoBehaviour
{
    private float despawnTimer;
    public float coolDown;
    public bool go;
    private bool waiting = false;

    private string currentScene;

    public GameObject teleportTo;
    // Start is called before the first frame update
    void OnEnable()
    {
        currentScene = SceneManager.GetActiveScene().name;
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
            if (currentScene == "map3")
            {
                
            }
            gameObject.SetActive(false);
        }
    }
}
