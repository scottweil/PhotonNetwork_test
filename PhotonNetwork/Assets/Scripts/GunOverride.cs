using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunOverride : MonoBehaviour
{
    public void FireActive()
    {
        Gun gun = GetComponentInParent<Gun>();
        gun.Fire();
    }
}
