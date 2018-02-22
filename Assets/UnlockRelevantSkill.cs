﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnlockRelevantSkill : MonoBehaviour, IPointerClickHandler {

    //The name of the object that contains the sript InitializeSkillTrees.cs
    private const string INITIALIZER = "Initializer";

    //Allows references to variables from InitializeSkillTrees.cs
    public InitializeSkillTrees skillTreeLibrary;

    //Allows references to variables from SkillTreeUIBinding.cs
    public SkillTreeUIBinding UIBindingLibrary;

	// Use this for initialization
	void Start () {
        skillTreeLibrary = GameObject.Find(INITIALIZER).GetComponent<InitializeSkillTrees>();
        UIBindingLibrary = GameObject.Find("SkillTreeView").GetComponent<SkillTreeUIBinding>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //Gives behaviour when panel is clicked on
    public void OnPointerClick(PointerEventData eventData)
    {
        int skillNodeIndex = UIBindingLibrary.UITreeNodes.FindIndex(x => x == gameObject);
        SkillNode selectedNode = skillTreeLibrary.metalworkingSkillTree.subtrees[0].skills[skillNodeIndex];
        processSelectedNode(selectedNode);
    }

    //Performs action based on selected node
    public void processSelectedNode(SkillNode node)
    {
        string message = "ID: " + node.getId() + " Tier: " + node.getTier();
        Debug.Log(message, gameObject);

    }
}
