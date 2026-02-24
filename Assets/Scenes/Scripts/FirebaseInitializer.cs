using System;
using System.Threading.Tasks;
using Firebase;
using Firebase.Extensions;
using Firebase.Analytics;
using Firebase.RemoteConfig;
using Firebase.Crashlytics;
using UnityEngine;

public class FirebaseInitializer : MonoBehaviour
{
    private void Start()
    {
        // Проверяем зависимости Firebase
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(DependencyStatusReceived);
        FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        FirebaseAnalytics.LogEvent("editor_test_event");

    }

    private void DependencyStatusReceived(Task<DependencyStatus> task)
    {
        if (task.IsFaulted || task.IsCanceled)
        {
            Debug.LogError($"Firebase dependencies failed: {task.Exception}");
            return;
        }

        var status = task.Result;
        if (status == DependencyStatus.Available)
        {
            // ВАЖНО: Создаем экземпляр приложения
            FirebaseApp app = FirebaseApp.DefaultInstance;
            Debug.Log("Firebase initialized successfully!");
            
            // Тестовое событие Analytics
            FirebaseAnalytics.LogEvent("test_event");
            
            // Инициализируем Remote Config
            InitializeRemoteConfig();
        }
        else
        {
            Debug.LogError($"Firebase dependencies not available: {status}");
        }
    }

    private void InitializeRemoteConfig()
    {
        // Значения по умолчанию
        var defaults = new System.Collections.Generic.Dictionary<string, object>();
        defaults.Add("testValue", 0); // число, как у вас на сайте
        
        FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults)
            .ContinueWithOnMainThread(OnDefaultsSet);
    }

    private void OnDefaultsSet(Task task)
    {
        if (task.IsFaulted || task.IsCanceled)
        {
            Debug.LogError($"Failed to set Remote Config defaults: {task.Exception}");
            return;
        }

        Debug.Log("Remote Config defaults set successfully");
        
        // Запускаем получение данных с сервера
        FetchDataAsync();
    }

    private void FetchDataAsync()
    {
        Debug.Log("Fetching Remote Config data...");
        
        FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero)
            .ContinueWithOnMainThread(FetchComplete);
    }

    private void FetchComplete(Task fetchTask)
    {
        if (!fetchTask.IsCompleted) 
        {
            Debug.LogError("Retrieval hasn't finished.");
            return;
        }

        var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
        var info = remoteConfig.Info;
        if (info.LastFetchStatus != LastFetchStatus.Success) 
        {
            Debug.LogError($"({nameof(FetchComplete)}) was unsuccessful\n({nameof(info.LastFetchStatus)}): {info.LastFetchStatus}");
            return;
        }

        // Fetch successful. Parameter values must be activated to use.
        remoteConfig.ActivateAsync()
        .ContinueWithOnMainThread(task => 
        {
            Debug.Log($"Remote data loaded and ready for use. Last fetch time {info.FetchTime}.");
            
            // Получаем значение testValue с сервера
            float value = (float)FirebaseRemoteConfig.DefaultInstance.GetValue("testValue").DoubleValue;
            Debug.Log($"testValue = {value}");
        });
    }

    public void ForceCrash()
    {
        Debug.Log("Force crash button pressed");

    // Отправляем лог в Firebase
        Crashlytics.Log("Manual crash for testing");

    // Специально вызываем ошибку
        throw new System.Exception("Test crash from button");
    }

}