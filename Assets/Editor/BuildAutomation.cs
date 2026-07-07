using UnityEditor;
using UnityEngine;

public class BuildAutomation
{
    //Windows用メニュー
    [MenuItem("Build/Windows/Development Version")]
    public static void BuildWindowsDevelopment()
    {
        ExecuteBuild(BuildOptions.Development | BuildOptions.ConnectWithProfiler, BuildTarget.StandaloneWindows64, ".exe");
    }

    [MenuItem("Build/Windows/Release Version")]
    public static void BuildWindowsRelease()
    {
        ExecuteBuild(BuildOptions.None, BuildTarget.StandaloneWindows64, ".exe");
    }

    //Android用メニュー
    [MenuItem("Build/Android/Development Version")]
    public static void BuildAndroidDevelopment()
    {
        ExecuteBuild(BuildOptions.Development | BuildOptions.ConnectWithProfiler, BuildTarget.Android, ".apk");
    }

    [MenuItem("Build/Android/Release Version")]
    public static void BuildAndroidRelease()
    {
        ExecuteBuild(BuildOptions.None, BuildTarget.Android, ".apk");
    }

    //WebGL用メニュー
    [MenuItem("Build/WebGL/Development Version")]
    public static void BuildWebGLDevelopment()
    {
        ExecuteBuild(BuildOptions.Development | BuildOptions.ConnectWithProfiler, BuildTarget.WebGL, "");
    }

    [MenuItem("Build/WebGL/Release Version")]
    public static void BuildWebGLRelease()
    {
        ExecuteBuild(BuildOptions.None, BuildTarget.WebGL, "");
    }

    private static void ExecuteBuild(BuildOptions options, BuildTarget target, string extension)
    {
        //ビルド対象のシーン一覧を取得する
        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
        string[] scenePaths = new string[scenes.Length];

        for (int i = 0; i < scenes.Length; i++)
        {
            scenePaths[i] = scenes[i].path;
        }

        string buildType;
        if ((options & BuildOptions.Development) != 0)
        {
            buildType = "Debug";
        }
        else
        {
            buildType = "Release";
        }

        string buildPath = "Builds/" + target.ToString() + "/" + buildType + "/" + Application.productName;

        if ((options & BuildOptions.Development) != 0)
        {
            buildPath += "_Debug";
        }
        //引数で受け取った拡張子を付与する
        buildPath += extension;

        //ビルド設定を構築する
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = scenePaths;
        buildPlayerOptions.locationPathName = buildPath;

        //引数で受け取ったターゲットを設定する
        buildPlayerOptions.target = target;
        buildPlayerOptions.options = options;

        //ビルドを実行する
        BuildPipeline.BuildPlayer(buildPlayerOptions);
        Debug.Log("ビルドが完了しました: " + buildPath);
    }
}