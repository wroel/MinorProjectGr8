using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {

	public GameObject desertEagle;
	public GameObject hammerHead;
	public GameObject fireFox;
    public GameObject polarBear;
    public GameObject meepMeep;
    public GameObject oilPhant;
	public int wave = 0;

	private int enemiesThisWave;
	public static int totalEnemiesSpawned;
	public static int enemiesDefeaten;
	public int enemiesToDefeat;
	
	public Text waveText;
	public Text enemiesToDefeatText;

	public float timeTillNextWave = 10f;
    public float CountDownTimerValue;
	private float minTimeBetweenWaves = 75f;
	private float maxTimeBetweenWaves = 120f;

	public Text timeTillNextWaveText;

	private float changeToSpawnDesertEagle = 1/3;
	private float changeToSpawnHammerHead = 1/3;
	private float changeToSpawnFireFox = 1/3;
    private float changeToSpawnPolarBear = 1/3;
    private float changeToSpawnMeepMeep = 0;
    private float changeToSpawnOilphant = 0;

	private float[] changeToSpawnByLevel = new float[5];
	private float mu = 0f;
	private float sigma = 1f;

    public List<EnemyController> unbuffedEnemies = new List<EnemyController>();

    Score score_;
    public Canvas canvas;
    public GameObject EnemyHealthBars;

	/// <summary>
	/// Called when a new game is started. Sets fields to default.
	/// </summary>
    public void FirstLoad () {
		wave = 0;
		waveText.text = "Current wave: 0";
		enemiesToDefeat = 0;
		enemiesDefeaten = 0;
		enemiesToDefeatText.text = "Enemies to defeat this wave: " + enemiesToDefeat;
		timeTillNextWaveText.text = "Time till next wave: " + timeTillNextWave;
		setEnemiesThisWave ();
		calculateLevelToSpawn ();
	}

	/// <summary>
	/// When awoken, gets the score;
	/// </summary>
    public void Awake(){
        score_ = Camera.main.GetComponent<Score>();
    }

	/// <summary>
	/// Updates a few fields and UI elements every frame. Like time till next wave, spawns enemies when next wave begins and updates
	/// the current wave and enemies to defeat text.
	/// </summary>
	void Update () {
		waveText.text = "Current wave: " + wave;
		if(Time.timeSinceLevelLoad > timeTillNextWave){
			timeTillNextWave = Time.timeSinceLevelLoad + Random.Range (minTimeBetweenWaves, maxTimeBetweenWaves);
			totalEnemiesSpawned += enemiesThisWave;
			enemiesToDefeatText.text = "Enemies to defeat: " + enemiesToDefeat;
            nextWave();
            // Add scores the beginning of the wave
            score_.addScoreWave(wave);
            mu += 0.25f;
			calculateLevelToSpawn ();
		}
        CountDownTimerValue = timeTillNextWave - Time.timeSinceLevelLoad; 

        enemiesToDefeatText.text = "Enemies to defeat: " + enemiesToDefeat;
		timeTillNextWaveText.text = "Time till next wave: " + (int)(CountDownTimerValue);
        enemiesToDefeat = MiniMapScript.enemies.Count;
		enemiesToDefeatText.text = "Enemies to defeat: " + enemiesToDefeat;
	}

	/// <summary>
	/// Gets a random position for a enemy to spawn in a square of 200 x 200 in the middle.
	/// </summary>
	/// <returns>The random position.</returns>
	Vector3 getRandomPosition(){
        return new Vector3(Random.Range(-100, 100), 0, Random.Range(-100, 100)) ;
	}

	/// <summary>
	/// First calculates the amount of enemies, the change to spawn each enemy and ech level and than spawns them accordingly.
	/// </summary>
	void nextWave(){
		wave++;
		setEnemiesThisWave();
        Camera.main.GetComponent<PSpawner>().nextWave();
        calculateChangeToSpawnFireFox();
        calculateChangeToSpawnHammerHead();
        calculateChangeToSpawnDesertEagle();
        calculateChangeToSpawnPolarBear();
        calculateChangeToSpawnMeepMeep();
        calculateChangeToSpawnOilphant();
        waveText.text = "Current wave: " + wave;
		for(int i = 0; i < enemiesThisWave; i++){
			float random = Random.Range (0.0f, changeToSpawnHammerHead + changeToSpawnDesertEagle + changeToSpawnFireFox + changeToSpawnPolarBear + changeToSpawnMeepMeep + changeToSpawnOilphant);
			if(random <= changeToSpawnHammerHead){
				GameObject waterEnemyClone = hammerHead;
				waterEnemyClone.GetComponent<EnemyController>().setLevel(getLevelToSpawn());
				Instantiate (waterEnemyClone, getRandomPosition(), Quaternion.identity);
                unbuffedEnemies.Add(waterEnemyClone.GetComponent<EnemyController>());
			}
			else if(random <= changeToSpawnHammerHead + changeToSpawnDesertEagle){
				GameObject windEnemyClone = desertEagle;
				windEnemyClone.GetComponent<EnemyController>().setLevel (getLevelToSpawn());
				Instantiate (windEnemyClone, getRandomPosition(), Quaternion.identity);
                unbuffedEnemies.Add(windEnemyClone.GetComponent<EnemyController>());
            }
            else if(random <= changeToSpawnHammerHead + changeToSpawnDesertEagle + changeToSpawnPolarBear)
            {
                GameObject polarBearClone = polarBear;
                polarBearClone.GetComponent<EnemyController>().setLevel(getLevelToSpawn());
                Instantiate(polarBearClone, getRandomPosition(), Quaternion.identity);
                unbuffedEnemies.Add(polarBearClone.GetComponent<EnemyController>());
            }
            else if (random <= changeToSpawnHammerHead + changeToSpawnDesertEagle + changeToSpawnPolarBear + changeToSpawnMeepMeep)
            {
                GameObject meepMeepClone = meepMeep;
                meepMeepClone.GetComponent<EnemyController>().setLevel(getLevelToSpawn());
                meepMeepClone.transform.Rotate(0f, 180f, 0f);
                Instantiate(meepMeepClone, getRandomPosition(), Quaternion.identity);
            }
            else if (random <= changeToSpawnHammerHead + changeToSpawnDesertEagle + changeToSpawnPolarBear + changeToSpawnMeepMeep + changeToSpawnOilphant)
            {
                GameObject oilphantClone = oilPhant;
                oilphantClone.GetComponent<EnemyController>().setLevel(getLevelToSpawn());
                Instantiate(oilphantClone, getRandomPosition(), Quaternion.identity);
            }
            else {
				GameObject earthEnemyClone = fireFox;
				earthEnemyClone.GetComponent<EnemyController>().setLevel (getLevelToSpawn());
				Instantiate (earthEnemyClone, getRandomPosition(), Quaternion.identity);
                unbuffedEnemies.Add(earthEnemyClone.GetComponent<EnemyController>());
            }
		}
	}



    /// <summary>
    /// Spawns enemies that were saved.
    /// </summary>
    /// <param name="path">Path.</param>
    public void savewave(string path){
        var Temp = MonsterCollection.MonsterLoad(path);
        var MonsterList = Temp.getMonsterlist();

		for (int i = 0; i < MonsterList.Length; i++){
            Vector3 location = new Vector3(MonsterList[i].location_x, MonsterList[i].location_y, MonsterList[i].location_z);
            Quaternion rotation = new Quaternion(MonsterList[i].rotation_w, MonsterList[i].rotation_x, MonsterList[i].rotation_y, MonsterList[i].rotation_z);
       
            switch (MonsterList[i].name){
                case "FireFox":
				{
                        GameObject earthEnemyClone = fireFox;
                        var monster = earthEnemyClone.GetComponent<EnemyController>();
                        monster.setLevel(MonsterList[i].level);
                        monster.setHealthFirstTime(MonsterList[i].health);
                        monster.setmaxhealth(MonsterList[i].maxHealth);
                        monster.setAttackPower(MonsterList[i].attackPower);
                        monster.setWalkingSpeed(MonsterList[i].walkingSpeed);
                        monster.setPoisoned(MonsterList[i].isPoisoned);
                        unbuffedEnemies.Add(earthEnemyClone.GetComponent<EnemyController>());
                        Instantiate(earthEnemyClone, location, rotation);
                        break;
                    }

                case "DesertEagle":
				{
                        GameObject windEnemyClone = desertEagle;
                        var monster = windEnemyClone.GetComponent<EnemyController>();
                        monster.setLevel(MonsterList[i].level);
                        monster.setHealthFirstTime(MonsterList[i].health);
                        monster.setmaxhealth(MonsterList[i].maxHealth);
                        monster.setAttackPower(MonsterList[i].attackPower);
                        monster.setWalkingSpeed(MonsterList[i].walkingSpeed);
                        monster.setPoisoned(MonsterList[i].isPoisoned);
                        unbuffedEnemies.Add(windEnemyClone.GetComponent<EnemyController>());
                        Instantiate(windEnemyClone, location, rotation);
                        break;
                    }
                
                case "HammerHead":
                    {
                        GameObject waterEnemyClone = hammerHead;
                        var monster = waterEnemyClone.GetComponent<EnemyController>();
                        monster.setLevel(MonsterList[i].level);
                        monster.setHealthFirstTime(MonsterList[i].health);
                        monster.setmaxhealth(MonsterList[i].maxHealth);
                        monster.setAttackPower(MonsterList[i].attackPower);
                        monster.setWalkingSpeed(MonsterList[i].walkingSpeed);
                        monster.setPoisoned(MonsterList[i].isPoisoned);
                        unbuffedEnemies.Add(waterEnemyClone.GetComponent<EnemyController>());
                        Instantiate(waterEnemyClone, location, rotation);
                        break;
                    }

                case "Oilphant":
                    {
                        GameObject oilphantClone = oilPhant;
                        var monster = oilphantClone.GetComponent<EnemyController>();
                        monster.setLevel(MonsterList[i].level);
                        monster.setHealthFirstTime(MonsterList[i].health);
                        monster.setmaxhealth(MonsterList[i].maxHealth);
                        monster.setAttackPower(MonsterList[i].attackPower);
                        monster.setWalkingSpeed(MonsterList[i].walkingSpeed);
                        monster.setPoisoned(MonsterList[i].isPoisoned);
                        unbuffedEnemies.Add(oilphantClone.GetComponent<EnemyController>());
                        Instantiate(oilphantClone, location, rotation);
                        break;
                    }

                case "MeepMeep":
                    {
                        GameObject meepMeepClone = meepMeep;
                        var monster = meepMeepClone.GetComponent<EnemyController>();
                        monster.setLevel(MonsterList[i].level);
                        monster.setHealthFirstTime(MonsterList[i].health);
                        monster.setmaxhealth(MonsterList[i].maxHealth);
                        monster.setAttackPower(MonsterList[i].attackPower);
                        monster.setWalkingSpeed(MonsterList[i].walkingSpeed);
                        monster.setPoisoned(MonsterList[i].isPoisoned);
                        unbuffedEnemies.Add(meepMeepClone.GetComponent<EnemyController>());
                        Instantiate(meepMeepClone, location, rotation);
                        break;
                    }

                case "PolarBear":
                    {
                        GameObject polarBearClone = polarBear;
                        var monster = polarBearClone.GetComponent<EnemyController>();
                        monster.setLevel(MonsterList[i].level);
                        monster.setHealthFirstTime(MonsterList[i].health);
                        monster.setmaxhealth(MonsterList[i].maxHealth);
                        monster.setAttackPower(MonsterList[i].attackPower);
                        monster.setWalkingSpeed(MonsterList[i].walkingSpeed);
                        monster.setPoisoned(MonsterList[i].isPoisoned);
                        unbuffedEnemies.Add(polarBearClone.GetComponent<EnemyController>());
                        Instantiate(polarBearClone, location, rotation);
                        break;
                    }
            }
 	     }
    }

	/// <summary>
	/// Calculates the change to spawn a desert eagle. It gets higher if the current weapon is weak to the desert Eagle and
	/// if a buildings that are weak to the desert Eagle are build.
	/// </summary>
    void calculateChangeToSpawnDesertEagle(){
		int tempType = PlayerAttacker.currentWeapon.getType().getType ();
        changeToSpawnDesertEagle = 0;
		changeToSpawnDesertEagle += (1f / 2f) * Analytics.getBuildings()[1];
        if (tempType == 1 || tempType == 0){
			changeToSpawnDesertEagle += 1f/3f;
		}
		if(tempType == 2){
			changeToSpawnDesertEagle += 1f/2f;
		}
		if(tempType == 3){
			changeToSpawnDesertEagle += 1f/3f;
		}
    }

	/// <summary>
	/// Calculates the change to spawn a hammerHead. It gets higher if the current weapon is weak to the hammerHead and
	/// if a buildings that are weak to the hammerHead are build.
	/// </summary>
	void calculateChangeToSpawnHammerHead(){
		int tempType = PlayerAttacker.currentWeapon.getType().getType ();
        changeToSpawnHammerHead = 0;
        changeToSpawnHammerHead += Analytics.getPlayerUpgrades()[1] * 1f / 4f;
        changeToSpawnHammerHead += 1f / 2f * Analytics.getBuildings()[2];

        if (tempType == 1){
			changeToSpawnHammerHead += 1f/6f;
		}
		if(tempType == 2 || tempType == 0){
			changeToSpawnHammerHead += 1f/3f;
		}
		if(tempType == 3){
			changeToSpawnHammerHead += 1f/2f;
		}
    }

	/// <summary>
	/// Calculates the change to spawn a fireFox. It gets higher if the current weapon is weak to the fireFox and
	/// if a buildings that are weak to the fireFox are build.
	/// </summary>
	void calculateChangeToSpawnFireFox(){
		int tempType = PlayerAttacker.currentWeapon.getType().getType ();
        changeToSpawnFireFox = 0;
        changeToSpawnFireFox += Analytics.getPlayerUpgrades()[0] * 1f / 4f;
        changeToSpawnFireFox += 1f / 2f * Analytics.getBuildings()[3];
        if (tempType == 1){
			changeToSpawnFireFox += 1f/2f;
		}
		if(tempType == 2){
			changeToSpawnFireFox += 1f/6f;
		}
		if(tempType == 3 || tempType == 0){
			changeToSpawnFireFox += 1f/3f;
		}
	}

	/// <summary>
	/// calculate the change to spawn a polarBear. it's always 6.25%
	/// </summary>
    void calculateChangeToSpawnPolarBear(){
        changeToSpawnPolarBear = 0.0625f * (changeToSpawnDesertEagle + changeToSpawnFireFox + changeToSpawnHammerHead + changeToSpawnMeepMeep + changeToSpawnOilphant);
    }

	/// <summary>
	/// calculate the change to spawn a meepMeep. it's always 3.125%
	/// </summary>
    void calculateChangeToSpawnMeepMeep(){
        changeToSpawnMeepMeep = 0.03125f * (changeToSpawnDesertEagle + changeToSpawnFireFox + changeToSpawnHammerHead + changeToSpawnPolarBear + changeToSpawnOilphant);
    }

	/// <summary>
	/// calculate the change to spawn a oilPhant. it's always 6.25%
	/// </summary>
    void calculateChangeToSpawnOilphant(){
        changeToSpawnOilphant = 0.0625f * (changeToSpawnDesertEagle + changeToSpawnFireFox + changeToSpawnHammerHead + changeToSpawnPolarBear + changeToSpawnMeepMeep);
    }

	/// <summary>
	/// calculates the amount of enemies that have to be spawned the next wave. It gets higher if the wave is higher.
	/// </summary>
    void setEnemiesThisWave(){
		enemiesThisWave = (int)Mathf.Ceil(25f * (2f / Mathf.PI) * Mathf.Atan ((float)wave / 12f));
		// Debug.Log (enemiesThisWave);
	}

	/// <summary>
	/// Gives changes to spawn for every level according to the normal distribution. With every wave the maximum of 
	/// the graph moves more to level 5.
	/// </summary>
	void calculateLevelToSpawn(){
		for (int i = 0; i < 5; i++) {
			changeToSpawnByLevel[i] = (1f / (sigma * Mathf.Sqrt (2f * Mathf.PI))) * Mathf.Exp(-0.5f * Mathf.Pow((((float)i - mu) / sigma), 2));
		}
	}

	/// <summary>
	/// Returns a level to give to an enemy according to the current normal distribution
	/// </summary>
	/// <returns>The level to spawn.</returns>
	int getLevelToSpawn(){
		float totalChange = 0;
		for (int i = 0; i < 5; i++) {
			totalChange += changeToSpawnByLevel [i];
		}
		float random = Random.Range (0f, totalChange);
		if (random <= changeToSpawnByLevel [0]) {
			return 1;
		} else if (random <= changeToSpawnByLevel [0] + changeToSpawnByLevel [1]) {
			return 2;
		} else if (random <= changeToSpawnByLevel [0] + changeToSpawnByLevel [1] + changeToSpawnByLevel [2]) {
			return 3;
		} else if (random <= changeToSpawnByLevel [0] + changeToSpawnByLevel [1] + changeToSpawnByLevel [2] + changeToSpawnByLevel [3]) {
			return 4;
		} else {
			return 5;
		}
	}
}