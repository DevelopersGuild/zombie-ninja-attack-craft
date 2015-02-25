using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    private static GameManager _instance;
    public static GameManager instance { get { return _instance ?? (_instance = new GameManager()); } }

    public int points;

	// Use this for initialization
	void Start () {
        points = 0;
	}
	
    public void AddPoints(int added) {
        points += added;
    }

    public void Reset() {
        points = 0;
    }

    public void setPoints(int pointsSet) {
        points = pointsSet;
    }
}
