using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Overgrow : Ability
{
    public Overgrow()
    {
        Name = "Overgrow";
        Description = "When HP is below 1/3rd its maximum, power of Grass-type moves is increased by 50%.";
    }

    protected override void AbilityFunction()
    {
        // Mechanic of the ability goes here.
    }
}
