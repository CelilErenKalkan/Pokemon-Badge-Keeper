using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokemon
{
    private PokemonData data;
    private string nickName;
    private bool hasNickname;
    
    public Pokemon()
    {
        
    }

    public string GetName()
    {
        if (hasNickname) return nickName;

        return data.pokemonName;
    }
}
