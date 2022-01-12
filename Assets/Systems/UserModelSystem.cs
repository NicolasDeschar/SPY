using UnityEngine;
using FYFY;
using System.Collections.Generic;

public class UserModelSystem : FSystem {

	public static UserModelSystem instance;

	// Load family
	private Family learnerModel = FamilyManager.getFamily(new AllOfComponents(typeof(UserModel)));
	private Family editableScriptContainer_f = FamilyManager.getFamily(new AllOfComponents(typeof(UITypeContainer)), new AnyOfTags("ScriptConstructor"));


	// Learner
	private GameObject currentLearner;

	// Other
	bool debugSystem = true;
	bool testSolution = false;
	private GameObject editableContainer; // L'objet qui continent la liste d'instruction cr�er par l'utilisateur, contient un enfant des le d�but (la barre rouge)
	GameObject infoLevelGen;

	public UserModelSystem()
	{
		if(Application.isPlaying)
		{
			currentLearner = GameObject.Find("Learner");
			currentLearner.GetComponent<UserModel>().endLevel = false; // Le niveau n'est pas termin�
			stratTentative();
			editableContainer = editableScriptContainer_f.First(); // On r�cup�re le container d'action �ditable
			infoLevelGen = GameObject.Find("infoLevelGen"); // On r�cup�re le gameobject contenant les infos du niveau
			Debug.Log("nb enfant au d�but : " + editableContainer.transform.childCount);
		}
		instance = this;
	}

	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.
	protected override void onPause(int currentFrame) {
	}

	// Use this to update member variables when system resume.
	// Advice: avoid to update your families inside this function.
	protected override void onResume(int currentFrame){
	}

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
        //testModelPresence();
        if (!testSolution) {
			timerResolution();
		}

