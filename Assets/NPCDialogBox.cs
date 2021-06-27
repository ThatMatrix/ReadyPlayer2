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

    public GameObject VictoryMenu;

    public GameObject teleportTo;
    // Start is called before the first frame update
    void OnEnable()
    {
        currentScene = SceneManager.GetActiveScene().name;
        VictoryMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (go && !waiting)
        {
            despawnTimer = Time.time + coolDown;
            waiting = true;
            gameObject.transform.position = teleportTo.transform.position;
            Debug.Log("Teleported");
        }
        
        if (waiting && despawnTimer < Time.time)
        {
            if (currentScene == "map3")
            {
                VictoryMenu.SetActive(true);
                Debug.Log("Activated victory menu");
            }
            gameObject.SetActive(false);
        }
    }
}
