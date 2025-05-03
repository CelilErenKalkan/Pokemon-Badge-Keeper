using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokemon
{
    private PokemonBase @base;
    private string nickName;
    private bool hasNickname;

    private int level;
    private int HP { get; set; }
    
    // IVs
    private int ivMaxHp;
    private int ivAttack;
    private int ivDefence;
    private int ivSpAttack;
    private int ivSpDefence;
    private int ivSpeed;
    
    // EVs
    private int evMaxHp;
    private int evAttack;
    private int evDefence;
    private int evSpAttack;
    private int evSpDefence;
    private int evSpeed;

    public List<Move> Moves { get; set; }

    public Pokemon(PokemonBase pBase, int pLevel)
    {
        @base = pBase;
        level = pLevel;
        HP = pBase.MaxHP;
        
        SetInitialMoves();
    }

    public string GetName()
    {
        if (hasNickname) return nickName;

        return @base.PokemonName;
    }

    private void SetInitialMoves()
    {
        Moves = new List<Move>();
        
        foreach (var move in @base.LearnableMoves)
        {
            if (move.RequiredLevel <= level)
                Moves.Add(new Move(move.MoveBase));
                
            if (Moves.Count >= 4)
                break;
        }
    }

    public int MaxHp() => Mathf.FloorToInt(((2 * @base.MaxHP + ivMaxHp + evMaxHp / 4) * level) / 100.0f) + level + 10;
    public int Attack() => Mathf.FloorToInt(((2 * @base.Attack + ivAttack + evAttack / 4) * level) / 100.0f) + 5;
    public int Defence() => Mathf.FloorToInt(((2 * @base.Defence + ivDefence + evDefence / 4) * level) / 100.0f) + 5;
    public int SpAttack() => Mathf.FloorToInt(((2 * @base.SpAttack + ivSpAttack + evSpAttack / 4) * level) / 100.0f) + 5;
    public int SpDefence() => Mathf.FloorToInt(((2 * @base.Attack + ivSpDefence + evSpDefence / 4) * level) / 100.0f) + 5;
    public int Speed() => Mathf.FloorToInt(((2 * @base.Speed + ivSpeed + evSpeed / 4) * level) / 100.0f) + 5;

    public int Move (int power, int oppDefence) => Mathf.FloorToInt(((2 * level) / 5 + 2) * power * (@base.Attack / oppDefence) / 50.0f) + 5 + 2;
}
