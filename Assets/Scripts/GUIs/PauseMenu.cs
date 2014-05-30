using UnityEngine;
using System.Collections;

namespace GUIs {

	/// <summary>
	/// Pause menu. A stripped down and customised version of the script found here:
	/// http://wiki.unity3d.com/index.php?title=PauseMenu
	/// </summary>
	public class PauseMenu : MonoBehaviour
	{
		
		public GUISkin skin;

		private float startTime = 0.1f;
		
		public Material mat;
		
		private long tris = 0;
		private long verts = 0;
		private float savedTimeScale;
		
		private bool showfps;
		private bool showtris;
		private bool showvtx;
		
		public Color lowFPSColor = Color.red;
		public Color highFPSColor = Color.green;
		
		public int lowFPS = 30;
		public int highFPS = 50;
		
		public GameObject start;
		
		public Color statColor = Color.yellow;

		public int nativeWidth = 480;
		public int nativeHeight = 270;
		
		public string[] credits =
		{
			"University of Cape Town",
			"Daniel Burnham-King",
			"Jamie Hewland",
			"Matthew Wood",
			"Wesley Robinson"
		};
		public Texture[] crediticons;
		
		public enum Page {
			None,Main,Options,Credits,Quit,GameOver
		}

		public PlatformerCharacter2D character;

		private Page currentPage;
		
		private float[] fpsarray;
		private float fps;
		
		private int toolbarInt = 0;
		private string[]  toolbarstrings =  {"Settings","Stats","System"};

		// Scaling for GUI elements to account for screen res
		private float scaleX;
		private float scaleY;
		private GUIStyle fontScaleButton;
		private GUIStyle fontScaleToggle;
		private GUIStyle fontScaleLabel;

		public Gestures.GestureHandler gestureHandler;
		public EffectsPlayer effectsPlayer;
		public MusicPlayer musicPlayer;
		
		void Start() {
			fpsarray = new float[Screen.width];
			Time.timeScale = 1;
			PauseGame();

			Debug.Log("Screen width = " + Screen.width);
			Debug.Log("Screen height = " + Screen.height);
			scaleX = Screen.width / nativeWidth;
			scaleY = Screen.height / nativeHeight;
		}
		
		void ScrollFPS() {
			for (int x = 1; x < fpsarray.Length; ++x) {
				fpsarray[x-1]=fpsarray[x];
			}
			if (fps < 1000) {
				fpsarray[fpsarray.Length - 1]=fps;
			}
		}
		
		void LateUpdate () {
			if (showfps) {
				FPSUpdate();
			}
			
			if ((Input.GetKeyDown("escape") || Input.GetKeyDown(KeyCode.Escape)))
			{
				switch (currentPage) 
				{
				case Page.None: 
					PauseGame(); 
					break;
					
				case Page.Main: 
					if (!IsBeginning()) 
						UnPauseGame(); 
					break;
				
				case Page.GameOver:
					Application.LoadLevel ("Main");
					break;
					
				default: 
					currentPage = Page.Main;
					break;
				}
			}

			if (!IsBeginning () && character.Dead && !IsGamePaused()) 
			{
				PauseGame();
			}

		
		}

		/// <summary>
		/// Thanks for nothing, Unity. :'(
		/// </summary>
		void initFontScale()
		{
			if (fontScaleButton != null)
				return;
			fontScaleButton = new GUIStyle(GUI.skin.button);
			fontScaleButton.fontSize = (int)(16 * scaleX);
			fontScaleToggle = new GUIStyle(GUI.skin.toggle);
			fontScaleToggle.fontSize = (int)(16 * scaleX);
			fontScaleLabel = new GUIStyle(GUI.skin.label);
			fontScaleLabel.fontSize = (int)(16 * scaleX);
		}
		
		void OnGUI () {
			if (skin != null) {
				GUI.skin = skin;
			}
			initFontScale();
			ShowStatNums();
			if (IsGamePaused()) {
				GUI.color = statColor;
				switch (currentPage) {
				case Page.Main: MainPauseMenu(); break;
				case Page.Options: ShowToolbar(); break;
				case Page.Credits: ShowCredits(); break;
				case Page.Quit: ShowQuitConfirm(); break;
				case Page.GameOver: GameOverScreen(); break;
				}
			}   
		}
		
		void ShowToolbar() {
			BeginPage(300,300);
			toolbarInt = GUILayout.Toolbar (toolbarInt, toolbarstrings, fontScaleButton);
			switch (toolbarInt) {
			case 0: Settings(); break;
			case 1: StatControl(); break;
			case 2: ShowDevice(); break;
			}
			EndPage();
		}
		
		void ShowCredits() {
			BeginPage(300,300);
			foreach(string credit in credits) {
				GUILayout.Label(credit, fontScaleLabel);
			}
			foreach( Texture credit in crediticons) {
				GUILayout.Label(credit, fontScaleLabel);
			}
			EndPage();
		}
		
		void ShowBackButton() {
			float x = 20 * scaleX;
			float y = 20 * scaleY;
			float w = 50 * scaleX;
			float h = 20 * scaleY;
			if (GUI.Button(new Rect(x, y, w, h), "Back", fontScaleButton)) {
				currentPage = Page.Main;
			}
		}
		
		void GameOverScreen() {
			BeginPage(200,200);
			GUILayout.Label (string.Format("Game Over! You ran {0}m", (int)character.Distance), fontScaleLabel);
			if (GUILayout.Button ("Try Again" , fontScaleButton))
			{
				startTime =0f;
				Application.LoadLevel ("Main");
			}

			EndPage();
		}

