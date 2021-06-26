using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaguesManagement : MonoBehaviour
{
    public int curVagues;
    public int nbVaguesMax;

    bool HasEnemiesLeftOnTheMap()
    {
        return GameObject.FindGameObjectsWithTag("Enemy")
    }
}
