﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine;
using LitJson;
using UnityEngine.EventSystems;


public class LoginForm : MonoBehaviour {

    public InputField usernameField;
    public InputField passwordField;
    public Toggle rememberToggle;
	public Toggle isGameHost;
	public Button loginButton;
    public Slider progressBar;
    public Text progressBarText;
	GameObject networkManager { get { return GameObject.FindWithTag ("NetworkManager"); } }
	GameObject apiManager { get { return GameObject.FindWithTag ("APIManager"); } }

	void Start() {

		if (PlayerPrefs.GetInt("rememberMe") == 1) {
			rememberToggle.isOn = true;	
			LoadCredentials();
		} else {
			rememberToggle.isOn = false;
		}

		if (!PlayerPrefs.HasKey("IsHost"))
			PlayerPrefs.SetInt("IsHost", 0);

		isGameHost.isOn = PlayerPrefs.GetInt("IsHost") == 1;

		GameObject loginContainer = GameObject.FindGameObjectWithTag("LoginContainer");
		GameObject background = loginContainer.transform.Find("Background").gameObject;
		Image bgImage = background.GetComponentInChildren<Image>();
		bgImage.color = new Color(69f/255f, 44f/255f, 15f/255f, 1f); // brown
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			Selectable next = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable> ().FindSelectableOnDown ();

			if (next != null)
			{
				InputField field = next.GetComponent<InputField> ();
				if (field != null)
				{
					field.OnPointerClick (new PointerEventData (EventSystem.current));
				}
				EventSystem.current.SetSelectedGameObject (next.gameObject, new BaseEventData (EventSystem.current));
			}

		}

		if (Input.GetKeyDown(KeyCode.KeypadEnter)) {
			OnClickLogin ();
		}
	}

    void LoadCredentials() {
        if (PlayerPrefs.HasKey("username")) {
            usernameField.text = PlayerPrefs.GetString("username");
        }

        if (PlayerPrefs.HasKey("password")) {
            passwordField.text = PlayerPrefs.GetString("password");
        }
    }

    void SaveCredentials() {
        PlayerPrefs.SetString("username", usernameField.text);
        PlayerPrefs.SetString("password", passwordField.text);
		PlayerPrefs.SetInt ("rememberMe", 1);
    }

    public void OnClickLogin() {
        PlayerPrefs.SetInt("IsLocalPlayer", 0);

		if (isGameHost.isOn)
			PlayerPrefs.SetInt("IsHost", 1);
		else
			PlayerPrefs.SetInt("IsHost", 0);

		APIManager manager = apiManager.GetComponent<APIManager>();

		loginButton.interactable = false;

		if (progressBar == null)
			progressBar = transform.Find("ProgressBar").GetComponent<Slider>();

		if (progressBarText == null)
			progressBarText = transform.Find("ProgressBarText").GetComponent<Text>();

        progressBar.gameObject.SetActive(true);
        progressBarText.gameObject.SetActive(true);

        progressBar.value = 0;
        progressBarText.text = "Logging in...";

		StartCoroutine(manager.Login(usernameField.text, passwordField.text, (success) => {

            progressBarText.text = "Login successful!";

            if (success) {
				if (isGameHost.isOn)
					networkManager.GetComponent<CSNetworkManager>().StartHost();
				else
					networkManager.GetComponent<CSNetworkManager>().StartClient();
			} else {
				loginButton.interactable = true;
			}

			if (rememberToggle.isOn)
				SaveCredentials();
			
		}));
		
    }

	public void OnClickIsLocalPlayer() {
		PlayerPrefs.SetInt("IsLocalPlayer", 1);
		networkManager.GetComponent<CSNetworkManager>().StartHost();
	}

    public void OnClickRememberToggle(bool value) {
        if (value) {
            SaveCredentials();
        } else {
            PlayerPrefs.DeleteKey("username");
            PlayerPrefs.DeleteKey("password");
			PlayerPrefs.DeleteKey ("rememberMe");
        }
        LoadCredentials();
    }

    

	public IEnumerator GetTexture(string url, SpriteRenderer renderer) {

		Texture2D texture;
		texture = new Texture2D (1, 1);

		using (WWW www = new WWW(url))
		{
			yield return www;
			www.LoadImageIntoTexture (texture);
			Sprite sprite = Sprite.Create (texture, 
				new Rect(0, 0, texture.width, texture.height), 
				Vector2.one
			);
			renderer.sprite = sprite;
		}

	}


	
}
