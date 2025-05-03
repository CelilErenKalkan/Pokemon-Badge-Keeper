using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPokemon", menuName = "ScriptableObjects/Pokemon", order = 1)]
public class PokemonData : ScriptableObject
{
    public string pokemonName;
    public classification classification;
    public string pokedex_entry;
    public int dex_no;

    [SerializeReference]
    public List<Ability> possibleAbilities = new();

    // Change Texture2D to Sprite for better display in the inspector
    public Sprite pokemonImage;
    public Sprite frontAnimatedSpriteSheet; // Assigned in Inspector for animation
    public Sprite backAnimatedSpriteSheet;
    public Sprite frontShinyAnimatedSpriteSheet;
    public Sprite backShinyAnimatedSpriteSheet;
}