		void ShowDevice() {
			GUILayout.Label("Unity player version "+Application.unityVersion, fontScaleLabel);
			GUILayout.Label("Graphics: "+SystemInfo.graphicsDeviceName+" "+
			                SystemInfo.graphicsMemorySize+"MB\n"+
			                SystemInfo.graphicsDeviceVersion+"\n"+
			                SystemInfo.graphicsDeviceVendor, fontScaleLabel);
			GUILayout.Label("Shadows: "+SystemInfo.supportsShadows, fontScaleLabel);
			GUILayout.Label("Image Effects: "+SystemInfo.supportsImageEffects, fontScaleLabel);
			GUILayout.Label("Render Textures: "+SystemInfo.supportsRenderTextures, fontScaleLabel);
		}

		void ShowQuitConfirm() {
			BeginPage(200,200);
			GUILayout.Label("Are you sure you want to quit?", fontScaleLabel);
			if (GUILayout.Button("Yes", fontScaleButton)) {
				Application.Quit();
			}
			if (GUILayout.Button("No", fontScaleButton)) {
				currentPage = Page.Main;
			}
			EndPage();
		}
		
		void Settings() {
			musicPlayer.Muted = !GUILayout.Toggle(!musicPlayer.Muted, "Game music", fontScaleToggle);
			effectsPlayer.muted = !GUILayout.Toggle(!effectsPlayer.muted, "Sound effects", fontScaleToggle);
			gestureHandler.inverted = GUILayout.Toggle(gestureHandler.inverted, "Invert controls", fontScaleToggle);
		}
		
		void StatControl() {
			GUILayout.BeginHorizontal();
			showfps = GUILayout.Toggle(showfps, "FPS", fontScaleToggle);
			showtris = GUILayout.Toggle(showtris, "Triangles", fontScaleToggle);
			showvtx = GUILayout.Toggle(showvtx, "Vertices", fontScaleToggle);
			GUILayout.EndHorizontal();
		}
		
		void FPSUpdate() {
			float delta = Time.smoothDeltaTime;
			if (!IsGamePaused() && delta !=0.0) {
				fps = 1 / delta;
			}
		}
		
		void ShowStatNums() {
			float x = Screen.width - 100 * scaleX;
			float y = 10 * scaleY;
			float w = 100 * scaleX;
			float h = 200 * scaleY;
			GUILayout.BeginArea(new Rect(x, y, w, h));
			if (showfps) {
				string fpsstring= fps.ToString ("#,##0 fps");
				GUI.color = Color.Lerp(lowFPSColor, highFPSColor,(fps-lowFPS)/(highFPS-lowFPS));
				GUILayout.Label (fpsstring, fontScaleLabel);
			}
			if (showtris || showvtx) {
				GetObjectStats();
				GUI.color = statColor;
			}
			if (showtris) {
				GUILayout.Label (tris+"tri", fontScaleLabel);
			}
			if (showvtx) {
				GUILayout.Label (verts+"vtx", fontScaleLabel);
			}
			GUILayout.EndArea();
		}
		
		void BeginPage(int width, int height) {
			float scaledWidth = width * scaleX;
			float scaledHeight = height * scaleY;
			GUILayout.BeginArea( new Rect((Screen.width - scaledWidth) / 2, (Screen.height - scaledHeight) / 2,
			                              scaledWidth, scaledHeight));
		}
		
		void EndPage() {
			GUILayout.EndArea();
			if (currentPage != Page.Main) {
				ShowBackButton();
			}
		}
		
		bool IsBeginning() {
			return (Time.time < startTime);
		}
		
		
		void MainPauseMenu() {
			BeginPage(200,200);
			if (GUILayout.Button (IsBeginning() ? "Play" : "Continue", fontScaleButton)) {
				UnPauseGame();				
			}
			if (GUILayout.Button ("Options", fontScaleButton)) {
				currentPage = Page.Options;
			}
			if (GUILayout.Button ("Credits", fontScaleButton)) {
				currentPage = Page.Credits;
			}
			// TODO: Restart option
			//if (!IsBeginning() && GUILayout.Button ("Restart")) {
				
			//}
			if (GUILayout.Button ("Quit", fontScaleButton)) {
				currentPage = Page.Quit;
			}
			EndPage();
		}
		
		void GetObjectStats() {
			verts = 0;
			tris = 0;
			GameObject[] ob = FindObjectsOfType(typeof(GameObject)) as GameObject[];
			foreach (GameObject obj in ob) {
				GetObjectStats(obj);
			}
		}
		
		void GetObjectStats(GameObject obj) {
			Component[] filters;
			filters = obj.GetComponentsInChildren<MeshFilter>();
			foreach( MeshFilter f  in filters )
			{
				tris += f.sharedMesh.triangles.Length/3;
				verts += f.sharedMesh.vertexCount;
			}
		}
		
		public void PauseGame() {
			savedTimeScale = Time.timeScale;
			Time.timeScale = 0;
			AudioListener.pause = true;
			if (!character.Dead) 
			{
				currentPage = Page.Main;						
			}
			else 
			{
				currentPage = Page.GameOver;
			}

		}
		
		void UnPauseGame() {
			Time.timeScale = savedTimeScale;
			AudioListener.pause = false;
			
			currentPage = Page.None;
			
			if (IsBeginning() && start != null) {
				start.SetActive(true);
			}
		}
		
		bool IsGamePaused() {
			return (Time.timeScale == 0);
		}
		
		void OnApplicationPause(bool pause) {
			if (IsGamePaused()) {
				AudioListener.pause = true;
			}
		}
	}

}
