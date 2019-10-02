﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using KKSpeech;
using UnityEngine.SceneManagement;

public class ControlarCuento1Escena6 : MonoBehaviour {

	public Button startRecordingButton;

	public Text sceneText; //texto propio de la escena
	public Text resultTextSpeech; //texto reconocido por voz

	public Text resultErrores; //visualizar error

	//variables para trabajar sceneText
	private string textoEscena = string.Empty; 
	private string[] palabrasEscena = null; 

	//variables para trabajar result(reconocimiento parcial de voz)
	private string[] palabrasSpeech = null;
	int cantPalabrasSpeech = 0;

	//variables para efectoParallax
	int efectoParallax = 0;
	public float parallaxSpeed = 0.12f;

	//variables de sonidos
	public AudioClip lobo;
	private AudioSource ambienteBosque;

	public GameObject player; //objeto para controlar animacion de personaje
	public GameObject bosque; //objeto para controlar escena
	public GameObject tronco; //objeto para controlar tronco

	int i=0;
	int n=0;

	public Animator circuloNegro;
	public Animator microfono;
	private Animator troncoEfecto;

	public GameObject contenedor;

	bool coroutineStarted = true;//para freezar ejecucion
	string coroutineStarted1 = string.Empty;//para freezar contenedor
	bool coroutineStarted2 = true;

	string modoRelato = string.Empty; 
	string modoVibracion = string.Empty; 

	int cambiarTexto = 0;

	bool textoCompleto = false;

    void Start() { 
		Screen.orientation = ScreenOrientation.Landscape;
		modoRelato = PlayerPrefs.GetString ("ModoReconocimiento");
		modoVibracion = PlayerPrefs.GetString ("ModoVibracion");

		//if (SpeechRecognizer.ExistsOnDevice()) {
			SpeechRecognizerListener listener = GameObject.FindObjectOfType<SpeechRecognizerListener>();
			//listener.onAuthorizationStatusFetched.AddListener(OnAuthorizationStatusFetched);
			//listener.onAvailabilityChanged.AddListener(OnAvailabilityChange);
			listener.onErrorDuringRecording.AddListener(OnError);
			//listener.onErrorOnStartRecording.AddListener(OnError);
			listener.onFinalResults.AddListener(OnFinalResult);
			listener.onPartialResults.AddListener(OnPartialResult);
			//listener.onEndOfSpeech.AddListener(OnEndOfSpeech);
			//startRecordingButton.enabled = false;
			//SpeechRecognizer.RequestAccess();

			//obtengo cantidad de palabras de escena actual
		textoEscena = sceneText.text = "el terror la paraliza";
			palabrasEscena = textoEscena.Split(' ');

			//para q se reproduzca mas rapido, es sonido ya esta asignado
			ambienteBosque = GetComponent<AudioSource> ();						
		ambienteBosque.clip = lobo;
			

			//iniciar objetos
			player.SetActive(true);
			bosque.SetActive(true);
			//player.SendMessage ("UpdateState", "PlayerRun");
			player.gameObject.GetComponent<Animator>().Play("PlayerIdle");
			efectoParallax = 0;

		//} else {			
			//resultErrores.text = "Sorry, but this device doesn't support speech recognition";
			//startRecordingButton.enabled = false;
		//}

		ActivarEscucha ();



	}

	/*RESULTADO FINAL DEL RECONOCIMIENTO DE VOZ*/
	public void OnFinalResult(string result) {		
		ReiniciarValoresEscena();
	}

