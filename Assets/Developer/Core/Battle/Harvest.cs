using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Players;

public class Harvest : MonoBehaviour
{
    public static GameObject harvester;
    static int totalHarvestPoints;
    void Start()
    {
        harvester = GameObject.Find("Harvester");
        harvester.SetActive(false);
    }
    public static Enemy2 enemyToHarvest;
    public static Enemy2 EnemyToHarvest
    {
        get {
            return enemyToHarvest;
        } set
        {
            if (value == null)
            {
                harvester.SetActive(false);
                healTarget = null;
            }  else
            {
                harvester.SetActive(true);
                GameObject.Find("Heroes").BroadcastMessage("LookForHarvest", SendMessageOptions.RequireReceiver);
            }
            enemyToHarvest = value;
        }
    }
    public static Ally2 healTarget;
    //DECIDE NOW WILL THIS EVER BE AN INTERFACE
    //we can put this on the enemy script?
    public void HarvestEnemy()
    {
        if(EnemyToHarvest != null)
        {
            StartHarvest();
        }
    }
    void StartHarvest()
    {
        int harvestPoints = -Mathf.Abs(EnemyToHarvest.maxHP);
        totalHarvestPoints += harvestPoints;
        healTarget.StartHarvest(harvestPoints);
        Destroy(EnemyToHarvest.gameObject);
        EnemyToHarvest = null;
    }
    /// You can't cut off heads it's a kids game for god's sake..or is it?
    /// ADULTS DONT PLAY GAMES BEHEADING CUTE ANIMALS 
    /// BATTLING OK, BEHEADING NOT OK
    /// call it whatever but dont start beheading animations

    ///Director: The player obviously wins the first battle
    ///Ally: What should we do?
    ///Director:    The player can choose to behead/harvest, spare or bury
    ///             Burying is gonna be boring but Mr Beast does boring things all the time
    ///When the player spares, the game goes on
    ///You can't bury without a shovel. Burying is not an option straight away
    ///When the player harvests just restore health
    ///
    ///This requires three buttons
    ///How will I spawn the dead bird at the end of a battle
    ///Can you harvest during battle? This breaks the noise
    ///
    ///Options
    ///A
    ///Hide a harvest button on the battle menu (requires this class)
    ///Then you can harvest this class as long as you are in position
    ///sparing closes the options

    ///Inner Me: I like it. Just make sure that you show the animals and not a weird energy thing
}
