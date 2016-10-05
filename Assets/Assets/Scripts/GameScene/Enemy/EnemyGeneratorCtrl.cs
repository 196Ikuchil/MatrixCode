using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyGeneratorCtrl : MonoBehaviour {
    // 生まれてくる敵プレハブ
    public GameObject[] enemyPref;

    //敵のタグ
    const string WARG = "Warg";
    const string DRAGON = "Dragon";
    const string CYCLOPS = "Cyclops";
    const string GOLEM = "Golem";
    const string SKYDRAGON = "SkyDragon";
    const string SKELTON = "Skelton";
    const string ROBOT = "Robot";
    const string TROLL = "Troll";

    int unremoveEnemyNum;

    GameMasterScript gameMaster;
    GameObject enemyPrefab;

	// 敵を格納
	GameObject[] existEnemys;
	// アクティブの最大数
	public int maxEnemy = 1;

    //GAmemasterから受け取った値
    int enemyLV=1;
    string enemyTag = WARG;
    int createAmount=1;

	void Start()
	{
        gameMaster = FindObjectOfType<GameMasterScript>();
        //今回のえねみーを指定
        enemyTag=gameMaster.GetTag();
        enemyLV = gameMaster.GetLV();
        createAmount = gameMaster.GetKillAmount();

        switch (enemyTag) {
            case WARG:
                enemyPrefab = enemyPref[0];
                unremoveEnemyNum = 0;
                break;
            case DRAGON:
                enemyPrefab = enemyPref[1];
                unremoveEnemyNum = 1;
                break;
            case CYCLOPS:
                enemyPrefab = enemyPref[2];
                unremoveEnemyNum = 2;
                break;
            case GOLEM:
                enemyPrefab = enemyPref[3];
                unremoveEnemyNum = 3;
                break;
            case SKYDRAGON:
                enemyPrefab = enemyPref[4];
                unremoveEnemyNum = 4;
                break;
            case SKELTON:
                enemyPrefab = enemyPref[5];
                unremoveEnemyNum = 5;
                break;
            case ROBOT:
                enemyPrefab = enemyPref[6];
                unremoveEnemyNum = 6;
                break;
            case TROLL:
                enemyPrefab = enemyPref[7];
                unremoveEnemyNum = 7;
                break;
        }

		// 配列確保
		existEnemys = new GameObject[maxEnemy];
		// 周期的に実行したい場合はコルーチンを使うと簡単に実装できます。
		StartCoroutine(Exec());
        //いらない奴らは消す
       // RemovePref();
	}
    void RemovePref()
    {
        for(int i = 0; i < enemyPref.Length; i++)
        {
            if (i == unremoveEnemyNum) continue;
            enemyPref[i] = null;
        }
    }

	// 敵を作成します
	IEnumerator Exec()
	{
		while(true){ 
			Generate();
            
			yield return new WaitForSeconds( 3.0f );
		}
	}

	void Generate()
	{
		for(int enemyCount = 0; enemyCount < existEnemys.Length; ++ enemyCount)
		{
			if( existEnemys[enemyCount] == null && createAmount>0){
				// 敵作成
				existEnemys[enemyCount] = Instantiate(enemyPrefab,transform.position,transform.rotation) as GameObject;
                createAmount--;
				return;
			}
		}
	}
}