	/*RESULTADO PARCIAL DEL RECONOCIMIENTO DE VOZ*/
	public void OnPartialResult(string result) {

		//obtengo cantidad de palabras de reconocimiento parcial de voz
		palabrasSpeech = result.ToLower().Split(' ');
		cantPalabrasSpeech = palabrasSpeech.Length;
		//resultErrores.text = result.ToLower() + " " + cantPalabrasSpeech + palabrasSpeech [0].ToString ().Trim() + " ";

		if (modoRelato == "0")
		{
////////////////////////////////////////////*COLOREO DE ORACION DE LA ESCENA*//*PALABRA-POR-PALABRA*////////////////////////////////////////////
			for (i = n; i < cantPalabrasSpeech; i++)
			{
				if (string.Equals (palabrasSpeech [i].ToString ().Trim(), palabrasEscena [i].ToString ().Trim()) && n == i)
				{
					//activar animacion segun palabra
					switch (palabrasSpeech [i].ToString ().Trim())
					{	
					case "terror":							
						
						PintarPalabra (palabrasSpeech [i].ToString ());
						ambienteBosque.Play ();			
						break;
					case "paraliza":							
						textoCompleto = true;
						DesactivarEscucha ();
						PintarPalabra (palabrasSpeech [i].ToString ());
						coroutineStarted1 = "pero con gran coraje";//para freezar contenedor				
						break;
				
					case "coraje":							
						textoCompleto = true;
						DesactivarEscucha ();
						PintarPalabra (palabrasSpeech [i].ToString ());
						coroutineStarted1 = "logra saltar el enorme tronco";//para freezar contenedor		

						break;
					case "saltar":							
						PintarPalabra (palabrasSpeech [i].ToString ());
						player.gameObject.GetComponent<Animator> ().Play ("PlayerJump");
						tronco.gameObject.GetComponent<Animator> ().enabled =true;
						tronco.gameObject.GetComponent<Animator>().Play("MoverTronco");
						efectoParallax = 1;			
						break;

					case "tronco":							
						textoCompleto = true;
						DesactivarEscucha ();
						coroutineStarted = false;//para freezar ejecucion
						PintarPalabra (palabrasSpeech [i].ToString ());				
						break;

						default:	
						PintarPalabra (palabrasSpeech [i].ToString ());
							break;
					}

					//resultTextSpeech.text = resultTextSpeech.text + palabrasSpeech [i].ToString () + " "; //coloreo
					//n++; //para no tener en cuenta palabra coloreada en el bucle

					break;
				}			
			}
		}
		else
		{
////////////////////////////////////////////*COLOREO DE ORACION DE LA ESCENA*//*POR-PALABRA-CLAVE*////////////////////////////////////////////
			//activar animacion segun palabra
			switch (palabrasSpeech [cantPalabrasSpeech-1].ToString ().Trim())
			{
			case "terror":
				if(Pintar ("terror", 0))
				{
					ambienteBosque.clip = lobo;	
				}
				break;
			case "paraliza":	
				if(Pintar ("paraliza", 1))
				{
					textoCompleto = true;
					DesactivarEscucha ();

					coroutineStarted1 = "pero con gran coraje";//para freezar contenedor	
				}
				break;	
			case "coraje":
				if(Pintar ("coraje", 0))
				{
					textoCompleto = true;
					DesactivarEscucha ();

					coroutineStarted1 = "logra saltar el enorme tronco";//para freezar contenedor	
				}
				break;
			case "saltar":					
				if(Pintar ("saltar", 0))
				{
					player.gameObject.GetComponent<Animator>().Play("PlayerJump");
					tronco.gameObject.GetComponent<Animator> ().enabled =true;

					tronco.gameObject.GetComponent<Animator>().Play("MoverTronco");
					efectoParallax = 1;	

				}
				break;

			case "tronco":					
				if(Pintar ("tronco", 1))
				{
					textoCompleto = true;
					DesactivarEscucha ();
					coroutineStarted = false;//para freezar ejecucion

				}
				break;

				default:					
					break;
			}		
		}

////////////////////////////////////////////*COLOREO DE ORACION DE LA ESCENA*//*POR-PALABRA-CLAVE(PSEUDO-REAL-TIME)*////////////////////////////////////////////					
			//activar animacion segun palabra
		/*switch (palabrasSpeech [cantPalabrasSpeech-1].ToString ().Trim())
		{
			case "adrenalina":	
				StartCoroutine(UsingYield());
				StopCoroutine ("UsingYield");
				break;
			case "varios":	
				StartCoroutine(UsingYield2());
				StopCoroutine ("UsingYield2");
				break;
			case "ágilmente":
				StartCoroutine(UsingYield3());
				StopCoroutine ("UsingYield3");
				coroutineStarted = false;
				break;

			default:					
				break;
		}*/
	}

	/*public void OnAvailabilityChange(bool available) {
		startRecordingButton.enabled = available;
		if (!available) {
			resultErrores.text = "Speech Recognition not available";
		} else {
			//resultErrores.text = "Say something :-)";
		}
	}*/

	/*public void OnAuthorizationStatusFetched(AuthorizationStatus status) {
		switch (status) {
		case AuthorizationStatus.Authorized:
			startRecordingButton.enabled = true;
			break;
		default:
			startRecordingButton.enabled = false;
			resultErrores.text = "Cannot use Speech Recognition, authorization status is " + status;
			break;
		}
	}*/

