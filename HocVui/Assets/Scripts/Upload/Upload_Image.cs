using Firebase;
using Firebase.Extensions;
using Firebase.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Upload_Image : MonoBehaviour
{
    public RawImage rawImage;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            // Initialize Firebase Storage
            FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        });
    }

    public void UploadImage(string imagePath)
    {
        // Reference to Firebase Storage
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;

        // Reference to the storage bucket
        StorageReference storageRef = storage.GetReferenceFromUrl("gs://hocvui-51d2c.appspot.com");

        // Create a reference to the image in the storage bucket
        StorageReference imageRef = storageRef.Child("images").Child("abc.png");

        // Upload the image
        imageRef.PutFileAsync(imagePath).ContinueWith(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Image upload failed.");
            }
            else if (task.IsCompleted)
            {
                Debug.Log("Image upload successful.");
            }
        });
    }

    public void DownloadImage()
    {
      
        //initialize storage reference
        var storage = FirebaseStorage.DefaultInstance;
        var storageReference = storage.GetReferenceFromUrl("gs://hocvui-51d2c.appspot.com");

        //get reference of image
        StorageReference image = storageReference.Child("images").Child("abc.png");

        //Get the download link of file
        image.GetDownloadUrlAsync().ContinueWithOnMainThread(task =>
        {
            if (!task.IsFaulted && !task.IsCanceled)
            {
                StartCoroutine(LoadImage(Convert.ToString(task.Result))); //Fetch file from the link
            }
            else
            {
                Debug.Log(task.Exception);
            }
        });
    }

    [Obsolete]
    IEnumerator LoadImage(string MediaUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl); //Create a request
        yield return request.SendWebRequest(); //Wait for the request to complete
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
            rawImage.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            // setting the loaded image to our object
        }
    }

}
