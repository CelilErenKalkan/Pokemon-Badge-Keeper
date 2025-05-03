using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Ability
{
    protected string Name { get; set; }
    public string AbilityName => Name;
    protected string Description { get; set; }
    public string AbilityDescription => Description;
    protected Pokemon Pokemon { get; private set; }

    [HideInInspector]public bool isHidden;
    
    private void SetPokemon(Pokemon pokemon)
    {
        Pokemon = pokemon;
    }

    protected virtual void AbilityFunction()
    {
        // Mechanic of the ability goes here.
    }

    public void CallAbilityFunction(Pokemon pokemon)
    {
        SetPokemon(pokemon);
        AbilityFunction();
    }
    
}
