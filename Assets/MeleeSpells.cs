using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSpells : PlayerSpells
{
    public float dashlength;
    

    public override void SetCooldowns()
    {
        cooldown1 = 2.5f;
        cooldown2 = 2f;
        cooldownM = 5f;
        cooldownU = 10f;
    }

    public override void MainSpell()
    {
        throw new System.NotImplementedException();
    }

    public override void SecondarySpell()
    {
        throw new System.NotImplementedException();
    }

    public override void Ultimate()
    {
        throw new System.NotImplementedException();
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
}
