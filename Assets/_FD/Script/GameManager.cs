/// <summary>
/// Game manager. 
/// Handle all the actions, parameter of the game
/// You can easy get the state of the game with the IListener script.
/// </summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager: MonoBehaviour {
	public static GameManager Instance{ get; private set;}
    public bool isWatchingAd { get; set; }


    public enum GameState{Menu,Playing, GameOver, Success, Pause};
	public GameState State{ get; set; }
    
    [ReadOnly] public int levelStarGot;
    public LayerMask layerGround, layerEnemy, layerPlayer;
    [HideInInspector]
	public List<IListener> listeners;

    //add listener called by late actived object
    public void AddListener(IListener _listener)
    {
        if (!listeners.Contains(_listener))     //check if this added or not
            listeners.Add(_listener);
    }
    //remove listener when Die or Disable
    public void RemoveListener(IListener _listener)
    {
        if (listeners.Contains(_listener))      //check if this added or not
            listeners.Remove(_listener);
    }

    #region EXP
    [ReadOnly] public int currentExp = 9999;
    
    public void AddExp(int _amount, Transform instigator)
    {
        currentExp += _amount;
        //FloatingTextManager.Instance.ShowText(_amount + "XP", new Vector2(0, 0.5f), Color.blue, instigator.position);
    }

    #endregion
    void Awake(){
		Instance = this;
		State = GameState.Menu;
		listeners = new List<IListener> ();

        //if (testLevel)
        //    Debug.LogError("TESTING LEVEL");
        //else
        //Instantiate(gameLevels[GlobalValue.levelPlaying - 1], Vector2.zero, Quaternion.identity);
	}

	IEnumerator Start(){
		yield return new WaitForEndOfFrame ();

        //menuManager = FindObjectOfType<MenuManager> ();

        //soundManager = FindObjectOfType<SoundManager> ();
        //		if (AdsController.instance)
        //			AdsController.instance.HideBanner ();

        if (GameLevelSetup.Instance)
            currentExp = GameLevelSetup.Instance.GetCurrentLevelExp();

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.R))
        {
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        //UNLOCK ALL
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.U))
        {
            bool isRemoveAd = GlobalValue.RemoveAds;

            PlayerPrefs.DeleteAll();
            GlobalValue.RemoveAds = isRemoveAd;

            GlobalValue.LevelPass = 9999;
            SceneManager.LoadScene(0);
        }
    }
    
    //called by MenuManager
    public void StartGame(){
		State = GameState.Playing;

		//Get all objects that have IListener

		var listener_ = FindObjectsOfType<MonoBehaviour>().OfType<IListener>();
		foreach (var _listener in listener_) {
			listeners.Add (_listener);
		}

		foreach (var item in listeners) {
			item.IPlay ();
		}
	}

	public void Gamepause(){
		State = GameState.Pause;
		foreach (var item in listeners)
			item.IPause ();
	}

	public void UnPause(){
		State = GameState.Playing;
		foreach (var item in listeners)
			item.IUnPause ();
	}

	public void Victory(){
        //Debug.LogError("Victory");

        if (State == GameState.Success)
            return;

        

        Time.timeScale = 1;
        SoundManager.Instance.PauseMusic(true);
        State = GameState.Success;

        if (AdsManager.Instance)
            AdsManager.Instance.ShowNormalAd(State);

        foreach (var item in listeners)
        {
            if (item != null) 
                item.ISuccess();
        }
        
        //save level and save star
        if (GlobalValue.levelPlaying > GlobalValue.LevelPass)
            GlobalValue.LevelPass = GlobalValue.levelPlaying;

        //if (point >= levelmanager.instance.point3stars)
        //    globalvalue.levelstar(scenemanager.getactivescene().name, 3);
        //else if (point >= levelmanager.instance.point2stars)
        //    globalvalue.levelstar(scenemanager.getactivescene().name, 2);
        //else
        //    globalvalue.LevelStar(SceneManager.GetActiveScene().name, 1);
        if (State == GameState.Success)
            GlobalValue.LevelStar(GlobalValue.levelPlaying, levelStarGot);
    }
   
    private void OnDisable()
    {
        
        
    }

    //IEnumerator GameFinishCo()
    //{
    //yield return new WaitForSeconds(.5f);
    //MenuManager.Instance.Gamefinish();
    //SoundManager.PlaySfx(SoundManager.Instance.soundGamefinish, 0.5f);

    //save coins and points
    //GlobalValue.SavedCoins = Coin;

    //save level and save star
    //GlobalValue.UnlockLevel(SceneManager.GetActiveScene().name);


    //if (Point >= LevelManager.Instance.point3Stars)
    //    GlobalValue.LevelStar(SceneManager.GetActiveScene().name, 3);
    //else if (Point >= LevelManager.Instance.point2Stars)
    //    GlobalValue.LevelStar(SceneManager.GetActiveScene().name, 2);
    //else
    //    GlobalValue.LevelStar(SceneManager.GetActiveScene().name, 1);
    //}

    public void UnlockNextLevel()
    {
        //if (GlobalValue.levelPlaying > GlobalValue.LevelPass)
        //    GlobalValue.LevelPass = GlobalValue.levelPlaying;
    }

    public void GameOver(){
       

        Time.timeScale = 1;
        SoundManager.Instance.PauseMusic(true);
        //Debug.LogError("GameOver");
        if (State == GameState.GameOver)
            return;
		
		State = GameState.GameOver;

        if (AdsManager.Instance)
            AdsManager.Instance.ShowNormalAd(GameState.GameOver);

        //Debug.LogError("CALL");

        foreach (var item in listeners)
			item.IGameOver ();
	}

    [HideInInspector]
    public List<GameObject> enemyAlives;
    [HideInInspector]
    public List<GameObject> listEnemyChasingPlayer;

    public void RigisterEnemy(GameObject obj)
    {
        enemyAlives.Add(obj);
    }

    public void RemoveEnemy(GameObject obj)
    {
        enemyAlives.Remove(obj);
        //		Debug.LogError ("ENEMY LEFT: " + enemyAlives.Count);
    }

    public int EnemyAlive()
    {
        return enemyAlives.Count;
    }
}
