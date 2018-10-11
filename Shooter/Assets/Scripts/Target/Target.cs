using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Target : MonoBehaviour
{
    public int Health = 0;
    public abstract void Damage(GunController gun);
}
