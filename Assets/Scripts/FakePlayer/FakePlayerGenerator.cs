using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FakePlayerGenerator : MonoBehaviour, IService
{
    [SerializeField] private int _playerCount = 7;
    [SerializeField] private int _minScore = 10000;
    [SerializeField] private int _maxScore = 20000;

    [SerializeField]
    private List<string> _availableNames = new List<string>()
    {
        "Alex", "Sam", "Jordan", "Chris", "Pat", "Taylor", "Robin",
        "Jamie", "Casey", "Morgan", "Dylan", "Riley", "Cameron", "Drew"
    };

    private const string FAKE_PLAYER_COUNT_KEY = "FAKE_PLAYER_COUNT";

    public List<FakePlayer> GetFakePlayers()
    {
        if (PlayerPrefs.HasKey(FAKE_PLAYER_COUNT_KEY))
        {
            return LoadFakePlayers();
        }
        else
        {
            return GenerateAndSaveFakePlayers();
        }
    }

    private List<FakePlayer> GenerateAndSaveFakePlayers()
    {
        List<FakePlayer> players = new List<FakePlayer>();
        List<string> namesPool = new List<string>(_availableNames);

        if (namesPool.Count < _playerCount)
        {
            Debug.LogWarning("Not enough unique names! Some names will be duplicated.");
        }

        for (int i = 0; i < _playerCount; i++)
        {
            string name = namesPool.Count > 0
                ? namesPool[Random.Range(0, namesPool.Count)]
                : "Player" + i;

            if (namesPool.Count > 0) namesPool.Remove(name);

            int score = Random.Range(_minScore, _maxScore + 1);

            SaveFakePlayer(i, name, score);
            players.Add(new FakePlayer(i, name, score));
        }

        PlayerPrefs.SetInt(FAKE_PLAYER_COUNT_KEY, _playerCount);
        PlayerPrefs.Save();

        return players;
    }

    private void SaveFakePlayer(int id, string name, int score)
    {
        PlayerPrefs.SetString($"FAKE_PLAYER_{id}_NAME", name);
        PlayerPrefs.SetInt($"FAKE_PLAYER_{id}_SCORE", score);
    }

    private List<FakePlayer> LoadFakePlayers()
    {
        int count = PlayerPrefs.GetInt(FAKE_PLAYER_COUNT_KEY);
        List<FakePlayer> players = new List<FakePlayer>();

        for (int i = 0; i < count; i++)
        {
            string name = PlayerPrefs.GetString($"FAKE_PLAYER_{i}_NAME", $"Player{i}");
            int score = PlayerPrefs.GetInt($"FAKE_PLAYER_{i}_SCORE", 0);

            players.Add(new FakePlayer(i, name, score));
        }

        return players;
    }

    [ContextMenu("Clear Fake Players")]
    public void ClearFakePlayers()
    {
        if (!PlayerPrefs.HasKey(FAKE_PLAYER_COUNT_KEY)) return;

        int count = PlayerPrefs.GetInt(FAKE_PLAYER_COUNT_KEY);

        for (int i = 0; i < count; i++)
        {
            PlayerPrefs.DeleteKey($"FAKE_PLAYER_{i}_NAME");
            PlayerPrefs.DeleteKey($"FAKE_PLAYER_{i}_SCORE");
        }

        PlayerPrefs.DeleteKey(FAKE_PLAYER_COUNT_KEY);
        PlayerPrefs.Save();
    }
}
