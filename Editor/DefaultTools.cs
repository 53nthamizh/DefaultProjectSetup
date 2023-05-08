// The namespaces used in this file.
using System; // For using DateTime, etc.
using System.Collections.Generic; // For using List<>
using System.IO; // For using Path, Directory
using System.Linq; // To enable LINQ (Language-Integrated Query)
using UnityEditor; // For using MenuItem, EditorUtility
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;  // For getting package version
using UnityEngine; 
using UnityEngine.Rendering; // To get Pipeline version

// The static imports used in this file.
using static System.IO.Directory; // For importing the static methods of Directory class.
using static System.IO.Path; // For importing the static methods of Path class.
using static UnityEngine.Application; // For importing the static methods of Application class.
using static UnityEditor.AssetDatabase; // For importing the static methods of AssetDatabase class.

namespace ATSAETechnologies
{
    /// <summary>
    /// The `DefaultTools` class contains various utility methods for working with directories and files in the Unity Editor.
    /// </summary>
    public static class DefaultTools
    {
        // This dictionary defines the default folder structure for the project.
        public static Dictionary<string, Dictionary<string, string[]>> directories = new Dictionary<string, Dictionary<string, string[]>>
        {
            {
                "", new Dictionary<string, string[]>
                {
                    { "Art", new[] { "3D Models", "Animation", "Font", "Materials", "References", "Textures", "UI" } },
                    { "Audio", new[] { "Music", "SFX" } },
                    { "Documentation", null },
                    { "Code", new[] { "Logics", "Shaders" } },
                    { "Plugins", null },
                    { "Prefabs", null },
                    { "Streaming Assets", null},
                    { "Scenes", null },
                    { "Settings", null}
                }
            },
            // Define nested directories inside the "Art" directory.
            {
                "Art", new Dictionary<string, string[]>
                {
                    { "Textures", new[] { "Skybox" } },
                    { "References", new[] { "Images", "Videos", "Documents" } }
                }
            },
            // Define nested directories inside the "Code/Logics" directory.
            {
                "Code", new Dictionary<string, string[]>
                {
                    { "Logics", new[] { "AI", "Navigation" } }
                }
            },
            // Define a nested directory inside the "Art/References" directory.
            {
                "Art/References", new Dictionary<string, string[]>
                {
                    { "Documents", new[] { "Pdf", "Text" } }
                }
            }
        };

        // <summary>
        /// Creates the default folders as defined in the "directories" dictionary.
        /// </summary>
        [MenuItem("Tools/Create/Default Folders")]
        public static void CreateDefaultFolders()
        {
            // Get the list of missing folders
            List<string> missingFolders = GetMissingFolders();
            // If no folders need to be created, display a message to the user.
            if (missingFolders.Count == 0)
            {
                EditorUtility.DisplayDialog("No New Folders Needed", "No new folders have to be created.", "OK");
                return;
            }

            string folderList = string.Join(", ", missingFolders);

            // If folders need to be created, show a confirmation dialog box to the user.
            if (EditorUtility.DisplayDialog("Create Default Folders",
                    $"This will create the following missing folders in your project: {folderList}. Do you want to proceed?",
                    "Yes", "No"))
            {
                // Create the missing folders
                foreach (string path in missingFolders)
                {
                    CreateDirectory(Combine(dataPath, path));
                }

                // Refresh the project view to reflect the new directories
                Refresh();
            }
        }


