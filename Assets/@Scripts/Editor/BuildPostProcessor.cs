using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;

public class BuildPostProcessor
{
    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        //// ChromeDriver�� ���� ���
        //string sourcePath = Path.Combine(Application.dataPath, "WebDriver");
        //// ����� ���ø����̼��� ���
        //string targetPath = Path.GetDirectoryName(pathToBuiltProject);

        //// Windows ������ ��� ���� ���ϰ� ���� ��ġ�� ����
        //// macOS �Ǵ� �ٸ� �÷����� ��� �ణ�� ��� ������ �ʿ��� �� ����
        //if (target == BuildTarget.StandaloneWindows || target == BuildTarget.StandaloneWindows64)
        //{
        //    targetPath = Path.Combine(targetPath, $"{Path.GetFileNameWithoutExtension(pathToBuiltProject)}_Data", "Plugins");
        //}
        //else if (target == BuildTarget.StandaloneOSX)
        //{
        //    // macOS�� ��� �� ������ ��� ó���� �ʿ��� �� ����
        //}

        //// ��ΰ� �������� ������ ����
        //if (!Directory.Exists(targetPath))
        //{
        //    Directory.CreateDirectory(targetPath);
        //}

        //// ChromeDriver�� ��� ��η� ����
        //foreach (var file in Directory.GetFiles(sourcePath))
        //{
        //    string destFile = Path.Combine(targetPath, Path.GetFileName(file));
        //    File.Copy(file, destFile, true);
        //}
    }
}
