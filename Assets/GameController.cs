using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public Transform gui;
	public Transform p1WinGUI;
	public Transform p2WinGUI;
	public Transform restartGUI;

	private static Vector2 gameBounds = new Vector2 (8.0f, 5.0f);

	public enum GameState
	{
		Waiting,
		Playing,
		GameOver
	}

	public GameState state;

	void Start ()
	{
		state = GameState.Waiting;

		DisableGUI (p1WinGUI);
		DisableGUI (p2WinGUI);
		DisableGUI (restartGUI);
	}

	public void EndGame (int winnerID)
	{
		state = GameState.GameOver;
		print ("winner is " + winnerID);
		switch(winnerID)
		{
			case 1:
				EnableGUI(p1WinGUI);
				break;
			case 2:
				EnableGUI(p2WinGUI);
				break;
			default:
				break;
		}

		EnableGUI (restartGUI);
	}

	public void RestartGame ()
	{
		if(state == GameState.GameOver) {
			Application.LoadLevel ("Prototype");
		}
	}	

	void DisableGUI (Transform someGUI)
	{
		someGUI.GetComponent<SpriteRenderer> ().enabled = false;
		someGUI.GetComponent<Animator> ().enabled = false;

	}

	void EnableGUI (Transform someGUI)
	{
		someGUI.GetComponent<SpriteRenderer> ().enabled = true;
		someGUI.GetComponent<Animator> ().enabled = true;
	}

	public static bool outOfBounds(Vector3 candidate) {
		return	candidate.x > gameBounds.x || candidate.y > gameBounds.y || 
				candidate.x < -gameBounds.x ||candidate.y < -gameBounds.y;

	}

}
