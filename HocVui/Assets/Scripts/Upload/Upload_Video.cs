//using Firebase;
//using Firebase.Extensions;
//using Firebase.Storage;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Networking;
//using UnityEngine.UI;
//using UnityEngine.Video;

//public class Upload_Video : MonoBehaviour
//{
//    void Start()
//    {
//        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
//        {
//            FirebaseApp app = FirebaseApp.DefaultInstance;
//            // Initialize Firebase Storage
//            FirebaseStorage storage = FirebaseStorage.DefaultInstance;
//        });
//    }

//    public void UploadVideo()
//    {
//        // Reference to Firebase Storage
//        FirebaseStorage storage = FirebaseStorage.DefaultInstance;

//        // Reference to the storage bucket
//        StorageReference storageRef = storage.GetReferenceFromUrl("gs://hocvui-51d2c.appspot.com");

//        // Create a reference to the image in the storage bucket
//        StorageReference imageRef = storageRef.Child("video").Child(Question_Manager.Instance.FileName);

//        // Upload the image
//        imageRef.PutFileAsync(Question_Manager.Instance.FilePath).ContinueWith(task =>
//        {
//            if (task.IsFaulted || task.IsCanceled)
//            {
//                Debug.LogError("Video upload failed.");
//            }
//            else if (task.IsCompleted)
//            {
//                Debug.Log("Video upload successful.");
//                DownloadVideo();
//            }
//        });
//    }

//    public void DownloadVideo()
//    {
//        // Reference to Firebase Storage
//        FirebaseStorage storage = FirebaseStorage.DefaultInstance;

//        // Reference to the storage bucket
//        StorageReference storageRef = storage.GetReferenceFromUrl("gs://hocvui-51d2c.appspot.com");

//        // Reference to the remote video in the storage bucket
//        StorageReference videoRef = storageRef.Child("video").Child(Question_Manager.Instance.FileName);

//        // Download the video to local storage
//        videoRef.GetDownloadUrlAsync().ContinueWithOnMainThread(task =>
//        {
//            if (!task.IsFaulted && !task.IsCanceled)
//            {
//                string videoUrl = task.Result.ToString();
//                Debug.Log("Success to get video download URL." + videoUrl);
//                Question_Manager.Instance.VideoObj.source = VideoSource.Url;
//                Question_Manager.Instance.VideoObj.url = videoUrl;
//                Question_Manager.Instance.VideoObj.Prepare();
//                Debug.Log("Success to get video download URL." + videoUrl);
//            }
//            else
//            {
//                Debug.LogError("Failed to get video download URL.");
//            }
//        });
//    }
//}
