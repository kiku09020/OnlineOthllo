using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptTemplateGeneratorSettings", menuName = "Editor/ScriptTemplateGeneratorSettings")]
public class ScriptTemplateGeneratorSettings : ScriptableObject
{
	/* Fields */
	[Header("Template Settings")]
	[SerializeField, Tooltip("Template")] string templateDirPath = "Assets/ScriptsTemplates/SubTemplates/";
	[SerializeField] string templateFileExtension = ".txt";
	[SerializeField] string createdFileExtension = ".cs";

	[Header("Generator Settings")]
	[SerializeField] bool enableDebugLog = true;

	//-------------------------------------------------------------------
	/* Properties */
	public string TemplateDirPath => templateDirPath;
	public string TemplateFileExtension => templateFileExtension;
	public string CreatedFileExtension => createdFileExtension;
	public bool EnableDebugLog => enableDebugLog;

	//-------------------------------------------------------------------
	/* Messages */

	
}