        /// <summary>
        /// This method checks if any folders from the project's default folders list are missing.
        /// </summary>
        /// <param name="missingFolders">A list of missing folders.</param>
        /// <returns>Returns true if all the folders exist, false otherwise.</returns>
        public static bool CheckIfFoldersExist(out List<string> missingFolders)
        {
            bool allExist = true;
            missingFolders = new List<string>();

            // Loop through each top-level directory in the dictionary.
            foreach (var topLevelEntry in directories)
            {
                string topLevelKey = topLevelEntry.Key;
                var topLevelValue = topLevelEntry.Value;

                // If the top-level directory has nested directories.
                if (topLevelValue != null)
                {
                    // Loop through each nested directory in the top-level directory.
                    foreach (var nestedEntry in topLevelValue)
                    {
                        string nestedKey = nestedEntry.Key;
                        var nestedValue = nestedEntry.Value;

                        // Construct the full path to the current directory.
                        string fullPath = topLevelKey == "" ? nestedKey : $"{topLevelKey}/{nestedKey}";

                        // Check if the directory exists, if not add it to the missingFolders list.
                        if (!Exists(Combine(dataPath, fullPath)))
                        {
                            allExist = false;
                            missingFolders.Add(fullPath);
                        }

                        // If the current directory has nested directories.
                        if (nestedValue != null)
                        {
                            // Loop through each nested directory in the current directory.
                            foreach (string subdir in nestedValue)
                            {
                                string subFullPath = $"{fullPath}/{subdir}";
                                // Check if the nested directory exists, if not add it to the missingFolders list.
                                if (!Exists(Combine(dataPath, subFullPath)))
                                {
                                    allExist = false;
                                    missingFolders.Add(subFullPath);
                                }
                            }
                        }
                    }
                }
            }

            return allExist;
        }
        
        /// <summary>
        /// Checks if any folders from the project's default folders list are missing.
        /// </summary>
        /// <returns>A list of missing folders.</returns>
        public static List<string> GetMissingFolders()
        {
            // Initialize the list of missing folders.
            List<string> missingFolders = new List<string>();

            // Loop through each top-level folder entry in the default folders list.
            foreach (var topLevelEntry in directories)
            {
                // Get the key and value for the current top-level folder.
                string topLevelKey = topLevelEntry.Key;
                var topLevelValue = topLevelEntry.Value;

                // If the current top-level folder has nested folders.
                if (topLevelValue != null)
                {
                    // Loop through each nested folder entry in the current top-level folder.
                    foreach (var nestedEntry in topLevelValue)
                    {
                        // Get the key and value for the current nested folder.
                        string nestedKey = nestedEntry.Key;
                        var nestedValue = nestedEntry.Value;

                        // Construct the full path for the current folder.
                        string fullPath = topLevelKey == "" ? nestedKey : $"{topLevelKey}/{nestedKey}";

                        // Check if the current folder exists in the project.
                        if (!Exists(Combine(dataPath, fullPath)))
                        {
                            // Add the current folder to the list of missing folders if it doesn't exist.
                            missingFolders.Add(fullPath);
                        }

                        // If the current nested folder has subfolders.
                        if (nestedValue != null)
                        {
                            // Loop through each subfolder in the current nested folder.
                            foreach (string subdir in nestedValue)
                            {
                                // Construct the full path for the current subfolder.
                                string subFullPath = $"{fullPath}/{subdir}";

                                // Check if the current subfolder exists in the project.
                                if (!Exists(Combine(dataPath, subFullPath)))
                                {
                                    // Add the current subfolder to the list of missing folders if it doesn't exist.
                                    missingFolders.Add(subFullPath);
                                }
                            }
                        }
                    }
                }
            }

            // Return the list of missing folders.
            return missingFolders;
        }
        
        /// <summary>
        /// Creates directories in the project's data path.
        /// </summary>
        /// <param name="root">The root directory for the new directories.</param>
        /// <param name="dir">The names of the new directories.</param>
        public static void Dir(string root = "", params string[] dir)
        {
            // Get the full path for the root directory.
            var fullpath = root == "" ? dataPath : Combine(dataPath, root);

            // Loop through each new directory name.
            foreach (var newDirectory in dir)
            {
                // Construct the full path for the current new directory.
                string subDirectoryPath = Combine(fullpath, newDirectory);

                // If the current new directory doesn't exist in the project.
                if (!Exists(subDirectoryPath))
                {
                    // Create the current new directory.
                    CreateDirectory(subDirectoryPath);
                }
            }
        }
        
