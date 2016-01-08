using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {

	public GameObject desertEagle;
	public GameObject hammerHead;
	public GameObject fireFox;
    public GameObject polarBear;
	public int wave = 1;

	private int enemiesThisWave;
	public static int totalEnemiesSpawned;
	public static int enemiesDefeaten;
	public int enemiesToDefeat;
	
	public Text waveText;
	public Text enemiesToDefeatText;

	public float timeTillNextWave = 10f;
    public float CountDownTimerValue;
	private float minTimeBetweenWaves = 45f;
	private float maxTimeBetweenWaves = 90f;

	public Text timeTillNextWaveText;

	private float changeToSpawnDesertEagle = 1/3;
	private float changeToSpawnHammerHead = 1/3;
	private float changeToSpawnFireFox = 1/3;
    private float changeToSpawnPolarBear = 1/3;

	private float[] changeToSpawnByLevel = new float[5];
	private float mu = 0f;
	private float sigma = 1f;


    Score score_;
    public Canvas canvas;
    public GameObject EnemyHealthBars;


    public void FirstLoad () {
		wave = 1;
		waveText.text = "Current wave: 1";
		enemiesToDefeat = 0;
		enemiesDefeaten = 0;
		enemiesToDefeatText.text = "Enemies to defeat this wave: " + enemiesToDefeat;
		timeTillNextWaveText.text = "Time till next wave: " + timeTillNextWave;
		setEnemiesThisWave ();
		calculateLevelToSpawn ();
	}

    public void Awake()
    {
        score_ = Camera.main.GetComponent<Score>();
    }

	void Update () {
		waveText.text = "Current wave: " + wave;
		if(Time.timeSinceLevelLoad > timeTillNextWave){
			setEnemiesThisWave();
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

	Vector3 getRandomPosition(){
        return new Vector3(Random.Range(-100, 100), 1, Random.Range(-100, 100)) ;
	}

	void nextWave(){
        calculateChangeToSpawnFireFox();
        calculateChangeToSpawnHammerHead();
        calculateChangeToSpawnDesertEagle();
        calculateChangeToSpawnPolarBear();
        waveText.text = "Current wave: " + wave;
		enemiesToDefeat = 0;
		for(int i = 0; i < enemiesThisWave; i++){
			float random = Random.Range (0.0f, changeToSpawnHammerHead + changeToSpawnDesertEagle + changeToSpawnFireFox + changeToSpawnPolarBear);
			if(random <= changeToSpawnHammerHead){
				GameObject waterEnemyClone = hammerHead;
				waterEnemyClone.GetComponent<EnemyController>().setLevel(getLevelToSpawn());
				Instantiate (waterEnemyClone, getRandomPosition(), Quaternion.identity);
			}
			else if(random <= changeToSpawnHammerHead + changeToSpawnDesertEagle){
				GameObject windEnemyClone = desertEagle;
				windEnemyClone.GetComponent<EnemyController>().setLevel (getLevelToSpawn());
				windEnemyClone.transform.Translate(new Vector3(0f, 5f, 0f));
				Instantiate (windEnemyClone, getRandomPosition(), Quaternion.identity);
			}
            else if(random <= changeToSpawnHammerHead + changeToSpawnDesertEagle + changeToSpawnPolarBear)
            {
                GameObject polarBearClone = polarBear;
                polarBearClone.GetComponent<EnemyController>().setLevel(getLevelToSpawn());
                Instantiate(polarBearClone, getRandomPosition(), Quaternion.identity);
            }
			else{
				GameObject earthEnemyClone = fireFox;
				earthEnemyClone.GetComponent<EnemyController>().setLevel (getLevelToSpawn());
				Instantiate (earthEnemyClone, getRandomPosition(), Quaternion.identity);
			}
		}
        wave++;
	}



    // starts wave from save
    public void savewave()
    {
        var Temp = MonsterCollection.MonsterLoad("Assets/saves/monsters.xml");
        var MonsterList = Temp.getMonsterlist();


        for (int i = 0; i < MonsterList.Length; i++)
        {
            Vector3 location = new Vector3(MonsterList[i].location_x, MonsterList[i].location_y, MonsterList[i].location_z);
            Quaternion rotation = new Quaternion(MonsterList[i].rotation_w, MonsterList[i].rotation_x, MonsterList[i].rotation_y, MonsterList[i].rotation_z);
       
            switch (MonsterList[i].type)
            {
                case "Earth":
                    {
                        GameObject earthEnemyClone = fireFox;
                        var monster = earthEnemyClone.GetComponent<EnemyController>();
                        monster.setLevel(MonsterList[i].level);
                        monster.setHealth(MonsterList[i].health);
                        monster.setmaxhealth(MonsterList[i].maxHealth);
                        monster.setAttackPower(MonsterList[i].attackPower);
                        monster.setWalkingSpeed(MonsterList[i].walkingSpeed);
                        monster.setPoisoned(MonsterList[i].isPoisoned);                 

                        Instantiate(earthEnemyClone, location, rotation);
                        break;
                    }

                case "Wind":
                    {
                        GameObject windEnemyClone = desertEagle;
                        var monster = windEnemyClone.GetComponent<EnemyController>();
                        monster.setLevel(MonsterList[i].level);
                        monster.setHealth(MonsterList[i].health);
                        monster.setmaxhealth(MonsterList[i].maxHealth);
                        monster.setAttackPower(MonsterList[i].attackPower);
                        monster.setWalkingSpeed(MonsterList[i].walkingSpeed);
                        monster.setPoisoned(MonsterList[i].isPoisoned);

                        Instantiate(windEnemyClone, location, rotation);
                        break;
                    }
                
                case "Water":
                    {
                        GameObject waterEnemyClone = hammerHead;
                        var monster = waterEnemyClone.GetComponent<EnemyController>();
                        monster.setLevel(MonsterList[i].level);
                        monster.setHealth(MonsterList[i].health);
                        monster.setmaxhealth(MonsterList[i].maxHealth);
                        monster.setAttackPower(MonsterList[i].attackPower);
                        monster.setWalkingSpeed(MonsterList[i].walkingSpeed);
                        monster.setPoisoned(MonsterList[i].isPoisoned);

                        Instantiate(waterEnemyClone, location, rotation);
                        break;
                    }


            }

        }
    }

    void calculateChangeToSpawnDesertEagle(){
		int tempType = PlayerAttacker.currentWeapon.getType().getType ();
        changeToSpawnDesertEagle = 0;
        changeToSpawnDesertEagle += 1f / 2f * Analytics.getBuildings()[1];
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

    void calculateChangeToSpawnPolarBear()
    {
        changeToSpawnPolarBear = 0.125f * (changeToSpawnDesertEagle + changeToSpawnFireFox + changeToSpawnHammerHead);
    }

	void setEnemiesThisWave(){
        enemiesThisWave = Random.Range(3 + wave, 5 + wave);
	}

	void calculateLevelToSpawn(){
		for (int i = 0; i < 5; i++) {
			changeToSpawnByLevel[i] = (1f / (sigma * Mathf.Sqrt (2f * Mathf.PI))) * Mathf.Exp(-0.5f * Mathf.Pow((((float)i - mu) / sigma), 2));
		}
	}

	int getLevelToSpawn(){
		float totalChange = 0;
		for(int i = 0; i < 5; i ++){
			totalChange += changeToSpawnByLevel[i];
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
