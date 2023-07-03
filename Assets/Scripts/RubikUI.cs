using System;
using UnityEngine;
using UnityEngine.UI;
using Screen = UnityEngine.Device.Screen;

public class RubikUI : MonoBehaviour
{
	[SerializeField] RubikCube rubikCube;
	[SerializeField] Button solveButton;
	[SerializeField] Button scrambleButton;
	[SerializeField] Button muteButton;
	[SerializeField] Button menuButton;
	[SerializeField] Button exitButton;
	[SerializeField] GameObject menu;
	[SerializeField] Image soundImage;
	[SerializeField] Button fullScreenButton;
	[SerializeField] RubikDance dance;

	[Space]
	[SerializeField] Sprite muteSprite;
	[SerializeField] Sprite unmuteSprite;

	Resolution windowedResolution = new () { width = 1080, height = 720 };

	void Start()
    {
		solveButton.onClick.AddListener(Solve);
		scrambleButton.onClick.AddListener(Scramble);
		muteButton.onClick.AddListener(Mute);
		fullScreenButton.onClick.AddListener(FullScreen);
		menuButton.onClick.AddListener(() => menu.SetActive(!menu.activeSelf));
		exitButton.onClick.AddListener(Exit);


		AudioListener.volume = 0;
		menu.SetActive(false);
		// Screen.SetResolution(windowedResolution.width, windowedResolution.height, false);

	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
			menu.SetActive(!menu.activeSelf);
	}

	private void Exit() => Application.Quit();

	void Solve()
	{
		rubikCube.Solve();
		dance.StartDance();
	}
	void Scramble()
	{
		rubikCube.Scramble();
		dance.StartDance();
	}

	void Mute()
	{
		AudioListener.volume = AudioListener.volume == 0 ? 1 : 0;
		soundImage.sprite = AudioListener.volume == 0 ? muteSprite : unmuteSprite;
	}	

	void FullScreen()
	{
		if (UnityEngine.Screen.fullScreen)
		{
			// TO WINDOWED
			Screen.SetResolution(windowedResolution.width, windowedResolution.height, false); 
		}
		else
		{
			// TO FULLSCREEN
			windowedResolution = UnityEngine.Screen.resolutions[0];
			Resolution fullScreenResolution = Screen.currentResolution;
			Screen.SetResolution(fullScreenResolution.width, fullScreenResolution.height, true); 
		}
	}
}
