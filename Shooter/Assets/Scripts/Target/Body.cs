using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : Target {

    private void Awake()
    {
        Health = 100;
    }
    public override void Damage(GunController gun)
    {
        Health -= gun.Damage;

        StartCoroutine(FlashDamage());

        if (Health <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    private IEnumerator FlashDamage()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;

        yield return new WaitForSeconds(0.25f);

        GetComponent<MeshRenderer>().material.color = Color.white;
    }
}
