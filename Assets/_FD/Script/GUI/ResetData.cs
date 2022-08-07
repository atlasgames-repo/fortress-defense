using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ResetData : MonoBehaviour {
	SoundManager soundManager;
	public bool ResetRemoveAd = false;

	void Start(){
		soundManager = FindObjectOfType<SoundManager> ();
	}

	public void Reset(){

		//bool isRemoveAd = GlobalValue.RemoveAds;

		//PlayerPrefs.DeleteAll ();

		//GlobalValue.RemoveAds = ResetRemoveAd ? false : isRemoveAd;
		SoundManager.PlaySfx (soundManager.soundClick);

		//if (DefaultValue.Instance)
		//	FindObjectOfType<DefaultValue> ().ResetDefaultValue ();
        GlobalValue.isNewGame = false;
        GlobalValue.LevelPass = 0;

		//if(ShopMenuPopupUI.instance)
		//	Destroy (ShopMenuPopupUI.instance.gameObject);
	}

	public void ResetGame(){
		SoundManager.Click ();

		bool isRemoveAd = GlobalValue.RemoveAds;

		PlayerPrefs.DeleteAll ();

        //if (CharacterHolder.Instance)
        //    CharacterHolder.Instance.UpdateUnlockCharacter();


        GlobalValue.RemoveAds = ResetRemoveAd ? false : isRemoveAd;

		//if (DefaultValue.Instance)
		//	FindObjectOfType<DefaultValue> ().ResetDefaultValue ();

//		Reset();

		SceneManager.LoadSceneAsync (SceneManager.GetActiveScene ().buildIndex);


	}
}