        public static string GetRenderPipelineVersion()
        {
            if (GraphicsSettings.renderPipelineAsset != null)
            {
                string pipelineName = GraphicsSettings.renderPipelineAsset.GetType().Name;

                // Check if using URP or HDRP
                if (pipelineName == "UniversalRenderPipelineAsset")
                {
                    // Get the version of the URP package
                    ListRequest listRequest = Client.List();
                    while (!listRequest.IsCompleted) {}

                    if (listRequest.Status == StatusCode.Success)
                    {
                        var packages = listRequest.Result;
                        foreach (var package in packages)
                        {
                            if (package.name.StartsWith("com.unity.render-pipelines.universal") && package.name.EndsWith("package"))
                            {
                                return package.version;
                            }
                        }
                    }
                }
                else if (pipelineName == "HDRenderPipelineAsset")
                {
                    // Get the version of the HDRP package
                    ListRequest listRequest = Client.List();
                    while (!listRequest.IsCompleted) {}

                    if (listRequest.Status == StatusCode.Success)
                    {
                        var packages = listRequest.Result;
                        foreach (var package in packages)
                        {
                            if (package.name.StartsWith("com.unity.render-pipelines.high-definition") && package.name.EndsWith("package"))
                            {
                                return package.version;
                            }
                        }
                    }
                }
            }

            // If not using URP or HDRP, return an empty string
            return "";
        }

        
        /// <summary>
        /// Creates or updates the README.md file in the Documentation folder of the project.
        /// If the file does not exist, it is created.
        /// If the file exists, it is updated with the project name, version, and other information.
        /// </summary>
        /// <remarks>
        /// This method uses the Unity editor API to get the project name, version, and Unity version.
        /// </remarks>
        [MenuItem("Tools/Create/Create or Update Readme")]
        public static void CreateOrUpdateReadme()
        {
            // Get the project name and version from the Unity editor.
            string projectName = Application.productName;
            string projectVersion = Application.version;
            
            // Get the company name from the Unity editor.
            string companyName = Application.companyName;

            // Get the active graphics tier.
            string graphicsTier = "Standard";
            if (GraphicsSettings.renderPipelineAsset != null)
            {
                if (GraphicsSettings.renderPipelineAsset.GetType().Name == "UniversalRenderPipelineAsset")
                {
                    graphicsTier = "URP";

                    // Get the version of the URP package.
                    string urpVersion = GetPackageVersion("com.unity.render-pipelines.universal");
                    graphicsTier += $"\n<i>Universal Render Pipeline Version:</i> {urpVersion}";
                }
                else if (GraphicsSettings.renderPipelineAsset.GetType().Name == "HDRenderPipelineAsset")
                {
                    graphicsTier = "HDRP";

                    // Get the version of the HDRP package.
                    string hdrpVersion = GetPackageVersion("com.unity.render-pipelines.high-definition");
                    graphicsTier += $"\n<i>High Definition Render Pipeline Version:</i> {hdrpVersion}";
                }
            }

            // Get the current date and time.
            DateTime currentTime = DateTime.Now;
            
            // Create the text to be written to the README.md file.
            string readmeText = $"# <b>{projectName}</b>\n\n" +
                                $"<i>Version:</i> {projectVersion}\n\n" +
                                $"<i>by:</i> {companyName}\n\n" +
                                $"<i>This project was created with Unity:</i> {Application.unityVersion}\n\n" +
                                $"<i>Graphics Tier:</i> {graphicsTier}\n\n" +
                                $"<i>Last updated:</i> {currentTime.ToString()}\n\n" +
                                $"## <b>{"Description"}<b>\n\n"+
                                $"<i>Project description here:</i>\n\n" +
                                $"## <b>Credits & Acknowledgments</b>\n\n" +
                                "<i>Add any additional information or instructions here.</i>";

            // Get the path to the README.md file.
            string readmePath = Combine(dataPath, "README.md");

            // Check if the file exists.
            if (File.Exists(readmePath))
            {
                // If the file exists, update its contents.
                File.WriteAllText(readmePath, readmeText);
                Debug.Log("README.md updated successfully!");
            }
            else
            {
                // If the file does not exist, create it.
                File.WriteAllText(readmePath, readmeText);
                AssetDatabase.ImportAsset(readmePath);
                Debug.Log("README.md created successfully! Check in Assets folder.");
            }
        }

        // Helper method to get the version of a package.
        private static string GetPackageVersion(string packageName)
        {
            string version = "";
            ListRequest request = Client.List(false, true);
            while (!request.IsCompleted)
            {
                // Wait for the request to complete.
            }
            if (request.Status == StatusCode.Success)
            {
                foreach (var package in request.Result)
                {
                    if (package.name == packageName)
                    {
                        version = package.version;
                        break;
                    }
                }
            }
            return version;
        }
    }
}