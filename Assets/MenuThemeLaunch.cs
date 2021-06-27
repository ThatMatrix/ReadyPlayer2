using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuThemeLaunch : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<AudioManager>().Play("NPCTheme");
    }
}