	/*public void OnEndOfSpeech() {
		startRecordingButton.GetComponentInChildren<Text>().text = "";
	}*/

public void OnError(string error) {
	//Debug.LogError(error);
	//resultErrores.text = "Something went wrong... Try again! \n [" + error + "]";
	//startRecordingButton.GetComponentInChildren<Text>().text = "";

	DesactivarEscucha();
}

public void OnStartRecordingPressed() {
	if (SpeechRecognizer.IsRecording()) {
		DesactivarEscucha ();
	} else {			
		ActivarEscucha ();
	}
}

public void PintarPalabra(string palabra)
{
	resultTextSpeech.text = resultTextSpeech.text + palabra + " "; //coloreo
	n++; //para no tener en cuenta palabra coloreada en el bucle
}

public void CambiarTexto(string textoNuevo)
{
	contenedor.SetActive (false);	
	i = 0;
	n = 0;
	textoEscena = sceneText.text = textoNuevo;
	palabrasEscena = textoEscena.Split (' ');

	contenedor.SetActive (true);//llama a otro contenedor de texto
	resultTextSpeech.text = string.Empty;//borra lo escuchado luego de llamar al otro contenedor
	//OnStartRecordingPressed ();//activa escucha

	textoCompleto = false;
	ActivarEscucha();
}

	bool Pintar(string palabraClave, int nroPalabraClave)
	{
		if (n == nroPalabraClave) {	
			n++;
			while (!string.Equals (palabrasEscena [i].ToString (), palabraClave)) {
				resultTextSpeech.text = resultTextSpeech.text + palabrasEscena [i].ToString () + " "; //coloreo
				i++;					
			}
			resultTextSpeech.text = resultTextSpeech.text + palabrasEscena [i].ToString () + " "; //coloreo
			i++;
			return true;
		} else
			return false;
	}  

public void ReiniciarValoresEscena() {	
	if(!textoCompleto)
	{
		resultTextSpeech.text = string.Empty;

		i=0;
		n=0;

		startRecordingButton.gameObject.SetActive(true);
		microfono.gameObject.SetActive(false);



	}
}

	// Update is called once per frame
	void Update()
	{
		if (efectoParallax == 1)
		{
			float finalSpeed= parallaxSpeed * Time.deltaTime;
			RawImage bosqueImagen = bosque.GetComponent<RawImage> ();				
			bosqueImagen.uvRect = new Rect(bosqueImagen.uvRect.x + finalSpeed , 0f, 1f, 1f);


		} 


		if (!coroutineStarted)
			StartCoroutine (EsperarSegundos (3));

	if (!string.IsNullOrEmpty(coroutineStarted1))			
		StartCoroutine (RetrasarContenedor (1, coroutineStarted1));

		
		
	}  

/*IEnumerator EfectoTemblor()
{	
	//coroutineStarted2 = true;

	//troncoEfecto.gameObject.SetActive (true);
	//yield return new WaitForSeconds(0.2f);

	RectTransform bosqueImagen = bosque.GetComponent<RectTransform> ();
	Vector3 myVector = new Vector3(bosqueImagen.position.x+0.5f, bosqueImagen.position.y, bosqueImagen.position.z);
	Vector3 myVector2 = new Vector3(bosqueImagen.position.x, bosqueImagen.position.y, bosqueImagen.position.z);
	for (int z = 0; z < 2; z++)
	{		
		bosqueImagen.position = myVector;
		yield return new WaitForSeconds(0.1f);
		bosqueImagen.position = myVector2;
		yield return new WaitForSeconds(0.1f);
	}

}*/

	IEnumerator EsperarSegundos(int seconds)
	{
		coroutineStarted = true;
		yield return new WaitForSeconds(seconds);

		StartCoroutine (SpriteShapeOut());
		StopCoroutine ("SpriteShapeOut");

		SceneManager.LoadScene("NewMenu");
	}

	IEnumerator SpriteShapeOut()
	{		
		circuloNegro.SetTrigger ("end");
		yield return new WaitForSeconds(1f);
	}

IEnumerator RetrasarContenedor(int seconds, string frase)
{		
	coroutineStarted1 = string.Empty;
	yield return new WaitForSeconds(seconds);

	CambiarTexto(frase);
}

public void ActivarEscucha() {	
	startRecordingButton.gameObject.SetActive(false);
	microfono.gameObject.SetActive(true);
	SpeechRecognizer.StartRecording(true);
}

public void DesactivarEscucha() {	
	SpeechRecognizer.StopIfRecording ();
	startRecordingButton.gameObject.SetActive(true);
	microfono.gameObject.SetActive(false);
}

public void Vibrar(){
	Handheld.Vibrate ();
}
}