using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;
using Photon.Pun;

public class ShotgunSpells : PlayerSpells
{
    [SerializeField] private float rangeMolly = 15f;
    public float shotGunForce = 20f;
    public float ultimateBulletSpeed = 20f;

    public GameObject DroiteHaut;
    public GameObject DroiteBas;
    public GameObject GaucheHaut;
    public GameObject GaucheBas;
    
    [SerializeField] private float dashSpeed;
    private float dashTime;

    public override void SetCooldowns()
    {
        cooldown1 = 1.5f;
        cooldown2 = 4f;
        cooldownM = 2f;
        cooldownU = 0.5f;
    }

    public override void MainSpell()
    {
        FindObjectOfType<AudioManager>().Play("shotgun");
        if (!right)
        {
            _animator.Play("ShotGunShootR");
        }
        else
        {
            _animator.Play("ShotGunShootL");
        }
        
        GameObject shotgunBullet1 = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "ShotGunBullet"), 
            firePoint.transform.position, firePoint.transform.rotation);
        
        Rigidbody2D rbBullet1 = shotgunBullet1.GetComponent<Rigidbody2D>();
        rbBullet1.AddForce(firePoint.transform.up * shotGunForce, ForceMode2D.Impulse);

        if (right)
        {
            GameObject shotgunBullet2 = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "ShotGunBullet"), 
                DroiteHaut.transform.position, firePoint.transform.rotation);
        
            Rigidbody2D rbBullet2 = shotgunBullet2.GetComponent<Rigidbody2D>();
            rbBullet2.AddForce(firePoint.transform.up * shotGunForce, ForceMode2D.Impulse);
            
            GameObject shotgunBullet3 = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "ShotGunBullet"), 
                DroiteBas.transform.position, firePoint.transform.rotation);
        
            Rigidbody2D rbBullet3 = shotgunBullet3.GetComponent<Rigidbody2D>();
            rbBullet3.AddForce(firePoint.transform.up * shotGunForce, ForceMode2D.Impulse);
        }
        else
        {
            GameObject shotgunBullet2 = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "ShotGunBullet"), 
                GaucheHaut.transform.position, firePoint.transform.rotation);
        
            Rigidbody2D rbBullet2 = shotgunBullet2.GetComponent<Rigidbody2D>();
            rbBullet2.AddForce(firePoint.transform.up * shotGunForce, ForceMode2D.Impulse);
            
            GameObject shotgunBullet3 = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "ShotGunBullet"), 
                GaucheBas.transform.position, firePoint.transform.rotation);
        
            Rigidbody2D rbBullet3 = shotgunBullet3.GetComponent<Rigidbody2D>();
            rbBullet3.AddForce(firePoint.transform.up * shotGunForce, ForceMode2D.Impulse);
        }
    }

    public override void SecondarySpell()
    {
        FindObjectOfType<AudioManager>().Play("bomb");
        if (right)
        {
            _animator.Play("ShotGunShootR");
        }
        else
        {
            _animator.Play("ShotGunShootL");
        }
        if (rangeMolly - Vector2.Distance(mousePos, gameObject.transform.position) >= 0)
        {
            Debug.Log("In range");
            GameObject molly = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Molly"),
                mousePos, Quaternion.identity);
        }
        else
        {
            Debug.Log("NOT In range");
            GameObject molly = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Molly"),
                gameObject.transform.position + (Vector3) lookDir.normalized * rangeMolly, Quaternion.identity);
        }
    }

    public override void MovementSpell()
    {
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
    }

    
    
    public override void Ultimate()
    {
        FindObjectOfType<AudioManager>().Play("canon");
        if (right)
        {
            _animator.Play("ShotGunShootR");
        }
        else
        {
            _animator.Play("ShotGunShootL");
        }
        GameObject ultimateBullet = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "UltimateBullet"), 
            firePoint.transform.position, firePoint.transform.rotation);
        Rigidbody2D rbBullet2 = ultimateBullet.GetComponent<Rigidbody2D>();
        rbBullet2.AddForce(firePoint.transform.up * ultimateBulletSpeed, ForceMode2D.Impulse);
    }
}
