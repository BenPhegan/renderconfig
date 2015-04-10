## Configuration Settings ##

| **Name** | 		**ConfigFile** |
|:---------|:-----------------|
| MSBuild Attribute | 		ConfigFile |
|Command Line	|		-f, --file=VALUE |
|Description	|		The config file to parse |
|Values		|			String|
|Default		|		null|
|Required		|		Yes|
|  |  |
| **Name** | 			**Configuration** |
| MSBuild Attribute | 			Configuration |
|Command Line	|		-c, --configuration=VALUE|
|Description	|		The configuration to render|
|Values		|			String|
|Default		|		null|
|Required		|		Yes|
|  |  |
| **Name** | 		**OutputDirectory** |
| MSBuild Attribute | 		OutputDirectory |
|Command Line	|		-o, --output=VALUE  |
|Description	|		The directory to use as the default output directory|
|Values		|			String|
|Default		|		null|
|Required		|		No, however if absent requires SubDirectoryEachConfiguration to be true.|
|  |  |
| **Name** | 		**SubDirectoryEachConfiguration** |
| MSBuild Attribute | 		SubDirectoryEachConfiguration |
|Command Line	|		-s, --subdirperconfig  |
|Description	|		Whether a subdirectory will be created for each configuration rendered. |
|Values		|			true/false|
|Default		|		false|
|Required		|		No, however if absent requires OutputDirectory to be provided.|
|  |  |
| **Name** | 		**PreserveSourceStructure** |
| MSBuild Attribute | 		PreserveSourceStructure |
|Command Line	|		-p, --preserve |
|Description	|		Whether to preserve the source structure when creating the target files. |
|Values		|			true/false|
|Default		|		false|
|Required		|		No|
|  |  |
| **Name** | 			**InputDirectory** |
| MSBuild Attribute | 			InputDirectory |
|Command Line	|		-i, --input=VALUE|
|Description	|		The input directory to use as the base for all relative source file paths.|
|Values		|			String|
|Default		|		null|
|Required		|		No|
|  |  |
| **Name** | 		**DeleteOutputDirectory** |
| MSBuild Attribute | 	DeleteOutputDirectory |
|Command Line	|		-d, --deleteoutput  |
|Description	|		Delete the output directory prior to rendering the configuration.|
|Values		|			true/false|
|Default		|		true|
|Required		|		No|
|  |  |
| **Name** | 			**CleanOutput** |
| MSBuild Attribute | 			CleanOutput |
|Command Line	|		-l, --clean |
|Description	|		When set the XML output is "cleaned", which deletes all non-functional xml.|
|Values		|			true/false|
|Default		|		false|
|Required	|			No|
|  |  |
| **Name** | 		**BreakOnNoMatch** |
| MSBuild Attribute | 		BreakOnNoMatch |
|Command Line		|	-b, --break |
|Description		|	When set to true, if a match is not found for a configuration change, an error is raised and the application aborts..|
|Values			|		true/false|
|Default			|	false|
|Required			|	No|