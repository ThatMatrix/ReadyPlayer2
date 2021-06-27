using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;
using Path = System.IO.Path;

public class MeleeSpells : PlayerSpells
{
    public float dashlength;
    public float kunaiForce;
    
    public int attackDamage = 20;
    public float attackRange = 1f;
    public Vector3 attackOffset;
    public LayerMask attackMask;
    
    public GameObject bas;
    public GameObject haut;
    public GameObject droiteHaut;
    public GameObject droiteBas;
    public GameObject gaucheBas;
    public GameObject gaucheHaut;

    public bool HasBeenBoosted = false;
        public void SetRotations()
    {
        haut.transform.rotation = new Quaternion(0, 1, 0, 0);
        bas.transform.rotation = new Quaternion(1, 0, 0, 0);
        droiteBas.transform.rotation = new Quaternion(1, 1, 1, 0);
        droiteHaut.transform.rotation = new Quaternion(1, 1, 0, 1);
        gaucheHaut.transform.rotation = new Quaternion(0, 1, 1, 1);
        gaucheBas.transform.rotation = new Quaternion(1, 0, 1, 1);
    }
    public override void SetCooldowns()
    {
        SetRotations();
        cooldown1 = 2.5f;
        cooldown2 = 2f;
        cooldownM = 5f;
        cooldownU = 10f;
    }

    public override void MainSpell()
    {
        _animator.SetTrigger("Attack");
        
        Vector2 movement = GetComponent<PlayerMovement>().GetMovement();
        GetComponent<PlayerMovement>().enabled = false;
        
        if (movement.x > 0 && movement.x > movement.y) // Dash Right
        {
            GetComponent<ShotGunDash>().SetDirection(2);
        }
        else if (movement.x < 0 && movement.x < movement.y) // Dash Left
        {
            GetComponent<ShotGunDash>().SetDirection(1);
        }
        else if (movement.y > 0 && movement.y > movement.x) // Dash Up
        {
            GetComponent<ShotGunDash>().SetDirection(3);
        }
        else if (movement.y < 0 && movement.y < movement.x) // Dash Down
        {
            GetComponent<ShotGunDash>().SetDirection(4);
        }

        GetComponent<ShotGunDash>().enabled = true;
        
        
        Attack(movement.x > 0);
    }

    public override void SecondarySpell()
    {
        GameObject kunai = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "kunai"), 
            firePoint.transform.position, firePoint.transform.rotation);
        
        Rigidbody2D rbBullet = kunai.GetComponent<Rigidbody2D>();
        rbBullet.AddForce(firePoint.transform.up * kunaiForce, ForceMode2D.Impulse);
    }

    public override void Ultimate()
    {
        // HAUT
        GameObject kunaiHAUT = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "kunaiTMP"), 
            haut.transform.position, haut.transform.rotation);
        
        Rigidbody2D rbBullet = kunaiHAUT.GetComponent<Rigidbody2D>();
        rbBullet.AddForce(haut.transform.up * kunaiForce, ForceMode2D.Impulse);
        
        // Bas
        GameObject kunaiBAS = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "kunaiTMP"), 
            bas.transform.position, bas.transform.rotation);
        
        Rigidbody2D rbBulletBAS = kunaiBAS.GetComponent<Rigidbody2D>();
        rbBulletBAS.AddForce(bas.transform.up * kunaiForce, ForceMode2D.Impulse);
        
        //Droite
        Quaternion droite = new Quaternion(1, 1, 0, 0);
        GameObject kunaiDROITE = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "kunaiTMP"), 
            firePointRight.transform.position, droite);
        
        Rigidbody2D rbBulletDROITE = kunaiDROITE.GetComponent<Rigidbody2D>();
        rbBulletDROITE.AddForce(kunaiDROITE.transform.up * kunaiForce, ForceMode2D.Impulse);
        
        //Gauche
        Quaternion gauche = new Quaternion(0, 0, 1, 1);
        GameObject kunaiGAUCHE = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "kunaiTMP"), 
            firePointLeft.transform.position, gauche);
        
        Rigidbody2D rbBulletGAUCHE = kunaiGAUCHE.GetComponent<Rigidbody2D>();
        rbBulletGAUCHE.AddForce(kunaiGAUCHE.transform.up * kunaiForce, ForceMode2D.Impulse);
        
        
        // BasGauche
        GameObject kunaiBASGAUCHE = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "kunaiTMP"), 
            gaucheBas.transform.position, gaucheBas.transform.rotation);
        
        Rigidbody2D rbBulletBASGAUCHE = kunaiBASGAUCHE.GetComponent<Rigidbody2D>();
        rbBulletBASGAUCHE.AddForce(gaucheBas.transform.up * kunaiForce, ForceMode2D.Impulse);
        
        // BasDroite
        GameObject kunaiBASDROITE = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "kunaiTMP"), 
            droiteBas.transform.position, droiteBas.transform.rotation);
        
        Rigidbody2D rbBulletBASDROITE = kunaiBASDROITE.GetComponent<Rigidbody2D>();
        rbBulletBASDROITE.AddForce(droiteBas.transform.up * kunaiForce, ForceMode2D.Impulse);
        
        
        // HautGauche
        GameObject kunaiHAUTGAUCHE = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "kunaiTMP"), 
            gaucheHaut.transform.position, gaucheHaut.transform.rotation);
        
        Rigidbody2D rbBulletHautGAUCHE = kunaiHAUTGAUCHE.GetComponent<Rigidbody2D>();
        rbBulletHautGAUCHE.AddForce(gaucheHaut.transform.up * kunaiForce, ForceMode2D.Impulse);
        
        // HautDroite
        GameObject kunaiHAUTDroite = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "kunaiTMP"), 
            droiteHaut.transform.position, droiteHaut.transform.rotation);
        
        Rigidbody2D rbBulletHautDroite = kunaiHAUTDroite.GetComponent<Rigidbody2D>();
        rbBulletHautDroite.AddForce(droiteHaut.transform.up * kunaiForce, ForceMode2D.Impulse);
    }

    public override void MovementSpell()
    {
        Debug.Log("Got to movement spell melee");
        Vector2 movement = GetComponent<PlayerMovement>().GetMovement();

        if (movement.x > 0 && movement.x > movement.y) // Dash Right
        {
            rb.position = rb.position + Vector2.right * dashlength;
        }
        else if (movement.x < 0 && movement.x < movement.y) // Dash Left
        {
            rb.position = rb.position + Vector2.left * dashlength;
        }
        else if (movement.y > 0 && movement.y > movement.x) // Dash Up
        {
            rb.position = rb.position + Vector2.up * dashlength;
        }
        else if (movement.y < 0 && movement.y < movement.x) // Dash Down
        {
            rb.position = rb.position + Vector2.down * dashlength;
        }
    }
    
    
    public void Attack(bool right)
    {
        Debug.Log("Attack melee facing right :" + right);
        Vector3 pos = transform.position;
        if (right)
        {
            pos += transform.right * attackOffset.x;
        }
        else
        {
            pos += transform.right * -attackOffset.x;
        }
        pos += transform.up * attackOffset.y;
        
        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
        if (colInfo != null && (colInfo.CompareTag("Enemy") || colInfo.CompareTag("Droid")))
        {
            colInfo.GetComponent<EnemyHealth>().DamageEnemy(attackDamage);
            Debug.Log("Attack done");
        }
    }
    
    void OnDrawGizmosSelected()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Gizmos.DrawWireSphere(pos, attackRange);
    }
}
