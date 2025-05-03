using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newMove", menuName = "ScriptableObjects/Moves", order = 2)]
public class MoveBase : ScriptableObject
{
    [SerializeField] private string moveName;
    public string MoveName => moveName;
    
    [TextArea][SerializeField] private string description;
    public string Description => description;

    [SerializeField] private PokemonType type;
    public PokemonType MoveType => type;
    [SerializeField] private int power;
    public int Power => power;
    [SerializeField] private int accuracy;
    public int Accuracy => accuracy;
    [SerializeField] private int pp;
    public int PP => pp;
}
