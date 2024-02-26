using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;

public class BuildPostProcessor
{
    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        //// ChromeDriver의 원본 경로
        //string sourcePath = Path.Combine(Application.dataPath, "WebDriver");
        //// 빌드된 애플리케이션의 경로
        //string targetPath = Path.GetDirectoryName(pathToBuiltProject);

        //// Windows 빌드의 경우 실행 파일과 같은 위치에 복사
        //// macOS 또는 다른 플랫폼의 경우 약간의 경로 조정이 필요할 수 있음
        //if (target == BuildTarget.StandaloneWindows || target == BuildTarget.StandaloneWindows64)
        //{
        //    targetPath = Path.Combine(targetPath, $"{Path.GetFileNameWithoutExtension(pathToBuiltProject)}_Data", "Plugins");
        //}
        //else if (target == BuildTarget.StandaloneOSX)
        //{
        //    // macOS의 경우 더 복잡한 경로 처리가 필요할 수 있음
        //}

        //// 경로가 존재하지 않으면 생성
        //if (!Directory.Exists(targetPath))
        //{
        //    Directory.CreateDirectory(targetPath);
        //}

        //// ChromeDriver를 대상 경로로 복사
        //foreach (var file in Directory.GetFiles(sourcePath))
        //{
        //    string destFile = Path.Combine(targetPath, Path.GetFileName(file));
        //    File.Copy(file, destFile, true);
        //}
    }
}
