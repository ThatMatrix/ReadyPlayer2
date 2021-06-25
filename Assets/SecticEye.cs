using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;

public class SecticEye : MonoBehaviour
{
    public int hp2ndPhase;
    public int hp3rdPhase;
    public List<GameObject> points;
    public float shotsForce;
    
    private EnemyHealth _health;
    private PhotonView PV;
    private Animator _animator;
    public float startTimeBtwShots;
    
    // Start is called before the first frame update
    void Start()
    {
        Quaternion droite = new Quaternion(1, 1, 0, 0);
        points[0].transform.rotation = droite;
        points[1].transform.rotation = droite;
        points[2].transform.rotation = droite;
        
        Quaternion gauche = new Quaternion(0, 0, 1, 1);
        points[3].transform.rotation = gauche;
        points[4].transform.rotation = gauche;
        points[5].transform.rotation = gauche;
        
        _animator = GetComponent<Animator>();
        PV = gameObject.GetComponent<PhotonView>();
        _health = gameObject.GetComponent<EnemyHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_health.curHealth <= hp3rdPhase)
        {
            _animator.SetTrigger("Third Phase");
        }
        else if (_health.curHealth <= hp2ndPhase)
        {
            _animator.SetTrigger("Second Phase");
        }
    }

    public void ShootFromLeft()
    {
        Quaternion droite = new Quaternion(1, 1, 0, 0);
        
        GameObject bullet1 = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Tear (SepticEye)"),
            points[0].transform.position, droite);
        Rigidbody2D RB1 = bullet1.GetComponent<Rigidbody2D>();
        RB1.AddForce(points[0].transform.up * shotsForce, ForceMode2D.Impulse);
        
        
        GameObject bullet2 = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Tear (SepticEye)"),
            points[1].transform.position, droite);
        Rigidbody2D RB2 = bullet2.GetComponent<Rigidbody2D>();
        RB2.AddForce(points[1].transform.up * shotsForce, ForceMode2D.Impulse);
        
        
        GameObject bullet3 = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Tear (SepticEye)"),
            points[2].transform.position, droite);
        Rigidbody2D RB3 = bullet3.GetComponent<Rigidbody2D>();
        RB3.AddForce(points[2].transform.up * shotsForce, ForceMode2D.Impulse);
    }
    
    public void ShootFromRight()
    {
        Quaternion gauche = new Quaternion(0, 0, 1, 1);
        
        GameObject bullet1 = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Tear (SepticEye)"),
            points[3].transform.position, gauche);
        Rigidbody2D RB1 = bullet1.GetComponent<Rigidbody2D>();
        RB1.AddForce(points[3].transform.up * shotsForce, ForceMode2D.Impulse);
        
        
        GameObject bullet2 = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Tear (SepticEye)"),
            points[4].transform.position, gauche);
        Rigidbody2D RB2 = bullet2.GetComponent<Rigidbody2D>();
        RB2.AddForce(points[4].transform.up * shotsForce, ForceMode2D.Impulse);
        
        
        GameObject bullet3 = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Tear (SepticEye)"),
            points[5].transform.position, gauche);
        Rigidbody2D RB3 = bullet3.GetComponent<Rigidbody2D>();
        RB3.AddForce(points[5].transform.up * shotsForce, ForceMode2D.Impulse);
    }
}
