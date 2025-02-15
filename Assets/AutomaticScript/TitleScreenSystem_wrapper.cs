using UnityEngine;
using FYFY;

[ExecuteInEditMode]
public class TitleScreenSystem_wrapper : MonoBehaviour
{
	private void Start()
	{
		this.hideFlags = HideFlags.HideInInspector; // Hide this component in Inspector
	}

	public void showCampagneMenu()
	{
		MainLoop.callAppropriateSystemMethod ("TitleScreenSystem", "showCampagneMenu", null);
	}

	public void genLvlProcedural()
	{
		MainLoop.callAppropriateSystemMethod ("TitleScreenSystem", "genLvlProcedural", null);
	}

	public void loadSceneForProceduralGeneration()
	{
		MainLoop.callAppropriateSystemMethod ("TitleScreenSystem", "loadSceneForProceduralGeneration", null);
	}

	public void backFromCampagneMenu()
	{
		MainLoop.callAppropriateSystemMethod ("TitleScreenSystem", "backFromCampagneMenu", null);
	}

	public void quitGame()
	{
		MainLoop.callAppropriateSystemMethod ("TitleScreenSystem", "quitGame", null);
	}

}
