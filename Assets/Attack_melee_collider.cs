using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Attack_melee_collider : MonoBehaviour
{
    public int damage;
    public GameObject Melee;
    private PhotonView PV;
    private void Start()
    {
        damage = Melee.GetComponent<MeleeSpells>().attackDamage;
        PV = GetComponent<PhotonView>();
    }
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("here");
        if (col.GetComponent<EnemyHealth>() != null)
        {
            Debug.Log("ici");
            col.GetComponent<EnemyHealth>().DamageEnemy(damage);
        }
    }
}
