# Unity scene loader

## Info
- Unity version 2018.3.8f1
- Scripting Runtime Version .NET 4.x Equivalent
- Api Compatibility Level .NET 4.x

## Setup

- Create Empty scene (loader scene)
- Add scene on the first place into Build Settings
- Add other scens into Build Settings
- Add prefab SceneLoader from SLoader/Prefabs into loader scene
- In inspector of the SceneLoader set First Scene To Load (be sure that scene is in the Build Settings)
- Let's play
- For loading any new scene ypu can use SceneLoader.Instance?.LoadScene(<SCENE_NAME_OR_INDEX>);

## Example

You can try example located in SLoader/Example. 
Start scene is SLoader/Example/Scenes/loader.unity

## Settings

- Tip list file is located at Resources/tips.json and you can easily edit it
- SceneLoader - First Scene To Load: Name of the first scene you want to load
- SceneLoader - Tip Time: Minimum tip display time (if the scene loading time is too short)
- SceneLoader - Load From Web: You can load tips from the web. If tip doesn't exist, tips from resources will be used
- SceneLoader - Url: In the example, I'm using web service from [davand.net](http://davand.net) for getting tips. 
	Yes, this is a to-do list, but it's good enough for example.  That to-do is my open 
	source project ([SimpleMVC PHP framework](https://github.com/zmalic/simple-mvc-php-framework)). I'm using random of 
	first 15 to-does for tips, so you can edit them on [davand.net](http://davand.net)
- SceneLoader/LoadingScreen: You can customize loading panel by params on LazyFollow script
- SceneLoader/Fader: You can customize fading animation duration



