using UnityEngine;
using System.Collections;

public class NotEnoughCoins : MonoBehaviour {
	public static NotEnoughCoins Instance;
	public GameObject Panel;

	void Awake(){
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		Panel.SetActive (false);
	}

	public void ShowUp(){
		Panel.SetActive (true);
	}

	public void Close(){
		Panel.SetActive (false);
	}
}
