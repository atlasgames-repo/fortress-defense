using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class enemies : MonoBehaviour
{
    int enemyNum;
    public TextMeshProUGUI enemyNameB;
    public TextMeshProUGUI enemySpeed;
    public TextMeshProUGUI enemyDamage;
    public TextMeshProUGUI enemyPower;
    public TextMeshProUGUI enemyType;
    public TextMeshProUGUI enemyHealth;
    public buttonTag enemiesScr;
    public enemyClasses guideInfo;
    public Image EnemyProf;
    public GameObject contents;

    public void enemyName()
    {
        //each button has uniqe tag which is attatched to itself as an script. each button gets a new teg as its being cloned and tags will
        //stay there forever since they're scripts. pressing each button will use its own tag to set a picture and related information of an enemy.
        if(GlobalValue.LevelPass >= guideInfo.enemiesInfo[enemiesScr.nTag].levelUnlocked)
        {
            enemyNameB.text = guideInfo.enemiesInfo[enemiesScr.nTag].name;
            enemySpeed.text = guideInfo.enemiesInfo[enemiesScr.nTag].speed.ToString();
            enemyDamage.text = guideInfo.enemiesInfo[enemiesScr.nTag].damage.ToString();
            enemyType.text = guideInfo.enemiesInfo[enemiesScr.nTag].type.ToString();
            enemyHealth.text = guideInfo.enemiesInfo[enemiesScr.nTag].health.ToString();
            enemyPower.text = guideInfo.enemiesInfo[enemiesScr.nTag].power.ToString();
            EnemyProf.sprite = guideInfo.enemiesInfo[enemiesScr.nTag].EnemyProfile;

            guideBook.lockIconStatic.SetActive(false);
            guideBook.coverStatic.SetActive(true);
        }
        else
        {
            enemyNameB.text = guideInfo.enemiesInfo[guideInfo.enemiesInfo.Length - 1].name;
            enemySpeed.text = guideInfo.enemiesInfo[guideInfo.enemiesInfo.Length - 1].speed.ToString();
            enemyDamage.text = guideInfo.enemiesInfo[guideInfo.enemiesInfo.Length - 1].damage.ToString();
            enemyType.text = guideInfo.enemiesInfo[guideInfo.enemiesInfo.Length - 1].type.ToString();
            enemyHealth.text = guideInfo.enemiesInfo[guideInfo.enemiesInfo.Length - 1].health.ToString();
            enemyPower.text = guideInfo.enemiesInfo[guideInfo.enemiesInfo.Length - 1].power.ToString();
            EnemyProf.sprite = guideInfo.enemiesInfo[guideInfo.enemiesInfo.Length - 1].EnemyProfile;
            
            guideBook.lockIconStatic.SetActive(true);
            guideBook.coverStatic.SetActive(false);

            Debug.Log("ggg");
        }

        SoundManager.PlaySfx(SoundManager.Instance.soundClick);
    }
}
