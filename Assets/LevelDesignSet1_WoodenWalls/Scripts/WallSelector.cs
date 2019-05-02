using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class WallSelector : MonoBehaviour {
	public bool editObj = true;
	public string scriptName;
	public GameObject[] assets;
	public AssetSelector[] selector;
	[HideInInspector]
	public string[] selectorNames;
	[HideInInspector]
	public int selectedStyle = 0;
	public Vector3 gizmoOffset = Vector3.zero;
	public Vector3 gizmoDimension = new Vector3(1,1,1);
	public Color gizmoColor = new Color(0,0,1,0.3f);

	void  OnDrawGizmos (){
		Gizmos.matrix = transform.localToWorldMatrix;
		Gizmos.color = gizmoColor;
		Gizmos.DrawCube(gizmoOffset, gizmoDimension);
		Gizmos.color = new Color(gizmoColor.r, gizmoColor.g, gizmoColor.b, 0.5f);
		Gizmos.DrawWireCube(gizmoOffset, gizmoDimension);
	}

	public void  UpdateStyle (){	
		if(selector.Length == 0 || assets.Length == 0 || !editObj)
			return;
		
		for(int i= 0; i < assets.Length; i++){
			assets[i].SetActive(false);
			
			for(int s= 0; s < selector[selectedStyle].includeAssets.Length; s++){
				if(i == selector[selectedStyle].includeAssets[s])
					assets[i].SetActive(true);
			}
		}
	}

	public void  SelectorNameUpdate (){
		selectorNames = new string[selector.Length];
		
		for(int n= 0; n < selector.Length; n++)
			selectorNames[n] = selector[n].selectionName;
	}
	
	public void SelectStyle(int i){
		#if UNITY_EDITOR
		SerializedObject so = new SerializedObject(this);
		SerializedProperty so_selectedStyle = so.FindProperty("selectedStyle");
		so_selectedStyle.intValue = i;
		so.ApplyModifiedProperties();
		#endif
	}

	[System.Serializable]
	public class AssetSelector {
		public int[] includeAssets;
		public string selectionName = "";
	}
}