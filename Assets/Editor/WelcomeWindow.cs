using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    public class WelcomeWindow : EditorWindow
    {
        private static string currentAnnouncement = "Welcome to the latest update of Ink! Be sure to check out our latest features.";
        private static int announcementVersion = 1;
        private static int announcementVersionPreviouslySeen = -1;
        private string gitBranchName = "Unknown Branch"; // Default value

        [MenuItem("Underneath/Welcome Window")]
        public static void ShowWindow()
        {
            WelcomeWindow window = GetWindow<WelcomeWindow>("Welcome Screen");
            window.minSize = new Vector2(520, 320);
        }

        public void CreateGUI()
        {
            // Load and clone the UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/Resources/WelcomeScreen.uxml");
            VisualElement root = visualTree.CloneTree();
            rootVisualElement.Add(root);

            // Load the stylesheet
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/Resources/WelcomeScreen.uss");
            root.styleSheets.Add(styleSheet);

            // Set the logo image
            var logoImage = root.Q<Image>("logoIcon");
            var logoTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Editor/Resources/LogoIcon.png");
            logoImage.image = logoTexture;

            // Display the Git branch instead of the version
            gitBranchName = GetGitBranchName();
            var branchLabel = root.Q<Label>("branchLabel");
            branchLabel.text = $"Current Branch: {gitBranchName}";

            // Bind button actions
            root.Q<Button>("GDDButton").clicked += () =>
                Application.OpenURL("https://joshuamoskowitz99.atlassian.net/wiki/spaces/ME/pages/66082/Underneath+DESIGN+DOCUMENT");
            root.Q<Button>("JiraButton").clicked += () =>
                Application.OpenURL("https://joshuamoskowitz99.atlassian.net/jira/software/c/projects/UNDER/boards/4");
            root.Q<Button>("AssetsButton").clicked += () =>
                Application.OpenURL("https://drive.google.com/drive/folders/1sF2dOCwXi3LPahK1PNIZp62QzJyY-BIJ?usp=sharing");
            root.Q<Button>("discordButton").clicked += () => Application.OpenURL("https://discord.gg/thnuHaCesK");
            root.Q<Button>("closeButton").clicked += Close;
        
            // HandleAnnouncements(root);
        }

        private string GetGitBranchName()
        {
            var gitHeadFilePath = Path.Combine(Application.dataPath, "../.git/HEAD");

            if (File.Exists(gitHeadFilePath))
            {
                var headFileContents = File.ReadAllText(gitHeadFilePath);
                if (headFileContents.StartsWith("ref: refs/heads/"))
                {
                    // Extract the branch name
                    var branchName = headFileContents.Substring("ref: refs/heads/".Length).Trim();
                    return branchName;
                }
            }

            return "Unknown Branch";
        }

        private void HandleAnnouncements(VisualElement root)
        {
            var announcementSection = root.Q<VisualElement>("announcementSection");
            var announcementTitle = root.Q<Label>("announcementTitle");
            var announcementText = root.Q<Label>("announcementText");

            if (announcementVersion > announcementVersionPreviouslySeen)
            {
                // Display the announcement
                announcementSection.style.display = DisplayStyle.Flex;
                announcementText.text = currentAnnouncement;

                // Update the previously seen announcement version
                announcementVersionPreviouslySeen = announcementVersion;
            }
            else
            {
                // Hide the announcement section if there's no new announcement
                announcementSection.style.display = DisplayStyle.None;
            }
        }
    }
}
