using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakePlayer 
{
    private int _id;
    private string _name;
    private int _score;

    public FakePlayer(int id, string name, int score)
    {
        _id = id;
        _name = name;
        _score = score;
    }
    public int Id => _id;
    public string Name => _name;
    public int Score => _score;

}
