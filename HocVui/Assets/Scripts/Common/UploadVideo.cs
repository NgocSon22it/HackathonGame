using System.Collections;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;

public class UploadVideo : MonoBehaviour
{
    [SerializeField] VideoPlayer videoPlayer;
    private void Start()
    {

        // Use Unity's cross-platform file picker
        string selectedFilePath = EditorUtility.OpenFilePanel("Select a Video File", "", "mp4");

        if (!string.IsNullOrEmpty(selectedFilePath))
        {
            StartCoroutine(CopyAndPlayVideoAsync(selectedFilePath));
        }
    }

    private IEnumerator CopyAndPlayVideoAsync(string sourceFilePath)
    {
        // Define the destination folder path within Unity's project folder
        string destinationFolderPath = Application.dataPath + "/Resources";

        // Get the file name from the source path
        string fileName = Path.GetFileName(sourceFilePath);

        // Combine the destination folder path with the file name
        string destinationFilePath = Path.Combine(destinationFolderPath, fileName);

        // Use C#'s asynchronous file copy method
        Task copyTask = CopyFileAsync(sourceFilePath, destinationFilePath);

        while (!copyTask.IsCompleted)
        {
            yield return null;
        }

        if (copyTask.Exception == null)
        {
            Debug.Log("File copied successfully.");

            // Load the video clip from Resources
            VideoClip videoClip = Resources.Load<VideoClip>(Path.GetFileNameWithoutExtension(fileName));

            if (videoClip != null)
            {
                // Assign the video clip to the VideoPlayer
                videoPlayer.clip = videoClip;

                // Play the video
                videoPlayer.Play();
            }
            else
            {
                Debug.LogError("Failed to load video clip from Resources.");
            }
        }
        else
        {
            Debug.LogError("Error copying file: " + copyTask.Exception.Message);
        }
    }

    private async Task CopyFileAsync(string sourceFilePath, string destinationFilePath)
    {
        try
        {
            using (FileStream sourceStream = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true))
            using (FileStream destinationStream = new FileStream(destinationFilePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true))
            {
                await sourceStream.CopyToAsync(destinationStream);
            }
        }
        catch (System.Exception e)
        {
            throw new System.Exception("File copy failed.", e);
        }
    }

}
