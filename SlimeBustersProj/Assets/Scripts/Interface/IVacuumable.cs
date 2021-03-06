using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVacuumable
{
    public void GetVacuumed(Transform point, float maxVacuumForce, float minVacuumForce, float multiplier);
}
