using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WallSelector))]
public class WallSelectorEditor : Editor {
	public override void OnInspectorGUI (){
		if(Application.isPlaying)
			return;
		
        WallSelector myScript = (WallSelector)target;	
		GUILayout.Label(myScript.scriptName);
        //if(myScript.editObj) DrawDefaultInspector();
		DrawDefaultInspector();
		if(myScript.selectedStyle >= 0 && myScript.selectedStyle < myScript.selector.Length){
			//GUILayout.Box("Selected Prefab: " + myScript.selector[myScript.selectedStyle].selectionName);
			if(myScript.editObj)
				for(int i= 0; i < myScript.selector.Length; i++){
					if(i == myScript.selectedStyle) GUI.color = new Color(0f,1f,0f);
					else GUI.color = new Color(1f,1f,1f);
					//GUILayout.Box(i + " " + myScript.selector[i].selectionName);
					if(GUILayout.Button(i + " " + myScript.selector[i].selectionName)){
						myScript.SelectStyle(i);
					}
				}
			
			if(myScript.selectorNames != null && myScript.selector != null) if(myScript.selector.Length > 0){
				if(myScript.selectorNames.Length != myScript.selector.Length)
					myScript.SelectorNameUpdate();
				//myScript.selectedStyle = EditorGUILayout.Popup("Select wall prefab:", myScript.selectedStyle, myScript.selectorNames);
				
				myScript.UpdateStyle();
			}
		}else if(myScript.selectedStyle < 0)
			myScript.selectedStyle = 0;
		else if(myScript.selectedStyle >= myScript.selector.Length)
			myScript.selectedStyle = myScript.selector.Length -1;
	}
}