# Byte Wars - Unity

## Overview

Byte Wars is the official tutorial game for Xsolla and AccelByte Gaming Services (AGS). It is intended to act as a sample project that can be used as a reference on the best practices to integrate our services into your game. We created Byte Wars from scratch as a fully functional offline game. This offline game was then brought online with the power of AccelByte's platform by adding different services from each of our service areas like access, play and engagement. Every tutorial module walks you through a step by step guide to add a specific feature to Byte Wars which you can then translate into your own game.

## Prerequisites

* Use **Unity Editor** 2021 LTS with minimum version 2021.3.16f1.

## Clone Byte Wars

This repository ships with the Xsolla plugin already wired in as a submodule:
* Xsolla Backend Unity SDK under `local-packages/XsollaSDK`.

To clone the repository and checkout the submodule at the same time, run the following command:

```batch
git clone --recursive git@bitbucket.org:accelbyte/accelbyte-unity-bytewars.git
```

The AccelByte and Xsolla Unity packages referenced in `Packages/manifest.json` (e.g. [AccelByte Unity SDK](https://github.com/AccelByte/accelbyte-unity-sdk), [Xsolla Store Unity SDK](https://github.com/xsolla/store-unity-sdk)) will be automatically resolved by the Unity Package Manager when the project is opened for the first time.

## Open Byte Wars in Unity

1. From Unity Hub, go to Projects sidebar and click open in the project panel 
2. A file browser will appear, then select Byte Wars project folder and click open. 
3. The Byte Wars project will be added into the project panel with the editor version. Click on the project to open it in the Unity Editor.

## Run Byte Wars Offline

### Run via Editor

1. Open the Byte Wars project on your Unity Editor. 
2. From project window, go to Assets>Scenes then double click MainMenu scene file. Make sure MainMenu has opened in the hierarchy.

### Run via Packaged Game

1. Open the Byte Wars project on your Unity Editor 
2. In your menu taskbar, go to File > Build Settings, a popup window will appear and and leave build setting as default and click build button.
3. Select the destination folder to save the package file and press select folder button.
4. Open the destination folder and double click the ByteWars.exe to play the game.

### Build from command line
1. Build Server run: `build_server.bat`
2. Build Client run: `build_client.bat`

### Debug Mode
1. To enable debug mode add `BYTEWARS_DEBUG` in Edit -> Project Settings -> Player -> Script Compilation -> Scripting Define Symbols. please remove `BYTEWARS_DEBUG` scripting define symbols before publish/release it 
2. To test peer to peer server and act as host on branch `Main` or `Master`, add `BYTEWARS_P2P_HOST` in Scripting Define symbols

### Peer to Peer Mode
Peer to Peer mode is currently disabled, to enable it check `Assets\Scripts\UI\MainMenu\PlayOnlineMenu.cs` line `28`.

## Run Byte Wars Online

Follow along Byte Wars [Learning Paths](https://docs.accelbyte.io/gaming-services/tutorials/byte-wars/unity/learning-paths/). We suggest you to start with the [Login with Device ID and Steam path](https://docs.accelbyte.io/gaming-services/tutorials/byte-wars/unity/learning-paths/authentication/unity-path-login-device-id-and-steam/) if you're unsure where to start.

## Git
To revert files with only line ending difference use --renormalize. Example this command will revert all prefabs with only line ending difference and also stage (add) other actually changed prefab files. 
```
git add --renormalize *.prefab
```
