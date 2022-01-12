using UnityEngine;
using System.Collections.Generic;

public class UserModel : MonoBehaviour {
	// Advice: FYFY component aims to contain only public members (according to Entity-Component-System paradigm).

	// Matice permettant de connaittre l'etat d'un apprenant, permet de voir aussi son �tat d'apprentissage lors que l'on m�lange plusieurs comp�tences ensemble
	// Le premier niveau du dictionnaire concerne la comp�tence que dont on souhaite obtenir les informations
	// Le dictionnaire du deuxi�me niveau contient en clef la liste des comp�tence mse ensemble lors d'une g�n�ration de niveau
	// Actuellement on repr�sente la listes de comp�tence comme ceci : [S�quence, Boucle, If...Then, N�gation, Console]
	// Attention � bien respecter l'ordre de cr�ation de la s�quence dans l'odre
	// Exemple d'utilisation -> S�quence + N�gation + Console = [1, 0, 0, 1, 1]
	public Dictionary<List<bool>, bool> learningState = new Dictionary<List<bool>, bool>();
	// M�me utilisation que learningState pour connaitre le nombre de fois ou l'apprenant � r�ussit
	// On incr�mente ou d�cr�mente selon les crit�re de r�ussite d'un niveau
	// Arriver � un certain nombre � d�finir, on valide la (ou le m�lange) de comp�tence dans learningState
	public Dictionary<List<bool>, float> balanceFailWin = new  Dictionary<List<bool>, float>();
	// Nom de l'apprenant
	public string learnerName;
	// Permet de connaitre ou en est l'aprenant dans son apprentissage
	public List<bool> stepLearning = new List<bool>();

	//Les variables suivante servent lors du calcule de connaissance de la comp�ence en fin de niveau

	// Temps (moyen) que l'apprenant � pass� a construire son chemin avant de lancer la tentative de r�solution
	public float meanLevelTime;
	// D�but timer pour calculer le temps de la r�solution du niveau
	public float timeStart;
	// Enregistre le temps total passer � construire la r�ponse au niveau (toutes les tentative sont aditionn�es)
	public float totalLevelTime;
	// Dif entre le minimum d'action � faire glisser et le nombre d'action utiliser par l'apprenant
	public int difNbAction;
	// Nombre de tentative fait pour finir le niveau
	public int nbTry;
	// D�fini si le joueur � atteind la fin du niveau
	public bool endLevel;

}