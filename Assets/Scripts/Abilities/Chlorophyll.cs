using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Chlorophyll : Ability
{
    public Chlorophyll()
    {
        Name = "Chlorophyll";
        Description = "When sunny, the Pokémon’s Speed doubles.";
    }

    protected override void AbilityFunction()
    {
        // Mechanic of the ability goes here.
    }
}
