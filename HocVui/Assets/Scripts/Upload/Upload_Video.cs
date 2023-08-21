using Firebase;
using Firebase.Extensions;
using Firebase.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Video;

public class Upload_Video : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            // Initialize Firebase Storage
            FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        });
    }

    public void UploadVideo(string imagePath)
    {
        // Reference to Firebase Storage
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;

        // Reference to the storage bucket
        StorageReference storageRef = storage.GetReferenceFromUrl("gs://hocvui-51d2c.appspot.com");

        // Create a reference to the image in the storage bucket
        StorageReference imageRef = storageRef.Child("video").Child("abc.mp4");

        // Upload the image
        imageRef.PutFileAsync(imagePath).ContinueWith(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Video upload failed.");
            }
            else if (task.IsCompleted)
            {
                Debug.Log("Video upload successful.");
            }
        });
    }

    public void DownloadVideo()
    {
        // Reference to Firebase Storage
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;

        // Reference to the storage bucket
        StorageReference storageRef = storage.GetReferenceFromUrl("gs://hocvui-51d2c.appspot.com");

        // Reference to the remote video in the storage bucket
        StorageReference videoRef = storageRef.Child("video").Child("abc.mp4");

        // Download the video to local storage
        videoRef.GetDownloadUrlAsync().ContinueWithOnMainThread(task =>
        {
            if (!task.IsFaulted && !task.IsCanceled)
            {
                Debug.Log("Success to get video download URL.");

                string videoUrl = task.Result.ToString();
                videoPlayer.url = videoUrl;
                videoPlayer.Play();
            }
            else
            {
                Debug.LogError("Failed to get video download URL.");
            }
        });
    }
}
