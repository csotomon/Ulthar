/*
 * Script para volver la camara un pixel perfect de acuerdo a la resolución
*/

using UnityEngine;
using System.Collections;


public class PixelPerfect : MonoBehaviour {

	//Valor configurado al importar las imagenes, por defecto es 100
	public float pixelsPerUnit=100f;

	//Zoom a aplicar para la camara
	public float zoom = 1;

	void Start () {
		Camera.main.orthographicSize = Screen.height / 2f / pixelsPerUnit / zoom;
	}
}

