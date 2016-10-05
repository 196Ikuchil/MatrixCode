using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

/// <summary>
/// シーン遷移時のフェードイン・アウトを制御するためのクラス .
/// </summary>
public class FadeManager : MonoBehaviour
{

	#region Singleton

	private static FadeManager instance;

	public static FadeManager Instance {
		get {
			if (instance == null) {
				instance = (FadeManager)FindObjectOfType (typeof(FadeManager));
				
				if (instance == null) {
					Debug.LogError (typeof(FadeManager) + "is nothing");
				}
			}
			
			return instance;
		}
	}

	#endregion Singleton

	/// <summary>フェード中の透明度</summary>
	private float fadeAlpha = 0;
	/// <summary>フェード中かどうか</summary>
	private bool isFading = false;
	/// <summary>フェード色</summary>
	public Color fadeColor = Color.black;


	public void Awake ()
	{
		if (this != Instance) {
			Destroy (this.gameObject);
			return;
		}
		
		DontDestroyOnLoad (this.gameObject);
	}

	public void OnGUI ()
	{
	
		// Fade .
		if (this.isFading) {
			//色と透明度を更新して白テクスチャを描画 .
			this.fadeColor.a = this.fadeAlpha;
			GUI.color = this.fadeColor;
			GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
		}		
		
		
		
	}

	/// <summary>
	/// 画面遷移 .
	/// </summary>
	/// <param name='scene'>シーン名</param>
	/// <param name='interval'>暗転にかかる時間(秒)</param>
	public void LoadLevel (string scene, float interval)
	{
		StartCoroutine (TransScene (scene, interval));
	}

	/// <summary>
	/// シーン遷移用コルーチン .
	/// </summary>
	/// <param name='scene'>シーン名</param>
	/// <param name='interval'>暗転にかかる時間(秒)</param>
	private IEnumerator TransScene (string scene, float interval)
	{
        UnityEngine.EventSystems.EventSystem e = FindObjectOfType<UnityEngine.EventSystems.EventSystem>();
        e.enabled = false;
		//だんだん暗く .
		this.isFading = true;
		float time = 0;
		while (time <= interval) {
			this.fadeAlpha = Mathf.Lerp (0f, 1f, time / interval);      
			time += Time.deltaTime;
			yield return 0;
		}

        //シーン切替 .
        //Application.LoadLevel (scene);
        SceneManager.LoadScene(scene);

		//だんだん明るく .
		time = 0;
		while (time <= (interval-0.8f)) {
			this.fadeAlpha = Mathf.Lerp (1f, 0f, time / (interval-0.8f));
			time += Time.deltaTime;
			yield return 0;
		}
        this.fadeAlpha = 1f;


        this.isFading = false;
	}
}

