using UnityEngine;
using System.Collections;

/// <summary>
/// Sets combinations randomly, by custom input, or by saved input.
/// </summary>
public class CreateCombination : MonoBehaviour {
	
	#region Variables
	/// <summary>
	/// Reference to the SwapSprite script that should be attached to the same character as this script.
	/// </summary>
	[Tooltip("Plug in the character to which this is attached.")]
	public SwapSprites character;
	
	/// <summary>
	/// The number of elements on the sprite sheet of this character.
	/// </summary>
	[Tooltip("Number of elements on the sprite sheet of this character.")]
	public int numOfElements;
	
	/// <summary>
	/// The names of the sprite sheet elements.
	/// </summary>
	[Tooltip("Enter in the names of the sprite sheet elements. (Body part names)")]
	public string[] spriteSheetElements;
	
	/// <summary>
	/// Enter in the types of the elements. (Difference between sprite sheets). Example: "Fire", "Water", "Steel"
	/// </summary>
	[Tooltip("Enter in the types of the elements. (Difference between sprite sheets)")]
	public string[] types;
	
	/// <summary>
	/// The folder's name.
	/// </summary>
	[Tooltip("Enter in the name of the folder in which the sprites are contained.")]
	public string folder;
	
	/// <summary>
	/// The name of the sprite sheet.  Include spaces.  Do NOT include the number.
	/// </summary>
	[Tooltip("Enter in the name of the sprite sheet.  Include spaces, but do NOT include the number.)")]
	public string spriteSheetName;
	
	/// <summary>
	/// The integer value of the current combination.  Numbers are used to get the correct sprite sheet.
	/// NOTE: set to public for testing purposes.
	/// </summary>
	private int[] combo;
	
	/// <summary>
	/// Input in the inspector the custom combination.
	/// </summary>
	[Tooltip("Enter in the names of the types of the elements to create a custom combination")]
	public string[] customCombo;
	
	/// <summary>
	/// The current combination.
	/// NOTE: set to public for testing purposes.
	/// </summary>
	private string[] currentCombo;
	
	/// <summary>
	/// The saved combination.
	/// NOTE: set to public for testing purposes.
	/// </summary>
	private string[] savedCombo;
	
	/// <summary>
	///	Is there a saved combination?
	/// </summary>
	private bool isSaved = false;
	#endregion Variables
	
	#region Monobehaviour
	void Start(){
		combo = new int[numOfElements];
		customCombo = new string[numOfElements];
		currentCombo = new string[numOfElements];
		savedCombo = new string[numOfElements];

		// initiate all sprites
		string[] temp = new string[numOfElements];
		/*for(int i = 0; i < numOfElements; i++){
			temp[i] = types[3];
		}*/
		SetCurrentCombo(temp);
	}
	
	#region For Testing
	// The following is used for testing without creating buttons.
	public bool random = false;		// Activate random combo.
	public bool custom = false;		// Activate custom combo.
	public bool saveCurrent = false;// Activate save current combo.
	public bool useSaved = false;	// Activate use saved combo.
	
	void Update(){
		if(random){
			UseRandomCombo();
			random = false;
		}
		if(custom){
			UseCustomCombo();
			custom = false;
		}
		if(saveCurrent){
			SaveCurrentCombo();
			saveCurrent = false;
		}
		if(useSaved){
			UseSavedCombo();
			useSaved = false;
		}
	}


	#endregion For Testing
	#endregion Monobehaviour
	
	#region Public Functions
	
	/// <summary>
	/// Gets the current combination and sets it to 1 string.
	/// </summary>
	/// <returns>1 string of the whole current combination.</returns>
	public string GetCurrentComboString(){
		string tempCurCombo = "";
		for(int i = 0; i < numOfElements; i++){
			tempCurCombo = tempCurCombo + currentCombo[i] + " " + spriteSheetElements[i] + "\n";
		}
		return tempCurCombo;
	}
	
	/// <summary>
	/// Gets the current combination in its array form.
	/// </summary>
	/// <returns>The current combination array.</returns>
	public string[] GetCurrentComboArray(){
		return currentCombo;
	}
	
	/// <summary>
	/// Used to pass a custom combination to this script from an outside source.
	/// </summary>
	/// <param name="c">string[] Should match the types for this character.</param>
	public void GiveCustomCombo(string[] c){
		if(c.Length == customCombo.Length){
			customCombo = c;
		}else{
			Debug.Log ("Input is too large for " + this.gameObject.name + "'s combination array");
		}
	}
	
	/// <summary>
	/// Saves the current combination.
	/// </summary>
	public void SaveCurrentCombo(){
		for(int i = 0; i < numOfElements; i++){
			savedCombo[i] = currentCombo[i];
		}
		isSaved = true;
	}
	
	/// <summary>
	///	Creates a random combination and sets the current combination to it.  Also calls the method to swap sprites.
	/// </summary>
	public void UseRandomCombo(){
		string[] temp = new string[numOfElements];
		for(int i = 0; i < numOfElements; i++){
			int rand = Random.Range(0,types.Length);
			temp[i] = types[rand];
		}
		SetCurrentCombo(temp);
	}
	
	/// <summary>
	/// Sets the current combination to the current combination.  Also calls the method to swap sprites.
	/// </summary>
	public void UseCustomCombo(){
		SetCurrentCombo(customCombo);
	}
	
	/// <summary>
	/// Sets the current combintion to the saved combination.  Also calls the method to swap sprites.
	/// </summary>
	public void UseSavedCombo(){
		if(isSaved)
			SetCurrentCombo(savedCombo);
	}
	
	#endregion Public Functions
	
	#region Private Functions
	/// <summary>
	/// Sets the current combination to whatever combination is passed to it.
	/// </summary>
	/// <param name="c">string array of a combination.</param>
	public void SetCurrentCombo(string[] c){
		for(int i = 0; i < numOfElements; i++){
			currentCombo[i] = c[i];
		}
		SetCombo(currentCombo);
		character.LoadCombination(folder, spriteSheetName, combo, numOfElements);
	}
	
	/// <summary>
	/// Translates the combination that is palssed to it into integers.
	/// </summary>
	/// <param name="stringCombo">String combo.</param>
	public void SetCombo(string[] stringCombo){
		for(int i = 0; i < stringCombo.Length; i++){
			for(int j = 0; j < types.Length; j++){
				if(stringCombo[i] == types[j]){
					combo[i] = j;
					break;
				}
			}
		}
	}
	#endregion Private Functions
}