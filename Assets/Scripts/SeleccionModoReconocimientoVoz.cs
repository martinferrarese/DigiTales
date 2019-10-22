﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SeleccionModoReconocimientoVoz : MonoBehaviour {

	public void Dropdown_IndexChange(int indice)
    {
		PlayerPrefs.SetString ("ModoReconocimiento", indice.ToString());
		PlayerPrefs.Save ();

		if(PlayerPrefs.GetString ("ModoReconocimiento").Equals("0"))
			Debug.Log("Modo: Tiempo Real");
		else
			Debug.Log("Modo: Palabras Clave");
    }

	void Start(){
		if (PlayerPrefs.GetString ("ModoReconocimiento").Equals ("0")) {
			GameObject.Find ("DropdownReconocimiento").GetComponent<TMP_Dropdown> ().value = 0;

		} else {
			GameObject.Find ("DropdownReconocimiento").GetComponent<TMP_Dropdown> ().value = 1;
		}
	}

	void Update(){
		if (PlayerPrefs.GetString ("ModoReconocimiento").Equals ("0")) {
			GameObject.Find ("Icono-TipoRelato-TiempoReal").GetComponent<Image> ().enabled = true;
			GameObject.Find ("Icono-TipoRelato-PalabrasClave").GetComponent<Image> ().enabled = false;

		} else {
			GameObject.Find ("Icono-TipoRelato-TiempoReal").GetComponent<Image> ().enabled = false;
			GameObject.Find ("Icono-TipoRelato-PalabrasClave").GetComponent<Image> ().enabled = true;
		}
	}
}