		// Si le niveau est termin�
		if (currentLearner.GetComponent<UserModel>().endLevel)
        {
			// On lance la maj du model
			endLevelMajModel();
		}
	}

	public void testModelPresence()
    {
		Debug.Log("testModelPresence taille : " + learnerModel.Count);
		foreach(GameObject model in learnerModel)
        {
			Debug.Log(model.GetComponent<UserModel>().learnerName);
		}

    }

	// Lorsque dans un niveau l'utilisateur appuie sur play, on calcule le temps mis pour construire la r�ponse de la tentative
	// on l'ajoute ensuite au totalLevelTime
	public void playLevelActivated()
	{
		if (debugSystem)
        {
			Debug.Log("play level activated : UserModelSysteme");
		}
		// Permet d'arr�ter de compter le temp passer � cr�er sa liste d'action
		testSolution = true;
		timerResolution();
		// Incr�mente une tentative
		addAttempt();

		currentLearner.GetComponent<UserModel>().totalLevelTime = currentLearner.GetComponent<UserModel>().timeStart;
		Debug.Log(currentLearner.GetComponent<UserModel>().totalLevelTime);


	}

	// Lorsqu'un niveau est charg�, on enregistre l'heure de d�but afin de calculer par la suite le temps de construction de la r�ponse au niveau
	private void stratTentative()
    {
		currentLearner.GetComponent<UserModel>().timeStart = 0;
	} 

	// Sert de timer lors de la r�sulition avant de lancer une tentative
	private void timerResolution()
    {
		currentLearner.GetComponent<UserModel>().timeStart = currentLearner.GetComponent<UserModel>().timeStart + Time.deltaTime;
	}

	// Imcr�mente de 1 le nombre de tentive de r�soudre le 
	private void addAttempt()
    {
		currentLearner.GetComponent<UserModel>().nbTry += 1;

	}

	// Effectue les mise � jour pour la mod�lisation de l'utilisation
	private void endLevelMajModel()
    {
		Debug.Log("end level");
		Debug.Log("nb enfant a la fin : " + editableContainer.transform.childCount);

		// On repasse tous de suite � la variable � false pour �viter des doubles calcules
		currentLearner.GetComponent<UserModel>().endLevel = false;
		// calcule le temps moyen pour avoir r�ussit le niveau
		currentLearner.GetComponent<UserModel>().meanLevelTime = currentLearner.GetComponent<UserModel>().totalLevelTime / currentLearner.GetComponent<UserModel>().nbTry;
		// On regarde la diff�rence du nombre d'action entre ce que � fait l'utilisateur et le minimum calcul� par le syst�me
		currentLearner.GetComponent<UserModel>().difNbAction = (editableContainer.transform.childCount - 1) - infoLevelGen.GetComponent<infoLevelGenerator>().nbActionMin;
		// On calcule si on consid�re que l'on peux augmenter ou non (et de combien) la balanceWinFail de l'uilisateur
		float point = 0;
		//Si 1 tentative = 2 points, 2 ou 3 = 1 point si plus de 10 tentative = -1 point, sinon 0 point
		if(currentLearner.GetComponent<UserModel>().nbTry == 1)
        {
			point = 2;
        }
		else if (currentLearner.GetComponent<UserModel>().nbTry == 2 || currentLearner.GetComponent<UserModel>().nbTry == 2)
        {
			point = 1;
		}
		else if (currentLearner.GetComponent<UserModel>().nbTry >= 10)
        {
			point = -1;
		}
        // Si le nombre d'�crat d'action et au moins de +20% alors -0.5 point
        if (currentLearner.GetComponent<UserModel>().difNbAction >= (infoLevelGen.GetComponent<infoLevelGenerator>().nbActionMin / 5))
        {
			point = point - 0.5f;
        }
		//si le temps est beaucoup trop long (20s par block minim) alors -0.5 point
		if (currentLearner.GetComponent<UserModel>().meanLevelTime > (20 * infoLevelGen.GetComponent<infoLevelGenerator>().nbActionMin)){
			point = point - 0.5f;
		}

		// on met � jour la balance pour la/les comp�tences test�es
		// Si le vecteur comp�tence n'est pas encore pr�sent on l'ajoutedans le suivis de la balance ET dans le dictionnaire de comp�tence
		if (!currentLearner.GetComponent<UserModel>().balanceFailWin.ContainsKey(infoLevelGen.GetComponent<infoLevelGenerator>().vectorCompetence))
		{
			Debug.Log("Ajout vector");
			if(point < 0)
            {
				point = 0;
            }
			currentLearner.GetComponent<UserModel>().balanceFailWin.Add(infoLevelGen.GetComponent<infoLevelGenerator>().vectorCompetence, point);
			currentLearner.GetComponent<UserModel>().learningState.Add(infoLevelGen.GetComponent<infoLevelGenerator>().vectorCompetence, false); // la comp�tence n'est pas aprice donc false
		}
		else // sinon on met juste � jour la valeur en faisant attention de ne pas avoir de valeur n�gative
		{
			if(currentLearner.GetComponent<UserModel>().balanceFailWin[infoLevelGen.GetComponent<infoLevelGenerator>().vectorCompetence] + point < 0)
            {
				currentLearner.GetComponent<UserModel>().balanceFailWin[infoLevelGen.GetComponent<infoLevelGenerator>().vectorCompetence] = 0;
			}
            else
            {
				currentLearner.GetComponent<UserModel>().balanceFailWin[infoLevelGen.GetComponent<infoLevelGenerator>().vectorCompetence] += point;
			}
		}
		currentLearner.GetComponent<UserModel>().balanceFailWin[infoLevelGen.GetComponent<infoLevelGenerator>().vectorCompetence] += point;
		// On valide une comp�tence (ou un ensemble de comp�tence) lorsque le r�sultat de la balance est � au moins 4
		if (currentLearner.GetComponent<UserModel>().balanceFailWin[infoLevelGen.GetComponent<infoLevelGenerator>().vectorCompetence] >= 4)
        {
			currentLearner.GetComponent<UserModel>().learningState.Add(infoLevelGen.GetComponent<infoLevelGenerator>().vectorCompetence, true);
		}

		// Envoie trace fin de niveau

		/////   A FAIRE    ////
	}
}