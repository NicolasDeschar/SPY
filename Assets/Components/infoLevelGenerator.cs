using UnityEngine;
using System.Collections.Generic;

public class infoLevelGenerator : MonoBehaviour {
	// Advice: FYFY component aims to contain only public members (according to Entity-Component-System paradigm).

	// Nombre d'action minimum pour finir le niveau
	public int nbActionMin;
	// Les comp�tence en test
	public List<bool> vectorCompetence = new List<bool>();
	// Difficult� du niveau
	public int hardLevel;
	// D�fini si le niveau est fini d'�tre cr�er pour l'envoie des traces
	public bool sendPara;
	// D�fini si un nouveau level � �t� lanc�
	public bool newLevelGen;
	// Pr�cise si des options on �t� choisis
	public bool optionOk;
}