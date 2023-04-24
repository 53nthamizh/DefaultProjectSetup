// Import necessary namespaces
using System.IO; // Used to work with file system
using UnityEditor; // Used to create custom editor tools in Unity
using UnityEngine; // Used to access Unity API

// Use static imports to simplify code
using static System.IO.Directory; // Imports Directory class to use static methods
using static System.IO.Path; // Imports Path class to use static methods
using static UnityEngine.Application; // Imports Application class to use static methods
using static UnityEditor.AssetDatabase; // Imports AssetDatabase class to use static methods

// Define a namespace for the script
namespace ATSAETechnologies // Namespace used to group related scripts
{
    // Define a static class named "Tools"
    public static class Tools
    {
        // Define a method that will be called when the "Create/Default Folders" menu item is selected
        [MenuItem("Tools/Create/Default Folders")]
        public static void CreateDefaultFolders()
        {
            // Call the Dir() method to create the top-level directories
            Dir("", "Art", "Audio", "Documentation", "Code", "Plugins", "Prefabs", "Scenes");
            
            // Refresh the project view to reflect the new directories
            Refresh();
            
            // Call the Dir() method to create the subdirectories within the "Art" directory
            Dir("Art", "3D Models", "Animation", "Font", "Materials", "References", "Textures", "UI");
            
            // Call the Dir() method to create the subdirectories within the "Textures" directory
            Dir("Art/Textures", "Skybox");
            
            // Call the Dir() method to create the subdirectories within the "Audio" directory
            Dir("Art/Audio", "Music", "SFX");
            
            // Call the Dir() method to create the subdirectories within the "Code" directory
            Dir("Art/Code", "Logics", "Shaders");
            
            // Refresh the project view again to reflect the new directories
            Refresh();
        }

        // Define a helper method that creates directories within the project
        public static void Dir(string root = "", params string[] dir)
        {
            // If no root is specified, create the directories in the project's data path
            if (root == "")
            {
                var fullpath = dataPath; // Get the project data path
                foreach (var newDirectory in dir) // Loop through each string in the dir array
                {
                    // Create a new directory using the Combine() method to combine the data path and the directory name
                    CreateDirectory(Combine(fullpath, newDirectory)); // Create a new directory at the specified path
                }
            }
            // If a root is specified, create the directories within that directory
            else
            {
                var fullpath = Combine(dataPath, root); // Get the full path of the root directory
                foreach (var newDirectory in dir) // Loop through each string in the dir array
                {
                    // Create a new directory within the specified root directory
                    CreateDirectory(Combine(fullpath, newDirectory));
                }
            }
        }
    }
}
