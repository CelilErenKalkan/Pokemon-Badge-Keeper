using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPokemon", menuName = "ScriptableObjects/Pokemon", order = 1)]
public class PokemonBase : ScriptableObject
{
    [SerializeField] private string pokemonName;
    public string PokemonName => pokemonName;
    [SerializeField] private classification classification;
    public classification Classification => classification;
    [TextArea][SerializeField] private string pokedex_entry;
    public string PokedexEntry => pokedex_entry;
    [SerializeField] private int dex_no;
    public int DexNo => dex_no;

    [SerializeReference]
    public List<Ability> possibleAbilities = new();

    // Change Texture2D to Sprite for better display in the inspector
    [SerializeField] private Sprite pokemonImage;
    public Sprite PokemonImage => pokemonImage;
    [SerializeField] private Sprite frontAnimatedSpriteSheet; // Assigned in Inspector for animation
    public Sprite FrontAnimatedSpriteSheet => frontAnimatedSpriteSheet;
    [SerializeField] private Sprite backAnimatedSpriteSheet;
    public Sprite BackAnimatedSpriteSheet => backAnimatedSpriteSheet;
    [SerializeField] private Sprite frontShinyAnimatedSpriteSheet;
    public Sprite FrontShinyAnimatedSpriteSheet => frontShinyAnimatedSpriteSheet;
    [SerializeField] private Sprite backShinyAnimatedSpriteSheet;
    public Sprite BackShinyAnimatedSpriteSheet => backShinyAnimatedSpriteSheet;

    // Type
    [SerializeField] private PokemonType type1;
    public PokemonType Type1 => type1;
    [SerializeField] private PokemonType type2;
    public PokemonType Type2 => type2;

    // Stats
    [SerializeField] private int maxHp;
    public int MaxHP => maxHp;
    [SerializeField] private int attack;
    public int Attack => attack;
    [SerializeField] private int defence;
    public int Defence => defence;
    [SerializeField] private int spAttack;
    public int SpAttack => spAttack;
    [SerializeField] private int spDefence;
    public int SpDefence => spDefence;
    [SerializeField] private int speed;
    public int Speed => speed;

    [SerializeField] private List<LearnableMoves> learnableMoves;
    public List<LearnableMoves> LearnableMoves => learnableMoves;
}

[Serializable]
public class LearnableMoves
{
    [SerializeField] private MoveBase moveBase;
    public MoveBase MoveBase => moveBase;
    [SerializeField] private int requiredLevel;
    public int RequiredLevel => requiredLevel;
}

public enum PokemonType
{
    None,
    Normal,
    Fire,
    Water,
    Electric,
    Grass,
    Ice,
    Fighting,
    Poison,
    Ground,
    Flying,
    Psychic,
    Bug,
    Rock,
    Ghost,
    Dragon,
    Dark,
    Steel,
    Fairy
}