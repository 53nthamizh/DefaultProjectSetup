# Default Tools
<i>Version:</i> 1.0.2

This Unity editor tool automatically creates a default folder structure for your Unity project. With just one click, you can create top-level folders such as "Art", "Audio", "Code", "Documentation", "Plugins", "Prefabs", and "Scenes". It also creates subdirectories for each of these top-level folders. This is a great way to organize your Unity project and keep it clean.

This tool also creates and updates Readme.md file for your Unity Project. With just a click. 

## Installation

### Option 1: Import via Unity Package Manager (UPM)

You can install this package via UPM by following these steps:

1. Open your Unity project.
2. Open the Package Manager window.
3. Click on the "+" button located on the top-left corner of the window.
4. Select "Add package from git URL" from the dropdown menu.
5. Paste the following URL into the text box: https://github.com/53nthamizh/DefaultProjectSetup.git
6. Click on the "Add" button.

### Option 2: Manual installation

You can also manually install this tool by following these steps:

1. Open the Unity project.
2. Open the Package Manager window.
3. Add a new Scoped Registry (or edit the existing OpenUPM entry)
    Name package.openupm.com
    URL https://package.openupm.com
    Scope(s) com.atsaetechnologies.defaultfolders
  click Save (or Apply)
4. Click on the "+" button located on the top-left corner of the window.
5. Select "Add package by name" from the dropdown menu.
6. Paste the following into the name text box: com.atsaetechnologies.defaultfolders
7. Clck on the "Add" button.

## Usage

To use this tool, simply open the Unity editor and select "Tools > Create > Default Folders" from the top menu. This will automatically create the default folder structure in your project. You can modify the folder structure by modifying the Dir() method in the DefaultTools.cs file.

To use this tool, open the Unity editor and select "Tools > Create/Create or Update Readme" from the top menu. A README.md file, detailing the project's history, name, version, creator, graphics tier, and last update time, will be created or updated in the root directory. The CreateOrUpdateReadme() method in DefaultTools.cs allows you to further tailor the README.md file's contents.

## License

This Unity package is licensed under the GNU GPL license, a copy of which can be found in the LICENSE file.

## Contact

For any questions or feedback, please feel free to contact us at hello@senthamizh.dev
