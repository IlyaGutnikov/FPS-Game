using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

    private const string PLAYER_ID_PREFIX = "Player";

    private static Dictionary<string, Player> players = new Dictionary<string, Player>();

    public static void RegisterPlayer(string _netId, Player _player)
    {

        string _playerId = PLAYER_ID_PREFIX + _netId;
        players.Add(_playerId, _player);
        _player.transform.name = _playerId;
    }

    public static void UnRegisterPlayer(string _playerId)
    {
        players.Remove(_playerId);
    }

    public static Player GetPlayer(string _playerId) {

        return players[_playerId];
    }

  /*  void OnGUI()
    {

        GUILayout.BeginArea(new Rect(200, 200, 200, 500));
        GUILayout.BeginVertical();

        foreach (string playerId in players.Keys) {

            GUILayout.Label(playerId + " - " + players[playerId].transform.name);
        }

            GUILayout.EndVertical();
        GUILayout.EndArea();
    }*/
}
